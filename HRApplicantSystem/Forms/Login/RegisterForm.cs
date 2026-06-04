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

            // Programmatically populate Gender Dropdown
            cmbGender.Items.Clear();
            cmbGender.Items.AddRange(new object[] { "Male", "Female", "Other" });
            cmbGender.SelectedIndex = 0;

            // Set max date limit for birthday input
            dtpBirthday.MaxDate = DateTime.Today;
            dtpBirthday.Value = DateTime.Today.AddYears(-18); // Default to 18 years ago
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            // 1. Mandatory Field Validations
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

                // Start transaction to verify account creation integrity across both tables
                transaction = con.BeginTransaction();

                // 2. Check if the Email already exists in ApplicantAccounts
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

                // 3. Insert into ApplicantAccounts
                string insertAccountQuery = "INSERT INTO ApplicantAccounts (Email, [Password], AccountStatus) VALUES (?, ?, ?)";
                using (OleDbCommand insertAccCmd = new OleDbCommand(insertAccountQuery, con, transaction))
                {
                    insertAccCmd.Parameters.Add("@Email", OleDbType.VarWChar).Value = txtUsername.Text.Trim();
                    insertAccCmd.Parameters.Add("@Password", OleDbType.VarWChar).Value = txtPassword.Text;
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
                // Note: 'WorkExperience' is used to match your exact Access database column header
                string insertProfileQuery = "INSERT INTO Applicants (ApplicantID, FirstName, MiddleName, LastName, Birthday, Gender, Address, ContactNumber, Education, Skills, WorkExperience) " +
                                            "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

                using (OleDbCommand insertProfileCmd = new OleDbCommand(insertProfileQuery, con, transaction))
                {
                    // Access matches parameters by strict order of declaration (?)
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

                // Commit transaction
                transaction.Commit();

                MessageBox.Show("Registration Successful! You can now log in.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    try { transaction.Rollback(); } catch { /* Ignore transaction cleanup issues */ }
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

        // Left to maintain designer structure
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