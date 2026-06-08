using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.OleDb;
using HRApplicantSystem.Database;
using HRApplicantSystem.Classes;
using System.Collections.Generic;
using System.IO;          // Added for File path handling
using System.Diagnostics; // Added for Process execution

namespace HRApplicantSystem.Forms.HR
{
    public partial class ApplicantReviewForm : Form
    {
        private int selectedApplicantID = 0;
        private int selectedApplicationID = 0;
        private string selectedApplicationStatus = "";
        private DataTable applicationsTable;

        // Programmatically created controls for the search bar
        private TextBox txtSearch;
        private Label lblSearch;

        // Programmatically created controls for the Document Validation Panel
        private GroupBox grpDocValidation;
        private ComboBox cmbDocStatus;
        private TextBox txtDocRemarks;
        private Button btnSaveDocValidation;

        // Programmatically created controls for the Back Button
        private Button btnBack;

        // Programmatically created textboxes for full Profile completeness
        private TextBox txtReviewMiddleName;
        private TextBox txtReviewBirthday;
        private TextBox txtReviewGender;
        private TextBox txtReviewAddress;
        private TextBox txtReviewWorkExperience;

        public ApplicantReviewForm()
        {
            InitializeComponent();

            // Programmatically configure initial form boundary metrics
            this.Size = new Size(1020, 620);
            this.StartPosition = FormStartPosition.CenterScreen;

            InitializeBackButton();
            EnsureAllProfileControlsExist();
        }

        private void ApplicantReviewForm_Load(object sender, EventArgs e)
        {
            InitializeSearchField();
            InitializeValidationPanel(); // Set up validation controls dynamically
            ConfigureGridViewStyle();
            LoadApplications();

            // Programmatically wire events to avoid designer file conflicts
            lstApplicantDocuments.DoubleClick += LstApplicantDocuments_DoubleClick;
            lstApplicantDocuments.SelectedIndexChanged += LstApplicantDocuments_SelectedIndexChanged;

            // Update the GroupBox title to guide the HR user on how to open files
            groupBox2.Text = "Documents (Double-click file to open)";

            ApplyModernStyles();
            AdjustControlsLayout();
        }

        /// <summary>
        /// Programmatically creates the Back button to keep designer files safe.
        /// </summary>
        private void InitializeBackButton()
        {
            Control[] matches = this.Controls.Find("btnBack", true);
            if (matches.Length > 0 && matches[0] is Button btn)
            {
                btnBack = btn;
                return;
            }

            btnBack = new Button
            {
                Name = "btnBack",
                Text = "Back"
            };
            btnBack.Click += btnBack_Click;
            this.Controls.Add(btnBack);
        }

        /// <summary>
        /// Clears residual designer labels to prevent clipping and verifies modern textboxes exist.
        /// </summary>
        private void EnsureAllProfileControlsExist()
        {
            // 1. Purge all residual designer labels inside groupBox1 to prevent overlapping ghost text
            List<Control> oldLabels = new List<Control>();
            foreach (Control c in groupBox1.Controls)
            {
                if (c is Label)
                {
                    oldLabels.Add(c);
                }
            }
            foreach (Control lbl in oldLabels)
            {
                groupBox1.Controls.Remove(lbl);
                lbl.Dispose();
            }

            // 2. Resolve read-only profile data fields
            txtReviewMiddleName = GetOrCreateTextBox("txtReviewMiddleName");
            txtReviewBirthday = GetOrCreateTextBox("txtReviewBirthday");
            txtReviewGender = GetOrCreateTextBox("txtReviewGender");
            txtReviewAddress = GetOrCreateTextBox("txtReviewAddress");
            txtReviewWorkExperience = GetOrCreateTextBox("txtReviewWorkExperience");

            // Set multiline attributes for wrap and scrollbars
            txtReviewAddress.Multiline = true;
            txtReviewAddress.ScrollBars = ScrollBars.Vertical;
            txtReviewWorkExperience.Multiline = true;
            txtReviewWorkExperience.ScrollBars = ScrollBars.Vertical;

            txtReviewEducation.Multiline = true;
            txtReviewEducation.ScrollBars = ScrollBars.Vertical;

            txtReviewSkills.Multiline = true;
            txtReviewSkills.ScrollBars = ScrollBars.Vertical;
        }

