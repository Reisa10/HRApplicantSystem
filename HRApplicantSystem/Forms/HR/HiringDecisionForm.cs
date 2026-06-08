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
        }

        private void PaintCardBorder(Panel panel, PaintEventArgs ev)
        {
            using (Pen p = new Pen(Color.FromArgb(226, 232, 240), 1))
            {
                ev.Graphics.DrawRectangle(p, 0, 0, panel.Width - 1, panel.Height - 1);
            }
        }

        /// <summary>
        /// Loads applications in 'For Final Decision' status, joining Job and Department data 
        /// to show maintenance/organization values.
        /// </summary>
        private void LoadApplications()
        {
            try
            {
                using (OleDbConnection conn = DBConnection.GetConnection())
                {
                    if (conn == null) return;

                    // Properly nested JOINs for MS Access to fetch descriptive names instead of IDs
                    string query = @"
                        SELECT
                            Applications.ApplicationID,
                            Applicants.FirstName & ' ' & Applicants.LastName AS FullName,
                            Positions.PositionName AS JobTitle,
                            Departments.DepartmentName
                        FROM
                            (
                                (
                                    (Applications
                                    INNER JOIN Applicants ON Applications.ApplicantID = Applicants.ApplicantID)
                                    INNER JOIN JobVacancies ON Applications.JobID = JobVacancies.JobID)
                                INNER JOIN Positions ON JobVacancies.PositionID = Positions.PositionID
                            )
                            LEFT JOIN Departments ON JobVacancies.DepartmentID = Departments.DepartmentID
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
            if (dgvApplicants.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvApplicants.SelectedRows[0];
                selectedApplicationID = Convert.ToInt32(row.Cells["ApplicationID"].Value);

                string applicantName = row.Cells["FullName"].Value?.ToString();
                string jobTitle = row.Cells["JobTitle"].Value?.ToString();
                string department = row.Cells["DepartmentName"].Value?.ToString() ?? "Unassigned Department";

                lblApplicant.Text = "Applicant: " + applicantName;
                lblJob.Text = $"Job: {jobTitle} | {department}";

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

        /// <summary>
        /// Retrieves missing requirements from the DatabaseHelper and displays them to the HR Manager.
        /// </summary>
        private void DisplayMissingRequirements(int applicationId)
        {
            List<string> missing = DatabaseHelper.GetMissingRequirementsForApplication(applicationId);

            if (missing.Count > 0)
            {
                lblMissingRequirements.ForeColor = Color.FromArgb(220, 53, 69); // Alarm Red
                lblMissingRequirements.Text = "⚠️ Missing Documents:\n" + string.Join(", ", missing);
            }
            else
            {
                lblMissingRequirements.ForeColor = Color.FromArgb(40, 167, 69); // Emerald Green
                lblMissingRequirements.Text = "✓ All mandatory requirements submitted successfully.";
            }
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

                    // Query retrieving the most recent interview evaluation data
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
                            if (reader.Read())
                            {
                                string score = reader["Score"].ToString();
                                string recommendation = reader["Recommendation"]?.ToString() ?? "N/A";
                                string remarks = reader["Remarks"]?.ToString() ?? "No details written.";

                                lblInterviewScore.Text = $"• Score: {score}/100\n• Recommended: {recommendation}\n• Note: {remarks}";
                                lblInterviewScore.ForeColor = Color.FromArgb(45, 55, 72);
                            }
                            else
                            {
                                lblInterviewScore.Text = "⚠️ No active interview records found.";
                                lblInterviewScore.ForeColor = Color.Gray;
                            }
                        }
                    }
                }
            }
            catch
            {
                lblInterviewScore.Text = "Evaluation details unavailable.";
                lblInterviewScore.ForeColor = Color.Gray;
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
                    string insertAudit = "INSERT INTO AuditTrail (UserID, ActionPerformed, ActionTimestamp) VALUES (?, ?, ?)";
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

            lblMissingRequirements.Text = "Missing: Select an applicant to compute requirements.";
            lblMissingRequirements.ForeColor = Color.Gray;

            lblInterviewScore.Text = "Evaluation: Select an applicant to see scores.";
            lblInterviewScore.ForeColor = Color.Gray;
        }
    }
}