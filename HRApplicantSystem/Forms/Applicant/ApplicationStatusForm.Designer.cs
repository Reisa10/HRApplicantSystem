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
            this.tcStatusDetails = new System.Windows.Forms.TabControl();
            this.tpTimeline = new System.Windows.Forms.TabPage();
            this.lblTimelineLabel = new System.Windows.Forms.Label();
            this.lstTrackingTimeline = new System.Windows.Forms.ListBox();
            this.tpScreening = new System.Windows.Forms.TabPage();
            this.lblScreeningResult = new System.Windows.Forms.Label();
            this.lblScreeningDate = new System.Windows.Forms.Label();
            this.lblScreeningRemarksLabel = new System.Windows.Forms.Label();
            this.lblScreeningRemarks = new System.Windows.Forms.Label();
            this.tpInterview = new System.Windows.Forms.TabPage();
            this.lblInterviewDate = new System.Windows.Forms.Label();
            this.lblInterviewInterviewer = new System.Windows.Forms.Label();
            this.lblInterviewVenue = new System.Windows.Forms.Label();
            this.lblInterviewMode = new System.Windows.Forms.Label();
            this.lblInterviewStatus = new System.Windows.Forms.Label();
            this.tpEvaluation = new System.Windows.Forms.TabPage();
            this.lblEvalScore = new System.Windows.Forms.Label();
            this.lblEvalResult = new System.Windows.Forms.Label();
            this.lblEvalRecommendation = new System.Windows.Forms.Label();
            this.lblEvalRemarksLabel = new System.Windows.Forms.Label();
            this.lblEvalRemarks = new System.Windows.Forms.Label();
            this.tpHiring = new System.Windows.Forms.TabPage();
            this.lblRemarksLabel = new System.Windows.Forms.Label();
            this.lblRemarksText = new System.Windows.Forms.Label();
            this.lblCurrentState = new System.Windows.Forms.Label();
            this.lblSelectedJob = new System.Windows.Forms.Label();
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
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.btnBack);
            this.pnlHeader.Controls.Add(this.lblHeader);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1280, 74);
            this.pnlHeader.TabIndex = 1;
            // 
            // btnBack
            // 
            this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBack.Location = new System.Drawing.Point(1140, 18);
            this.btnBack.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(113, 37);
            this.btnBack.TabIndex = 0;
            this.btnBack.Text = "Back";
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblHeader.Location = new System.Drawing.Point(20, 18);
            this.lblHeader.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(343, 37);
            this.lblHeader.TabIndex = 1;
            this.lblHeader.Text = "Applicant Status Tracking";
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 74);
            this.splitContainer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.dgvStatusSummary);
            this.splitContainer.Panel1.Padding = new System.Windows.Forms.Padding(13, 12, 13, 12);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.grpTracking);
            this.splitContainer.Panel2.Padding = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.splitContainer.Size = new System.Drawing.Size(1280, 615);
            this.splitContainer.SplitterDistance = 573;
            this.splitContainer.SplitterWidth = 5;
            this.splitContainer.TabIndex = 0;
            // 
            // dgvStatusSummary
            // 
            this.dgvStatusSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStatusSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStatusSummary.Location = new System.Drawing.Point(13, 12);
            this.dgvStatusSummary.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvStatusSummary.Name = "dgvStatusSummary";
            this.dgvStatusSummary.RowHeadersWidth = 51;
            this.dgvStatusSummary.Size = new System.Drawing.Size(547, 591);
            this.dgvStatusSummary.TabIndex = 0;
            this.dgvStatusSummary.SelectionChanged += new System.EventHandler(this.dgvStatusSummary_SelectionChanged);
            // 
            // grpTracking
            // 
            this.grpTracking.Controls.Add(this.tcStatusDetails);
            this.grpTracking.Controls.Add(this.lblCurrentState);
            this.grpTracking.Controls.Add(this.lblSelectedJob);
            this.grpTracking.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpTracking.Location = new System.Drawing.Point(13, 12);
            this.grpTracking.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpTracking.Name = "grpTracking";
            this.grpTracking.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpTracking.Size = new System.Drawing.Size(676, 591);
            this.grpTracking.TabIndex = 0;
            this.grpTracking.TabStop = false;
            this.grpTracking.Text = "Application Status Center";
            // 
            // tcStatusDetails
            // 
            this.tcStatusDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcStatusDetails.Controls.Add(this.tpTimeline);
            this.tcStatusDetails.Controls.Add(this.tpScreening);
            this.tcStatusDetails.Controls.Add(this.tpInterview);
            this.tcStatusDetails.Controls.Add(this.tpEvaluation);
            this.tcStatusDetails.Controls.Add(this.tpHiring);
            this.tcStatusDetails.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tcStatusDetails.Location = new System.Drawing.Point(20, 98);
            this.tcStatusDetails.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tcStatusDetails.Name = "tcStatusDetails";
            this.tcStatusDetails.SelectedIndex = 0;
            this.tcStatusDetails.Size = new System.Drawing.Size(636, 474);
            this.tcStatusDetails.TabIndex = 0;
            // 
            // tpTimeline
            // 
            this.tpTimeline.Controls.Add(this.lblTimelineLabel);
            this.tpTimeline.Controls.Add(this.lstTrackingTimeline);
            this.tpTimeline.Location = new System.Drawing.Point(4, 29);
            this.tpTimeline.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpTimeline.Name = "tpTimeline";
            this.tpTimeline.Padding = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.tpTimeline.Size = new System.Drawing.Size(628, 441);
            this.tpTimeline.TabIndex = 0;
            this.tpTimeline.Text = "Timeline";
            // 
            // lblTimelineLabel
            // 
            this.lblTimelineLabel.AutoSize = true;
            this.lblTimelineLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTimelineLabel.Location = new System.Drawing.Point(13, 12);
            this.lblTimelineLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTimelineLabel.Name = "lblTimelineLabel";
            this.lblTimelineLabel.Size = new System.Drawing.Size(144, 20);
            this.lblTimelineLabel.TabIndex = 0;
            this.lblTimelineLabel.Text = "Status Progression:";
            // 
            // lstTrackingTimeline
            // 
            this.lstTrackingTimeline.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstTrackingTimeline.ItemHeight = 20;
            this.lstTrackingTimeline.Location = new System.Drawing.Point(13, 37);
            this.lstTrackingTimeline.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lstTrackingTimeline.Name = "lstTrackingTimeline";
            this.lstTrackingTimeline.Size = new System.Drawing.Size(594, 364);
            this.lstTrackingTimeline.TabIndex = 1;
            // 
            // tpScreening
            // 
            this.tpScreening.Controls.Add(this.lblScreeningResult);
            this.tpScreening.Controls.Add(this.lblScreeningDate);
            this.tpScreening.Controls.Add(this.lblScreeningRemarksLabel);
            this.tpScreening.Controls.Add(this.lblScreeningRemarks);
            this.tpScreening.Location = new System.Drawing.Point(4, 29);
            this.tpScreening.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpScreening.Name = "tpScreening";
            this.tpScreening.Padding = new System.Windows.Forms.Padding(20, 18, 20, 18);
            this.tpScreening.Size = new System.Drawing.Size(619, 441);
            this.tpScreening.TabIndex = 1;
            this.tpScreening.Text = "1. Screening";
            // 
            // lblScreeningResult
            // 
            this.lblScreeningResult.AutoSize = true;
            this.lblScreeningResult.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblScreeningResult.Location = new System.Drawing.Point(20, 18);
            this.lblScreeningResult.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblScreeningResult.Name = "lblScreeningResult";
            this.lblScreeningResult.Size = new System.Drawing.Size(218, 23);
            this.lblScreeningResult.TabIndex = 0;
            this.lblScreeningResult.Text = "Screening Result: Pending";
            // 
            // lblScreeningDate
            // 
            this.lblScreeningDate.AutoSize = true;
            this.lblScreeningDate.Location = new System.Drawing.Point(20, 55);
            this.lblScreeningDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblScreeningDate.Name = "lblScreeningDate";
            this.lblScreeningDate.Size = new System.Drawing.Size(125, 20);
            this.lblScreeningDate.TabIndex = 1;
            this.lblScreeningDate.Text = "Date Screened: --";
            // 
            // lblScreeningRemarksLabel
            // 
            this.lblScreeningRemarksLabel.AutoSize = true;
            this.lblScreeningRemarksLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblScreeningRemarksLabel.Location = new System.Drawing.Point(20, 98);
            this.lblScreeningRemarksLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblScreeningRemarksLabel.Name = "lblScreeningRemarksLabel";
            this.lblScreeningRemarksLabel.Size = new System.Drawing.Size(99, 20);
            this.lblScreeningRemarksLabel.TabIndex = 2;
            this.lblScreeningRemarksLabel.Text = "HR Remarks:";
            // 
            // lblScreeningRemarks
            // 
            this.lblScreeningRemarks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblScreeningRemarks.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblScreeningRemarks.Location = new System.Drawing.Point(20, 129);
            this.lblScreeningRemarks.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblScreeningRemarks.Name = "lblScreeningRemarks";
            this.lblScreeningRemarks.Size = new System.Drawing.Size(573, 283);
            this.lblScreeningRemarks.TabIndex = 3;
            this.lblScreeningRemarks.Text = "No screening remarks available yet.";
            // 
            // tpInterview
            // 
            this.tpInterview.Controls.Add(this.lblInterviewDate);
            this.tpInterview.Controls.Add(this.lblInterviewInterviewer);
            this.tpInterview.Controls.Add(this.lblInterviewVenue);
            this.tpInterview.Controls.Add(this.lblInterviewMode);
            this.tpInterview.Controls.Add(this.lblInterviewStatus);
            this.tpInterview.Location = new System.Drawing.Point(4, 29);
            this.tpInterview.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpInterview.Name = "tpInterview";
            this.tpInterview.Padding = new System.Windows.Forms.Padding(20, 18, 20, 18);
            this.tpInterview.Size = new System.Drawing.Size(628, 441);
            this.tpInterview.TabIndex = 2;
            this.tpInterview.Text = "2. Interview";
            // 
            // lblInterviewDate
            // 
            this.lblInterviewDate.AutoSize = true;
            this.lblInterviewDate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblInterviewDate.Location = new System.Drawing.Point(20, 18);
            this.lblInterviewDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInterviewDate.Name = "lblInterviewDate";
            this.lblInterviewDate.Size = new System.Drawing.Size(253, 23);
            this.lblInterviewDate.TabIndex = 0;
            this.lblInterviewDate.Text = "Date & Time: Not scheduled yet";
            // 
            // lblInterviewInterviewer
            // 
            this.lblInterviewInterviewer.AutoSize = true;
            this.lblInterviewInterviewer.Location = new System.Drawing.Point(20, 62);
            this.lblInterviewInterviewer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInterviewInterviewer.Name = "lblInterviewInterviewer";
            this.lblInterviewInterviewer.Size = new System.Drawing.Size(101, 20);
            this.lblInterviewInterviewer.TabIndex = 1;
            this.lblInterviewInterviewer.Text = "Interviewer: --";
            // 
            // lblInterviewVenue
            // 
            this.lblInterviewVenue.AutoSize = true;
            this.lblInterviewVenue.Location = new System.Drawing.Point(20, 98);
            this.lblInterviewVenue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInterviewVenue.Name = "lblInterviewVenue";
            this.lblInterviewVenue.Size = new System.Drawing.Size(131, 20);
            this.lblInterviewVenue.TabIndex = 2;
            this.lblInterviewVenue.Text = "Venue/Location: --";
            // 
            // lblInterviewMode
            // 
            this.lblInterviewMode.AutoSize = true;
            this.lblInterviewMode.Location = new System.Drawing.Point(20, 135);
            this.lblInterviewMode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInterviewMode.Name = "lblInterviewMode";
            this.lblInterviewMode.Size = new System.Drawing.Size(67, 20);
            this.lblInterviewMode.TabIndex = 3;
            this.lblInterviewMode.Text = "Mode: --";
            // 
            // lblInterviewStatus
            // 
            this.lblInterviewStatus.AutoSize = true;
            this.lblInterviewStatus.Location = new System.Drawing.Point(20, 172);
            this.lblInterviewStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInterviewStatus.Name = "lblInterviewStatus";
            this.lblInterviewStatus.Size = new System.Drawing.Size(132, 20);
            this.lblInterviewStatus.TabIndex = 4;
            this.lblInterviewStatus.Text = "Schedule Status: --";
            // 
            // tpEvaluation
            // 
            this.tpEvaluation.Controls.Add(this.lblEvalScore);
            this.tpEvaluation.Controls.Add(this.lblEvalResult);
            this.tpEvaluation.Controls.Add(this.lblEvalRecommendation);
            this.tpEvaluation.Controls.Add(this.lblEvalRemarksLabel);
            this.tpEvaluation.Controls.Add(this.lblEvalRemarks);
            this.tpEvaluation.Location = new System.Drawing.Point(4, 29);
            this.tpEvaluation.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpEvaluation.Name = "tpEvaluation";
            this.tpEvaluation.Padding = new System.Windows.Forms.Padding(20, 18, 20, 18);
            this.tpEvaluation.Size = new System.Drawing.Size(628, 441);
            this.tpEvaluation.TabIndex = 3;
            this.tpEvaluation.Text = "3. Evaluation";
            // 
            // lblEvalScore
            // 
            this.lblEvalScore.AutoSize = true;
            this.lblEvalScore.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblEvalScore.Location = new System.Drawing.Point(20, 18);
            this.lblEvalScore.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEvalScore.Name = "lblEvalScore";
            this.lblEvalScore.Size = new System.Drawing.Size(78, 23);
            this.lblEvalScore.TabIndex = 0;
            this.lblEvalScore.Text = "Score: --";
            // 
            // lblEvalResult
            // 
            this.lblEvalResult.AutoSize = true;
            this.lblEvalResult.Location = new System.Drawing.Point(20, 55);
            this.lblEvalResult.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEvalResult.Name = "lblEvalResult";
            this.lblEvalResult.Size = new System.Drawing.Size(68, 20);
            this.lblEvalResult.TabIndex = 1;
            this.lblEvalResult.Text = "Result: --";
            // 
            // lblEvalRecommendation
            // 
            this.lblEvalRecommendation.AutoSize = true;
            this.lblEvalRecommendation.Location = new System.Drawing.Point(20, 92);
            this.lblEvalRecommendation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEvalRecommendation.Name = "lblEvalRecommendation";
            this.lblEvalRecommendation.Size = new System.Drawing.Size(146, 20);
            this.lblEvalRecommendation.TabIndex = 2;
            this.lblEvalRecommendation.Text = "Recommendation: --";
            // 
            // lblEvalRemarksLabel
            // 
            this.lblEvalRemarksLabel.AutoSize = true;
            this.lblEvalRemarksLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblEvalRemarksLabel.Location = new System.Drawing.Point(20, 135);
            this.lblEvalRemarksLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEvalRemarksLabel.Name = "lblEvalRemarksLabel";
            this.lblEvalRemarksLabel.Size = new System.Drawing.Size(144, 20);
            this.lblEvalRemarksLabel.TabIndex = 3;
            this.lblEvalRemarksLabel.Text = "Evaluator Remarks:";
            // 
            // lblEvalRemarks
            // 
            this.lblEvalRemarks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEvalRemarks.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblEvalRemarks.Location = new System.Drawing.Point(20, 166);
            this.lblEvalRemarks.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEvalRemarks.Name = "lblEvalRemarks";
            this.lblEvalRemarks.Size = new System.Drawing.Size(582, 246);
            this.lblEvalRemarks.TabIndex = 4;
            this.lblEvalRemarks.Text = "No interview evaluation remarks available yet.";
            // 
            // tpHiring
            // 
            this.tpHiring.Controls.Add(this.lblRemarksLabel);
            this.tpHiring.Controls.Add(this.lblRemarksText);
            this.tpHiring.Location = new System.Drawing.Point(4, 29);
            this.tpHiring.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpHiring.Name = "tpHiring";
            this.tpHiring.Padding = new System.Windows.Forms.Padding(20, 18, 20, 18);
            this.tpHiring.Size = new System.Drawing.Size(619, 441);
            this.tpHiring.TabIndex = 4;
            this.tpHiring.Text = "4. Final Decision";
            // 
            // lblRemarksLabel
            // 
            this.lblRemarksLabel.AutoSize = true;
            this.lblRemarksLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblRemarksLabel.Location = new System.Drawing.Point(20, 18);
            this.lblRemarksLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRemarksLabel.Name = "lblRemarksLabel";
            this.lblRemarksLabel.Size = new System.Drawing.Size(156, 23);
            this.lblRemarksLabel.TabIndex = 0;
            this.lblRemarksLabel.Text = "Decision Remarks:";
            // 
            // lblRemarksText
            // 
            this.lblRemarksText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRemarksText.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblRemarksText.Location = new System.Drawing.Point(20, 55);
            this.lblRemarksText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRemarksText.Name = "lblRemarksText";
            this.lblRemarksText.Size = new System.Drawing.Size(573, 357);
            this.lblRemarksText.TabIndex = 1;
            this.lblRemarksText.Text = "Your application is active. Final decision has not been declared yet.";
            // 
            // lblCurrentState
            // 
            this.lblCurrentState.AutoSize = true;
            this.lblCurrentState.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblCurrentState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(126)))), ((int)(((byte)(34)))));
            this.lblCurrentState.Location = new System.Drawing.Point(20, 62);
            this.lblCurrentState.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrentState.Name = "lblCurrentState";
            this.lblCurrentState.Size = new System.Drawing.Size(143, 23);
            this.lblCurrentState.TabIndex = 1;
            this.lblCurrentState.Text = "Current Status: --";
            // 
            // lblSelectedJob
            // 
            this.lblSelectedJob.AutoSize = true;
            this.lblSelectedJob.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblSelectedJob.Location = new System.Drawing.Point(20, 31);
            this.lblSelectedJob.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSelectedJob.Name = "lblSelectedJob";
            this.lblSelectedJob.Size = new System.Drawing.Size(213, 28);
            this.lblSelectedJob.TabIndex = 2;
            this.lblSelectedJob.Text = "Select an Application";
            // 
            // ApplicationStatusForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 689);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.pnlHeader);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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