using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;
using HRApplicantSystem.Database;
using HRApplicantSystem.Classes;

namespace HRApplicantSystem.Forms.HR
{
    public partial class MaintenanceForm : Form
    {
        // Selected record IDs (0 indicates New / Insert Mode)
        private int selectedDeptId = 0;
        private int selectedPosId = 0;
        private int selectedEmpId = 0;
        private int selectedReqId = 0;
        private int selectedIntId = 0;
        private int selectedAssId = 0;
        private int selectedUserId = 0; // Tracks selected account in Tab 7

        // Programmatically generated User Management Controls to resolve compiler CS0103 issues
        private TabControl mainTabControl;
        private TabPage tabUsers;
        private DataGridView dgvUsers;
        private TextBox txtUserUsername;
        private TextBox txtUserFullName;
        private TextBox txtUserPassword;
        private ComboBox cmbUserRole;
        private Button btnSaveUser;
        private Button btnClearUser;
        private Label lblPasswordHelp;
        private TextBox txtSearchUsers;

        // Suspends selection logic during reloads and clearing states to prevent UI feedback loops
        private bool isResetting = false;

        public MaintenanceForm()
        {
            InitializeComponent();
        }

        private void MaintenanceForm_Load(object sender, EventArgs e)
        {
            // Security verification
            string userRole = UserSession.Role;
            if (userRole != "Admin" && userRole != "HR Manager")
            {
                MessageBox.Show("Access Denied. Only Admins and HR Managers can access system maintenance settings.",
                                "Security Exception", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.BeginInvoke(new MethodInvoker(this.Close));
                return;
            }

            // Programmatically construct Tab 7 (User Management UI) dynamically
            InitializeUserManagementTab();

            // Apply modern UI Styling dynamically on Load
            ApplyThemeStyles();

            LoadAllData();
        }

        /// <summary>
        /// Programmatic layout manager that builds the user account interface dynamically.
        /// </summary>
        private void InitializeUserManagementTab()
        {
            // Search controls to locate existing TabControl on form
            mainTabControl = FindTabControl(this);

            if (mainTabControl == null) return;

            // Instantiation and sizing of Tab 7
            tabUsers = new TabPage("User Accounts");
            tabUsers.BackColor = Color.White;
            mainTabControl.TabPages.Add(tabUsers);

            // Left Side Panel: Inputs
            Panel pnlInput = new Panel
            {
                Location = new Point(15, 15),
                Size = new Size(290, tabUsers.Height - 30),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left
            };
            tabUsers.Controls.Add(pnlInput);

            int yOffset = 10;

            Label lblUser = new Label { Text = "Username:", Location = new Point(5, yOffset), AutoSize = true, Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold), ForeColor = UITheme.ColorSecondary };
            pnlInput.Controls.Add(lblUser);
            yOffset += 20;

            txtUserUsername = new TextBox { Location = new Point(5, yOffset), Size = new Size(270, 25), Font = new Font("Segoe UI", 9F) };
            pnlInput.Controls.Add(txtUserUsername);
            yOffset += 35;

            Label lblFullName = new Label { Text = "Full Name:", Location = new Point(5, yOffset), AutoSize = true, Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold), ForeColor = UITheme.ColorSecondary };
            pnlInput.Controls.Add(lblFullName);
            yOffset += 20;

            txtUserFullName = new TextBox { Location = new Point(5, yOffset), Size = new Size(270, 25), Font = new Font("Segoe UI", 9F) };
            pnlInput.Controls.Add(txtUserFullName);
            yOffset += 35;

            Label lblPassword = new Label { Text = "Password:", Location = new Point(5, yOffset), AutoSize = true, Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold), ForeColor = UITheme.ColorSecondary };
            pnlInput.Controls.Add(lblPassword);
            yOffset += 20;

            txtUserPassword = new TextBox { Location = new Point(5, yOffset), Size = new Size(270, 25), PasswordChar = '•', Font = new Font("Segoe UI", 9F) };
            pnlInput.Controls.Add(txtUserPassword);
            yOffset += 30;

            lblPasswordHelp = new Label { Text = "(Leave blank during updates to keep current password)", Location = new Point(5, yOffset), AutoSize = true, ForeColor = Color.DimGray, Font = new Font("Segoe UI", 8F, FontStyle.Italic) };
            pnlInput.Controls.Add(lblPasswordHelp);
            yOffset += 20;

