using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;
using System.Windows.Forms;
using HRApplicantSystem.Database;

namespace HRApplicantSystem.Forms.HR
{
    public partial class ReportsForm : Form
    {
        public ReportsForm()
        {
            InitializeComponent();
        }

        private void ReportsForm_Load(object sender, EventArgs e)
        {
            rdoAllApplicants.Checked = true;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (rdoAllApplicants.Checked) GenerateAllApplicants();
            else if (rdoPending.Checked) GeneratePendingApplications();
            else if (rdoInterviews.Checked) GenerateScheduledInterviews();
            else if (rdoHired.Checked) GenerateHiredRejected();
            else if (rdoMissingDocs.Checked) GenerateMissingRequirements();
        }

        // ── Report 1: All Applicants ───────────────────────────
        // FIX: Removed the period from "Contact No." alias — MS Access
        //      treats the dot as object notation and throws a bracket error.
        private void GenerateAllApplicants()
        {
            lblReportTitle.Text = "Report: All Applicants";

            string query =
                "SELECT " +
                "  a.ApplicantID, " +
                "  a.FirstName & ' ' & a.LastName AS [Full Name], " +
                "  aa.Email, " +
                "  a.ContactNumber AS [Contact Number], " +
                "  jv.JobTitle AS [Applied Position], " +
                "  app.Status AS [Application Status], " +
                "  app.DateApplied AS [Date Applied] " +
                "FROM ((Applicants a " +
                "  INNER JOIN ApplicantAccounts aa ON a.ApplicantID = aa.ApplicantID) " +
                "  LEFT JOIN Applications app ON a.ApplicantID = app.ApplicantID) " +
                "  LEFT JOIN JobVacancies jv ON app.JobID = jv.JobID " +
                "ORDER BY a.ApplicantID ASC";

            LoadReport(query);
        }

        // ── Report 2: Pending Applications ────────────────────
        // FIX: MS Access requires all multi-table JOINs wrapped in
        //      parentheses — ((...) INNER JOIN ...) INNER JOIN ...
        private void GeneratePendingApplications()
        {
            lblReportTitle.Text = "Report: Pending Applications (Draft / Submitted)";

            string query =
                "SELECT " +
                "  a.FirstName & ' ' & a.LastName AS [Full Name], " +
                "  jv.JobTitle AS [Applied Position], " +
                "  jv.Department, " +
                "  app.Status AS [Application Status], " +
                "  app.DateApplied AS [Date Applied] " +
                "FROM ((Applications app " +
                "  INNER JOIN Applicants a ON app.ApplicantID = a.ApplicantID) " +
                "  INNER JOIN JobVacancies jv ON app.JobID = jv.JobID) " +
                "WHERE app.Status IN ('Draft', 'Submitted') " +
                "ORDER BY app.DateApplied DESC";

            LoadReport(query);
        }

        // ── Report 3: Scheduled Interviews ────────────────────
        private void GenerateScheduledInterviews()
        {
            lblReportTitle.Text = "Report: Scheduled Interviews";

            string query =
                "SELECT " +
                "  a.FirstName & ' ' & a.LastName AS [Applicant Name], " +
                "  jv.JobTitle AS [Position], " +
                "  isc.InterviewDate AS [Interview Date], " +
                "  isc.Interviewer, " +
                "  isc.Location, " +
                "  isc.Mode, " +
                "  isc.Status AS [Schedule Status] " +
                "FROM (((InterviewSchedules isc " +
                "  INNER JOIN Applications app ON isc.ApplicationID = app.ApplicationID) " +
                "  INNER JOIN Applicants a ON app.ApplicantID = a.ApplicantID) " +
                "  INNER JOIN JobVacancies jv ON app.JobID = jv.JobID) " +
                "ORDER BY isc.InterviewDate ASC";

            LoadReport(query);
        }

        // ── Report 4: Accepted / Rejected ─────────────────────
        private void GenerateHiredRejected()
        {
            lblReportTitle.Text = "Report: Accepted / Rejected Applicants";

            string query =
                "SELECT " +
                "  a.FirstName & ' ' & a.LastName AS [Applicant Name], " +
                "  jv.JobTitle AS [Position], " +
                "  app.Status AS [Final Status], " +
                "  hd.Decision, " +
                "  hd.Remarks AS [Decision Remarks], " +
                "  hd.DecisionDate AS [Decision Date] " +
                "FROM (((HiringDecisions hd " +
                "  INNER JOIN Applications app ON hd.ApplicationID = app.ApplicationID) " +
                "  INNER JOIN Applicants a ON app.ApplicantID = a.ApplicantID) " +
                "  INNER JOIN JobVacancies jv ON app.JobID = jv.JobID) " +
                "WHERE app.Status IN ('Accepted', 'Rejected') " +
                "ORDER BY hd.DecisionDate DESC";

            LoadReport(query);
        }

