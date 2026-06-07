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
    public partial class InterviewEvaluationForm : Form
    {
        private int selectedApplicationID = 0;
        private DataTable dtApplications = new DataTable();
        public InterviewEvaluationForm()
        {
            InitializeComponent();
        }
        private void LoadApplications()
        {
            try
            {
                using (OleDbConnection conn =
                    DBConnection.GetConnection())
                {
                    if (conn == null)
                        return;

                    string query = @"
                SELECT
                    Applications.ApplicationID,
                    Applicants.FirstName & ' ' &
                    Applicants.LastName AS FullName,
                    JobVacancies.JobTitle,
                    InterviewSchedules.InterviewDate,
                    InterviewSchedules.Interviewer
                FROM
                    (
                        (
                            Applications
                            INNER JOIN Applicants
                            ON Applications.ApplicantID =
                               Applicants.ApplicantID
                        )
                        INNER JOIN JobVacancies
                        ON Applications.JobID =
                           JobVacancies.JobID
                    )
                    INNER JOIN InterviewSchedules
                    ON Applications.ApplicationID =
                       InterviewSchedules.ApplicationID
                WHERE Applications.Status =
                      'Interview Scheduled'";

                    using (OleDbDataAdapter adapter =
                        new OleDbDataAdapter(query, conn))
                    {
                        dtApplications.Clear();

                        adapter.Fill(dtApplications);

                        dgvApplicants.DataSource =
                            dtApplications;

                        if (dgvApplicants.Columns["ApplicationID"] != null)
                            dgvApplicants.Columns["ApplicationID"].Visible = false;

                        dgvApplicants.AutoSizeColumnsMode =
                            DataGridViewAutoSizeColumnsMode.Fill;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading applicants:\n" +
                    ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void InterviewEvaluationForm_Load(object sender, EventArgs e)
        {
            LoadApplications();

            rdoPass.Checked = true;
        }

        private void dgvApplicants_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvApplicants.SelectedRows.Count > 0)
            {
                DataGridViewRow row =
                    dgvApplicants.SelectedRows[0];

                selectedApplicationID =
                    Convert.ToInt32(
                        row.Cells["ApplicationID"].Value);

                lblApplicant.Text =
                    "Applicant: " +
                    row.Cells["FullName"].Value?.ToString();

                lblJob.Text =
                    "Job: " +
                    row.Cells["JobTitle"].Value?.ToString();

                lblInterviewDate.Text =
                    "Interview Date: " +
                    Convert.ToDateTime(
                        row.Cells["InterviewDate"].Value)
                    .ToShortDateString();
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

            if (string.IsNullOrWhiteSpace(
                txtRecommendation.Text))
            {
                MessageBox.Show(
                    "Please enter recommendation.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }
            string result =
    rdoPass.Checked
    ? "Pass"
    : "Fail";

            string applicationStatus =
                rdoPass.Checked
                ? "For Final Decision"
                : "Rejected";

            DialogResult confirm =
    MessageBox.Show(
        "Save interview evaluation?",
        "Confirm",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            OleDbConnection conn =
    DBConnection.GetConnection();

            if (conn == null)
                return;

            OleDbTransaction transaction = null;

            try
            {
                conn.Open();

                transaction =
                    conn.BeginTransaction();

                string insertEvaluation =
    @"INSERT INTO InterviewEvaluations
    (
        ApplicationID,
        Score,
        Remarks,
        Result,
        Recommendation
    )
    VALUES
    (?, ?, ?, ?, ?)";

                using (OleDbCommand cmd =
    new OleDbCommand(
        insertEvaluation,
        conn,
        transaction))
                {
                    cmd.Parameters.Add(
                        "@ApplicationID",
                        OleDbType.Integer)
                        .Value = selectedApplicationID;

                    cmd.Parameters.Add(
                        "@Score",
                        OleDbType.Integer)
                        .Value =
                        Convert.ToInt32(
                            nudScore.Value);

                    cmd.Parameters.Add(
                        "@Remarks",
                        OleDbType.VarWChar)
                        .Value =
                        txtRemarks.Text.Trim();

                    cmd.Parameters.Add(
                        "@Result",
                        OleDbType.VarWChar)
                        .Value = result;

                    cmd.Parameters.Add(
                        "@Recommendation",
                        OleDbType.VarWChar)
                        .Value =
                        txtRecommendation.Text.Trim();

                    cmd.ExecuteNonQuery();
                }

                string updateApplication =
    @"UPDATE Applications
      SET [Status] = ?
      WHERE ApplicationID = ?";

                using (OleDbCommand cmd =
    new OleDbCommand(
        updateApplication,
        conn,
        transaction))
                {
                    cmd.Parameters.Add(
                        "@Status",
                        OleDbType.VarWChar)
                        .Value =
                        applicationStatus;

                    cmd.Parameters.Add(
                        "@ApplicationID",
                        OleDbType.Integer)
                        .Value =
                        selectedApplicationID;

                    cmd.ExecuteNonQuery();
                }
                string insertHistory =
    @"INSERT INTO
      ApplicationStatusHistory
      (
        ApplicationID,
        [Status],
        DateChanged
      )
      VALUES
      (?, ?, ?)";
                using (OleDbCommand cmd =
    new OleDbCommand(
        insertHistory,
        conn,
        transaction))
                {
                    cmd.Parameters.Add(
                        "@ApplicationID",
                        OleDbType.Integer)
                        .Value =
                        selectedApplicationID;

                    cmd.Parameters.Add(
                        "@Status",
                        OleDbType.VarWChar)
                        .Value =
                        applicationStatus;

                    cmd.Parameters.Add(
                        "@DateChanged",
                        OleDbType.Date)
                        .Value =
                        DateTime.Now;

                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();

                MessageBox.Show(
                    "Interview evaluation saved.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                selectedApplicationID = 0;

                lblApplicant.Text =
                    "Applicant: -";

                lblJob.Text =
                    "Job: -";

                lblInterviewDate.Text =
                    "Interview Date: -";

                txtRemarks.Clear();
                txtRecommendation.Clear();

                nudScore.Value = 75;

                LoadApplications();
            }
            catch (Exception ex)
            {
                transaction?.Rollback();

                MessageBox.Show(
                    "Error saving evaluation:\n" +
                    ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State ==
                    ConnectionState.Open)
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
