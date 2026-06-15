using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;
using HRApplicantSystem.Classes;
using HRApplicantSystem.Database;

namespace HRApplicantSystem.Forms.HR
{
    public partial class JobVacancyManagementForm : Form
    {
        private int selectedJobID = -1;

        // Corporate Modern Palette 
        private readonly Color PrimaryHeaderColor = Color.FromArgb(15, 23, 42);   // Slate-900 (Dark Slate)
        private readonly Color AppBackgroundColor = Color.FromArgb(241, 245, 249); // Slate-100 (Warm gray)
        private readonly Color BorderColor = Color.FromArgb(226, 232, 240);       // Slate-200
        private readonly Color TextMainColor = Color.FromArgb(51, 65, 85);         // Slate-700
        private readonly Color TextMutedColor = Color.FromArgb(100, 116, 139);     // Slate-500

        // Semantic Action Colors
        private readonly Color IndigoAccent = Color.FromArgb(79, 70, 229);        // Indigo-600 (Publish/Save)
        private readonly Color EmeraldSuccess = Color.FromArgb(5, 150, 105);      // Emerald-600 (Open Status / Reopen)
        private readonly Color CrimsonDanger = Color.FromArgb(225, 29, 72);       // Rose-600 (Close Status)
        private readonly Color SlateNeutral = Color.FromArgb(71, 85, 105);         // Slate-600 (Clear)

        public JobVacancyManagementForm()
        {
            InitializeComponent();
            ApplyEnterpriseStyles();
        }

        private void ApplyEnterpriseStyles()
        {
            this.BackColor = AppBackgroundColor;

            // Form Headers
            pnlHeader.BackColor = PrimaryHeaderColor;
            lblTitle.ForeColor = Color.White;
            lblTitle.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblSubtitle.ForeColor = Color.FromArgb(148, 163, 184); // Slate-400
            lblSubtitle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);

            // Left Panel (Grid Container)
            pnlLeft.BackColor = Color.White;
            pnlLeft.BorderStyle = BorderStyle.None;
            lblSearchLabel.ForeColor = TextMainColor;
            lblSearchLabel.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);

            // Right Panel (Editor Form)
            pnlEditor.BackColor = Color.White;
            pnlEditor.BorderStyle = BorderStyle.None;
            pnlEditorHeader.BackColor = Color.FromArgb(248, 250, 252); // Slate-50

