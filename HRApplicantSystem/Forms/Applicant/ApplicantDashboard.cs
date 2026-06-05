using System;
using System.Drawing;
using System.Windows.Forms;
using HRApplicantSystem.Classes;
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
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            using (ProfileForm profileForm = new ProfileForm())
            {
                this.Hide();
                profileForm.ShowDialog();
                this.Show();
            }
        }

        private void btnJobVacancies_Click(object sender, EventArgs e)
        {
            using (JobVacancyForm jobForm = new JobVacancyForm())
            {
                this.Hide();
                jobForm.ShowDialog();
                this.Show();
            }
        }

        private void btnMyApplications_Click(object sender, EventArgs e)
        {
            using (MyApplicationForm appForm = new MyApplicationForm())
            {
                this.Hide();
                appForm.ShowDialog();
                this.Show();
            }
        }

        private void btnStatusTracking_Click(object sender, EventArgs e)
        {
            using (ApplicationStatusForm statusForm = new ApplicationStatusForm())
            {
                this.Hide();
                statusForm.ShowDialog();
                this.Show();
            }
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

            // Logout Button Style
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.FlatAppearance.BorderSize = 1;
            btnLogout.FlatAppearance.BorderColor = Color.FromArgb(192, 57, 43);
            btnLogout.BackColor = Color.White;
            btnLogout.ForeColor = Color.FromArgb(192, 57, 43);
            btnLogout.Cursor = Cursors.Hand;
            btnLogout.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        }

        private void btnDocuments_Click(object sender, EventArgs e)
        {
            DocumentsForm docForm = new DocumentsForm(UserSession.UserID);
            docForm.ShowDialog();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}