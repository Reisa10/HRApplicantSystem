using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using HRApplicantSystem.Database;
using HRApplicantSystem.Classes;
using System.Drawing.Printing;

namespace HRApplicantSystem.Forms.HR
{
    // Ensure Class is named 'ReportsForm' to avoid name collisions with the dashboard button 'Reports'
    public partial class ReportsForm : Form
    {
        private Panel pnlSidebar;
        private Panel pnlTopBar;
        private Panel pnlMainContent;
        private DataGridView dgvReports;
        private Label lblTitle;
        private Label lblSubtitle;
        private ComboBox cmbDepartment;
        private TextBox txtSearch;
        private Button btnExport;
        private Button btnPrint;

        private Button btnApplicantList;
        private Button btnPending;
        private Button btnInterviews;
        private Button btnHiring;
        private Button btnMissingReqs;
        private Button btnBack; // Declared Back button variable

        private PrintDocument printDoc = new PrintDocument();
        private PrintPreviewDialog printPreview = new PrintPreviewDialog();

        private BindingSource bindingSource = new BindingSource();
        private string activeReportType = "ApplicantList";
        private bool isInitializing = true;

        // Ensure the constructor matches the class name 'ReportsForm'
        public ReportsForm()
        {
            InitializeCustomLayout();
            ConfigurePrintDocument();
        }

        private void Reports_Load(object sender, EventArgs e)
        {
            string userRole = UserSession.Role;
            if (userRole != "Admin" && userRole != "HR Manager" && userRole != "HR Staff")
            {
                MessageBox.Show("Access Denied. You do not have permission to view recruitment reports.",
                                "Security Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.BeginInvoke(new MethodInvoker(this.Close));
                return;
            }

            // Centering Fix: Manually align the form over the parent dashboard form
            CenterFormOnDashboard();

            LoadDepartmentsDropdown();
            isInitializing = false;

            SwitchReport("ApplicantList", btnApplicantList);
        }

        /// <summary>
        /// Scans active application forms and centers this window precisely over the main dashboard.
        /// Bypasses WinForms high-DPI scaling offsets.
        /// </summary>
        private void CenterFormOnDashboard()
        {
            Form dashboard = null;

            // Search for the dashboard or main menu form
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm != this && (openForm.Name.Contains("Dashboard") || openForm.Name.Contains("Main") || openForm.Name.Contains("Portal")))
                {
                    dashboard = openForm;
                    break;
                }
            }

            // Fallback to the first open form if no name matches
            if (dashboard == null)
            {
                foreach (Form openForm in Application.OpenForms)
                {
                    if (openForm != this)
                    {
                        dashboard = openForm;
                        break;
                    }
                }
            }

            if (dashboard != null)
            {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new Point(
                    dashboard.Location.X + (dashboard.Width - this.Width) / 2,
                    dashboard.Location.Y + (dashboard.Height - this.Height) / 2
                );
            }
            else
            {
                // Absolute fallback to physical monitor center if no other forms are running
                this.StartPosition = FormStartPosition.CenterScreen;
            }
        }

        #region Modern UI Layout Initialization
        private void InitializeCustomLayout()
        {
            // Main Form Properties
            this.Size = new Size(1100, 700);
            this.MinimumSize = new Size(950, 600);
            this.Text = "HR Recruitment Reports Dashboard";
            this.BackColor = Color.FromArgb(243, 244, 246); // Modern Light Gray Background

            // Subscribe to the Load event so Reports_Load runs when the form displays
            this.Load += new System.EventHandler(this.Reports_Load);

            // 1. Sidebar Panel
            pnlSidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 240,
                BackColor = Color.FromArgb(31, 41, 55) // Clean Dark Slate Blue
            };

            Label lblLogo = new Label
            {
                Text = "RECRUITMENT\nREPORTS",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                Location = new Point(20, 25),
                Size = new Size(200, 50),
                TextAlign = ContentAlignment.MiddleLeft
            };
            pnlSidebar.Controls.Add(lblLogo);

            // Initialize Sidebar Buttons
            btnApplicantList = CreateSidebarButton("📋  Applicant Register", 100);
            btnPending = CreateSidebarButton("⏳  Pending Applications", 155);
            btnInterviews = CreateSidebarButton("🗓️  Interview Schedules", 210);
            btnHiring = CreateSidebarButton("🤝  Hiring Outcomes", 265);
            btnMissingReqs = CreateSidebarButton("⚠️  Missing Documents", 320);

