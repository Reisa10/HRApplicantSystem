using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;
using HRApplicantSystem.Database;
using HRApplicantSystem.Classes;

namespace HRApplicantSystem.Forms.Applicant
{
    public partial class MyApplicationForm : Form
    {
        private DataTable dtApplications = new DataTable();
        private int currentApplicantId;

        public MyApplicationForm()
        {
            InitializeComponent();
            currentApplicantId = UserSession.UserID;
            ApplyModernStyles();
        }

        private void MyApplicationForm_Load(object sender, EventArgs e)
        {
            LoadApplications();
        }

        private void LoadApplications()
        {
            OleDbConnection conn = DBConnection.GetConnection();
            if (conn == null) return;

            try
            {
                // Join Applications with JobVacancies to display Job Title
                string query = "SELECT a.ApplicationID, j.JobTitle, j.Department, a.Status, a.DateApplied, a.JobID " +
                               "FROM (Applications a " +
                               "INNER JOIN JobVacancies j ON a.JobID = j.JobID) " +
                               "WHERE a.ApplicantID = ? " +
                               "ORDER BY a.DateApplied DESC";

                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.Add("@ApplicantID", OleDbType.Integer).Value = currentApplicantId;

                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                    {
                        dtApplications.Clear();
                        adapter.Fill(dtApplications);
                        dgvApplications.DataSource = dtApplications;

                        // Hide technical columns
                        if (dgvApplications.Columns["ApplicationID"] != null) dgvApplications.Columns["ApplicationID"].Visible = false;
                        if (dgvApplications.Columns["JobID"] != null) dgvApplications.Columns["JobID"].Visible = false;

                        // Formatting Headers
                        if (dgvApplications.Columns["JobTitle"] != null) dgvApplications.Columns["JobTitle"].HeaderText = "Job Title";
                        if (dgvApplications.Columns["Department"] != null) dgvApplications.Columns["Department"].HeaderText = "Department";
                        if (dgvApplications.Columns["Status"] != null) dgvApplications.Columns["Status"].HeaderText = "Status";
                        if (dgvApplications.Columns["DateApplied"] != null) dgvApplications.Columns["DateApplied"].HeaderText = "Date Applied";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading applications: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvApplications_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvApplications.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvApplications.SelectedRows[0];
                string status = row.Cells["Status"].Value?.ToString() ?? "";

                lblTitle.Text = row.Cells["JobTitle"].Value?.ToString() ?? "";
                lblStatus.Text = "Status: " + status;
                lblDate.Text = "Applied On: " + Convert.ToDateTime(row.Cells["DateApplied"].Value).ToString("g");

                // Lock/Unlock Logic: Handle permissions based on your current applicant-side actions
                if (status == "Draft")
                {
                    btnSubmit.Enabled = true;
                    btnWithdraw.Enabled = true;
                    lblLockMessage.Visible = false;
                }
                else if (status == "Submitted")
                {
                    btnSubmit.Enabled = false; // Already submitted, cannot re-submit
                    btnWithdraw.Enabled = true;  // Can still withdraw before review starts
                    lblLockMessage.Visible = false;
                }
                else if (status == "Withdrawn")
                {
                    btnSubmit.Enabled = false;
                    btnWithdraw.Enabled = false;
                    lblLockMessage.Text = "This application has been withdrawn.";
                    lblLockMessage.ForeColor = Color.Gray;
                    lblLockMessage.Visible = true;
                }
                else
                {
                    // Fallback for any other state (e.g. Under Review) managed by the HR modules
                    btnSubmit.Enabled = false;
                    btnWithdraw.Enabled = false;
                    lblLockMessage.Text = "This application is currently locked because HR review has started.";
                    lblLockMessage.ForeColor = Color.FromArgb(192, 57, 43); // Dark Red
                    lblLockMessage.Visible = true;
                }
            }
            else
            {
                lblTitle.Text = "Select an Application";
                lblStatus.Text = "Status: --";
                lblDate.Text = "Applied On: --";
                btnSubmit.Enabled = false;
                btnWithdraw.Enabled = false;
                lblLockMessage.Visible = false;
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (dgvApplications.SelectedRows.Count == 0) return;

            DataGridViewRow row = dgvApplications.SelectedRows[0];
            int applicationId = Convert.ToInt32(row.Cells["ApplicationID"].Value);

            DialogResult result = MessageBox.Show("Are you sure you want to officially submit this application?",
                "Confirm Submission", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                UpdateApplicationStatus(applicationId, "Submitted", "Applicant submitted the application.");
            }
        }

        private void btnWithdraw_Click(object sender, EventArgs e)
        {
            if (dgvApplications.SelectedRows.Count == 0) return;

            DataGridViewRow row = dgvApplications.SelectedRows[0];
            int applicationId = Convert.ToInt32(row.Cells["ApplicationID"].Value);

            DialogResult result = MessageBox.Show("Are you sure you want to withdraw this application? This action cannot be undone.",
                "Confirm Withdrawal", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                UpdateApplicationStatus(applicationId, "Withdrawn", "Applicant withdrew the application.");
            }
        }

        private void UpdateApplicationStatus(int applicationId, string newStatus, string historyRemarks)
        {
            OleDbConnection conn = DBConnection.GetConnection();
            if (conn == null) return;

            OleDbTransaction transaction = null;

            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();

                // 1. Update status in Applications table
                string updateQuery = "UPDATE Applications SET [Status] = ? WHERE ApplicationID = ?";
                using (OleDbCommand cmd = new OleDbCommand(updateQuery, conn, transaction))
                {
                    cmd.Parameters.Add("@Status", OleDbType.VarWChar).Value = newStatus;
                    cmd.Parameters.Add("@ApplicationID", OleDbType.Integer).Value = applicationId;
                    cmd.ExecuteNonQuery();
                }

                // 2. Insert tracking log in ApplicationStatusHistory table
                string historyQuery = "INSERT INTO ApplicationStatusHistory (ApplicationID, [Status], DateChanged) VALUES (?, ?, ?)";
                using (OleDbCommand historyCmd = new OleDbCommand(historyQuery, conn, transaction))
                {
                    historyCmd.Parameters.Add("@ApplicationID", OleDbType.Integer).Value = applicationId;
                    historyCmd.Parameters.Add("@Status", OleDbType.VarWChar).Value = newStatus;
                    historyCmd.Parameters.Add("@DateChanged", OleDbType.Date).Value = DateTime.Now;
                    historyCmd.ExecuteNonQuery();
                }

                transaction.Commit();
                MessageBox.Show($"Application state updated to '{newStatus}' successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadApplications();
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                MessageBox.Show("Failed to update status: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ApplyModernStyles()
        {
            this.BackColor = Color.FromArgb(245, 247, 250);

            btnSubmit.FlatStyle = FlatStyle.Flat;
            btnSubmit.FlatAppearance.BorderSize = 0;
            btnSubmit.BackColor = Color.FromArgb(39, 174, 96); // Soft green
            btnSubmit.ForeColor = Color.White;
            btnSubmit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            btnWithdraw.FlatStyle = FlatStyle.Flat;
            btnWithdraw.FlatAppearance.BorderSize = 0;
            btnWithdraw.BackColor = Color.FromArgb(192, 57, 43); // Dark Red
            btnWithdraw.ForeColor = Color.White;
            btnWithdraw.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.FlatAppearance.BorderColor = Color.FromArgb(127, 140, 141);
            btnBack.BackColor = Color.White;
            btnBack.ForeColor = Color.FromArgb(44, 62, 80);
            btnBack.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        }
    }
}