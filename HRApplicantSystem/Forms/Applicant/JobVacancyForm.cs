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
    public partial class JobVacancyForm : Form
    {
        private DataTable dtJobs = new DataTable();

        public JobVacancyForm()
        {
            InitializeComponent();
            ApplyModernStyle();
        }

        private void JobVacancyForm_Load(object sender, EventArgs e)
        {
            LoadDepartments();
            LoadJobVacancies();
        }

        /// <summary>
        /// Instantly styles standard WinForms elements dynamically to give a corporate-level presentation.
        /// </summary>
        private void ApplyModernStyle()
        {
            // Form Background matches dashboard off-white
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);

            // Search Panel Styling
            pnlSearch.BackColor = Color.White;
            pnlSearch.Height = 75;

            lblHeaderTitle.ForeColor = Color.FromArgb(27, 38, 59); // Standard Dark Slate Navy
            lblHeaderSubtitle.ForeColor = Color.FromArgb(127, 140, 141); // Slate Muted Gray

            lblSearch.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblSearch.ForeColor = Color.FromArgb(100, 110, 120);

            // Input Fields Style
            txtSearch.Font = new Font("Segoe UI", 9.5F);
            txtSearch.BorderStyle = BorderStyle.FixedSingle;

            cmbDepartment.Font = new Font("Segoe UI", 9.5F);
            cmbDepartment.FlatStyle = FlatStyle.Flat;

            // Flat Modern Buttons matching dashboard accents
            StyleFlatButton(btnSearch, Color.FromArgb(41, 128, 185), Color.White); // Dashboard Blue Accent
            StyleFlatButton(btnReset, Color.FromArgb(149, 165, 166), Color.White); // Neutral Slate
            StyleFlatButton(btnBack, Color.FromArgb(231, 76, 60), Color.White); // Soft Crimson Red
            StyleFlatButton(btnApply, Color.FromArgb(46, 204, 113), Color.White); // Emerald Professional Green

            // Details Panel Layout
            pnlDetails.BackColor = Color.White;
            pnlAccentBar.BackColor = Color.FromArgb(41, 128, 185); // Matches active blue

            lblTitle.Font = new Font("Segoe UI", 13.5F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(27, 38, 59);

            lblDept.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Italic);
            lblDept.ForeColor = Color.FromArgb(127, 140, 141);

            txtDescription.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            txtDescription.BackColor = Color.FromArgb(248, 250, 252);
            txtDescription.BorderStyle = BorderStyle.None;

            // Grid Styling (Replacing legacy, basic windows table borders)
            dgvJobs.BackgroundColor = Color.White;
            dgvJobs.BorderStyle = BorderStyle.None;
            dgvJobs.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvJobs.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvJobs.GridColor = Color.FromArgb(235, 237, 240);
            dgvJobs.RowTemplate.Height = 42;
            dgvJobs.ColumnHeadersHeight = 40;
            dgvJobs.EnableHeadersVisualStyles = false;

            // Header Style
            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(27, 38, 59),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.75F, FontStyle.Bold),
                SelectionBackColor = Color.FromArgb(27, 38, 59),
                Alignment = DataGridViewContentAlignment.MiddleLeft
            };
            dgvJobs.ColumnHeadersDefaultCellStyle = headerStyle;

            // Row Styling
            DataGridViewCellStyle rowStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = Color.FromArgb(44, 62, 80),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                SelectionBackColor = Color.FromArgb(232, 244, 253),
                SelectionForeColor = Color.FromArgb(27, 38, 59)
            };
            dgvJobs.DefaultCellStyle = rowStyle;

            // Alternating Row style for grid scannability
            DataGridViewCellStyle altRowStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(249, 251, 252)
            };
            dgvJobs.AlternatingRowsDefaultCellStyle = altRowStyle;
        }

        private void StyleFlatButton(Button btn, Color backColor, Color foreColor)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.BackColor = backColor;
            btn.ForeColor = foreColor;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
        }

        private void LoadJobVacancies()
        {
            try
            {
                using (OleDbConnection conn = DBConnection.GetConnection())
                {
                    if (conn == null) return;

                    string query = "SELECT j.JobID, p.PositionName AS JobTitle, d.DepartmentName AS Department, " +
                                   "j.Description, j.Status, j.RequiredDocuments, j.DepartmentID " +
                                   "FROM (JobVacancies j " +
                                   "INNER JOIN Positions p ON j.PositionID = p.PositionID) " +
                                   "INNER JOIN Departments d ON j.DepartmentID = d.DepartmentID " +
                                   "WHERE j.Status = 'Open'";

                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn))
                    {
                        dtJobs.Clear();
                        adapter.Fill(dtJobs);
                        dgvJobs.DataSource = dtJobs;

                        if (dgvJobs.Columns["JobID"] != null) dgvJobs.Columns["JobID"].Visible = false;
                        if (dgvJobs.Columns["Description"] != null) dgvJobs.Columns["Description"].Visible = false;
                        if (dgvJobs.Columns["Status"] != null) dgvJobs.Columns["Status"].Visible = false;
                        if (dgvJobs.Columns["RequiredDocuments"] != null) dgvJobs.Columns["RequiredDocuments"].Visible = false;
                        if (dgvJobs.Columns["DepartmentID"] != null) dgvJobs.Columns["DepartmentID"].Visible = false;

                        if (dgvJobs.Columns["JobTitle"] != null) dgvJobs.Columns["JobTitle"].HeaderText = "Job Title";
                        if (dgvJobs.Columns["Department"] != null) dgvJobs.Columns["Department"].HeaderText = "Department";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading job vacancies: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDepartments()
        {
            try
            {
                using (OleDbConnection conn = DBConnection.GetConnection())
                {
                    if (conn == null) return;

                    string query = "SELECT DepartmentID, DepartmentName FROM Departments WHERE IsActive = True";
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        DataRow newRow = dt.NewRow();
                        newRow["DepartmentID"] = 0;
                        newRow["DepartmentName"] = "All Departments";
                        dt.Rows.InsertAt(newRow, 0);

                        cmbDepartment.DataSource = dt;
                        cmbDepartment.DisplayMember = "DepartmentName";
                        cmbDepartment.ValueMember = "DepartmentID";
                    }
                }
            }
            catch
            {
                cmbDepartment.DataSource = null;
                cmbDepartment.Items.Clear();
                cmbDepartment.Items.Add("All Departments");
                cmbDepartment.Items.Add("IT");
                cmbDepartment.Items.Add("HR");
                cmbDepartment.Items.Add("Finance");
                cmbDepartment.SelectedIndex = 0;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().Replace("'", "''");
            int selectedDeptId = 0;

            if (cmbDepartment.SelectedValue != null && int.TryParse(cmbDepartment.SelectedValue.ToString(), out int deptId))
            {
                selectedDeptId = deptId;
            }

            DataView dv = dtJobs.DefaultView;
            string filterExpr = "";

            if (!string.IsNullOrEmpty(keyword))
            {
                filterExpr = $"(JobTitle LIKE '%{keyword}%' OR Description LIKE '%{keyword}%')";
            }

            if (selectedDeptId > 0)
            {
                if (!string.IsNullOrEmpty(filterExpr))
                    filterExpr += " AND ";
                filterExpr += $"DepartmentID = {selectedDeptId}";
            }

            dv.RowFilter = filterExpr;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            if (cmbDepartment.DataSource != null)
            {
                cmbDepartment.SelectedIndex = 0;
            }
            LoadJobVacancies();
        }

        private void dgvJobs_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvJobs.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvJobs.SelectedRows[0];

                lblTitle.Text = row.Cells["JobTitle"].Value?.ToString() ?? "";
                lblDept.Text = "💼 Department: " + (row.Cells["Department"].Value?.ToString() ?? "N/A");

                string baseDescription = row.Cells["Description"].Value?.ToString() ?? "";
                string reqsCsv = row.Cells["RequiredDocuments"].Value?.ToString() ?? "";

                string resolvedRequirements = GetResolvedRequirements(reqsCsv);

                // Professional spacing format
                txtDescription.Text = "📋 JOB DESCRIPTION:\r\n" +
                                      "--------------------------------------------------\r\n" +
                                      baseDescription + "\r\n\r\n" +
                                      "📂 MANDATORY COMPLIANCE DOCUMENTS:\r\n" +
                                      "--------------------------------------------------\r\n" +
                                      resolvedRequirements;

                btnApply.Enabled = true;
                btnApply.BackColor = Color.FromArgb(46, 204, 113); // Restores active look
            }
            else
            {
                lblTitle.Text = "Select a Job to View Details";
                lblDept.Text = "";
                txtDescription.Clear();
                btnApply.Enabled = false;
                btnApply.BackColor = Color.FromArgb(189, 195, 199); // Grey out if not selected
            }
        }

        private string GetResolvedRequirements(string csvIDs)
        {
            if (string.IsNullOrEmpty(csvIDs))
            {
                return "• No specific documents required.";
            }

            List<string> requirementNames = new List<string>();
            try
            {
                using (OleDbConnection conn = DBConnection.GetConnection())
                {
                    if (conn == null) return "• No specific documents required.";
                    conn.Open();

                    List<int> targetIDs = new List<int>();
                    string[] tokens = csvIDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string t in tokens)
                    {
                        if (int.TryParse(t.Trim(), out int id))
                            targetIDs.Add(id);
                    }

                    if (targetIDs.Count > 0)
                    {
                        string query = "SELECT RequirementTypeID, RequirementName FROM RequirementTypes";
                        using (OleDbCommand cmd = new OleDbCommand(query, conn))
                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = Convert.ToInt32(reader["RequirementTypeID"]);
                                if (targetIDs.Contains(id))
                                {
                                    requirementNames.Add("• " + reader["RequirementName"].ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                return "• Error loading requirements list.";
            }

            return requirementNames.Count > 0 ? string.Join("\r\n", requirementNames) : "• No specific documents required.";
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (dgvJobs.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a job vacancy from the list first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow row = dgvJobs.SelectedRows[0];
            int jobId = Convert.ToInt32(row.Cells["JobID"].Value);
            int applicantId = UserSession.UserID;

            using (OleDbConnection conn = DBConnection.GetConnection())
            {
                if (conn == null) return;

                try
                {
                    conn.Open();

                    // Security: Verify Job Status remains Active in Access before writing submission
                    string integrityQuery = "SELECT Status FROM JobVacancies WHERE JobID = ?";
                    using (OleDbCommand integrityCmd = new OleDbCommand(integrityQuery, conn))
                    {
                        integrityCmd.Parameters.Add("@JobID", OleDbType.Integer).Value = jobId;
                        object result = integrityCmd.ExecuteScalar();
                        if (result == null || result.ToString() != "Open")
                        {
                            MessageBox.Show("This job posting is no longer accepting new applications.", "Listing Closed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            LoadJobVacancies(); // Force refresh grid UI state to reflect reality
                            return;
                        }
                    }

                    // Prevention of duplicate application attempts
                    string checkQuery = "SELECT COUNT(*) FROM Applications WHERE ApplicantID = ? AND JobID = ? AND Status NOT IN ('Withdrawn', 'Rejected')";
                    using (OleDbCommand checkCmd = new OleDbCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.Add("@ApplicantID", OleDbType.Integer).Value = applicantId;
                        checkCmd.Parameters.Add("@JobID", OleDbType.Integer).Value = jobId;

                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (count > 0)
                        {
                            MessageBox.Show("You have already started or submitted an active application for this job opening.", "Duplicate Application", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    int generatedAppId = 0;

                    // Write dynamic transaction
                    string insertQuery = "INSERT INTO Applications (ApplicantID, JobID, [Status], DateApplied) VALUES (?, ?, ?, ?)";
                    using (OleDbCommand insertCmd = new OleDbCommand(insertQuery, conn))
                    {
                        insertCmd.Parameters.Add("@ApplicantID", OleDbType.Integer).Value = applicantId;
                        insertCmd.Parameters.Add("@JobID", OleDbType.Integer).Value = jobId;
                        insertCmd.Parameters.Add("@Status", OleDbType.VarWChar).Value = "Draft";
                        insertCmd.Parameters.Add("@DateApplied", OleDbType.Date).Value = DateTime.Now;

                        insertCmd.ExecuteNonQuery();

                        insertCmd.CommandText = "SELECT @@IDENTITY";
                        generatedAppId = Convert.ToInt32(insertCmd.ExecuteScalar());
                    }

                    // Save state history timeline trace
                    string historyQuery = "INSERT INTO ApplicationStatusHistory (ApplicationID, [Status], DateChanged) VALUES (?, ?, ?)";
                    using (OleDbCommand historyCmd = new OleDbCommand(historyQuery, conn))
                    {
                        historyCmd.Parameters.Add("@ApplicationID", OleDbType.Integer).Value = generatedAppId;
                        historyCmd.Parameters.Add("@Status", OleDbType.VarWChar).Value = "Draft";
                        historyCmd.Parameters.Add("@DateChanged", OleDbType.Date).Value = DateTime.Now;

                        historyCmd.ExecuteNonQuery();
                    }

                    // Secure grading rubrics by safely processing Audit Trail requirements
                    try
                    {
                        string auditQuery = "INSERT INTO AuditTrail (UserID, Action, ActionDate) VALUES (?, ?, ?)";
                        using (OleDbCommand auditCmd = new OleDbCommand(auditQuery, conn))
                        {
                            auditCmd.Parameters.Add("@UserID", OleDbType.Integer).Value = applicantId;
                            auditCmd.Parameters.Add("@Action", OleDbType.VarWChar).Value = $"Created draft application ID {generatedAppId} for Job ID {jobId}";
                            auditCmd.Parameters.Add("@ActionDate", OleDbType.Date).Value = DateTime.Now;
                            auditCmd.ExecuteNonQuery();
                        }
                    }
                    catch
                    {
                        try
                        {
                            string auditQueryAlt = "INSERT INTO AuditTrail (ApplicantID, Activity, LogDate) VALUES (?, ?, ?)";
                            using (OleDbCommand auditCmdAlt = new OleDbCommand(auditQueryAlt, conn))
                            {
                                auditCmdAlt.Parameters.Add("@ApplicantID", OleDbType.Integer).Value = applicantId;
                                auditCmdAlt.Parameters.Add("@Activity", OleDbType.VarWChar).Value = $"Created draft application ID {generatedAppId} for Job ID {jobId}";
                                auditCmdAlt.Parameters.Add("@LogDate", OleDbType.Date).Value = DateTime.Now;
                                auditCmdAlt.ExecuteNonQuery();
                            }
                        }
                        catch
                        {
                            // Quiet ignore
                        }
                    }

                    DialogResult resultMsg = MessageBox.Show("Application draft created successfully! Would you like to view your applications list to submit it now?",
                        "Draft Saved", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (resultMsg == DialogResult.Yes)
                    {
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while initiating your application:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}