            // Text / Label Styling
            lblEditorTitle.ForeColor = PrimaryHeaderColor;
            lblEditorTitle.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);

            ConfigureLabel(lblJobTitle);
            ConfigureLabel(lblDepartment);
            ConfigureLabel(lblEmploymentType); // Style new label control
            ConfigureLabel(lblDescription);
            ConfigureLabel(lblRequirements);
            ConfigureLabel(lblStatusText);

            // Inputs
            cmbJobTitle.FlatStyle = FlatStyle.Flat;
            cmbDepartment.FlatStyle = FlatStyle.Flat;
            cmbEmploymentType.FlatStyle = FlatStyle.Flat; // Style new combobox control
            txtDescription.BorderStyle = BorderStyle.FixedSingle;
            clbRequirements.BorderStyle = BorderStyle.FixedSingle;

            // Badges & Labels
            lblStatusValue.Font = new Font("Segoe UI Black", 10F, FontStyle.Bold);

            // Style Flat Buttons
            StyleButton(btnSave, IndigoAccent);
            StyleButton(btnToggleStatus, EmeraldSuccess);
            StyleButton(btnClear, SlateNeutral);
            StyleButton(btnBack, SlateNeutral); // Styled the back button using SlateNeutral

            // Enterprise Grid Custom Styling
            dgvVacancies.BackgroundColor = Color.White;
            dgvVacancies.BorderStyle = BorderStyle.None;
            dgvVacancies.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvVacancies.GridColor = BorderColor;
            dgvVacancies.RowHeadersVisible = false;
            dgvVacancies.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVacancies.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252); // Slate-50
            dgvVacancies.DefaultCellStyle.SelectionBackColor = Color.FromArgb(238, 242, 255);      // Indigo-50
            dgvVacancies.DefaultCellStyle.SelectionForeColor = Color.FromArgb(67, 56, 202);        // Indigo-700
            dgvVacancies.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            dgvVacancies.DefaultCellStyle.ForeColor = TextMainColor;

            dgvVacancies.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            dgvVacancies.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(241, 245, 249);  // Slate-100
            dgvVacancies.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(15, 23, 42);     // Slate-900
            dgvVacancies.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(241, 245, 249);
            dgvVacancies.EnableHeadersVisualStyles = false;
        }

        private void ConfigureLabel(Label lbl)
        {
            lbl.ForeColor = TextMainColor;
            lbl.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
        }

        private void StyleButton(Button btn, Color baseColor)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.BackColor = baseColor;
            btn.ForeColor = Color.White;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = ControlPaint.Light(baseColor, 0.15f);
            btn.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
        }

        private void JobVacancyManagementForm_Load(object sender, EventArgs e)
        {
            // 1. Role-Based Access Check
            if (UserSession.Role != "Admin" && UserSession.Role != "HR Manager")
            {
                MessageBox.Show("Access Denied. Only HR Managers and Admins are permitted to manage job vacancies.",
                                "Authorization Restriction", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.Close();
                return;
            }

            // Centering Fix: Snap form coordinates precisely over the main dashboard
            CenterFormOnDashboard();

            // 2. Load Lookups and Vacancies
            LoadDropdownMetadata();
            LoadRequirementTypes();
            LoadVacancies();
            ResetForm();
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

        #region Database Processing Logic

        private void LoadDropdownMetadata()
        {
            using (OleDbConnection con = DBConnection.GetConnection())
            {
                if (con == null) return;
                try
                {
                    con.Open();

                    // Retrieve Position mapping
                    string posQuery = "SELECT PositionID, PositionName FROM Positions WHERE IsActive = True ORDER BY PositionName ASC";
                    using (OleDbDataAdapter da = new OleDbDataAdapter(posQuery, con))
                    {
                        DataTable dtPos = new DataTable();
                        da.Fill(dtPos);
                        cmbJobTitle.DataSource = dtPos;
                        cmbJobTitle.DisplayMember = "PositionName";
                        cmbJobTitle.ValueMember = "PositionID";
                    }

                    // Retrieve Department mapping
                    string deptQuery = "SELECT DepartmentID, DepartmentName FROM Departments WHERE IsActive = True ORDER BY DepartmentName ASC";
                    using (OleDbDataAdapter da = new OleDbDataAdapter(deptQuery, con))
                    {
                        DataTable dtDept = new DataTable();
                        da.Fill(dtDept);
                        cmbDepartment.DataSource = dtDept;
                        cmbDepartment.DisplayMember = "DepartmentName";
                        cmbDepartment.ValueMember = "DepartmentID";
                    }

                    // Retrieve Employment Type mapping
                    string empQuery = "SELECT EmploymentTypeID, TypeName FROM EmploymentTypes WHERE IsActive = True ORDER BY TypeName ASC";
                    using (OleDbDataAdapter da = new OleDbDataAdapter(empQuery, con))
                    {
                        DataTable dtEmp = new DataTable();
                        da.Fill(dtEmp);
                        cmbEmploymentType.DataSource = dtEmp;
                        cmbEmploymentType.DisplayMember = "TypeName";
                        cmbEmploymentType.ValueMember = "EmploymentTypeID";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load reference items: " + ex.Message, "Metadata Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadRequirementTypes()
        {
            using (OleDbConnection con = DBConnection.GetConnection())
            {
                if (con == null) return;
                try
                {
                    con.Open();
                    string query = "SELECT RequirementTypeID, RequirementName FROM RequirementTypes ORDER BY RequirementName ASC";
                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        clbRequirements.Items.Clear();
                        while (reader.Read())
                        {
                            clbRequirements.Items.Add(new RequirementItem
                            {
                                ID = Convert.ToInt32(reader["RequirementTypeID"]),
                                Name = reader["RequirementName"].ToString()
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load checklist definitions: " + ex.Message, "Metadata Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadVacancies(string searchKeyword = "")
        {
            using (OleDbConnection con = DBConnection.GetConnection())
            {
                if (con == null) return;
                try
                {
                    con.Open();

                    // Access SQL nested join syntax incorporating EmploymentTypes table
                    string query = "SELECT j.JobID, p.PositionName AS JobTitle, d.DepartmentName AS Department, " +
                                   "et.TypeName AS EmploymentType, j.Description, j.Status, j.RequiredDocuments, " +
                                   "j.PositionID, j.DepartmentID, j.EmploymentTypeID " +
                                   "FROM ((JobVacancies j " +
                                   "INNER JOIN Positions p ON j.PositionID = p.PositionID) " +
                                   "INNER JOIN Departments d ON j.DepartmentID = d.DepartmentID) " +
                                   "LEFT JOIN EmploymentTypes et ON j.EmploymentTypeID = et.EmploymentTypeID";

                    if (!string.IsNullOrEmpty(searchKeyword))
                    {
                        query += " WHERE p.PositionName LIKE ? OR d.DepartmentName LIKE ? OR et.TypeName LIKE ?";
                    }
                    query += " ORDER BY j.JobID DESC";

                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    {
                        if (!string.IsNullOrEmpty(searchKeyword))
                        {
                            cmd.Parameters.AddWithValue("?", "%" + searchKeyword + "%");
                            cmd.Parameters.AddWithValue("?", "%" + searchKeyword + "%");
                            cmd.Parameters.AddWithValue("?", "%" + searchKeyword + "%");
                        }

                        using (OleDbDataAdapter da = new OleDbDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dgvVacancies.DataSource = dt;

                            if (dgvVacancies.Columns.Count > 0)
                            {
                                // Remove ID column visually, keeping it in bound memory
                                dgvVacancies.Columns["JobID"].Visible = false;

                                dgvVacancies.Columns["JobTitle"].HeaderText = "POSITION OPEN TITLE";
                                dgvVacancies.Columns["JobTitle"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                                dgvVacancies.Columns["Department"].HeaderText = "DEPARTMENT BLOCK";
                                dgvVacancies.Columns["Department"].Width = 130;

                                // Added Grid Column configuration for EmploymentType
                                dgvVacancies.Columns["EmploymentType"].HeaderText = "EMPLOYMENT TYPE";
                                dgvVacancies.Columns["EmploymentType"].Width = 130;

                                dgvVacancies.Columns["Status"].HeaderText = "STATUS";
                                dgvVacancies.Columns["Status"].Width = 85;

                                // Hide functional metadata columns
                                dgvVacancies.Columns["Description"].Visible = false;
                                dgvVacancies.Columns["RequiredDocuments"].Visible = false;
                                dgvVacancies.Columns["PositionID"].Visible = false;
                                dgvVacancies.Columns["DepartmentID"].Visible = false;
                                dgvVacancies.Columns["EmploymentTypeID"].Visible = false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error refreshing vacancy lists: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion

        #region Form Interactions

        private void dgvVacancies_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvVacancies.Rows[e.RowIndex];
                selectedJobID = Convert.ToInt32(row.Cells["JobID"].Value);

                cmbJobTitle.SelectedValue = Convert.ToInt32(row.Cells["PositionID"].Value);
                cmbDepartment.SelectedValue = Convert.ToInt32(row.Cells["DepartmentID"].Value);
                txtDescription.Text = row.Cells["Description"].Value.ToString();

                // Select current Employment Type mapping safely
                if (row.Cells["EmploymentTypeID"].Value != DBNull.Value)
                {
                    cmbEmploymentType.SelectedValue = Convert.ToInt32(row.Cells["EmploymentTypeID"].Value);
                }
                else
                {
                    cmbEmploymentType.SelectedIndex = -1;
                }

                string status = row.Cells["Status"].Value.ToString();
                lblStatusValue.Text = status.ToUpper();

                if (status == "Open")
                {
                    lblStatusValue.ForeColor = EmeraldSuccess;
                    btnToggleStatus.Text = "Close Vacancy";
                    btnToggleStatus.BackColor = CrimsonDanger;
                }
                else
                {
                    lblStatusValue.ForeColor = CrimsonDanger;
                    btnToggleStatus.Text = "Reopen Vacancy";
                    btnToggleStatus.BackColor = EmeraldSuccess;
                }

                // Parse document requirement checkboxes
                string reqsString = row.Cells["RequiredDocuments"].Value.ToString();
                List<string> reqList = new List<string>(reqsString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));

                for (int i = 0; i < clbRequirements.Items.Count; i++)
                {
                    RequirementItem item = (RequirementItem)clbRequirements.Items[i];
                    clbRequirements.SetItemChecked(i, reqList.Contains(item.ID.ToString()));
                }

                btnSave.Text = "Update Vacancy";
                btnSave.BackColor = IndigoAccent;
                btnToggleStatus.Enabled = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbJobTitle.SelectedValue == null || cmbDepartment.SelectedValue == null || cmbEmploymentType.SelectedValue == null)
            {
                MessageBox.Show("Please select a Position Title, Department, and Employment Type mapping.", "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("Job description or qualification summaries cannot be left empty.", "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Map checkbox items into a parsed string format
            List<string> selectedReqs = new List<string>();
            foreach (object item in clbRequirements.CheckedItems)
            {
                RequirementItem req = (RequirementItem)item;
                selectedReqs.Add(req.ID.ToString());
            }
            string reqsJoined = string.Join(",", selectedReqs);

            using (OleDbConnection con = DBConnection.GetConnection())
            {
                if (con == null) return;
                try
                {
                    con.Open();
                    string query;

                    // Updated insert / update SQL to persist EmploymentTypeID mapping parameter
                    if (selectedJobID == -1)
                    {
                        query = "INSERT INTO JobVacancies (PositionID, DepartmentID, EmploymentTypeID, Description, [Status], RequiredDocuments) VALUES (?, ?, ?, ?, 'Open', ?)";
                    }
                    else
                    {
                        query = "UPDATE JobVacancies SET PositionID = ?, DepartmentID = ?, EmploymentTypeID = ?, Description = ?, RequiredDocuments = ? WHERE JobID = ?";
                    }

                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("?", Convert.ToInt32(cmbJobTitle.SelectedValue));
                        cmd.Parameters.AddWithValue("?", Convert.ToInt32(cmbDepartment.SelectedValue));
                        cmd.Parameters.AddWithValue("?", Convert.ToInt32(cmbEmploymentType.SelectedValue));
                        cmd.Parameters.AddWithValue("?", txtDescription.Text.Trim());
                        cmd.Parameters.AddWithValue("?", reqsJoined);

                        if (selectedJobID != -1)
                        {
                            cmd.Parameters.AddWithValue("?", selectedJobID);
                        }

                        cmd.ExecuteNonQuery();
                    }

                    string logMsg = (selectedJobID == -1)
                        ? $"Created new open job posting: {cmbJobTitle.Text}"
                        : $"Modified properties for Job ID {selectedJobID} ({cmbJobTitle.Text})";
                    LogAuditAction(logMsg);

                    MessageBox.Show("The vacancy details have been successfully saved and published.", "Database Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadVacancies();
                    ResetForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to save changes: " + ex.Message, "Database Transaction Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnToggleStatus_Click(object sender, EventArgs e)
        {
            if (selectedJobID == -1) return;

            string currentStatus = lblStatusValue.Text.Trim();
            string newStatus = (currentStatus == "OPEN") ? "Closed" : "Open";

            DialogResult result = MessageBox.Show($"Are you sure you want to change the status of this vacancy to '{newStatus}'?\n" +
                                                  "This determines whether applicants can view and submit new portfolios.",
                                                  "Confirm State Change", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            using (OleDbConnection con = DBConnection.GetConnection())
            {
                if (con == null) return;
                try
                {
                    con.Open();
                    string query = "UPDATE JobVacancies SET [Status] = ? WHERE JobID = ?";
                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("?", newStatus);
                        cmd.Parameters.AddWithValue("?", selectedJobID);
                        cmd.ExecuteNonQuery();
                    }

                    LogAuditAction($"Updated status of Job ID {selectedJobID} from {currentStatus} to {newStatus}");

                    MessageBox.Show($"The job vacancy is now set to: {newStatus}.", "Status Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadVacancies();
                    ResetForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to update status values: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadVacancies(txtSearch.Text.Trim());
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ResetForm()
        {
            selectedJobID = -1;
            cmbJobTitle.SelectedIndex = -1;
            cmbDepartment.SelectedIndex = -1;
            if (cmbEmploymentType != null) cmbEmploymentType.SelectedIndex = -1; // Added new control reset
            txtDescription.Clear();

            lblStatusValue.Text = "NEW DRAFT";
            lblStatusValue.ForeColor = TextMutedColor;

            for (int i = 0; i < clbRequirements.Items.Count; i++)
            {
                clbRequirements.SetItemChecked(i, false);
            }

            btnSave.Text = "Publish Job";
            btnSave.BackColor = IndigoAccent;

            btnToggleStatus.Text = "Toggle Status";
            btnToggleStatus.BackColor = SlateNeutral;
            btnToggleStatus.Enabled = false;
        }

        private void LogAuditAction(string actionText)
        {
            using (OleDbConnection con = DBConnection.GetConnection())
            {
                if (con == null) return;
                try
                {
                    con.Open();
                    // Fixed: Changed ActionPerformed and ActionTimestamp to [Action] and DateCreated
                    string query = "INSERT INTO AuditTrail (UserID, [Action], DateCreated) VALUES (?, ?, ?)";
                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("?", UserSession.UserID > 0 ? UserSession.UserID : 1);
                        cmd.Parameters.AddWithValue("?", actionText);
                        cmd.Parameters.AddWithValue("?", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("LogAuditAction Error: " + ex.Message);
                }
            }
        }

        #endregion

        private class RequirementItem
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public override string ToString() => Name;
        }
    }
}