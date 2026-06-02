using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using HRApplicantSystem.Database;
using HRApplicantSystem.Classes;
using HRApplicantSystem.Forms.Login;

namespace HRApplicantSystem.Forms.Applicant
{
    public partial class JobVacancyForm : Form
    {
        private DataTable dtJobs = new DataTable();

        public JobVacancyForm()
        {
            InitializeComponent();
        }

        private void JobVacancyForm_Load(object sender, EventArgs e)
        {
            LoadDepartments();
            LoadJobVacancies();
        }

        private void LoadJobVacancies()
        {
            try
            {
                using (OleDbConnection conn = DBConnection.GetConnection())
                {
                    if (conn == null) return;

                    string query = "SELECT JobID, JobTitle, Department, Description, Status " +
                                   "FROM JobVacancies WHERE Status = 'Open'";

                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn))
                    {
                        dtJobs.Clear();
                        adapter.Fill(dtJobs);
                        dgvJobs.DataSource = dtJobs;

                        if (dgvJobs.Columns["JobID"] != null) dgvJobs.Columns["JobID"].Visible = false;
                        if (dgvJobs.Columns["Description"] != null) dgvJobs.Columns["Description"].Visible = false;
                        if (dgvJobs.Columns["Status"] != null) dgvJobs.Columns["Status"].Visible = false;

                        if (dgvJobs.Columns["JobTitle"] != null) dgvJobs.Columns["JobTitle"].HeaderText = "Job Title";
                        if (dgvJobs.Columns["Department"] != null) dgvJobs.Columns["Department"].HeaderText = "Department";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading job vacancies: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDepartments()
        {
            cmbDepartment.Items.Clear();
            cmbDepartment.Items.Add("All Departments");

            try
            {
                using (OleDbConnection conn = DBConnection.GetConnection())
                {
                    if (conn == null) return;

                    string query = "SELECT DISTINCT Department FROM JobVacancies WHERE Status = 'Open'";
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        conn.Open();
                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string dept = reader["Department"]?.ToString();
                                if (!string.IsNullOrEmpty(dept))
                                {
                                    cmbDepartment.Items.Add(dept);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                cmbDepartment.Items.Add("IT");
                cmbDepartment.Items.Add("HR");
                cmbDepartment.Items.Add("Finance");
            }

            cmbDepartment.SelectedIndex = 0;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().Replace("'", "''");
            string selectedDept = cmbDepartment.SelectedItem?.ToString() ?? "All Departments";

            DataView dv = dtJobs.DefaultView;
            string filterExpr = "";

            if (!string.IsNullOrEmpty(keyword))
            {
                filterExpr = $"(JobTitle LIKE '%{keyword}%' OR Description LIKE '%{keyword}%')";
            }

            if (selectedDept != "All Departments")
            {
                if (!string.IsNullOrEmpty(filterExpr))
                    filterExpr += " AND ";
                filterExpr += $"Department = '{selectedDept}'";
            }

            dv.RowFilter = filterExpr;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            if (cmbDepartment.Items.Count > 0)
            {
                cmbDepartment.SelectedIndex = 0;
            }
            LoadJobVacancies();
        }

        private void dgvJobs_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvJobs.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvJobs.SelectedRows[0];

                lblTitle.Text = row.Cells["JobTitle"].Value?.ToString() ?? "";
                lblDept.Text = "Department: " + (row.Cells["Department"].Value?.ToString() ?? "N/A");
                txtDescription.Text = row.Cells["Description"].Value?.ToString() ?? "";

                grpJobDetails.Enabled = true;
            }
            else
            {
                lblTitle.Text = "Select a Job to View Details";
                lblDept.Text = "";
                txtDescription.Clear();
                grpJobDetails.Enabled = false;
            }
        }

        // Apply Button Click logic implementing your business rules
        // Apply Button Click logic with explicit OleDbTypes to prevent mismatch errors
        private void btnApply_Click(object sender, EventArgs e)
        {
            if (dgvJobs.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a job vacancy from the list first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Retrieve structural IDs
            DataGridViewRow row = dgvJobs.SelectedRows[0];
            int jobId = Convert.ToInt32(row.Cells["JobID"].Value);
            int applicantId = UserSession.UserID; // Set during login

            OleDbConnection conn = DBConnection.GetConnection();
            if (conn == null) return;

            try
            {
                conn.Open();

                // 1. DUPLICATE CHECK: Prevent same applicant from applying twice to the same job [4]
                string checkQuery = "SELECT COUNT(*) FROM Applications WHERE ApplicantID = ? AND JobID = ?";
                using (OleDbCommand checkCmd = new OleDbCommand(checkQuery, conn))
                {
                    // Explicitly defining Integer parameters
                    checkCmd.Parameters.Add("@ApplicantID", OleDbType.Integer).Value = applicantId;
                    checkCmd.Parameters.Add("@JobID", OleDbType.Integer).Value = jobId;

                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count > 0)
                    {
                        // Block duplicate application attempt [4]
                        MessageBox.Show("You have already applied for this job opening.", "Duplicate Application", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // 2. INSERT: Save record to Applications table
                // We use explicit OleDbTypes (especially OleDbType.Date for DateApplied) to prevent type mismatches
                string insertQuery = "INSERT INTO Applications (ApplicantID, JobID, [Status], DateApplied) VALUES (?, ?, ?, ?)";
                using (OleDbCommand insertCmd = new OleDbCommand(insertQuery, conn))
                {
                    insertCmd.Parameters.Add("@ApplicantID", OleDbType.Integer).Value = applicantId;
                    insertCmd.Parameters.Add("@JobID", OleDbType.Integer).Value = jobId;
                    insertCmd.Parameters.Add("@Status", OleDbType.VarWChar).Value = "Submitted"; // Escaped [Status]
                    insertCmd.Parameters.Add("@DateApplied", OleDbType.Date).Value = DateTime.Now; // Correct Date format for Access

                    insertCmd.ExecuteNonQuery();
                }

                MessageBox.Show("Your application has been submitted successfully!", "Application Submitted", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while submitting your application:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            UserSession.UserID = 0;
            UserSession.Username = null;
            UserSession.FullName = null;
            UserSession.Role = null;

            LoginForm loginForm = new LoginForm();
            loginForm.Show();

            this.Close();
        }
    }
}