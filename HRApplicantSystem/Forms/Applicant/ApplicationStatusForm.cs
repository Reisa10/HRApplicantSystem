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

        // Professional SaaS Colors
        private readonly Color ColorSlate900 = Color.FromArgb(15, 23, 42);
        private readonly Color ColorSlate700 = Color.FromArgb(51, 65, 85);
        private readonly Color ColorSlate500 = Color.FromArgb(100, 116, 139);
        private readonly Color ColorSlate100 = Color.FromArgb(241, 245, 249);
        private readonly Color ColorBorder = Color.FromArgb(226, 232, 240);

        private readonly Color ColorGreenBg = Color.FromArgb(240, 253, 244);
        private readonly Color ColorGreenFg = Color.FromArgb(21, 128, 61);
        private readonly Color ColorRedBg = Color.FromArgb(254, 242, 242);
        private readonly Color ColorRedFg = Color.FromArgb(185, 28, 28);
        private readonly Color ColorAmberBg = Color.FromArgb(255, 251, 235);
        private readonly Color ColorAmberFg = Color.FromArgb(180, 83, 9);
        private readonly Color ColorBlueBg = Color.FromArgb(239, 246, 255);
        private readonly Color ColorBlueFg = Color.FromArgb(29, 78, 216);

        public ApplicationStatusForm()
        {
            InitializeComponent();
            currentApplicantId = UserSession.UserID;
            InitializeDynamicTimeline();
            ApplyModernStyles();

            // Force creation of lazy tab page controls to ensure correct DPI layout scaling on hidden tabs
            foreach (TabPage tab in tcStatusDetails.TabPages)
            {
                tab.CreateControl();
            }
        }

        private void ApplicationStatusForm_Load(object sender, EventArgs e)
        {
            LoadApplications();
            UpdateRemarksLayout();
        }

        private void InitializeDynamicTimeline()
        {
            try
            {
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
                lstTrackingTimeline.Visible = true;
            }
        }

        private void LoadApplications()
        {
            bool success = false;

            try
            {
                using (OleDbConnection conn = DBConnection.GetConnection())
                {
                    if (conn == null) return;

                    string query = @"SELECT a.ApplicationID, p.PositionName, et.TypeName AS EmploymentType, d.DepartmentName, a.Status, a.DateApplied 
                                     FROM ((((Applications a 
                                     INNER JOIN JobVacancies j ON a.JobID = j.JobID) 
                                     INNER JOIN Positions p ON j.PositionID = p.PositionID) 
                                     LEFT JOIN Departments d ON j.DepartmentID = d.DepartmentID) 
                                     LEFT JOIN EmploymentTypes et ON j.EmploymentTypeID = et.EmploymentTypeID) 
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
                success = false;
            }

            if (!success)
            {
                try
                {
                    using (OleDbConnection connFallback = DBConnection.GetConnection())
                    {
                        if (connFallback == null) return;

                        string fallbackQuery = @"SELECT a.ApplicationID, p.PositionName, et.TypeName AS EmploymentType, a.Status, a.DateApplied 
                                                 FROM ((Applications a 
                                                 INNER JOIN JobVacancies j ON a.JobID = j.JobID) 
                                                 INNER JOIN Positions p ON j.PositionID = p.PositionID) 
                                                 LEFT JOIN EmploymentTypes et ON j.EmploymentTypeID = et.EmploymentTypeID
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
            if (dgvStatusSummary.Columns["EmploymentType"] != null) dgvStatusSummary.Columns["EmploymentType"].HeaderText = "Employment Type";
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

                string jobTitle = dgvStatusSummary.Columns.Contains("PositionName") ? row.Cells["PositionName"].Value?.ToString() : "";
                string dept = dgvStatusSummary.Columns.Contains("DepartmentName") ? row.Cells["DepartmentName"].Value?.ToString() : "";
                string empType = dgvStatusSummary.Columns.Contains("EmploymentType") ? row.Cells["EmploymentType"].Value?.ToString() : "";

                if (!string.IsNullOrEmpty(empType))
                {
                    lblSelectedJob.Text = $"{jobTitle.ToUpper()} ({empType})";
                }
                else
                {
                    lblSelectedJob.Text = jobTitle.ToUpper();
                }

                if (!string.IsNullOrEmpty(dept))
                {
                    lblCurrentState.Text = $"Status: {status}  ({dept})";
                }
                else
                {
                    lblCurrentState.Text = "Status: " + status;
                }

                lblCurrentState.ForeColor = GetStatusThemeColor(status);

                // Load individual details dynamically
                LoadTimeline(appId);
                LoadScreeningDetails(appId);
                LoadInterviewDetails(appId);
                LoadEvaluationDetails(appId);
                LoadDecisionDetails(appId, status);
                CheckRequirements(appId);

                // Re-evaluate container boundaries
                UpdateRemarksLayout();
            }
            else
            {
                ResetDetails();
            }
        }

        private void CheckRequirements(int appId)
        {
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

            // Clear dynamic cards in each tab
            PurgeProgrammaticCards(tpScreening);
            PurgeProgrammaticCards(tpInterview);
            PurgeProgrammaticCards(tpEvaluation);
            PurgeProgrammaticCards(tpHiring);
        }

        private void PurgeProgrammaticCards(TabPage tab)
        {
            List<Control> toRemove = new List<Control>();
            foreach (Control c in tab.Controls)
            {
                if (c.Name.StartsWith("prog_"))
                {
                    toRemove.Add(c);
                }
                else
                {
                    c.Visible = true; // Restore designer visibility if hidden
                }
            }
            foreach (Control c in toRemove)
            {
                tab.Controls.Remove(c);
                c.Dispose();
            }
        }

        #region Card & Badge Layout Generators (UI Design Improvements)

        private Panel CreateBadge(string text, Color bg, Color fg, Point loc)
        {
            Label lbl = new Label
            {
                Text = text.ToUpper(),
                ForeColor = fg,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                Location = new Point(12, 5),
                AutoSize = true
            };

            // Fixed: Explicit bounds sizing instead of nested AutoSize to prevent WinForms layout bugs
            Panel pnl = new Panel
            {
                Name = "prog_badge",
                BackColor = bg,
                Location = loc,
                Size = new Size(lbl.PreferredWidth + 24, lbl.PreferredHeight + 10)
            };

            pnl.Controls.Add(lbl);
            return pnl;
        }

        private Panel CreateRemarksCard(string title, string text, Point loc, int width, int height, Color accentColor)
        {
            Panel card = new Panel
            {
                Name = "prog_card",
                Location = loc,
                Size = new Size(width, height),
                BackColor = Color.White,
                Padding = new Padding(15, 10, 15, 10)
            };

            card.Paint += (s, e) =>
            {
                // Accent left-hand border stripe (5px)
                using (SolidBrush b = new SolidBrush(accentColor))
                {
                    e.Graphics.FillRectangle(b, 0, 0, 5, card.Height);
                }
                // Light card boundary lines
                using (Pen p = new Pen(ColorBorder, 1))
                {
                    e.Graphics.DrawRectangle(p, 5, 0, card.Width - 6, card.Height - 1);
                }
            };

            Label lblTitle = new Label
            {
                Text = title.ToUpper(),
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = ColorSlate500,
                Location = new Point(15, 12),
                AutoSize = true
            };

            TextBox txtText = new TextBox
            {
                Text = string.IsNullOrEmpty(text) ? "No remarks submitted." : $"\"{text}\"",
                Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                ForeColor = ColorSlate700,
                Location = new Point(15, 30),
                Size = new Size(card.Width - 30, card.Height - 45),
                Multiline = true,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                BackColor = Color.White,
                ScrollBars = ScrollBars.Vertical
            };

            card.Controls.Add(lblTitle);
            card.Controls.Add(txtText);
            return card;
        }

        #endregion

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
            PurgeProgrammaticCards(tpScreening);
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
                        // Hide designer defaults to paint our card
                        lblScreeningResult.Visible = false;
                        lblScreeningDate.Visible = false;
                        lblScreeningRemarksLabel.Visible = false;
                        lblScreeningRemarks.Visible = false;

                        if (reader.Read())
                        {
                            string result = reader["ScreeningResult"]?.ToString() ?? "Pending";
                            string dateStr = reader["DateScreened"] != DBNull.Value ? Convert.ToDateTime(reader["DateScreened"]).ToString("f") : "--";
                            string remarks = reader["Remarks"]?.ToString() ?? "";

                            bool isQualified = result.Equals("Qualified", StringComparison.OrdinalIgnoreCase);
                            Color badgeBg = isQualified ? ColorGreenBg : ColorRedBg;
                            Color badgeFg = isQualified ? ColorGreenFg : ColorRedFg;
                            string badgeText = isQualified ? "✓ Qualified" : "⚠️ Not Qualified";

                            Panel badge = CreateBadge(badgeText, badgeBg, badgeFg, new Point(20, 20));
                            tpScreening.Controls.Add(badge);

                            Label lblDate = new Label
                            {
                                Name = "prog_lblDate",
                                Text = $"Screened on: {dateStr}",
                                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                                ForeColor = ColorSlate500,
                                Location = new Point(22, 55),
                                AutoSize = true
                            };
                            tpScreening.Controls.Add(lblDate);

                            Panel remarksCard = CreateRemarksCard("Screening feedback", remarks, new Point(20, 85), tpScreening.Width - 40, tpScreening.Height - 110, badgeFg);
                            tpScreening.Controls.Add(remarksCard);
                        }
                        else
                        {
                            Panel badge = CreateBadge("Pending Screening", ColorAmberBg, ColorAmberFg, new Point(20, 20));
                            tpScreening.Controls.Add(badge);

                            Label lblPending = new Label
                            {
                                Name = "prog_lblPending",
                                Text = "Your application submission is queued and awaiting initial screening reviews.",
                                Font = new Font("Segoe UI", 9.5F, FontStyle.Italic),
                                ForeColor = ColorSlate500,
                                Location = new Point(22, 60),
                                Size = new Size(tpScreening.Width - 44, 100)
                            };
                            tpScreening.Controls.Add(lblPending);
                        }
                    }
                }
            }
            catch
            {
                lblScreeningResult.Visible = true;
                lblScreeningDate.Visible = true;
                lblScreeningRemarksLabel.Visible = true;
                lblScreeningRemarks.Visible = true;
            }
            finally
            {
                conn.Close();
            }
        }

        private void LoadInterviewDetails(int applicationId)
        {
            PurgeProgrammaticCards(tpInterview);
            OleDbConnection conn = DBConnection.GetConnection();
            if (conn == null) return;

            try
            {
                // Updated with TOP 1 descending order to retrieve the most recent reschedule details
                string query = @"SELECT TOP 1 [InterviewDate], [Interviewer], [Location], [Mode], [Status] 
                                 FROM [InterviewSchedules] 
                                 WHERE [ApplicationID] = ? 
                                 ORDER BY [InterviewScheduleID] DESC";

                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.Add("@ApplicationID", OleDbType.Integer).Value = applicationId;
                    conn.Open();
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        // Hide designer defaults
                        lblInterviewDate.Visible = false;
                        lblInterviewInterviewer.Visible = false;
                        lblInterviewVenue.Visible = false;
                        lblInterviewMode.Visible = false;
                        lblInterviewStatus.Visible = false;

                        if (reader.Read())
                        {
                            string rawDate = reader["InterviewDate"] != DBNull.Value ? Convert.ToDateTime(reader["InterviewDate"]).ToString("f") : "--";
                            string interviewer = reader["Interviewer"] != DBNull.Value ? reader["Interviewer"].ToString() : "N/A";
                            string venue = reader["Location"] != DBNull.Value ? reader["Location"].ToString() : "--";
                            string status = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : "--";

                            // Generate Schedule Status Badge dynamically
                            Color badgeBg = ColorBlueBg;
                            Color badgeFg = ColorBlueFg;
                            if (status.Equals("Cancelled", StringComparison.OrdinalIgnoreCase)) { badgeBg = ColorRedBg; badgeFg = ColorRedFg; }
                            else if (status.Equals("Completed", StringComparison.OrdinalIgnoreCase)) { badgeBg = ColorGreenBg; badgeFg = ColorGreenFg; }

                            Panel badge = CreateBadge($"Schedule State: {status}", badgeBg, badgeFg, new Point(20, 20));
                            tpInterview.Controls.Add(badge);

                            // Structured Schedule Container Panel
                            Panel card = new Panel
                            {
                                Name = "prog_card",
                                Location = new Point(20, 60),
                                Size = new Size(tpInterview.Width - 40, tpInterview.Height - 85),
                                BackColor = Color.White,
                                Padding = new Padding(20, 15, 20, 15)
                            };

                            card.Paint += (s, ev) =>
                            {
                                using (Pen p = new Pen(ColorBorder, 1))
                                {
                                    ev.Graphics.DrawRectangle(p, 0, 0, card.Width - 1, card.Height - 1);
                                }
                            };

                            int yOffset = 15;
                            AddMetaLabelToCard(card, "Date & Time:", rawDate, ref yOffset);
                            AddMetaLabelToCard(card, "Assigned Interviewer:", interviewer, ref yOffset);
                            AddMetaLabelToCard(card, "Venue/Location:", venue, ref yOffset);
                            // Fixed: Removed the misleading "Assigned Mode" row entirely as requested

                            tpInterview.Controls.Add(card);
                        }
                        else
                        {
                            Panel badge = CreateBadge("Not Scheduled Yet", ColorAmberBg, ColorAmberFg, new Point(20, 20));
                            tpInterview.Controls.Add(badge);

                            Label lblPending = new Label
                            {
                                Name = "prog_lblPending",
                                Text = "Your application is currently being evaluated for interview schedule allocations.",
                                Font = new Font("Segoe UI", 9.5F, FontStyle.Italic),
                                ForeColor = ColorSlate500,
                                Location = new Point(22, 60),
                                Size = new Size(tpInterview.Width - 44, 100)
                            };
                            tpInterview.Controls.Add(lblPending);
                        }
                    }
                }
            }
            catch
            {
                lblInterviewDate.Visible = true;
                lblInterviewInterviewer.Visible = true;
                lblInterviewVenue.Visible = true;
                lblInterviewMode.Visible = true;
                lblInterviewStatus.Visible = true;
            }
            finally
            {
                conn.Close();
            }
        }

        private void AddMetaLabelToCard(Panel card, string header, string val, ref int y)
        {
            Label lblHeader = new Label
            {
                Text = header,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = ColorSlate500,
                Location = new Point(15, y),
                AutoSize = true
            };
            Label lblVal = new Label
            {
                Text = val,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                ForeColor = ColorSlate900,
                Location = new Point(150, y - 1),
                AutoSize = true
            };
            card.Controls.Add(lblHeader);
            card.Controls.Add(lblVal);
            y += 32;
        }

        private void LoadEvaluationDetails(int applicationId)
        {
            PurgeProgrammaticCards(tpEvaluation);
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
                        // Hide designer defaults
                        lblEvalScore.Visible = false;
                        lblEvalResult.Visible = false;
                        lblEvalRecommendation.Visible = false;
                        lblEvalRemarksLabel.Visible = false;
                        lblEvalRemarks.Visible = false;

                        if (reader.Read())
                        {
                            string score = reader["Score"] != DBNull.Value ? reader["Score"].ToString() : "--";
                            string result = reader["Result"]?.ToString() ?? "--";
                            string recommendation = reader["Recommendation"]?.ToString() ?? "--";
                            string remarks = reader["Remarks"]?.ToString() ?? "";

                            bool isPass = result.Equals("Pass", StringComparison.OrdinalIgnoreCase);
                            Color badgeBg = isPass ? ColorGreenBg : ColorRedBg;
                            Color badgeFg = isPass ? ColorGreenFg : ColorRedFg;
                            string badgeText = isPass ? "✓ Recommendation: Pass" : "⚠️ Recommendation: Fail";

                            Panel badge = CreateBadge(badgeText, badgeBg, badgeFg, new Point(20, 20));
                            tpEvaluation.Controls.Add(badge);

                            // Draw a score badge/circle on the top-right dynamically
                            Panel scoreCard = new Panel
                            {
                                Name = "prog_score",
                                Size = new Size(110, 32),
                                Location = new Point(tpEvaluation.Width - 130, 22),
                                BackColor = isPass ? ColorGreenBg : ColorRedBg
                            };
                            scoreCard.Paint += (s, ev) =>
                            {
                                using (Pen p = new Pen(badgeFg, 1F))
                                {
                                    ev.Graphics.DrawRectangle(p, 0, 0, scoreCard.Width - 1, scoreCard.Height - 1);
                                }
                            };
                            Label lblScoreVal = new Label
                            {
                                Text = $"{score} / 100",
                                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                                ForeColor = badgeFg,
                                Dock = DockStyle.Fill,
                                TextAlign = ContentAlignment.MiddleCenter
                            };
                            scoreCard.Controls.Add(lblScoreVal);
                            tpEvaluation.Controls.Add(scoreCard);

                            Label lblRec = new Label
                            {
                                Name = "prog_lblRec",
                                Text = $"Action Route: {recommendation}",
                                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                                ForeColor = ColorSlate900,
                                Location = new Point(22, 58),
                                Size = new Size(tpEvaluation.Width - 170, 35),
                                AutoSize = false // Enabled wrapping to prevent overlaps
                            };
                            tpEvaluation.Controls.Add(lblRec);

                            // Shifted the remarks card downward to Y=100 to prevent text overlaps
                            int remarksCardTop = 100;
                            Panel remarksCard = CreateRemarksCard("Evaluator feedback remarks", remarks, new Point(20, remarksCardTop), tpEvaluation.Width - 40, tpEvaluation.Height - remarksCardTop - 15, badgeFg);
                            tpEvaluation.Controls.Add(remarksCard);
                        }
                        else
                        {
                            Panel badge = CreateBadge("Evaluation Pending", ColorAmberBg, ColorAmberFg, new Point(20, 20));
                            tpEvaluation.Controls.Add(badge);

                            Label lblPending = new Label
                            {
                                Name = "prog_lblPending",
                                Text = "Your interview performance has not been evaluated by the HR department yet.",
                                Font = new Font("Segoe UI", 9.5F, FontStyle.Italic),
                                ForeColor = ColorSlate500,
                                Location = new Point(22, 60),
                                Size = new Size(tpEvaluation.Width - 44, 100)
                            };
                            tpEvaluation.Controls.Add(lblPending);
                        }
                    }
                }
            }
            catch
            {
                lblEvalScore.Visible = true;
                lblEvalResult.Visible = true;
                lblEvalRecommendation.Visible = true;
                lblEvalRemarksLabel.Visible = true;
                lblEvalRemarks.Visible = true;
            }
            finally
            {
                conn.Close();
            }
        }

        private void LoadDecisionDetails(int applicationId, string currentStatus)
        {
            PurgeProgrammaticCards(tpHiring);
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
                        // Hide designer defaults
                        lblRemarksLabel.Visible = false;
                        lblRemarksText.Visible = false;

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

                            bool isAccepted = decision.Equals("Accepted", StringComparison.OrdinalIgnoreCase);
                            bool isRejected = decision.Equals("Rejected", StringComparison.OrdinalIgnoreCase);

                            Color bannerBg = isAccepted ? ColorGreenBg : (isRejected ? ColorRedBg : ColorAmberBg);
                            Color bannerFg = isAccepted ? ColorGreenFg : (isRejected ? ColorRedFg : ColorAmberFg);
                            string bannerText = isAccepted ? "✓ Approved & Hired" : (isRejected ? "⚠️ Process Closed: Rejected" : "⏱️ On Hold");

                            Panel banner = CreateBadge(bannerText, bannerBg, bannerFg, new Point(20, 20));
                            tpHiring.Controls.Add(banner);

                            Label lblDate = new Label
                            {
                                Name = "prog_lblDate",
                                Text = $"Processed on: {dateStr}  |  Reviewed by: {manager}",
                                Font = new Font("Segoe UI", 8.5F, FontStyle.Regular),
                                ForeColor = ColorSlate500,
                                Location = new Point(22, 55),
                                AutoSize = true
                            };
                            tpHiring.Controls.Add(lblDate);

                            Panel remarksCard = CreateRemarksCard("Executive board message", remarks, new Point(20, 85), tpHiring.Width - 40, tpHiring.Height - 110, bannerFg);
                            tpHiring.Controls.Add(remarksCard);
                        }
                        else
                        {
                            Panel badge = CreateBadge("Under Review", ColorAmberBg, ColorAmberFg, new Point(20, 20));
                            tpHiring.Controls.Add(badge);

                            // Provide descriptive guidance text depending on active states
                            string prompt = "";
                            switch (currentStatus)
                            {
                                case "Draft":
                                    prompt = "This application is currently in Draft state. Please review your details and click Submit to start the HR recruitment process.";
                                    break;
                                case "Submitted":
                                    prompt = "Your application has been received successfully! It is queued and awaiting initial screening reviews.";
                                    break;
                                case "Under Review":
                                    prompt = "Your application is currently under active screening by the HR department. Final decision pending.";
                                    break;
                                case "Shortlisted":
                                    prompt = "Congratulations! You have been shortlisted for further evaluation. HR will schedule your interviews shortly.";
                                    break;
                                case "For Interview":
                                case "Interview Scheduled":
                                    prompt = "You are currently undergoing the Interview stage. Executive hiring decisions are declared once all evaluations are finalized.";
                                    break;
                                case "For Assessment":
                                    prompt = "You are scheduled for a skills assessment. Executive decisions are finalized once results are submitted.";
                                    break;
                                case "For Final Review":
                                case "For Final Decision":
                                    prompt = "Your application is undergoing final executive board review. Results will be posted here shortly.";
                                    break;
                                case "On Hold":
                                    prompt = "Your application has been placed on hold. HR will contact you if further requirement clarifications are needed.";
                                    break;
                                case "Withdrawn":
                                    prompt = "This application was withdrawn.";
                                    break;
                                default:
                                    prompt = "Your application is active and under review. The final hiring decision has not been declared yet.";
                                    break;
                            }

                            Label lblPending = new Label
                            {
                                Name = "prog_lblPending",
                                Text = prompt,
                                Font = new Font("Segoe UI", 9.5F, FontStyle.Italic),
                                ForeColor = ColorSlate500,
                                Location = new Point(22, 60),
                                Size = new Size(tpHiring.Width - 44, 100)
                            };
                            tpHiring.Controls.Add(lblPending);
                        }
                    }
                }
            }
            catch
            {
                lblRemarksLabel.Visible = true;
                lblRemarksText.Visible = true;
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

        private void tcStatusDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateRemarksLayout();
        }

        private void ApplicationStatusForm_Resize(object sender, EventArgs e)
        {
            UpdateRemarksLayout();
        }

        private void splitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            UpdateRemarksLayout();
        }

        private void UpdateRemarksLayout()
        {
            if (tcStatusDetails != null)
            {
                tcStatusDetails.PerformLayout();
                foreach (TabPage tab in tcStatusDetails.TabPages)
                {
                    tab.PerformLayout();
                }
            }

            // Dynamically scale our programmatic cards to stretch elegantly on form resizes/splitter moves
            ScaleProgrammaticCardToFit(tpScreening);
            ScaleProgrammaticCardToFit(tpInterview);
            ScaleProgrammaticCardToFit(tpEvaluation);
            ScaleProgrammaticCardToFit(tpHiring);
        }

        private void ScaleProgrammaticCardToFit(TabPage tab)
        {
            foreach (Control c in tab.Controls)
            {
                if (c.Name == "prog_card")
                {
                    c.Width = tab.Width - 40;

                    // Fixed: Apply customized layout scaling offsets for Evaluation tab to prevent overlaps
                    if (tab == tpEvaluation)
                    {
                        c.Top = 100;
                        c.Height = tab.Height - 125;
                    }
                    else
                    {
                        c.Top = 85;
                        c.Height = tab.Height - 110;
                    }

                    // Also stretch the internal scrollable remarks textbox inside the card
                    foreach (Control child in c.Controls)
                    {
                        if (child is TextBox txt)
                        {
                            child.Width = c.Width - 30;
                            child.Height = c.Height - 45;
                        }
                    }
                }
                else if (c.Name == "prog_lblRec")
                {
                    c.Width = tab.Width - 170;
                }
                else if (c.Name == "prog_lblPending")
                {
                    c.Width = tab.Width - 44;
                }
                else if (c.Name == "prog_score")
                {
                    c.Left = tab.Width - 130;
                }
            }
        }

        private void ApplyModernStyles()
        {
            this.BackColor = Color.FromArgb(245, 247, 250);

            // Sleek Header styling
            pnlHeader.BackColor = Color.White;
            lblHeader.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblHeader.ForeColor = Color.FromArgb(27, 38, 59);

            // Grouping panels styling
            grpTracking.BackColor = Color.White;
            grpTracking.ForeColor = Color.FromArgb(127, 140, 141);
            lblSelectedJob.ForeColor = Color.FromArgb(27, 38, 59);

            // Tab layouts backgrounds
            tpTimeline.BackColor = Color.White;
            tpScreening.BackColor = Color.White;
            tpInterview.BackColor = Color.White;
            tpEvaluation.BackColor = Color.White;
            tpHiring.BackColor = Color.White;

            lblTimelineLabel.ForeColor = Color.FromArgb(127, 140, 141);
            lblScreeningRemarksLabel.ForeColor = Color.FromArgb(127, 140, 141);
            lblEvalRemarksLabel.ForeColor = Color.FromArgb(127, 140, 141);
            lblRemarksLabel.ForeColor = Color.FromArgb(127, 140, 141);

            // Seamless TextBox styling for remarks to prevent italic clipping and allow scrolling
            StyleRemarksTextBox(lblScreeningRemarks);
            StyleRemarksTextBox(lblEvalRemarks);
            StyleRemarksTextBox(lblRemarksText);

            // Grid Styling
            dgvStatusSummary.BackgroundColor = Color.White;
            dgvStatusSummary.BorderStyle = BorderStyle.None;
            dgvStatusSummary.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvStatusSummary.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvStatusSummary.GridColor = Color.FromArgb(235, 237, 240);
            dgvStatusSummary.RowTemplate.Height = 42;
            dgvStatusSummary.ColumnHeadersHeight = 40;
            dgvStatusSummary.EnableHeadersVisualStyles = false;

            // Header style
            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(27, 38, 59),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.75f, FontStyle.Bold),
                SelectionBackColor = Color.FromArgb(27, 38, 59),
                Alignment = DataGridViewContentAlignment.MiddleLeft
            };
            dgvStatusSummary.ColumnHeadersDefaultCellStyle = headerStyle;

            // Row style
            DataGridViewCellStyle rowStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = Color.FromArgb(44, 62, 80),
                Font = new Font("Segoe UI", 9f, FontStyle.Regular),
                SelectionBackColor = Color.FromArgb(232, 244, 253),
                SelectionForeColor = Color.FromArgb(27, 38, 59)
            };
            dgvStatusSummary.DefaultCellStyle = rowStyle;

            // Alternating Row style
            DataGridViewCellStyle altRowStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(249, 251, 252)
            };
            dgvStatusSummary.AlternatingRowsDefaultCellStyle = altRowStyle;

            // Header elements vertical positioning calculations
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.BackColor = Color.FromArgb(231, 76, 60); // Soft Red Standard back button style
            btnBack.ForeColor = Color.White;
            btnBack.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            btnBack.Cursor = Cursors.Hand;

            btnBack.Top = (pnlHeader.Height - btnBack.Height) / 2;
            lblHeader.Top = (pnlHeader.Height - lblHeader.Height) / 2;
        }

        private void StyleRemarksTextBox(TextBox txt)
        {
            txt.Multiline = true;
            txt.ReadOnly = true;
            txt.WordWrap = true; // Explicitly ensure wrapping is activated
            txt.BorderStyle = BorderStyle.None;
            txt.BackColor = Color.White;
            txt.ForeColor = Color.FromArgb(44, 62, 80);
            txt.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            txt.ScrollBars = ScrollBars.Vertical;
        }
    }
}