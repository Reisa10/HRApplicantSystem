using HRApplicantSystem.Database;
using HRApplicantSystem.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HRApplicantSystem.Forms.HR
{
    public partial class InterviewEvaluationForm : Form
    {
        private int selectedApplicationID = 0;
        private int selectedScheduleID = 0;
        private DataTable dtApplications = new DataTable();

        // Dynamically added professional workflow controls
        private Label lblScoreDesc;
        private Label lblWorkflowTitle;
        private Label lblRecDropdown;
        private ComboBox cmbRecommendationType;
        private Label lblAssessmentPrompt;
        private ComboBox cmbAssessmentTypes;
        private Label lblEvaluatorContext;
        private Panel pnlInfoAlert; // Informational banner to fill empty space

        public InterviewEvaluationForm()
        {
            InitializeComponent();
        }

        private void InterviewEvaluationForm_Load(object sender, EventArgs e)
        {
            ApplyProfessionalTheme();

            // Centering Fix: Snap form coordinates precisely over the main dashboard
            CenterFormOnDashboard();

            LoadApplications();
            LoadAssessmentTypes();

            // Set initial state
            rdoPass.Checked = true;
            UpdateRecommendationDropdown();
            UpdateScoreDescriptor();

            if (lblEvaluatorContext != null)
            {
                lblEvaluatorContext.Text = $"Evaluator: {UserSession.FullName ?? "HR Specialist"} ({UserSession.Role ?? "Staff"})";
            }
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
        /// Programs modern UI styling, adjusts boundaries dynamically to prevent clipping, and binds maintenance dropdowns.
        /// </summary>
        private void ApplyProfessionalTheme()
        {
            // Set corporate-sized window to prevent clipping under Windows DPI scaling
            this.Width = 840;
            this.Height = 580; // Increased to accommodate the expanded groupbox safely

            // Expand internal GroupBoxes to occupy the new widescreen layout and give height breathing room
            grpApplicants.Width = 800;

            grpEvaluation.Width = 800;
            grpEvaluation.Height = 285; // Increased from 265 to prevent bottom button/textbox clipping!

            // Corporate Color Palette 
            Color navyPrimary = Color.FromArgb(30, 41, 59);      // Slate 800 (Headers/Main theme)
            Color accentBlue = Color.FromArgb(37, 99, 235);      // Blue 600 (Primary actions)
            Color bgNeutral = Color.FromArgb(241, 245, 249);     // Slate 100 (Form Background)
            Color textDark = Color.FromArgb(15, 23, 42);         // Slate 900
            Color textMuted = Color.FromArgb(100, 116, 139);     // Slate 500
            Color borderLight = Color.FromArgb(226, 232, 240);   // Slate 200

            this.BackColor = bgNeutral;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular);

            // GroupBoxes styling
            grpApplicants.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            grpApplicants.ForeColor = navyPrimary;
            grpEvaluation.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            grpEvaluation.ForeColor = navyPrimary;

            // Left-hand Labels inside GroupBox
            lblApplicant.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblApplicant.ForeColor = textDark;
            lblJob.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            lblJob.ForeColor = textMuted;
            lblInterviewDate.Font = new Font("Segoe UI", 9.5F, FontStyle.Italic);
            lblInterviewDate.ForeColor = textMuted;
            Scorelabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            Scorelabel.ForeColor = textDark;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            label1.ForeColor = textDark;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            label2.ForeColor = textDark;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            label3.ForeColor = textDark;

            // Inputs styling
            nudScore.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            nudScore.Minimum = 0;
            nudScore.Maximum = 100;
            nudScore.ValueChanged += (s, ev) => UpdateScoreDescriptor();

            rdoPass.FlatStyle = FlatStyle.Flat;
            rdoPass.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            rdoPass.ForeColor = Color.FromArgb(22, 163, 74); // Green
            rdoPass.CheckedChanged += (s, ev) => UpdateRecommendationDropdown();

            rdoFail.FlatStyle = FlatStyle.Flat;
            rdoFail.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            rdoFail.ForeColor = Color.FromArgb(220, 38, 38); // Red
            rdoFail.CheckedChanged += (s, ev) => UpdateRecommendationDropdown();

            txtRemarks.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            txtRemarks.ForeColor = textDark;
            txtRemarks.Width = 273; // Lock standard width
            txtRecommendation.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            txtRecommendation.ForeColor = textDark;
            txtRecommendation.Width = 273; // Lock standard width

            // Save & Back Buttons styling
            StylePrimaryButton(btnSave, accentBlue);
            StyleSecondaryButton(btnBack, textMuted);

            // Move action buttons dynamically to align with the bottom-right baseline of the evaluation box (Y = 230)
            btnBack.Width = 90;
            btnBack.Height = 30;
            btnBack.Left = grpEvaluation.Width - 15 - btnBack.Width; // Aligns perfectly to the right edge
            btnBack.Top = 230;

            btnSave.Width = 120;
            btnSave.Height = 30;
            btnSave.Left = btnBack.Left - 10 - btnSave.Width; // Positions next to the back button
            btnSave.Top = 230;

            // Wire up Back button to exit safely
            btnBack.Click += (s, ev) => this.Close();

            // Stylize the DataGridView to match modern enterprise systems
            StyleDataGridView(dgvApplicants, borderLight);

            // --------------------------------------------------------
            // SPATIALLY BALANCED INJECTIONS (Placed safely in the X: 440 to 780 region)
            // --------------------------------------------------------

            // Dynamic Live Score Grade descriptor badge
            lblScoreDesc = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Location = new Point(nudScore.Right + 12, nudScore.Top + 3)
            };
            grpEvaluation.Controls.Add(lblScoreDesc);

            // Dynamic Evaluator context title
            lblEvaluatorContext = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Regular),
                ForeColor = textMuted,
                Location = new Point(440, 24),
                Text = "Evaluator: -"
            };
            grpEvaluation.Controls.Add(lblEvaluatorContext);

            // Separator/Section Header label
            lblWorkflowTitle = new Label
            {
                Text = "WORKFLOW ROUTING",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = accentBlue,
                Location = new Point(440, 48),
                Size = new Size(320, 18)
            };
            grpEvaluation.Controls.Add(lblWorkflowTitle);

            // Action Recommendation label
            lblRecDropdown = new Label
            {
                Text = "Next Application State:",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Regular),
                ForeColor = textDark,
                Location = new Point(440, 75),
                Size = new Size(320, 18)
            };
            grpEvaluation.Controls.Add(lblRecDropdown);

            // Recommendation dropdown
            cmbRecommendationType = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                Location = new Point(440, 95),
                Size = new Size(325, 25)
            };
            cmbRecommendationType.SelectedIndexChanged += cmbRecommendationType_SelectedIndexChanged;
            grpEvaluation.Controls.Add(cmbRecommendationType);

            // Dynamic Maintenance-connected Assessment Prompt Label
            lblAssessmentPrompt = new Label
            {
                Text = "Required Assessment Type (From Maintenance):",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Regular),
                ForeColor = textDark,
                Location = new Point(440, 132),
                Size = new Size(320, 18),
                Visible = false
            };
            grpEvaluation.Controls.Add(lblAssessmentPrompt);

            // Dynamic Maintenance-connected Assessment Dropdown
            cmbAssessmentTypes = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                Location = new Point(440, 152),
                Size = new Size(325, 25),
                Visible = false
            };
            grpEvaluation.Controls.Add(cmbAssessmentTypes);

            // --------------------------------------------------------
            // STREAMLINED INFO ALERT PANEL (Adjusted height and font size to prevent clipping)
            // --------------------------------------------------------
            pnlInfoAlert = new Panel
            {
                Location = new Point(12, 102),
                Size = new Size(394, 52), // Height reduced slightly to provide a 12px gap to 'Remarks'
                BackColor = Color.FromArgb(239, 246, 255), // Light professional blue tint
                Visible = false,
                Padding = new Padding(10, 6, 10, 6)
            };
            // Paint thin border on alert panel
            pnlInfoAlert.Paint += (s, ev) =>
            {
                using (Pen p = new Pen(Color.FromArgb(191, 219, 254), 1))
                {
                    ev.Graphics.DrawRectangle(p, 0, 0, pnlInfoAlert.Width - 1, pnlInfoAlert.Height - 1);
                }
            };

            Label lblAlertText = new Label
            {
                Text = "ℹ️  Direct Route: Skipping numeric scoring to route candidate straight to Final Review. Please write summary comments below.",
                Font = new Font("Segoe UI", 8F, FontStyle.Regular), // Size adjusted slightly to prevent text clipping
                ForeColor = Color.FromArgb(30, 64, 175), // Deep blue text
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            pnlInfoAlert.Controls.Add(lblAlertText);
            grpEvaluation.Controls.Add(pnlInfoAlert);
        }

        private void StyleDataGridView(DataGridView dgv, Color borderColor)
        {
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = borderColor;
            dgv.EnableHeadersVisualStyles = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.RowHeadersVisible = false;

            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(30, 41, 59),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(8, 4, 8, 4)
            };
            dgv.ColumnHeadersDefaultCellStyle = headerStyle;
            dgv.ColumnHeadersHeight = 35;

            DataGridViewCellStyle cellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = Color.FromArgb(51, 65, 85),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                SelectionBackColor = Color.FromArgb(239, 246, 255),
                SelectionForeColor = Color.FromArgb(29, 78, 216),
                Padding = new Padding(8, 4, 8, 4)
            };
            dgv.DefaultCellStyle = cellStyle;
            dgv.RowTemplate.Height = 32;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
        }

        private void StylePrimaryButton(Button btn, Color themeColor)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.BackColor = themeColor;
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.FlatAppearance.BorderSize = 0;
        }

        private void StyleSecondaryButton(Button btn, Color textColor)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.BackColor = Color.White;
            btn.ForeColor = textColor;
            btn.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            btn.Cursor = Cursors.Hand;
            btn.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            btn.FlatAppearance.BorderSize = 1;
        }

        private void LoadApplications()
        {
            try
            {
                using (OleDbConnection conn = DBConnection.GetConnection())
                {
                    if (conn == null) return;

                    // Updated query to filter out cancelled/completed schedules
                    string query = @"
                        SELECT
                            a.ApplicationID,
                            i.InterviewScheduleID,
                            ap.FirstName & ' ' & ap.LastName AS FullName,
                            p.PositionName AS JobTitle,
                            et.TypeName AS EmploymentType,
                            i.InterviewDate,
                            i.Interviewer,
                            i.Mode,
                            i.Location
                        FROM ((((Applications a
                        INNER JOIN Applicants ap ON a.ApplicantID = ap.ApplicantID)
                        INNER JOIN JobVacancies j ON a.JobID = j.JobID)
                        INNER JOIN Positions p ON j.PositionID = p.PositionID)
                        LEFT JOIN EmploymentTypes et ON j.EmploymentTypeID = et.EmploymentTypeID)
                        INNER JOIN InterviewSchedules i ON a.ApplicationID = i.ApplicationID
                        WHERE a.Status IN ('For Interview', 'Interview Scheduled')
                          AND i.Status = 'Scheduled'"; // Fixed: Only show the active scheduled interview

                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn))
                    {
                        dtApplications.Clear();
                        adapter.Fill(dtApplications);
                        dgvApplicants.DataSource = dtApplications;

                        // Hide primary ID keys from display
                        if (dgvApplicants.Columns["ApplicationID"] != null)
                            dgvApplicants.Columns["ApplicationID"].Visible = false;
                        if (dgvApplicants.Columns["InterviewScheduleID"] != null)
                            dgvApplicants.Columns["InterviewScheduleID"].Visible = false;

                        // Format headers neatly
                        dgvApplicants.Columns["FullName"].HeaderText = "Applicant Name";
                        dgvApplicants.Columns["JobTitle"].HeaderText = "Applied Position";
                        dgvApplicants.Columns["EmploymentType"].HeaderText = "Employment Type";
                        dgvApplicants.Columns["InterviewDate"].HeaderText = "Schedule Date";
                        dgvApplicants.Columns["Interviewer"].HeaderText = "Interviewer";

                        dgvApplicants.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading applicants:\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LoadAssessmentTypes()
        {
            try
            {
                using (OleDbConnection conn = DBConnection.GetConnection())
                {
                    if (conn == null) return;
                    conn.Open();

                    string query = "SELECT TypeName FROM AssessmentTypes WHERE IsActive = True ORDER BY TypeName ASC";
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        cmbAssessmentTypes.Items.Clear();
                        while (reader.Read())
                        {
                            cmbAssessmentTypes.Items.Add(reader["TypeName"].ToString());
                        }
                    }

                    if (cmbAssessmentTypes.Items.Count > 0)
                        cmbAssessmentTypes.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading maintenance configurations (Assessment Types):\n" + ex.Message,
                    "Maintenance Connect Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void UpdateScoreDescriptor()
        {
            int val = (int)nudScore.Value;
            if (val >= 90)
            {
                lblScoreDesc.Text = "Outstanding";
                lblScoreDesc.ForeColor = Color.FromArgb(22, 163, 74); // Green
            }
            else if (val >= 75)
            {
                lblScoreDesc.Text = "Competent (Pass)";
                lblScoreDesc.ForeColor = Color.FromArgb(37, 99, 235); // Blue
            }
            else if (val >= 50)
            {
                lblScoreDesc.Text = "Marginal Pass";
                lblScoreDesc.ForeColor = Color.FromArgb(217, 119, 6); // Amber
            }
            else
            {
                lblScoreDesc.Text = "Unsatisfactory";
                lblScoreDesc.ForeColor = Color.FromArgb(220, 38, 38); // Red
            }
        }

        private void UpdateRecommendationDropdown()
        {
            cmbRecommendationType.Items.Clear();

            if (rdoPass.Checked)
            {
                cmbRecommendationType.Items.Add("Recommend for Final Decision");
                cmbRecommendationType.Items.Add("Recommend for Assessment");
                cmbRecommendationType.SelectedIndex = 0;
            }
            else
            {
                cmbRecommendationType.Items.Add("Do Not Recommend (Reject)");
                cmbRecommendationType.SelectedIndex = 0;
            }
        }

        private void cmbRecommendationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedVal = cmbRecommendationType.SelectedItem?.ToString();
            bool isAssessment = (selectedVal == "Recommend for Assessment");

            // Toggle visibility for Maintenance choices
            lblAssessmentPrompt.Visible = isAssessment;
            cmbAssessmentTypes.Visible = isAssessment;

            // Define which details to show. Hide if "Recommend for Final Decision" is chosen.
            bool showEvaluationDetails = (selectedVal != "Recommend for Final Decision");

            // Kept always visible so the form never feels empty or lacks applicant context
            lblApplicant.Visible = true;
            lblJob.Visible = true;
            lblInterviewDate.Visible = true;

            // Toggle intermediate grading input controls
            Scorelabel.Visible = showEvaluationDetails;
            nudScore.Visible = showEvaluationDetails;
            label1.Visible = showEvaluationDetails; // "Result:" label
            rdoPass.Visible = showEvaluationDetails;
            rdoFail.Visible = showEvaluationDetails;

            if (lblScoreDesc != null)
            {
                lblScoreDesc.Visible = showEvaluationDetails;
            }

            // Toggle dynamic alert banner to fill the empty layout gap
            if (pnlInfoAlert != null)
            {
                pnlInfoAlert.Visible = !showEvaluationDetails;
            }
        }

        private void dgvApplicants_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvApplicants.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvApplicants.SelectedRows[0];

                selectedApplicationID = Convert.ToInt32(row.Cells["ApplicationID"].Value);

                // Handle conditional selected schedule ID mapping safely (Left Joins might return DBNull values)
                object scheduleCell = row.Cells["InterviewScheduleID"].Value;
                selectedScheduleID = (scheduleCell != DBNull.Value && scheduleCell != null) ? Convert.ToInt32(scheduleCell) : 0;

                lblApplicant.Text = "Applicant: " + row.Cells["FullName"].Value?.ToString();

                string jobTitle = row.Cells["JobTitle"].Value?.ToString() ?? "";
                string empType = row.Cells["EmploymentType"].Value?.ToString() ?? "";

                // Append dynamic EmploymentType into the job summary string safely
                if (!string.IsNullOrEmpty(empType))
                {
                    lblJob.Text = $"Job: {jobTitle} ({empType})";
                }
                else
                {
                    lblJob.Text = "Job: " + jobTitle;
                }

                if (row.Cells["InterviewDate"].Value != DBNull.Value && row.Cells["InterviewDate"].Value != null)
                {
                    lblInterviewDate.Text = "Interview Date: " + Convert.ToDateTime(row.Cells["InterviewDate"].Value).ToString("MMMM dd, yyyy");
                }
                else
                {
                    lblInterviewDate.Text = "Interview Date: -";
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (selectedApplicationID == 0)
            {
                MessageBox.Show("Please select an applicant from the schedule list first.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtRemarks.Text))
            {
                MessageBox.Show("Please provide evaluation feedback remarks.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRemarks.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtRecommendation.Text))
            {
                MessageBox.Show("Please enter specific recommendation action notes.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRecommendation.Focus();
                return;
            }

            string recommendationType = cmbRecommendationType.SelectedItem?.ToString();
            string selectedAssessment = cmbAssessmentTypes.SelectedItem?.ToString();

            if (recommendationType == "Recommend for Assessment" && string.IsNullOrEmpty(selectedAssessment))
            {
                MessageBox.Show("Please configure or select a valid Assessment Type from Maintenance.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Determine strict status matching requirement workflow flow rules
            string result;
            int finalScore;
            string applicationStatus = "Rejected";

            // If "Recommend for Final Decision" is selected, the details were hidden. 
            // We apply baseline values programmatically to satisfy database schema constraints.
            if (recommendationType == "Recommend for Final Decision")
            {
                result = "Pass";
                finalScore = 75; // Standard passing score default
                applicationStatus = "For Final Decision";
            }
            else if (recommendationType == "Recommend for Assessment")
            {
                result = "Pass";
                finalScore = Convert.ToInt32(nudScore.Value);
                applicationStatus = "For Final Decision"; // Direct transition to prevent getting stuck
            }
            else
            {
                result = "Fail";
                finalScore = Convert.ToInt32(nudScore.Value);
                applicationStatus = "Rejected";
            }

            string confirmationPrompt = $"Submit evaluation details for this candidate?\n\nOutcome: {result}\nNew Application State: {applicationStatus}";
            DialogResult confirm = MessageBox.Show(confirmationPrompt, "Confirm Evaluation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            using (OleDbConnection conn = DBConnection.GetConnection())
            {
                if (conn == null) return;
                OleDbTransaction transaction = null;

                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction();

                    // Prepend Maintenance Assessment Category dynamically to the final text box if applicable
                    string finalRecommendation = txtRecommendation.Text.Trim();
                    if (recommendationType == "Recommend for Assessment")
                    {
                        finalRecommendation = $"[Required Assessment Type: {selectedAssessment}] - " + finalRecommendation;
                    }

                    // 1. Write the evaluation results to the DB
                    string insertEvaluation = @"
                        INSERT INTO InterviewEvaluations (ApplicationID, Score, Remarks, Result, Recommendation)
                        VALUES (?, ?, ?, ?, ?)";

                    using (OleDbCommand cmd = new OleDbCommand(insertEvaluation, conn, transaction))
                    {
                        cmd.Parameters.Add("@AppID", OleDbType.Integer).Value = selectedApplicationID;
                        cmd.Parameters.Add("@Score", OleDbType.Integer).Value = finalScore;
                        cmd.Parameters.Add("@Remarks", OleDbType.VarWChar).Value = txtRemarks.Text.Trim();
                        cmd.Parameters.Add("@Result", OleDbType.VarWChar).Value = result;
                        cmd.Parameters.Add("@Rec", OleDbType.VarWChar).Value = finalRecommendation;
                        cmd.ExecuteNonQuery();
                    }

                    // 2. Set new overall Application status
                    string updateApplication = "UPDATE Applications SET [Status] = ? WHERE ApplicationID = ?";
                    using (OleDbCommand cmd = new OleDbCommand(updateApplication, conn, transaction))
                    {
                        cmd.Parameters.Add("@Status", OleDbType.VarWChar).Value = applicationStatus;
                        cmd.Parameters.Add("@AppID", OleDbType.Integer).Value = selectedApplicationID;
                        cmd.ExecuteNonQuery();
                    }

                    // 3. Complete the Interview Schedule record status so it moves off the active dashboard list (if a schedule exists)
                    if (selectedScheduleID > 0)
                    {
                        string updateSchedule = "UPDATE InterviewSchedules SET [Status] = ? WHERE InterviewScheduleID = ?";
                        using (OleDbCommand cmd = new OleDbCommand(updateSchedule, conn, transaction))
                        {
                            cmd.Parameters.Add("@Status", OleDbType.VarWChar).Value = "Completed";
                            cmd.Parameters.Add("@SchedID", OleDbType.Integer).Value = selectedScheduleID;
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // 4. Log the timeline change inside ApplicationStatusHistory
                    string insertHistory = "INSERT INTO ApplicationStatusHistory (ApplicationID, [Status], DateChanged) VALUES (?, ?, ?)";
                    using (OleDbCommand cmd = new OleDbCommand(insertHistory, conn, transaction))
                    {
                        cmd.Parameters.Add("@AppID", OleDbType.Integer).Value = selectedApplicationID;
                        cmd.Parameters.Add("@Status", OleDbType.VarWChar).Value = applicationStatus;
                        cmd.Parameters.Add("@Date", OleDbType.Date).Value = DateTime.Now;
                        cmd.ExecuteNonQuery();
                    }

                    // 5. Safely register action logs to the AuditTrail
                    try
                    {
                        // Fixed: Uses correct column 'DateCreated' instead of 'ActionDate'
                        string insertAudit = "INSERT INTO AuditTrail (UserID, [Action], DateCreated) VALUES (?, ?, ?)";
                        using (OleDbCommand cmd = new OleDbCommand(insertAudit, conn, transaction))
                        {
                            int activeUserID = UserSession.UserID > 0 ? UserSession.UserID : 1; // Fallback helper ID
                            cmd.Parameters.Add("@UserID", OleDbType.Integer).Value = activeUserID;
                            cmd.Parameters.Add("@Action", OleDbType.VarWChar).Value =
                                $"Evaluated Application ID {selectedApplicationID}. Result: {result}, Recommended Status: {applicationStatus}.";
                            cmd.Parameters.Add("@DateCreated", OleDbType.Date).Value = DateTime.Now;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("AuditTrail Log Error: " + ex.Message);
                    }

                    transaction.Commit();

                    MessageBox.Show("Interview evaluation recorded. Status history updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetFormState();
                    LoadApplications();
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();
                    MessageBox.Show("Error recording evaluation transactions:\n" + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ResetFormState()
        {
            selectedApplicationID = 0;
            selectedScheduleID = 0;
            lblApplicant.Text = "Applicant: —";
            lblJob.Text = "Job: —";
            lblInterviewDate.Text = "Interview Date: -";
            txtRemarks.Clear();
            txtRecommendation.Clear();
            nudScore.Value = 75;
            rdoPass.Checked = true;
            UpdateRecommendationDropdown();
        }
    }
}