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
        }

        private void ApplicantReview_Click(object sender, EventArgs e)
        {
            ApplicantReviewForm frm = new ApplicantReviewForm();
            frm.Show();
        }

        private void Screening_Click(object sender, EventArgs e)
        {
            ScreeningForm frm = new ScreeningForm();
            frm.Show();

            MessageBox.Show("Coming in Day 4");
        }

        private void InterviewSchedule_Click(object sender, EventArgs e)
        {
            InterviewScheduleForm frm = new InterviewScheduleForm();
            frm.Show();

            MessageBox.Show("Coming in Day 4");
        }

        private void InterviewEvaluation_Click(object sender, EventArgs e)
        {
            InterviewEvaluationForm frm = new InterviewEvaluationForm();
            frm.Show();

            MessageBox.Show("Coming in Day 4");
        }

        private void HiringDecision_Click(object sender, EventArgs e)
        {
            HiringDecisionForm frm = new HiringDecisionForm();
            frm.Show();

            MessageBox.Show("Coming in Day 4");
        }

        private void Reports_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Coming in Day 4");
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