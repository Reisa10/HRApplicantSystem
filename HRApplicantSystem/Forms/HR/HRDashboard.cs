using System;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;
using HRApplicantSystem.Classes;       // Reference UserSession
using HRApplicantSystem.Forms.Login;     // Reference LoginForm
using HRApplicantSystem.Database;        // Reference DBConnection

namespace HRApplicantSystem.Forms.HR
{
    public partial class HRDashboard : Form
    {
        public HRDashboard()
        {
            InitializeComponent();
            SetupDashboardConsole();
        }

        private void HRDashboard_Load(object sender, EventArgs e)
        {
            // Populate welcome user details dynamically
            lblUserFullName.Text = !string.IsNullOrEmpty(UserSession.FullName) ? UserSession.FullName : "System User";
            lblUserRole.Text = !string.IsNullOrEmpty(UserSession.Role) ? UserSession.Role.ToUpper() : "SPECIALIST";

            // Enforce role-based access restrictions
            ApplyRolePermissions();

            // Start the live system clock
            SystemClockTimer.Start();
        }

        /// <summary>
        /// Programs access policies based on the established session role.
        /// </summary>
        private void ApplyRolePermissions()
        {
            string role = UserSession.Role;
            bool isManagerOrAdmin = (role == "HR Manager" || role == "Admin");

            // HR Staff can view, review, screen, and schedule.
            ApplicantReview.Visible = true;
            Screening.Visible = true;
            InterviewSchedule.Visible = true;
            InterviewEvaluation.Visible = true;

            // Restrict sensitive functions strictly to HR Manager / Admin roles
            HiringDecision.Visible = isManagerOrAdmin;
            SystemMaintenance.Visible = isManagerOrAdmin;
            JobVacancyManagement.Visible = isManagerOrAdmin;
            Reports.Visible = isManagerOrAdmin;
        }

        private void SystemClockTimer_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy   h:mm:ss tt");
        }

