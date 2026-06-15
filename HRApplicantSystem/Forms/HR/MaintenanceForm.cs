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
        private int selectedUserId = 0;     // Selected administrative user account
        private int selectedAppAccId = 0;    // Selected applicant account

        // Programmatically generated User Management Controls to resolve compiler CS0103 issues
        private TabControl mainTabControl;
        private TabPage tabUsers;
        private DataGridView dgvUsers;
        private TextBox txtUserUsername;
        private TextBox txtUserFullName;
        private TextBox txtUserPassword;
        private ComboBox cmbUserRole;
        private CheckBox chkUserActive; // Added User Status Control
        private Button btnSaveUser;
        private Button btnClearUser;
        private Label lblPasswordHelp;
        private TextBox txtSearchUsers;

        // Programmatically generated Applicant Account Status Controls
        private TabPage tabApplicantAccounts;
        private DataGridView dgvAppAccounts;
        private TextBox txtAppEmail;
        private TextBox txtAppFullName;
        private ComboBox cmbAppStatus;
        private Button btnSaveAppAcc;
        private Button btnClearAppAcc;
        private TextBox txtSearchAppAcc;

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

            // Programmatically construct Tab 7 (User Accounts) dynamically
            InitializeUserManagementTab();

            // Programmatically construct Tab 8 (Applicant Accounts) dynamically
            InitializeApplicantAccountsTab();

            // Apply modern UI Styling dynamically on Load
            ApplyThemeStyles();

            LoadAllData();
        }

        /// <summary>
        /// Programmatic layout manager that builds the user account interface dynamically.
        /// </summary>
        private void InitializeUserManagementTab()
        {
            mainTabControl = FindTabControl(this);
            if (mainTabControl == null) return;

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
            yOffset += 35;

            // Administrative Active Status Checkbox
            chkUserActive = new CheckBox { Text = "Account Active", Location = new Point(5, yOffset), Size = new Size(270, 20), Font = new Font("Segoe UI", 9F), Checked = true };
            pnlInput.Controls.Add(chkUserActive);
            yOffset += 30;

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

            txtSearchUsers.Tag = dgvUsers;
        }

        /// <summary>
        /// Programmatic layout manager that builds the Applicant Account administration tab.
        /// </summary>
        private void InitializeApplicantAccountsTab()
        {
            mainTabControl = FindTabControl(this);
            if (mainTabControl == null) return;

            tabApplicantAccounts = new TabPage("Applicant Accounts");
            tabApplicantAccounts.BackColor = Color.White;
            mainTabControl.TabPages.Add(tabApplicantAccounts);

            // Left Side Panel: Inputs
            Panel pnlInput = new Panel
            {
                Location = new Point(15, 15),
                Size = new Size(290, tabApplicantAccounts.Height - 30),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left
            };
            tabApplicantAccounts.Controls.Add(pnlInput);

            int yOffset = 10;

            Label lblEmail = new Label { Text = "Email Address (Read-Only):", Location = new Point(5, yOffset), AutoSize = true, Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold), ForeColor = UITheme.ColorSecondary };
            pnlInput.Controls.Add(lblEmail);
            yOffset += 20;

            txtAppEmail = new TextBox { Location = new Point(5, yOffset), Size = new Size(270, 25), Font = new Font("Segoe UI", 9F), ReadOnly = true, BackColor = Color.FromArgb(241, 245, 249) };
            pnlInput.Controls.Add(txtAppEmail);
            yOffset += 35;

            Label lblFullName = new Label { Text = "Full Name (Read-Only):", Location = new Point(5, yOffset), AutoSize = true, Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold), ForeColor = UITheme.ColorSecondary };
            pnlInput.Controls.Add(lblFullName);
            yOffset += 20;

            txtAppFullName = new TextBox { Location = new Point(5, yOffset), Size = new Size(270, 25), Font = new Font("Segoe UI", 9F), ReadOnly = true, BackColor = Color.FromArgb(241, 245, 249) };
            pnlInput.Controls.Add(txtAppFullName);
            yOffset += 35;

            Label lblStatus = new Label { Text = "Account Status:", Location = new Point(5, yOffset), AutoSize = true, Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold), ForeColor = UITheme.ColorSecondary };
            pnlInput.Controls.Add(lblStatus);
            yOffset += 20;

            cmbAppStatus = new ComboBox { Location = new Point(5, yOffset), Size = new Size(270, 25), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9F) };
            cmbAppStatus.Items.AddRange(new object[] { "Active", "Inactive" });
            cmbAppStatus.SelectedIndex = 0;
            pnlInput.Controls.Add(cmbAppStatus);
            yOffset += 40;

            btnSaveAppAcc = new Button { Text = "Update Status", Location = new Point(5, yOffset), Size = new Size(130, 32) };
            btnSaveAppAcc.Click += btnSaveAppAcc_Click;
            pnlInput.Controls.Add(btnSaveAppAcc);

            btnClearAppAcc = new Button { Text = "Clear", Location = new Point(145, yOffset), Size = new Size(130, 32) };
            btnClearAppAcc.Click += btnClearAppAcc_Click;
            pnlInput.Controls.Add(btnClearAppAcc);

            // Right Side Panel: Grid & Filters
            Panel pnlGrid = new Panel
            {
                Location = new Point(320, 15),
                Size = new Size(tabApplicantAccounts.Width - 335, tabApplicantAccounts.Height - 30),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            tabApplicantAccounts.Controls.Add(pnlGrid);

            Label lblSearch = new Label { Text = "Search Applicant Accounts:", Location = new Point(5, 10), AutoSize = true, Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold), ForeColor = UITheme.ColorSecondary };
            pnlGrid.Controls.Add(lblSearch);

            txtSearchAppAcc = new TextBox { Location = new Point(180, 7), Size = new Size(220, 25), Font = new Font("Segoe UI", 9F) };
            txtSearchAppAcc.TextChanged += txtSearch_TextChanged;
            pnlGrid.Controls.Add(txtSearchAppAcc);

            dgvAppAccounts = new DataGridView
            {
                Location = new Point(5, 40),
                Size = new Size(pnlGrid.Width - 10, pnlGrid.Height - 50),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AllowUserToAddRows = false,
                ReadOnly = true
            };
            dgvAppAccounts.SelectionChanged += dgvAppAccounts_SelectionChanged;
            pnlGrid.Controls.Add(dgvAppAccounts);

            txtSearchAppAcc.Tag = dgvAppAccounts;
        }

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

            // Dynamic Grids styling
            if (dgvUsers != null) UITheme.StyleDataGridView(dgvUsers);
            if (dgvAppAccounts != null) UITheme.StyleDataGridView(dgvAppAccounts);

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

            // Dynamic User and Applicant Actions Styling
            if (btnSaveUser != null) UITheme.StylePrimaryButton(btnSaveUser);
            if (btnClearUser != null) UITheme.StyleSecondaryButton(btnClearUser);

            if (btnSaveAppAcc != null) UITheme.StylePrimaryButton(btnSaveAppAcc);
            if (btnClearAppAcc != null) UITheme.StyleSecondaryButton(btnClearAppAcc);

            if (btnBack != null) UITheme.StyleSecondaryButton(btnBack);
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
            LoadUserAccounts();
            LoadApplicantAccounts();
        }

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
                    return 0;
                }
            }
        }

        private void LogActivity(string action, string details)
        {
            // Combines the action and details into a single string to save in the single 'Action' column
            string combinedAction = $"{action}: {details}";
            string query = "INSERT INTO AuditTrail (UserID, [Action], DateCreated) VALUES (?, ?, ?)";
            using (OleDbConnection con = DBConnection.GetConnection())
            {
                if (con == null) return;
                try
                {
                    con.Open();
                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("?", UserSession.UserID > 0 ? UserSession.UserID : 1);
                        cmd.Parameters.AddWithValue("?", combinedAction);
                        cmd.Parameters.AddWithValue("?", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("LogActivity Error: " + ex.Message);
                }
            }
        }

        // --- TAB 1: DEPARTMENTS LOGIC ---
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

        // --- TAB 2: POSITIONS LOGIC ---
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

        // --- TAB 3: EMPLOYMENT TYPES LOGIC ---
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

        // --- TAB 4: REQUIREMENT TYPES LOGIC ---
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

        // --- TAB 5: INTERVIEW TYPES LOGIC ---
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

        // --- TAB 6: ASSESSMENT TYPES LOGIC ---
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

        // --- TAB 7: USER ACCOUNTS LOGIC (Administrative Logins with Status Support) ---
        private void LoadUserAccounts()
        {
            // Safely retrieves administrative status indicator
            string query = "SELECT UserID, Username, [Full Name] as FullName, Role, IsActive FROM Users";
            try
            {
                if (dgvUsers != null)
                {
                    ExecuteDataFill(query, dgvUsers);
                }
            }
            catch
            {
                // Graceful fallback if database schema does not have IsActive column yet
                string fallbackQuery = "SELECT UserID, Username, [Full Name] as FullName, Role FROM Users";
                if (dgvUsers != null)
                {
                    ExecuteDataFill(fallbackQuery, dgvUsers);
                }
            }
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

                    // CORRECTED: Check the Grid columns directly to see if IsActive is present
                    if (dgvUsers.Columns.Contains("IsActive") && row.Cells["IsActive"].Value != DBNull.Value)
                    {
                        chkUserActive.Checked = Convert.ToBoolean(row.Cells["IsActive"].Value);
                    }
                    else
                    {
                        chkUserActive.Checked = true;
                    }

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
                if (chkUserActive != null) chkUserActive.Checked = true;
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
            bool isActive = chkUserActive.Checked;

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
                query = "INSERT INTO Users (Username, [Password], [Full Name], Role, IsActive) VALUES (?, ?, ?, ?, ?)";
            }
            else
            {
                query = updatingPassword
                    ? "UPDATE Users SET Username = ?, [Password] = ?, [Full Name] = ?, Role = ?, IsActive = ? WHERE UserID = ?"
                    : "UPDATE Users SET Username = ?, [Full Name] = ?, Role = ?, IsActive = ? WHERE UserID = ?";
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
                        cmd.Parameters.AddWithValue("?", isActive);

                        if (selectedUserId > 0)
                            cmd.Parameters.AddWithValue("?", selectedUserId);

                        cmd.ExecuteNonQuery();
                    }

                    string logAction = selectedUserId == 0 ? "Create User" : "Update User";
                    LogActivity(logAction, $"User Account '{username}' ({role}, Active: {isActive}) modified by Admin/Manager: {UserSession.Username}.");

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

        // --- TAB 8: APPLICANT ACCOUNTS LOGIC (Controls AccountStatus) ---
        private void LoadApplicantAccounts()
        {
            // Dynamically joins profiles for clear and unified views
            string query = @"SELECT aa.ApplicantID, aa.Email, aa.AccountStatus, 
                             (a.FirstName + ' ' + a.LastName) AS FullName 
                             FROM ApplicantAccounts aa 
                             LEFT JOIN Applicants a ON aa.ApplicantID = a.ApplicantID";
            try
            {
                if (dgvAppAccounts != null)
                {
                    ExecuteDataFill(query, dgvAppAccounts);
                }
            }
            catch { }
        }

        private void dgvAppAccounts_SelectionChanged(object sender, EventArgs e)
        {
            if (isResetting) return;

            try
            {
                if (dgvAppAccounts != null && dgvAppAccounts.SelectedRows.Count > 0)
                {
                    DataGridViewRow row = dgvAppAccounts.SelectedRows[0];
                    if (row.Cells["ApplicantID"].Value == null || row.Cells["ApplicantID"].Value == DBNull.Value) return;

                    selectedAppAccId = Convert.ToInt32(row.Cells["ApplicantID"].Value);
                    txtAppEmail.Text = Convert.ToString(row.Cells["Email"].Value);
                    txtAppFullName.Text = Convert.ToString(row.Cells["FullName"].Value);

                    string status = Convert.ToString(row.Cells["AccountStatus"].Value);
                    if (cmbAppStatus.Items.Contains(status))
                    {
                        cmbAppStatus.SelectedItem = status;
                    }
                    else
                    {
                        cmbAppStatus.SelectedIndex = 0;
                    }
                }
            }
            catch { }
        }

        private void btnClearAppAcc_Click(object sender, EventArgs e)
        {
            isResetting = true;
            try
            {
                selectedAppAccId = 0;
                if (txtAppEmail != null) txtAppEmail.Clear();
                if (txtAppFullName != null) txtAppFullName.Clear();
                if (cmbAppStatus != null && cmbAppStatus.Items.Count > 0) cmbAppStatus.SelectedIndex = 0;
                if (dgvAppAccounts != null) dgvAppAccounts.ClearSelection();
            }
            catch { }
            finally
            {
                isResetting = false;
            }
        }

        private void btnSaveAppAcc_Click(object sender, EventArgs e)
        {
            if (selectedAppAccId == 0)
            {
                MessageBox.Show("Please select an applicant account from the table to update.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string newStatus = cmbAppStatus.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(newStatus)) return;

            string query = "UPDATE ApplicantAccounts SET AccountStatus = ? WHERE ApplicantID = ?";

            using (OleDbConnection con = DBConnection.GetConnection())
            {
                if (con == null) return;
                try
                {
                    con.Open();
                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("?", newStatus);
                        cmd.Parameters.AddWithValue("?", selectedAppAccId);
                        cmd.ExecuteNonQuery();
                    }

                    LogActivity("Update Applicant Account Status", $"Applicant account status for ID {selectedAppAccId} ({txtAppEmail.Text}) updated to {newStatus} by {UserSession.Username}.");

                    MessageBox.Show("Applicant account status updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadApplicantAccounts();
                    btnClearAppAcc_Click(null, null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating applicant account status: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // --- DYNAMIC IN-MEMORY FILTER LOGIC (SEARCH) ---
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
                // Dynamic checks to prevent search queries from throwing errors on fallback schemas
                if (bs.DataSource is DataTable dt && dt.Columns.Contains("IsActive"))
                {
                    filterExpression = $"Username LIKE '%{filterText}%' OR FullName LIKE '%{filterText}%' OR Role LIKE '%{filterText}%'";
                }
                else
                {
                    filterExpression = $"Username LIKE '%{filterText}%' OR FullName LIKE '%{filterText}%' OR Role LIKE '%{filterText}%'";
                }
            }
            else if (dgv == dgvAppAccounts)
            {
                filterExpression = $"Email LIKE '%{filterText}%' OR FullName LIKE '%{filterText}%' OR AccountStatus LIKE '%{filterText}%'";
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

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}