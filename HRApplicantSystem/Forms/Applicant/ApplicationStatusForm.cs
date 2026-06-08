using System;
using System.Collections.Generic;
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
        private FlowLayoutPanel flpTimeline; // Programmatic modern timeline container

        public ApplicationStatusForm()
        {
            InitializeComponent();
            currentApplicantId = UserSession.UserID;
            InitializeDynamicTimeline();
            ApplyModernStyles();
        }

        private void ApplicationStatusForm_Load(object sender, EventArgs e)
        {
            LoadApplications();
        }

        /// <summary>
        /// Safe dynamic UI initialization to avoid modifying the .Designer.cs file manually.
        /// </summary>
        private void InitializeDynamicTimeline()
        {
            try
            {
                // Programmatically replace lstTrackingTimeline with a FlowLayoutPanel
                flpTimeline = new FlowLayoutPanel();
                flpTimeline.Location = lstTrackingTimeline.Location;
                flpTimeline.Size = lstTrackingTimeline.Size;
                flpTimeline.Anchor = lstTrackingTimeline.Anchor;
                flpTimeline.AutoScroll = true;
                flpTimeline.FlowDirection = FlowDirection.TopDown;
                flpTimeline.WrapContents = false;
                flpTimeline.BackColor = Color.FromArgb(245, 247, 250);
                flpTimeline.Padding = new Padding(10);

                lstTrackingTimeline.Visible = false; // Hide old standard listbox
                tpTimeline.Controls.Add(flpTimeline); // Add our dynamic card layout
            }
            catch
            {
                // Fallback protection: if dynamic creation fails, show standard listbox
                lstTrackingTimeline.Visible = true;
            }
        }

        private void LoadApplications()
        {
            bool success = false;

            // 1. Try advanced join query on a fresh connection mapping Applications -> JobVacancies -> Positions -> Departments
            try
            {
                using (OleDbConnection conn = DBConnection.GetConnection())
                {
                    if (conn == null) return;

                    string query = @"SELECT a.ApplicationID, p.PositionName, d.DepartmentName, a.Status, a.DateApplied 
                                     FROM (((Applications a 
                                     INNER JOIN JobVacancies j ON a.JobID = j.JobID) 
                                     INNER JOIN Positions p ON j.PositionID = p.PositionID) 
                                     LEFT JOIN Departments d ON j.DepartmentID = d.DepartmentID) 
                                     WHERE a.ApplicantID = ? 
                                     ORDER BY a.DateApplied DESC";

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.Add("@ApplicantID", OleDbType.Integer).Value = currentApplicantId;

                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                        {
                            dtApplications.Clear();
                            adapter.Fill(dtApplications);
                            dgvStatusSummary.DataSource = dtApplications;

                            ConfigureHeaders(true);
                            success = true;
                        }
                    }
                }
            }
            catch
            {
                success = false; // Silent failover to prevent popups if schema mismatches occur
            }

            // 2. Fallback query on a fresh connection mapping Applications -> JobVacancies -> Positions
            if (!success)
            {
                try
                {
                    using (OleDbConnection connFallback = DBConnection.GetConnection())
                    {
                        if (connFallback == null) return;

                        string fallbackQuery = @"SELECT a.ApplicationID, p.PositionName, a.Status, a.DateApplied 
                                                 FROM (Applications a 
                                                 INNER JOIN JobVacancies j ON a.JobID = j.JobID) 
                                                 INNER JOIN Positions p ON j.PositionID = p.PositionID 
                                                 WHERE a.ApplicantID = ? 
                                                 ORDER BY a.DateApplied DESC";

                        using (OleDbCommand cmd = new OleDbCommand(fallbackQuery, connFallback))
                        {
                            cmd.Parameters.Add("@ApplicantID", OleDbType.Integer).Value = currentApplicantId;

                            using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                            {
                                dtApplications.Clear();
                                adapter.Fill(dtApplications);
                                dgvStatusSummary.DataSource = dtApplications;

                                ConfigureHeaders(false);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading applications: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ConfigureHeaders(bool hasJoins)
        {
            if (dgvStatusSummary.Columns["ApplicationID"] != null) dgvStatusSummary.Columns["ApplicationID"].Visible = false;

            if (dgvStatusSummary.Columns["PositionName"] != null) dgvStatusSummary.Columns["PositionName"].HeaderText = "Job Position";
            if (dgvStatusSummary.Columns["Status"] != null) dgvStatusSummary.Columns["Status"].HeaderText = "Status";
            if (dgvStatusSummary.Columns["DateApplied"] != null) dgvStatusSummary.Columns["DateApplied"].HeaderText = "Applied Date";

            if (hasJoins)
            {
                if (dgvStatusSummary.Columns["DepartmentName"] != null) dgvStatusSummary.Columns["DepartmentName"].HeaderText = "Department";
            }
        }

        private void dgvStatusSummary_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvStatusSummary.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvStatusSummary.SelectedRows[0];
                int appId = Convert.ToInt32(row.Cells["ApplicationID"].Value);
                string status = row.Cells["Status"].Value?.ToString() ?? "";

                // Safely read job title and department parameters
                string jobTitle = dgvStatusSummary.Columns.Contains("PositionName") ? row.Cells["PositionName"].Value?.ToString() : "";
                string dept = dgvStatusSummary.Columns.Contains("DepartmentName") ? row.Cells["DepartmentName"].Value?.ToString() : "";

                lblSelectedJob.Text = jobTitle.ToUpper();

                if (!string.IsNullOrEmpty(dept))
                {
                    lblCurrentState.Text = $"Status: {status}  ({dept})";
                }
                else
                {
                    lblCurrentState.Text = "Status: " + status;
                }

                // Apply contextual colors to the status label
                lblCurrentState.ForeColor = GetStatusThemeColor(status);

                // Load individual details dynamically
                LoadTimeline(appId);
                LoadScreeningDetails(appId);
                LoadInterviewDetails(appId);
                LoadEvaluationDetails(appId);
                LoadDecisionDetails(appId, status);
                CheckRequirements(appId);
            }
            else
            {
                ResetDetails();
            }
        }

        private void CheckRequirements(int appId)
        {
            // Connects with DatabaseHelper computation
            var missing = DatabaseHelper.GetMissingRequirementsForApplication(appId);
            if (missing.Count > 0)
            {
                lblCurrentState.Text += $"  |  ⚠️ {missing.Count} Missing Documents";
            }
        }

        private void ResetDetails()
        {
            lblSelectedJob.Text = "Select an Application";
            lblCurrentState.Text = "Current Status: --";
            lblCurrentState.ForeColor = Color.FromArgb(230, 126, 34);

            if (flpTimeline != null) flpTimeline.Controls.Clear();
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
            if (flpTimeline != null)
            {
                flpTimeline.SuspendLayout();
                flpTimeline.Controls.Clear();
            }
            lstTrackingTimeline.Items.Clear();

            OleDbConnection conn = DBConnection.GetConnection();
            if (conn == null) return;

            try
            {
                string query = "SELECT [Status], [DateChanged] FROM [ApplicationStatusHistory] WHERE [ApplicationID] = ? ORDER BY [DateChanged] DESC";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.Add("@ApplicationID", OleDbType.Integer).Value = applicationId;
                    conn.Open();
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string status = reader["Status"]?.ToString() ?? "";
                            DateTime dateVal = reader["DateChanged"] != DBNull.Value ? Convert.ToDateTime(reader["DateChanged"]) : DateTime.Now;

                            // Safe dynamic UI card rendering
                            if (flpTimeline != null)
                            {
                                AddTimelineCard(status, dateVal);
                            }
                            else
                            {
                                lstTrackingTimeline.Items.Add($"[{dateVal:g}] Status: {status}");
                            }
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
                if (flpTimeline != null) flpTimeline.ResumeLayout();
                conn.Close();
            }
        }

        private void AddTimelineCard(string status, DateTime date)
        {
            Panel card = new Panel
            {
                Size = new Size(flpTimeline.Width - 25, 60),
                BackColor = Color.White,
                Margin = new Padding(0, 0, 0, 8),
                Padding = new Padding(5)
            };

            Label lblIndicator = new Label
            {
                Text = "●",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = GetStatusThemeColor(status),
                Location = new Point(8, 8),
                AutoSize = true
            };

            Label lblStatusText = new Label
            {
                Text = status.ToUpper(),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(24, 11),
                AutoSize = true
            };

            Label lblTimeText = new Label
            {
                Text = date.ToString("MMM dd, yyyy - hh:mm tt"),
                Font = new Font("Segoe UI", 8, FontStyle.Regular),
                ForeColor = Color.DimGray,
                Location = new Point(24, 32),
                AutoSize = true
            };

            card.Controls.Add(lblIndicator);
            card.Controls.Add(lblStatusText);
            card.Controls.Add(lblTimeText);
            flpTimeline.Controls.Add(card);
        }

        private Color GetStatusThemeColor(string status)
        {
            switch (status)
            {
                case "Accepted": return Color.FromArgb(39, 174, 96);       // Emerald Green
                case "Rejected": return Color.FromArgb(192, 57, 43);       // Alizarin Red
                case "Under Review": return Color.FromArgb(230, 126, 34);   // Orange
                case "Shortlisted": return Color.FromArgb(22, 160, 133);    // Teal
                case "For Interview": return Color.FromArgb(142, 68, 173);  // Purple
                default: return Color.FromArgb(127, 140, 141);              // Slate Gray
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
                string query = "SELECT [InterviewDate], [Interviewer], [Location], [Mode], [Status] FROM [InterviewSchedules] WHERE [ApplicationID] = ?";
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
            dgvStatusSummary.GridColor = Color.FromArgb(230, 230, 230);

            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.BackColor = Color.FromArgb(52, 152, 219);
            btnBack.ForeColor = Color.White;
            btnBack.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
        }
    }
}