        #region Operational Window Launcher (Hides Dashboard & Restores on Close)
        /// <summary>
        /// Launches child forms, hides the dashboard, and restores it when the child form is closed.
        /// </summary>
        private void LaunchModule(Form childForm, Button senderButton)
        {
            try
            {
                // Subscribe to the FormClosed event to bring back the dashboard
                childForm.FormClosed += (sender, e) =>
                {
                    this.Show();
                    SetupDashboardConsole(); // Optional: Refresh dashboard counts upon return
                };

                // Center the form on the screen
                childForm.StartPosition = FormStartPosition.CenterScreen;

                // Hide the dashboard and display the module
                this.Hide();
                childForm.Show();
            }
            catch (Exception ex)
            {
                // Ensure the dashboard is visible if the child form fails to open
                this.Show();
                MessageBox.Show("Failed to open module: " + ex.Message, "System Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        #region Dashboard Console Setup
        private void SetupDashboardConsole()
        {
            workspaceContainerPanel.Controls.Clear();

            // Create metrics grid
            TableLayoutPanel kpiPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 125,
                ColumnCount = 3,
                RowCount = 1,
                Padding = new Padding(10)
            };
            kpiPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3F));
            kpiPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3F));
            kpiPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3F));

            workspaceContainerPanel.Controls.Add(kpiPanel);

            // Load counts dynamically from database
            int candidates = 0, openApps = 0, hiredCount = 0;
            using (OleDbConnection con = DBConnection.GetConnection())
            {
                if (con != null)
                {
                    try
                    {
                        con.Open();
                        using (OleDbCommand cmd = new OleDbCommand("SELECT COUNT(*) FROM Applicants", con))
                            candidates = Convert.ToInt32(cmd.ExecuteScalar());

                        using (OleDbCommand cmd = new OleDbCommand("SELECT COUNT(*) FROM Applications WHERE Status IN ('Submitted', 'Under Review', 'Shortlisted')", con))
                            openApps = Convert.ToInt32(cmd.ExecuteScalar());

                        using (OleDbCommand cmd = new OleDbCommand("SELECT COUNT(*) FROM Applications WHERE Status = 'Accepted'", con))
                            hiredCount = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    catch { /* Fallback */ }
                }
            }

            kpiPanel.Controls.Add(CreateStatCard("TOTAL CANDIDATES IN SYSTEM", candidates.ToString(), Color.FromArgb(41, 128, 185)), 0, 0);
            kpiPanel.Controls.Add(CreateStatCard("ACTIVE OPERATION PIPELINE", openApps.ToString(), Color.FromArgb(230, 126, 34)), 1, 0);
            kpiPanel.Controls.Add(CreateStatCard("SUCCESSFUL PLACEMENTS", hiredCount.ToString(), Color.FromArgb(39, 174, 96)), 2, 0);

            // Add welcome banner graphic panel
            Panel welcomeBanner = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(20),
                BackColor = Color.White,
                Padding = new Padding(30)
            };
            workspaceContainerPanel.Controls.Add(welcomeBanner);
            welcomeBanner.BringToFront();

            Label lblWelcomeHeader = new Label
            {
                Text = "Talent Acquisition & Recruitment Portal",
                Font = new Font("Segoe UI Semibold", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(30, 30),
                AutoSize = true
            };
            welcomeBanner.Controls.Add(lblWelcomeHeader);

            Label lblWelcomeBody = new Label
            {
                Text = "Select an operation from the sidebar navigation panel on the left to launch the respective module. " +
                       "The Dashboard will temporarily close and return once you exit the selected module.",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.DimGray,
                Location = new Point(33, 75),
                Size = new Size(700, 80)
            };
            welcomeBanner.Controls.Add(lblWelcomeBody);
        }

        private Panel CreateStatCard(string header, string value, Color color)
        {
            Panel card = new Panel { BackColor = Color.White, Dock = DockStyle.Fill, Padding = new Padding(15, 10, 10, 10), Margin = new Padding(8) };
            Panel indicator = new Panel { Dock = DockStyle.Left, Width = 5, BackColor = color };
            card.Controls.Add(indicator);

            Label lblHeader = new Label { Text = header, Font = new Font("Segoe UI", 8, FontStyle.Bold), ForeColor = Color.DarkGray, Location = new Point(15, 12), Size = new Size(220, 15) };
            Label lblValue = new Label { Text = value, Font = new Font("Segoe UI Semibold", 22, FontStyle.Bold), ForeColor = Color.FromArgb(44, 62, 80), Location = new Point(15, 28), Size = new Size(220, 40) };

            card.Controls.Add(lblHeader);
            card.Controls.Add(lblValue);
            return card;
        }
        #endregion

        #region Navigation Events
        private void btnHome_Click(object sender, EventArgs e)
        {
            SetupDashboardConsole();
        }

        private void ApplicantReview_Click(object sender, EventArgs e)
        {
            LaunchModule(new ApplicantReviewForm(), ApplicantReview);
        }

        private void Screening_Click(object sender, EventArgs e)
        {
            LaunchModule(new ScreeningForm(), Screening);
        }

        private void InterviewSchedule_Click(object sender, EventArgs e)
        {
            LaunchModule(new InterviewScheduleForm(), InterviewSchedule);
        }

        private void InterviewEvaluation_Click(object sender, EventArgs e)
        {
            LaunchModule(new InterviewEvaluationForm(), InterviewEvaluation);
        }

        private void HiringDecision_Click(object sender, EventArgs e)
        {
            LaunchModule(new HiringDecisionForm(), HiringDecision);
        }

        private void SystemMaintenance_Click(object sender, EventArgs e)
        {
            try
            {
                this.Hide();
                using (MaintenanceForm frm = new MaintenanceForm())
                {
                    frm.StartPosition = FormStartPosition.CenterScreen;
                    frm.ShowDialog();
                }
            }
            finally
            {
                this.Show();
                SetupDashboardConsole();
            }
        }

        private void JobVacancyManagement_Click(object sender, EventArgs e)
        {
            LaunchModule(new JobVacancyManagementForm(), JobVacancyManagement);
        }

        private void Reports_Click(object sender, EventArgs e)
        {
            LaunchModule(new ReportsForm(), Reports);
        }

        private void Logout_Click(object sender, EventArgs e)
        {
            DialogResult confirm = MessageBox.Show("Are you sure you want to sign out?", "Logout Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.No) return;

            UserSession.UserID = 0;
            UserSession.Username = null;
            UserSession.FullName = null;
            UserSession.Role = null;

            LoginForm loginForm = new LoginForm();
            loginForm.Show();

            this.Close();
        }
        #endregion
    }
}