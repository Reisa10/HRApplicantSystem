using HRApplicantSystem.Database;
using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace HRApplicantSystem.Forms.HR
{
    public partial class AssessmentEvaluationForm : Form
    {
        private int selectedApplicationID = 0;
        private DataTable dtApplications = new DataTable();

        public AssessmentEvaluationForm()
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
                    Applications.DateApplied
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
                WHERE Applications.Status = 'For Assessment'";

                    using (OleDbDataAdapter adapter =
                        new OleDbDataAdapter(query, conn))
                    {
                        dtApplications.Clear();
                        adapter.Fill(dtApplications);
                        dgvApplicants.DataSource = dtApplications;

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
                    "Error loading applicants:\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void AssessmentEvaluationForm_Load(object sender, EventArgs e)
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

            if (string.IsNullOrWhiteSpace(txtAssessmentType.Text))
            {
                MessageBox.Show(
                    "Please enter the assessment type.",
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

            string result =
                rdoPass.Checked ? "Pass" : "Fail";

            string applicationStatus =
                rdoPass.Checked
                ? "For Final Decision"
                : "Rejected";

            DialogResult confirm = MessageBox.Show(
                "Save assessment evaluation for this applicant?",
                "Confirm",
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

                // Insert into AssessmentEvaluations table
                string insertAssessment = @"
                    INSERT INTO AssessmentEvaluations
                    (
                        ApplicationID,
                        AssessmentType,
                        Score,
                        Result,
                        Remarks,
                        AssessedBy,
                        DateAssessed
                    )
                    VALUES (?, ?, ?, ?, ?, ?, ?)";

                using (OleDbCommand cmd =
                    new OleDbCommand(
                        insertAssessment,
                        conn,
                        transaction))
                {
                    cmd.Parameters.Add(
                        "@ApplicationID",
                        OleDbType.Integer)
                        .Value = selectedApplicationID;

                    cmd.Parameters.Add(
                        "@AssessmentType",
                        OleDbType.VarWChar)
                        .Value = txtAssessmentType.Text.Trim();

                    cmd.Parameters.Add(
                        "@Score",
                        OleDbType.Integer)
                        .Value = Convert.ToInt32(nudScore.Value);

                    cmd.Parameters.Add(
                        "@Result",
                        OleDbType.VarWChar)
                        .Value = result;

                    cmd.Parameters.Add(
                        "@Remarks",
                        OleDbType.VarWChar)
                        .Value = txtRemarks.Text.Trim();

                    cmd.Parameters.Add(
                        "@AssessedBy",
                        OleDbType.Integer)
                        .Value = DBNull.Value;

                    cmd.Parameters.Add(
                        "@DateAssessed",
                        OleDbType.Date)
                        .Value = DateTime.Now;

                    cmd.ExecuteNonQuery();
                }

                // Update Applications.Status
                string updateApplication = @"
                    UPDATE Applications
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
                        .Value = applicationStatus;

                    cmd.Parameters.Add(
                        "@ApplicationID",
                        OleDbType.Integer)
                        .Value = selectedApplicationID;

                    cmd.ExecuteNonQuery();
                }

                // Insert into ApplicationStatusHistory
                string insertHistory = @"
                    INSERT INTO ApplicationStatusHistory
                    (
                        ApplicationID,
                        [Status],
                        DateChanged
                    )
                    VALUES (?, ?, ?)";

                using (OleDbCommand cmd =
                    new OleDbCommand(
                        insertHistory,
                        conn,
                        transaction))
                {
                    cmd.Parameters.Add(
                        "@ApplicationID",
                        OleDbType.Integer)
                        .Value = selectedApplicationID;

                    cmd.Parameters.Add(
                        "@Status",
                        OleDbType.VarWChar)
                        .Value = applicationStatus;

                    cmd.Parameters.Add(
                        "@DateChanged",
                        OleDbType.Date)
                        .Value = DateTime.Now;

                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();

                MessageBox.Show(
                    "Assessment evaluation saved successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Reset form
                selectedApplicationID = 0;
                lblApplicant.Text = "Applicant: —";
                lblJob.Text = "Job: —";
                txtAssessmentType.Clear();
                txtRemarks.Clear();
                nudScore.Value = 75;
                rdoPass.Checked = true;

                LoadApplications();
            }
            catch (Exception ex)
            {
                transaction?.Rollback();

                MessageBox.Show(
                    "Error saving assessment:\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
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
