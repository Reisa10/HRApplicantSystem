using System;
using System.Data.OleDb;
using System.Windows.Forms;
using HRApplicantSystem.Database;

namespace HRApplicantSystem.Forms.Login
{
    public class ChangePasswordForm : Form
    {
        private bool _isHR;

        // Controls
        private Label lblTitle, lblIdentifier, lblOld, lblNew, lblConfirm;
        private TextBox txtIdentifier, txtOldPassword, txtNewPassword, txtConfirmPassword;
        private Button btnSave, btnCancel;

        public ChangePasswordForm(bool isHR)
        {
            _isHR = isHR;
            BuildUI();
        }

        private void BuildUI()
        {
            this.Text = "Change Password";
            this.Size = new System.Drawing.Size(380, 320);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            lblTitle = new Label()
            {
                Text = "Change Password",
                Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(100, 15),
                AutoSize = true
            };

            // Label changes depending on who is logging in
            lblIdentifier = new Label()
            {
                Text = _isHR ? "Username:" : "Email Address:",
                Location = new System.Drawing.Point(20, 60),
                AutoSize = true
            };
            txtIdentifier = new TextBox()
            {
                Location = new System.Drawing.Point(160, 57),
                Width = 180
            };

            lblOld = new Label()
            {
                Text = "Current Password:",
                Location = new System.Drawing.Point(20, 100),
                AutoSize = true
            };
            txtOldPassword = new TextBox()
            {
                Location = new System.Drawing.Point(160, 97),
                Width = 180,
                PasswordChar = '*'
            };

            lblNew = new Label()
            {
                Text = "New Password:",
                Location = new System.Drawing.Point(20, 140),
                AutoSize = true
            };
            txtNewPassword = new TextBox()
            {
                Location = new System.Drawing.Point(160, 137),
                Width = 180,
                PasswordChar = '*'
            };

            lblConfirm = new Label()
            {
                Text = "Confirm Password:",
                Location = new System.Drawing.Point(20, 180),
                AutoSize = true
            };
            txtConfirmPassword = new TextBox()
            {
                Location = new System.Drawing.Point(160, 177),
                Width = 180,
                PasswordChar = '*'
            };

            btnSave = new Button()
            {
                Text = "Save",
                Location = new System.Drawing.Point(90, 230),
                Width = 90
            };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button()
            {
                Text = "Cancel",
                Location = new System.Drawing.Point(195, 230),
                Width = 90
            };
            btnCancel.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[]
            {
                lblTitle,
                lblIdentifier, txtIdentifier,
                lblOld, txtOldPassword,
                lblNew, txtNewPassword,
                lblConfirm, txtConfirmPassword,
                btnSave, btnCancel
            });
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // 1. Empty field check
            if (string.IsNullOrWhiteSpace(txtIdentifier.Text) ||
                string.IsNullOrWhiteSpace(txtOldPassword.Text) ||
                string.IsNullOrWhiteSpace(txtNewPassword.Text) ||
                string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            {
                MessageBox.Show("All fields are required.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. New password length check
            if (txtNewPassword.Text.Length < 6)
            {
                MessageBox.Show("New password must be at least 6 characters.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 3. Confirm match check
            if (txtNewPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("New password and confirmation do not match.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 4. Same as old password check
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

                // 5. Verify the identifier + old password exists in the correct table
                string verifyQuery = _isHR
                    ? "SELECT COUNT(*) FROM Users WHERE Username = ? AND [Password] = ?"
                    : "SELECT COUNT(*) FROM ApplicantAccounts WHERE Email = ? AND [Password] = ?";

                using (OleDbCommand verifyCmd = new OleDbCommand(verifyQuery, con))
                {
                    verifyCmd.Parameters.AddWithValue("@Identifier", txtIdentifier.Text.Trim());
                    verifyCmd.Parameters.AddWithValue("@OldPass", txtOldPassword.Text);

                    int match = Convert.ToInt32(verifyCmd.ExecuteScalar());
                    if (match == 0)
                    {
                        MessageBox.Show("The username/email or current password is incorrect.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // 6. Update to the new password
                string updateQuery = _isHR
                    ? "UPDATE Users SET [Password] = ? WHERE Username = ?"
                    : "UPDATE ApplicantAccounts SET [Password] = ? WHERE Email = ?";

                using (OleDbCommand updateCmd = new OleDbCommand(updateQuery, con))
                {
                    updateCmd.Parameters.AddWithValue("@NewPass", txtNewPassword.Text);
                    updateCmd.Parameters.AddWithValue("@Identifier", txtIdentifier.Text.Trim());
                    updateCmd.ExecuteNonQuery();
                }

                MessageBox.Show("Password changed successfully! Please log in with your new password.",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred:\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
            }
        }
    }
}