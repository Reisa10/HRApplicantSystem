using HRApplicantSystem.Database;
using HRApplicantSystem.Forms.HR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HRApplicantSystem.Forms.Login
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click_2(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            RegisterForm registerForm = new RegisterForm();

            registerForm.ShowDialog();
        }

        private void label1_Click_3(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Please enter your username.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter your password.");
                return;
            }

            OleDbConnection con = DBConnection.GetConnection();

            try
            {
                con.Open();

                string query =
                    "SELECT * FROM Users " +
                    "WHERE Username=? AND Password=?";

                OleDbCommand cmd =
                    new OleDbCommand(query, con);

                cmd.Parameters.AddWithValue(
                    "@Username",
                    txtUsername.Text);

                cmd.Parameters.AddWithValue(
                    "@Password",
                    txtPassword.Text);

                OleDbDataReader reader =
                    cmd.ExecuteReader();

                if (reader.Read())
                {
                    MessageBox.Show("Login Successful!");

                    HRDashboard dashboard =
                        new HRDashboard();

                    dashboard.Show();

                    this.Hide();
                }
                else
                {
                    MessageBox.Show(
                        "Invalid Username or Password");
                }

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
    }
}
