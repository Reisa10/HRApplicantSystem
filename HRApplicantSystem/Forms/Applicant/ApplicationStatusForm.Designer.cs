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
            this.lblSelectedJob = new System.Windows.Forms.Label();
            this.lblCurrentState = new System.Windows.Forms.Label();
            this.tcStatusDetails = new System.Windows.Forms.TabControl();

            // Tab Pages
            this.tpTimeline = new System.Windows.Forms.TabPage();
            this.tpScreening = new System.Windows.Forms.TabPage();
            this.tpInterview = new System.Windows.Forms.TabPage();
            this.tpEvaluation = new System.Windows.Forms.TabPage();
            this.tpHiring = new System.Windows.Forms.TabPage();

            // tpTimeline Controls
            this.lblTimelineLabel = new System.Windows.Forms.Label();
            this.lstTrackingTimeline = new System.Windows.Forms.ListBox();

            // tpScreening Controls
            this.lblScreeningResult = new System.Windows.Forms.Label();
            this.lblScreeningDate = new System.Windows.Forms.Label();
            this.lblScreeningRemarksLabel = new System.Windows.Forms.Label();
            this.lblScreeningRemarks = new System.Windows.Forms.Label();

            // tpInterview Controls
            this.lblInterviewDate = new System.Windows.Forms.Label();
            this.lblInterviewInterviewer = new System.Windows.Forms.Label();
            this.lblInterviewVenue = new System.Windows.Forms.Label();
            this.lblInterviewMode = new System.Windows.Forms.Label();
            this.lblInterviewStatus = new System.Windows.Forms.Label();

            // tpEvaluation Controls
            this.lblEvalScore = new System.Windows.Forms.Label();
            this.lblEvalResult = new System.Windows.Forms.Label();
            this.lblEvalRecommendation = new System.Windows.Forms.Label();
            this.lblEvalRemarksLabel = new System.Windows.Forms.Label();
            this.lblEvalRemarks = new System.Windows.Forms.Label();

            // tpHiring Controls
            this.lblRemarksLabel = new System.Windows.Forms.Label();
            this.lblRemarksText = new System.Windows.Forms.Label();

            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatusSummary)).BeginInit();
            this.grpTracking.SuspendLayout();
            this.tcStatusDetails.SuspendLayout();

            this.tpTimeline.SuspendLayout();
            this.tpScreening.SuspendLayout();
            this.tpInterview.SuspendLayout();
            this.tpEvaluation.SuspendLayout();
            this.tpHiring.SuspendLayout();
            this.SuspendLayout();

            // pnlHeader
            this.pnlHeader.Controls.Add(this.btnBack);
            this.pnlHeader.Controls.Add(this.lblHeader);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Size = new System.Drawing.Size(960, 60);

            // btnBack
            this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBack.Location = new System.Drawing.Point(855, 15);
            this.btnBack.Size = new System.Drawing.Size(85, 30);
            this.btnBack.Text = "Back";
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);

            // lblHeader
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblHeader.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblHeader.Location = new System.Drawing.Point(15, 15);
            this.lblHeader.Text = "Applicant Status Tracking";

            // splitContainer
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 60);
            this.splitContainer.Size = new System.Drawing.Size(960, 500);
            this.splitContainer.SplitterDistance = 430;

            // splitContainer.Panel1
            this.splitContainer.Panel1.Controls.Add(this.dgvStatusSummary);
            this.splitContainer.Panel1.Padding = new System.Windows.Forms.Padding(10);

            // splitContainer.Panel2
            this.splitContainer.Panel2.Controls.Add(this.grpTracking);
            this.splitContainer.Panel2.Padding = new System.Windows.Forms.Padding(10);

            // dgvStatusSummary
            this.dgvStatusSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStatusSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStatusSummary.Location = new System.Drawing.Point(10, 10);
            this.dgvStatusSummary.SelectionChanged += new System.EventHandler(this.dgvStatusSummary_SelectionChanged);

            // grpTracking
            this.grpTracking.Controls.Add(this.tcStatusDetails);
            this.grpTracking.Controls.Add(this.lblCurrentState);
            this.grpTracking.Controls.Add(this.lblSelectedJob);
            this.grpTracking.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpTracking.Location = new System.Drawing.Point(10, 10);
            this.grpTracking.Size = new System.Drawing.Size(500, 480);
            this.grpTracking.Text = "Application Status Center";

            // lblSelectedJob
            this.lblSelectedJob.AutoSize = true;
            this.lblSelectedJob.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblSelectedJob.Location = new System.Drawing.Point(15, 25);
            this.lblSelectedJob.Text = "Select an Application";

            // lblCurrentState
            this.lblCurrentState.AutoSize = true;
            this.lblCurrentState.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblCurrentState.ForeColor = System.Drawing.Color.FromArgb(230, 126, 34);
            this.lblCurrentState.Location = new System.Drawing.Point(15, 50);
            this.lblCurrentState.Text = "Current Status: --";

            // tcStatusDetails
            this.tcStatusDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcStatusDetails.Controls.Add(this.tpTimeline);
            this.tcStatusDetails.Controls.Add(this.tpScreening);
            this.tcStatusDetails.Controls.Add(this.tpInterview);
            this.tcStatusDetails.Controls.Add(this.tpEvaluation);
            this.tcStatusDetails.Controls.Add(this.tpHiring);
            this.tcStatusDetails.Location = new System.Drawing.Point(15, 80);
            this.tcStatusDetails.Size = new System.Drawing.Size(470, 385);
            this.tcStatusDetails.Font = new System.Drawing.Font("Segoe UI", 9F);

            // ==================== tpTimeline ====================
            this.tpTimeline.Controls.Add(this.lblTimelineLabel);
            this.tpTimeline.Controls.Add(this.lstTrackingTimeline);
            this.tpTimeline.Location = new System.Drawing.Point(4, 24);
            this.tpTimeline.Padding = new System.Windows.Forms.Padding(10);
            this.tpTimeline.Text = "Timeline";

            this.lblTimelineLabel.AutoSize = true;
            this.lblTimelineLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTimelineLabel.Location = new System.Drawing.Point(10, 10);
            this.lblTimelineLabel.Text = "Status Progression:";

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
            this.tpHiring.Controls.Add(this.lblRemarksLabel);
            this.tpHiring.Controls.Add(this.lblRemarksText);
            this.tpHiring.Padding = new System.Windows.Forms.Padding(15);
            this.tpHiring.Text = "4. Final Decision";

            this.lblRemarksLabel.AutoSize = true;
            this.lblRemarksLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblRemarksLabel.Location = new System.Drawing.Point(15, 15);
            this.lblRemarksLabel.Text = "Decision Remarks:";

            this.lblRemarksText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRemarksText.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblRemarksText.Location = new System.Drawing.Point(15, 45);
            this.lblRemarksText.Size = new System.Drawing.Size(430, 290);
            this.lblRemarksText.Text = "Your application is active. Final decision has not been declared yet.";

            // ApplicationStatusForm Layout
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 560);
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
            this.tcStatusDetails.ResumeLayout(false);

            this.tpTimeline.ResumeLayout(false);
            this.tpTimeline.PerformLayout();
            this.tpScreening.ResumeLayout(false);
            this.tpScreening.PerformLayout();
            this.tpInterview.ResumeLayout(false);
            this.tpInterview.PerformLayout();
            this.tpEvaluation.ResumeLayout(false);
            this.tpEvaluation.PerformLayout();
            this.tpHiring.ResumeLayout(false);
            this.tpHiring.PerformLayout();

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

        // Tab Control
        private System.Windows.Forms.TabControl tcStatusDetails;
        private System.Windows.Forms.TabPage tpTimeline;
        private System.Windows.Forms.TabPage tpScreening;
        private System.Windows.Forms.TabPage tpInterview;
        private System.Windows.Forms.TabPage tpEvaluation;
        private System.Windows.Forms.TabPage tpHiring;

        // tpTimeline
        private System.Windows.Forms.Label lblTimelineLabel;
        private System.Windows.Forms.ListBox lstTrackingTimeline;

        // tpScreening
        private System.Windows.Forms.Label lblScreeningResult;
        private System.Windows.Forms.Label lblScreeningDate;
        private System.Windows.Forms.Label lblScreeningRemarksLabel;
        private System.Windows.Forms.Label lblScreeningRemarks;

        // tpInterview
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
    }
}