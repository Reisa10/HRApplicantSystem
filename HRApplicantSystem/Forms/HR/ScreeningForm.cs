using HRApplicantSystem.Classes;
using HRApplicantSystem.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace HRApplicantSystem.Forms.HR
{
    public partial class ScreeningForm : Form
    {
        private int selectedApplicationID = 0;
        private int selectedApplicantID = 0;
        private DataTable dtApplications = new DataTable();

        // Dynamically managed UI controls for the profile card
        private Panel pnlProfileCard;
        private Label lblProfileTitle;
        private Label lblContactNo;
        private Label lblEduTitle;
        private TextBox txtEducation;
        private Label lblSkillsTitle;
        private TextBox txtSkills;
        private Label lblExpTitle;
        private TextBox txtExperience;

        // Styling Colors
        private readonly Color BrandPrimary = Color.FromArgb(44, 62, 80);      // Slate Blue
        private readonly Color ColorSuccess = Color.FromArgb(39, 174, 96);     // Emerald
        private readonly Color ColorDanger = Color.FromArgb(192, 57, 43);      // Crimson
        private readonly Color ColorLightBg = Color.FromArgb(248, 249, 250);   // Soft Gray
        private readonly Color BorderColor = Color.FromArgb(200, 214, 229);    // Soft border gray

        public ScreeningForm()
        {
            InitializeComponent();

            // Role Authorization Check
            if (UserSession.Role != "HR Staff" && UserSession.Role != "HR Manager" && UserSession.Role != "Admin")
            {
                MessageBox.Show("Access Denied. You do not have permission to access Screening.",
                    "Unauthorized", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Load += (s, e) => this.Close();
            }
        }

        private void ScreeningForm_Load(object sender, EventArgs e)
        {
            InitializeDynamicProfilePanel();
            ApplyProfessionalStyles();
            LoadApplications();

            // Execute the layout auto-stacking adjustment
            ArrangeLayoutStack();
        }

        /// <summary>
        /// Programmatically creates and styles the profile highlights panel.
        /// </summary>
        private void InitializeDynamicProfilePanel()
        {
            pnlProfileCard = new Panel();
            pnlProfileCard.BackColor = Color.White;
            pnlProfileCard.BorderStyle = BorderStyle.FixedSingle;
            pnlProfileCard.Size = new Size(grpDecision.Width, 200); // Optimized compact height

            lblProfileTitle = new Label();
            lblProfileTitle.Text = "APPLICANT PROFILE HIGHLIGHTS";
            lblProfileTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblProfileTitle.ForeColor = BrandPrimary;
            lblProfileTitle.Location = new Point(12, 10);
            lblProfileTitle.AutoSize = true;

            lblContactNo = new Label();
            lblContactNo.Text = "Contact: —";
            lblContactNo.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic);
            lblContactNo.ForeColor = Color.DimGray;
            lblContactNo.Location = new Point(12, 30);
            lblContactNo.Size = new Size(pnlProfileCard.Width - 24, 20);

            lblEduTitle = new Label();
            lblEduTitle.Text = "Education:";
            lblEduTitle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblEduTitle.ForeColor = Color.DarkSlateGray;
            lblEduTitle.Location = new Point(12, 55);
            lblEduTitle.Size = new Size(80, 15);

            txtEducation = new TextBox();
            txtEducation.Multiline = true;
            txtEducation.ReadOnly = true;
            txtEducation.BackColor = Color.FromArgb(245, 246, 250);
            txtEducation.BorderStyle = BorderStyle.None;
            txtEducation.Font = new Font("Segoe UI", 8.5F);
            txtEducation.Location = new Point(12, 72);
            txtEducation.Size = new Size(pnlProfileCard.Width - 24, 30);

            lblSkillsTitle = new Label();
            lblSkillsTitle.Text = "Skills:";
            lblSkillsTitle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblSkillsTitle.ForeColor = Color.DarkSlateGray;
            lblSkillsTitle.Location = new Point(12, 105);
            lblSkillsTitle.Size = new Size(80, 15);

            txtSkills = new TextBox();
            txtSkills.Multiline = true;
            txtSkills.ReadOnly = true;
            txtSkills.BackColor = Color.FromArgb(245, 246, 250);
            txtSkills.BorderStyle = BorderStyle.None;
            txtSkills.Font = new Font("Segoe UI", 8.5F);
            txtSkills.Location = new Point(12, 122);
            txtSkills.Size = new Size(pnlProfileCard.Width - 24, 30);

            lblExpTitle = new Label();
            lblExpTitle.Text = "Work Experience:";
            lblExpTitle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblExpTitle.ForeColor = Color.DarkSlateGray;
            lblExpTitle.Location = new Point(12, 155);
            lblExpTitle.Size = new Size(120, 15);

            txtExperience = new TextBox();
            txtExperience.Multiline = true;
            txtExperience.ReadOnly = true;
            txtExperience.BackColor = Color.FromArgb(245, 246, 250);
            txtExperience.BorderStyle = BorderStyle.None;
            txtExperience.Font = new Font("Segoe UI", 8.5F);
            txtExperience.Location = new Point(12, 172);
            txtExperience.Size = new Size(pnlProfileCard.Width - 24, 20);

            pnlProfileCard.Controls.Add(lblProfileTitle);
            pnlProfileCard.Controls.Add(lblContactNo);
            pnlProfileCard.Controls.Add(lblEduTitle);
            pnlProfileCard.Controls.Add(txtEducation);
            pnlProfileCard.Controls.Add(lblSkillsTitle);
            pnlProfileCard.Controls.Add(txtSkills);
            pnlProfileCard.Controls.Add(lblExpTitle);
            pnlProfileCard.Controls.Add(txtExperience);

            this.Controls.Add(pnlProfileCard);
        }

        /// <summary>
        /// Auto-arranges all form sections vertically and resizes the form window dynamically.
        /// </summary>
        private void ArrangeLayoutStack()
        {
            // Identify the container holding your DataGridView
            Control topContainer = dgvApplications.Parent != this ? dgvApplications.Parent : dgvApplications;

            int leftPosition = topContainer.Left;
            int widthSetting = topContainer.Width;

            pnlProfileCard.Width = widthSetting;
            grpDecision.Width = widthSetting;

            pnlProfileCard.Location = new Point(leftPosition, topContainer.Bottom + 12);
            grpDecision.Location = new Point(leftPosition, pnlProfileCard.Bottom + 12);

            Button btnSaveCtrl = GetOrCreateButton("btnSave", "Save Decision", btnSave_Click);
            Button btnBackCtrl = GetOrCreateButton("btnBack", "Back", btnBack_Click);

            btnSaveCtrl.Size = new Size(150, 36);
            btnSaveCtrl.FlatStyle = FlatStyle.Flat;
            btnSaveCtrl.BackColor = BrandPrimary;
            btnSaveCtrl.ForeColor = Color.White;
            btnSaveCtrl.FlatAppearance.BorderSize = 0;
            btnSaveCtrl.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            btnBackCtrl.Size = new Size(90, 36);
            btnBackCtrl.FlatStyle = FlatStyle.Flat;
            btnBackCtrl.BackColor = Color.FromArgb(230, 235, 240);
            btnBackCtrl.ForeColor = Color.FromArgb(60, 70, 80);
            btnBackCtrl.FlatAppearance.BorderColor = Color.FromArgb(180, 190, 200);
            btnBackCtrl.Font = new Font("Segoe UI", 9F, FontStyle.Regular);

            btnSaveCtrl.Location = new Point(grpDecision.Right - btnSaveCtrl.Width, grpDecision.Bottom + 12);
            btnBackCtrl.Location = new Point(btnSaveCtrl.Left - btnBackCtrl.Width - 10, grpDecision.Bottom + 12);

            this.ClientSize = new Size(this.ClientSize.Width, btnSaveCtrl.Bottom + 15);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private Button GetOrCreateButton(string name, string defaultText, EventHandler clickHandler)
        {
            Control[] found = this.Controls.Find(name, true);
            if (found.Length > 0 && found[0] is Button existingButton)
            {
                if (existingButton.Parent != this)
                {
                    existingButton.Parent.Controls.Remove(existingButton);
                    this.Controls.Add(existingButton);
                }
                return existingButton;
            }

            Button newButton = new Button();
            newButton.Name = name;
            newButton.Text = defaultText;
            newButton.Click += clickHandler;
            this.Controls.Add(newButton);
            return newButton;
        }

        private void ApplyProfessionalStyles()
        {
            this.BackColor = ColorLightBg;
            grpDecision.Enabled = false;

            dgvApplications.BorderStyle = BorderStyle.None;
            dgvApplications.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 244, 247);
            dgvApplications.EnableHeadersVisualStyles = false;
            dgvApplications.ColumnHeadersDefaultCellStyle.BackColor = BrandPrimary;
            dgvApplications.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvApplications.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvApplications.GridColor = BorderColor;
        }

        private void LoadApplications()
        {
            try
            {
                using (OleDbConnection conn = DBConnection.GetConnection())
                {
                    if (conn == null) return;

                    string query = @"
                        SELECT App.ApplicationID,
                               App.ApplicantID,
                               Cand.FirstName & ' ' & Cand.LastName AS FullName,
                               Pos.PositionName AS JobTitle,
                               Dept.DepartmentName AS Department,
                               App.Status
                        FROM ((((Applications App
                        INNER JOIN Applicants Cand ON App.ApplicantID = Cand.ApplicantID)
                        INNER JOIN JobVacancies Job ON App.JobID = Job.JobID)
                        INNER JOIN Positions Pos ON Job.PositionID = Pos.PositionID)
                        INNER JOIN Departments Dept ON Job.DepartmentID = Dept.DepartmentID)
                        WHERE App.Status = 'Under Review'";

                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn))
                    {
                        dtApplications.Clear();
                        adapter.Fill(dtApplications);
                        dgvApplications.DataSource = dtApplications;

                        if (dgvApplications.Columns["ApplicationID"] != null)
                            dgvApplications.Columns["ApplicationID"].Visible = false;
                        if (dgvApplications.Columns["ApplicantID"] != null)
                            dgvApplications.Columns["ApplicantID"].Visible = false;
                        if (dgvApplications.Columns["Status"] != null)
                            dgvApplications.Columns["Status"].Visible = false;

                        if (dgvApplications.Columns["FullName"] != null)
                            dgvApplications.Columns["FullName"].HeaderText = "Applicant Name";
                        if (dgvApplications.Columns["JobTitle"] != null)
                            dgvApplications.Columns["JobTitle"].HeaderText = "Applied Position";
                        if (dgvApplications.Columns["Department"] != null)
                            dgvApplications.Columns["Department"].HeaderText = "Department";

                        dgvApplications.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading applications from database:\n" + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvApplications_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvApplications.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvApplications.SelectedRows[0];

                selectedApplicationID = Convert.ToInt32(row.Cells["ApplicationID"].Value);
                selectedApplicantID = Convert.ToInt32(row.Cells["ApplicantID"].Value);

                lblSelectedApplicant.Text = "Applicant:  " + row.Cells["FullName"].Value?.ToString();

                string jobTitle = row.Cells["JobTitle"].Value?.ToString() ?? "";
                string department = row.Cells["Department"].Value?.ToString() ?? "";

                List<string> missing = DatabaseHelper.GetMissingRequirementsForApplication(selectedApplicationID);
                if (missing != null && missing.Count > 0)
                {
                    lblSelectedJob.Text = $"Job: {jobTitle} ({department})  [⚠️ Missing: {string.Join(", ", missing)}]";
                    lblSelectedJob.ForeColor = ColorDanger;
                }
                else
                {
                    lblSelectedJob.Text = $"Job: {jobTitle} ({department})  [✅ All Docs Uploaded]";
                    lblSelectedJob.ForeColor = ColorSuccess;
                }

                LoadApplicantProfileDetails(selectedApplicantID);

                rdoQualified.Checked = true;
                txtRemarks.Clear();
                grpDecision.Enabled = true;
            }
            else
            {
                ResetDetailsView();
            }
        }

        private void LoadApplicantProfileDetails(int applicantId)
        {
            try
            {
                using (OleDbConnection conn = DBConnection.GetConnection())
                {
                    if (conn == null) return;
                    string query = "SELECT ContactNumber, Education, Skills, WorkExperience FROM Applicants WHERE ApplicantID = ?";

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("?", applicantId);
                        conn.Open();
                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtEducation.Text = reader["Education"] != DBNull.Value ? reader["Education"].ToString() : "Not Specified";
                                txtSkills.Text = reader["Skills"] != DBNull.Value ? reader["Skills"].ToString() : "Not Specified";
                                txtExperience.Text = reader["WorkExperience"] != DBNull.Value ? reader["WorkExperience"].ToString() : "Not Specified";
                                lblContactNo.Text = "Contact: " + (reader["ContactNumber"] != DBNull.Value ? reader["ContactNumber"].ToString() : "Not Specified");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error displaying applicant profile details:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ResetDetailsView()
        {
            selectedApplicationID = 0;
            selectedApplicantID = 0;
            lblSelectedApplicant.Text = "Applicant: —";
            lblSelectedJob.Text = "Job: —";
            lblSelectedJob.ForeColor = SystemColors.ControlText;
            lblContactNo.Text = "Contact: —";
            txtEducation.Clear();
            txtSkills.Clear();
            txtExperience.Clear();
            grpDecision.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (selectedApplicationID == 0)
            {
                MessageBox.Show("Please select an application from the list first.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtRemarks.Text))
            {
                MessageBox.Show("Please enter screening remarks explaining the decision.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string decision = rdoQualified.Checked ? "Qualified" : "Not Qualified";
            string newStatus = rdoQualified.Checked ? "Shortlisted" : "Rejected";

            DialogResult confirm = MessageBox.Show(
                $"Mark this applicant as '{decision}'?\n\nThe status will transition to '{newStatus}'.",
                "Confirm Screening Decision",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            using (OleDbConnection conn = DBConnection.GetConnection())
            {
                if (conn == null) return;
                OleDbTransaction transaction = null;

                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction();

                    // Step A: Insert Evaluation Record into ScreeningResults
                    string insertScreening = @"
                        INSERT INTO ScreeningResults (ApplicationID, ScreeningResult, Remarks, ScreenedBy, DateScreened) 
                        VALUES (?, ?, ?, ?, ?)";

                    using (OleDbCommand cmd = new OleDbCommand(insertScreening, conn, transaction))
                    {
                        cmd.Parameters.Add("?", OleDbType.Integer).Value = selectedApplicationID;
                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = decision;
                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = txtRemarks.Text.Trim();
                        cmd.Parameters.Add("?", OleDbType.Integer).Value = UserSession.UserID > 0 ? UserSession.UserID : 1;
                        cmd.Parameters.Add("?", OleDbType.Date).Value = DateTime.Now;
                        cmd.ExecuteNonQuery();
                    }

                    // Step B: Update application state in Applications
                    string updateApp = "UPDATE Applications SET [Status] = ? WHERE ApplicationID = ?";
                    using (OleDbCommand cmd = new OleDbCommand(updateApp, conn, transaction))
                    {
                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = newStatus;
                        cmd.Parameters.Add("?", OleDbType.Integer).Value = selectedApplicationID;
                        cmd.ExecuteNonQuery();
                    }

                    // Step C: Log state modification to ApplicationStatusHistory
                    string insertHistory = @"
                        INSERT INTO ApplicationStatusHistory (ApplicationID, [Status], DateChanged) 
                        VALUES (?, ?, ?)";

                    using (OleDbCommand cmd = new OleDbCommand(insertHistory, conn, transaction))
                    {
                        cmd.Parameters.Add("?", OleDbType.Integer).Value = selectedApplicationID;
                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = newStatus;
                        cmd.Parameters.Add("?", OleDbType.Date).Value = DateTime.Now;
                        cmd.ExecuteNonQuery();
                    }

                    // Step D: Dynamically map and write to the AuditTrail table depending on database schema
                    List<string> columns = new List<string>();
                    using (OleDbCommand schemaCmd = new OleDbCommand("SELECT TOP 1 * FROM AuditTrail", conn, transaction))
                    using (OleDbDataReader reader = schemaCmd.ExecuteReader())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            columns.Add(reader.GetName(i).ToLower());
                        }
                    }

                    List<string> insertCols = new List<string>();
                    List<string> valuePlaceholders = new List<string>();
                    List<object> values = new List<object>();
                    List<OleDbType> types = new List<OleDbType>();

                    // 1. Map UserID column variation
                    string matchedUser = null;
                    if (columns.Contains("userid")) matchedUser = "UserID";
                    else if (columns.Contains("user_id")) matchedUser = "User_ID";

                    if (matchedUser != null)
                    {
                        insertCols.Add($"[{matchedUser}]");
                        valuePlaceholders.Add("?");
                        values.Add(UserSession.UserID > 0 ? UserSession.UserID : 1);
                        types.Add(OleDbType.Integer);
                    }

                    // 2. Map Action / Details column variation
                    string matchedAction = null;
                    if (columns.Contains("action")) matchedAction = "Action";
                    else if (columns.Contains("actionperformed")) matchedAction = "ActionPerformed";
                    else if (columns.Contains("description")) matchedAction = "Description";
                    else if (columns.Contains("details")) matchedAction = "Details";
                    else if (columns.Contains("logmessage")) matchedAction = "LogMessage";

                    if (matchedAction != null)
                    {
                        insertCols.Add($"[{matchedAction}]");
                        valuePlaceholders.Add("?");
                        values.Add($"Completed screening evaluation for Application ID: {selectedApplicationID}. Result: {decision}.");
                        types.Add(OleDbType.VarWChar);
                    }

                    // 3. Map Date / Timestamp column variation
                    string matchedDate = null;
                    if (columns.Contains("actiondate")) matchedDate = "ActionDate";
                    else if (columns.Contains("logdate")) matchedDate = "LogDate";
                    else if (columns.Contains("datechanged")) matchedDate = "DateChanged";
                    else if (columns.Contains("datelogged")) matchedDate = "DateLogged";
                    else if (columns.Contains("timestamp")) matchedDate = "Timestamp";
                    else if (columns.Contains("logdatetime")) matchedDate = "LogDateTime";

                    if (matchedDate != null)
                    {
                        insertCols.Add($"[{matchedDate}]");
                        valuePlaceholders.Add("?");
                        values.Add(DateTime.Now);
                        types.Add(OleDbType.Date);
                    }

                    // 4. Map Table Name column variation
                    string matchedTable = null;
                    if (columns.Contains("tableaffected")) matchedTable = "TableAffected";
                    else if (columns.Contains("tablename")) matchedTable = "TableName";
                    else if (columns.Contains("table_affected")) matchedTable = "Table_Affected";

                    if (matchedTable != null)
                    {
                        insertCols.Add($"[{matchedTable}]");
                        valuePlaceholders.Add("?");
                        values.Add("Applications");
                        types.Add(OleDbType.VarWChar);
                    }

                    // 5. Map Record ID column variation
                    string matchedRecord = null;
                    if (columns.Contains("recordid")) matchedRecord = "RecordID";
                    else if (columns.Contains("record_id")) matchedRecord = "Record_ID";
                    else if (columns.Contains("record")) matchedRecord = "Record";

                    if (matchedRecord != null)
                    {
                        insertCols.Add($"[{matchedRecord}]");
                        valuePlaceholders.Add("?");
                        values.Add(selectedApplicationID);
                        types.Add(OleDbType.Integer);
                    }

                    // Execute the dynamic insert statement using only the matching columns found in your AuditTrail table
                    if (insertCols.Count > 0)
                    {
                        string sqlAudit = $"INSERT INTO AuditTrail ({string.Join(", ", insertCols)}) VALUES ({string.Join(", ", valuePlaceholders)})";
                        using (OleDbCommand cmd = new OleDbCommand(sqlAudit, conn, transaction))
                        {
                            for (int i = 0; i < values.Count; i++)
                            {
                                cmd.Parameters.Add("?", types[i]).Value = values[i];
                            }
                            cmd.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();

                    MessageBox.Show(
                        $"Screening successfully recorded!\n\nApplicant: {decision}\nStatus updated to: {newStatus}",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ResetDetailsView();
                    LoadApplications();
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        try { transaction.Rollback(); } catch { /* Fallback */ }
                    }
                    MessageBox.Show("Error processing evaluation. Changes rolled back.\n" + ex.Message,
                        "Database Transaction Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}