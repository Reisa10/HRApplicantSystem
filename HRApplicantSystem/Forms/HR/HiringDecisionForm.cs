using HRApplicantSystem.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using HRApplicantSystem.Database;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HRApplicantSystem.Forms.HR
{
    public partial class HiringDecisionForm : Form
    {
        private int selectedApplicationID = 0;
        private DataTable dtApplications = new DataTable();

        public HiringDecisionForm()
        {
            InitializeComponent();

            if (UserSession.Role != "HR Manager" && UserSession.Role != "Admin")
            {
                MessageBox.Show("Access Denied. Only HR Manager or Admin can make hiring decisions.",
                    "Unauthorized", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Load += (s, e) => this.Close();
            }
        }

        private void LoadApplications()
        {
            try
            {
                using (OleDbConnection conn = DBConnection.GetConnection())
                {
                    if (conn == null)
                        return;

                    string query = @"
                SELECT
                    Applications.ApplicationID,
                    Applicants.FirstName & ' ' &
                    Applicants.LastName AS FullName,
                    JobVacancies.JobTitle
                FROM
                    (
                        Applications
                        INNER JOIN Applicants
                        ON Applications.ApplicantID =
                           Applicants.ApplicantID
                    )
                    INNER JOIN JobVacancies
                    ON Applications.JobID =
                       JobVacancies.JobID
                WHERE Applications.Status =
                      'For Final Decision'";

                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn))
                    {
                        dtApplications.Clear();
                        adapter.Fill(dtApplications);
                        dgvApplicants.DataSource = dtApplications;

                        if (dgvApplicants.Columns["ApplicationID"] != null)
                            dgvApplicants.Columns["ApplicationID"].Visible = false;

                        dgvApplicants.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading applicants:\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void HiringDecisionForm_Load(object sender, EventArgs e)
        {
            LoadApplications();
            rdoAccepted.Checked = true;
        }

        private void dgvApplicants_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvApplicants.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvApplicants.SelectedRows[0];
                selectedApplicationID = Convert.ToInt32(row.Cells["ApplicationID"].Value);

                lblApplicant.Text = "Applicant: " + row.Cells["FullName"].Value?.ToString();
                lblJob.Text = "Job: " + row.Cells["JobTitle"].Value?.ToString();
            }
            else
            {
                selectedApplicationID = 0;
                lblApplicant.Text = "Applicant: -";
                lblJob.Text = "Job: -";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (selectedApplicationID == 0)
            {
                MessageBox.Show(
                    "Please select an applicant first.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtRemarks.Text))
            {
                MessageBox.Show(
                    "Please enter remarks.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            string decision;
            if (rdoAccepted.Checked)
            {
                decision = "Accepted";
            }
            else if (rdoRejected.Checked)
            {
                decision = "Rejected";
            }
            else
            {
                decision = "On Hold";
            }

            DialogResult confirm = MessageBox.Show(
                "Save hiring decision?",
                "Confirm Decision",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            OleDbConnection conn = DBConnection.GetConnection();
            if (conn == null)
                return;

            OleDbTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();

                string insertDecision = @"
                    INSERT INTO HiringDecisions
                    (
                        ApplicationID,
                        Decision,
                        Remarks,
                        DecisionBy,
                        DecisionDate
                    )
                    VALUES
                    (?, ?, ?, ?, ?)";

                using (OleDbCommand cmd = new OleDbCommand(insertDecision, conn, transaction))
                {
                    cmd.Parameters.Add("@ApplicationID", OleDbType.Integer).Value = selectedApplicationID;
                    cmd.Parameters.Add("@Decision", OleDbType.VarWChar).Value = decision;
                    cmd.Parameters.Add("@Remarks", OleDbType.VarWChar).Value = txtRemarks.Text.Trim();
                    cmd.Parameters.Add("@DecisionBy", OleDbType.Integer).Value = UserSession.UserID > 0 ? UserSession.UserID : 1;
                    cmd.Parameters.Add("@DecisionDate", OleDbType.Date).Value = DateTime.Now;

                    cmd.ExecuteNonQuery();
                }

                string updateApplication = @"
                    UPDATE Applications
                    SET [Status] = ?
                    WHERE ApplicationID = ?";

                using (OleDbCommand cmd = new OleDbCommand(updateApplication, conn, transaction))
                {
                    cmd.Parameters.Add("@Status", OleDbType.VarWChar).Value = decision;
                    cmd.Parameters.Add("@ApplicationID", OleDbType.Integer).Value = selectedApplicationID;

                    cmd.ExecuteNonQuery();
                }

                string insertHistory = @"
                    INSERT INTO ApplicationStatusHistory
                    (
                        ApplicationID,
                        [Status],
                        DateChanged
                    )
                    VALUES
                    (?, ?, ?)";

                using (OleDbCommand cmd = new OleDbCommand(insertHistory, conn, transaction))
                {
                    cmd.Parameters.Add("@ApplicationID", OleDbType.Integer).Value = selectedApplicationID;
                    cmd.Parameters.Add("@Status", OleDbType.VarWChar).Value = decision;
                    cmd.Parameters.Add("@DateChanged", OleDbType.Date).Value = DateTime.Now;

                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();

                MessageBox.Show(
                    "Hiring decision saved successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                selectedApplicationID = 0;
                lblApplicant.Text = "Applicant: -";
                lblJob.Text = "Job: -";
                txtRemarks.Clear();
                rdoAccepted.Checked = true;

                LoadApplications();
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                MessageBox.Show(
                    "Error saving decision:\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            HRDashboard dashboard = new HRDashboard();
            dashboard.Show();
            this.Close();
        }
    }
}