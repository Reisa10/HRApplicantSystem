using HRApplicantSystem.Classes;
using HRApplicantSystem.Database;
using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace HRApplicantSystem.Forms.Login
{
    public partial class ChangePasswordForm : Form
    {
        private bool _isHR;

        

        public ChangePasswordForm(bool isHR)
        {
            _isHR = isHR;
            InitializeComponent();
            InitializeSessionState();
        }

        private void ChangePasswordForm_Load(object sender, EventArgs e)
        {
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Checks active login session configuration to populate identification field
        private void InitializeSessionState()
        {
            if (!string.IsNullOrWhiteSpace(UserSession.Username))
            {
                txtIdentifier.Text = UserSession.Username;
                txtIdentifier.Enabled = false; // Lock editing to prevent altering credentials outside current session identity
            }
        }

        // Strong password rule compliance validation helper
        private bool IsStrongPassword(string password, out string errorMessage)
        {
            errorMessage = "";
            if (password.Length < 8)
            {
                errorMessage = "New password must be at least 8 characters long.";
                return false;
            }
            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                errorMessage = "New password must contain at least one uppercase letter.";
                return false;
            }
            if (!Regex.IsMatch(password, @"[a-z]"))
            {
                errorMessage = "New password must contain at least one lowercase letter.";
                return false;
            }
            if (!Regex.IsMatch(password, @"[0-9]"))
            {
                errorMessage = "New password must contain at least one numeric digit.";
                return false;
            }
            if (!Regex.IsMatch(password, @"[\W_]"))
            {
                errorMessage = "New password must contain at least one special character (e.g., @, $, !, %, *, ?, &).";
                return false;
            }
            return true;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // 1. Validation for Empty Inputs
            if (string.IsNullOrWhiteSpace(txtIdentifier.Text) ||
                string.IsNullOrWhiteSpace(txtOldPassword.Text) ||
                string.IsNullOrWhiteSpace(txtNewPassword.Text) ||
                string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            {
                MessageBox.Show("All fields are required.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Strong Password Validation
            if (!IsStrongPassword(txtNewPassword.Text, out string strengthError))
            {
                MessageBox.Show(strengthError, "Weak Password Policy",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 3. Confirm matching inputs
            if (txtNewPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("New password and confirmation do not match.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 4. Check that new password is not identical to current password
            if (txtNewPassword.Text == txtOldPassword.Text)
            {
                MessageBox.Show("New password must be different from your current password.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            OleDbConnection con = DBConnection.GetConnection();
            if (con == null) return;

            try
            {
                con.Open();

                // 5. Verify database credentials matching the active user identification entry
                string verifyQuery = _isHR
                    ? "SELECT COUNT(*) FROM Users WHERE Username = ? AND [Password] = ?"
                    : "SELECT COUNT(*) FROM ApplicantAccounts WHERE Email = ? AND [Password] = ?";

                using (OleDbCommand verifyCmd = new OleDbCommand(verifyQuery, con))
                {
                    // Parameter types mapped explicitly for strict sequential verification safety
                    verifyCmd.Parameters.Add("@Identifier", OleDbType.VarWChar).Value = txtIdentifier.Text.Trim();
                    verifyCmd.Parameters.Add("@OldPass", OleDbType.VarWChar).Value = txtOldPassword.Text;

                    int match = Convert.ToInt32(verifyCmd.ExecuteScalar());
                    if (match == 0)
                    {
                        MessageBox.Show("The current username/email or current password you entered is incorrect.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // 6. Execute password modification query
                string updateQuery = _isHR
                    ? "UPDATE Users SET [Password] = ? WHERE Username = ?"
                    : "UPDATE ApplicantAccounts SET [Password] = ? WHERE Email = ?";

                using (OleDbCommand updateCmd = new OleDbCommand(updateQuery, con))
                {
                    updateCmd.Parameters.Add("@NewPass", OleDbType.VarWChar).Value = txtNewPassword.Text;
                    updateCmd.Parameters.Add("@Identifier", OleDbType.VarWChar).Value = txtIdentifier.Text.Trim();
                    updateCmd.ExecuteNonQuery();
                }

                MessageBox.Show("Password changed successfully! Please log in with your new password on next session.",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updating the password:\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }
    }
}