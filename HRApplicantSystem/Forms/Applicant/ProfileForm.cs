using System;
using System.Data.OleDb;
using System.Windows.Forms;

namespace ProfileForm
{
    public partial class ProfileForm : Form
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();
        private int _applicantId = 0; // 0 = new profile, > 0 = editing existing
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=HRDatabase.accdb;";


        public ProfileForm()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtApplicantID.Text))
            {
                MessageBox.Show("Please enter an Applicant ID.");
                return;
            }

            int applicantID = Convert.ToInt32(txtApplicantID.Text);

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();

                // Check if profile already exists
                string checkQuery = "SELECT COUNT(*) FROM Applicants WHERE ApplicantID =?";
                using (OleDbCommand checkCmd = new OleDbCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("?", applicantID);
                    int exists = (int)checkCmd.ExecuteScalar();

                    if (exists > 0)
                    {
                        MessageBox.Show("Profile already exists. Use Update instead.");
                        return;
                    }
                }

                // Insert new profile
                string query = "INSERT INTO Applicants (ApplicantID, FirstName, LastName, Birthday, Gender, Address, ContactNumber, Education, Skills, WorkExperience) VALUES (?,?,?,?,?,?,?,?,?,?)";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("?", applicantID);
                    cmd.Parameters.AddWithValue("?", txtFirstName.Text);
                    cmd.Parameters.AddWithValue("?", txtLastName.Text);
                    cmd.Parameters.AddWithValue("?", dtpBirthdate.Value.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("?", cmbGender.SelectedItem?.ToString() ?? "");
                    cmd.Parameters.AddWithValue("?", txtAddress.Text);
                    cmd.Parameters.AddWithValue("?", txtPhoneNum.Text);
                    cmd.Parameters.AddWithValue("?", txtEduc.Text);
                    cmd.Parameters.AddWithValue("?", txtSkills.Text);
                    cmd.Parameters.AddWithValue("?", txtWorkExp.Text);
                    cmd.ExecuteNonQuery();
                }
            }
            ClearFields();
            MessageBox.Show("Profile saved successfully!");
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtApplicantID.Text))
            {
                MessageBox.Show("Please enter an Applicant ID to update.");
                return;
            }

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Applicants SET FirstName =?, LastName =?, Birthday =?, Gender =?, Address =?, ContactNumber =?, Education =?, Skills =?, WorkExperience =? WHERE ApplicantID =?";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("?", txtFirstName.Text);
                    cmd.Parameters.AddWithValue("?", txtLastName.Text);
                    cmd.Parameters.AddWithValue("?", dtpBirthdate.Value.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("?", cmbGender.SelectedItem?.ToString() ?? "");
                    cmd.Parameters.AddWithValue("?", txtAddress.Text);
                    cmd.Parameters.AddWithValue("?", txtPhoneNum.Text);
                    cmd.Parameters.AddWithValue("?", txtEduc.Text);
                    cmd.Parameters.AddWithValue("?", txtSkills.Text);
                    cmd.Parameters.AddWithValue("?", txtWorkExp.Text);
                    cmd.Parameters.AddWithValue("?", Convert.ToInt32(txtApplicantID.Text));
                    cmd.ExecuteNonQuery();
                }
            }
            ClearFields();
            MessageBox.Show("Profile updated successfully!");
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            txtApplicantID.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            dtpBirthdate.Value = DateTime.Now;
            cmbGender.SelectedIndex = -1;
            txtAddress.Clear();
            txtPhoneNum.Clear();
            txtEduc.Clear();
            txtSkills.Clear();
            txtWorkExp.Clear();
        }
    }
}