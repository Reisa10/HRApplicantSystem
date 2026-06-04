using System;
using System.Drawing;
using System.Windows.Forms;
using HRApplicantSystem.Classes;
using HRApplicantSystem.Forms.Login; // Ensure reference to LoginForm is imported

namespace HRApplicantSystem.Forms.Applicant
{
    public partial class ApplicantDashboard : Form
    {
        // Track if the form is closing due to a conscious logout action
        private bool isLoggingOut = false;

        public ApplicantDashboard()
        {
            InitializeComponent();
            ApplyModernStyles();
        }

        private void ApplicantDashboard_Load(object sender, EventArgs e)
        {
            // Dynamically welcome the applicant using UserSession details
            lblWelcome.Text = $"Welcome back, {UserSession.FullName ?? "Applicant"}!";
        }

        /// <summary>
        /// Opens the Profile Form, hiding the dashboard until the user returns.
        /// </summary>
        private void btnProfile_Click(object sender, EventArgs e)
        {
            using (ProfileForm profileForm = new ProfileForm())
            {
                this.Hide();
                profileForm.ShowDialog();
                this.Show();
            }
        }

        /// <summary>
        /// Opens the Job Vacancy Form, hiding the dashboard until the user returns.
        /// </summary>
        private void btnJobVacancies_Click(object sender, EventArgs e)
        {
            using (JobVacancyForm jobForm = new JobVacancyForm())
            {
                this.Hide();
                jobForm.ShowDialog();
                this.Show();
            }
        }

        /// <summary>
        /// Initiates the logout process.
        /// </summary>
        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to log out?",
                                                  "Logout Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                // Reset active session
                UserSession.UserID = 0;
                UserSession.Username = string.Empty;
                UserSession.FullName = string.Empty;
                UserSession.Role = string.Empty;

                isLoggingOut = true; // Mark that we are logging out
                this.Close();        // Trigger FormClosing event
            }
        }

        /// <summary>
        /// Handles the window close event to route back to the Login Portal or exit cleanly [3].
        /// </summary>
        private void ApplicantDashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isLoggingOut)
            {
                Form loginForm = null;

                // Find the hidden login form that is currently running in the background [5]
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
                    loginForm.Show(); // Make the hidden login form visible again
                }
                else
                {
                    // Fallback to instantiate a new login form if none were running
                    LoginForm newLogin = new LoginForm();
                    newLogin.Show();
                }
            }
            else
            {
                // If they closed with the 'X' button, exit the entire app to prevent ghost background processes
                Application.Exit();
            }
        }

        /// <summary>
        /// Programmatic design styling for a modern, flat appearance.
        /// </summary>
        private void ApplyModernStyles()
        {
            this.BackColor = Color.FromArgb(245, 247, 250); // Soft background color

            // Profile Button Card Style
            btnProfile.FlatStyle = FlatStyle.Flat;
            btnProfile.FlatAppearance.BorderSize = 0;
            btnProfile.BackColor = Color.FromArgb(41, 128, 185); // Professional Blue
            btnProfile.ForeColor = Color.White;
            btnProfile.Cursor = Cursors.Hand;
            btnProfile.Font = new Font("Segoe UI", 12F, FontStyle.Bold);

            // Job Vacancies Button Card Style
            btnJobVacancies.FlatStyle = FlatStyle.Flat;
            btnJobVacancies.FlatAppearance.BorderSize = 0;
            btnJobVacancies.BackColor = Color.FromArgb(39, 174, 96); // Professional Green
            btnJobVacancies.ForeColor = Color.White;
            btnJobVacancies.Cursor = Cursors.Hand;
            btnJobVacancies.Font = new Font("Segoe UI", 12F, FontStyle.Bold);

            // Logout Button Style
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.FlatAppearance.BorderSize = 1;
            btnLogout.FlatAppearance.BorderColor = Color.FromArgb(192, 57, 43); // Dark Red Border
            btnLogout.BackColor = Color.White;
            btnLogout.ForeColor = Color.FromArgb(192, 57, 43);
            btnLogout.Cursor = Cursors.Hand;
            btnLogout.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        }
    }
}