            // Back to Dashboard button docked elegantly to the bottom of the Sidebar
            btnBack = new Button
            {
                Text = "⬅  Back to Dashboard",
                Dock = DockStyle.Bottom,
                Height = 55,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FromArgb(156, 163, 175), // Muted Gray
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.FlatAppearance.MouseOverBackColor = Color.FromArgb(55, 65, 81);
            btnBack.Click += (s, e) => this.Close();
            pnlSidebar.Controls.Add(btnBack);

            btnApplicantList.Click += (s, e) => SwitchReport("ApplicantList", btnApplicantList);
            btnPending.Click += (s, e) => SwitchReport("Pending", btnPending);
            btnInterviews.Click += (s, e) => SwitchReport("Interviews", btnInterviews);
            btnHiring.Click += (s, e) => SwitchReport("Hiring", btnHiring);
            btnMissingReqs.Click += (s, e) => SwitchReport("MissingReqs", btnMissingReqs);

            // 2. Top Navigation & Filters Bar
            int topBarBaseWidth = 1100 - 240; // Form Width - Sidebar Width = 860

            pnlTopBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 85,
                Width = topBarBaseWidth,
                BackColor = Color.White,
                Padding = new Padding(20, 10, 20, 10)
            };

            lblTitle = new Label
            {
                Text = "Applicant Register",
                Font = new Font("Segoe UI", 15, FontStyle.Bold),
                ForeColor = Color.FromArgb(17, 24, 39),
                Location = new Point(20, 15),
                Size = new Size(250, 30)
            };
            lblSubtitle = new Label
            {
                Text = "Overview of registered job applicants",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(21, 45),
                Size = new Size(250, 20)
            };
            pnlTopBar.Controls.Add(lblTitle);
            pnlTopBar.Controls.Add(lblSubtitle);

            // Print Button (Anchored to Far Right)
            btnPrint = new Button
            {
                Text = "🖨️ Print",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(59, 130, 246), // Modern Blue
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(topBarBaseWidth - 80 - 20, 34),
                Size = new Size(80, 32),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Cursor = Cursors.Hand
            };
            btnPrint.FlatAppearance.BorderSize = 0;
            btnPrint.Click += btnPrint_Click;
            pnlTopBar.Controls.Add(btnPrint);

            // Export Button (Anchored to Right, left of Print)
            btnExport = new Button
            {
                Text = "💾 Export CSV",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(16, 185, 129), // Emerald Green
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(topBarBaseWidth - 80 - 20 - 110 - 10, 34),
                Size = new Size(110, 32),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Cursor = Cursors.Hand
            };
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.Click += btnExport_Click;
            pnlTopBar.Controls.Add(btnExport);

            // Dynamic Search Box (Anchored to Right)
            Label lblSearch = new Label
            {
                Text = "Search Filter:",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Location = new Point(topBarBaseWidth - 80 - 20 - 110 - 10 - 150 - 15, 18),
                Size = new Size(150, 15),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                ForeColor = Color.FromArgb(75, 85, 99)
            };
            txtSearch = new TextBox
            {
                Location = new Point(topBarBaseWidth - 80 - 20 - 110 - 10 - 150 - 15, 38),
                Size = new Size(150, 25),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Font = new Font("Segoe UI", 9)
            };
            txtSearch.TextChanged += txtSearch_TextChanged;
            pnlTopBar.Controls.Add(lblSearch);
            pnlTopBar.Controls.Add(txtSearch);

