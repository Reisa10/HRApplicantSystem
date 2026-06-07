using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;
using HRApplicantSystem.Database;
using HRApplicantSystem.Classes;

namespace HRApplicantSystem.Forms.Applicant
{
    public partial class ApplicationStatusForm : Form
    {
        private DataTable dtApplications = new DataTable();
        private int currentApplicantId;

        public ApplicationStatusForm()
        {
            InitializeComponent();
            currentApplicantId = UserSession.UserID;
            ApplyModernStyles();
        }

        private void ApplicationStatusForm_Load(object sender, EventArgs e)
        {
            LoadApplications();
        }

        private void LoadApplications()
        {
            OleDbConnection conn = DBConnection.GetConnection();
            if (conn == null) return;

            try
            {
                // Removed "AND a.Status <> 'Draft'" to allow active Drafts to be tracked immediately [2]
                string query = "SELECT a.ApplicationID, j.JobTitle, a.Status, a.DateApplied " +
                               "FROM Applications a " +
                               "INNER JOIN JobVacancies j ON a.JobID = j.JobID " +
                               "WHERE a.ApplicantID = ? " +
                               "ORDER BY a.DateApplied DESC";

                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.Add("@ApplicantID", OleDbType.Integer).Value = currentApplicantId;

                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                    {
                        dtApplications.Clear();
                        adapter.Fill(dtApplications);
                        dgvStatusSummary.DataSource = dtApplications;

                        if (dgvStatusSummary.Columns["ApplicationID"] != null) dgvStatusSummary.Columns["ApplicationID"].Visible = false;
                        if (dgvStatusSummary.Columns["JobTitle"] != null) dgvStatusSummary.Columns["JobTitle"].HeaderText = "Job Title";
                        if (dgvStatusSummary.Columns["Status"] != null) dgvStatusSummary.Columns["Status"].HeaderText = "Status";
                        if (dgvStatusSummary.Columns["DateApplied"] != null) dgvStatusSummary.Columns["DateApplied"].HeaderText = "Applied On";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading applications: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvStatusSummary_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvStatusSummary.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvStatusSummary.SelectedRows[0];
                int applicationId = Convert.ToInt32(row.Cells["ApplicationID"].Value);
                string status = row.Cells["Status"].Value?.ToString() ?? "";

                lblSelectedJob.Text = row.Cells["JobTitle"].Value?.ToString() ?? "";
                lblCurrentState.Text = "Current Status: " + status;

                LoadTimeline(applicationId);
                LoadInterviewDetails(applicationId);
                LoadDecisionDetails(applicationId);
            }
            else
            {
                lblSelectedJob.Text = "Select an Application";
                lblCurrentState.Text = "Current Status: --";
                lstTrackingTimeline.Items.Clear();
                lblInterviewDate.Text = "Date: --";
                lblInterviewTime.Text = "Time: --";
                lblInterviewDetails.Text = "Interviewer/Location: --";
                lblRemarksText.Text = "No evaluation details processed yet.";
            }
        }

        /// <summary>
        /// Reads chronological tracking logs from ApplicationStatusHistory.
        /// </summary>
        private void LoadTimeline(int applicationId)
        {
            lstTrackingTimeline.Items.Clear();
            OleDbConnection conn = DBConnection.GetConnection();
            if (conn == null) return;

            try
            {
                string query = "SELECT [Status], [DateChanged] FROM [ApplicationStatusHistory] WHERE [ApplicationID] = ? ORDER BY [DateChanged] ASC";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.Add("@ApplicationID", OleDbType.Integer).Value = applicationId;
                    conn.Open();
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string status = reader["Status"]?.ToString() ?? "";
                            string dateStr = reader["DateChanged"] != DBNull.Value ? Convert.ToDateTime(reader["DateChanged"]).ToString("g") : "";
                            lstTrackingTimeline.Items.Add($"[{dateStr}] Status: {status}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading timeline: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Safely queries the interview bookings table dynamically to prevent column mismatch crashes.
        /// </summary>
        private void LoadInterviewDetails(int applicationId)
        {
            OleDbConnection conn = DBConnection.GetConnection();
            if (conn == null) return;

            try
            {
                string query = "SELECT * FROM [InterviewSchedules] WHERE [ApplicationID] = ?";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.Add("@ApplicationID", OleDbType.Integer).Value = applicationId;
                    conn.Open();
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string rawDate = reader["InterviewDate"] != DBNull.Value ? Convert.ToDateTime(reader["InterviewDate"]).ToShortDateString() : "--";
                            string rawTime = reader["InterviewTime"] != DBNull.Value ? reader["InterviewTime"].ToString() : "--";
                            string interviewer = reader["Interviewer"] != DBNull.Value ? reader["Interviewer"].ToString() : "N/A";

                            string location = "Online/Office Mode";
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string name = reader.GetName(i).ToLower();
                                if (name.Contains("location") || name.Contains("venue") || name.Contains("mode"))
                                {
                                    location = reader.GetValue(i).ToString();
                                    break;
                                }
                            }

                            lblInterviewDate.Text = "Date: " + rawDate;
                            lblInterviewTime.Text = "Time: " + rawTime;
                            lblInterviewDetails.Text = $"Interviewer: {interviewer} | Venue: {location}";
                        }
                        else
                        {
                            lblInterviewDate.Text = "Date: Not scheduled yet";
                            lblInterviewTime.Text = "Time: Not scheduled yet";
                            lblInterviewDetails.Text = "Interviewer/Location: --";
                        }
                    }
                }
            }
            catch
            {
                lblInterviewDate.Text = "Date: --";
                lblInterviewTime.Text = "Time: --";
                lblInterviewDetails.Text = "Interviewer/Location: --";
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Reads decision evaluation comments from the database.
        /// </summary>
        private void LoadDecisionDetails(int applicationId)
        {
            OleDbConnection conn = DBConnection.GetConnection();
            if (conn == null) return;

            try
            {
                string query = "SELECT Decision, Remarks, DecisionDate FROM HiringDecisions WHERE ApplicationID =?";

                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.Add("@ApplicationID", OleDbType.Integer).Value = applicationId;
                    conn.Open();

                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string decision = reader ["Decision"]?.ToString() ?? "";
                            string remarks = reader ["Remarks"]?.ToString() ?? "";
                            DateTime? decisionDate = reader ["DecisionDate"] != DBNull.Value
                           ? Convert.ToDateTime(reader ["DecisionDate"])
                           : (DateTime?)null;

                            // Color-code the decision
                            switch (decision.ToLower())
                            {
                                case "accepted":
                                    lblDecisionResult.ForeColor = Color.Green;
                                    break;
                                case "rejected":
                                    lblDecisionResult.ForeColor = Color.Red;
                                    break;
                                case "on hold":
                                    lblDecisionResult.ForeColor = Color.Orange;
                                    break;
                                default:
                                    lblDecisionResult.ForeColor = Color.Black;
                                    break;
                            }

                            lblDecisionResult.Text = "Decision: " + decision;
                            lblDecisionDate.Text = decisionDate.HasValue
                           ? "Decision Date: " + decisionDate.Value.ToString("MMMM dd, yyyy")
                           : "Decision Date: --";
                            lblRemarksText.Text = !string.IsNullOrEmpty(remarks)
                           ? remarks
                           : "No remarks provided.";
                        }
                        else
                        {
                            // No hiring decision record yet
                            lblDecisionResult.Text = "Decision: Pending";
                            lblDecisionResult.ForeColor = Color.Gray;
                            lblDecisionDate.Text = "Decision Date: --";
                            lblRemarksText.Text = "Your application is active. Final decision has not been declared yet.";
                        }
                    }
                }
            }
            catch
            {
                lblDecisionResult.Text = "Decision: Pending";
                lblDecisionResult.ForeColor = Color.Gray;
                lblDecisionDate.Text = "Decision Date: --";
                lblRemarksText.Text = "HR review details are currently pending.";
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ApplyModernStyles()
        {
            this.BackColor = Color.FromArgb(245, 247, 250);

            // Grid Styling
            dgvStatusSummary.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStatusSummary.MultiSelect = false;
            dgvStatusSummary.RowHeadersVisible = false;
            dgvStatusSummary.AllowUserToAddRows = false;
            dgvStatusSummary.ReadOnly = true;
            dgvStatusSummary.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvStatusSummary.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 250);

            // List Style
            lstTrackingTimeline.BorderStyle = BorderStyle.FixedSingle;
            lstTrackingTimeline.Font = new Font("Segoe UI", 9F);
            lstTrackingTimeline.ForeColor = Color.FromArgb(44, 62, 80);

            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.FlatAppearance.BorderColor = Color.FromArgb(127, 140, 141);
            btnBack.BackColor = Color.White;
            btnBack.ForeColor = Color.FromArgb(44, 62, 80);
            btnBack.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        }
    }
}