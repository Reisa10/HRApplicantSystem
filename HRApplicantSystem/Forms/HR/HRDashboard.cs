using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using HRApplicantSystem.Forms.HR;

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

            MessageBox.Show("Coming in Day 4");
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
            MessageBox.Show("Coming in Day 4");
        }

        
    }
}