        private TextBox GetOrCreateTextBox(string name)
        {
            Control[] matches = groupBox1.Controls.Find(name, true);
            if (matches.Length > 0 && matches[0] is TextBox txt)
            {
                return txt;
            }

            TextBox newTxt = new TextBox();
            newTxt.Name = name;
            newTxt.ReadOnly = true;
            groupBox1.Controls.Add(newTxt);
            return newTxt;
        }

        private Label GetOrCreateLabel(string name, string text)
        {
            Control[] matches = groupBox1.Controls.Find(name, true);
            if (matches.Length > 0 && matches[0] is Label lbl)
            {
                return lbl;
            }

            Label newLbl = new Label();
            newLbl.Name = name;
            newLbl.Text = text;
            newLbl.AutoSize = true;
            groupBox1.Controls.Add(newLbl);
            return newLbl;
        }

        /// <summary>
        /// Programmatically creates and places a document validation panel to keep designer file clean.
        /// </summary>
        private void InitializeValidationPanel()
        {
            grpDocValidation = new GroupBox();
            grpDocValidation.Text = "Validate Selected Document";
            grpDocValidation.Enabled = false; // Disabled until a valid document is selected

            Label lblStatus = new Label();
            lblStatus.Text = "Status:";
            lblStatus.Location = new Point(15, 25);
            lblStatus.AutoSize = true;

            cmbDocStatus = new ComboBox();
            cmbDocStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDocStatus.Items.AddRange(new object[] { "Submitted", "Approved", "Rejected" });
            cmbDocStatus.Location = new Point(15, 45);
            cmbDocStatus.Width = 120;

            Label lblRemarks = new Label();
            lblRemarks.Text = "HR Remarks:";
            lblRemarks.Location = new Point(150, 25);
            lblRemarks.AutoSize = true;

            txtDocRemarks = new TextBox();
            txtDocRemarks.Location = new Point(150, 45);

            btnSaveDocValidation = new Button();
            btnSaveDocValidation.Text = "Save Validation";
            btnSaveDocValidation.Click += btnSaveDocValidation_Click;

            grpDocValidation.Controls.Add(lblStatus);
            grpDocValidation.Controls.Add(cmbDocStatus);
            grpDocValidation.Controls.Add(lblRemarks);
            grpDocValidation.Controls.Add(txtDocRemarks);
            grpDocValidation.Controls.Add(btnSaveDocValidation);

            this.Controls.Add(grpDocValidation);
        }

