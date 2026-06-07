using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;

namespace HRApplicantSystem.Database
{
    public class DatabaseHelper
    {
        public static bool IsDatabaseAvailable()
        {
            OleDbConnection con = DBConnection.GetConnection();
            if (con == null) return false;

            try
            {
                con.Open();
                con.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Computes the list of missing mandatory requirement types for an applicant.
        /// </summary>
        public static List<string> GetMissingRequirements(int applicantId)
        {
            List<string> missing = new List<string>();

            using (OleDbConnection con = DBConnection.GetConnection())
            {
                if (con == null) return missing;
                try
                {
                    con.Open();

                    // 1. Get all mandatory requirement types (IsRequired = True)
                    List<int> requiredIds = new List<int>();
                    Dictionary<int, string> idToName = new Dictionary<int, string>();

                    // Access SQL uses True/False or Yes/No for boolean
                    string reqQuery = "SELECT RequirementTypeID, RequirementName FROM RequirementTypes WHERE IsRequired = True";
                    using (OleDbCommand cmd = new OleDbCommand(reqQuery, con))
                    {
                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = Convert.ToInt32(reader["RequirementTypeID"]);
                                string name = reader["RequirementName"].ToString();
                                requiredIds.Add(id);
                                idToName[id] = name;
                            }
                        }
                    }

                    // 2. Get uploaded documents that are not flagged as missing
                    List<int> uploadedIds = new List<int>();
                    string uploadQuery = "SELECT RequirementTypeID FROM ApplicantDocuments WHERE ApplicantID = ? AND Status <> 'Missing'";
                    using (OleDbCommand cmd = new OleDbCommand(uploadQuery, con))
                    {
                        cmd.Parameters.AddWithValue("?", applicantId);
                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                uploadedIds.Add(Convert.ToInt32(reader["RequirementTypeID"]));
                            }
                        }
                    }

                    // 3. Subtract uploaded IDs from mandatory IDs
                    foreach (int reqId in requiredIds)
                    {
                        if (!uploadedIds.Contains(reqId))
                        {
                            missing.Add(idToName[reqId]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Error computing requirements: " + ex.Message, "Database Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            }

            return missing;
        }

        /// <summary>
        /// Checks if the applicant's editing capabilities are locked based on the required status flow.
        /// Unlocked for: Draft, Submitted, Rejected, and Withdrawn. [2]
        /// </summary>
        public static bool IsApplicationLocked(int applicantId)
        {
            using (OleDbConnection con = DBConnection.GetConnection())
            {
                if (con == null) return false;
                try
                {
                    con.Open();
                    string query = "SELECT Status FROM Applications WHERE ApplicantID = ?";
                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("?", applicantId);
                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string status = reader["Status"].ToString();

                                // Lock ONLY during active processing and evaluation stages
                                if (status == "Under Review" ||
                                    status == "Shortlisted" ||
                                    status == "Interview Scheduled" ||
                                    status == "For Interview" ||
                                    status == "For Assessment" ||
                                    status == "For Final Review" ||
                                    status == "For Final Decision" ||
                                    status == "Accepted")
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
                catch
                {
                    // Fail-safe default to unlocked
                    return false;
                }
            }
            return false;
        }
    }
}