        // ── Report 5: Missing Requirements ────────────────────
        // FIX: Replaced NOT EXISTS + comma-join (incompatible with Access)
        //      with LEFT JOIN ... WHERE IS NULL pattern, which Access handles
        //      cleanly. All JOINs are parenthesised correctly.
        private void GenerateMissingRequirements()
        {
            lblReportTitle.Text = "Report: Applicants with Missing Requirements";

            // MS Access fix:
            // 1. RequirementTypes has no FK to the other tables, so it can't use
            //    INNER JOIN — we cross-join it with a comma in the FROM clause.
            // 2. The other three tables use properly parenthesised INNER JOINs.
            // 3. NOT IN subquery replaces LEFT JOIN IS NULL to avoid the
            //    multi-condition ON clause that Access rejects.
            string query =
                "SELECT " +
                "  a.FirstName & ' ' & a.LastName AS [Full Name], " +
                "  rt.RequirementName             AS [Missing Document], " +
                "  app.Status                     AS [Application Status], " +
                "  jv.JobTitle                    AS [Applied Position] " +
                "FROM RequirementTypes AS rt, " +
                "     ((Applicants AS a " +
                "       INNER JOIN Applications AS app ON a.ApplicantID = app.ApplicantID) " +
                "       INNER JOIN JobVacancies AS jv  ON app.JobID     = jv.JobID) " +
                "WHERE rt.IsRequired = TRUE " +
                "  AND app.Status NOT IN ('Draft', 'Withdrawn', 'Rejected') " +
                "  AND rt.RequirementTypeID NOT IN ( " +
                "        SELECT ad.RequirementTypeID " +
                "        FROM   ApplicantDocuments AS ad " +
                "        WHERE  ad.ApplicantID = a.ApplicantID " +
                "      ) " +
                "ORDER BY a.LastName, rt.RequirementName";

            LoadReport(query);
        }

        // ── Shared: run query → fill DataGridView ─────────────
        private void LoadReport(string query)
        {
            OleDbConnection conn = DBConnection.GetConnection();
            if (conn == null) return;

            try
            {
                conn.Open();
                OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgvReport.DataSource = dt;
                dgvReport.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                lblStatus.Text = $"Showing {dt.Rows.Count} record(s).";
                lblStatus.ForeColor = dt.Rows.Count > 0
                    ? System.Drawing.Color.DarkGreen
                    : System.Drawing.Color.Gray;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error generating report:\n\n" + ex.Message,
                    "Report Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        // ── Export to CSV ──────────────────────────────────────
        private void btnExportCSV_Click(object sender, EventArgs e)
        {
            if (dgvReport.Rows.Count == 0)
            {
                MessageBox.Show(
                    "No data to export. Please generate a report first.",
                    "No Data",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            string suggestedName = lblReportTitle.Text
                .Replace("Report: ", "")
                .Replace("/", "-")
                .Trim();

            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv",
                FileName = suggestedName
            };

            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                StringBuilder sb = new StringBuilder();

                // Header row
                for (int i = 0; i < dgvReport.Columns.Count; i++)
                {
                    sb.Append(QuoteCSV(dgvReport.Columns[i].HeaderText));
                    if (i < dgvReport.Columns.Count - 1) sb.Append(",");
                }
                sb.AppendLine();

                // Data rows
                foreach (DataGridViewRow row in dgvReport.Rows)
                {
                    if (row.IsNewRow) continue;
                    for (int i = 0; i < dgvReport.Columns.Count; i++)
                    {
                        sb.Append(QuoteCSV(row.Cells[i].Value?.ToString() ?? ""));
                        if (i < dgvReport.Columns.Count - 1) sb.Append(",");
                    }
                    sb.AppendLine();
                }

                File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                MessageBox.Show(
                    "Export successful!\n\n" + sfd.FileName,
                    "Export Complete",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Export failed:\n\n" + ex.Message,
                    "Export Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private string QuoteCSV(string value)
        {
            if (value.Contains(",") || value.Contains("\n") || value.Contains("\""))
                return "\"" + value.Replace("\"", "\"\"") + "\"";
            return value;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            HRDashboard dashboard = new HRDashboard();
            dashboard.Show();
            this.Close();
        }
    }
}
