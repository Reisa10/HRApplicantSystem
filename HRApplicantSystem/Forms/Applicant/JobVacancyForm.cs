using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using HRApplicantSystem.Database; // Connects to your database helper namespace

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

        // Retrieves all open job vacancies from your database helper
        private void LoadJobVacancies()
        {
            try
            {
                using (OleDbConnection conn = DBConnection.GetConnection())
                {
                    if (conn == null)
                    {
                        return; // DBConnection already warns the user if connection fails
                    }

                    // Query matches your exact columns: JobID, JobTitle, Department, Description, Status
                    string query = "SELECT JobID, JobTitle, Department, Description, Status " +
                                   "FROM JobVacancies WHERE Status = 'Open'";

                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn))
                    {
                        dtJobs.Clear();
                        adapter.Fill(dtJobs);
                        dgvJobs.DataSource = dtJobs;

                        // Hide technical columns from display
                        if (dgvJobs.Columns["JobID"] != null) dgvJobs.Columns["JobID"].Visible = false;
                        if (dgvJobs.Columns["Description"] != null) dgvJobs.Columns["Description"].Visible = false;
                        if (dgvJobs.Columns["Status"] != null) dgvJobs.Columns["Status"].Visible = false;

                        // Customize visible column headers
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

        // Populates search dropdown filter with unique departments from active vacancies
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
                // Fallback options in case table contains no records yet
                cmbDepartment.Items.Add("IT");
                cmbDepartment.Items.Add("HR");
                cmbDepartment.Items.Add("Finance");
            }

            cmbDepartment.SelectedIndex = 0;
        }

        // Logic for search filtering of the local grid data
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

        // Populates the detail viewing section when a grid row is clicked
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
    }
}