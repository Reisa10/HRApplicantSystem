using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.OleDb;
using HRApplicantSystem.Database;

namespace HRApplicantSystem.Forms.HR
{
    
    public partial class ApplicantReviewForm : Form
    {
        private int selectedApplicantID = 0;
        private int selectedApplicationID = 0;
        private void LoadApplications()
        {
            try
            {
                using (OleDbConnection con =
                    DBConnection.GetConnection())
                {
                    con.Open();

                    string query =
                        @"SELECT
                    Applications.ApplicationID,
                    Applicants.ApplicantID,
                    Applicants.FirstName,
                    Applicants.LastName,
                    JobVacancies.JobTitle,
                    Applications.Status
                FROM
                    (Applications
                    INNER JOIN Applicants
                    ON Applications.ApplicantID =
                    Applicants.ApplicantID)
                INNER JOIN JobVacancies
                    ON Applications.JobID =
                    JobVacancies.JobID";

                    OleDbDataAdapter da =
                        new OleDbDataAdapter(query, con);

                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    dgvApplications.DataSource = dt;

                    dgvApplications.Columns["ApplicantID"].Visible = false; //If dont wnat to sho ApplicantID in the grid

                    foreach (DataGridViewRow row in dgvApplications.Rows)  //Change color if status is locked
                    {
                        if (row.Cells["Status"].Value != null &&
                            row.Cells["Status"].Value.ToString() == "Locked")
                        {
                            row.DefaultCellStyle.BackColor =
                                Color.LightGray;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public ApplicantReviewForm()
        {
            InitializeComponent();
        }

        private void ApplicantReviewForm_Load(object sender, EventArgs e)
        {
            LoadApplications();
        }

        private void LoadProfile()
        {
            try
            {
                using (OleDbConnection con =
                    DBConnection.GetConnection())
                {
                    con.Open();

                    string query =
                        "SELECT * FROM Applicants " +
                        "WHERE ApplicantID = ?";

                    OleDbCommand cmd =
                        new OleDbCommand(query, con);

                    cmd.Parameters.AddWithValue(
                        "@ApplicantID",
                        selectedApplicantID);

                    OleDbDataReader reader =
                        cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        txtReviewFirstName.Text =
                            reader["FirstName"].ToString();

                        txtReviewLastName.Text =
                            reader["LastName"].ToString();

                        txtReviewContact.Text =
                            reader["ContactNumber"].ToString();

                        txtReviewEducation.Text =
                            reader["Education"].ToString();

                        txtReviewSkills.Text =
                            reader["Skills"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadDocuments()
        {
            lstApplicantDocuments.Items.Clear();

            try
            {
                using (OleDbConnection con =
                    DBConnection.GetConnection())
                {
                    con.Open();

                    string query =
                        "SELECT DocumentName " +
                        "FROM ApplicantDocuments " +
                        "WHERE ApplicantID = ?";

                    OleDbCommand cmd =
                        new OleDbCommand(query, con);

                    cmd.Parameters.AddWithValue(
                        "@ApplicantID",
                        selectedApplicantID);

                    OleDbDataReader reader =
                        cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        lstApplicantDocuments.Items.Add(
                            reader["DocumentName"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LockApplication()
        {
            try
            {
                using (OleDbConnection con =
                    DBConnection.GetConnection())
                {
                    con.Open();

                    string query =
                        "UPDATE Applications " +
                        "SET Status = 'Locked' " +
                        "WHERE ApplicationID = ?";

                    OleDbCommand cmd =
                        new OleDbCommand(query, con);

                    cmd.Parameters.AddWithValue(
                        "@ApplicationID",
                        selectedApplicationID);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show(
                        "Application Locked!");

                    LoadApplications();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void btnLoadProfile_Click(object sender, EventArgs e)
        {
            if (selectedApplicantID == 0)
            {
                MessageBox.Show(
                    "Please select an application first.");
                return;
            }

            LoadProfile();
        }

        private void btnLoadDocuments_Click(object sender, EventArgs e)
        {
            {
                if (selectedApplicantID == 0)
                {
                    MessageBox.Show(
                        "Please select an application first.");
                    return;
                }

                LoadDocuments();
            }
        }

        private void btnLockApplication_Click(object sender, EventArgs e)
        {
            if (selectedApplicationID == 0)
            {
                MessageBox.Show(
                    "Please select an application first.");
                return;
            }

            DialogResult result =
                MessageBox.Show(
                    "Lock this application?",
                    "Confirm",
                    MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                LockApplication();
            }
        }

        private void dgvApplications_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvApplications_CellClick(object sender,DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedApplicationID =
                    Convert.ToInt32(
                    dgvApplications.Rows[e.RowIndex]
                    .Cells["ApplicationID"].Value);

                selectedApplicantID =
                    Convert.ToInt32(
                    dgvApplications.Rows[e.RowIndex]
                    .Cells["ApplicantID"].Value);

                MessageBox.Show(
                    "Applicant ID: " + selectedApplicantID);
            }
        }
    }
}
