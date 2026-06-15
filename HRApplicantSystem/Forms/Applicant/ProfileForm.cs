using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;
using HRApplicantSystem.Classes;
using HRApplicantSystem.Database;

namespace HRApplicantSystem.Forms.Applicant
{
    public partial class ProfileForm : Form
    {
        // Tracks whether this is a new profile insertion or an update to an existing one
        private bool isNewProfile = true;

        // Dynamic completeness banner control
        private Label lblCompletenessCard;

        public ProfileForm()
        {
            InitializeComponent();
            InitializeCompletenessBanner();
            ApplyModernUI(); // Programmatically apply clean, modern styles to the form controls

            // Populate Gender dropdown list
            cmbGender.Items.Clear();
            cmbGender.Items.AddRange(new object[] { "Male", "Female", "Other" });
            cmbGender.SelectedIndex = 0;

            dtpBirthday.MaxDate = DateTime.Today;
        }

        private void ProfileForm_Load(object sender, EventArgs e)
        {
            LoadApplicantProfile();
            HookCompletenessTracker();
            AdjustControlsLayout();
        }

        /// <summary>
        /// Instantiates the profile completeness banner programmatically at the top of the form.
        /// </summary>
        private void InitializeCompletenessBanner()
        {
            Control[] bannerMatches = this.Controls.Find("lblCompletenessCard", true);
            if (bannerMatches.Length > 0)
            {
                lblCompletenessCard = (Label)bannerMatches[0];
            }
            else
            {
                lblCompletenessCard = new Label
                {
                    Name = "lblCompletenessCard",
                    Text = "   🔄 Calculating profile completeness...",
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    BorderStyle = BorderStyle.None,
                    FlatStyle = FlatStyle.Flat,
                    Padding = new Padding(12, 0, 12, 0) // Clean internal margins
                };

                this.Controls.Add(lblCompletenessCard);
            }
        }

        /// <summary>
        /// Recalculates and displays the profile completion score based on populated fields.
        /// </summary>
        private void UpdateProfileCompleteness()
        {
            int filledCount = 0;
            int totalFields = 8;

            if (!string.IsNullOrWhiteSpace(txtFirstName.Text)) filledCount++;
            if (!string.IsNullOrWhiteSpace(txtLastName.Text)) filledCount++;
            if (!string.IsNullOrWhiteSpace(txtAddress.Text)) filledCount++;
            if (!string.IsNullOrWhiteSpace(txtContactNumber.Text)) filledCount++;
            if (!string.IsNullOrWhiteSpace(txtEducation.Text)) filledCount++;
            if (!string.IsNullOrWhiteSpace(txtSkills.Text)) filledCount++;
            if (!string.IsNullOrWhiteSpace(txtWorkExperience.Text)) filledCount++;

            // Birthday is counted as complete if altered from today's date
            if (dtpBirthday.Value.Date != DateTime.Today) filledCount++;

            double percentage = (filledCount / (double)totalFields) * 100;

            if (lblCompletenessCard != null)
            {
                if (percentage >= 100)
                {
                    lblCompletenessCard.Text = "✅ Your professional profile is complete and ready for application reviews!";
                    lblCompletenessCard.BackColor = Color.FromArgb(223, 240, 216); // Soft Corporate Green
                    lblCompletenessCard.ForeColor = Color.FromArgb(60, 118, 61);  // Dark Corporate Green
                }
                else
                {
                    lblCompletenessCard.Text = $"⚠️ Profile Completeness: {percentage:0}% — Please fill in all fields ({filledCount}/{totalFields} completed) for HR evaluation.";
                    lblCompletenessCard.BackColor = Color.FromArgb(252, 248, 227); // Soft Corporate Amber
                    lblCompletenessCard.ForeColor = Color.FromArgb(138, 109, 59);  // Dark Corporate Amber
                }
            }
        }

