using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.OleDb;
using HRApplicantSystem.Database;
using HRApplicantSystem.Classes;
using System.Collections.Generic;

namespace HRApplicantSystem.Forms.HR
{
    public partial class ApplicantReviewForm : Form
    {
        private int selectedApplicantID = 0;
        private int selectedApplicationID = 0;
        private string selectedApplicationStatus = "";
        private DataTable applicationsTable;

        // Programmatically created controls for the search bar
        private TextBox txtSearch;
        private Label lblSearch;

        public ApplicantReviewForm()
        {
            InitializeComponent();
        }

        private void ApplicantReviewForm_Load(object sender, EventArgs e)
        {
            InitializeSearchField();
            ConfigureGridViewStyle();
            LoadApplications();
        }

        /// <summary>
        /// Programmatically creates and places a search bar on the form to avoid breaking the designer file.
        /// </summary>
        private void InitializeSearchField()
        {
            lblSearch = new Label();
            lblSearch.Text = "Search:";
            lblSearch.Location = new Point(69, 44);
            lblSearch.AutoSize = true;

            txtSearch = new TextBox();
            txtSearch.Location = new Point(125, 41);
            txtSearch.Width = 250;
            txtSearch.TextChanged += TxtSearch_TextChanged;

            this.Controls.Add(lblSearch);
            this.Controls.Add(txtSearch);

            // Shifting the grid down slightly to make room for the new search bar
            dgvApplications.Location = new Point(69, 70);
            dgvApplications.Height = 145;
        }

        /// <summary>
        /// Instantly filters the loaded grid data on the client side across multiple columns.
        /// </summary>
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            if (applicationsTable != null)
            {
                string filterText = txtSearch.Text.Replace("'", "''"); // Safe escape for quotes

                // Dynamic filter on applicant names, job title, and status
                applicationsTable.DefaultView.RowFilter = string.Format(
                    "FirstName LIKE '%{0}%' OR LastName LIKE '%{0}%' OR JobTitle LIKE '%{0}%' OR Status LIKE '%{0}%'",
                    filterText);

                ApplyRowColoring();
            }
        }

