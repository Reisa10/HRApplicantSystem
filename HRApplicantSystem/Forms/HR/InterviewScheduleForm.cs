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
    public partial class InterviewScheduleForm : Form
    {
        private int selectedApplicationID = 0;
        private DataTable dtApplications = new DataTable();
        public InterviewScheduleForm()
        {
            InitializeComponent();

            cboStatus.SelectedIndex = 0;
            cboMode.SelectedIndex = 0;

            LoadApplications();
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
                    Applications.Status
                FROM
                    (Applications
                    INNER JOIN Applicants
                    ON Applications.ApplicantID =
                       Applicants.ApplicantID)
                    INNER JOIN JobVacancies
                    ON Applications.JobID =
                       JobVacancies.JobID
                WHERE Applications.Status = 'Shortlisted'";

                    using (OleDbDataAdapter adapter =
                        new OleDbDataAdapter(query, conn))
                    {
                        dtApplications.Clear();

                        adapter.Fill(dtApplications);

                        dgvApplicants.DataSource =
                            dtApplications;

                        if (dgvApplicants.Columns["ApplicationID"] != null)
                            dgvApplicants.Columns["ApplicationID"].Visible = false;

                        if (dgvApplicants.Columns["Status"] != null)
                            dgvApplicants.Columns["Status"].Visible = false;

                        if (dgvApplicants.Columns["FullName"] != null)
                            dgvApplicants.Columns["FullName"].HeaderText =
                                "Applicant Name";

                        if (dgvApplicants.Columns["JobTitle"] != null)
                            dgvApplicants.Columns["JobTitle"].HeaderText =
                                "Job Title";

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

        private void dgvApplicants_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row =
                    dgvApplicants.Rows[e.RowIndex];

                selectedApplicationID =
                    Convert.ToInt32(
                        row.Cells["ApplicationID"].Value);

                lblApplicant.Text =
                    "Applicant: " +
                    row.Cells["FullName"].Value.ToString();

                lblJob.Text =
                    "Job: " +
                    row.Cells["JobTitle"].Value.ToString();
            }
        }

        private void InterviewScheduleForm_Load(object sender, EventArgs e)
        {
            LoadApplications();
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

            if (string.IsNullOrWhiteSpace(txtInterviewer.Text))
            {
                MessageBox.Show(
                    "Please enter the interviewer name.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (string.IsNullOrWhiteSpace(txtLocation.Text))
            {
                MessageBox.Show(
                    "Please enter the interview location.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }
            DialogResult result = 
                MessageBox.Show(
                    "Save this interview schedule?",
                    "Confirm Schedule",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
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

                string insertSchedule =
    @"INSERT INTO InterviewSchedules
    (
        ApplicationID,
        InterviewDate,
        Interviewer,
        Location,
        Mode,
        Status
    )
    VALUES
    (?, ?, ?, ?, ?, ?)";

                using (OleDbCommand cmd =
    new OleDbCommand(
        insertSchedule,
        conn,
        transaction))
                {
                    cmd.Parameters.Add(
                        "@ApplicationID",
                        OleDbType.Integer)
                        .Value = selectedApplicationID;

                    cmd.Parameters.Add(
                        "@InterviewDate",
                        OleDbType.Date)
                        .Value = dtpInterviewDate.Value;

                    cmd.Parameters.Add(
                        "@Interviewer",
                        OleDbType.VarWChar)
                        .Value = txtInterviewer.Text.Trim();

                    cmd.Parameters.Add(
                        "@Location",
                        OleDbType.VarWChar)
                        .Value = txtLocation.Text.Trim();

                    cmd.Parameters.Add(
                        "@Mode",
                        OleDbType.VarWChar)
                        .Value = cboMode.Text;

                    cmd.Parameters.Add(
                        "@Status",
                        OleDbType.VarWChar)
                        .Value = cboStatus.Text;

                    cmd.ExecuteNonQuery();
                }
                string updateApplication =
    @"UPDATE Applications
      SET Status = ?
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
                        .Value = "Interview Scheduled";

                    cmd.Parameters.Add(
                        "@ApplicationID",
                        OleDbType.Integer)
                        .Value = selectedApplicationID;

                    cmd.ExecuteNonQuery();
                }

                string insertHistory =
    @"INSERT INTO
        ApplicationStatusHistory
        (
            ApplicationID,
            Status,
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
                        .Value = selectedApplicationID;

                    cmd.Parameters.Add(
                        "@Status",
                        OleDbType.VarWChar)
                        .Value = "Interview Scheduled";

                    cmd.Parameters.Add(
                        "@DateChanged",
                        OleDbType.Date)
                        .Value = DateTime.Now;

                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();

                MessageBox.Show(
                    "Interview schedule saved successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                selectedApplicationID = 0;

                lblApplicant.Text =
                    "Applicant: -";

                lblJob.Text =
                    "Job: -";

                txtInterviewer.Clear();

                txtLocation.Clear();

                LoadApplications();

            }
            catch (Exception ex)
            {
                transaction?.Rollback();

                MessageBox.Show(
                    "Error saving schedule:\n" +
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
    }
}