            // Department Dropdown Filter (Anchored to Right)
            Label lblDept = new Label
            {
                Text = "Department:",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Location = new Point(topBarBaseWidth - 80 - 20 - 110 - 10 - 150 - 15 - 150 - 15, 18),
                Size = new Size(150, 15),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                ForeColor = Color.FromArgb(75, 85, 99)
            };
            cmbDepartment = new ComboBox
            {
                Location = new Point(topBarBaseWidth - 80 - 20 - 110 - 10 - 150 - 15 - 150 - 15, 38),
                Size = new Size(150, 25),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9)
            };
            cmbDepartment.SelectedIndexChanged += cmbDepartment_SelectedIndexChanged;
            pnlTopBar.Controls.Add(lblDept);
            pnlTopBar.Controls.Add(cmbDepartment);

            // 3. Main Data Workspace
            pnlMainContent = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(25)
            };

            dgvReports = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(229, 231, 235), // Subtle border color
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                RowTemplate = { Height = 36 },
                EnableHeadersVisualStyles = false
            };

            // Modern Grid Header Style
            dgvReports.ColumnHeadersHeight = 40;
            dgvReports.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(37, 99, 235); // Accent Royal Blue
            dgvReports.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvReports.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgvReports.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            // Modern Grid Alternate Row Style
            dgvReports.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 251);
            dgvReports.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dgvReports.DefaultCellStyle.SelectionForeColor = Color.FromArgb(30, 58, 138);
            dgvReports.DefaultCellStyle.Font = new Font("Segoe UI", 9);

            pnlMainContent.Controls.Add(dgvReports);

            // Add all panels to the main form (reverse Z-order handles docking priority)
            this.Controls.Add(pnlMainContent);
            this.Controls.Add(pnlTopBar);
            this.Controls.Add(pnlSidebar);
        }

        private Button CreateSidebarButton(string text, int top)
        {
            Button btn = new Button
            {
                Text = text,
                Location = new Point(0, top),
                Size = new Size(240, 50),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FromArgb(156, 163, 175), // Muted Gray
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(55, 65, 81);
            pnlSidebar.Controls.Add(btn);
            return btn;
        }

        private void HighlightActiveButton(Button activeButton)
        {
            // Reset colors of all buttons
            foreach (Control ctrl in pnlSidebar.Controls)
            {
                if (ctrl is Button btn)
                {
                    // Do not reset back button highlight properties
                    if (btn == btnBack) continue;

                    btn.BackColor = Color.Transparent;
                    btn.ForeColor = Color.FromArgb(156, 163, 175);
                }
            }
            // Set active colors
            activeButton.BackColor = Color.FromArgb(17, 24, 39); // Deep dark accent
            activeButton.ForeColor = Color.FromArgb(59, 130, 246); // Bright Blue text
        }
        #endregion

        #region Report Data Logic & Access Queries
        private void LoadDepartmentsDropdown()
        {
            string query = "SELECT DepartmentID, DepartmentName FROM Departments WHERE IsActive = True";
            using (OleDbConnection con = DBConnection.GetConnection())
            {
                if (con == null) return;
                try
                {
                    con.Open();
                    using (OleDbDataAdapter da = new OleDbDataAdapter(query, con))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        DataRow row = dt.NewRow();
                        row["DepartmentID"] = 0;
                        row["DepartmentName"] = "All Departments";
                        dt.Rows.InsertAt(row, 0);

                        cmbDepartment.DataSource = dt;
                        cmbDepartment.DisplayMember = "DepartmentName";
                        cmbDepartment.ValueMember = "DepartmentID";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading department filter: " + ex.Message, "Database Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SwitchReport(string reportType, Button correspondingButton)
        {
            activeReportType = reportType;
            HighlightActiveButton(correspondingButton);
            txtSearch.Clear();

            switch (reportType)
            {
                case "ApplicantList":
                    lblTitle.Text = "Registered Applicant Register";
                    lblSubtitle.Text = "Comprehensive list of all registered applicants and their statuses";
                    break;
                case "Pending":
                    lblTitle.Text = "Pending & Review Applications";
                    lblSubtitle.Text = "Applications currently marked under Draft, Submitted, or Under Review";
                    break;
                case "Interviews":
                    lblTitle.Text = "Interview Schedules Report";
                    lblSubtitle.Text = "Active interview schedules, including dates, locations, and modes";
                    break;
                case "Hiring":
                    lblTitle.Text = "Final Hiring Outcomes Log";
                    lblSubtitle.Text = "Closed processes of candidates officially Accepted or Rejected";
                    break;
                case "MissingReqs":
                    lblTitle.Text = "Missing Compliance Documents Tracker";
                    lblSubtitle.Text = "Mandatory requirement gaps calculated against active profiles";
                    break;
            }

            LoadReportData();
        }

        private void LoadReportData()
        {
            string query = "";

            // Access-compatible SQL query design utilizing safe parentheses for multiple INNER/LEFT JOINs
            switch (activeReportType)
            {
                case "ApplicantList":
                    query = @"SELECT a.ApplicantID, a.FirstName, a.LastName, a.ContactNumber, 
                                     p.PositionName, et.TypeName AS EmploymentType, d.DepartmentName, ap.Status, ap.DateApplied
                             FROM (((((Applicants a
                             LEFT JOIN Applications ap ON a.ApplicantID = ap.ApplicantID)
                             LEFT JOIN JobVacancies jv ON ap.JobID = jv.JobID)
                             LEFT JOIN Positions p ON jv.PositionID = p.PositionID)
                             LEFT JOIN Departments d ON jv.DepartmentID = d.DepartmentID)
                             LEFT JOIN EmploymentTypes et ON jv.EmploymentTypeID = et.EmploymentTypeID)";
                    break;

                case "Pending":
                    query = @"SELECT ap.ApplicationID, a.FirstName, a.LastName, p.PositionName, et.TypeName AS EmploymentType, 
                                     d.DepartmentName, ap.Status, ap.DateApplied
                             FROM (((((Applications ap
                             INNER JOIN Applicants a ON ap.ApplicantID = a.ApplicantID)
                             INNER JOIN JobVacancies jv ON ap.JobID = jv.JobID)
                             INNER JOIN Positions p ON jv.PositionID = p.PositionID)
                             INNER JOIN Departments d ON jv.DepartmentID = d.DepartmentID)
                             LEFT JOIN EmploymentTypes et ON jv.EmploymentTypeID = et.EmploymentTypeID)
                             WHERE ap.Status IN ('Draft', 'Submitted', 'Under Review')";
                    break;

                case "Interviews":
                    query = @"SELECT i.InterviewScheduleID, a.FirstName, a.LastName, p.PositionName, et.TypeName AS EmploymentType, 
                                     d.DepartmentName, i.InterviewDate, i.Interviewer, i.Mode, i.Location, i.Status
                             FROM ((((((InterviewSchedules i
                             INNER JOIN Applications ap ON i.ApplicationID = ap.ApplicationID)
                             INNER JOIN Applicants a ON ap.ApplicantID = a.ApplicantID)
                             INNER JOIN JobVacancies jv ON ap.JobID = jv.JobID)
                             INNER JOIN Positions p ON jv.PositionID = p.PositionID)
                             INNER JOIN Departments d ON jv.DepartmentID = d.DepartmentID)
                             LEFT JOIN EmploymentTypes et ON jv.EmploymentTypeID = et.EmploymentTypeID)";
                    break;

                case "Hiring":
                    query = @"SELECT ap.ApplicationID, a.FirstName, a.LastName, p.PositionName, et.TypeName AS EmploymentType, 
                                     d.DepartmentName, ap.Status, ap.DateApplied
                             FROM (((((Applications ap
                             INNER JOIN Applicants a ON ap.ApplicantID = a.ApplicantID)
                             INNER JOIN JobVacancies jv ON ap.JobID = jv.JobID)
                             INNER JOIN Positions p ON jv.PositionID = p.PositionID)
                             INNER JOIN Departments d ON jv.DepartmentID = d.DepartmentID)
                             LEFT JOIN EmploymentTypes et ON jv.EmploymentTypeID = et.EmploymentTypeID)
                             WHERE ap.Status IN ('Accepted', 'Rejected')";
                    break;

                case "MissingReqs":
                    // Generates missing documents list only for applicants with active, non-finalized applications
                    query = @"SELECT a.ApplicantID, a.FirstName, a.LastName, r.RequirementName, 'Missing' AS DocumentStatus
                             FROM Applicants a, RequirementTypes r
                             WHERE r.IsRequired = True
                               AND a.ApplicantID IN (
                                   SELECT ap.ApplicantID 
                                   FROM Applications ap 
                                   WHERE ap.Status NOT IN ('Accepted', 'Rejected', 'Withdrawn')
                               )
                               AND NOT EXISTS (
                                   SELECT 1 
                                   FROM ApplicantDocuments ad 
                                   WHERE ad.ApplicantID = a.ApplicantID 
                                     AND ad.RequirementTypeID = r.RequirementTypeID 
                                     AND ad.Status = 'Submitted'
                               )";
                    break;
            }

            // Append Department Filter dynamically
            if (cmbDepartment.SelectedValue != null && Convert.ToInt32(cmbDepartment.SelectedValue) > 0)
            {
                int deptId = Convert.ToInt32(cmbDepartment.SelectedValue);
                if (activeReportType == "MissingReqs")
                {
                    // For missing documents, check the department of the applicant's current active applications
                    query += $@" AND a.ApplicantID IN (
                                    SELECT ap.ApplicantID 
                                    FROM Applications ap 
                                    INNER JOIN JobVacancies jv ON ap.JobID = jv.JobID 
                                    WHERE jv.DepartmentID = {deptId}
                                )";
                }
                else
                {
                    query += query.Contains("WHERE") ? $" AND d.DepartmentID = {deptId}" : $" WHERE d.DepartmentID = {deptId}";
                }
            }

            using (OleDbConnection con = DBConnection.GetConnection())
            {
                if (con == null) return;
                try
                {
                    con.Open();
                    using (OleDbDataAdapter da = new OleDbDataAdapter(query, con))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        bindingSource.DataSource = dt;
                        dgvReports.DataSource = bindingSource;

                        // Modernize Column Headers Display on UI
                        AutoFormatGridColumns();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Query Execution Failure: " + ex.Message, "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void AutoFormatGridColumns()
        {
            if (dgvReports.Columns.Count == 0) return;

            foreach (DataGridViewColumn col in dgvReports.Columns)
            {
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                // Better readable names instead of database column keys
                switch (col.Name)
                {
                    case "ApplicantID": col.HeaderText = "ID"; col.FillWeight = 40; break;
                    case "FirstName": col.HeaderText = "First Name"; break;
                    case "LastName": col.HeaderText = "Last Name"; break;
                    case "ContactNumber": col.HeaderText = "Phone No."; break;
                    case "PositionName": col.HeaderText = "Applied Position"; break;
                    case "EmploymentType": col.HeaderText = "Employment Type"; break;
                    case "DepartmentName": col.HeaderText = "Department"; break;
                    case "Status": col.HeaderText = "Workflow Status"; break;
                    case "DateApplied": col.HeaderText = "Application Date"; break;
                    case "InterviewDate": col.HeaderText = "Schedule Date"; break;
                    case "Interviewer": col.HeaderText = "Interviewer(s)"; break;
                    case "Mode": col.HeaderText = "Interview Mode"; break;
                    case "Location": col.HeaderText = "Venue/Link"; break;
                    case "RequirementName": col.HeaderText = "Mandatory Requirement"; break;
                    case "DocumentStatus": col.HeaderText = "Document Status"; break;
                }
            }
        }

        private void cmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isInitializing) return;
            LoadReportData();
        }

        // Live In-Memory Client-side Data Filtering matching MaintenanceForm.cs patterns
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (dgvReports.DataSource == null) return;

            string searchText = txtSearch.Text.Trim().Replace("'", "''");
            if (string.IsNullOrEmpty(searchText))
            {
                bindingSource.Filter = "";
                return;
            }

            // Build dynamic search criteria based on loaded active columns
            string filterExpression = "";
            if (activeReportType == "MissingReqs")
            {
                filterExpression = $"FirstName LIKE '%{searchText}%' OR LastName LIKE '%{searchText}%' OR RequirementName LIKE '%{searchText}%'";
            }
            else if (activeReportType == "Interviews")
            {
                filterExpression = $"FirstName LIKE '%{searchText}%' OR LastName LIKE '%{searchText}%' OR PositionName LIKE '%{searchText}%' OR Interviewer LIKE '%{searchText}%' OR EmploymentType LIKE '%{searchText}%'";
            }
            else
            {
                filterExpression = $"FirstName LIKE '%{searchText}%' OR LastName LIKE '%{searchText}%' OR PositionName LIKE '%{searchText}%' OR EmploymentType LIKE '%{searchText}%'";
            }

            try
            {
                bindingSource.Filter = filterExpression;
            }
            catch
            {
                bindingSource.Filter = "";
            }
        }
        #endregion

        #region Export and Printing Engine
        private void btnExport_Click(object sender, EventArgs e)
        {
            if (dgvReports.Rows.Count == 0)
            {
                MessageBox.Show("No data records are present to export.", "Empty Report Set", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "CSV UTF-8 Document (*.csv)|*.csv",
                FileName = $"HR_Report_{activeReportType}_{DateTime.Now:yyyyMMdd}"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(new FileStream(sfd.FileName, FileMode.Create), System.Text.Encoding.UTF8))
                    {
                        // Write CSV Headers
                        string[] headers = new string[dgvReports.Columns.Count];
                        for (int i = 0; i < dgvReports.Columns.Count; i++)
                        {
                            headers[i] = $"\"{dgvReports.Columns[i].HeaderText}\"";
                        }
                        sw.WriteLine(string.Join(",", headers));

                        // Write CSV Data Rows safely handling delimiters
                        foreach (DataGridViewRow row in dgvReports.Rows)
                        {
                            if (row.IsNewRow) continue;
                            string[] fields = new string[dgvReports.Columns.Count];
                            for (int i = 0; i < dgvReports.Columns.Count; i++)
                                sw.WriteLine(string.Join(",", fields));
                        }
                    }

                    MessageBox.Show("The report spreadsheet has been exported successfully.",
                                    "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Writing process interrupted: " + ex.Message, "IO Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ConfigurePrintDocument()
        {
            printDoc.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);
            printPreview.Document = printDoc;
            printPreview.Width = 800;
            printPreview.Height = 600;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dgvReports.Rows.Count == 0)
            {
                MessageBox.Show("There is no records data to send to the printer.", "Empty Record Set", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            printPreview.ShowDialog();
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Title Header Drawing
            string reportName = lblTitle.Text;
            string dateStr = "Generated on: " + DateTime.Now.ToString("g");

            // FIXED: Instantiated SolidBrushes with using statements to prevent CS0117 errors and free GDI memory safely
            using (Brush titleBrush = new SolidBrush(Color.FromArgb(17, 24, 39)))
            using (Brush subtitleBrush = new SolidBrush(Color.FromArgb(59, 130, 246)))
            {
                e.Graphics.DrawString("HR APPLICANT PROCESSING SYSTEM", new Font("Segoe UI", 16, FontStyle.Bold), titleBrush, 50, 40);
                e.Graphics.DrawString(reportName.ToUpper(), new Font("Segoe UI", 11, FontStyle.Bold), subtitleBrush, 50, 70);
            }

            e.Graphics.DrawString(dateStr, new Font("Segoe UI", 8.5F, FontStyle.Italic), Brushes.Gray, 50, 90);
            e.Graphics.DrawLine(new Pen(Color.FromArgb(229, 231, 235), 2), 50, 110, e.PageBounds.Width - 50, 110);

            // Table Header Drawing
            int currentY = 130;
            int startX = 50;
            int colWidth = (e.PageBounds.Width - 100) / Math.Max(1, dgvReports.Columns.Count);

            // Draw headers
            for (int i = 0; i < dgvReports.Columns.Count; i++)
            {
                e.Graphics.DrawString(dgvReports.Columns[i].HeaderText, new Font("Segoe UI", 8.5F, FontStyle.Bold), Brushes.Black, startX + (i * colWidth), currentY);
            }

            currentY += 25;
            e.Graphics.DrawLine(Pens.Black, 50, currentY, e.PageBounds.Width - 50, currentY);
            currentY += 10;

            // Draw Data Rows
            Font rowFont = new Font("Segoe UI", 8.5F, FontStyle.Regular);
            foreach (DataGridViewRow row in dgvReports.Rows)
            {
                if (row.IsNewRow) continue;
                if (currentY + 25 > e.PageBounds.Height - 60) // Simple Page-Break check
                {
                    e.HasMorePages = true;
                    return;
                }

                for (int i = 0; i < dgvReports.Columns.Count; i++)
                {
                    string textValue = row.Cells[i].Value?.ToString() ?? "";
                    if (textValue.Length > 20) textValue = textValue.Substring(0, 17) + "..."; // Ellipsis truncation to fit page columns

                    e.Graphics.DrawString(textValue, rowFont, Brushes.DarkSlateGray, startX + (i * colWidth), currentY);
                }
                currentY += 22;
            }

            e.HasMorePages = false;
        }
        #endregion
    }
}