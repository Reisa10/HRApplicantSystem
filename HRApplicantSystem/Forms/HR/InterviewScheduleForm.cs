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
        private int existingScheduleID = 0;  // tracks if selected applicant already has a schedule
        private string selectedApplicantStatus = ""; // tracks current application status
        private DataTable dtApplications = new DataTable();

        public InterviewScheduleForm()
        {
            InitializeComponent();

            cboStatus.SelectedIndex = 2; // "Scheduled"
            cboMode.SelectedIndex = 0;

            LoadApplications();
        }

        // ─────────────────────────────────────────────────────────────────────
        // LOAD — show Shortlisted, Interview Scheduled, and Cancelled-interview
        // applicants so they remain visible until truly accepted/rejected/on-hold
        // ─────────────────────────────────────────────────────────────────────
        private void LoadApplications()
        {
            try
            {
                using (OleDbConnection conn = DBConnection.GetConnection())
                {
                    if (conn == null) return;

                    // Include Shortlisted + Interview Scheduled + Interview Cancelled
                    // Exclude terminal statuses: Accepted, Rejected, On Hold, For Final Decision
                    string query = @"
                SELECT
                    Applications.ApplicationID,
                    Applicants.FirstName & ' ' & Applicants.LastName AS FullName,
                    JobVacancies.JobTitle,
                    Applications.Status
                FROM
                    (Applications
                    INNER JOIN Applicants
                        ON Applications.ApplicantID = Applicants.ApplicantID)
                    INNER JOIN JobVacancies
                        ON Applications.JobID = JobVacancies.JobID
                WHERE Applications.Status IN
                      ('Shortlisted', 'Interview Scheduled', 'Interview Cancelled')";

                    using (OleDbDataAdapter adapter =
                        new OleDbDataAdapter(query, conn))
                    {
                        dtApplications.Clear();
                        adapter.Fill(dtApplications);

                        dgvApplicants.DataSource = dtApplications;

                        if (dgvApplicants.Columns["ApplicationID"] != null)
                            dgvApplicants.Columns["ApplicationID"].Visible = false;

                        if (dgvApplicants.Columns["FullName"] != null)
                            dgvApplicants.Columns["FullName"].HeaderText = "Applicant Name";

                        if (dgvApplicants.Columns["JobTitle"] != null)
                            dgvApplicants.Columns["JobTitle"].HeaderText = "Job Title";

                        if (dgvApplicants.Columns["Status"] != null)
                            dgvApplicants.Columns["Status"].HeaderText = "Status";

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

        // ─────────────────────────────────────────────────────────────────────
        // ROW CLICK — populate form and check for an existing schedule
        // ─────────────────────────────────────────────────────────────────────
        private void dgvApplicants_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvApplicants.Rows[e.RowIndex];

            selectedApplicationID =
                Convert.ToInt32(row.Cells["ApplicationID"].Value);

            selectedApplicantStatus =
                row.Cells["Status"].Value?.ToString() ?? "";

            lblApplicant.Text =
                "Applicant: " + row.Cells["FullName"].Value?.ToString();

            lblJob.Text =
                "Job: " + row.Cells["JobTitle"].Value?.ToString();

            // Check if a schedule record already exists for this applicant
            existingScheduleID = GetExistingScheduleID(selectedApplicationID);

            // If a previous (possibly cancelled) schedule exists, pre-fill the fields
            if (existingScheduleID > 0)
                PopulateScheduleFields(existingScheduleID);

            // If the interview was cancelled, present the reschedule/cancel options
            if (selectedApplicantStatus == "Interview Cancelled")
                HandleCancelledInterview();
        }

        // ─────────────────────────────────────────────────────────────────────
        // HELPER — return ScheduleID (most recent) for an application, or 0
        // ─────────────────────────────────────────────────────────────────────
        private int GetExistingScheduleID(int applicationID)
        {
            try
            {
                using (OleDbConnection conn = DBConnection.GetConnection())
                {
                    if (conn == null) return 0;

                    // Access does not support TOP with a subquery ORDER BY easily,
                    // so we fetch all and take the max ScheduleID in code.
                    string query = @"
                        SELECT ScheduleID
                        FROM   InterviewSchedules
                        WHERE  ApplicationID = ?";

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.Add("@ApplicationID", OleDbType.Integer)
                            .Value = applicationID;

                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            int maxID = 0;
                            while (reader.Read())
                            {
                                int id = Convert.ToInt32(reader["ScheduleID"]);
                                if (id > maxID) maxID = id;
                            }
                            return maxID;
                        }
                    }
                }
            }
            catch
            {
                return 0;
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        // HELPER — pre-fill schedule fields from an existing schedule record
        // ─────────────────────────────────────────────────────────────────────
        private void PopulateScheduleFields(int scheduleID)
        {
            try
            {
                using (OleDbConnection conn = DBConnection.GetConnection())
                {
                    if (conn == null) return;

                    string query = @"
                        SELECT InterviewDate, Interviewer, Location, Mode, Status
                        FROM   InterviewSchedules
                        WHERE  ScheduleID = ?";

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.Add("@ScheduleID", OleDbType.Integer)
                            .Value = scheduleID;

                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                dtpInterviewDate.Value =
                                    Convert.ToDateTime(reader["InterviewDate"]);

                                txtInterviewer.Text =
                                    reader["Interviewer"]?.ToString();

                                txtLocation.Text =
                                    reader["Location"]?.ToString();

                                // Match combo items
                                cboMode.Text = reader["Mode"]?.ToString();
                                cboStatus.Text = reader["Status"]?.ToString();
                            }
                        }
                    }
                }
            }
            catch { /* Non-critical — leave fields at defaults */ }
        }

        // ─────────────────────────────────────────────────────────────────────
        // CANCELLED INTERVIEW — ask user: Reschedule or Cancel Fully?
        // ─────────────────────────────────────────────────────────────────────
        private void HandleCancelledInterview()
        {
            DialogResult choice = MessageBox.Show(
                "This applicant's interview was cancelled.\n\n" +
                "Click YES to Reschedule the interview.\n" +
                "Click NO to Cancel Fully and Reject the applicant.",
                "Cancelled Interview",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (choice == DialogResult.Yes)
            {
                // Enable fields and let the user fill in a new schedule
                EnableScheduleFields(true);
                btnSave.Text = "Save Reschedule";
                MessageBox.Show(
                    "Please update the schedule details and click 'Save Reschedule'.",
                    "Reschedule",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                // Reject the applicant — ask for a rejection remark
                RejectAndCancel();
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        // REJECT FULLY — sets application to Rejected and logs in history
        // Remark is captured via an input dialog-style prompt using txtLocation
        // (re-used as a remarks field here and cleared afterward)
        // ─────────────────────────────────────────────────────────────────────
        private void RejectAndCancel()
        {
            // Ask for a rejection remark using a simple InputBox pattern
            using (Form remarkForm = new Form())
            {
                remarkForm.Text = "Rejection Remark";
                remarkForm.Size = new Size(420, 180);
                remarkForm.StartPosition = FormStartPosition.CenterParent;
                remarkForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                remarkForm.MaximizeBox = false;
                remarkForm.MinimizeBox = false;

                Label lbl = new Label
                {
                    Text = "Enter rejection remark (will be saved to Interview Remarks):",
                    Location = new Point(12, 12),
                    Size = new Size(380, 32),
                    AutoSize = false
                };

                TextBox txtRemark = new TextBox
                {
                    Location = new Point(12, 50),
                    Size = new Size(380, 22),
                    Text = "Interview cancelled – applicant rejected."
                };

                Button btnOk = new Button
                {
                    Text = "Confirm Rejection",
                    DialogResult = DialogResult.OK,
                    Location = new Point(200, 90),
                    Size = new Size(140, 28)
                };

                Button btnCancel = new Button
                {
                    Text = "Cancel",
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(350, 90),
                    Size = new Size(60, 28)
                };

                remarkForm.Controls.AddRange(
                    new Control[] { lbl, txtRemark, btnOk, btnCancel });
                remarkForm.AcceptButton = btnOk;
                remarkForm.CancelButton = btnCancel;

                if (remarkForm.ShowDialog() != DialogResult.OK)
                {
                    // User backed out — clear selection
                    ClearSelection();
                    return;
                }

                string remark = txtRemark.Text.Trim();
                if (string.IsNullOrWhiteSpace(remark))
                    remark = "Interview cancelled – applicant rejected.";

                SaveRejection(remark);
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        // DB WRITE — persist the full rejection (status + history + remark)
        // ─────────────────────────────────────────────────────────────────────
        private void SaveRejection(string remark)
        {
            OleDbConnection conn = DBConnection.GetConnection();
            if (conn == null) return;

            OleDbTransaction transaction = null;

            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();

                // 1. Update InterviewSchedules — set Status to Cancelled, store remark
                //    We write the remark into the existing schedule's Notes/Remarks column.
                //    If your schema uses a different column name, adjust accordingly.
                if (existingScheduleID > 0)
                {
                    string updateSchedule = @"
                        UPDATE InterviewSchedules
                        SET    Status  = ?,
                               Remarks = ?
                        WHERE  ScheduleID = ?";

                    using (OleDbCommand cmd =
                        new OleDbCommand(updateSchedule, conn, transaction))
                    {
                        cmd.Parameters.Add("@Status", OleDbType.VarWChar)
                            .Value = "Cancelled";
                        cmd.Parameters.Add("@Remarks", OleDbType.VarWChar)
                            .Value = remark;
                        cmd.Parameters.Add("@ScheduleID", OleDbType.Integer)
                            .Value = existingScheduleID;
                        cmd.ExecuteNonQuery();
                    }
                }

                // 2. Update Applications status to Rejected
                string updateApp = @"
                    UPDATE Applications
                    SET    Status = ?
                    WHERE  ApplicationID = ?";

                using (OleDbCommand cmd =
                    new OleDbCommand(updateApp, conn, transaction))
                {
                    cmd.Parameters.Add("@Status", OleDbType.VarWChar)
                        .Value = "Rejected";
                    cmd.Parameters.Add("@ApplicationID", OleDbType.Integer)
                        .Value = selectedApplicationID;
                    cmd.ExecuteNonQuery();
                }

                // 3. Insert status history
                string insertHistory = @"
                    INSERT INTO ApplicationStatusHistory
                        (ApplicationID, Status, DateChanged)
                    VALUES (?, ?, ?)";

                using (OleDbCommand cmd =
                    new OleDbCommand(insertHistory, conn, transaction))
                {
                    cmd.Parameters.Add("@ApplicationID", OleDbType.Integer)
                        .Value = selectedApplicationID;
                    cmd.Parameters.Add("@Status", OleDbType.VarWChar)
                        .Value = "Rejected";
                    cmd.Parameters.Add("@DateChanged", OleDbType.Date)
                        .Value = DateTime.Now;
                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();

                MessageBox.Show(
                    "Applicant has been rejected and the cancellation remark has been saved.",
                    "Rejection Saved",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ClearSelection();
                LoadApplications();
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                MessageBox.Show(
                    "Error saving rejection:\n" + ex.Message,
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

        // ─────────────────────────────────────────────────────────────────────
        // FORM LOAD
        // ─────────────────────────────────────────────────────────────────────
        private void InterviewScheduleForm_Load(object sender, EventArgs e)
        {
            LoadApplications();
        }

        // ─────────────────────────────────────────────────────────────────────
        // SAVE — handles both new schedule and reschedule
        // ─────────────────────────────────────────────────────────────────────
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
            if (dtpInterviewDate.Value.Date < DateTime.Today)
            {
                MessageBox.Show(
                    "Invalid interview date.\n" +
                    "Please select today or a future date.",
                    "Invalid Date",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                dtpInterviewDate.Focus();
                return;
            }

            bool isReschedule = (existingScheduleID > 0);

            string confirmMsg = isReschedule
                ? "Save the rescheduled interview?"
                : "Save this interview schedule?";

            DialogResult result = MessageBox.Show(
                confirmMsg,
                "Confirm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            if (isReschedule)
                SaveReschedule();
            else
                SaveNewSchedule();
        }

        // ─────────────────────────────────────────────────────────────────────
        // DB WRITE — brand-new schedule
        // ─────────────────────────────────────────────────────────────────────
        private void SaveNewSchedule()
        {
            OleDbConnection conn = DBConnection.GetConnection();
            if (conn == null) return;

            OleDbTransaction transaction = null;

            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();

                string insertSchedule = @"
                    INSERT INTO InterviewSchedules
                        (ApplicationID, InterviewDate, Interviewer, Location, Mode, Status)
                    VALUES (?, ?, ?, ?, ?, ?)";

                using (OleDbCommand cmd =
                    new OleDbCommand(insertSchedule, conn, transaction))
                {
                    cmd.Parameters.Add("@ApplicationID", OleDbType.Integer)
                        .Value = selectedApplicationID;
                    cmd.Parameters.Add("@InterviewDate", OleDbType.Date)
                        .Value = dtpInterviewDate.Value;
                    cmd.Parameters.Add("@Interviewer", OleDbType.VarWChar)
                        .Value = txtInterviewer.Text.Trim();
                    cmd.Parameters.Add("@Location", OleDbType.VarWChar)
                        .Value = txtLocation.Text.Trim();
                    cmd.Parameters.Add("@Mode", OleDbType.VarWChar)
                        .Value = cboMode.Text;
                    cmd.Parameters.Add("@Status", OleDbType.VarWChar)
                        .Value = cboStatus.Text;
                    cmd.ExecuteNonQuery();
                }

                // Map the schedule status to the correct application status
                string appStatus = cboStatus.Text == "Cancelled"
                    ? "Interview Cancelled"
                    : "Interview Scheduled";

                UpdateApplicationStatusAndHistory(
                    conn, transaction,
                    selectedApplicationID,
                    appStatus);

                transaction.Commit();

                MessageBox.Show(
                    "Interview schedule saved successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ClearSelection();
                LoadApplications();
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                MessageBox.Show(
                    "Error saving schedule:\n" + ex.Message,
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

        // ─────────────────────────────────────────────────────────────────────
        // DB WRITE — reschedule an existing (cancelled) schedule record
        // ─────────────────────────────────────────────────────────────────────
        private void SaveReschedule()
        {
            OleDbConnection conn = DBConnection.GetConnection();
            if (conn == null) return;

            OleDbTransaction transaction = null;

            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();

                // Update the existing schedule row
                string updateSchedule = @"
                    UPDATE InterviewSchedules
                    SET    InterviewDate = ?,
                           Interviewer   = ?,
                           Location      = ?,
                           Mode          = ?,
                           Status        = ?
                    WHERE  ScheduleID = ?";

                using (OleDbCommand cmd =
                    new OleDbCommand(updateSchedule, conn, transaction))
                {
                    cmd.Parameters.Add("@InterviewDate", OleDbType.Date)
                        .Value = dtpInterviewDate.Value;
                    cmd.Parameters.Add("@Interviewer", OleDbType.VarWChar)
                        .Value = txtInterviewer.Text.Trim();
                    cmd.Parameters.Add("@Location", OleDbType.VarWChar)
                        .Value = txtLocation.Text.Trim();
                    cmd.Parameters.Add("@Mode", OleDbType.VarWChar)
                        .Value = cboMode.Text;
                    cmd.Parameters.Add("@Status", OleDbType.VarWChar)
                        .Value = "Scheduled";  // reset schedule status
                    cmd.Parameters.Add("@ScheduleID", OleDbType.Integer)
                        .Value = existingScheduleID;
                    cmd.ExecuteNonQuery();
                }

                UpdateApplicationStatusAndHistory(
                    conn, transaction,
                    selectedApplicationID,
                    "Interview Scheduled");

                transaction.Commit();

                MessageBox.Show(
                    "Interview rescheduled successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                btnSave.Text = "Save Schedule"; // reset button label
                ClearSelection();
                LoadApplications();
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                MessageBox.Show(
                    "Error saving reschedule:\n" + ex.Message,
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

        // ─────────────────────────────────────────────────────────────────────
        // HELPER — update Applications.Status and insert into history
        // ─────────────────────────────────────────────────────────────────────
        private void UpdateApplicationStatusAndHistory(
            OleDbConnection conn,
            OleDbTransaction transaction,
            int applicationID,
            string newStatus)
        {
            string updateApp = @"
                UPDATE Applications
                SET    Status = ?
                WHERE  ApplicationID = ?";

            using (OleDbCommand cmd =
                new OleDbCommand(updateApp, conn, transaction))
            {
                cmd.Parameters.Add("@Status", OleDbType.VarWChar)
                    .Value = newStatus;
                cmd.Parameters.Add("@ApplicationID", OleDbType.Integer)
                    .Value = applicationID;
                cmd.ExecuteNonQuery();
            }

            string insertHistory = @"
                INSERT INTO ApplicationStatusHistory
                    (ApplicationID, Status, DateChanged)
                VALUES (?, ?, ?)";

            using (OleDbCommand cmd =
                new OleDbCommand(insertHistory, conn, transaction))
            {
                cmd.Parameters.Add("@ApplicationID", OleDbType.Integer)
                    .Value = applicationID;
                cmd.Parameters.Add("@Status", OleDbType.VarWChar)
                    .Value = newStatus;
                cmd.Parameters.Add("@DateChanged", OleDbType.Date)
                    .Value = DateTime.Now;
                cmd.ExecuteNonQuery();
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        // HELPER — reset form state after a save/cancel action
        // ─────────────────────────────────────────────────────────────────────
        private void ClearSelection()
        {
            selectedApplicationID = 0;
            existingScheduleID = 0;
            selectedApplicantStatus = "";

            lblApplicant.Text = "Applicant: -";
            lblJob.Text = "Job: -";

            txtInterviewer.Clear();
            txtLocation.Clear();

            cboStatus.SelectedIndex = 2;
            cboMode.SelectedIndex = 0;

            btnSave.Text = "Save Schedule";

            EnableScheduleFields(true);
        }

        // ─────────────────────────────────────────────────────────────────────
        // HELPER — enable or disable schedule input fields
        // ─────────────────────────────────────────────────────────────────────
        private void EnableScheduleFields(bool enabled)
        {
            dtpInterviewDate.Enabled = enabled;
            txtInterviewer.Enabled = enabled;
            txtLocation.Enabled = enabled;
            cboMode.Enabled = enabled;
            cboStatus.Enabled = enabled;
            btnSave.Enabled = enabled;
        }

        // ─────────────────────────────────────────────────────────────────────
        // BACK
        // ─────────────────────────────────────────────────────────────────────
        private void btnBack_Click(object sender, EventArgs e)
        {
            HRDashboard dashboard = new HRDashboard();
            dashboard.Show();
            this.Close();
        }
    }
}
