    using System;
    using System.Data.OleDb;
    using HRApplicantSystem.Database;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    namespace HRApplicantSystem.Forms.Login
    {
        public partial class RegisterForm : Form
        {
            public RegisterForm()
            {
                InitializeComponent();
            }

            private void lblTitle_Click(object sender, EventArgs e)
            {

            }

            private void btnExit_Click(object sender, EventArgs e)
            {

            }

            private void btnRegister_Click(object sender, EventArgs e)
            {
                if (string.IsNullOrWhiteSpace(txtFullName.Text))
                {
                    MessageBox.Show("Please enter your full name.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    MessageBox.Show("Please enter a username.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    MessageBox.Show("Please enter a password.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
                {
                    MessageBox.Show("Please confirm your password.");
                    return;
                }

                if (txtPassword.Text != txtConfirmPassword.Text)
                {
                    MessageBox.Show("Passwords do not match.");
                    return;
                }
                OleDbConnection con = DBConnection.GetConnection();

                try
                {
                    con.Open();
                string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username=?";

                OleDbCommand checkCmd =
                    new OleDbCommand(checkQuery, con);

                checkCmd.Parameters.AddWithValue(
                    "@Username",
                    txtUsername.Text);

                int count = Convert.ToInt32(
                    checkCmd.ExecuteScalar());

                if (count > 0)
                {
                    MessageBox.Show(
                        "Username already exists.");

                    con.Close();
                    return;
                }

                string query =
                        "INSERT INTO Users " +
                        "(FullName, Username, Password, Role) " +
                        "VALUES (?, ?, ?, ?)";

                    OleDbCommand cmd =
                        new OleDbCommand(query, con);

                    cmd.Parameters.AddWithValue("@FullName",
                        txtFullName.Text);

                    cmd.Parameters.AddWithValue("@Username",
                        txtUsername.Text);

                    cmd.Parameters.AddWithValue("@Password",
                        txtPassword.Text);

                    cmd.Parameters.AddWithValue("@Role",
                        "Applicant");

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Registration Successful!");

                    con.Close();

                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            private void btnLogin_Click(object sender, EventArgs e)
            {

            }

            private void txtPassword_TextChanged(object sender, EventArgs e)
            {

            }

            private void lblPassword_Click(object sender, EventArgs e)
            {

            }

            private void txtUsername_TextChanged(object sender, EventArgs e)
            {

            }

            private void lblUsername_Click(object sender, EventArgs e)
            {

            }

            private void label1_Click(object sender, EventArgs e)
            {

            }

            private void textBox1_TextChanged(object sender, EventArgs e)
            {

            }

            private void textBox1_TextChanged_1(object sender, EventArgs e)
            {

            }

            private void label1_Click_1(object sender, EventArgs e)
            {

            }

            private void RegisterForm_Load(object sender, EventArgs e)
            {

            }

            private void txtPassword_TextChanged_1(object sender, EventArgs e)
            {

            }

            private void btnBack_Click(object sender, EventArgs e)
            {
                this.Close();
            }
        }
    }
