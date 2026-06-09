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
            ApplyModernStyles();
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
            UpdateDateTimeLabel(); // Initial layout alignment
        }

        /// <summary>
        /// Programs access policies based on the established session role.
        /// </summary>
        private void ApplyRolePermissions()
        {
            string role = UserSession.Role;
            bool isManagerOrAdmin = (role == "HR Manager" || role == "Admin");

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
            UpdateDateTimeLabel();
        }

        private void UpdateDateTimeLabel()
        {
            lblDateTime.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy   h:mm:ss tt");

            // Reposition date-time control on the right based on its auto-sized width
            lblDateTime.Left = headerPanel.Width - lblDateTime.Width - 25;

            // Vertically center labels within the header panel height dynamically
            lblDateTime.Top = (headerPanel.Height - lblDateTime.Height) / 2;
            lblHeaderTitle.Top = (headerPanel.Height - lblHeaderTitle.Height) / 2;
        }

        #region Operational Window Launcher (Hides Dashboard & Restores on Close)
        private void LaunchModule(Form childForm, Button senderButton)
        {
            try
            {
                childForm.FormClosed += (sender, e) =>
                {
                    this.Show();
                    SetupDashboardConsole();
                };

                childForm.StartPosition = FormStartPosition.CenterScreen;
                this.Hide();
                childForm.Show();
            }
            catch (Exception ex)
            {
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

            // Add welcome banner panel
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
            // Removed parent padding so docked accent bar sits cleanly at X = 0
            Panel card = new Panel { BackColor = Color.White, Dock = DockStyle.Fill, Margin = new Padding(8) };

            Panel indicator = new Panel { Dock = DockStyle.Left, Width = 6, BackColor = color };
            card.Controls.Add(indicator);

            // Positioned text starting at X = 22 to leave a comfortable gap from the indicator bar
            Label lblHeader = new Label
            {
                Text = header,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                ForeColor = Color.DarkGray,
                Location = new Point(22, 14),
                AutoSize = true
            };

            Label lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI Semibold", 22, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(22, 32),
                AutoSize = true
            };

            card.Controls.Add(lblHeader);
            card.Controls.Add(lblValue);
            return card;
        }
        #endregion

        #region Theme Customization
        private void ApplyModernStyles()
        {
            this.BackColor = Color.FromArgb(245, 247, 250);
            sidebarPanel.BackColor = Color.FromArgb(26, 34, 45);
            profilePanel.BackColor = Color.FromArgb(20, 27, 36);

            lblWelcome.Font = new Font("Segoe UI", 8.5F);
            lblWelcome.ForeColor = Color.FromArgb(200, 214, 229);

            StyleSidebarButton(btnHome, "🏠  Dashboard Home", true);
            StyleSidebarButton(ApplicantReview, "👥  Applicant Review", false);
            StyleSidebarButton(Screening, "🔍  Basic Screening", false);
            StyleSidebarButton(InterviewSchedule, "📅  Interview Schedule", false);
            StyleSidebarButton(InterviewEvaluation, "📝  Interview Evaluation", false);
            StyleSidebarButton(HiringDecision, "⚖️  Hiring Decisions", false);
            StyleSidebarButton(JobVacancyManagement, "💼  Job Vacancies", false);
            StyleSidebarButton(SystemMaintenance, "⚙️  System Maintenance", false);
            StyleSidebarButton(Reports, "📊  Recruitment Reports", false);

            Logout.FlatStyle = FlatStyle.Flat;
            Logout.FlatAppearance.BorderSize = 0;
            Logout.FlatAppearance.MouseOverBackColor = Color.FromArgb(34, 45, 60);
            Logout.FlatAppearance.MouseDownBackColor = Color.FromArgb(20, 27, 36);
            Logout.BackColor = Color.Transparent;
            Logout.ForeColor = Color.FromArgb(231, 76, 60);
            Logout.Cursor = Cursors.Hand;
            Logout.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            Logout.TextAlign = ContentAlignment.MiddleLeft;
            Logout.Text = "🚪  Sign Out";
        }

        private void StyleSidebarButton(Button btn, string text, bool isBold)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(34, 45, 60);
            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(20, 27, 36);
            btn.BackColor = Color.Transparent;
            btn.ForeColor = isBold ? Color.White : Color.FromArgb(200, 214, 229);
            btn.Cursor = Cursors.Hand;
            btn.Font = new Font("Segoe UI", 9.5F, isBold ? FontStyle.Bold : FontStyle.Regular);
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Text = text;
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