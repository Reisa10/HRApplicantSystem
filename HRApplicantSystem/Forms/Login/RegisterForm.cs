using System;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Windows.Forms;
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
            dtpBirthday.Value = DateTime.Today.AddYears(-18); // Default to 18 years ago
        }

        // Email format validation helper
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            try
            {
                // Simple standard regex for basic email structure validation
                return Regex.IsMatch(email.Trim(),
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        // Strong password rule checker
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
            if (!Regex.IsMatch(password, @"[\W_]")) // \W detects special characters (non-word alphanumeric)
            {
                errorMessage = "Password must contain at least one special character (e.g., @, $, !, %, *, ?, &).";
                return false;
            }
            return true;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            // 1. Mandatory Field and Format Validations (Client-side)
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

            // Strong password evaluation
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

                // 2. Check if the Email already exists (Verify database duplicates before opening a transaction)
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

                // Start transaction for atomic insertion across multi-table relationship
                transaction = con.BeginTransaction();

                // 3. Insert into ApplicantAccounts
                string insertAccountQuery = "INSERT INTO ApplicantAccounts (Email, [Password], AccountStatus) VALUES (?, ?, ?)";
                using (OleDbCommand insertAccCmd = new OleDbCommand(insertAccountQuery, con, transaction))
                {
                    insertAccCmd.Parameters.Add("@Email", OleDbType.VarWChar).Value = emailInput;
                    insertAccCmd.Parameters.Add("@Password", OleDbType.VarWChar).Value = passwordInput;
                    insertAccCmd.Parameters.Add("@AccountStatus", OleDbType.VarWChar).Value = "Active";

                    insertAccCmd.ExecuteNonQuery();
                }

                // 4. Retrieve the auto-incremented Identity ID [1, 2]
                int newApplicantId = 0;
                string identityQuery = "SELECT @@IDENTITY";
                using (OleDbCommand identityCmd = new OleDbCommand(identityQuery, con, transaction))
                {
                    newApplicantId = Convert.ToInt32(identityCmd.ExecuteScalar());
                }

                // 5. Insert full profile record into the Applicants table
                string insertProfileQuery = "INSERT INTO Applicants (ApplicantID, FirstName, MiddleName, LastName, Birthday, Gender, Address, ContactNumber, Education, Skills, WorkExperience) " +
                                            "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

                using (OleDbCommand insertProfileCmd = new OleDbCommand(insertProfileQuery, con, transaction))
                {
                    // Access evaluates parameters strictly by order of declaration (?)
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

                // Commit transaction once everything completes successfully
                transaction.Commit();

                MessageBox.Show("Registration Successful! You can now log in.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    try { transaction.Rollback(); } catch { /* Suppress rollback cascade errors */ }
                }
                MessageBox.Show("An error occurred during registration:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
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