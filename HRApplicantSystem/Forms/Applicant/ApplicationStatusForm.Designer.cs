namespace HRApplicantSystem.Forms.Applicant
{
    partial class ApplicationStatusForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnBack = new System.Windows.Forms.Button();
            this.lblHeader = new System.Windows.Forms.Label();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.dgvStatusSummary = new System.Windows.Forms.DataGridView();
            this.grpTracking = new System.Windows.Forms.GroupBox();
            this.grpRemarks = new System.Windows.Forms.GroupBox();
            this.lblRemarksText = new System.Windows.Forms.Label();
            this.grpInterview = new System.Windows.Forms.GroupBox();
            this.lblInterviewDetails = new System.Windows.Forms.Label();
            this.lblInterviewTime = new System.Windows.Forms.Label();
            this.lblInterviewDate = new System.Windows.Forms.Label();
            this.lblTimelineLabel = new System.Windows.Forms.Label();
            this.lstTrackingTimeline = new System.Windows.Forms.ListBox();
            this.lblCurrentState = new System.Windows.Forms.Label();
            this.lblSelectedJob = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatusSummary)).BeginInit();
            this.grpTracking.SuspendLayout();
            this.grpRemarks.SuspendLayout();
            this.grpInterview.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.btnBack);
            this.pnlHeader.Controls.Add(this.lblHeader);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(940, 60);
            this.pnlHeader.TabIndex = 0;
            // 
            // btnBack
            // 
            this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBack.Location = new System.Drawing.Point(835, 15);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(85, 30);
            this.btnBack.TabIndex = 1;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblHeader.Location = new System.Drawing.Point(15, 15);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(288, 30);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "Applicant Status Tracking";
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 60);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.dgvStatusSummary);
            this.splitContainer.Panel1.Padding = new System.Windows.Forms.Padding(10);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.grpTracking);
            this.splitContainer.Panel2.Padding = new System.Windows.Forms.Padding(10);
            this.splitContainer.Size = new System.Drawing.Size(940, 500);
            this.splitContainer.SplitterDistance = 450;
            this.splitContainer.TabIndex = 1;
            // 
            // dgvStatusSummary
            // 
            this.dgvStatusSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStatusSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStatusSummary.Location = new System.Drawing.Point(10, 10);
            this.dgvStatusSummary.Name = "dgvStatusSummary";
            this.dgvStatusSummary.Size = new System.Drawing.Size(430, 480);
            this.dgvStatusSummary.TabIndex = 0;
            this.dgvStatusSummary.SelectionChanged += new System.EventHandler(this.dgvStatusSummary_SelectionChanged);
            // 
            // grpTracking
            // 
            this.grpTracking.Controls.Add(this.grpRemarks);
            this.grpTracking.Controls.Add(this.grpInterview);
            this.grpTracking.Controls.Add(this.lblTimelineLabel);
            this.grpTracking.Controls.Add(this.lstTrackingTimeline);
            this.grpTracking.Controls.Add(this.lblCurrentState);
            this.grpTracking.Controls.Add(this.lblSelectedJob);
            this.grpTracking.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpTracking.Location = new System.Drawing.Point(10, 10);
            this.grpTracking.Name = "grpTracking";
            this.grpTracking.Size = new System.Drawing.Size(466, 480);
            this.grpTracking.TabIndex = 0;
            this.grpTracking.TabStop = false;
            this.grpTracking.Text = "Progress Timeline Details";
            // 
            // grpRemarks
            // 
            this.grpRemarks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpRemarks.Controls.Add(this.lblRemarksText);
            this.grpRemarks.Location = new System.Drawing.Point(20, 375);
            this.grpRemarks.Name = "grpRemarks";
            this.grpRemarks.Size = new System.Drawing.Size(426, 85);
            this.grpRemarks.TabIndex = 5;
            this.grpRemarks.TabStop = false;
            this.grpRemarks.Text = "Hiring Remarks";
            // 
            // lblRemarksText
            // 
            this.lblRemarksText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRemarksText.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblRemarksText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.lblRemarksText.Location = new System.Drawing.Point(3, 16);
            this.lblRemarksText.Name = "lblRemarksText";
            this.lblRemarksText.Size = new System.Drawing.Size(420, 66);
            this.lblRemarksText.TabIndex = 0;
            this.lblRemarksText.Text = "No evaluation details processed yet.";
            // 
            // grpInterview
            // 
            this.grpInterview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpInterview.Controls.Add(this.lblInterviewDetails);
            this.grpInterview.Controls.Add(this.lblInterviewTime);
            this.grpInterview.Controls.Add(this.lblInterviewDate);
            this.grpInterview.Location = new System.Drawing.Point(20, 260);
            this.grpInterview.Name = "grpInterview";
            this.grpInterview.Size = new System.Drawing.Size(426, 100);
            this.grpInterview.TabIndex = 4;
            this.grpInterview.TabStop = false;
            this.grpInterview.Text = "Interview Schedule";
            // 
            // lblInterviewDetails
            // 
            this.lblInterviewDetails.AutoSize = true;
            this.lblInterviewDetails.Location = new System.Drawing.Point(15, 70);
            this.lblInterviewDetails.Name = "lblInterviewDetails";
            this.lblInterviewDetails.Size = new System.Drawing.Size(124, 13);
            this.lblInterviewDetails.TabIndex = 2;
            this.lblInterviewDetails.Text = "Interviewer/Location: --";
            // 
            // lblInterviewTime
            // 
            this.lblInterviewTime.AutoSize = true;
            this.lblInterviewTime.Location = new System.Drawing.Point(15, 45);
            this.lblInterviewTime.Name = "lblInterviewTime";
            this.lblInterviewTime.Size = new System.Drawing.Size(49, 13);
            this.lblInterviewTime.TabIndex = 1;
            this.lblInterviewTime.Text = "Time: --";
            // 
            // lblInterviewDate
            // 
            this.lblInterviewDate.AutoSize = true;
            this.lblInterviewDate.Location = new System.Drawing.Point(15, 20);
            this.lblInterviewDate.Name = "lblInterviewDate";
            this.lblInterviewDate.Size = new System.Drawing.Size(49, 13);
            this.lblInterviewDate.TabIndex = 0;
            this.lblInterviewDate.Text = "Date: --";
            // 
            // lblTimelineLabel
            // 
            this.lblTimelineLabel.AutoSize = true;
            this.lblTimelineLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTimelineLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblTimelineLabel.Location = new System.Drawing.Point(20, 110);
            this.lblTimelineLabel.Name = "lblTimelineLabel";
            this.lblTimelineLabel.Size = new System.Drawing.Size(164, 15);
            this.lblTimelineLabel.TabIndex = 3;
            this.lblTimelineLabel.Text = "Application Status Timeline:";
            // 
            // lstTrackingTimeline
            // 
            this.lstTrackingTimeline.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstTrackingTimeline.Location = new System.Drawing.Point(10, 30);
            this.lstTrackingTimeline.Size = new System.Drawing.Size(440, 310);

            // ==================== tpScreening ====================
            this.tpScreening.Controls.Add(this.lblScreeningResult);
            this.tpScreening.Controls.Add(this.lblScreeningDate);
            this.tpScreening.Controls.Add(this.lblScreeningRemarksLabel);
            this.tpScreening.Controls.Add(this.lblScreeningRemarks);
            this.tpScreening.Padding = new System.Windows.Forms.Padding(15);
            this.tpScreening.Text = "1. Screening";

            this.lblScreeningResult.AutoSize = true;
            this.lblScreeningResult.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblScreeningResult.Location = new System.Drawing.Point(15, 15);
            this.lblScreeningResult.Text = "Screening Result: Pending";

            this.lblScreeningDate.AutoSize = true;
            this.lblScreeningDate.Location = new System.Drawing.Point(15, 45);
            this.lblScreeningDate.Text = "Date Screened: --";

            this.lblScreeningRemarksLabel.AutoSize = true;
            this.lblScreeningRemarksLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblScreeningRemarksLabel.Location = new System.Drawing.Point(15, 80);
            this.lblScreeningRemarksLabel.Text = "HR Remarks:";

            this.lblScreeningRemarks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblScreeningRemarks.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblScreeningRemarks.Location = new System.Drawing.Point(15, 105);
            this.lblScreeningRemarks.Size = new System.Drawing.Size(430, 230);
            this.lblScreeningRemarks.Text = "No screening remarks available yet.";

            // ==================== tpInterview ====================
            this.tpInterview.Controls.Add(this.lblInterviewDate);
            this.tpInterview.Controls.Add(this.lblInterviewInterviewer);
            this.tpInterview.Controls.Add(this.lblInterviewVenue);
            this.tpInterview.Controls.Add(this.lblInterviewMode);
            this.tpInterview.Controls.Add(this.lblInterviewStatus);
            this.tpInterview.Padding = new System.Windows.Forms.Padding(15);
            this.tpInterview.Text = "2. Interview";

            this.lblInterviewDate.AutoSize = true;
            this.lblInterviewDate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblInterviewDate.Location = new System.Drawing.Point(15, 15);
            this.lblInterviewDate.Text = "Date & Time: Not scheduled yet";

            this.lblInterviewInterviewer.AutoSize = true;
            this.lblInterviewInterviewer.Location = new System.Drawing.Point(15, 50);
            this.lblInterviewInterviewer.Text = "Interviewer: --";

            this.lblInterviewVenue.AutoSize = true;
            this.lblInterviewVenue.Location = new System.Drawing.Point(15, 80);
            this.lblInterviewVenue.Text = "Venue/Location: --";

            this.lblInterviewMode.AutoSize = true;
            this.lblInterviewMode.Location = new System.Drawing.Point(15, 110);
            this.lblInterviewMode.Text = "Mode: --";

            this.lblInterviewStatus.AutoSize = true;
            this.lblInterviewStatus.Location = new System.Drawing.Point(15, 140);
            this.lblInterviewStatus.Text = "Schedule Status: --";

            // ==================== tpEvaluation ====================
            this.tpEvaluation.Controls.Add(this.lblEvalScore);
            this.tpEvaluation.Controls.Add(this.lblEvalResult);
            this.tpEvaluation.Controls.Add(this.lblEvalRecommendation);
            this.tpEvaluation.Controls.Add(this.lblEvalRemarksLabel);
            this.tpEvaluation.Controls.Add(this.lblEvalRemarks);
            this.tpEvaluation.Padding = new System.Windows.Forms.Padding(15);
            this.tpEvaluation.Text = "3. Evaluation";

            this.lblEvalScore.AutoSize = true;
            this.lblEvalScore.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblEvalScore.Location = new System.Drawing.Point(15, 15);
            this.lblEvalScore.Text = "Score: --";

            this.lblEvalResult.AutoSize = true;
            this.lblEvalResult.Location = new System.Drawing.Point(15, 45);
            this.lblEvalResult.Text = "Result: --";

            this.lblEvalRecommendation.AutoSize = true;
            this.lblEvalRecommendation.Location = new System.Drawing.Point(15, 75);
            this.lblEvalRecommendation.Text = "Recommendation: --";

            this.lblEvalRemarksLabel.AutoSize = true;
            this.lblEvalRemarksLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblEvalRemarksLabel.Location = new System.Drawing.Point(15, 110);
            this.lblEvalRemarksLabel.Text = "Evaluator Remarks:";

            this.lblEvalRemarks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEvalRemarks.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblEvalRemarks.Location = new System.Drawing.Point(15, 135);
            this.lblEvalRemarks.Size = new System.Drawing.Size(430, 200);
            this.lblEvalRemarks.Text = "No interview evaluation remarks available yet.";

            // ==================== tpHiring ====================
            // lblDecisionResult
            this.lblDecisionResult = new System.Windows.Forms.Label();
            this.lblDecisionResult.AutoSize = true;
            this.lblDecisionResult.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDecisionResult.Location = new System.Drawing.Point(15, 15);
            this.lblDecisionResult.Name = "lblDecisionResult";
            this.lblDecisionResult.Size = new System.Drawing.Size(110, 21);
            this.lblDecisionResult.TabIndex = 0;
            this.lblDecisionResult.Text = "Decision: Pending";

            // lblDecisionDate
            this.lblDecisionDate = new System.Windows.Forms.Label();
            this.lblDecisionDate.AutoSize = true;
            this.lblDecisionDate.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDecisionDate.Location = new System.Drawing.Point(15, 45);
            this.lblDecisionDate.Name = "lblDecisionDate";
            this.lblDecisionDate.Size = new System.Drawing.Size(100, 17);
            this.lblDecisionDate.TabIndex = 1;
            this.lblDecisionDate.Text = "Decision Date: --";

            this.tpHiring.Controls.Add(this.lblDecisionResult);
            this.tpHiring.Controls.Add(this.lblDecisionDate);
            this.tpHiring.Controls.Add(this.lblRemarksLabel);
            this.tpHiring.Controls.Add(this.lblRemarksText);
            this.tpHiring.Padding = new System.Windows.Forms.Padding(15);
            this.tpHiring.Text = "4. Final Decision";

            this.lblRemarksLabel.AutoSize = true;
            this.lblRemarksLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblRemarksLabel.Location = new System.Drawing.Point(15, 80);
            this.lblRemarksLabel.Text = "Decision Remarks:";

            this.lblRemarksText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRemarksText.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblRemarksText.Location = new System.Drawing.Point(15, 105);
            this.lblRemarksText.Size = new System.Drawing.Size(430, 210);
            this.lblRemarksText.Text = "Your application is active. Final decision has not been declared yet.";

            // ApplicationStatusForm Layout
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(940, 560);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.pnlHeader);
            this.Name = "ApplicationStatusForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "My Applications Status History";
            this.Load += new System.EventHandler(this.ApplicationStatusForm_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatusSummary)).EndInit();
            this.grpTracking.ResumeLayout(false);
            this.grpTracking.PerformLayout();
            this.grpRemarks.ResumeLayout(false);
            this.grpInterview.ResumeLayout(false);
            this.grpInterview.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.DataGridView dgvStatusSummary;
        private System.Windows.Forms.GroupBox grpTracking;
        private System.Windows.Forms.Label lblSelectedJob;
        private System.Windows.Forms.Label lblCurrentState;
        private System.Windows.Forms.Label lblTimelineLabel;
        private System.Windows.Forms.ListBox lstTrackingTimeline;
        private System.Windows.Forms.GroupBox grpRemarks;
        private System.Windows.Forms.Label lblRemarksText;
        private System.Windows.Forms.GroupBox grpInterview;
        private System.Windows.Forms.Label lblInterviewDetails;
        private System.Windows.Forms.Label lblInterviewTime;
        private System.Windows.Forms.Label lblInterviewDate;
        private System.Windows.Forms.Label lblInterviewInterviewer;
        private System.Windows.Forms.Label lblInterviewVenue;
        private System.Windows.Forms.Label lblInterviewMode;
        private System.Windows.Forms.Label lblInterviewStatus;

        // tpEvaluation
        private System.Windows.Forms.Label lblEvalScore;
        private System.Windows.Forms.Label lblEvalResult;
        private System.Windows.Forms.Label lblEvalRecommendation;
        private System.Windows.Forms.Label lblEvalRemarksLabel;
        private System.Windows.Forms.Label lblEvalRemarks;

        // tpHiring
        private System.Windows.Forms.Label lblRemarksLabel;
        private System.Windows.Forms.Label lblRemarksText;
        private System.Windows.Forms.Label lblDecisionResult;
        private System.Windows.Forms.Label lblDecisionDate;
    }
}