        /// <summary>
        /// Sets professional formatting options for the grid.
        /// </summary>
        private void ConfigureGridViewStyle()
        {
            dgvApplications.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvApplications.MultiSelect = false;
            dgvApplications.RowHeadersVisible = false;
            dgvApplications.AllowUserToAddRows = false;
            dgvApplications.ReadOnly = true;
            dgvApplications.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvApplications.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 250);
        }

        /// <summary>
        /// Loads all applications from the database for manual selection and testing.
        /// </summary>
        private void LoadApplications()
        {
            try
            {
                using (OleDbConnection con = DBConnection.GetConnection())
                {
                    if (con == null) return;
                    con.Open();

                    string query = @"
                        SELECT
                            Applications.ApplicationID,
                            Applicants.ApplicantID,
                            Applicants.FirstName,
                            Applicants.LastName,
                            JobVacancies.JobTitle,
                            Applications.Status
                        FROM
                            (Applications
                            INNER JOIN Applicants ON Applications.ApplicantID = Applicants.ApplicantID)
                            INNER JOIN JobVacancies ON Applications.JobID = JobVacancies.JobID";

                    using (OleDbDataAdapter da = new OleDbDataAdapter(query, con))
                    {
                        applicationsTable = new DataTable();
                        da.Fill(applicationsTable);
                        dgvApplications.DataSource = applicationsTable;

                        if (dgvApplications.Columns.Contains("ApplicantID"))
                            dgvApplications.Columns["ApplicantID"].Visible = false;

                        ApplyRowColoring();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading applications: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Dynamically highlights rows based on status (Amber for Under Review/Locked, Blue for Submitted).
        /// </summary>
        private void ApplyRowColoring()
        {
            foreach (DataGridViewRow row in dgvApplications.Rows)
            {
                if (row.Cells["Status"].Value != null)
                {
                    string status = row.Cells["Status"].Value.ToString();
                    if (status == "Under Review" || status == "Locked")
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(254, 243, 199);
                        row.DefaultCellStyle.ForeColor = Color.FromArgb(146, 64, 14);
                    }
                    else if (status == "Submitted")
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(219, 234, 254);
                        row.DefaultCellStyle.ForeColor = Color.FromArgb(30, 58, 138);
                    }
                }
            }
        }

        /// <summary>
        /// Loads the profile information of the selected applicant.
        /// </summary>
        private void LoadProfile()
        {
            if (selectedApplicantID == 0) return;

            try
            {
                using (OleDbConnection con = DBConnection.GetConnection())
                {
                    if (con == null) return;
                    con.Open();

                    string query = "SELECT * FROM Applicants WHERE ApplicantID = ?";

                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.Add("@ApplicantID", OleDbType.Integer).Value = selectedApplicantID;

                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtReviewFirstName.Text = reader["FirstName"].ToString();
                                txtReviewLastName.Text = reader["LastName"].ToString();
                                txtReviewContact.Text = reader["ContactNumber"] != DBNull.Value ? reader["ContactNumber"].ToString() : "";
                                txtReviewEducation.Text = reader["Education"] != DBNull.Value ? reader["Education"].ToString() : "";
                                txtReviewSkills.Text = reader["Skills"] != DBNull.Value ? reader["Skills"].ToString() : "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading profile: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Loads the documents list associated with the selected applicant.
        /// </summary>
        private void LoadDocuments()
        {
            lstApplicantDocuments.Items.Clear();
            if (selectedApplicantID == 0) return;

            try
            {
                using (OleDbConnection con = DBConnection.GetConnection())
                {
                    if (con == null) return;
                    con.Open();

                    string query = "SELECT DocumentName FROM ApplicantDocuments WHERE ApplicantID = ?";

                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.Add("@ApplicantID", OleDbType.Integer).Value = selectedApplicantID;

                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lstApplicantDocuments.Items.Add(reader["DocumentName"].ToString());
                            }
                        }
                    }
                }

                if (lstApplicantDocuments.Items.Count == 0)
                {
                    lstApplicantDocuments.Items.Add("No documents submitted.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading documents: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Locks the application to 'Under Review', logs to history, and records audit trails within one transaction.
        /// </summary>
        private void LockApplication()
        {
            OleDbConnection con = DBConnection.GetConnection();
            if (con == null) return;

            OleDbTransaction transaction = null;

            try
            {
                con.Open();
                transaction = con.BeginTransaction();

                // 1. Update Application status to 'Under Review' (Safely escaped tables and columns)
                string updateQuery = "UPDATE [Applications] SET [Status] = 'Under Review' WHERE [ApplicationID] = ?";
                using (OleDbCommand cmd = new OleDbCommand(updateQuery, con, transaction))
                {
                    cmd.Parameters.Add("@ApplicationID", OleDbType.Integer).Value = selectedApplicationID;
                    cmd.ExecuteNonQuery();
                }

                // 2. Insert into ApplicationStatusHistory (Escaped Status column as it can be reserved)
                string historyQuery = @"
                    INSERT INTO [ApplicationStatusHistory] ([ApplicationID], [Status], [DateChanged]) 
                    VALUES (?, ?, ?)";
                using (OleDbCommand cmd = new OleDbCommand(historyQuery, con, transaction))
                {
                    cmd.Parameters.Add("@ApplicationID", OleDbType.Integer).Value = selectedApplicationID;
                    cmd.Parameters.Add("@Status", OleDbType.VarWChar).Value = "Under Review";
                    cmd.Parameters.Add("@DateChanged", OleDbType.Date).Value = DateTime.Now;
                    cmd.ExecuteNonQuery();
                }

                // 3. Insert into AuditTrail (Escaped reserved Action keyword)
                string auditQuery = @"
                    INSERT INTO [AuditTrail] ([Action], [DateCreated], [UserID]) 
                    VALUES (?, ?, ?)";
                using (OleDbCommand cmd = new OleDbCommand(auditQuery, con, transaction))
                {
                    cmd.Parameters.Add("@Action", OleDbType.VarWChar).Value = $"Locked Application ID {selectedApplicationID} for review.";
                    cmd.Parameters.Add("@DateCreated", OleDbType.Date).Value = DateTime.Now;

                    // Fallback to ID 1 if tested locally outside of standard user login sessions
                    int activeUserId = UserSession.UserID > 0 ? UserSession.UserID : 1;
                    cmd.Parameters.Add("@UserID", OleDbType.Integer).Value = activeUserId;

                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();

                MessageBox.Show("Application locked under review successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadApplications();
                ResetSelection();
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    try { transaction.Rollback(); } catch { }
                }

                // Maintain diagnostics feedback system in case of future structural additions
                string dbDiagnostics = "";
                try
                {
                    using (OleDbConnection diagCon = DBConnection.GetConnection())
                    {
                        diagCon.Open();
                        dbDiagnostics = "\n\n=== Database Diagnostics ===\n" +
                                        "ApplicationStatusHistory columns: \n" + GetTableColumns(diagCon, "ApplicationStatusHistory") + "\n\n" +
                                        "AuditTrail columns: \n" + GetTableColumns(diagCon, "AuditTrail");
                    }
                }
                catch
                {
                    dbDiagnostics = "\n\n(Could not load table diagnostic info)";
                }

                MessageBox.Show("Failed to process transaction: " + ex.Message + dbDiagnostics,
                                "Transaction Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        /// <summary>
        /// Reads physical column headers from the database table.
        /// </summary>
        private string GetTableColumns(OleDbConnection con, string tableName)
        {
            try
            {
                DataTable schemaTable = con.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null, tableName, null });
                List<string> columns = new List<string>();
                foreach (DataRow row in schemaTable.Rows)
                {
                    columns.Add(row["COLUMN_NAME"].ToString());
                }
                return "[" + string.Join(", ", columns) + "]";
            }
            catch (Exception ex)
            {
                return "Error reading table " + tableName + ": " + ex.Message;
            }
        }

        private void ResetSelection()
        {
            selectedApplicationID = 0;
            selectedApplicantID = 0;
            selectedApplicationStatus = "";
            btnLockApplication.Enabled = false;

            txtReviewFirstName.Clear();
            txtReviewLastName.Clear();
            txtReviewContact.Clear();
            txtReviewEducation.Clear();
            txtReviewSkills.Clear();
            lstApplicantDocuments.Items.Clear();
        }

        // --- Event Handlers & Designer Fallback Methods ---

        private void dgvApplications_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvApplications.Rows[e.RowIndex];

                selectedApplicationID = Convert.ToInt32(row.Cells["ApplicationID"].Value);
                selectedApplicantID = Convert.ToInt32(row.Cells["ApplicantID"].Value);
                selectedApplicationStatus = row.Cells["Status"].Value != DBNull.Value ? row.Cells["Status"].Value.ToString() : "";

                // Enable locking for any status that is NOT already reviewed
                btnLockApplication.Enabled = (selectedApplicationStatus != "Under Review" && selectedApplicationStatus != "Locked");

                // Auto-loading updates profile and documents list dynamically on selection
                LoadProfile();
                LoadDocuments();
            }
        }

        private void btnLockApplication_Click(object sender, EventArgs e)
        {
            if (selectedApplicationID == 0)
            {
                MessageBox.Show("Please select an application first.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Lock this application for review? The applicant will no longer be allowed to modify submitted details.",
                "Confirm Start Review",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                LockApplication();
            }
        }

        // Silent preservation of original designer events to prevent errors
        private void dgvApplications_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void groupBox1_Enter(object sender, EventArgs e) { }
        private void lblTitle_Click(object sender, EventArgs e) { }
        private void groupBox2_Enter(object sender, EventArgs e) { }
    }
}