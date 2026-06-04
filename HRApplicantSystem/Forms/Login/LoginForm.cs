using HRApplicantSystem.Database;
using HRApplicantSystem.Forms.HR;
using HRApplicantSystem.Forms.Applicant;
using HRApplicantSystem.Classes;
using System;
using System.Data; // Ensure ConnectionState is fully resolved
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace HRApplicantSystem.Forms.Login
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();

            // Set default state
            rdoApplicant.Checked = true;
            UpdateUiLayout();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string usernamePrompt = rdoAdmin.Checked ? "username" : "email address";

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Please enter your " + usernamePrompt + ".", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter your password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            OleDbConnection con = DBConnection.GetConnection();
            if (con == null) return;

            try
            {
                con.Open();

                // 1. Check HR / Manager / Admin Users table
                if (rdoAdmin.Checked)
                {
                    string hrQuery = "SELECT * FROM Users WHERE Username = ? AND [Password] = ?";
                    using (OleDbCommand hrCmd = new OleDbCommand(hrQuery, con))
                    {
                        hrCmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                        hrCmd.Parameters.AddWithValue("@Password", txtPassword.Text);

                        using (OleDbDataReader reader = hrCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Save user details to active session
                                UserSession.UserID = Convert.ToInt32(reader["UserID"]);
                                UserSession.Username = reader["Username"].ToString();
                                UserSession.FullName = reader["Full Name"].ToString();
                                UserSession.Role = reader["Role"].ToString();

                                MessageBox.Show("Welcome, " + UserSession.FullName + "!", "Login Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                HRDashboard dashboard = new HRDashboard();
                                dashboard.Show();
                                this.Hide();
                                return; // Exit method
                            }
                        }
                    }
                } // End if (rdoAdmin.Checked)

                // 2. Check ApplicantAccounts table
                if (rdoApplicant.Checked)
                {
                    string appQuery = "SELECT * FROM ApplicantAccounts WHERE Email = ? AND [Password] = ?";
                    using (OleDbCommand appCmd = new OleDbCommand(appQuery, con))
                    {
                        appCmd.Parameters.AddWithValue("@Email", txtUsername.Text.Trim());
                        appCmd.Parameters.AddWithValue("@Password", txtPassword.Text);

                        using (OleDbDataReader reader = appCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (reader["AccountStatus"].ToString() != "Active")
                                {
                                    MessageBox.Show("Your account is currently inactive. Please contact HR.", "Account Inactive", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }

                                // Save core credentials to active session
                                UserSession.UserID = Convert.ToInt32(reader["ApplicantID"]);
                                UserSession.Username = reader["Email"].ToString();
                                UserSession.Role = "Applicant";

                                // Attempt to pull their real name from the Applicants table if profile exists
                                string nameQuery = "SELECT FirstName, LastName FROM Applicants WHERE ApplicantID = ?";
                                using (OleDbCommand nameCmd = new OleDbCommand(nameQuery, con))
                                {
                                    nameCmd.Parameters.AddWithValue("?", UserSession.UserID);
                                    using (OleDbDataReader nameReader = nameCmd.ExecuteReader())
                                    {
                                        if (nameReader.Read())
                                        {
                                            string fName = (nameReader["FirstName"] != DBNull.Value && nameReader["FirstName"] != null) ? nameReader["FirstName"].ToString() : "";
                                            string lName = (nameReader["LastName"] != DBNull.Value && nameReader["LastName"] != null) ? nameReader["LastName"].ToString() : "";
                                            UserSession.FullName = (fName + " " + lName).Trim();
                                        }

                                        // Fallback if they haven't filled out a profile yet
                                        if (string.IsNullOrWhiteSpace(UserSession.FullName))
                                        {
                                            UserSession.FullName = "Applicant";
                                        }
                                    }
                                }

                                MessageBox.Show("Login Successful! Welcome, " + UserSession.FullName + ".", "Login Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Redirect to applicant portal dashboard
                                ApplicantDashboard dashboard = new ApplicantDashboard();
                                dashboard.Show();
                                this.Hide();
                                return;
                            }
                        }
                    }
                } // End if (rdoApplicant.Checked)

                // If neither query succeeds
                MessageBox.Show("Invalid " + usernamePrompt + " or Password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred during login:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        } // End btnLogin_Click method

        private void button1_Click(object sender, EventArgs e)
        {
            RegisterForm registerForm = new RegisterForm();
            registerForm.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void RoleSelection_CheckedChanged(object sender, EventArgs e)
        {
            UpdateUiLayout();
        }

        /// <summary>
        /// Dynamically alters UI labels, hides register controls, and adjusts form height [3].
        /// </summary>
        private void UpdateUiLayout()
        {
            if (rdoApplicant.Checked)
            {
                lblUsername.Text = "Email Address:";
                label1.Visible = true;
                btnRegister.Visible = true;
                btnExit.Location = new Point(10, 340);
                this.ClientSize = new Size(350, 385);
            }
            else // HR / Management Login Mode
            {
                lblUsername.Text = "Username:";
                label1.Visible = false;
                btnRegister.Visible = false;
                btnExit.Location = new Point(10, 265);
                this.ClientSize = new Size(350, 310);
            }
        }

        // Empty event handlers kept to avoid designer breakage
        private void label1_Click(object sender, EventArgs e) { }
        private void label1_Click_1(object sender, EventArgs e) { }
        private void label1_Click_2(object sender, EventArgs e) { }
        private void label1_Click_3(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) {
        }
        private void textBox1_TextChanged_1(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
    }
}