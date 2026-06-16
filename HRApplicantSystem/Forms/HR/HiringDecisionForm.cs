using HRApplicantSystem.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using HRApplicantSystem.Database;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HRApplicantSystem.Forms.HR
{
    public partial class HiringDecisionForm : Form
    {
        private int selectedApplicationID = 0;
        private DataTable dtApplications = new DataTable();

        // Professional SaaS Colors
        private readonly Color ColorSlate900 = Color.FromArgb(15, 23, 42);
        private readonly Color ColorSlate700 = Color.FromArgb(51, 65, 85);
        private readonly Color ColorSlate500 = Color.FromArgb(100, 116, 139);
        private readonly Color ColorBorder = Color.FromArgb(226, 232, 240);

        private readonly Color ColorGreenBg = Color.FromArgb(240, 253, 244);
        private readonly Color ColorGreenFg = Color.FromArgb(21, 128, 61);
        private readonly Color ColorRedBg = Color.FromArgb(254, 242, 242);
        private readonly Color ColorRedFg = Color.FromArgb(185, 28, 28);
        private readonly Color ColorBlueBg = Color.FromArgb(239, 246, 255);
        private readonly Color ColorBlueFg = Color.FromArgb(29, 78, 216);

        public HiringDecisionForm()
        {
            InitializeComponent();

            // Role-based Access Validation
            if (UserSession.Role != "HR Manager" && UserSession.Role != "Admin")
            {
                MessageBox.Show("Access Denied. Only an HR Manager or Admin can make final hiring decisions.",
                    "Unauthorized Access", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Load += (s, e) => this.Close();
            }
        }

        private void HiringDecisionForm_Load(object sender, EventArgs e)
        {
            ApplyThemeColors();

            // Centering Fix: Snap form coordinates precisely over the main dashboard
            CenterFormOnDashboard();

            LoadApplications();
            rdoAccepted.Checked = true;
            UpdateDecisionCardUI();
        }

        /// <summary>
        /// Locates the open HR Dashboard application window and centers this workspace directly over it.
        /// Resolves high-DPI scaling offsets at runtime.
        /// </summary>
        private void CenterFormOnDashboard()
        {
            Form dashboard = null;

            // Search for the dashboard or main menu form
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm != this && (openForm.Name.Contains("Dashboard") || openForm.Name.Contains("Main") || openForm.Name.Contains("Portal")))
                {
                    dashboard = openForm;
                    break;
                }
            }

            // Fallback to the first open form if no exact name matches
            if (dashboard == null)
            {
                foreach (Form openForm in Application.OpenForms)
                {
                    if (openForm != this)
                    {
                        dashboard = openForm;
                        break;
                    }
                }
            }

            if (dashboard != null)
            {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new Point(
                    dashboard.Location.X + (dashboard.Width - this.Width) / 2,
                    dashboard.Location.Y + (dashboard.Height - this.Height) / 2
                );
            }
            else
            {
                // Fallback to primary screen center if no caller dashboard is detected
                this.StartPosition = FormStartPosition.CenterScreen;
            }
        }

        /// <summary>
        /// Stylizes standard WinForm controls programmatically to ensure a corporate aesthetic.
        /// </summary>
        private void ApplyThemeColors()
        {
            // Lock form to standard expansive workspace boundaries
            this.Size = new Size(976, 612);

            this.BackColor = Color.FromArgb(241, 245, 249); // Slate Neutral Background

            // Card panel stylings
            pnlLeftCard.BackColor = Color.White;
            pnlRightCard.BackColor = Color.White;

            // Paint flat subtle borders on cards
            pnlLeftCard.Paint += (s, ev) => PaintCardBorder(pnlLeftCard, ev);
            pnlRightCard.Paint += (s, ev) => PaintCardBorder(pnlRightCard, ev);

            // Left panel (List Area)
            if (dgvApplicants != null)
            {
                dgvApplicants.BackgroundColor = Color.White;
                dgvApplicants.BorderStyle = BorderStyle.None;
                dgvApplicants.EnableHeadersVisualStyles = false;
                dgvApplicants.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 41, 59);
                dgvApplicants.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvApplicants.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                dgvApplicants.DefaultCellStyle.SelectionBackColor = Color.FromArgb(239, 246, 255);
                dgvApplicants.DefaultCellStyle.SelectionForeColor = Color.FromArgb(29, 78, 216);
            }

            // Save Action Button
            if (btnSave != null)
            {
                btnSave.FlatStyle = FlatStyle.Flat;
                btnSave.FlatAppearance.BorderSize = 0;
                btnSave.BackColor = Color.FromArgb(40, 167, 69); // Standard Emerald Green default
                btnSave.ForeColor = Color.White;
                btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            }

            // Back Action Button
            if (btnBack != null)
            {
                btnBack.FlatStyle = FlatStyle.Flat;
                btnBack.BackColor = Color.FromArgb(100, 116, 139);
                btnBack.ForeColor = Color.White;
                btnBack.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
                btnBack.FlatAppearance.BorderSize = 0;
            }

            // Hide old designer lines and static labels to paint our custom cards
            if (pnlSeparator1 != null) pnlSeparator1.Visible = false;
            if (pnlSeparator2 != null) pnlSeparator2.Visible = false;
            if (lblEvalHeader != null) lblEvalHeader.Visible = false;
            if (lblInterviewScore != null) lblInterviewScore.Visible = false;
            if (lblMissingRequirements != null) lblMissingRequirements.Visible = false;

            // Align form input labels & textboxes cleanly
            lblApplicant.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblApplicant.ForeColor = ColorSlate900;
            lblApplicant.Location = new Point(20, 15);

            lblJob.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            lblJob.ForeColor = ColorSlate500;
            lblJob.Location = new Point(20, 41);

            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.ForeColor = ColorSlate900;
            label1.Location = new Point(20, 245);

            rdoAccepted.Location = new Point(165, 243);
            rdoAccepted.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            rdoAccepted.ForeColor = ColorSlate700;

            rdoRejected.Location = new Point(265, 243);
            rdoRejected.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            rdoRejected.ForeColor = ColorSlate700;

            rdoOnHold.Location = new Point(365, 243);
            rdoOnHold.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            rdoOnHold.ForeColor = ColorSlate700;

            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.ForeColor = ColorSlate900;
            label2.Location = new Point(20, 282);

            txtRemarks.BorderStyle = BorderStyle.FixedSingle;
            txtRemarks.BackColor = Color.White;
            txtRemarks.ForeColor = ColorSlate900;
            txtRemarks.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            txtRemarks.Location = new Point(20, 305);
            txtRemarks.Width = pnlRightCard.Width - 40;
            txtRemarks.Height = pnlRightCard.Height - 305 - 60; // Auto-scales height elegantly
        }

        private void PaintCardBorder(Panel panel, PaintEventArgs ev)
        {
            using (Pen p = new Pen(Color.FromArgb(226, 232, 240), 1))
            {
                ev.Graphics.DrawRectangle(p, 0, 0, panel.Width - 1, panel.Height - 1);
            }
        }

        /// <summary>
        /// Loads applications in 'For Final Decision' status, joining Job, Department, and Employment Type data (6-table nested join).
        /// </summary>
        private void LoadApplications()
        {
            try
            {
                using (OleDbConnection conn = DBConnection.GetConnection())
                {
                    if (conn == null) return;

                    // Fixed: Explicit OLEDB join nesting syntax adjusted for 6 tables including EmploymentTypes et
                    string query = @"
                        SELECT
                            Applications.ApplicationID,
                            Applicants.FirstName & ' ' & Applicants.LastName AS FullName,
                            Positions.PositionName AS JobTitle,
                            et.TypeName AS EmploymentType,
                            Departments.DepartmentName
                        FROM
                            (
                                (
                                    (
                                        (
                                            Applications
                                            INNER JOIN Applicants ON Applications.ApplicantID = Applicants.ApplicantID
                                        )
                                        INNER JOIN JobVacancies ON Applications.JobID = JobVacancies.JobID
                                    )
                                    INNER JOIN Positions ON JobVacancies.PositionID = Positions.PositionID
                                )
                                LEFT JOIN Departments ON JobVacancies.DepartmentID = Departments.DepartmentID
                            )
                            LEFT JOIN EmploymentTypes et ON JobVacancies.EmploymentTypeID = et.EmploymentTypeID
                        WHERE Applications.Status = 'For Final Decision'";

                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn))
                    {
                        dtApplications.Clear();
                        adapter.Fill(dtApplications);
                        dgvApplicants.DataSource = dtApplications;

                        if (dgvApplicants.Columns["ApplicationID"] != null)
                            dgvApplicants.Columns["ApplicationID"].Visible = false;

                        // Configure visible, user-friendly headers
                        if (dgvApplicants.Columns["FullName"] != null)
                            dgvApplicants.Columns["FullName"].HeaderText = "Applicant Name";
                        if (dgvApplicants.Columns["JobTitle"] != null)
                            dgvApplicants.Columns["JobTitle"].HeaderText = "Position Applied";
                        if (dgvApplicants.Columns["EmploymentType"] != null)
                            dgvApplicants.Columns["EmploymentType"].HeaderText = "Employment Type";
                        if (dgvApplicants.Columns["DepartmentName"] != null)
                            dgvApplicants.Columns["DepartmentName"].HeaderText = "Department";

                        dgvApplicants.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading applicants:\n" + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvApplicants_SelectionChanged(object sender, EventArgs e)
        {
            PurgeProgrammaticCards(pnlRightCard);

            if (dgvApplicants.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvApplicants.SelectedRows[0];
                selectedApplicationID = Convert.ToInt32(row.Cells["ApplicationID"].Value);

                string applicantName = row.Cells["FullName"].Value?.ToString();
                string jobTitle = row.Cells["JobTitle"].Value?.ToString();
                string department = row.Cells["DepartmentName"].Value?.ToString() ?? "Unassigned Department";
                string empType = row.Cells["EmploymentType"].Value?.ToString() ?? "";

                lblApplicant.Text = "Applicant: " + applicantName;

                if (!string.IsNullOrEmpty(empType))
                {
                    lblJob.Text = $"Job: {jobTitle} | {department} ({empType})";
                }
                else
                {
                    lblJob.Text = $"Job: {jobTitle} | {department}";
                }

                // 1. Fetch details from the Document Management & Requirements system
                DisplayMissingRequirements(selectedApplicationID);

                // 2. Fetch details from the Interview Evaluation module
                DisplayInterviewEvaluations(selectedApplicationID);
            }
            else
            {
                ResetDetailsCard();
            }
        }

        private void PurgeProgrammaticCards(Panel parent)
        {
            List<Control> toRemove = new List<Control>();
            foreach (Control c in parent.Controls)
            {
                if (c.Name.StartsWith("prog_"))
                {
                    toRemove.Add(c);
                }
            }
            foreach (Control c in toRemove)
            {
                parent.Controls.Remove(c);
                c.Dispose();
            }
        }

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

        /// <summary>
        /// Retrieves missing requirements from the DatabaseHelper and displays them to the HR Manager.
        /// </summary>
        private void DisplayMissingRequirements(int applicationId)
        {
            List<string> missing = DatabaseHelper.GetMissingRequirementsForApplication(applicationId);

            Color bannerBg = ColorGreenBg;
            Color bannerFg = ColorGreenFg;
            string bannerText = "✓ Compliance: All mandatory requirements submitted successfully.";

            if (missing.Count > 0)
            {
                bannerBg = ColorRedBg;
                bannerFg = ColorRedFg;
                bannerText = "⚠️ Compliance Gap: Missing required documents.";
            }

            Panel banner = CreateBadge(bannerText, bannerBg, bannerFg, new Point(20, 195));
            pnlRightCard.Controls.Add(banner);
        }

        /// <summary>
        /// Safely fetches the evaluation results from InterviewEvaluations to help guide the final decision.
        /// </summary>
        private void DisplayInterviewEvaluations(int applicationId)
        {
            try
            {
                using (OleDbConnection conn = DBConnection.GetConnection())
                {
                    if (conn == null) return;
                    conn.Open();

                    string query = @"
                        SELECT TOP 1 Score, Remarks, Recommendation 
                        FROM InterviewEvaluations 
                        WHERE ApplicationID = ? 
                        ORDER BY EvaluationID DESC";

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("?", applicationId);
                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            // Structured SaaS evaluation card panel
                            Panel evalCard = new Panel
                            {
                                Name = "prog_card",
                                Location = new Point(20, 75),
                                Size = new Size(pnlRightCard.Width - 40, 110),
                                BackColor = Color.White,
                                Padding = new Padding(15, 10, 15, 10)
                            };

                            evalCard.Paint += (s, e) =>
                            {
                                using (Pen p = new Pen(ColorBorder, 1))
                                {
                                    e.Graphics.DrawRectangle(p, 0, 0, evalCard.Width - 1, evalCard.Height - 1);
                                }
                                using (SolidBrush b = new SolidBrush(ColorBlueFg))
                                {
                                    e.Graphics.FillRectangle(b, 0, 0, 5, evalCard.Height);
                                }
                            };

                            Label lblHeader = new Label
                            {
                                Text = "INTERVIEW EVALUATION SUMMARY",
                                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                                ForeColor = ColorSlate500,
                                Location = new Point(15, 12),
                                AutoSize = true
                            };
                            evalCard.Controls.Add(lblHeader);

                            if (reader.Read())
                            {
                                string score = reader["Score"].ToString();
                                string recommendation = reader["Recommendation"]?.ToString() ?? "N/A";
                                string remarks = reader["Remarks"]?.ToString() ?? "No details written.";

                                // Round numeric score badge on the right side
                                Panel scoreCard = new Panel
                                {
                                    Size = new Size(95, 45),
                                    Location = new Point(evalCard.Width - 110, 15),
                                    BackColor = ColorBlueBg
                                };
                                scoreCard.Paint += (s, ev) =>
                                {
                                    using (Pen p = new Pen(ColorBlueFg, 1))
                                    {
                                        ev.Graphics.DrawRectangle(p, 0, 0, scoreCard.Width - 1, scoreCard.Height - 1);
                                    }
                                };
                                Label lblScoreVal = new Label
                                {
                                    Text = $"{score} / 100",
                                    Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                                    ForeColor = ColorBlueFg,
                                    Dock = DockStyle.Fill,
                                    TextAlign = ContentAlignment.MiddleCenter
                                };
                                scoreCard.Controls.Add(lblScoreVal);
                                evalCard.Controls.Add(scoreCard);

                                // Details list on the left side
                                Label lblRec = new Label
                                {
                                    Text = $"Recommended:  {recommendation}",
                                    Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                                    ForeColor = ColorSlate700,
                                    Location = new Point(15, 34),
                                    AutoSize = true
                                };
                                evalCard.Controls.Add(lblRec);

                                Label lblNote = new Label
                                {
                                    Text = $"Evaluator Note: \"{remarks}\"",
                                    Font = new Font("Segoe UI", 8.5F, FontStyle.Italic),
                                    ForeColor = ColorSlate500,
                                    Location = new Point(15, 56),
                                    Size = new Size(evalCard.Width - 140, 45)
                                };
                                evalCard.Controls.Add(lblNote);
                            }
                            else
                            {
                                Label lblNoData = new Label
                                {
                                    Text = "⚠️ No interview evaluation records found for this candidate yet.",
                                    Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                                    ForeColor = ColorSlate500,
                                    Location = new Point(15, 35),
                                    AutoSize = true
                                };
                                evalCard.Controls.Add(lblNoData);
                            }

                            pnlRightCard.Controls.Add(evalCard);
                        }
                    }
                }
            }
            catch
            {
                // Fallback default
            }
        }

        private void rdoDecision_CheckedChanged(object sender, EventArgs e)
        {
            UpdateDecisionCardUI();
        }

        /// <summary>
        /// Dynamically changes form theme accents depending on the selected decision pathway.
        /// </summary>
        private void UpdateDecisionCardUI()
        {
            if (btnSave == null) return;

            if (rdoAccepted.Checked)
            {
                btnSave.BackColor = Color.FromArgb(40, 167, 69); // Emerald Green
                btnSave.Text = "Approve & Hire";
            }
            else if (rdoRejected.Checked)
            {
                btnSave.BackColor = Color.FromArgb(220, 53, 69); // Crimson Red
                btnSave.Text = "Reject Candidate";
            }
            else
            {
                btnSave.BackColor = Color.FromArgb(240, 173, 78); // Soft Amber
                btnSave.Text = "Place On Hold";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (selectedApplicationID == 0)
            {
                MessageBox.Show("Please select an applicant from the list first.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtRemarks.Text))
            {
                MessageBox.Show("Please provide final remarks or justification for this action.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string decision = rdoAccepted.Checked ? "Accepted" : (rdoRejected.Checked ? "Rejected" : "On Hold");

            DialogResult confirm = MessageBox.Show(
                $"Are you sure you want to finalize this decision as: {decision}?",
                "Confirm Executive Action",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            OleDbConnection conn = DBConnection.GetConnection();
            if (conn == null) return;

            OleDbTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();

                // 1. Log detailed decision details inside the HiringDecisions table
                string insertDecision = @"
                    INSERT INTO HiringDecisions
                    (
                        ApplicationID,
                        Decision,
                        Remarks,
                        DecisionBy,
                        DecisionDate
                    )
                    VALUES
                    (?, ?, ?, ?, ?)";

                using (OleDbCommand cmd = new OleDbCommand(insertDecision, conn, transaction))
                {
                    cmd.Parameters.Add("@ApplicationID", OleDbType.Integer).Value = selectedApplicationID;
                    cmd.Parameters.Add("@Decision", OleDbType.VarWChar).Value = decision;
                    cmd.Parameters.Add("@Remarks", OleDbType.VarWChar).Value = txtRemarks.Text.Trim();
                    cmd.Parameters.Add("@DecisionBy", OleDbType.Integer).Value = UserSession.UserID > 0 ? UserSession.UserID : 1;
                    cmd.Parameters.Add("@DecisionDate", OleDbType.Date).Value = DateTime.Now;

                    cmd.ExecuteNonQuery();
                }

                // 2. Set the official status inside the primary Applications registry
                string updateApplication = @"
                    UPDATE Applications
                    SET [Status] = ?
                    WHERE ApplicationID = ?";

                using (OleDbCommand cmd = new OleDbCommand(updateApplication, conn, transaction))
                {
                    cmd.Parameters.Add("@Status", OleDbType.VarWChar).Value = decision;
                    cmd.Parameters.Add("@ApplicationID", OleDbType.Integer).Value = selectedApplicationID;

                    cmd.ExecuteNonQuery();
                }

                // 3. Document the step timeline in ApplicationStatusHistory
                string insertHistory = @"
                    INSERT INTO ApplicationStatusHistory
                    (
                        ApplicationID,
                        [Status],
                        DateChanged
                    )
                    VALUES
                    (?, ?, ?)";

                using (OleDbCommand cmd = new OleDbCommand(insertHistory, conn, transaction))
                {
                    cmd.Parameters.Add("@ApplicationID", OleDbType.Integer).Value = selectedApplicationID;
                    cmd.Parameters.Add("@Status", OleDbType.VarWChar).Value = decision;
                    cmd.Parameters.Add("@DateChanged", OleDbType.Date).Value = DateTime.Now;

                    cmd.ExecuteNonQuery();
                }

                // 4. Record the final administrative decision inside AuditTrail
                try
                {
                    string logActionText = $"Final decision '{decision}' declared for application #{selectedApplicationID} by User ID {UserSession.UserID}.";
                    string insertAudit = "INSERT INTO AuditTrail (UserID, [Action], DateCreated) VALUES (?, ?, ?)";
                    using (OleDbCommand cmd = new OleDbCommand(insertAudit, conn, transaction))
                    {
                        cmd.Parameters.Add("@UserID", OleDbType.Integer).Value = UserSession.UserID > 0 ? UserSession.UserID : 1;
                        cmd.Parameters.Add("@Action", OleDbType.VarWChar).Value = logActionText;
                        cmd.Parameters.Add("@Timestamp", OleDbType.Date).Value = DateTime.Now;
                        cmd.ExecuteNonQuery();
                    }
                }
                catch
                {
                    // Fail-safe to bypass if audit trail configuration varies slightly
                }

                transaction.Commit();

                MessageBox.Show("Hiring decision saved successfully and timeline history updated.", "Action Succeeded", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ResetDetailsCard();
                txtRemarks.Clear();
                rdoAccepted.Checked = true;

                LoadApplications();
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                MessageBox.Show("An error occurred during transaction execution. Changes rolled back.\nDetails: " + ex.Message, "Transaction Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ResetDetailsCard()
        {
            selectedApplicationID = 0;
            lblApplicant.Text = "Applicant: —";
            lblJob.Text = "Job: —";

            PurgeProgrammaticCards(pnlRightCard);
        }
    }
}