using HRApplicantSystem.Classes;
using HRApplicantSystem.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace HRApplicantSystem.Forms.HR
{
    public partial class InterviewScheduleForm : Form
    {
        private int selectedApplicationID = 0;
        private DataTable dtApplications = new DataTable();

        // State tracking for the queue segments
        private bool isViewingActiveSchedules = false;

        // Modern UI Components
        private Panel pnlHeader;
        private Label lblHeaderTitle;
        private Label lblHeaderSubtitle;
        private Panel pnlWorkspace;
        private Panel pnlLeftCol;
        private Label lblGridTitle;
        private Panel pnlRightCol;
        private Label lblCardTitle;
        private Label lblDivider;
        private Panel pnlCandidateHighlight;
        private Label lblCandidateName;
        private Label lblCandidateJob;
        private Label lblDate;
        private Label lblTime;
        private DateTimePicker dtpInterviewTime; // Dynamic Time Picker companion
        private Label lblInterviewer;
        private Label lblInterviewType;
        private ComboBox cboInterviewType;
        private Label lblStatus; // Label for the designer's cboStatus dropdown
        private Label lblLocation;
        private CheckBox chkAssessmentRequired;
        private Label lblAssessmentType;
        private ComboBox cboAssessmentType;

        // Custom segment control buttons for switching lists
        private Button btnPendingQueue;
        private Button btnActiveQueue;

        // Corporate SaaS Color Palette
        private readonly Color ThemeDarkSlate = Color.FromArgb(30, 41, 59);    // #1E293B
        private readonly Color ThemePrimary = Color.FromArgb(44, 62, 80);      // #2C3E50
        private readonly Color ThemeBackground = Color.FromArgb(248, 250, 252); // #F8FAFC
        private readonly Color ThemeCardBg = Color.White;
        private readonly Color ColorSuccess = Color.FromArgb(39, 174, 96);     // Emerald Green
        private readonly Color ColorDanger = Color.FromArgb(192, 57, 43);      // Crimson Red
        private readonly Color ColorTextMuted = Color.FromArgb(148, 163, 184); // #94A3B8
        private readonly Color ColorBorder = Color.FromArgb(226, 232, 240);    // #E2E8F0

        public InterviewScheduleForm()
        {
            InitializeComponent();

            // Run layout transformation
            InitializeModernLayout();
        }

        private void InterviewScheduleForm_Load(object sender, EventArgs e)
        {
            // Centering Fix: Snap form coordinates precisely over the main dashboard
            CenterFormOnDashboard();

            LoadApplications();
            LoadMaintenanceData();
            ResetFields(); // Ensures the default state is set to 'Scheduled' on initial load
        }

        /// <summary>
        /// Locates the open HR Dashboard application window and centers this workspace directly over it.
        /// Resolves high-DPI scaling offsets at runtime.
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

            // Fallback to the first open form if no exact name matches
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
                // Fallback to primary screen center if no caller dashboard is detected
                this.StartPosition = FormStartPosition.CenterScreen;
            }
        }

        /// <summary>
        /// Deletes old groupboxes and dynamically injects the modern corporate layout with segment buttons.
        /// </summary>
        private void InitializeModernLayout()
        {
            this.Size = new Size(1024, 640);
            this.Text = "Interview Allocations — HR Applicant System";
            this.BackColor = ThemeBackground;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Step 1: Detach candidate grid and standard input controls from old designer parents
            if (dgvApplicants != null) dgvApplicants.Parent?.Controls.Remove(dgvApplicants);
            if (dtpInterviewDate != null) dtpInterviewDate.Parent?.Controls.Remove(dtpInterviewDate);
            if (txtInterviewer != null) txtInterviewer.Parent?.Controls.Remove(txtInterviewer);
            if (txtLocation != null) txtLocation.Parent?.Controls.Remove(txtLocation);
            if (cboStatus != null) cboStatus.Parent?.Controls.Remove(cboStatus);
            if (btnSave != null) btnSave.Parent?.Controls.Remove(btnSave);
            if (btnBack != null) btnBack.Parent?.Controls.Remove(btnBack);

            // Step 2: Delete old designer GroupBoxes to prevent layout overlapping artifacts
            if (grpApplicants != null)
            {
                this.Controls.Remove(grpApplicants);
                grpApplicants.Dispose();
            }
            if (grpSchedule != null)
            {
                this.Controls.Remove(grpSchedule);
                grpSchedule.Dispose();
            }

            // Hide old Mode controls safely
            if (cboMode != null) cboMode.Visible = false;

            // Step 3: Sleek Header Banner
            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 65,
                BackColor = ThemeDarkSlate
            };

            lblHeaderTitle = new Label
            {
                Text = "INTERVIEW SCHEDULING UNIT",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 12),
                AutoSize = true
            };

            lblHeaderSubtitle = new Label
            {
                Text = "Shortlisted candidate interview assignments and configuration-linked routing",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Regular),
                ForeColor = ColorTextMuted,
                Location = new Point(20, 35),
                AutoSize = true
            };

            pnlHeader.Controls.Add(lblHeaderTitle);
            pnlHeader.Controls.Add(lblHeaderSubtitle);
            this.Controls.Add(pnlHeader);

            // Step 4: Central Workspace Panel
            pnlWorkspace = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                BackColor = ThemeBackground
            };
            this.Controls.Add(pnlWorkspace);
            pnlWorkspace.BringToFront();

            // Step 5: Left Column Candidate Queue Panel
            pnlLeftCol = new Panel
            {
                Width = 460,
                Dock = DockStyle.Left,
                BackColor = ThemeCardBg,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(12)
            };
            pnlWorkspace.Controls.Add(pnlLeftCol);

            lblGridTitle = new Label
            {
                Text = "CANDIDATE DISPATCH QUEUE",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = ThemePrimary,
                Location = new Point(12, 12),
                AutoSize = true
            };
            pnlLeftCol.Controls.Add(lblGridTitle);

            // SEGMENT QUEUE BUTTONS
            btnPendingQueue = new Button
            {
                Text = "Pending Schedule",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Location = new Point(12, 40),
                Size = new Size(215, 32),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnPendingQueue.FlatAppearance.BorderSize = 0;
            btnPendingQueue.Click += btnPendingQueue_Click;
            pnlLeftCol.Controls.Add(btnPendingQueue);

            btnActiveQueue = new Button
            {
                Text = "Active Schedules",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Location = new Point(231, 40),
                Size = new Size(215, 32),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnActiveQueue.FlatAppearance.BorderSize = 0;
            btnActiveQueue.Click += btnActiveQueue_Click;
            pnlLeftCol.Controls.Add(btnActiveQueue);

            // Style and attach DataGridView dgvApplicants
            if (dgvApplicants != null)
            {
                dgvApplicants.Dock = DockStyle.None;
                dgvApplicants.BorderStyle = BorderStyle.None;
                dgvApplicants.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
                dgvApplicants.EnableHeadersVisualStyles = false;
                dgvApplicants.ColumnHeadersDefaultCellStyle.BackColor = ThemePrimary;
                dgvApplicants.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvApplicants.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                dgvApplicants.GridColor = ColorBorder;
                dgvApplicants.Location = new Point(12, 80);
                dgvApplicants.Size = new Size(434, 420);

                pnlLeftCol.Controls.Add(dgvApplicants);
            }

            // Step 6: Right Column Parameter Card Panel
            pnlRightCol = new Panel
            {
                Width = 500,
                Dock = DockStyle.Right,
                BackColor = ThemeCardBg,
                BorderStyle = BorderStyle.FixedSingle
            };
            pnlWorkspace.Controls.Add(pnlRightCol);

            lblCardTitle = new Label
            {
                Text = "INTERVIEW CONFIGURATION PARAMETERS",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = ThemePrimary,
                Location = new Point(20, 15),
                AutoSize = true
            };
            pnlRightCol.Controls.Add(lblCardTitle);

            lblDivider = new Label
            {
                BorderStyle = BorderStyle.Fixed3D,
                Location = new Point(20, 40),
                Size = new Size(460, 2)
            };
            pnlRightCol.Controls.Add(lblDivider);

            // Highlights Sub-Panel
            pnlCandidateHighlight = new Panel
            {
                Location = new Point(20, 50),
                Size = new Size(460, 52),
                BackColor = Color.FromArgb(241, 245, 249),
                BorderStyle = BorderStyle.None
            };
            pnlRightCol.Controls.Add(pnlCandidateHighlight);

            lblCandidateName = new Label
            {
                Text = "Candidate: Please select from the queue",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = ThemePrimary,
                Location = new Point(10, 8),
                AutoSize = true
            };
            lblApplicant = lblCandidateName;
            pnlCandidateHighlight.Controls.Add(lblCandidateName);

            lblCandidateJob = new Label
            {
                Text = "Job Position: —",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Regular),
                ForeColor = Color.FromArgb(71, 85, 105),
                Location = new Point(10, 28),
                AutoSize = true
            };
            lblJob = lblCandidateJob;
            pnlCandidateHighlight.Controls.Add(lblCandidateJob);

            // Position Date and Time controls
            lblDate = new Label { Text = "Interview Date:", Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), Location = new Point(20, 115), AutoSize = true };
            pnlRightCol.Controls.Add(lblDate);
            if (dtpInterviewDate != null)
            {
                dtpInterviewDate.Format = DateTimePickerFormat.Short;
                dtpInterviewDate.Location = new Point(20, 133);
                dtpInterviewDate.Width = 220;
                pnlRightCol.Controls.Add(dtpInterviewDate);
            }

            lblTime = new Label { Text = "Interview Time:", Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), Location = new Point(260, 115), AutoSize = true };
            pnlRightCol.Controls.Add(lblTime);

            // Explicitly initialize, style, and prioritize the time-picker control's Z-order rendering
            dtpInterviewTime = new DateTimePicker();
            dtpInterviewTime.Format = DateTimePickerFormat.Time;
            dtpInterviewTime.ShowUpDown = true;
            dtpInterviewTime.Location = new Point(260, 133);
            dtpInterviewTime.Width = 220;
            dtpInterviewTime.Font = new Font("Segoe UI", 9F);

            pnlRightCol.Controls.Add(dtpInterviewTime);
            dtpInterviewTime.BringToFront();

            // Assigned Interviewer
            lblInterviewer = new Label { Text = "Assigned Interviewer:", Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), Location = new Point(20, 170), AutoSize = true };
            pnlRightCol.Controls.Add(lblInterviewer);
            if (txtInterviewer != null)
            {
                txtInterviewer.BorderStyle = BorderStyle.FixedSingle;
                txtInterviewer.Location = new Point(20, 188);
                txtInterviewer.Width = 460;
                txtInterviewer.Font = new Font("Segoe UI", 9F);
                pnlRightCol.Controls.Add(txtInterviewer);
            }

            // Interview Type - aligned on left side
            lblInterviewType = new Label { Text = "Interview Type:", Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), Location = new Point(20, 225), AutoSize = true };
            cboInterviewType = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(20, 243), Width = 220, Font = new Font("Segoe UI", 9F) };
            pnlRightCol.Controls.Add(lblInterviewType);
            pnlRightCol.Controls.Add(cboInterviewType);

            // Schedule Status - positioned on the right side next to Interview Type
            lblStatus = new Label { Text = "Schedule Status:", Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), Location = new Point(260, 225), AutoSize = true };
            pnlRightCol.Controls.Add(lblStatus);
            if (cboStatus != null)
            {
                cboStatus.Visible = true;
                cboStatus.DropDownStyle = ComboBoxStyle.DropDownList;
                cboStatus.Location = new Point(260, 243);
                cboStatus.Width = 220;
                cboStatus.Font = new Font("Segoe UI", 9F);
                pnlRightCol.Controls.Add(cboStatus);
            }

            // Location
            lblLocation = new Label { Text = "Location / Virtual Link / Venue Details:", Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), Location = new Point(20, 280), AutoSize = true };
            pnlRightCol.Controls.Add(lblLocation);
            if (txtLocation != null)
            {
                txtLocation.BorderStyle = BorderStyle.FixedSingle;
                txtLocation.Location = new Point(20, 298);
                txtLocation.Width = 460;
                txtLocation.Font = new Font("Segoe UI", 9F);
                pnlRightCol.Controls.Add(txtLocation);
            }

            // Assessment Configuration Checkbox
            chkAssessmentRequired = new CheckBox
            {
                Text = "Requires Assessment/Exam after Interview",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Italic),
                ForeColor = Color.DimGray,
                Location = new Point(20, 335),
                AutoSize = true
            };
            chkAssessmentRequired.CheckedChanged += chkAssessmentRequired_CheckedChanged;
            pnlRightCol.Controls.Add(chkAssessmentRequired);

            lblAssessmentType = new Label { Text = "Assessment Type:", Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), Location = new Point(20, 365), AutoSize = true, Enabled = false };
            cboAssessmentType = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(20, 383), Width = 460, Font = new Font("Segoe UI", 9F), Enabled = false };
            pnlRightCol.Controls.Add(lblAssessmentType);
            pnlRightCol.Controls.Add(cboAssessmentType);

            // Save and Back Buttons style and layout
            if (btnSave != null)
            {
                btnSave.FlatStyle = FlatStyle.Flat;
                btnSave.BackColor = ColorSuccess;
                btnSave.ForeColor = Color.White;
                btnSave.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                btnSave.Location = new Point(330, 455);
                btnSave.Size = new Size(150, 38);
                btnSave.FlatAppearance.BorderSize = 0;
                btnSave.Cursor = Cursors.Hand;
                pnlRightCol.Controls.Add(btnSave);
            }

            if (btnBack != null)
            {
                btnBack.FlatStyle = FlatStyle.Flat;
                btnBack.BackColor = Color.FromArgb(230, 235, 240);
                btnBack.ForeColor = Color.FromArgb(64, 74, 86);
                btnBack.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
                btnBack.Location = new Point(230, 455);
                btnBack.Size = new Size(90, 38);
                btnBack.FlatAppearance.BorderColor = ColorBorder;
                btnBack.Cursor = Cursors.Hand;

                btnBack.Click -= btnBack_Click;
                btnBack.Click += btnBack_Click;

                pnlRightCol.Controls.Add(btnBack);
            }

            UpdateSegmentButtonStyles();
        }

        private void btnPendingQueue_Click(object sender, EventArgs e)
        {
            isViewingActiveSchedules = false;
            UpdateSegmentButtonStyles();
            ResetFields();
            LoadApplications();
            if (btnSave != null) btnSave.Text = "Schedule Interview";
        }

        private void btnActiveQueue_Click(object sender, EventArgs e)
        {
            isViewingActiveSchedules = true;
            UpdateSegmentButtonStyles();
            ResetFields();
            LoadApplications();
            if (btnSave != null) btnSave.Text = "Update Schedule";
        }

        private void UpdateSegmentButtonStyles()
        {
            if (!isViewingActiveSchedules)
            {
                btnPendingQueue.BackColor = ThemePrimary;
                btnPendingQueue.ForeColor = Color.White;

                btnActiveQueue.BackColor = Color.FromArgb(226, 232, 240);
                btnActiveQueue.ForeColor = Color.FromArgb(71, 85, 105);
            }
            else
            {
                btnActiveQueue.BackColor = ThemePrimary;
                btnActiveQueue.ForeColor = Color.White;

                btnPendingQueue.BackColor = Color.FromArgb(226, 232, 240);
                btnPendingQueue.ForeColor = Color.FromArgb(71, 85, 105);
            }
        }

        private void chkAssessmentRequired_CheckedChanged(object sender, EventArgs e)
        {
            lblAssessmentType.Enabled = chkAssessmentRequired.Checked;
            cboAssessmentType.Enabled = chkAssessmentRequired.Checked;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadMaintenanceData()
        {
            cboInterviewType.Items.Clear();
            cboAssessmentType.Items.Clear();

            try
            {
                using (OleDbConnection conn = DBConnection.GetConnection())
                {
                    if (conn == null) return;
                    conn.Open();

                    string qInterview = "SELECT TypeName FROM InterviewTypes WHERE IsActive = True ORDER BY TypeName ASC";
                    using (OleDbCommand cmd = new OleDbCommand(qInterview, conn))
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cboInterviewType.Items.Add(reader["TypeName"].ToString());
                        }
                    }

                    string qAssessment = "SELECT TypeName FROM AssessmentTypes WHERE IsActive = True ORDER BY TypeName ASC";
                    using (OleDbCommand cmd = new OleDbCommand(qAssessment, conn))
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cboAssessmentType.Items.Add(reader["TypeName"].ToString());
                        }
                    }
                }
            }
            catch
            {
                cboInterviewType.Items.AddRange(new object[] { "Initial HR Screening", "Technical Interview", "Managerial Interview", "Final Interview" });
                cboAssessmentType.Items.AddRange(new object[] { "Technical Programming Exam", "Cognitive Aptitude Test", "Personality Assessment" });
            }

            if (cboInterviewType.Items.Count > 0) cboInterviewType.SelectedIndex = 0;
            if (cboAssessmentType.Items.Count > 0) cboAssessmentType.SelectedIndex = 0;
        }

        private void LoadApplications()
        {
            try
            {
                using (OleDbConnection conn = DBConnection.GetConnection())
                {
                    if (conn == null)
                        return;

                    // FIX: Once evaluated and moved to 'For Assessment' or 'For Final Review', 
                    // the applicant is removed from active scheduling.
                    string statusFilter = isViewingActiveSchedules
                        ? "a.Status IN ('For Interview', 'Interview Scheduled')"
                        : "a.Status = 'Shortlisted'";

                    string query = $@"
                        SELECT
                            a.ApplicationID,
                            ap.FirstName & ' ' & ap.LastName AS FullName,
                            p.PositionName AS JobTitle,
                            a.Status
                        FROM ((Applications a
                        INNER JOIN Applicants ap ON a.ApplicantID = ap.ApplicantID)
                        INNER JOIN JobVacancies j ON a.JobID = j.JobID)
                        INNER JOIN Positions p ON j.PositionID = p.PositionID
                        WHERE {statusFilter}";

                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn))
                    {
                        dtApplications.Clear();
                        adapter.Fill(dtApplications);
                        dgvApplicants.DataSource = dtApplications;

                        if (dgvApplicants.Columns["ApplicationID"] != null)
                            dgvApplicants.Columns["ApplicationID"].Visible = false;

                        if (dgvApplicants.Columns["Status"] != null)
                            dgvApplicants.Columns["Status"].Visible = false;

                        if (dgvApplicants.Columns["FullName"] != null)
                            dgvApplicants.Columns["FullName"].HeaderText = "Applicant Name";

                        if (dgvApplicants.Columns["JobTitle"] != null)
                            dgvApplicants.Columns["JobTitle"].HeaderText = "Applied Position";

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

        private void dgvApplicants_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvApplicants.Rows[e.RowIndex];

                selectedApplicationID = Convert.ToInt32(row.Cells["ApplicationID"].Value);

                lblCandidateName.Text = "Candidate: " + row.Cells["FullName"].Value.ToString();
                lblCandidateJob.Text = "Job Position: " + row.Cells["JobTitle"].Value.ToString();

                if (isViewingActiveSchedules)
                {
                    LoadActiveScheduleDetails(selectedApplicationID);
                }
            }
        }

        private void LoadActiveScheduleDetails(int applicationId)
        {
            try
            {
                using (OleDbConnection conn = DBConnection.GetConnection())
                {
                    if (conn == null) return;
                    conn.Open();

                    string query = @"
                        SELECT TOP 1 InterviewDate, Interviewer, Location, Status 
                        FROM InterviewSchedules 
                        WHERE ApplicationID = ? AND Status = 'Scheduled'
                        ORDER BY InterviewScheduleID DESC";

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("?", applicationId);
                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                DateTime dbDateTime = Convert.ToDateTime(reader["InterviewDate"]);
                                dtpInterviewDate.Value = dbDateTime.Date;
                                dtpInterviewTime.Value = dbDateTime;

                                string rawInterviewer = reader["Interviewer"].ToString();

                                if (rawInterviewer.Contains(" - "))
                                {
                                    string[] tokens = rawInterviewer.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries);
                                    if (tokens.Length > 1)
                                    {
                                        string typeToken = tokens[0];
                                        if (cboInterviewType.Items.Contains(typeToken))
                                            cboInterviewType.SelectedItem = typeToken;

                                        txtInterviewer.Text = tokens[1];
                                    }
                                }
                                else
                                {
                                    txtInterviewer.Text = rawInterviewer;
                                }

                                txtLocation.Text = reader["Location"].ToString();

                                string dbStatus = reader["Status"].ToString();
                                if (cboStatus.Items.Contains(dbStatus))
                                    cboStatus.SelectedItem = dbStatus;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading active schedule details:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ResetFields()
        {
            selectedApplicationID = 0;
            lblCandidateName.Text = "Candidate: Please select from the queue";
            lblCandidateJob.Text = "Job Position: —";
            txtInterviewer.Clear();
            txtLocation.Clear();
            chkAssessmentRequired.Checked = false;

            // Ensures a standard non-null default value on resets
            if (cboStatus != null && cboStatus.Items.Count > 0)
            {
                cboStatus.SelectedIndex = cboStatus.Items.IndexOf("Scheduled") >= 0
                    ? cboStatus.Items.IndexOf("Scheduled")
                    : 0;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (selectedApplicationID == 0)
            {
                MessageBox.Show(
                    "Please select an applicant from the list first.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            DateTime interviewDate = dtpInterviewDate.Value.Date;
            TimeSpan interviewTime = dtpInterviewTime.Value.TimeOfDay;
            DateTime combinedDateTime = interviewDate.Add(interviewTime);

            // Rejects invalid past-date parameters
            if (combinedDateTime < DateTime.Now)
            {
                MessageBox.Show(
                    "Invalid Schedule. The interview date and time cannot be set in the past.",
                    "Date Validation Error",
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
                    "Please enter the interview location or online link details.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // Correct mapping with fallbacks to avoid null references
            string selectedStatus = cboStatus.SelectedItem?.ToString() ?? "Scheduled";
            string nextStatus = "For Interview"; // Baseline capstone compliance state

            if (selectedStatus == "Cancelled")
            {
                nextStatus = "Shortlisted"; // Revert status so candidate can be rescheduled
            }
            else if (selectedStatus == "Completed")
            {
                nextStatus = chkAssessmentRequired.Checked ? "For Assessment" : "For Final Review";

                // Explanatory dialog warning that marking as Completed skips evaluation
                MessageBox.Show(
                    "Warning: Saving this interview schedule directly as 'Completed' will transition the applicant " +
                    $"straight to '{nextStatus}', skipping the Interview Evaluation workflow stage.\n\n" +
                    "To evaluate the candidate first, please save this schedule as 'Scheduled' and open the Evaluation tab.",
                    "Workflow Advice",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            string interviewType = cboInterviewType.SelectedItem?.ToString() ?? "Standard Interview";

            DialogResult result = MessageBox.Show(
                $"Save this {interviewType} schedule with status '{selectedStatus}'?\n\nApplicant status will transition to '{nextStatus}'.",
                "Confirm Schedule",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            OleDbConnection conn = DBConnection.GetConnection();
            if (conn == null)
                return;

            OleDbTransaction transaction = null;

            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();

                if (!isViewingActiveSchedules)
                {
                    // CASE A: Create a brand new Interview Schedule record
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
                        VALUES (?, ?, ?, ?, ?, ?)";

                    using (OleDbCommand cmd = new OleDbCommand(insertSchedule, conn, transaction))
                    {
                        cmd.Parameters.Add("@ApplicationID", OleDbType.Integer).Value = selectedApplicationID;
                        cmd.Parameters.Add("@InterviewDate", OleDbType.Date).Value = combinedDateTime;
                        cmd.Parameters.Add("@Interviewer", OleDbType.VarWChar).Value = $"{interviewType} - {txtInterviewer.Text.Trim()}";
                        cmd.Parameters.Add("@Location", OleDbType.VarWChar).Value = txtLocation.Text.Trim();
                        cmd.Parameters.Add("@Mode", OleDbType.VarWChar).Value = "Standard";
                        cmd.Parameters.Add("@Status", OleDbType.VarWChar).Value = selectedStatus;

                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    // CASE B: Update the existing Schedule record
                    string updateSchedule =
                        @"UPDATE InterviewSchedules 
                          SET InterviewDate = ?, Interviewer = ?, Location = ?, Status = ?
                          WHERE ApplicationID = ? AND Status = 'Scheduled'";

                    using (OleDbCommand cmd = new OleDbCommand(updateSchedule, conn, transaction))
                    {
                        cmd.Parameters.Add("@InterviewDate", OleDbType.Date).Value = combinedDateTime;
                        cmd.Parameters.Add("@Interviewer", OleDbType.VarWChar).Value = $"{interviewType} - {txtInterviewer.Text.Trim()}";
                        cmd.Parameters.Add("@Location", OleDbType.VarWChar).Value = txtLocation.Text.Trim();
                        cmd.Parameters.Add("@Status", OleDbType.VarWChar).Value = selectedStatus;
                        cmd.Parameters.Add("@ApplicationID", OleDbType.Integer).Value = selectedApplicationID;

                        cmd.ExecuteNonQuery();
                    }
                }

                // 2. Transition state in the Applications Table
                string updateApplication =
                    @"UPDATE Applications
                      SET Status = ?
                      WHERE ApplicationID = ?";

                using (OleDbCommand cmd = new OleDbCommand(updateApplication, conn, transaction))
                {
                    cmd.Parameters.Add("@Status", OleDbType.VarWChar).Value = nextStatus;
                    cmd.Parameters.Add("@ApplicationID", OleDbType.Integer).Value = selectedApplicationID;

                    cmd.ExecuteNonQuery();
                }

                // 3. Log history of this transition
                string insertHistory =
                    @"INSERT INTO ApplicationStatusHistory (ApplicationID, Status, DateChanged)
                      VALUES (?, ?, ?)";

                using (OleDbCommand cmd = new OleDbCommand(insertHistory, conn, transaction))
                {
                    cmd.Parameters.Add("@ApplicationID", OleDbType.Integer).Value = selectedApplicationID;
                    cmd.Parameters.Add("@Status", OleDbType.VarWChar).Value = nextStatus;
                    cmd.Parameters.Add("@DateChanged", OleDbType.Date).Value = DateTime.Now;

                    cmd.ExecuteNonQuery();
                }

                // 4. Safely query actual columns to record Audit Log entry without schema crashes
                List<string> columns = new List<string>();
                using (OleDbCommand schemaCmd = new OleDbCommand("SELECT TOP 1 * FROM AuditTrail", conn, transaction))
                using (OleDbDataReader reader = schemaCmd.ExecuteReader())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        columns.Add(reader.GetName(i).ToLower());
                    }
                }

                List<string> insertCols = new List<string>();
                List<string> valuePlaceholders = new List<string>();
                List<object> values = new List<object>();
                List<OleDbType> types = new List<OleDbType>();

                // UserID column checks
                string matchedUser = columns.Contains("userid") ? "UserID" : (columns.Contains("user_id") ? "User_ID" : null);
                if (matchedUser != null)
                {
                    insertCols.Add($"[{matchedUser}]");
                    valuePlaceholders.Add("?");
                    values.Add(UserSession.UserID > 0 ? UserSession.UserID : 1);
                    types.Add(OleDbType.Integer);
                }

                // Action Column checks
                string matchedAction = columns.Contains("action") ? "Action" : (columns.Contains("actionperformed") ? "ActionPerformed" : (columns.Contains("description") ? "Description" : null));
                if (matchedAction != null)
                {
                    insertCols.Add($"[{matchedAction}]");
                    valuePlaceholders.Add("?");
                    string actionPrefix = isViewingActiveSchedules ? "Updated" : "Created";
                    string auditMsg = $"{actionPrefix} schedule for {interviewType} as '{selectedStatus}' on {combinedDateTime:MM/dd/yyyy} (App ID: {selectedApplicationID}).";
                    if (chkAssessmentRequired.Checked)
                    {
                        auditMsg += $" Post-interview route requires Assessment: {cboAssessmentType.SelectedItem?.ToString()}.";
                    }
                    values.Add(auditMsg);
                    types.Add(OleDbType.VarWChar);
                }

                // Log Date checks
                string matchedDate = columns.Contains("actiondate") ? "ActionDate" : (columns.Contains("logdate") ? "LogDate" : (columns.Contains("datechanged") ? "DateChanged" : null));
                if (matchedDate != null)
                {
                    insertCols.Add($"[{matchedDate}]");
                    valuePlaceholders.Add("?");
                    values.Add(DateTime.Now);
                    types.Add(OleDbType.Date);
                }

                if (insertCols.Count > 0)
                {
                    string sqlAudit = $"INSERT INTO AuditTrail ({string.Join(", ", insertCols)}) VALUES ({string.Join(", ", valuePlaceholders)})";
                    using (OleDbCommand cmd = new OleDbCommand(sqlAudit, conn, transaction))
                    {
                        for (int i = 0; i < values.Count; i++)
                        {
                            cmd.Parameters.Add("?", types[i]).Value = values[i];
                        }
                        cmd.ExecuteNonQuery();
                    }
                }

                transaction.Commit();

                MessageBox.Show(
                    "Interview schedule saved successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ResetFields();
                LoadApplications();
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    try { transaction.Rollback(); } catch { }
                }

                MessageBox.Show(
                    "Error saving schedule:\n" + ex.Message,
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
    }
}