        /// <summary>
        /// Registers key input and value events to dynamically update completion indicators on the fly.
        /// </summary>
        private void HookCompletenessTracker()
        {
            txtFirstName.TextChanged += (s, e) => UpdateProfileCompleteness();
            txtLastName.TextChanged += (s, e) => UpdateProfileCompleteness();
            txtAddress.TextChanged += (s, e) => UpdateProfileCompleteness();
            txtContactNumber.TextChanged += (s, e) => UpdateProfileCompleteness();
            txtEducation.TextChanged += (s, e) => UpdateProfileCompleteness();
            txtSkills.TextChanged += (s, e) => UpdateProfileCompleteness();
            txtWorkExperience.TextChanged += (s, e) => UpdateProfileCompleteness();
            dtpBirthday.ValueChanged += (s, e) => UpdateProfileCompleteness();
        }

        /// <summary>
        /// Automatically scales columns and layout padding dynamically using the window's ClientSize.
        /// </summary>
        private void AdjustControlsLayout()
        {
            int clientWidth = this.ClientSize.Width;
            int clientHeight = this.ClientSize.Height;

            int margin = 40;
            int topOffset = 85;
            int bottomMargin = 65;

            int totalWidth = clientWidth - (2 * margin);
            int columnGap = 40;
            int colWidth = (totalWidth - columnGap) / 2;

            int leftX = margin;
            int rightX = margin + colWidth + columnGap;

            // 1. Dynamic Header Positioning & Title-Overlap Resolution
            Label titleLabel = null;
            foreach (Control c in this.Controls)
            {
                if (c is Label && (c.Text.ToUpper().Contains("PROFILE") || c.Name.ToLower().Contains("title") || c.Name.ToLower().Contains("header")))
                {
                    if (c.Name != "lblCompletenessCard")
                    {
                        titleLabel = (Label)c;
                        break;
                    }
                }
            }

            if (titleLabel != null)
            {
                titleLabel.Location = new Point(margin, 20);
                titleLabel.AutoSize = true;
                titleLabel.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
                titleLabel.ForeColor = Color.FromArgb(27, 38, 59);

                // Place the completeness banner exactly where the header text finishes
                int bannerX = titleLabel.Right + 15;
                int bannerWidth = (clientWidth - margin) - bannerX;

                if (lblCompletenessCard != null)
                {
                    lblCompletenessCard.Location = new Point(bannerX, 20);
                    lblCompletenessCard.Size = new Size(bannerWidth, titleLabel.Height);
                }
                topOffset = titleLabel.Top + titleLabel.Height + 35; // Dynamically push grids down
            }
            else
            {
                if (lblCompletenessCard != null)
                {
                    lblCompletenessCard.Location = new Point(margin, 20);
                    lblCompletenessCard.Size = new Size(totalWidth, 36);
                    topOffset = lblCompletenessCard.Bottom + 35;
                }
            }

            // Calculate vertical grid limits
            int buttonsTop = clientHeight - bottomMargin;
            int availHeight = buttonsTop - topOffset - 20;

            // 2. Align Left Column (6 Personal Information fields)
            int leftItemsCount = 6;
            int gapYLeft = availHeight / leftItemsCount;
            int leftY = topOffset;

            PositionField("lblFirstName", txtFirstName, leftX, leftY, colWidth, 28);
            PositionField("lblMiddleName", txtMiddleName, leftX, leftY += gapYLeft, colWidth, 28);
            PositionField("lblLastName", txtLastName, leftX, leftY += gapYLeft, colWidth, 28);
            PositionField("lblBirthday", dtpBirthday, leftX, leftY += gapYLeft, colWidth, 28);
            PositionField("lblGender", cmbGender, leftX, leftY += gapYLeft, colWidth, 28);
            PositionField("lblContactNumber", txtContactNumber, leftX, leftY += gapYLeft, colWidth, 28);

            // 3. Align Right Column (4 dynamic multiline boxes spanning complete horizontal width)
            int rightItemsCount = 4;
            int gapYRight = availHeight / rightItemsCount;
            int rightY = topOffset;
            int multilineHeight = gapYRight - 28;

            PositionField("lblAddress", txtAddress, rightX, rightY, colWidth, multilineHeight);
            PositionField("lblEducation", txtEducation, rightX, rightY += gapYRight, colWidth, multilineHeight);
            PositionField("lblSkills", txtSkills, rightX, rightY += gapYRight, colWidth, multilineHeight);
            PositionField("lblWorkExperience", txtWorkExperience, rightX, rightY += gapYRight, colWidth, multilineHeight);

            // 4. Align Bottom Action Buttons
            int btnWidth = 130;
            int btnHeight = 35;

            btnSave.Size = new Size(btnWidth, btnHeight);
            btnSave.Location = new Point(leftX, buttonsTop);

            btnClear.Size = new Size(btnWidth, btnHeight);
            btnClear.Location = new Point(leftX + btnWidth + 10, buttonsTop);

            btnBack.Size = new Size(btnWidth, btnHeight);
            btnBack.Location = new Point((rightX + colWidth) - btnWidth, buttonsTop);
        }