        /// <summary>
        /// Automatically scales and positions all grid, profile, list, and action panels.
        /// </summary>
        private void AdjustControlsLayout()
        {
            int clientWidth = this.ClientSize.Width;
            int clientHeight = this.ClientSize.Height;

            int margin = 20;
            int topOffset = 90; // Expanded to clear search title safely

            // Compute split-pane dimensions dynamically
            int leftColWidth = (int)(clientWidth * 0.40);
            int rightColWidth = clientWidth - leftColWidth - (3 * margin);

            int leftX = margin;
            int rightX = leftX + leftColWidth + margin;

            // 1. Arrange Header Search Bar and overlap-safe Title
            Label titleLabel = null;
            foreach (Control c in this.Controls)
            {
                if (c is Label && (c.Text.ToUpper().Contains("PROFILE") || c.Text.ToUpper().Contains("REVIEW") || c.Name.ToLower().Contains("title") || c.Name.ToLower().Contains("header")))
                {
                    if (c.Name != "lblCompletenessCard" && c.Name != "lblSearch")
                    {
                        titleLabel = (Label)c;
                        break;
                    }
                }
            }

            if (titleLabel != null)
            {
                titleLabel.Location = new Point(leftX, 15);
                titleLabel.AutoSize = true;
                titleLabel.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
                titleLabel.ForeColor = Color.FromArgb(44, 62, 80);
            }

            if (lblSearch != null && txtSearch != null)
            {
                lblSearch.Location = new Point(leftX, 58);
                txtSearch.Location = new Point(leftX + 85, 55);
                txtSearch.Width = leftColWidth - 85;
            }

            // 2. Position Master Applications Grid and Core Review Action Button
            int dgvHeight = clientHeight - topOffset - margin - 55;
            dgvApplications.Location = new Point(leftX, topOffset);
            dgvApplications.Size = new Size(leftColWidth, dgvHeight);

            btnLockApplication.Location = new Point(leftX, dgvApplications.Bottom + 12);
            btnLockApplication.Size = new Size(leftColWidth, 34);

            // 3. Arrange Right Column - Profile Details Container (groupBox1)
            int availRightHeight = clientHeight - topOffset - margin;
            int profileBoxHeight = (int)(availRightHeight * 0.45);

            groupBox1.Location = new Point(rightX, topOffset);
            groupBox1.Size = new Size(rightColWidth, profileBoxHeight);

            LayoutProfileReviewFields(rightColWidth, profileBoxHeight);

            // 4. Arrange Right Column - Documents List Container (groupBox2)
            int docBoxHeight = (int)(availRightHeight * 0.28);
            groupBox2.Location = new Point(rightX, groupBox1.Bottom + 10);
            groupBox2.Size = new Size(rightColWidth, docBoxHeight);

            if (lstApplicantDocuments != null)
            {
                lstApplicantDocuments.Location = new Point(12, 22);
                lstApplicantDocuments.Size = new Size(rightColWidth - 24, docBoxHeight - 34);
            }

            // 5. Arrange Right Column - Programmatic Document Validation Container (grpDocValidation)
            int valBoxHeight = availRightHeight - profileBoxHeight - docBoxHeight - 20;
            if (grpDocValidation != null)
            {
                grpDocValidation.Location = new Point(rightX, groupBox2.Bottom + 10);
                grpDocValidation.Size = new Size(rightColWidth, valBoxHeight);

                // Re-align internal controls inside validation groupbox dynamically
                int valInnerWidth = rightColWidth - 30;
                int valSplitX = (int)(valInnerWidth * 0.40);

                cmbDocStatus.Width = valSplitX;
                txtDocRemarks.Location = new Point(15 + valSplitX + 15, 45);
                txtDocRemarks.Width = valInnerWidth - valSplitX - 15;

                // Adjust labels
                foreach (Control c in grpDocValidation.Controls)
                {
                    if (c is Label && c.Text.Contains("HR Remarks"))
                    {
                        c.Location = new Point(txtDocRemarks.Left, 25);
                    }
                }

                btnSaveDocValidation.Size = new Size(txtDocRemarks.Width, 30);
                btnSaveDocValidation.Location = new Point(txtDocRemarks.Left, 80);
            }

            // 6. Layout Symmetrical Back Button horizontally aligned with Lock Application
            if (btnBack != null)
            {
                btnBack.Size = new Size(110, 34);
                btnBack.Location = new Point((rightX + rightColWidth) - 110, dgvApplications.Bottom + 12);
            }
        }

        /// <summary>
        /// Programmatically arranges all 10 Profile Fields inside groupBox1.
        /// </summary>
        private void LayoutProfileReviewFields(int width, int height)
        {
            int margin = 15;
            int innerWidth = width - (3 * margin);
            int colWidth = innerWidth / 2;

            int leftX = margin;
            int rightX = margin + colWidth + margin;

            // Left Column (6 single-line fields)
            int leftItemsCount = 6;
            int gapYLeft = (height - 35) / leftItemsCount;
            int leftY = 32;

            PositionFieldInGroupBox("lblReviewFirstName", txtReviewFirstName, "First Name:", leftX, leftY, colWidth, 24);
            PositionFieldInGroupBox("lblReviewMiddleName", txtReviewMiddleName, "Middle Name:", leftX, leftY += gapYLeft, colWidth, 24);
            PositionFieldInGroupBox("lblReviewLastName", txtReviewLastName, "Last Name:", leftX, leftY += gapYLeft, colWidth, 24);
            PositionFieldInGroupBox("lblReviewBirthday", txtReviewBirthday, "Birthday:", leftX, leftY += gapYLeft, colWidth, 24);
            PositionFieldInGroupBox("lblReviewGender", txtReviewGender, "Gender:", leftX, leftY += gapYLeft, colWidth, 24);
            PositionFieldInGroupBox("lblReviewContact", txtReviewContact, "Contact Number:", leftX, leftY += gapYLeft, colWidth, 24);

            // Right Column (4 multiline fields)
            int rightItemsCount = 4;
            int gapYRight = (height - 35) / rightItemsCount;
            int rightY = 32;
            int multilineHeight = gapYRight - 24;

            PositionFieldInGroupBox("lblReviewAddress", txtReviewAddress, "Address:", rightX, rightY, colWidth, multilineHeight);
            PositionFieldInGroupBox("lblReviewEducation", txtReviewEducation, "Education:", rightX, rightY += gapYRight, colWidth, multilineHeight);
            PositionFieldInGroupBox("lblReviewSkills", txtReviewSkills, "Skills:", rightX, rightY += gapYRight, colWidth, multilineHeight);
            PositionFieldInGroupBox("lblReviewWorkExperience", txtReviewWorkExperience, "Work Experience:", rightX, rightY += gapYRight, colWidth, multilineHeight);
        }

