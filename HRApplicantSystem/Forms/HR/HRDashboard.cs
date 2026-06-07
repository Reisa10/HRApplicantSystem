using System;
using System.Windows.Forms;
using HRApplicantSystem.Classes;       // Added to reference UserSession
using HRApplicantSystem.Forms.Login;     // Added to reference LoginForm

namespace HRApplicantSystem.Forms.HR
{
    public partial class HRDashboard : Form
    {
        public HRDashboard()
        {
            InitializeComponent();

            // Hide Hiring Decision button entirely for HR Staff
            if (UserSession.Role == "HR Staff")
            {
                HiringDecision.Visible = false;
            }
        }

        private void ApplicantReview_Click(object sender, EventArgs e)
        {
            ApplicantReviewForm frm = new ApplicantReviewForm();
            frm.Show();
            this.Close();
        }

        private void Screening_Click(object sender, EventArgs e)
        {
            ScreeningForm frm = new ScreeningForm();
            frm.Show();
            this.Close();
        }

        private void InterviewSchedule_Click(object sender, EventArgs e)
        {
            InterviewScheduleForm frm = new InterviewScheduleForm();
            frm.Show();
            this.Close();

        }

        private void InterviewEvaluation_Click(object sender, EventArgs e)
        {
            InterviewEvaluationForm frm = new InterviewEvaluationForm();
            frm.Show();
            this.Close();

        }

        private void HiringDecision_Click(object sender, EventArgs e)
        {
            if (UserSession.Role != "HR Manager" && UserSession.Role != "Admin")
            {
                MessageBox.Show("Access Denied. Only HR Manager or Admin can make hiring decisions.",
                    "Unauthorized", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            HiringDecisionForm frm = new HiringDecisionForm();
            frm.Show();
            this.Close();
        }

        private void Reports_Click(object sender, EventArgs e)
        {
            ReportsForm frm = new ReportsForm();
            frm.Show();
            this.Close();
        }

        private void Logout_Click(object sender, EventArgs e)
        {
            // 1. Clear session variables
            UserSession.UserID = 0;
            UserSession.Username = null;
            UserSession.FullName = null;
            UserSession.Role = null;

            // 2. Open the LoginForm
            LoginForm loginForm = new LoginForm();
            loginForm.Show();

            // 3. Close the current dashboard
            this.Close();
        }
    }
}