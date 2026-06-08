namespace HRApplicantSystem.Forms.Applicant
{
    partial class MyApplicationForm
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
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.dgvApplications = new System.Windows.Forms.DataGridView();
            this.grpDetails = new System.Windows.Forms.GroupBox();
            this.pnlProgress = new System.Windows.Forms.Panel();
            this.lblStepDraft = new System.Windows.Forms.Label();
            this.lblArrow1 = new System.Windows.Forms.Label();
            this.lblStepSubmitted = new System.Windows.Forms.Label();
            this.lblArrow2 = new System.Windows.Forms.Label();
            this.lblStepReview = new System.Windows.Forms.Label();
            this.lblArrow3 = new System.Windows.Forms.Label();
            this.lblStepDecision = new System.Windows.Forms.Label();
            this.pnlEditApplication = new System.Windows.Forms.Panel();
            this.lblEditTitle = new System.Windows.Forms.Label();
            this.cmbJobVacancy = new System.Windows.Forms.ComboBox();
            this.btnSaveChanges = new System.Windows.Forms.Button();
            this.lblLockMessage = new System.Windows.Forms.Label();
            this.btnUploadShortcut = new System.Windows.Forms.Button();
            this.btnWithdraw = new System.Windows.Forms.Button();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnBack = new System.Windows.Forms.Button();
            this.lblHeader = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvApplications)).BeginInit();
            this.grpDetails.SuspendLayout();
            this.pnlProgress.SuspendLayout();
            this.pnlEditApplication.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 60);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.dgvApplications);
            this.splitContainer.Panel1.Padding = new System.Windows.Forms.Padding(15, 15, 10, 15);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.grpDetails);
            this.splitContainer.Panel2.Padding = new System.Windows.Forms.Padding(10, 15, 15, 15);
            this.splitContainer.Size = new System.Drawing.Size(900, 440);
            this.splitContainer.SplitterDistance = 440;
            this.splitContainer.TabIndex = 0;
            // 
            // dgvApplications
            // 
            this.dgvApplications.AllowUserToAddRows = false;
            this.dgvApplications.AllowUserToDeleteRows = false;
            this.dgvApplications.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvApplications.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvApplications.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvApplications.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvApplications.Location = new System.Drawing.Point(15, 15);
            this.dgvApplications.MultiSelect = false;
            this.dgvApplications.Name = "dgvApplications";
            this.dgvApplications.ReadOnly = true;
            this.dgvApplications.RowHeadersVisible = false;
            this.dgvApplications.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvApplications.Size = new System.Drawing.Size(415, 410);
            this.dgvApplications.TabIndex = 0;
            this.dgvApplications.SelectionChanged += new System.EventHandler(this.dgvApplications_SelectionChanged);
            // 
            // grpDetails
            // 
            this.grpDetails.Controls.Add(this.pnlProgress);
            this.grpDetails.Controls.Add(this.pnlEditApplication);
            this.grpDetails.Controls.Add(this.lblLockMessage);
            this.grpDetails.Controls.Add(this.btnUploadShortcut);
            this.grpDetails.Controls.Add(this.btnWithdraw);
            this.grpDetails.Controls.Add(this.btnSubmit);
            this.grpDetails.Controls.Add(this.lblDate);
            this.grpDetails.Controls.Add(this.lblStatus);
            this.grpDetails.Controls.Add(this.lblTitle);
            this.grpDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDetails.Location = new System.Drawing.Point(10, 15);
            this.grpDetails.Name = "grpDetails";
            this.grpDetails.Size = new System.Drawing.Size(431, 410);
            this.grpDetails.TabIndex = 0;
            this.grpDetails.TabStop = false;
            this.grpDetails.Text = "Application Process Details";
            // 
            // pnlProgress
            // 
            this.pnlProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlProgress.Controls.Add(this.lblStepDraft);
            this.pnlProgress.Controls.Add(this.lblArrow1);
            this.pnlProgress.Controls.Add(this.lblStepSubmitted);
            this.pnlProgress.Controls.Add(this.lblArrow2);
            this.pnlProgress.Controls.Add(this.lblStepReview);
            this.pnlProgress.Controls.Add(this.lblArrow3);
            this.pnlProgress.Controls.Add(this.lblStepDecision);
            this.pnlProgress.Location = new System.Drawing.Point(15, 100);
            this.pnlProgress.Name = "pnlProgress";
            this.pnlProgress.Size = new System.Drawing.Size(401, 50);
            this.pnlProgress.TabIndex = 7;
            // 
            // lblStepDraft
            // 
            this.lblStepDraft.AutoSize = true;
            this.lblStepDraft.Location = new System.Drawing.Point(5, 15);
            this.lblStepDraft.Name = "lblStepDraft";
            this.lblStepDraft.Size = new System.Drawing.Size(30, 13);
            this.lblStepDraft.TabIndex = 0;
            this.lblStepDraft.Text = "Draft";
            // 
            // lblArrow1
            // 
            this.lblArrow1.AutoSize = true;
            this.lblArrow1.ForeColor = System.Drawing.Color.DarkGray;
            this.lblArrow1.Location = new System.Drawing.Point(55, 15);
            this.lblArrow1.Name = "lblArrow1";
            this.lblArrow1.Size = new System.Drawing.Size(19, 13);
            this.lblArrow1.TabIndex = 1;
            this.lblArrow1.Text = "➔";
            // 
            // lblStepSubmitted
            // 
            this.lblStepSubmitted.AutoSize = true;
            this.lblStepSubmitted.Location = new System.Drawing.Point(85, 15);
            this.lblStepSubmitted.Name = "lblStepSubmitted";
            this.lblStepSubmitted.Size = new System.Drawing.Size(53, 13);
            this.lblStepSubmitted.TabIndex = 2;
            this.lblStepSubmitted.Text = "Submitted";
            // 
            // lblArrow2
            // 
            this.lblArrow2.AutoSize = true;
            this.lblArrow2.ForeColor = System.Drawing.Color.DarkGray;
            this.lblArrow2.Location = new System.Drawing.Point(160, 15);
            this.lblArrow2.Name = "lblArrow2";
            this.lblArrow2.Size = new System.Drawing.Size(19, 13);
            this.lblArrow2.TabIndex = 3;
            this.lblArrow2.Text = "➔";
            // 
            // lblStepReview
            // 
            this.lblStepReview.AutoSize = true;
            this.lblStepReview.Location = new System.Drawing.Point(190, 15);
            this.lblStepReview.Name = "lblStepReview";
            this.lblStepReview.Size = new System.Drawing.Size(51, 13);
            this.lblStepReview.TabIndex = 4;
            this.lblStepReview.Text = "In Review";
            // 
            // lblArrow3
            // 
            this.lblArrow3.AutoSize = true;
            this.lblArrow3.ForeColor = System.Drawing.Color.DarkGray;
            this.lblArrow3.Location = new System.Drawing.Point(265, 15);
            this.lblArrow3.Name = "lblArrow3";
            this.lblArrow3.Size = new System.Drawing.Size(19, 13);
            this.lblArrow3.TabIndex = 5;
            this.lblArrow3.Text = "➔";
            // 
            // lblStepDecision
            // 
            this.lblStepDecision.AutoSize = true;
            this.lblStepDecision.Location = new System.Drawing.Point(295, 15);
            this.lblStepDecision.Name = "lblStepDecision";
            this.lblStepDecision.Size = new System.Drawing.Size(48, 13);
            this.lblStepDecision.TabIndex = 6;
            this.lblStepDecision.Text = "Decision";
            // 
            // pnlEditApplication
            // 
            this.pnlEditApplication.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlEditApplication.Controls.Add(this.lblEditTitle);
            this.pnlEditApplication.Controls.Add(this.cmbJobVacancy);
            this.pnlEditApplication.Controls.Add(this.btnSaveChanges);
            this.pnlEditApplication.Location = new System.Drawing.Point(15, 160);
            this.pnlEditApplication.Name = "pnlEditApplication";
            this.pnlEditApplication.Size = new System.Drawing.Size(401, 85);
            this.pnlEditApplication.TabIndex = 8;
            // 
            // lblEditTitle
            // 
            this.lblEditTitle.AutoSize = true;
            this.lblEditTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lblEditTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblEditTitle.Location = new System.Drawing.Point(0, 5);
            this.lblEditTitle.Name = "lblEditTitle";
            this.lblEditTitle.Size = new System.Drawing.Size(166, 15);
            this.lblEditTitle.TabIndex = 0;
            this.lblEditTitle.Text = "Modify Choice of Job Vacancy:";
            // 
            // cmbJobVacancy
            // 
            this.cmbJobVacancy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbJobVacancy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbJobVacancy.FormattingEnabled = true;
            this.cmbJobVacancy.Location = new System.Drawing.Point(0, 25);
            this.cmbJobVacancy.Name = "cmbJobVacancy";
            this.cmbJobVacancy.Size = new System.Drawing.Size(250, 21);
            this.cmbJobVacancy.TabIndex = 1;
            // 
            // btnSaveChanges
            // 
            this.btnSaveChanges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveChanges.Location = new System.Drawing.Point(260, 23);
            this.btnSaveChanges.Name = "btnSaveChanges";
            this.btnSaveChanges.Size = new System.Drawing.Size(141, 26);
            this.btnSaveChanges.TabIndex = 2;
            this.btnSaveChanges.Text = "Save Details";
            this.btnSaveChanges.UseVisualStyleBackColor = true;
            this.btnSaveChanges.Click += new System.EventHandler(this.btnSaveChanges_Click);
            // 
            // lblLockMessage
            // 
            this.lblLockMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLockMessage.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLockMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(57)))), ((int)(((byte)(43)))));
            this.lblLockMessage.Location = new System.Drawing.Point(15, 255);
            this.lblLockMessage.Name = "lblLockMessage";
            this.lblLockMessage.Size = new System.Drawing.Size(401, 35);
            this.lblLockMessage.TabIndex = 5;
            this.lblLockMessage.Text = "This application is currently locked because HR review has started.";
            this.lblLockMessage.Visible = false;
            // 
            // btnUploadShortcut
            // 
            this.btnUploadShortcut.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUploadShortcut.Location = new System.Drawing.Point(15, 295);
            this.btnUploadShortcut.Name = "btnUploadShortcut";
            this.btnUploadShortcut.Size = new System.Drawing.Size(401, 32);
            this.btnUploadShortcut.TabIndex = 6;
            this.btnUploadShortcut.Text = "Upload Missing Requirements";
            this.btnUploadShortcut.UseVisualStyleBackColor = true;
            this.btnUploadShortcut.Visible = false;
            this.btnUploadShortcut.Click += new System.EventHandler(this.btnUploadShortcut_Click);
            // 
            // btnWithdraw
            // 
            this.btnWithdraw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWithdraw.Location = new System.Drawing.Point(220, 345);
            this.btnWithdraw.Name = "btnWithdraw";
            this.btnWithdraw.Size = new System.Drawing.Size(196, 45);
            this.btnWithdraw.TabIndex = 4;
            this.btnWithdraw.Text = "Withdraw Application";
            this.btnWithdraw.UseVisualStyleBackColor = true;
            this.btnWithdraw.Click += new System.EventHandler(this.btnWithdraw_Click);
            // 
            // btnSubmit
            // 
            this.btnSubmit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSubmit.Location = new System.Drawing.Point(15, 345);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(196, 45);
            this.btnSubmit.TabIndex = 3;
            this.btnSubmit.Text = "Submit Application";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDate.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblDate.Location = new System.Drawing.Point(15, 75);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(73, 15);
            this.lblDate.TabIndex = 2;
            this.lblDate.Text = "Applied On: ";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.lblStatus.Location = new System.Drawing.Point(15, 50);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(55, 19);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "Status: ";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(15, 25);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(167, 21);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Select an Application";
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.btnBack);
            this.pnlHeader.Controls.Add(this.lblHeader);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(900, 60);
            this.pnlHeader.TabIndex = 1;
            // 
            // btnBack
            // 
            this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBack.Location = new System.Drawing.Point(795, 15);
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
            this.lblHeader.Size = new System.Drawing.Size(209, 30);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "My Job Applications";
            // 
            // MyApplicationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 500);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.pnlHeader);
            this.Name = "MyApplicationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "My Applications";
            this.Load += new System.EventHandler(this.MyApplicationForm_Load);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvApplications)).EndInit();
            this.grpDetails.ResumeLayout(false);
            this.grpDetails.PerformLayout();
            this.pnlProgress.ResumeLayout(false);
            this.pnlProgress.PerformLayout();
            this.pnlEditApplication.ResumeLayout(false);
            this.pnlEditApplication.PerformLayout();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.DataGridView dgvApplications;
        private System.Windows.Forms.GroupBox grpDetails;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnWithdraw;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Label lblLockMessage;

        // Newly added elements:
        private System.Windows.Forms.Panel pnlProgress;
        private System.Windows.Forms.Label lblStepDraft;
        private System.Windows.Forms.Label lblArrow1;
        private System.Windows.Forms.Label lblStepSubmitted;
        private System.Windows.Forms.Label lblArrow2;
        private System.Windows.Forms.Label lblStepReview;
        private System.Windows.Forms.Label lblArrow3;
        private System.Windows.Forms.Label lblStepDecision;
        private System.Windows.Forms.Panel pnlEditApplication;
        private System.Windows.Forms.Label lblEditTitle;
        private System.Windows.Forms.ComboBox cmbJobVacancy;
        private System.Windows.Forms.Button btnSaveChanges;
        private System.Windows.Forms.Button btnUploadShortcut;
    }
}