namespace HRApplicantSystem.Forms.HR
{
    partial class HRDashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ApplicantReview = new System.Windows.Forms.Button();
            this.Screening = new System.Windows.Forms.Button();
            this.InterviewSchedule = new System.Windows.Forms.Button();
            this.Reports = new System.Windows.Forms.Button();
            this.Logout = new System.Windows.Forms.Button();
            this.InterviewEvaluation = new System.Windows.Forms.Button();
            this.AssessmentEvaluation = new System.Windows.Forms.Button();
            this.HiringDecision = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ApplicantReview
            // 
            this.ApplicantReview.Location = new System.Drawing.Point(12, 39);
            this.ApplicantReview.Name = "ApplicantReview";
            this.ApplicantReview.Size = new System.Drawing.Size(128, 42);
            this.ApplicantReview.TabIndex = 0;
            this.ApplicantReview.Text = "Applicant Review";
            this.ApplicantReview.UseVisualStyleBackColor = true;
            this.ApplicantReview.Click += new System.EventHandler(this.ApplicantReview_Click);
            // 
            // Screening
            // 
            this.Screening.Location = new System.Drawing.Point(12, 88);
            this.Screening.Name = "Screening";
            this.Screening.Size = new System.Drawing.Size(128, 47);
            this.Screening.TabIndex = 1;
            this.Screening.Text = "Screening";
            this.Screening.UseVisualStyleBackColor = true;
            this.Screening.Click += new System.EventHandler(this.Screening_Click);
            // 
            // InterviewSchedule
            // 
            this.InterviewSchedule.Location = new System.Drawing.Point(12, 135);
            this.InterviewSchedule.Name = "InterviewSchedule";
            this.InterviewSchedule.Size = new System.Drawing.Size(128, 42);
            this.InterviewSchedule.TabIndex = 2;
            this.InterviewSchedule.Text = "Interview Schedule";
            this.InterviewSchedule.UseVisualStyleBackColor = true;
            this.InterviewSchedule.Click += new System.EventHandler(this.InterviewSchedule_Click);
            // 
            // Reports
            // 
            this.Reports.Location = new System.Drawing.Point(12, 337);
            this.Reports.Name = "Reports";
            this.Reports.Size = new System.Drawing.Size(128, 42);
            this.Reports.TabIndex = 3;
            this.Reports.Text = "Reports";
            this.Reports.UseVisualStyleBackColor = true;
            this.Reports.Click += new System.EventHandler(this.Reports_Click);
            // 
            // Logout
            // 
            this.Logout.Location = new System.Drawing.Point(12, 384);
            this.Logout.Name = "Logout";
            this.Logout.Size = new System.Drawing.Size(128, 42);
            this.Logout.TabIndex = 4;
            this.Logout.Text = "Logout";
            this.Logout.UseVisualStyleBackColor = true;
            this.Logout.Click += new System.EventHandler(this.Logout_Click);
            // 
            // InterviewEvaluation
            // 
            this.InterviewEvaluation.Location = new System.Drawing.Point(13, 184);
            this.InterviewEvaluation.Name = "InterviewEvaluation";
            this.InterviewEvaluation.Size = new System.Drawing.Size(127, 44);
            this.InterviewEvaluation.TabIndex = 5;
            this.InterviewEvaluation.Text = "Interview Evaluation";
            this.InterviewEvaluation.UseVisualStyleBackColor = true;
            this.InterviewEvaluation.Click += new System.EventHandler(this.InterviewEvaluation_Click);
            // 
            // AssessmentEvaluation
            // 
            this.AssessmentEvaluation.Location = new System.Drawing.Point(12, 234);
            this.AssessmentEvaluation.Name = "AssessmentEvaluation";
            this.AssessmentEvaluation.Size = new System.Drawing.Size(128, 44);
            this.AssessmentEvaluation.TabIndex = 7;
            this.AssessmentEvaluation.Text = "Assessment Evaluation";
            this.AssessmentEvaluation.UseVisualStyleBackColor = true;
            this.AssessmentEvaluation.Click += new System.EventHandler(this.AssessmentEvaluation_Click);
            // 
            // HiringDecision
            // 
            this.HiringDecision.Location = new System.Drawing.Point(12, 285);
            this.HiringDecision.Name = "HiringDecision";
            this.HiringDecision.Size = new System.Drawing.Size(128, 45);
            this.HiringDecision.TabIndex = 6;
            this.HiringDecision.Text = "Hiring Decision";
            this.HiringDecision.UseVisualStyleBackColor = true;
            this.HiringDecision.Click += new System.EventHandler(this.HiringDecision_Click);
            // 
            // HRDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Controls.Add(this.HiringDecision);
            this.Controls.Add(this.AssessmentEvaluation);
            this.Controls.Add(this.InterviewEvaluation);
            this.Controls.Add(this.Logout);
            this.Controls.Add(this.Reports);
            this.Controls.Add(this.InterviewSchedule);
            this.Controls.Add(this.Screening);
            this.Controls.Add(this.ApplicantReview);
            this.Name = "HRDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HRDashboard";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ApplicantReview;
        private System.Windows.Forms.Button Screening;
        private System.Windows.Forms.Button InterviewSchedule;
        private System.Windows.Forms.Button Reports;
        private System.Windows.Forms.Button Logout;
        private System.Windows.Forms.Button InterviewEvaluation;
        private System.Windows.Forms.Button AssessmentEvaluation;
        private System.Windows.Forms.Button HiringDecision;
    }
}