        /// <summary>
        /// Positions field inputs and aligns their respective labels exactly 20 pixels above.
        /// </summary>
        private void PositionField(string labelName, Control ctrl, int x, int y, int width, int height)
        {
            ctrl.Location = new Point(x, y);
            ctrl.Size = new Size(width, height);

            Label lbl = null;
            Control[] matches = this.Controls.Find(labelName, true);
            if (matches.Length > 0 && matches[0] is Label)
            {
                lbl = (Label)matches[0];
            }
            else
            {
                string normalizedSearchName = ctrl.Name.Replace("txt", "").Replace("cmb", "").Replace("dtp", "").ToLower();
                foreach (Control c in this.Controls)
                {
                    if (c is Label && c.Name.ToLower().Contains(normalizedSearchName))
                    {
                        lbl = (Label)c;
                        break;
                    }
                }
            }

            if (lbl != null)
            {
                lbl.Location = new Point(x, y - 20);
                lbl.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                lbl.ForeColor = Color.FromArgb(127, 140, 141); // Unified Slate Gray
            }
        }

        /// <summary>
        /// Retrieves and displays the current user's profile details if they exist in the database.
        /// </summary>
        private void LoadApplicantProfile()
        {
            int currentUserId = UserSession.UserID;

            if (currentUserId <= 0)
            {
                MessageBox.Show("No active user session detected. Please log in again.",
                                "Session Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            string query = "SELECT FirstName, MiddleName, LastName, Birthday, Gender, Address, ContactNumber, Education, Skills, WorkExperience " +
                           "FROM Applicants WHERE ApplicantID = ?";

            using (OleDbConnection conn = DBConnection.GetConnection())
            {
                if (conn == null) return;

                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("?", currentUserId);

                    try
                    {
                        conn.Open();
                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isNewProfile = false;

                                txtFirstName.Text = reader["FirstName"]?.ToString();
                                txtMiddleName.Text = reader["MiddleName"]?.ToString();
                                txtLastName.Text = reader["LastName"]?.ToString();

                                if (reader["Birthday"] != DBNull.Value && reader["Birthday"] != null)
                                {
                                    dtpBirthday.Value = Convert.ToDateTime(reader["Birthday"]);
                                }
                                else
                                {
                                    dtpBirthday.Value = DateTime.Today.AddYears(-18);
                                }

                                cmbGender.SelectedItem = reader["Gender"]?.ToString() ?? "Male";
                                txtAddress.Text = reader["Address"]?.ToString();
                                txtContactNumber.Text = reader["ContactNumber"]?.ToString();
                                txtEducation.Text = reader["Education"]?.ToString();
                                txtSkills.Text = reader["Skills"]?.ToString();
                                txtWorkExperience.Text = reader["WorkExperience"]?.ToString();

                                btnSave.Text = "Update Profile";
                            }
                            else
                            {
                                isNewProfile = true;
                                btnSave.Text = "Save Profile";

                                if (!string.IsNullOrEmpty(UserSession.FullName))
                                {
                                    ParseFullName(UserSession.FullName);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading profile details: " + ex.Message,
                                        "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            UpdateProfileCompleteness();
        }

        /// <summary>
        /// Validates input controls before sending data to the database.
        /// </summary>
        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) || string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("First Name and Last Name are required.",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtContactNumber.Text))
            {
                MessageBox.Show("Please enter a valid Contact Number.",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            int age = DateTime.Today.Year - dtpBirthday.Value.Year;
            if (dtpBirthday.Value.Date > DateTime.Today.AddYears(-age)) age--;

            if (age < 15)
            {
                MessageBox.Show("Applicants must be at least 15 years of age to apply.",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            string contact = txtContactNumber.Text.Trim();
            bool isNumberValid = true;
            foreach (char c in contact)
            {
                if (!char.IsDigit(c) && c != '+' && c != '-' && c != ' ')
                {
                    isNumberValid = false;
                    break;
                }
            }

            if (!isNumberValid || contact.Length < 7)
            {
                MessageBox.Show("Please enter a valid telephone/mobile number (minimum 7 digits).",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Save Button click handler (Performs Save or Update).
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            using (OleDbConnection conn = DBConnection.GetConnection())
            {
                if (conn == null) return;

                string query;
                if (isNewProfile)
                {
                    query = "INSERT INTO Applicants (ApplicantID, FirstName, MiddleName, LastName, Birthday, Gender, Address, ContactNumber, Education, Skills, WorkExperience) " +
                            "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
                }
                else
                {
                    query = "UPDATE Applicants SET FirstName = ?, MiddleName = ?, LastName = ?, Birthday = ?, Gender = ?, Address = ?, ContactNumber = ?, Education = ?, Skills = ?, WorkExperience = ? " +
                            "WHERE ApplicantID = ?";
                }

                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    if (isNewProfile)
                    {
                        cmd.Parameters.AddWithValue("?", UserSession.UserID);
                        cmd.Parameters.AddWithValue("?", txtFirstName.Text.Trim());
                        cmd.Parameters.AddWithValue("?", txtMiddleName.Text.Trim());
                        cmd.Parameters.AddWithValue("?", txtLastName.Text.Trim());
                        cmd.Parameters.AddWithValue("?", dtpBirthday.Value.Date);
                        cmd.Parameters.AddWithValue("?", cmbGender.SelectedItem?.ToString() ?? "");
                        cmd.Parameters.AddWithValue("?", txtAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("?", txtContactNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("?", txtEducation.Text.Trim());
                        cmd.Parameters.AddWithValue("?", txtSkills.Text.Trim());
                        cmd.Parameters.AddWithValue("?", txtWorkExperience.Text.Trim());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("?", txtFirstName.Text.Trim());
                        cmd.Parameters.AddWithValue("?", txtMiddleName.Text.Trim());
                        cmd.Parameters.AddWithValue("?", txtLastName.Text.Trim());
                        cmd.Parameters.AddWithValue("?", dtpBirthday.Value.Date);
                        cmd.Parameters.AddWithValue("?", cmbGender.SelectedItem?.ToString() ?? "");
                        cmd.Parameters.AddWithValue("?", txtAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("?", txtContactNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("?", txtEducation.Text.Trim());
                        cmd.Parameters.AddWithValue("?", txtSkills.Text.Trim());
                        cmd.Parameters.AddWithValue("?", txtWorkExperience.Text.Trim());
                        cmd.Parameters.AddWithValue("?", UserSession.UserID);
                    }

                    try
                    {
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show(isNewProfile ? "Profile created successfully!" : "Profile updated successfully!",
                                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Write transaction events directly to your audit trail
                            string actionMsg = isNewProfile ? "Created applicant profile details" : "Updated applicant profile details";
                            LogAuditTrail(actionMsg);

                            isNewProfile = false;
                            btnSave.Text = "Update Profile";
                            UpdateProfileCompleteness();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error processing profile save action: " + ex.Message,
                                        "Database Action Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to clear all input fields?",
                                                        "Confirm Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                txtFirstName.Clear();
                txtMiddleName.Clear();
                txtLastName.Clear();
                txtAddress.Clear();
                txtContactNumber.Clear();
                txtEducation.Clear();
                txtSkills.Clear();
                txtWorkExperience.Clear();
                cmbGender.SelectedIndex = 0;
                dtpBirthday.Value = DateTime.Today.AddYears(-18);
                UpdateProfileCompleteness();
            }
        }

        private void ParseFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName)) return;

            string[] parts = fullName.Trim().Split(' ');
            if (parts.Length > 1)
            {
                txtFirstName.Text = parts[0];
                txtLastName.Text = parts[parts.Length - 1];
                if (parts.Length > 2)
                {
                    txtMiddleName.Text = parts[1];
                }
            }
            else
            {
                txtFirstName.Text = fullName;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ApplyModernUI()
        {
            this.BackColor = Color.FromArgb(245, 247, 250); // Matches dashboard off-white
            this.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);

            // Style header
            lblHeader.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblHeader.ForeColor = Color.FromArgb(27, 38, 59); // Standard corporate navy

            // Style buttons to match main dashboard action standards
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.BackColor = Color.FromArgb(41, 128, 185); // Standard Blue
            btnSave.ForeColor = Color.White;
            btnSave.Cursor = Cursors.Hand;
            btnSave.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);

            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.FlatAppearance.BorderSize = 1;
            btnClear.FlatAppearance.BorderColor = Color.FromArgb(189, 195, 199);
            btnClear.BackColor = Color.White;
            btnClear.ForeColor = Color.FromArgb(127, 140, 141);
            btnClear.Cursor = Cursors.Hand;
            btnClear.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);

            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.FlatAppearance.BorderSize = 1;
            btnBack.FlatAppearance.BorderColor = Color.FromArgb(127, 140, 141);
            btnBack.BackColor = Color.White;
            btnBack.ForeColor = Color.FromArgb(44, 62, 80);
            btnBack.Cursor = Cursors.Hand;
            btnBack.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);

            // Sleek input borders formatting
            StyleBorderLayout(txtFirstName);
            StyleBorderLayout(txtMiddleName);
            StyleBorderLayout(txtLastName);
            StyleBorderLayout(txtContactNumber);
            StyleBorderLayout(txtAddress);
            StyleBorderLayout(txtEducation);
            StyleBorderLayout(txtSkills);
            StyleBorderLayout(txtWorkExperience);
            StyleBorderLayout(cmbGender);
            StyleBorderLayout(dtpBirthday);

            // Multiline scrollbar policies
            txtAddress.Multiline = true;
            txtAddress.ScrollBars = ScrollBars.Vertical;

            txtEducation.Multiline = true;
            txtEducation.ScrollBars = ScrollBars.Vertical;

            txtSkills.Multiline = true;
            txtSkills.ScrollBars = ScrollBars.Vertical;

            txtWorkExperience.Multiline = true;
            txtWorkExperience.ScrollBars = ScrollBars.Vertical;
        }

        private void StyleBorderLayout(Control ctrl)
        {
            ctrl.BackColor = Color.White;
            ctrl.ForeColor = Color.FromArgb(44, 62, 80);
            ctrl.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);

            if (ctrl is TextBox txt)
            {
                txt.BorderStyle = BorderStyle.FixedSingle;
            }
            else if (ctrl is ComboBox cmb)
            {
                cmb.FlatStyle = FlatStyle.Flat;
            }
        }

        /// <summary>
        /// Logs applicant profile actions to the AuditTrail table safely.
        /// </summary>
        private void LogAuditTrail(string action)
        {
            using (OleDbConnection conn = DBConnection.GetConnection())
            {
                if (conn == null) return;
                try
                {
                    conn.Open();
                    string query = "INSERT INTO AuditTrail (UserID, [Action], DateCreated) VALUES (?, ?, ?)";
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("?", UserSession.UserID);
                        cmd.Parameters.AddWithValue("?", action);
                        cmd.Parameters.AddWithValue("?", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Audit Log Error: " + ex.Message);
                }
            }
        }
    }
}