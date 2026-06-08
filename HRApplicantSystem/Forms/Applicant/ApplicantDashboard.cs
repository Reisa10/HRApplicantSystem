using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using HRApplicantSystem.Classes;
using HRApplicantSystem.Database;
using HRApplicantSystem.Forms.Login;

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
            lblWelcome.Text = $"Welcome back, {UserSession.FullName ?? "Applicant"}!";
            RefreshDashboardSummary();
        }

        private void RefreshDashboardSummary()
        {
            try
            {
                // Retrieve the list of missing mandatory files
                List<string> missing = DatabaseHelper.GetMissingRequirements(UserSession.UserID);

                // Dynamically update the button text to prevent DPI overlapping [1]
                if (missing == null || missing.Count == 0)
                {
                    btnDocuments.Text = "MY DOCUMENTS\r\n\r\n[Upload and Edit Documents]\r\n✅ All Uploaded";
                }
                else
                {
                    btnDocuments.Text = $"MY DOCUMENTS\r\n\r\n[Upload and Edit Documents]\r\n⚠️ {missing.Count} File{(missing.Count > 1 ? "s" : "")} Missing";
                }
            }
            catch
            {
                // Fallback to default text if database query fails
                btnDocuments.Text = "MY DOCUMENTS\r\n\r\n[Upload and Edit Documents]";
            }
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
                this.Hide();
                docForm.ShowDialog();
                this.Show();
                RefreshDashboardSummary();
            }
            // Instantly refresh the button status line when returning from Documents form
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

            // Card Style 1: Profile Blue
            btnProfile.FlatStyle = FlatStyle.Flat;
            btnProfile.FlatAppearance.BorderSize = 0;
            btnProfile.BackColor = Color.FromArgb(41, 128, 185);
            btnProfile.ForeColor = Color.White;
            btnProfile.Cursor = Cursors.Hand;
            btnProfile.Font = new Font("Segoe UI", 11F, FontStyle.Bold);

            // Card Style 2: Job Vacancy Green
            btnJobVacancies.FlatStyle = FlatStyle.Flat;
            btnJobVacancies.FlatAppearance.BorderSize = 0;
            btnJobVacancies.BackColor = Color.FromArgb(39, 174, 96);
            btnJobVacancies.ForeColor = Color.White;
            btnJobVacancies.Cursor = Cursors.Hand;
            btnJobVacancies.Font = new Font("Segoe UI", 11F, FontStyle.Bold);

            // Card Style 3: Applications Purple
            btnMyApplications.FlatStyle = FlatStyle.Flat;
            btnMyApplications.FlatAppearance.BorderSize = 0;
            btnMyApplications.BackColor = Color.FromArgb(142, 68, 173);
            btnMyApplications.ForeColor = Color.White;
            btnMyApplications.Cursor = Cursors.Hand;
            btnMyApplications.Font = new Font("Segoe UI", 11F, FontStyle.Bold);

            // Card Style 4: Status Tracking Amber/Orange [2]
            btnStatusTracking.FlatStyle = FlatStyle.Flat;
            btnStatusTracking.FlatAppearance.BorderSize = 0;
            btnStatusTracking.BackColor = Color.FromArgb(230, 126, 34);
            btnStatusTracking.ForeColor = Color.White;
            btnStatusTracking.Cursor = Cursors.Hand;
            btnStatusTracking.Font = new Font("Segoe UI", 11F, FontStyle.Bold);

            // Card Style 5: Documents Teal Accent (Immune to layout overlapping)
            btnDocuments.FlatStyle = FlatStyle.Flat;
            btnDocuments.FlatAppearance.BorderSize = 0;
            btnDocuments.BackColor = Color.FromArgb(26, 188, 156);
            btnDocuments.ForeColor = Color.White;
            btnDocuments.Cursor = Cursors.Hand;
            btnDocuments.Font = new Font("Segoe UI", 11F, FontStyle.Bold);

            // Logout Button Style
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.FlatAppearance.BorderSize = 1;
            btnLogout.FlatAppearance.BorderColor = Color.FromArgb(192, 57, 43);
            btnLogout.BackColor = Color.White;
            btnLogout.ForeColor = Color.FromArgb(192, 57, 43);
            btnLogout.Cursor = Cursors.Hand;
            btnLogout.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}