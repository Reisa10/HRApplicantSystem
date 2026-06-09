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
        /// Computes the list of missing mandatory requirement types specifically defined 
        /// by the job vacancy the applicant is currently applying to.
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

                    // 1. Get the comma-separated RequiredDocuments string from the job vacancy linked to the applicant's active application
                    string activeJobQuery = @"SELECT j.RequiredDocuments 
                                              FROM Applications a 
                                              INNER JOIN JobVacancies j ON a.JobID = j.JobID 
                                              WHERE a.ApplicantID = ? AND a.Status NOT IN ('Withdrawn', 'Rejected', 'Accepted')";

                    string requiredDocsCSV = "";
                    using (OleDbCommand jobCmd = new OleDbCommand(activeJobQuery, con))
                    {
                        jobCmd.Parameters.AddWithValue("?", applicantId);
                        object result = jobCmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            requiredDocsCSV = result.ToString();
                        }
                    }

                    // 2. Parse the dynamic vacancy requirements
                    List<int> requiredIds = new List<int>();
                    if (!string.IsNullOrEmpty(requiredDocsCSV))
                    {
                        string[] tokens = requiredDocsCSV.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string t in tokens)
                        {
                            if (int.TryParse(t.Trim(), out int id))
                            {
                                requiredIds.Add(id);
                            }
                        }
                    }

                    Dictionary<int, string> idToName = new Dictionary<int, string>();

                    if (requiredIds.Count > 0)
                    {
                        // Load requirement names only for the IDs specified in the active vacancy
                        string reqQuery = "SELECT RequirementTypeID, RequirementName FROM RequirementTypes";
                        using (OleDbCommand cmd = new OleDbCommand(reqQuery, con))
                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = Convert.ToInt32(reader["RequirementTypeID"]);
                                string name = reader["RequirementName"].ToString();
                                if (requiredIds.Contains(id))
                                {
                                    idToName[id] = name;
                                }
                            }
                        }
                    }
                    else
                    {
                        // Fallback: If no application is active, default to global mandatory requirements (IsRequired = True)
                        string reqQuery = "SELECT RequirementTypeID, RequirementName FROM RequirementTypes WHERE IsRequired = True";
                        using (OleDbCommand cmd = new OleDbCommand(reqQuery, con))
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

                    // 3. Get already uploaded documents that are not flagged as missing
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

                    // 4. Subtract uploaded IDs from vacancy requirements
                    foreach (int reqId in requiredIds)
                    {
                        if (!uploadedIds.Contains(reqId) && idToName.ContainsKey(reqId))
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
        /// Unlocked ONLY for Draft and Submitted (before HR review begins).
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

                                // Lock any status that has progressed past the initial Draft/Submitted phase
                                if (status != "Draft" && status != "Submitted")
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
                catch
                {
                    // Fail-safe default to locked for security integrity
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Computes the list of missing mandatory requirement types for a specific Job ID.
        /// </summary>
        public static List<string> GetMissingRequirementsForJob(int applicantId, int jobId)
        {
            List<string> missing = new List<string>();

            using (OleDbConnection con = DBConnection.GetConnection())
            {
                if (con == null) return missing;
                try
                {
                    con.Open();

                    // 1. Get the comma-separated RequiredDocuments string from the specific job vacancy
                    string jobQuery = "SELECT RequiredDocuments FROM JobVacancies WHERE JobID = ?";
                    string requiredDocsCSV = "";
                    using (OleDbCommand jobCmd = new OleDbCommand(jobQuery, con))
                    {
                        jobCmd.Parameters.AddWithValue("?", jobId);
                        object result = jobCmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            requiredDocsCSV = result.ToString();
                        }
                    }

                    // 2. Parse vacancy requirements
                    List<int> requiredIds = new List<int>();
                    if (!string.IsNullOrEmpty(requiredDocsCSV))
                    {
                        string[] tokens = requiredDocsCSV.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string t in tokens)
                        {
                            if (int.TryParse(t.Trim(), out int id))
                            {
                                requiredIds.Add(id);
                            }
                        }
                    }

                    Dictionary<int, string> idToName = new Dictionary<int, string>();
                    if (requiredIds.Count > 0)
                    {
                        string reqQuery = "SELECT RequirementTypeID, RequirementName FROM RequirementTypes";
                        using (OleDbCommand cmd = new OleDbCommand(reqQuery, con))
                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = Convert.ToInt32(reader["RequirementTypeID"]);
                                string name = reader["RequirementName"].ToString();
                                if (requiredIds.Contains(id))
                                {
                                    idToName[id] = name;
                                }
                            }
                        }
                    }

                    // 3. Get uploaded documents
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

                    // 4. Subtract uploaded IDs
                    foreach (int reqId in requiredIds)
                    {
                        if (!uploadedIds.Contains(reqId) && idToName.ContainsKey(reqId))
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
        /// Computes the list of missing mandatory requirement types for a specific Application ID.
        /// </summary>
        public static List<string> GetMissingRequirementsForApplication(int applicationId)
        {
            List<string> missing = new List<string>();

            using (OleDbConnection con = DBConnection.GetConnection())
            {
                if (con == null) return missing;
                try
                {
                    con.Open();

                    // 1. Retrieve the ApplicantID and JobID for this specific application
                    int applicantId = 0;
                    int jobId = 0;
                    string appQuery = "SELECT ApplicantID, JobID FROM Applications WHERE ApplicationID = ?";
                    using (OleDbCommand appCmd = new OleDbCommand(appQuery, con))
                    {
                        appCmd.Parameters.AddWithValue("?", applicationId);
                        using (OleDbDataReader reader = appCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                applicantId = Convert.ToInt32(reader["ApplicantID"]);
                                jobId = Convert.ToInt32(reader["JobID"]);
                            }
                            else
                            {
                                return missing; // Application record not found
                            }
                        }
                    }

                    // 2. Get the comma-separated RequiredDocuments string from the specific job vacancy
                    string jobQuery = "SELECT RequiredDocuments FROM JobVacancies WHERE JobID = ?";
                    string requiredDocsCSV = "";
                    using (OleDbCommand jobCmd = new OleDbCommand(jobQuery, con))
                    {
                        jobCmd.Parameters.AddWithValue("?", jobId);
                        object result = jobCmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            requiredDocsCSV = result.ToString();
                        }
                    }

                    // 3. Parse vacancy requirements
                    List<int> requiredIds = new List<int>();
                    if (!string.IsNullOrEmpty(requiredDocsCSV))
                    {
                        string[] tokens = requiredDocsCSV.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string t in tokens)
                        {
                            if (int.TryParse(t.Trim(), out int id))
                            {
                                requiredIds.Add(id);
                            }
                        }
                    }

                    Dictionary<int, string> idToName = new Dictionary<int, string>();
                    if (requiredIds.Count > 0)
                    {
                        string reqQuery = "SELECT RequirementTypeID, RequirementName FROM RequirementTypes";
                        using (OleDbCommand cmd = new OleDbCommand(reqQuery, con))
                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = Convert.ToInt32(reader["RequirementTypeID"]);
                                string name = reader["RequirementName"].ToString();
                                if (requiredIds.Contains(id))
                                {
                                    idToName[id] = name;
                                }
                            }
                        }
                    }

                    // 4. Get uploaded documents
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

                    // 5. Subtract uploaded IDs from requirements
                    foreach (int reqId in requiredIds)
                    {
                        if (!uploadedIds.Contains(reqId) && idToName.ContainsKey(reqId))
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
    }
}