            Label lblRole = new Label { Text = "System Role:", Location = new Point(5, yOffset), AutoSize = true, Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold), ForeColor = UITheme.ColorSecondary };
            pnlInput.Controls.Add(lblRole);
            yOffset += 20;

            cmbUserRole = new ComboBox { Location = new Point(5, yOffset), Size = new Size(270, 25), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9F) };
            cmbUserRole.Items.AddRange(new object[] { "Admin", "HR Manager", "HR Staff" });
            if (cmbUserRole.Items.Count > 0) cmbUserRole.SelectedIndex = 0;
            pnlInput.Controls.Add(cmbUserRole);
            yOffset += 40;

            btnSaveUser = new Button { Text = "Save", Location = new Point(5, yOffset), Size = new Size(130, 32) };
            btnSaveUser.Click += btnSaveUser_Click;
            pnlInput.Controls.Add(btnSaveUser);

            btnClearUser = new Button { Text = "Clear", Location = new Point(145, yOffset), Size = new Size(130, 32) };
            btnClearUser.Click += btnClearUser_Click;
            pnlInput.Controls.Add(btnClearUser);

            // Right Side Panel: Grid & Filters
            Panel pnlGrid = new Panel
            {
                Location = new Point(320, 15),
                Size = new Size(tabUsers.Width - 335, tabUsers.Height - 30),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            tabUsers.Controls.Add(pnlGrid);

            Label lblSearch = new Label { Text = "Search User Accounts:", Location = new Point(5, 10), AutoSize = true, Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold), ForeColor = UITheme.ColorSecondary };
            pnlGrid.Controls.Add(lblSearch);

            txtSearchUsers = new TextBox { Location = new Point(160, 7), Size = new Size(220, 25), Font = new Font("Segoe UI", 9F) };
            txtSearchUsers.TextChanged += txtSearch_TextChanged;
            pnlGrid.Controls.Add(txtSearchUsers);

            dgvUsers = new DataGridView
            {
                Location = new Point(5, 40),
                Size = new Size(pnlGrid.Width - 10, pnlGrid.Height - 50),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AllowUserToAddRows = false,
                ReadOnly = true
            };
            dgvUsers.SelectionChanged += dgvUsers_SelectionChanged;
            pnlGrid.Controls.Add(dgvUsers);

            // Map search textbox tag directly to its datagrid to inherit base-class filtering automatically
            txtSearchUsers.Tag = dgvUsers;
        }

        /// <summary>
        /// Recursively navigates nested layouts to find parent tab containers automatically.
        /// </summary>
        private TabControl FindTabControl(Control parent)
        {
            foreach (Control child in parent.Controls)
            {
                if (child is TabControl tc) return tc;
                TabControl subTc = FindTabControl(child);
                if (subTc != null) return subTc;
            }
            return null;
        }

        /// <summary>
        /// Programs aesthetic formatting across form controls on startup.
        /// </summary>
        private void ApplyThemeStyles()
        {
            this.BackColor = UITheme.ColorBg;

            // Standard grids styling
            UITheme.StyleDataGridView(dgvDepartments);
            UITheme.StyleDataGridView(dgvPositions);
            UITheme.StyleDataGridView(dgvEmployment);
            UITheme.StyleDataGridView(dgvRequirements);
            UITheme.StyleDataGridView(dgvInterviews);
            UITheme.StyleDataGridView(dgvAssessments);

            // Dynamic User Grid styling
            if (dgvUsers != null) UITheme.StyleDataGridView(dgvUsers);

            // Form element actions styling
            UITheme.StylePrimaryButton(btnSaveDept);
            UITheme.StyleSecondaryButton(btnClearDept);

            UITheme.StylePrimaryButton(btnSavePos);
            UITheme.StyleSecondaryButton(btnClearPos);

            UITheme.StylePrimaryButton(btnSaveEmp);
            UITheme.StyleSecondaryButton(btnClearEmp);

            UITheme.StylePrimaryButton(btnSaveReq);
            UITheme.StyleSecondaryButton(btnClearReq);

            UITheme.StylePrimaryButton(btnSaveInt);
            UITheme.StyleSecondaryButton(btnClearInt);

            UITheme.StylePrimaryButton(btnSaveAss);
            UITheme.StyleSecondaryButton(btnClearAss);

            // Dynamic User action styling
            if (btnSaveUser != null) UITheme.StylePrimaryButton(btnSaveUser);
            if (btnClearUser != null) UITheme.StyleSecondaryButton(btnClearUser);
        }

        private void LoadAllData()
        {
            LoadDepartments();
            LoadPositions();
            LoadActiveDepartmentsDropdown();
            LoadEmploymentTypes();
            LoadRequirementTypes();
            LoadInterviewTypes();
            LoadAssessmentTypes();
            LoadUserAccounts(); // Auto-load users
        }

        // --- Database Helper Methods ---
        private void ExecuteDataFill(string query, DataGridView dgv)
        {
            using (OleDbConnection con = DBConnection.GetConnection())
            {
                if (con == null) return;
                try
                {
                    con.Open();
                    using (OleDbDataAdapter da = new OleDbDataAdapter(query, con))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        // Use BindingSource to safely support programmatic searching/filtering
                        BindingSource bs = new BindingSource { DataSource = dt };
                        dgv.DataSource = bs;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database Load Error: " + ex.Message, "DB Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool IsDuplicateValue(string tableName, string columnName, string value, string idColumn, int currentId, string additionalCondition = "")
        {
            using (OleDbConnection con = DBConnection.GetConnection())
            {
                if (con == null) return false;
                try
                {
                    con.Open();
                    string query = $"SELECT COUNT(*) FROM [{tableName}] WHERE LCase([{columnName}]) = ? AND [{idColumn}] <> ?";
                    if (!string.IsNullOrEmpty(additionalCondition))
                    {
                        query += " AND " + additionalCondition;
                    }

                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("?", value.Trim().ToLower());
                        cmd.Parameters.AddWithValue("?", currentId);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }

        private int GetReferenceCount(string tableName, string foreignKeyColumn, int foreignKeyValue, string additionalWhere = "")
        {
            using (OleDbConnection con = DBConnection.GetConnection())
            {
                if (con == null) return 0;
                try
                {
                    con.Open();
                    string query = $"SELECT COUNT(*) FROM [{tableName}] WHERE [{foreignKeyColumn}] = ?";
                    if (!string.IsNullOrEmpty(additionalWhere))
                    {
                        query += " AND " + additionalWhere;
                    }

                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("?", foreignKeyValue);
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
                catch
                {
                    // Fallback to 0 if table or column does not exist, keeping execution safe
                    return 0;
                }
            }
        }

        private void LogActivity(string action, string details)
        {
            string query = "INSERT INTO AuditTrail (UserID, [Action], ActionTimestamp, Details) VALUES (?, ?, ?, ?)";
            using (OleDbConnection con = DBConnection.GetConnection())
            {
                if (con == null) return;
                try
                {
                    con.Open();
                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("?", UserSession.UserID);
                        cmd.Parameters.AddWithValue("?", action);
                        cmd.Parameters.AddWithValue("?", DateTime.Now);
                        cmd.Parameters.AddWithValue("?", details);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch
                {
                    // Fail silently so auditing issues do not interrupt configurations
                }
            }
        }

        // ==========================================
        // TAB 1: DEPARTMENTS LOGIC
        // ==========================================
        private void LoadDepartments()
        {
            string query = "SELECT DepartmentID, DepartmentName, Description, IsActive FROM Departments";
            ExecuteDataFill(query, dgvDepartments);
        }

        private void dgvDepartments_SelectionChanged(object sender, EventArgs e)
        {
            if (isResetting) return;

            if (dgvDepartments.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvDepartments.SelectedRows[0];
                if (row.Cells["DepartmentID"].Value == null || row.Cells["DepartmentID"].Value == DBNull.Value) return;

                selectedDeptId = Convert.ToInt32(row.Cells["DepartmentID"].Value);
                txtDeptName.Text = Convert.ToString(row.Cells["DepartmentName"].Value);
                txtDeptDesc.Text = Convert.ToString(row.Cells["Description"].Value);
                chkDeptActive.Checked = Convert.ToBoolean(row.Cells["IsActive"].Value);
                btnSaveDept.Text = "Update";
            }
        }

        private void btnClearDept_Click(object sender, EventArgs e)
        {
            isResetting = true;
            try
            {
                selectedDeptId = 0;
                txtDeptName.Clear();
                txtDeptDesc.Clear();
                chkDeptActive.Checked = true;
                btnSaveDept.Text = "Save";
                dgvDepartments.ClearSelection();
            }
            finally
            {
                isResetting = false;
            }
        }

        private void btnSaveDept_Click(object sender, EventArgs e)
        {
            string deptName = txtDeptName.Text.Trim();
            if (string.IsNullOrWhiteSpace(deptName))
            {
                MessageBox.Show("Please enter a Department Name.", "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (IsDuplicateValue("Departments", "DepartmentName", deptName, "DepartmentID", selectedDeptId))
            {
                MessageBox.Show("A department with this name already exists.", "Duplicate Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!chkDeptActive.Checked && selectedDeptId > 0)
            {
                int linkedPositions = GetReferenceCount("Positions", "DepartmentID", selectedDeptId, "IsActive = True");
                if (linkedPositions > 0)
                {
                    DialogResult result = MessageBox.Show(
                        $"Warning: There are {linkedPositions} active positions associated with this department. " +
                        "Deactivating this department will make its positions unavailable for new vacancies. Proceed?",
                        "Referential Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.No) return;
                }
            }

            string query = selectedDeptId == 0
                ? "INSERT INTO Departments (DepartmentName, Description, IsActive) VALUES (?, ?, ?)"
                : "UPDATE Departments SET DepartmentName = ?, Description = ?, IsActive = ? WHERE DepartmentID = ?";

            using (OleDbConnection con = DBConnection.GetConnection())
            {
                if (con == null) return;
                try
                {
                    con.Open();
                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("?", deptName);
                        cmd.Parameters.AddWithValue("?", txtDeptDesc.Text.Trim());
                        cmd.Parameters.AddWithValue("?", chkDeptActive.Checked);

                        if (selectedDeptId > 0)
                            cmd.Parameters.AddWithValue("?", selectedDeptId);

                        cmd.ExecuteNonQuery();
                    }

                    string logAction = selectedDeptId == 0 ? "Create Department" : "Update Department";
                    LogActivity(logAction, $"Department '{deptName}' saved by {UserSession.Username}.");

                    MessageBox.Show("Department details recorded successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDepartments();
                    LoadActiveDepartmentsDropdown();
                    btnClearDept_Click(null, null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error writing to database: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ==========================================
        // TAB 2: POSITIONS LOGIC
        // ==========================================
        private void LoadPositions()
        {
            string query = "SELECT p.PositionID, p.PositionName, d.DepartmentName, p.IsActive, p.DepartmentID " +
                           "FROM Positions p LEFT JOIN Departments d ON p.DepartmentID = d.DepartmentID";
            ExecuteDataFill(query, dgvPositions);
        }

        private void LoadActiveDepartmentsDropdown()
        {
            string query = "SELECT DepartmentID, DepartmentName, IsActive FROM Departments";
            using (OleDbConnection con = DBConnection.GetConnection())
            {
                if (con == null) return;
                try
                {
                    con.Open();
                    using (OleDbDataAdapter da = new OleDbDataAdapter(query, con))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        dt.Columns.Add("DisplayLabel", typeof(string));
                        foreach (DataRow row in dt.Rows)
                        {
                            bool active = Convert.ToBoolean(row["IsActive"]);
                            string name = Convert.ToString(row["DepartmentName"]);
                            row["DisplayLabel"] = active ? name : $"{name} (Inactive)";
                        }

                        cmbPosDept.DataSource = dt;
                        cmbPosDept.DisplayMember = "DisplayLabel";
                        cmbPosDept.ValueMember = "DepartmentID";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading departments dropdown: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvPositions_SelectionChanged(object sender, EventArgs e)
        {
            if (isResetting) return;

            if (dgvPositions.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvPositions.SelectedRows[0];
                if (row.Cells["PositionID"].Value == null || row.Cells["PositionID"].Value == DBNull.Value) return;

                selectedPosId = Convert.ToInt32(row.Cells["PositionID"].Value);
                txtPosName.Text = Convert.ToString(row.Cells["PositionName"].Value);
                chkPosActive.Checked = Convert.ToBoolean(row.Cells["IsActive"].Value);

                if (row.Cells["DepartmentID"].Value != DBNull.Value)
                    cmbPosDept.SelectedValue = Convert.ToInt32(row.Cells["DepartmentID"].Value);

                btnSavePos.Text = "Update";
            }
        }

        private void btnClearPos_Click(object sender, EventArgs e)
        {
            isResetting = true;
            try
            {
                selectedPosId = 0;
                txtPosName.Clear();
                chkPosActive.Checked = true;
                if (cmbPosDept.Items.Count > 0) cmbPosDept.SelectedIndex = 0;
                btnSavePos.Text = "Save";
                dgvPositions.ClearSelection();
            }
            finally
            {
                isResetting = false;
            }
        }

        private void btnSavePos_Click(object sender, EventArgs e)
        {
            string posName = txtPosName.Text.Trim();
            if (string.IsNullOrWhiteSpace(posName) || cmbPosDept.SelectedValue == null)
            {
                MessageBox.Show("Please enter a Position Name and select a Department.", "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int deptId = Convert.ToInt32(cmbPosDept.SelectedValue);

            if (IsDuplicateValue("Positions", "PositionName", posName, "PositionID", selectedPosId, $"DepartmentID = {deptId}"))
            {
                MessageBox.Show("This position already exists in the selected department.", "Duplicate Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!chkPosActive.Checked && selectedPosId > 0)
            {
                int activeJobsCount = GetReferenceCount("JobVacancies", "PositionID", selectedPosId, "Status = 'Open'");
                if (activeJobsCount > 0)
                {
                    DialogResult result = MessageBox.Show(
                        $"Warning: There are {activeJobsCount} active job vacancies utilizing this position. Proceed?",
                        "Referential Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.No) return;
                }
            }

            string query = selectedPosId == 0
                ? "INSERT INTO Positions (PositionName, DepartmentID, IsActive) VALUES (?, ?, ?)"
                : "UPDATE Positions SET PositionName = ?, DepartmentID = ?, IsActive = ? WHERE PositionID = ?";

            using (OleDbConnection con = DBConnection.GetConnection())
            {
                if (con == null) return;
                try
                {
                    con.Open();
                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("?", posName);
                        cmd.Parameters.AddWithValue("?", deptId);
                        cmd.Parameters.AddWithValue("?", chkPosActive.Checked);

                        if (selectedPosId > 0)
                            cmd.Parameters.AddWithValue("?", selectedPosId);

                        cmd.ExecuteNonQuery();
                    }

                    string logAction = selectedPosId == 0 ? "Create Position" : "Update Position";
                    LogActivity(logAction, $"Position '{posName}' in Dept ID {deptId} saved by {UserSession.Username}.");

                    MessageBox.Show("Position recorded successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadPositions();
                    btnClearPos_Click(null, null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error writing to database: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ==========================================
        // TAB 3: EMPLOYMENT TYPES LOGIC
        // ==========================================
        private void LoadEmploymentTypes()
        {
            string query = "SELECT EmploymentTypeID, TypeName, IsActive FROM EmploymentTypes";
            ExecuteDataFill(query, dgvEmployment);
        }

        private void dgvEmployment_SelectionChanged(object sender, EventArgs e)
        {
            if (isResetting) return;

            if (dgvEmployment.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvEmployment.SelectedRows[0];
                if (row.Cells["EmploymentTypeID"].Value == null || row.Cells["EmploymentTypeID"].Value == DBNull.Value) return;

                selectedEmpId = Convert.ToInt32(row.Cells["EmploymentTypeID"].Value);
                txtEmpTypeName.Text = Convert.ToString(row.Cells["TypeName"].Value);
                chkEmpTypeActive.Checked = Convert.ToBoolean(row.Cells["IsActive"].Value);
                btnSaveEmp.Text = "Update";
            }
        }

        private void btnClearEmp_Click(object sender, EventArgs e)
        {
            isResetting = true;
            try
            {
                selectedEmpId = 0;
                txtEmpTypeName.Clear();
                chkEmpTypeActive.Checked = true;
                btnSaveEmp.Text = "Save";
                dgvEmployment.ClearSelection();
            }
            finally
            {
                isResetting = false;
            }
        }

        private void btnSaveEmp_Click(object sender, EventArgs e)
        {
            string empTypeName = txtEmpTypeName.Text.Trim();
            if (string.IsNullOrWhiteSpace(empTypeName))
            {
                MessageBox.Show("Please enter an Employment Type Name.", "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (IsDuplicateValue("EmploymentTypes", "TypeName", empTypeName, "EmploymentTypeID", selectedEmpId))
            {
                MessageBox.Show("An employment type with this name already exists.", "Duplicate Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = selectedEmpId == 0
                ? "INSERT INTO EmploymentTypes (TypeName, IsActive) VALUES (?, ?)"
                : "UPDATE EmploymentTypes SET TypeName = ?, IsActive = ? WHERE EmploymentTypeID = ?";

            using (OleDbConnection con = DBConnection.GetConnection())
            {
                if (con == null) return;
                try
                {
                    con.Open();
                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("?", empTypeName);
                        cmd.Parameters.AddWithValue("?", chkEmpTypeActive.Checked);

                        if (selectedEmpId > 0)
                            cmd.Parameters.AddWithValue("?", selectedEmpId);

                        cmd.ExecuteNonQuery();
                    }

                    string logAction = selectedEmpId == 0 ? "Create Employment Type" : "Update Employment Type";
                    LogActivity(logAction, $"Employment Type '{empTypeName}' saved by {UserSession.Username}.");

                    MessageBox.Show("Employment type configuration saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadEmploymentTypes();
                    btnClearEmp_Click(null, null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error writing to database: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ==========================================
        // TAB 4: REQUIREMENT TYPES LOGIC
        // ==========================================
        private void LoadRequirementTypes()
        {
            string query = "SELECT RequirementTypeID, RequirementName, IsRequired FROM RequirementTypes";
            ExecuteDataFill(query, dgvRequirements);
        }

        private void dgvRequirements_SelectionChanged(object sender, EventArgs e)
        {
            if (isResetting) return;

            if (dgvRequirements.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvRequirements.SelectedRows[0];
                if (row.Cells["RequirementTypeID"].Value == null || row.Cells["RequirementTypeID"].Value == DBNull.Value) return;

                selectedReqId = Convert.ToInt32(row.Cells["RequirementTypeID"].Value);
                txtReqName.Text = Convert.ToString(row.Cells["RequirementName"].Value);
                chkReqRequired.Checked = Convert.ToBoolean(row.Cells["IsRequired"].Value);
                btnSaveReq.Text = "Update";
            }
        }

        private void btnClearReq_Click(object sender, EventArgs e)
        {
            isResetting = true;
            try
            {
                selectedReqId = 0;
                txtReqName.Clear();
                chkReqRequired.Checked = true;
                btnSaveReq.Text = "Save";
                dgvRequirements.ClearSelection();
            }
            finally
            {
                isResetting = false;
            }
        }

        private void btnSaveReq_Click(object sender, EventArgs e)
        {
            string reqName = txtReqName.Text.Trim();
            if (string.IsNullOrWhiteSpace(reqName))
            {
                MessageBox.Show("Please enter a Requirement Name.", "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (IsDuplicateValue("RequirementTypes", "RequirementName", reqName, "RequirementTypeID", selectedReqId))
            {
                MessageBox.Show("A requirement type with this name already exists.", "Duplicate Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = selectedReqId == 0
                ? "INSERT INTO RequirementTypes (RequirementName, IsRequired) VALUES (?, ?)"
                : "UPDATE RequirementTypes SET RequirementName = ?, IsRequired = ? WHERE RequirementTypeID = ?";

            using (OleDbConnection con = DBConnection.GetConnection())
            {
                if (con == null) return;
                try
                {
                    con.Open();
                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("?", reqName);
                        cmd.Parameters.AddWithValue("?", chkReqRequired.Checked);

                        if (selectedReqId > 0)
                            cmd.Parameters.AddWithValue("?", selectedReqId);

                        cmd.ExecuteNonQuery();
                    }

                    string logAction = selectedReqId == 0 ? "Create Requirement Type" : "Update Requirement Type";
                    LogActivity(logAction, $"Requirement Type '{reqName}' saved by {UserSession.Username}.");

                    MessageBox.Show("Requirement configuration saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadRequirementTypes();
                    btnClearReq_Click(null, null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error writing to database: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ==========================================
        // TAB 5: INTERVIEW TYPES LOGIC
        // ==========================================
        private void LoadInterviewTypes()
        {
            string query = "SELECT InterviewTypeID, TypeName, IsActive FROM InterviewTypes";
            ExecuteDataFill(query, dgvInterviews);
        }

        private void dgvInterviews_SelectionChanged(object sender, EventArgs e)
        {
            if (isResetting) return;

            if (dgvInterviews.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvInterviews.SelectedRows[0];
                if (row.Cells["InterviewTypeID"].Value == null || row.Cells["InterviewTypeID"].Value == DBNull.Value) return;

                selectedIntId = Convert.ToInt32(row.Cells["InterviewTypeID"].Value);
                txtIntTypeName.Text = Convert.ToString(row.Cells["TypeName"].Value);
                chkIntActive.Checked = Convert.ToBoolean(row.Cells["IsActive"].Value);
                btnSaveInt.Text = "Update";
            }
        }

        private void btnClearInt_Click(object sender, EventArgs e)
        {
            isResetting = true;
            try
            {
                selectedIntId = 0;
                txtIntTypeName.Clear();
                chkIntActive.Checked = true;
                btnSaveInt.Text = "Save";
                dgvInterviews.ClearSelection();
            }
            finally
            {
                isResetting = false;
            }
        }

        private void btnSaveInt_Click(object sender, EventArgs e)
        {
            string intTypeName = txtIntTypeName.Text.Trim();
            if (string.IsNullOrWhiteSpace(intTypeName))
            {
                MessageBox.Show("Please enter an Interview Type Name.", "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (IsDuplicateValue("InterviewTypes", "TypeName", intTypeName, "InterviewTypeID", selectedIntId))
            {
                MessageBox.Show("An interview type with this name already exists.", "Duplicate Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = selectedIntId == 0
                ? "INSERT INTO InterviewTypes (TypeName, IsActive) VALUES (?, ?)"
                : "UPDATE InterviewTypes SET TypeName = ?, IsActive = ? WHERE InterviewTypeID = ?";

            using (OleDbConnection con = DBConnection.GetConnection())
            {
                if (con == null) return;
                try
                {
                    con.Open();
                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("?", intTypeName);
                        cmd.Parameters.AddWithValue("?", chkIntActive.Checked);

                        if (selectedIntId > 0)
                            cmd.Parameters.AddWithValue("?", selectedIntId);

                        cmd.ExecuteNonQuery();
                    }

                    string logAction = selectedIntId == 0 ? "Create Interview Type" : "Update Interview Type";
                    LogActivity(logAction, $"Interview Type '{intTypeName}' saved by {UserSession.Username}.");

                    MessageBox.Show("Interview type configuration saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadInterviewTypes();
                    btnClearInt_Click(null, null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error writing to database: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ==========================================
        // TAB 6: ASSESSMENT TYPES LOGIC
        // ==========================================
        private void LoadAssessmentTypes()
        {
            string query = "SELECT AssessmentTypeID, TypeName, IsActive FROM AssessmentTypes";
            ExecuteDataFill(query, dgvAssessments);
        }

        private void dgvAssessments_SelectionChanged(object sender, EventArgs e)
        {
            if (isResetting) return;

            if (dgvAssessments.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvAssessments.SelectedRows[0];
                if (row.Cells["AssessmentTypeID"].Value == null || row.Cells["AssessmentTypeID"].Value == DBNull.Value) return;

                selectedAssId = Convert.ToInt32(row.Cells["AssessmentTypeID"].Value);
                txtAssTypeName.Text = Convert.ToString(row.Cells["TypeName"].Value);
                chkAssActive.Checked = Convert.ToBoolean(row.Cells["IsActive"].Value);
                btnSaveAss.Text = "Update";
            }
        }

        private void btnClearAss_Click(object sender, EventArgs e)
        {
            isResetting = true;
            try
            {
                selectedAssId = 0;
                txtAssTypeName.Clear();
                chkAssActive.Checked = true;
                btnSaveAss.Text = "Save";
                dgvAssessments.ClearSelection();
            }
            finally
            {
                isResetting = false;
            }
        }

        private void btnSaveAss_Click(object sender, EventArgs e)
        {
            string assTypeName = txtAssTypeName.Text.Trim();
            if (string.IsNullOrWhiteSpace(assTypeName))
            {
                MessageBox.Show("Please enter an Assessment Type Name.", "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (IsDuplicateValue("AssessmentTypes", "TypeName", assTypeName, "AssessmentTypeID", selectedAssId))
            {
                MessageBox.Show("An assessment type with this name already exists.", "Duplicate Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = selectedAssId == 0
                ? "INSERT INTO AssessmentTypes (TypeName, IsActive) VALUES (?, ?)"
                : "UPDATE AssessmentTypes SET TypeName = ?, IsActive = ? WHERE AssessmentTypeID = ?";

            using (OleDbConnection con = DBConnection.GetConnection())
            {
                if (con == null) return;
                try
                {
                    con.Open();
                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("?", assTypeName);
                        cmd.Parameters.AddWithValue("?", chkAssActive.Checked);

                        if (selectedAssId > 0)
                            cmd.Parameters.AddWithValue("?", selectedAssId);

                        cmd.ExecuteNonQuery();
                    }

                    string logAction = selectedAssId == 0 ? "Create Assessment Type" : "Update Assessment Type";
                    LogActivity(logAction, $"Assessment Type '{assTypeName}' saved by {UserSession.Username}.");

                    MessageBox.Show("Assessment type configuration saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadAssessmentTypes();
                    btnClearAss_Click(null, null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error writing to database: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ==========================================
        // TAB 7: USER ACCOUNTS LOGIC
        // ==========================================
        private void LoadUserAccounts()
        {
            // Note: Users table column mapping handles reserved Access keyword [Password]
            string query = "SELECT UserID, Username, [Full Name] as FullName, Role FROM Users";
            try
            {
                if (dgvUsers != null)
                {
                    ExecuteDataFill(query, dgvUsers);
                }
            }
            catch { }
        }

        private void dgvUsers_SelectionChanged(object sender, EventArgs e)
        {
            if (isResetting) return;

            try
            {
                if (dgvUsers != null && dgvUsers.SelectedRows.Count > 0)
                {
                    DataGridViewRow row = dgvUsers.SelectedRows[0];
                    if (row.Cells["UserID"].Value == null || row.Cells["UserID"].Value == DBNull.Value) return;

                    selectedUserId = Convert.ToInt32(row.Cells["UserID"].Value);
                    txtUserUsername.Text = Convert.ToString(row.Cells["Username"].Value);
                    txtUserFullName.Text = Convert.ToString(row.Cells["FullName"].Value);
                    cmbUserRole.SelectedItem = Convert.ToString(row.Cells["Role"].Value);

                    // Clear password textbox for visual security
                    txtUserPassword.Clear();

                    btnSaveUser.Text = "Update";
                }
            }
            catch { }
        }

        private void btnClearUser_Click(object sender, EventArgs e)
        {
            isResetting = true;
            try
            {
                selectedUserId = 0;
                if (txtUserUsername != null) txtUserUsername.Clear();
                if (txtUserFullName != null) txtUserFullName.Clear();
                if (txtUserPassword != null) txtUserPassword.Clear();
                if (cmbUserRole != null && cmbUserRole.Items.Count > 0) cmbUserRole.SelectedIndex = 0;
                if (btnSaveUser != null) btnSaveUser.Text = "Save";
                if (dgvUsers != null) dgvUsers.ClearSelection();
            }
            catch { }
            finally
            {
                isResetting = false;
            }
        }

        private void btnSaveUser_Click(object sender, EventArgs e)
        {
            string username = txtUserUsername.Text.Trim();
            string fullName = txtUserFullName.Text.Trim();
            string role = cmbUserRole.SelectedItem?.ToString();
            string password = txtUserPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(fullName) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Please fill out Username, Full Name, and Role.", "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedUserId == 0 && string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("A password is required for new accounts.", "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (IsDuplicateValue("Users", "Username", username, "UserID", selectedUserId))
            {
                MessageBox.Show("A user with this username already exists.", "Duplicate Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query;
            bool updatingPassword = !string.IsNullOrWhiteSpace(password);

            if (selectedUserId == 0)
            {
                query = "INSERT INTO Users (Username, [Password], [Full Name], Role) VALUES (?, ?, ?, ?)";
            }
            else
            {
                query = updatingPassword
                    ? "UPDATE Users SET Username = ?, [Password] = ?, [Full Name] = ?, Role = ? WHERE UserID = ?"
                    : "UPDATE Users SET Username = ?, [Full Name] = ?, Role = ? WHERE UserID = ?";
            }

            using (OleDbConnection con = DBConnection.GetConnection())
            {
                if (con == null) return;
                try
                {
                    con.Open();
                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("?", username);
                        if (selectedUserId == 0 || updatingPassword)
                        {
                            cmd.Parameters.AddWithValue("?", password);
                        }
                        cmd.Parameters.AddWithValue("?", fullName);
                        cmd.Parameters.AddWithValue("?", role);

                        if (selectedUserId > 0)
                            cmd.Parameters.AddWithValue("?", selectedUserId);

                        cmd.ExecuteNonQuery();
                    }

                    string logAction = selectedUserId == 0 ? "Create User" : "Update User";
                    LogActivity(logAction, $"User Account '{username}' ({role}) modified by Admin/Manager: {UserSession.Username}.");

                    MessageBox.Show("User configuration recorded successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUserAccounts();
                    btnClearUser_Click(null, null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error writing user to database: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ==========================================
        // DYNAMIC IN-MEMORY FILTER LOGIC (SEARCH)
        // ==========================================
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt == null || txt.Tag == null) return;

            DataGridView dgv = txt.Tag as DataGridView;
            if (dgv == null || dgv.DataSource == null) return;

            BindingSource bs = dgv.DataSource as BindingSource;
            if (bs == null) return;

            string rawText = txt.Text.Trim();
            string filterText = rawText.Replace("'", "''").Replace("[", "[[]").Replace("]", "[]]");

            if (string.IsNullOrEmpty(filterText))
            {
                bs.Filter = "";
                return;
            }

            string filterExpression = "";

            if (dgv == dgvDepartments)
            {
                filterExpression = $"DepartmentName LIKE '%{filterText}%' OR Description LIKE '%{filterText}%'";
            }
            else if (dgv == dgvPositions)
            {
                filterExpression = $"PositionName LIKE '%{filterText}%' OR DepartmentName LIKE '%{filterText}%'";
            }
            else if (dgv == dgvEmployment)
            {
                filterExpression = $"TypeName LIKE '%{filterText}%'";
            }
            else if (dgv == dgvRequirements)
            {
                filterExpression = $"RequirementName LIKE '%{filterText}%'";
            }
            else if (dgv == dgvInterviews)
            {
                filterExpression = $"TypeName LIKE '%{filterText}%'";
            }
            else if (dgv == dgvAssessments)
            {
                filterExpression = $"TypeName LIKE '%{filterText}%'";
            }
            else if (dgv == dgvUsers)
            {
                filterExpression = $"Username LIKE '%{filterText}%' OR FullName LIKE '%{filterText}%' OR Role LIKE '%{filterText}%'";
            }

            try
            {
                bs.Filter = filterExpression;
            }
            catch
            {
                bs.Filter = "";
            }
        }
    }
}