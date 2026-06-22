using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using HRApplicantSystem.Classes;
using HRApplicantSystem.Database;

namespace HRApplicantSystem.Forms.Login
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();

            // Programmatically populate Gender Dropdown
            cmbGender.Items.Clear();
            cmbGender.Items.AddRange(new object[] { "Male", "Female", "Other" });
            cmbGender.SelectedIndex = 0;

            // Set max date limit for birthday input
            dtpBirthday.MaxDate = DateTime.Today;
            dtpBirthday.Value = DateTime.Today.AddYears(-18);

            ApplyThemeStyles();
        }

        /// <summary>
        /// Formats and styles registration controls dynamically to match the dashboard's theme.
        /// </summary>
        private void ApplyThemeStyles()
        {
            // Main canvas background matches the dashboard's content container
            this.BackColor = Color.FromArgb(244, 246, 247);

            StyleControlsRecursive(this);

            if (lblTitle != null)
            {
                lblTitle.ForeColor = Color.FromArgb(27, 38, 59); // Dark slate blue matching the dashboard headers
                lblTitle.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            }

            if (pnlSidebar != null)
            {
                pnlSidebar.BackColor = Color.FromArgb(27, 38, 59); // Sidebar background
            }

            if (pnlAccentBar != null)
            {
                pnlAccentBar.BackColor = Color.FromArgb(41, 128, 185); // Indicator blue
            }

            // Button visual tuning matching system buttons
            if (btnRegister != null)
            {
                UITheme.StylePrimaryButton(btnRegister);
                btnRegister.FlatStyle = FlatStyle.Flat;
                btnRegister.FlatAppearance.BorderSize = 0;
                btnRegister.BackColor = Color.FromArgb(41, 128, 185); // Accent Blue
                btnRegister.ForeColor = Color.White;
                btnRegister.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            }

            if (btnBack != null)
            {
                UITheme.StyleSecondaryButton(btnBack);
                btnBack.FlatStyle = FlatStyle.Flat;
                btnBack.FlatAppearance.BorderSize = 1;
                btnBack.FlatAppearance.BorderColor = Color.FromArgb(180, 190, 200);
                btnBack.BackColor = Color.White;
                btnBack.ForeColor = Color.FromArgb(127, 140, 141);
                btnBack.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            }
        }

        private void StyleControlsRecursive(Control parent)
        {
            // Do not override branding styles inside the custom sidebar
            if (parent.Name == "pnlSidebar") return;

            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is Label lbl && lbl.Name != "lblTitle" && lbl.Parent?.Name != "pnlSidebar")
                {
                    lbl.ForeColor = Color.FromArgb(127, 140, 141); // Slate gray muted text
                    lbl.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
                }
                else if (ctrl is TextBox txt)
                {
                    txt.BackColor = Color.White;
                    txt.ForeColor = Color.FromArgb(27, 38, 59); // Dark navy text
                    txt.Font = new Font("Segoe UI", 9.5F);
                    txt.BorderStyle = BorderStyle.FixedSingle;
                }
                else if (ctrl is ComboBox cmb)
                {
                    cmb.BackColor = Color.White;
                    cmb.ForeColor = Color.FromArgb(27, 38, 59);
                    cmb.Font = new Font("Segoe UI", 9.5F);
                }
                else if (ctrl is DateTimePicker dtp)
                {
                    dtp.Font = new Font("Segoe UI", 9.5F);
                }
                else if (ctrl.HasChildren)
                {
                    StyleControlsRecursive(ctrl);
                }
            }
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            try
            {
                return Regex.IsMatch(email.Trim(),
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private bool IsStrongPassword(string password, out string errorMessage)
        {
            errorMessage = "";
            if (password.Length < 8)
            {
                errorMessage = "Password must be at least 8 characters long.";
                return false;
            }
            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                errorMessage = "Password must contain at least one uppercase letter.";
                return false;
            }
            if (!Regex.IsMatch(password, @"[a-z]"))
            {
                errorMessage = "Password must contain at least one lowercase letter.";
                return false;
            }
            if (!Regex.IsMatch(password, @"[0-9]"))
            {
                errorMessage = "Password must contain at least one numeric digit.";
                return false;
            }
            if (!Regex.IsMatch(password, @"[\W_]"))
            {
                errorMessage = "Password must contain at least one special character (e.g., @, $, !, %, *, ?, &).";
                return false;
            }
            return true;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string emailInput = txtUsername.Text.Trim();
            if (string.IsNullOrWhiteSpace(emailInput))
            {
                MessageBox.Show("Please enter an email address for your username.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IsValidEmail(emailInput))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string passwordInput = txtPassword.Text;
            if (string.IsNullOrWhiteSpace(passwordInput))
            {
                MessageBox.Show("Please enter a password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IsStrongPassword(passwordInput, out string passwordValidationError))
            {
                MessageBox.Show(passwordValidationError, "Weak Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (passwordInput != txtConfirmPassword.Text)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtFirstName.Text) || string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("First Name and Last Name are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtContactNum.Text))
            {
                MessageBox.Show("Please enter your Contact Number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            OleDbConnection con = DBConnection.GetConnection();
            if (con == null) return;

            OleDbTransaction transaction = null;

            try
            {
                con.Open();

                string checkQuery = "SELECT COUNT(*) FROM ApplicantAccounts WHERE Email = ?";
                using (OleDbCommand checkCmd = new OleDbCommand(checkQuery, con))
                {
                    checkCmd.Parameters.Add("@Email", OleDbType.VarWChar).Value = emailInput;
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("An account with this email already exists.", "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                transaction = con.BeginTransaction();

                // Accounts default to Active Status
                string insertAccountQuery = "INSERT INTO ApplicantAccounts (Email, [Password], AccountStatus) VALUES (?, ?, ?)";
                using (OleDbCommand insertAccCmd = new OleDbCommand(insertAccountQuery, con, transaction))
                {
                    insertAccCmd.Parameters.Add("@Email", OleDbType.VarWChar).Value = emailInput;
                    insertAccCmd.Parameters.Add("@Password", OleDbType.VarWChar).Value = passwordInput;
                    insertAccCmd.Parameters.Add("@AccountStatus", OleDbType.VarWChar).Value = "Active";

                    insertAccCmd.ExecuteNonQuery();
                }

                int newApplicantId = 0;
                string identityQuery = "SELECT @@IDENTITY";
                using (OleDbCommand identityCmd = new OleDbCommand(identityQuery, con, transaction))
                {
                    newApplicantId = Convert.ToInt32(identityCmd.ExecuteScalar());
                }

                string insertProfileQuery = "INSERT INTO Applicants (ApplicantID, FirstName, MiddleName, LastName, Birthday, Gender, Address, ContactNumber, Education, Skills, WorkExperience) " +
                                            "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

                using (OleDbCommand insertProfileCmd = new OleDbCommand(insertProfileQuery, con, transaction))
                {
                    insertProfileCmd.Parameters.Add("@ApplicantID", OleDbType.Integer).Value = newApplicantId;
                    insertProfileCmd.Parameters.Add("@FirstName", OleDbType.VarWChar).Value = txtFirstName.Text.Trim();
                    insertProfileCmd.Parameters.Add("@MiddleName", OleDbType.VarWChar).Value = txtMiddleName.Text.Trim();
                    insertProfileCmd.Parameters.Add("@LastName", OleDbType.VarWChar).Value = txtLastName.Text.Trim();
                    insertProfileCmd.Parameters.Add("@Birthday", OleDbType.Date).Value = dtpBirthday.Value.Date;
                    insertProfileCmd.Parameters.Add("@Gender", OleDbType.VarWChar).Value = cmbGender.SelectedItem?.ToString() ?? "";
                    insertProfileCmd.Parameters.Add("@Address", OleDbType.VarWChar).Value = txtAddress.Text.Trim();
                    insertProfileCmd.Parameters.Add("@ContactNumber", OleDbType.VarWChar).Value = txtContactNum.Text.Trim();
                    insertProfileCmd.Parameters.Add("@Education", OleDbType.VarWChar).Value = txtEducation.Text.Trim();
                    insertProfileCmd.Parameters.Add("@Skills", OleDbType.VarWChar).Value = txtSkills.Text.Trim();
                    insertProfileCmd.Parameters.Add("@WorkExperience", OleDbType.VarWChar).Value = txtWorkExperience.Text.Trim();

                    insertProfileCmd.ExecuteNonQuery();
                }

                transaction.Commit();

                // Log successful applicant registration
                LogAuditTrail(newApplicantId, $"Registered new applicant account: '{emailInput}'");

                MessageBox.Show("Registration Successful! You can now log in.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    try { transaction.Rollback(); } catch { }
                }
                MessageBox.Show("An error occurred during registration:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Safely registers registration events to the AuditTrail.
        /// </summary>
        private void LogAuditTrail(int userId, string action)
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
                        cmd.Parameters.Add("?", OleDbType.Integer).Value = userId;
                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = action;
                        cmd.Parameters.Add("?", OleDbType.Date).Value = DateTime.Now;
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Audit Log Error: " + ex.Message);
                }
            }
        }

        private void lblTitle_Click(object sender, EventArgs e) { }
        private void lblPassword_Click(object sender, EventArgs e) { }
        private void lblUsername_Click(object sender, EventArgs e) { }
        private void txtPassword_TextChanged_1(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox1_TextChanged_1(object sender, EventArgs e) { }
        private void label1_Click_1(object sender, EventArgs e) { }
        private void RegisterForm_Load(object sender, EventArgs e) { }
    }
}