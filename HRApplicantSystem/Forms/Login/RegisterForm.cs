using System;
using System.Data.OleDb;
using HRApplicantSystem.Database;
using System.Windows.Forms;

namespace HRApplicantSystem.Forms.Login
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Please enter your full name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Please enter an email address for your username.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter a password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // --- Split the Full Name into FirstName and LastName for your database ---
            string fullName = txtFullName.Text.Trim();
            string firstName = "";
            string lastName = "";

            int spaceIndex = fullName.IndexOf(' ');
            if (spaceIndex > 0)
            {
                firstName = fullName.Substring(0, spaceIndex).Trim();
                lastName = fullName.Substring(spaceIndex + 1).Trim();
            }
            else
            {
                firstName = fullName;
                lastName = "N/A"; // Fallback placeholder if only one name is typed
            }
            // ------------------------------------------------------------------------

            OleDbConnection con = DBConnection.GetConnection();
            if (con == null) return;

            OleDbTransaction transaction = null;

            try
            {
                con.Open();

                // Start a transaction to ensure both account and profile are created together
                transaction = con.BeginTransaction();

                // 1. Check if the Email already exists in ApplicantAccounts
                string checkQuery = "SELECT COUNT(*) FROM ApplicantAccounts WHERE Email = ?";
                using (OleDbCommand checkCmd = new OleDbCommand(checkQuery, con, transaction))
                {
                    checkCmd.Parameters.Add("@Email", OleDbType.VarWChar).Value = txtUsername.Text.Trim();
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("An account with this email already exists.", "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // 2. Insert into ApplicantAccounts
                string insertAccountQuery = "INSERT INTO ApplicantAccounts (Email, [Password], AccountStatus) VALUES (?, ?, ?)";
                using (OleDbCommand insertAccCmd = new OleDbCommand(insertAccountQuery, con, transaction))
                {
                    insertAccCmd.Parameters.Add("@Email", OleDbType.VarWChar).Value = txtUsername.Text.Trim();
                    insertAccCmd.Parameters.Add("@Password", OleDbType.VarWChar).Value = txtPassword.Text;
                    insertAccCmd.Parameters.Add("@AccountStatus", OleDbType.VarWChar).Value = "Active";

                    insertAccCmd.ExecuteNonQuery();
                }

                // 3. Get the newly generated ApplicantID AutoNumber [1, 2]
                int newApplicantId = 0;
                string identityQuery = "SELECT @@IDENTITY";
                using (OleDbCommand identityCmd = new OleDbCommand(identityQuery, con, transaction))
                {
                    newApplicantId = Convert.ToInt32(identityCmd.ExecuteScalar());
                }

                // 4. Insert corresponding Profile record into the Applicants table
                // This satisfies the Foreign Key Constraint required by your database layout [4]
                string insertProfileQuery = "INSERT INTO Applicants (ApplicantID, FirstName, LastName) VALUES (?, ?, ?)";
                using (OleDbCommand insertProfileCmd = new OleDbCommand(insertProfileQuery, con, transaction))
                {
                    insertProfileCmd.Parameters.Add("@ApplicantID", OleDbType.Integer).Value = newApplicantId;
                    insertProfileCmd.Parameters.Add("@FirstName", OleDbType.VarWChar).Value = firstName;
                    insertProfileCmd.Parameters.Add("@LastName", OleDbType.VarWChar).Value = lastName;

                    insertProfileCmd.ExecuteNonQuery();
                }

                // Commit both insertions successfully
                transaction.Commit();

                MessageBox.Show("Registration Successful! You can now log in.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    try { transaction.Rollback(); } catch { /* Ignore rollback failure */ }
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

        // Keep remaining empty event handlers to prevent designer errors
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