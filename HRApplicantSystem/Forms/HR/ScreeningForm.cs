using HRApplicantSystem.Classes;
using HRApplicantSystem.Database;
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

namespace HRApplicantSystem.Forms.HR
{
    public partial class ScreeningForm : Form
    {
        // Stores the ApplicationID of whichever row the HR clicks in the grid
        private int selectedApplicationID = 0;

        // This holds the data we load from the database
        private DataTable dtApplications = new DataTable();

        public ScreeningForm()
        {
            InitializeComponent();
            if (UserSession.Role != "HR Staff" && UserSession.Role != "HR Manager" && UserSession.Role != "Admin")
            {
                MessageBox.Show("Access Denied. You do not have permission to access Screening.",
                    "Unauthorized", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Load += (s, e) => this.Close();
            }
        }

        private void ScreeningForm_Load(object sender, EventArgs e)
        {
            LoadApplications();
        }

        // ─────────────────────────────────────────────────────────────────
        // LOAD APPLICATIONS
        // Reads all applications with status "Under Review" from the database
        // and shows them in the DataGridView.
        // ─────────────────────────────────────────────────────────────────
        private void LoadApplications()
        {
            try
            {
                using (OleDbConnection conn = DBConnection.GetConnection())
                {
                    if (conn == null) return;

                    // Fetch ApplicantID [2]
                    string query = @"
                        SELECT Applications.ApplicationID,
                               Applicants.ApplicantID,
                               Applicants.FirstName & ' ' & Applicants.LastName AS FullName,
                               JobVacancies.JobTitle,
                               Applications.Status
                        FROM (Applications
                        INNER JOIN Applicants ON Applications.ApplicantID = Applicants.ApplicantID)
                        INNER JOIN JobVacancies ON Applications.JobID = JobVacancies.JobID
                        WHERE Applications.Status = 'Under Review'";

                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn))
                    {
                        dtApplications.Clear();
                        adapter.Fill(dtApplications);

                        // Point the DataGridView to our DataTable
                        dgvApplications.DataSource = dtApplications;

                        // Hide structural columns from the DataGridView
                        if (dgvApplications.Columns["ApplicationID"] != null)
                            dgvApplications.Columns["ApplicationID"].Visible = false;
                        if (dgvApplications.Columns["ApplicantID"] != null)
                            dgvApplications.Columns["ApplicantID"].Visible = false;
                        if (dgvApplications.Columns["Status"] != null)
                            dgvApplications.Columns["Status"].Visible = false;

                        // Rename the column headers so they look nice
                        if (dgvApplications.Columns["FullName"] != null)
                            dgvApplications.Columns["FullName"].HeaderText = "Applicant Name";
                        if (dgvApplications.Columns["JobTitle"] != null)
                            dgvApplications.Columns["JobTitle"].HeaderText = "Job Title";

                        // Stretch both visible columns to fill the grid width
                        dgvApplications.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading applications:\n" + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ─────────────────────────────────────────────────────────────────
        // SELECTION CHANGED — fires every time the HR clicks a different row
        // ─────────────────────────────────────────────────────────────────
        private void dgvApplications_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvApplications.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvApplications.SelectedRows[0];

                selectedApplicationID = Convert.ToInt32(row.Cells["ApplicationID"].Value);
                int applicantId = Convert.ToInt32(row.Cells["ApplicantID"].Value);

                lblSelectedApplicant.Text = "Applicant:  " + row.Cells["FullName"].Value?.ToString();

                // Append the missing status directly inside the Job label to prevent layout overlaps [2]
                string jobTitle = row.Cells["JobTitle"].Value?.ToString() ?? "";
                List<string> missing = DatabaseHelper.GetMissingRequirements(applicantId);
                if (missing != null && missing.Count > 0)
                {
                    lblSelectedJob.Text = $"Job: {jobTitle}   [⚠️ Missing: {string.Join(", ", missing)}]";
                    lblSelectedJob.ForeColor = Color.FromArgb(192, 57, 43); // Red to highlight issues
                }
                else
                {
                    lblSelectedJob.Text = $"Job: {jobTitle}   [✅ All Docs Uploaded]";
                    lblSelectedJob.ForeColor = Color.FromArgb(39, 174, 96); // Green for complete
                }

                // Reset the decision fields and enable the group box
                rdoQualified.Checked = true;
                txtRemarks.Clear();
                grpDecision.Enabled = true;
            }
            else
            {
                // Nothing is selected — disable and reset the decision section
                selectedApplicationID = 0;
                lblSelectedApplicant.Text = "Applicant: —";
                lblSelectedJob.Text = "Job: —";
                lblSelectedJob.ForeColor = SystemColors.ControlText;
                grpDecision.Enabled = false;
            }
        }

        // ─────────────────────────────────────────────────────────────────
        // SAVE DECISION — fires when the HR clicks "Save Decision"
        // Does 3 things in one transaction:
        //   A) Insert a record into ScreeningResults
        //   B) Update the application's Status in Applications
        //   C) Insert a record into ApplicationStatusHistory
        // ─────────────────────────────────────────────────────────────────
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Guard: make sure an application is actually selected
            if (selectedApplicationID == 0)
            {
                MessageBox.Show(
                    "Please select an application from the list first.",
                    "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Guard: remarks must not be empty
            if (string.IsNullOrWhiteSpace(txtRemarks.Text))
            {
                MessageBox.Show(
                    "Please add HR remarks before saving the decision.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Figure out what the HR decided
            string decision = rdoQualified.Checked ? "Qualified" : "Not Qualified";
            string newStatus = rdoQualified.Checked ? "Shortlisted" : "Rejected";

            // Ask HR to confirm before saving
            DialogResult confirm = MessageBox.Show(
                "Mark this applicant as '" + decision + "'?\n\n" +
                "The application status will change to '" + newStatus + "'.",
                "Confirm Screening Decision",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            // ── Open connection and start transaction ──────────────────
            OleDbConnection conn = DBConnection.GetConnection();
            if (conn == null) return;

            OleDbTransaction transaction = null;

            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();

                // ── Step A: Save to ScreeningResults ───────────────────
                string insertScreening =
                    "INSERT INTO ScreeningResults " +
                    "(ApplicationID, ScreeningResult, Remarks, ScreenedBy, DateScreened) " +
                    "VALUES (?, ?, ?, ?, ?)";

                using (OleDbCommand cmd = new OleDbCommand(insertScreening, conn, transaction))
                {
                    cmd.Parameters.Add("@ApplicationID", OleDbType.Integer).Value = selectedApplicationID;
                    cmd.Parameters.Add("@ScreeningResult", OleDbType.VarWChar).Value = decision;
                    cmd.Parameters.Add("@Remarks", OleDbType.VarWChar).Value = txtRemarks.Text.Trim();
                    // UserSession.UserID is the logged-in HR's ID. Fallback to 1 if somehow 0.
                    cmd.Parameters.Add("@ScreenedBy", OleDbType.Integer).Value = UserSession.UserID > 0 ? UserSession.UserID : 1;
                    cmd.Parameters.Add("@DateScreened", OleDbType.Date).Value = DateTime.Now;
                    cmd.ExecuteNonQuery();
                }

                // ── Step B: Update Applications.Status ─────────────────
                string updateApp =
                    "UPDATE Applications SET [Status] = ? WHERE ApplicationID = ?";

                using (OleDbCommand cmd = new OleDbCommand(updateApp, conn, transaction))
                {
                    cmd.Parameters.Add("@Status", OleDbType.VarWChar).Value = newStatus;
                    cmd.Parameters.Add("@ApplicationID", OleDbType.Integer).Value = selectedApplicationID;
                    cmd.ExecuteNonQuery();
                }

                // ── Step C: Log to ApplicationStatusHistory ────────────
                string insertHistory =
                    "INSERT INTO ApplicationStatusHistory " +
                    "(ApplicationID, [Status], DateChanged) " +
                    "VALUES (?, ?, ?)";

                using (OleDbCommand cmd = new OleDbCommand(insertHistory, conn, transaction))
                {
                    cmd.Parameters.Add("@ApplicationID", OleDbType.Integer).Value = selectedApplicationID;
                    cmd.Parameters.Add("@Status", OleDbType.VarWChar).Value = newStatus;
                    cmd.Parameters.Add("@DateChanged", OleDbType.Date).Value = DateTime.Now;
                    cmd.ExecuteNonQuery();
                }

                // All 3 steps worked — commit everything
                transaction.Commit();

                MessageBox.Show(
                    "Screening saved!\n\n" +
                    "Applicant marked as '" + decision + "'.\n" +
                    "Status updated to '" + newStatus + "'.",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Reset the form and reload the list
                // (the screened applicant will disappear — they're no longer "Under Review")
                selectedApplicationID = 0;
                lblSelectedApplicant.Text = "Applicant: —";
                lblSelectedJob.Text = "Job: —";
                lblSelectedJob.ForeColor = SystemColors.ControlText;
                txtRemarks.Clear();
                grpDecision.Enabled = false;
                LoadApplications();
            }
            catch (Exception ex)
            {
                // Something failed — undo everything to keep the database consistent
                transaction?.Rollback();
                MessageBox.Show(
                    "Error saving screening result:\n" + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Always close the connection whether it succeeded or failed
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }

        // ─────────────────────────────────────────────────────────────────
        // BACK — just closes this form and returns to HR Dashboard
        // ─────────────────────────────────────────────────────────────────
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}