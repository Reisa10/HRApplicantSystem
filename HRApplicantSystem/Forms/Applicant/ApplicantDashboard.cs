using HRApplicantSystem.Classes;
using HRApplicantSystem.Database;
using HRApplicantSystem.Forms.Login;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace HRApplicantSystem.Forms.Applicant
{
    public partial class ApplicantDashboard : Form
    {
        private bool isLoggingOut = false;

        public ApplicantDashboard()
        {
            InitializeComponent();
            ApplyModernStyles();
        }

        private void ApplicantDashboard_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = $"Welcome back,\r\n{UserSession.FullName ?? "Applicant"}";
            RefreshDashboardSummary();
        }

        private void RefreshDashboardSummary()
        {
            // Retain your original sidebar styling logic
            btnDocuments.Text = "    📁   My Documents";

            int totalApps = 0;
            int activeApps = 0;
            int approvedApps = 0;

            using (OleDbConnection con = DBConnection.GetConnection())
            {
                if (con != null)
                {
                    try
                    {
                        con.Open();

                        // 1. Fetch Total Applications for this specific logged-in applicant
                        string totalQuery = "SELECT COUNT(*) FROM Applications WHERE ApplicantID = ?";
                        using (OleDbCommand cmd = new OleDbCommand(totalQuery, con))
                        {
                            cmd.Parameters.AddWithValue("?", UserSession.UserID);
                            totalApps = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        // 2. Fetch Active Applications (applications not yet finalized as Accepted, Rejected, or Withdrawn)
                        string activeQuery = "SELECT COUNT(*) FROM Applications WHERE ApplicantID = ? AND Status NOT IN ('Accepted', 'Rejected', 'Withdrawn')";
                        using (OleDbCommand cmd = new OleDbCommand(activeQuery, con))
                        {
                            cmd.Parameters.AddWithValue("?", UserSession.UserID);
                            activeApps = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        // 3. Fetch Approved (Hired) Applications for this applicant
                        string approvedQuery = "SELECT COUNT(*) FROM Applications WHERE ApplicantID = ? AND Status = 'Accepted'";
                        using (OleDbCommand cmd = new OleDbCommand(approvedQuery, con))
                        {
                            cmd.Parameters.AddWithValue("?", UserSession.UserID);
                            approvedApps = Convert.ToInt32(cmd.ExecuteScalar());
                        }
                    }
                    catch (Exception ex)
                    {
                        // Logs errors silently during debugging to keep the dashboard stable if database is locked
                        System.Diagnostics.Debug.WriteLine("Error loading dashboard metrics: " + ex.Message);
                    }
                }
            }

            // Safely update the designer-defined labels with the calculated counts
            lblCard1Value.Text = totalApps.ToString();
            lblCard2Value.Text = activeApps.ToString();
            lblCard3Value.Text = approvedApps.ToString();
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            using (ProfileForm profileForm = new ProfileForm())
            {
                this.Hide();
                profileForm.ShowDialog();
                this.Show();
                RefreshDashboardSummary();
            }
        }

        private void btnJobVacancies_Click(object sender, EventArgs e)
        {
            using (JobVacancyForm jobForm = new JobVacancyForm())
            {
                this.Hide();
                jobForm.ShowDialog();
                this.Show();
                RefreshDashboardSummary();
            }
        }

        private void btnMyApplications_Click(object sender, EventArgs e)
        {
            using (MyApplicationForm appForm = new MyApplicationForm())
            {
                this.Hide();
                appForm.ShowDialog();
                this.Show();
                RefreshDashboardSummary();
            }
        }

        private void btnStatusTracking_Click(object sender, EventArgs e)
        {
            using (ApplicationStatusForm statusForm = new ApplicationStatusForm())
            {
                this.Hide();
                statusForm.ShowDialog();
                this.Show();
                RefreshDashboardSummary();
            }
        }

        private void btnDocuments_Click(object sender, EventArgs e)
        {
            using (DocumentsForm docForm = new DocumentsForm(UserSession.UserID))
            {
                docForm.ShowDialog();
            }
            RefreshDashboardSummary();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to log out?",
                                                  "Logout Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                UserSession.UserID = 0;
                UserSession.Username = string.Empty;
                UserSession.FullName = string.Empty;
                UserSession.Role = string.Empty;

                isLoggingOut = true;
                this.Close();
            }
        }

        private void ApplicantDashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isLoggingOut)
            {
                Form loginForm = null;

                foreach (Form openForm in Application.OpenForms)
                {
                    if (openForm is LoginForm)
                    {
                        loginForm = openForm;
                        break;
                    }
                }

                if (loginForm != null)
                {
                    loginForm.Show();
                }
                else
                {
                    LoginForm newLogin = new LoginForm();
                    newLogin.Show();
                }
            }
            else
            {
                Application.Exit();
            }
        }

        private void ApplyModernStyles()
        {
            this.BackColor = Color.FromArgb(245, 247, 250);

            lblHeader.Text = "APPLICANT PROCESSING DASHBOARD";

            lblMainTitle.Text = "Application Portal";

            // 3. Make the body description message fit the page dynamically
            lblMainDesc.AutoSize = false;
            lblMainDesc.Width = panelContent.Width - 50; // Sets a proportional width with padding
            lblMainDesc.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right; // Resizes automatically with the window size

            // Keep your sidebar styling logic
            StyleSidebarButton(btnProfile, "    👤   My Profile");
            StyleSidebarButton(btnJobVacancies, "    💼   Job Vacancies");
            StyleSidebarButton(btnMyApplications, "    📝   My Applications");
            StyleSidebarButton(btnStatusTracking, "    📊   Status Tracking");
            StyleSidebarButton(btnDocuments, "    📁   My Documents");

            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatAppearance.MouseOverBackColor = Color.FromArgb(34, 45, 60);
            btnLogout.FlatAppearance.MouseDownBackColor = Color.FromArgb(20, 27, 36);
            btnLogout.BackColor = Color.Transparent;
            btnLogout.ForeColor = Color.FromArgb(231, 76, 60);
            btnLogout.Cursor = Cursors.Hand;
            btnLogout.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLogout.TextAlign = ContentAlignment.MiddleLeft;
            btnLogout.Text = "    🚪   Sign Out";

            timerDateTime.Start();
            UpdateDateTimeLabel();
        }

        private void StyleSidebarButton(Button btn, string text)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(34, 45, 60);
            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(20, 27, 36);
            btn.BackColor = Color.Transparent;
            btn.ForeColor = Color.FromArgb(200, 214, 229);
            btn.Cursor = Cursors.Hand;
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Text = text;
        }

        private void timerDateTime_Tick(object sender, EventArgs e)
        {
            UpdateDateTimeLabel();
        }

        private void UpdateDateTimeLabel()
        {
            lblDateTime.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy  h:mm:ss tt");

            // Reposition date-time control on the right based on its auto-sized width
            lblDateTime.Left = panelHeader.Width - lblDateTime.Width - 25;

            // Vertically center labels within the header panel height dynamically
            lblDateTime.Top = (panelHeader.Height - lblDateTime.Height) / 2;
            lblHeader.Top = (panelHeader.Height - lblHeader.Height) / 2;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}