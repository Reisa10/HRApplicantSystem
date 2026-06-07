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

                // Load individual details dynamically [2]
                LoadTimeline(applicationId);
                LoadScreeningDetails(applicationId);
                LoadInterviewDetails(applicationId);
                LoadEvaluationDetails(applicationId);
                LoadDecisionDetails(applicationId, status);
            }
            else
            {
                ResetDetails();
            }
        }

        private void ResetDetails()
        {
            lblSelectedJob.Text = "Select an Application";
            lblCurrentState.Text = "Current Status: --";
            lstTrackingTimeline.Items.Clear();

            lblScreeningResult.Text = "Screening Result: Pending";
            lblScreeningDate.Text = "Date Screened: --";
            lblScreeningRemarks.Text = "No screening remarks available yet.";

            lblInterviewDate.Text = "Date: Not scheduled yet";
            lblInterviewInterviewer.Text = "Interviewer: --";
            lblInterviewVenue.Text = "Venue/Location: --";
            lblInterviewMode.Text = "Mode: --";
            lblInterviewStatus.Text = "Status: --";

            lblEvalScore.Text = "Score: --";
            lblEvalResult.Text = "Result: --";
            lblEvalRecommendation.Text = "Recommendation: --";
            lblEvalRemarks.Text = "No interview evaluation remarks available yet.";

            lblRemarksText.Text = "Please select an application from the list to view decision details.";
        }

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

        private void LoadScreeningDetails(int applicationId)
        {
            OleDbConnection conn = DBConnection.GetConnection();
            if (conn == null) return;

            try
            {
                string query = "SELECT [ScreeningResult], [Remarks], [DateScreened] FROM [ScreeningResults] WHERE [ApplicationID] = ?";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.Add("@ApplicationID", OleDbType.Integer).Value = applicationId;
                    conn.Open();
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string result = reader["ScreeningResult"]?.ToString() ?? "Pending";
                            string dateStr = reader["DateScreened"] != DBNull.Value ? Convert.ToDateTime(reader["DateScreened"]).ToShortDateString() : "--";
                            string remarks = reader["Remarks"]?.ToString() ?? "";

                            lblScreeningResult.Text = "Screening Result: " + result;
                            lblScreeningDate.Text = "Date Screened: " + dateStr;
                            lblScreeningRemarks.Text = string.IsNullOrWhiteSpace(remarks) ? "No screening remarks recorded." : remarks;
                        }
                        else
                        {
                            lblScreeningResult.Text = "Screening Result: Pending";
                            lblScreeningDate.Text = "Date Screened: --";
                            lblScreeningRemarks.Text = "Your application has not been screened yet.";
                        }
                    }
                }
            }
            catch
            {
                lblScreeningResult.Text = "Screening Result: Pending";
                lblScreeningDate.Text = "Date Screened: --";
                lblScreeningRemarks.Text = "Pending HR review.";
            }
            finally
            {
                conn.Close();
            }
        }

        private void LoadInterviewDetails(int applicationId)
        {
            OleDbConnection conn = DBConnection.GetConnection();
            if (conn == null) return;

            try
            {
                string query = @"
SELECT TOP 1
    [InterviewDate],
    [Interviewer],
    [Location],
    [Mode],
    [Status]
FROM [InterviewSchedules]
WHERE [ApplicationID] = ?
ORDER BY [ScheduleID] DESC";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.Add("@ApplicationID", OleDbType.Integer).Value = applicationId;
                    conn.Open();
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string rawDate = reader["InterviewDate"] != DBNull.Value ? Convert.ToDateTime(reader["InterviewDate"]).ToString("g") : "--";
                            string interviewer = reader["Interviewer"] != DBNull.Value ? reader["Interviewer"].ToString() : "N/A";
                            string venue = reader["Location"] != DBNull.Value ? reader["Location"].ToString() : "--";
                            string mode = reader["Mode"] != DBNull.Value ? reader["Mode"].ToString() : "--";
                            string status = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : "--";

                            lblInterviewDate.Text = "Date & Time: " + rawDate;
                            lblInterviewInterviewer.Text = "Interviewer: " + interviewer;
                            lblInterviewVenue.Text = "Venue/Location: " + venue;
                            lblInterviewMode.Text = "Mode: " + mode;
                            lblInterviewStatus.Text = "Schedule Status: " + status;
                        }
                        else
                        {
                            lblInterviewDate.Text = "Date: Not scheduled yet";
                            lblInterviewInterviewer.Text = "Interviewer: --";
                            lblInterviewVenue.Text = "Venue/Location: --";
                            lblInterviewMode.Text = "Mode: --";
                            lblInterviewStatus.Text = "Status: --";
                        }
                    }
                }
            }
            catch
            {
                lblInterviewDate.Text = "Date: --";
                lblInterviewInterviewer.Text = "Interviewer: --";
                lblInterviewVenue.Text = "Venue/Location: --";
                lblInterviewMode.Text = "Mode: --";
                lblInterviewStatus.Text = "Status: --";
            }
            finally
            {
                conn.Close();
            }
        }

        private void LoadEvaluationDetails(int applicationId)
        {
            OleDbConnection conn = DBConnection.GetConnection();
            if (conn == null) return;

            try
            {
                string query = "SELECT [Score], [Remarks], [Result], [Recommendation] FROM [InterviewEvaluations] WHERE [ApplicationID] = ?";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.Add("@ApplicationID", OleDbType.Integer).Value = applicationId;
                    conn.Open();
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string score = reader["Score"] != DBNull.Value ? reader["Score"].ToString() : "--";
                            string result = reader["Result"]?.ToString() ?? "--";
                            string recommendation = reader["Recommendation"]?.ToString() ?? "--";
                            string remarks = reader["Remarks"]?.ToString() ?? "";

                            lblEvalScore.Text = "Score: " + score;
                            lblEvalResult.Text = "Result: " + result;
                            lblEvalRecommendation.Text = "Recommendation: " + recommendation;
                            lblEvalRemarks.Text = string.IsNullOrWhiteSpace(remarks) ? "No evaluation remarks recorded." : remarks;
                        }
                        else
                        {
                            lblEvalScore.Text = "Score: --";
                            lblEvalResult.Text = "Result: --";
                            lblEvalRecommendation.Text = "Recommendation: --";
                            lblEvalRemarks.Text = "Interview evaluation has not been processed yet.";
                        }
                    }
                }
            }
            catch
            {
                lblEvalScore.Text = "Score: --";
                lblEvalResult.Text = "Result: --";
                lblEvalRecommendation.Text = "Recommendation: --";
                lblEvalRemarks.Text = "Evaluation pending.";
            }
            finally
            {
                conn.Close();
            }
        }

        private void LoadDecisionDetails(int applicationId, string currentStatus)
        {
            OleDbConnection conn = DBConnection.GetConnection();
            if (conn == null) return;

            try
            {
                // Joins with the Users table to retrieve the Decision Maker's Full Name [1]
                string query = "SELECT hd.Decision, hd.Remarks, hd.DecisionDate, u.[Full Name] AS ManagerName " +
                               "FROM HiringDecisions hd " +
                               "LEFT JOIN Users u ON hd.DecisionBy = u.UserID " +
                               "WHERE hd.ApplicationID = ?";

                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.Add("@ApplicationID", OleDbType.Integer).Value = applicationId;
                    conn.Open();

                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string decision = reader["Decision"]?.ToString() ?? "";
                            string remarks = reader["Remarks"]?.ToString() ?? "";
                            string dateStr = reader["DecisionDate"] != DBNull.Value
                                ? Convert.ToDateTime(reader["DecisionDate"]).ToString("f")
                                : "--";
                            string manager = reader["ManagerName"] != DBNull.Value
                                ? reader["ManagerName"].ToString()
                                : "HR Department";

                            lblRemarksText.Text = $"Status: {decision.ToUpper()}\r\n" +
                                                 $"Processed On: {dateStr}\r\n" +
                                                 $"Reviewed By: {manager}\r\n\r\n" +
                                                 $"HR Message:\r\n\"{remarks}\"";
                        }
                        else
                        {
                            // Context-aware fallback messages to replace the generic message when no decision entry exists yet
                            switch (currentStatus)
                            {
                                case "Draft":
                                    lblRemarksText.Text = "This application is currently a Draft. Please finalize your details and submit to start the HR recruitment process.";
                                    break;
                                case "Submitted":
                                    lblRemarksText.Text = "Your application has been submitted successfully. HR review has not started yet.";
                                    break;
                                case "Under Review":
                                    lblRemarksText.Text = "Your application is currently Under Review by the HR department. Final decision pending.";
                                    break;
                                case "Shortlisted":
                                    lblRemarksText.Text = "Congratulations! You have been Shortlisted for further evaluation. Final decision pending.";
                                    break;
                                case "For Interview":
                                case "Interview Scheduled":
                                    lblRemarksText.Text = "You are currently in the Interview stage. The final decision will be declared after your interviews are complete.";
                                    break;
                                case "For Assessment":
                                    lblRemarksText.Text = "You are scheduled for an assessment. The final decision will be declared after your results are evaluated.";
                                    break;
                                case "For Final Review":
                                case "For Final Decision":
                                    lblRemarksText.Text = "Your application is undergoing final review by HR Management. Final decision pending.";
                                    break;
                                case "On Hold":
                                    lblRemarksText.Text = "Your application has been put On Hold. An HR representative will reach out to you if further requirements are needed.";
                                    break;
                                case "Rejected":
                                    lblRemarksText.Text = "Thank you for your interest in our company. Your application has been reviewed, and we regret to inform you that we are moving forward with other candidates.";
                                    break;
                                case "Withdrawn":
                                    lblRemarksText.Text = "This application has been withdrawn.";
                                    break;
                                default:
                                    lblRemarksText.Text = "Your application is active and currently under review. Final HR decision pending.";
                                    break;
                            }
                        }
                    }
                }
            }
            catch
            {
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