        private void PositionFieldInGroupBox(string labelName, Control ctrl, string labelText, int x, int y, int width, int height)
        {
            ctrl.Location = new Point(x, y);
            ctrl.Size = new Size(width, height);

            Label lbl = GetOrCreateLabel(labelName, labelText);
            lbl.Location = new Point(x, y - 18);
            lbl.BringToFront();
            ctrl.BringToFront();
        }

        /// <summary>
        /// Activates validation panel and loads existing remarks when a valid document is selected.
        /// </summary>
        private void LstApplicantDocuments_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstApplicantDocuments.SelectedItem == null)
            {
                grpDocValidation.Enabled = false;
                return;
            }

            string selectedFileName = lstApplicantDocuments.SelectedItem.ToString();

            // Guard against selection headers, info blocks, and placeholders
            if (selectedFileName.StartsWith("===") ||
                selectedFileName.StartsWith("⚠️") ||
                selectedFileName.StartsWith("(") ||
                selectedFileName == "(No documents uploaded yet)" ||
                selectedFileName == "None! All mandatory requirements uploaded.")
            {
                grpDocValidation.Enabled = false;
                cmbDocStatus.SelectedIndex = -1;
                txtDocRemarks.Clear();
                return;
            }

            grpDocValidation.Enabled = true;
            LoadSelectedDocumentValidation(selectedFileName);
        }

        private void LoadSelectedDocumentValidation(string fileName)
        {
            using (OleDbConnection con = DBConnection.GetConnection())
            {
                if (con == null) return;
                try
                {
                    con.Open();
                    string query = "SELECT Status, Remarks FROM ApplicantDocuments WHERE ApplicantID = ? AND DocumentName = ?";
                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("?", selectedApplicantID);
                        cmd.Parameters.AddWithValue("?", fileName);
                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                cmbDocStatus.Text = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : "Submitted";
                                txtDocRemarks.Text = reader["Remarks"] != DBNull.Value ? reader["Remarks"].ToString() : "";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading document status: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSaveDocValidation_Click(object sender, EventArgs e)
        {
            if (lstApplicantDocuments.SelectedItem == null) return;
            string selectedFileName = lstApplicantDocuments.SelectedItem.ToString();

            if (cmbDocStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a validation status (Approved or Rejected).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (OleDbConnection con = DBConnection.GetConnection())
            {
                if (con == null) return;
                try
                {
                    con.Open();
                    string query = "UPDATE ApplicantDocuments SET Status = ?, Remarks = ? WHERE ApplicantID = ? AND DocumentName = ?";
                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("?", cmbDocStatus.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("?", txtDocRemarks.Text.Trim());
                        cmd.Parameters.AddWithValue("?", selectedApplicantID);
                        cmd.Parameters.AddWithValue("?", selectedFileName);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show($"Document '{selectedFileName}' validation saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Refresh documents display
                    LoadDocuments();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving document validation: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Programmatically opens the selected document from the applicant-specific subfolder.
        /// </summary>
        private void LstApplicantDocuments_DoubleClick(object sender, EventArgs e)
        {
            if (lstApplicantDocuments.SelectedItem == null) return;

            string selectedFileName = lstApplicantDocuments.SelectedItem.ToString();

            // Guard against selection headers, info blocks, and placeholders
            if (selectedFileName.StartsWith("===") ||
                selectedFileName.StartsWith("⚠️") ||
                selectedFileName.StartsWith("(") ||
                selectedFileName == "No documents submitted." ||
                selectedFileName == "None! All mandatory requirements uploaded.")
            {
                return;
            }

            // Construct the path to the document inside the applicant's subfolder
            string uploadsDirectory = Path.Combine(Application.StartupPath, "UploadedDocuments");
            string filePath = Path.Combine(uploadsDirectory, selectedApplicantID.ToString(), selectedFileName);

            try
            {
                if (File.Exists(filePath))
                {
                    Process.Start(new ProcessStartInfo(filePath)
                    {
                        UseShellExecute = true // Ensures compatibility across operating system layers
                    });
                }
                else
                {
                    MessageBox.Show(
                        $"The physical file could not be found locally.\n\nExpected Location:\n{filePath}",
                        "File Not Found",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open the selected document: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Programmatically creates and places a search bar on the form to avoid breaking the designer file.
        /// </summary>
        private void InitializeSearchField()
        {
            lblSearch = new Label();
            lblSearch.Text = "Search Grid:";
            lblSearch.AutoSize = true;

            txtSearch = new TextBox();
            txtSearch.TextChanged += TxtSearch_TextChanged;

            this.Controls.Add(lblSearch);
            this.Controls.Add(txtSearch);
        }

        /// <summary>
        /// Instantly filters the loaded grid data on the client side across multiple columns.
        /// </summary>
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            if (applicationsTable != null)
            {
                string filterText = txtSearch.Text.Replace("'", "''"); // Safe escape for quotes

                // Dynamic filter on applicant names, job title, and status
                applicationsTable.DefaultView.RowFilter = string.Format(
                    "FirstName LIKE '%{0}%' OR LastName LIKE '%{0}%' OR JobTitle LIKE '%{0}%' OR Status LIKE '%{0}%'",
                    filterText);

                ApplyRowColoring();
            }
        }

        /// <summary>
        /// Sets professional formatting options for the grid.
        /// </summary>
        private void ConfigureGridViewStyle()
        {
            dgvApplications.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvApplications.MultiSelect = false;
            dgvApplications.RowHeadersVisible = false;
            dgvApplications.AllowUserToAddRows = false;
            dgvApplications.ReadOnly = true;
            dgvApplications.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvApplications.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 250);
        }

        /// <summary>
        /// Loads applications from the database, displaying ONLY those with 'Submitted' status.
        /// </summary>
        private void LoadApplications()
        {
            try
            {
                using (OleDbConnection con = DBConnection.GetConnection())
                {
                    if (con == null) return;
                    con.Open();

                    // Query corrected to resolve Jobs to their Position titles in Positions table
                    string query = @"
                        SELECT
                            Applications.ApplicationID,
                            Applicants.ApplicantID,
                            Applicants.FirstName,
                            Applicants.LastName,
                            Positions.PositionName AS JobTitle,
                            Applications.Status
                        FROM
                            (((Applications
                            INNER JOIN Applicants ON Applications.ApplicantID = Applicants.ApplicantID)
                            INNER JOIN JobVacancies ON Applications.JobID = JobVacancies.JobID)
                            INNER JOIN Positions ON JobVacancies.PositionID = Positions.PositionID)
                        WHERE
                            Applications.Status = 'Submitted'";

                    using (OleDbDataAdapter da = new OleDbDataAdapter(query, con))
                    {
                        applicationsTable = new DataTable();
                        da.Fill(applicationsTable);
                        dgvApplications.DataSource = applicationsTable;

                        if (dgvApplications.Columns.Contains("ApplicantID"))
                            dgvApplications.Columns["ApplicantID"].Visible = false;

                        ApplyRowColoring();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading applications: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Dynamically highlights rows based on status.
        /// </summary>
        private void ApplyRowColoring()
        {
            foreach (DataGridViewRow row in dgvApplications.Rows)
            {
                if (row.Cells["Status"].Value != null)
                {
                    string status = row.Cells["Status"].Value.ToString();
                    if (status == "Under Review" || status == "Locked")
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(254, 243, 199);
                        row.DefaultCellStyle.ForeColor = Color.FromArgb(146, 64, 14);
                    }
                    else if (status == "Submitted")
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(219, 234, 254);
                        row.DefaultCellStyle.ForeColor = Color.FromArgb(30, 58, 138);
                    }
                    else if (status == "Withdrawn")
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(243, 244, 246);
                        row.DefaultCellStyle.ForeColor = Color.FromArgb(107, 114, 128);
                    }
                }
            }
        }

        /// <summary>
        /// Loads the profile information of the selected applicant.
        /// </summary>
        private void LoadProfile()
        {
            if (selectedApplicantID == 0) return;

            try
            {
                using (OleDbConnection con = DBConnection.GetConnection())
                {
                    if (con == null) return;
                    con.Open();

                    string query = "SELECT FirstName, MiddleName, LastName, Birthday, Gender, Address, ContactNumber, Education, Skills, WorkExperience " +
                                   "FROM Applicants WHERE ApplicantID = ?";

                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.Add("@ApplicantID", OleDbType.Integer).Value = selectedApplicantID;

                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtReviewFirstName.Text = reader["FirstName"]?.ToString() ?? "";
                                txtReviewMiddleName.Text = reader["MiddleName"]?.ToString() ?? "";
                                txtReviewLastName.Text = reader["LastName"]?.ToString() ?? "";

                                if (reader["Birthday"] != DBNull.Value && reader["Birthday"] != null)
                                {
                                    txtReviewBirthday.Text = Convert.ToDateTime(reader["Birthday"]).ToString("MM/dd/yyyy");
                                }
                                else
                                {
                                    txtReviewBirthday.Text = "—";
                                }

                                txtReviewGender.Text = reader["Gender"]?.ToString() ?? "";
                                txtReviewAddress.Text = reader["Address"]?.ToString() ?? "";
                                txtReviewContact.Text = reader["ContactNumber"] != DBNull.Value ? reader["ContactNumber"].ToString() : "";
                                txtReviewEducation.Text = reader["Education"] != DBNull.Value ? reader["Education"].ToString() : "";
                                txtReviewSkills.Text = reader["Skills"] != DBNull.Value ? reader["Skills"].ToString() : "";
                                txtReviewWorkExperience.Text = reader["WorkExperience"] != DBNull.Value ? reader["WorkExperience"].ToString() : "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading profile: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Loads documents and lists missing mandatory requirements inside the listbox.
        /// Filters uploaded documents to match ONLY the selected vacancy's requirements.
        /// </summary>
        private void LoadDocuments()
        {
            lstApplicantDocuments.Items.Clear();
            if (selectedApplicantID == 0 || selectedApplicationID == 0) return;

            try
            {
                // 1. Fetch the RequiredDocuments CSV string for the selected Application ID
                List<int> requiredIds = new List<int>();
                using (OleDbConnection con = DBConnection.GetConnection())
                {
                    if (con == null) return;
                    con.Open();

                    string jobQuery = @"SELECT j.RequiredDocuments 
                                        FROM Applications a 
                                        INNER JOIN JobVacancies j ON a.JobID = j.JobID 
                                        WHERE a.ApplicationID = ?";
                    using (OleDbCommand jobCmd = new OleDbCommand(jobQuery, con))
                    {
                        jobCmd.Parameters.AddWithValue("?", selectedApplicationID);
                        object result = jobCmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            string csv = result.ToString();
                            string[] tokens = csv.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string t in tokens)
                            {
                                if (int.TryParse(t.Trim(), out int id))
                                {
                                    requiredIds.Add(id);
                                }
                            }
                        }
                    }
                }

                // 2. Group 1: Uploaded Files (filtered to match only this specific job vacancy's requirements)
                lstApplicantDocuments.Items.Add("=== Uploaded Documents ===");
                int uploadedCount = 0;

                using (OleDbConnection con = DBConnection.GetConnection())
                {
                    if (con == null) return;
                    con.Open();

                    string query = "SELECT DocumentName, RequirementTypeID FROM ApplicantDocuments WHERE ApplicantID = ?";

                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.Add("@ApplicantID", OleDbType.Integer).Value = selectedApplicantID;

                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int reqTypeId = Convert.ToInt32(reader["RequirementTypeID"]);

                                // Only display this file if it belongs to the target vacancy's requirements
                                if (requiredIds.Contains(reqTypeId))
                                {
                                    lstApplicantDocuments.Items.Add(reader["DocumentName"].ToString());
                                    uploadedCount++;
                                }
                            }
                        }
                    }
                }

                if (uploadedCount == 0)
                {
                    lstApplicantDocuments.Items.Add("(No documents uploaded yet)");
                }

                // 3. Group 2: Missing Requirements
                lstApplicantDocuments.Items.Add("");
                lstApplicantDocuments.Items.Add("=== Missing Requirements ===");

                List<string> missing = DatabaseHelper.GetMissingRequirementsForApplication(selectedApplicationID);
                if (missing == null || missing.Count == 0)
                {
                    lstApplicantDocuments.Items.Add("None! All mandatory requirements uploaded.");
                }
                else
                {
                    foreach (string item in missing)
                    {
                        lstApplicantDocuments.Items.Add("⚠️ " + item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading documents: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Locks the application to 'Under Review', logs to history, and records audit trails within one transaction.
        /// </summary>
        private void LockApplication()
        {
            OleDbConnection con = DBConnection.GetConnection();
            if (con == null) return;

            OleDbTransaction transaction = null;

            try
            {
                con.Open();
                transaction = con.BeginTransaction();

                // 1. Update Application status to 'Under Review'
                string updateQuery = "UPDATE [Applications] SET [Status] = 'Under Review' WHERE [ApplicationID] = ?";
                using (OleDbCommand cmd = new OleDbCommand(updateQuery, con, transaction))
                {
                    cmd.Parameters.Add("@ApplicationID", OleDbType.Integer).Value = selectedApplicationID;
                    cmd.ExecuteNonQuery();
                }

                // 2. Insert into ApplicationStatusHistory
                string historyQuery = @"
                    INSERT INTO [ApplicationStatusHistory] ([ApplicationID], [Status], [DateChanged]) 
                    VALUES (?, ?, ?)";
                using (OleDbCommand cmd = new OleDbCommand(historyQuery, con, transaction))
                {
                    cmd.Parameters.Add("@ApplicationID", OleDbType.Integer).Value = selectedApplicationID;
                    cmd.Parameters.Add("@Status", OleDbType.VarWChar).Value = "Under Review";
                    cmd.Parameters.Add("@DateChanged", OleDbType.Date).Value = DateTime.Now;
                    cmd.ExecuteNonQuery();
                }

                // 3. Insert into AuditTrail
                string auditQuery = @"
                    INSERT INTO [AuditTrail] ([Action], [DateCreated], [UserID]) 
                    VALUES (?, ?, ?)";
                using (OleDbCommand cmd = new OleDbCommand(auditQuery, con, transaction))
                {
                    cmd.Parameters.Add("@Action", OleDbType.VarWChar).Value = $"Locked Application ID {selectedApplicationID} for review.";
                    cmd.Parameters.Add("@DateCreated", OleDbType.Date).Value = DateTime.Now;

                    // Fallback to ID 1 if tested locally outside of standard user login sessions
                    int activeUserId = UserSession.UserID > 0 ? UserSession.UserID : 1;
                    cmd.Parameters.Add("@UserID", OleDbType.Integer).Value = activeUserId;

                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();

                MessageBox.Show("Application locked under review successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadApplications();
                ResetSelection();
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    try { transaction.Rollback(); } catch { }
                }

                MessageBox.Show("Failed to process transaction: " + ex.Message, "Transaction Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        private void ResetSelection()
        {
            selectedApplicationID = 0;
            selectedApplicantID = 0;
            selectedApplicationStatus = "";
            btnLockApplication.Enabled = false;

            txtReviewFirstName.Clear();
            txtReviewMiddleName.Clear();
            txtReviewLastName.Clear();
            txtReviewBirthday.Clear();
            txtReviewGender.Clear();
            txtReviewAddress.Clear();
            txtReviewContact.Clear();
            txtReviewEducation.Clear();
            txtReviewSkills.Clear();
            txtReviewWorkExperience.Clear();
            lstApplicantDocuments.Items.Clear();

            grpDocValidation.Enabled = false;
            cmbDocStatus.SelectedIndex = -1;
            txtDocRemarks.Clear();
        }

        // --- Event Handlers & Designer Fallback Methods ---

        private void dgvApplications_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvApplications.Rows[e.RowIndex];

                selectedApplicationID = Convert.ToInt32(row.Cells["ApplicationID"].Value);
                selectedApplicantID = Convert.ToInt32(row.Cells["ApplicantID"].Value);
                selectedApplicationStatus = row.Cells["Status"].Value != DBNull.Value ? row.Cells["Status"].Value.ToString() : "";

                // Status Trap Logic: Enable locking ONLY if the application status is currently "Submitted"
                btnLockApplication.Enabled = (selectedApplicationStatus == "Submitted");

                // Auto-loading updates profile and documents list dynamically on selection
                LoadProfile();
                LoadDocuments();
            }
        }

        private void btnLockApplication_Click(object sender, EventArgs e)
        {
            if (selectedApplicationID == 0)
            {
                MessageBox.Show("Please select an application first.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Status Trap Logic: Ensure we block actions on withdrawn or draft applications
            if (selectedApplicationStatus == "Withdrawn")
            {
                MessageBox.Show("This application has been withdrawn by the applicant and cannot be locked for review.",
                                "Action Blocked", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedApplicationStatus == "Draft")
            {
                MessageBox.Show("This application is still a Draft and has not been officially submitted yet.",
                                "Action Blocked", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedApplicationStatus != "Submitted")
            {
                MessageBox.Show($"This application cannot be locked because its current status is '{selectedApplicationStatus}'.",
                                "Action Blocked", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Lock this application for review? The applicant will no longer be allowed to modify submitted details.",
                "Confirm Start Review",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                LockApplication();
            }
        }

        private void ApplyModernStyles()
        {
            this.BackColor = Color.FromArgb(244, 246, 249); // Clean backplane gray
            this.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);

            // Style groupboxes
            StyleGroupBox(groupBox1);
            StyleGroupBox(groupBox2);
            if (grpDocValidation != null) StyleGroupBox(grpDocValidation);

            // Style primary control buttons
            StylePrimaryButton(btnLockApplication, Color.FromArgb(41, 128, 185)); // Deep Blue
            if (btnSaveDocValidation != null) StylePrimaryButton(btnSaveDocValidation, Color.FromArgb(39, 174, 96)); // Success Green
            if (btnBack != null) StyleSecondaryButton(btnBack);

            // Style search bar
            if (lblSearch != null)
            {
                lblSearch.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                lblSearch.ForeColor = Color.FromArgb(52, 73, 94);
            }

            // Style documents listbox
            lstApplicantDocuments.BorderStyle = BorderStyle.None;
            lstApplicantDocuments.BackColor = Color.White;
            lstApplicantDocuments.ForeColor = Color.FromArgb(44, 62, 80);
            lstApplicantDocuments.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);

            // Format standard input boxes within the Profile details container
            foreach (Control c in groupBox1.Controls)
            {
                if (c is TextBox txt)
                {
                    txt.BackColor = Color.FromArgb(248, 249, 250);
                    txt.BorderStyle = BorderStyle.FixedSingle;
                    txt.Font = new Font("Segoe UI", 9.25F, FontStyle.Regular);
                    txt.ForeColor = Color.FromArgb(44, 62, 80);
                    txt.ReadOnly = true;
                }
                else if (c is Label lbl)
                {
                    lbl.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
                    lbl.ForeColor = Color.FromArgb(127, 140, 141);
                }
            }
        }

        private void StyleGroupBox(GroupBox grp)
        {
            grp.FlatStyle = FlatStyle.Flat;
            grp.ForeColor = Color.FromArgb(44, 62, 80);
            grp.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        }

        private void StylePrimaryButton(Button btn, Color bg)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = bg;
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
        }

        private void StyleSecondaryButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = Color.FromArgb(127, 140, 141);
            btn.BackColor = Color.White;
            btn.ForeColor = Color.FromArgb(44, 62, 80);
            btn.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Silent preservation of original designer events to prevent errors
        private void dgvApplications_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void groupBox1_Enter(object sender, EventArgs e) { }
        private void lblTitle_Click(object sender, EventArgs e) { }
        private void groupBox2_Enter(object sender, EventArgs e) { }
    }
}