using HRApplicantSystem.Database;
using HRApplicantSystem.Forms.HR;
using HRApplicantSystem.Forms.Applicant; // Ensure this matches your namespace for JobVacancyForm
using HRApplicantSystem.Classes;
using System;
using System.Data.OleDb;
using System.Windows.Forms;

namespace HRApplicantSystem.Forms.Login
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Please enter your username/email.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                // 1. Check Users table first (HR / Manager / Admin)
                // Note the square brackets around [Full Name] and [Password] because they are reserved keywords or contain spaces
                string hrQuery = "SELECT * FROM Users WHERE Username = ? AND [Password] = ?";
                using (OleDbCommand hrCmd = new OleDbCommand(hrQuery, con))
                {
                    hrCmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                    hrCmd.Parameters.AddWithValue("@Password", txtPassword.Text);

                    using (OleDbDataReader reader = hrCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Save user to session
                            UserSession.UserID = Convert.ToInt32(reader["UserID"]);
                            UserSession.Username = reader["Username"].ToString();
                            UserSession.FullName = reader["Full Name"].ToString(); // Matches your current table column name
                            UserSession.Role = reader["Role"].ToString();

                            MessageBox.Show($"Welcome, {UserSession.FullName}!", "Login Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            HRDashboard dashboard = new HRDashboard();
                            dashboard.Show();
                            this.Hide();
                            return; // Exit method
                        }
                    }
                }

                // 2. If not found in Users, check ApplicantAccounts table
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

                            // Save applicant details to session
                            UserSession.UserID = Convert.ToInt32(reader["ApplicantID"]);
                            UserSession.Username = reader["Email"].ToString();
                            UserSession.FullName = "Applicant"; // Placeholder until profile is filled
                            UserSession.Role = "Applicant";

                            MessageBox.Show("Login Successful! Redirecting to Job Vacancies.", "Login Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Redirect to applicant side
                            JobVacancyForm jobForm = new JobVacancyForm();
                            jobForm.Show();
                            this.Hide();
                            return;
                        }
                    }
                }

                // If neither query succeeds
                MessageBox.Show("Invalid Username/Email or Password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred during login:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RegisterForm registerForm = new RegisterForm();
            registerForm.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Empty event handlers left to avoid designer breakage
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