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
            this.lstTrackingTimeline.FormattingEnabled = true;
            this.lstTrackingTimeline.Location = new System.Drawing.Point(20, 130);
            this.lstTrackingTimeline.Name = "lstTrackingTimeline";
            this.lstTrackingTimeline.Size = new System.Drawing.Size(426, 108);
            this.lstTrackingTimeline.TabIndex = 2;
            // 
            // lblCurrentState
            // 
            this.lblCurrentState.AutoSize = true;
            this.lblCurrentState.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblCurrentState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(126)))), ((int)(((byte)(34)))));
            this.lblCurrentState.Location = new System.Drawing.Point(20, 70);
            this.lblCurrentState.Name = "lblCurrentState";
            this.lblCurrentState.Size = new System.Drawing.Size(111, 19);
            this.lblCurrentState.TabIndex = 1;
            this.lblCurrentState.Text = "Current Status: --";
            // 
            // lblSelectedJob
            // 
            this.lblSelectedJob.AutoSize = true;
            this.lblSelectedJob.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblSelectedJob.Location = new System.Drawing.Point(18, 35);
            this.lblSelectedJob.Name = "lblSelectedJob";
            this.lblSelectedJob.Size = new System.Drawing.Size(167, 21);
            this.lblSelectedJob.TabIndex = 0;
            this.lblSelectedJob.Text = "Select an Application";
            // 
            // ApplicationStatusForm
            // 
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
    }
}