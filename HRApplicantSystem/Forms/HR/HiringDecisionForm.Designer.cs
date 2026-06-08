namespace HRApplicantSystem.Forms.HR
{
    partial class HiringDecisionForm
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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblFormSubtitle = new System.Windows.Forms.Label();
            this.lblFormTitle = new System.Windows.Forms.Label();
            this.pnlLeftCard = new System.Windows.Forms.Panel();
            this.lblListTitle = new System.Windows.Forms.Label();
            this.dgvApplicants = new System.Windows.Forms.DataGridView();
            this.pnlRightCard = new System.Windows.Forms.Panel();
            this.pnlSeparator2 = new System.Windows.Forms.Panel();
            this.pnlSeparator1 = new System.Windows.Forms.Panel();
            this.lblEvalHeader = new System.Windows.Forms.Label();
            this.lblInterviewScore = new System.Windows.Forms.Label();
            this.lblMissingRequirements = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.rdoOnHold = new System.Windows.Forms.RadioButton();
            this.rdoRejected = new System.Windows.Forms.RadioButton();
            this.rdoAccepted = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.lblApplicant = new System.Windows.Forms.Label();
            this.lblJob = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            this.pnlLeftCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvApplicants)).BeginInit();
            this.pnlRightCard.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.pnlHeader.Controls.Add(this.lblFormSubtitle);
            this.pnlHeader.Controls.Add(this.lblFormTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(960, 65);
            this.pnlHeader.TabIndex = 2;
            // 
            // lblFormSubtitle
            // 
            this.lblFormSubtitle.AutoSize = true;
            this.lblFormSubtitle.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblFormSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblFormSubtitle.Location = new System.Drawing.Point(18, 35);
            this.lblFormSubtitle.Name = "lblFormSubtitle";
            this.lblFormSubtitle.Size = new System.Drawing.Size(437, 20);
            this.lblFormSubtitle.TabIndex = 1;
            this.lblFormSubtitle.Text = "Review candidate evaluation summaries and record employment status";
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.AutoSize = true;
            this.lblFormTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblFormTitle.ForeColor = System.Drawing.Color.White;
            this.lblFormTitle.Location = new System.Drawing.Point(16, 10);
            this.lblFormTitle.Name = "lblFormTitle";
            this.lblFormTitle.Size = new System.Drawing.Size(286, 28);
            this.lblFormTitle.TabIndex = 0;
            this.lblFormTitle.Text = "EXECUTIVE HIRING DECISION";
            // 
            // pnlLeftCard
            // 
            this.pnlLeftCard.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlLeftCard.BackColor = System.Drawing.Color.White;
            this.pnlLeftCard.Controls.Add(this.lblListTitle);
            this.pnlLeftCard.Controls.Add(this.dgvApplicants);
            this.pnlLeftCard.Location = new System.Drawing.Point(12, 77);
            this.pnlLeftCard.Name = "pnlLeftCard";
            this.pnlLeftCard.Size = new System.Drawing.Size(420, 480);
            this.pnlLeftCard.TabIndex = 3;
            // 
            // lblListTitle
            // 
            this.lblListTitle.AutoSize = true;
            this.lblListTitle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblListTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblListTitle.Location = new System.Drawing.Point(10, 10);
            this.lblListTitle.Name = "lblListTitle";
            this.lblListTitle.Size = new System.Drawing.Size(167, 21);
            this.lblListTitle.TabIndex = 1;
            this.lblListTitle.Text = "Authorization Queue";
            // 
            // dgvApplicants
            // 
            this.dgvApplicants.AllowUserToAddRows = false;
            this.dgvApplicants.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvApplicants.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvApplicants.Location = new System.Drawing.Point(10, 35);
            this.dgvApplicants.MultiSelect = false;
            this.dgvApplicants.Name = "dgvApplicants";
            this.dgvApplicants.ReadOnly = true;
            this.dgvApplicants.RowHeadersWidth = 51;
            this.dgvApplicants.RowTemplate.Height = 24;
            this.dgvApplicants.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvApplicants.Size = new System.Drawing.Size(400, 433);
            this.dgvApplicants.TabIndex = 0;
            this.dgvApplicants.SelectionChanged += new System.EventHandler(this.dgvApplicants_SelectionChanged);
            // 
            // pnlRightCard
            // 
            this.pnlRightCard.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlRightCard.BackColor = System.Drawing.Color.White;
            this.pnlRightCard.Controls.Add(this.pnlSeparator2);
            this.pnlRightCard.Controls.Add(this.pnlSeparator1);
            this.pnlRightCard.Controls.Add(this.lblEvalHeader);
            this.pnlRightCard.Controls.Add(this.lblInterviewScore);
            this.pnlRightCard.Controls.Add(this.lblMissingRequirements);
            this.pnlRightCard.Controls.Add(this.btnSave);
            this.pnlRightCard.Controls.Add(this.btnBack);
            this.pnlRightCard.Controls.Add(this.txtRemarks);
            this.pnlRightCard.Controls.Add(this.label2);
            this.pnlRightCard.Controls.Add(this.rdoOnHold);
            this.pnlRightCard.Controls.Add(this.rdoRejected);
            this.pnlRightCard.Controls.Add(this.rdoAccepted);
            this.pnlRightCard.Controls.Add(this.label1);
            this.pnlRightCard.Controls.Add(this.lblApplicant);
            this.pnlRightCard.Controls.Add(this.lblJob);
            this.pnlRightCard.Location = new System.Drawing.Point(444, 77);
            this.pnlRightCard.Name = "pnlRightCard";
            this.pnlRightCard.Size = new System.Drawing.Size(504, 480);
            this.pnlRightCard.TabIndex = 4;
            // 
            // pnlSeparator2
            // 
            this.pnlSeparator2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSeparator2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.pnlSeparator2.Location = new System.Drawing.Point(20, 210);
            this.pnlSeparator2.Name = "pnlSeparator2";
            this.pnlSeparator2.Size = new System.Drawing.Size(464, 1);
            this.pnlSeparator2.TabIndex = 22;
            // 
            // pnlSeparator1
            // 
            this.pnlSeparator1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSeparator1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.pnlSeparator1.Location = new System.Drawing.Point(20, 68);
            this.pnlSeparator1.Name = "pnlSeparator1";
            this.pnlSeparator1.Size = new System.Drawing.Size(464, 1);
            this.pnlSeparator1.TabIndex = 21;
            // 
            // lblEvalHeader
            // 
            this.lblEvalHeader.AutoSize = true;
            this.lblEvalHeader.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblEvalHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.lblEvalHeader.Location = new System.Drawing.Point(20, 78);
            this.lblEvalHeader.Name = "lblEvalHeader";
            this.lblEvalHeader.Size = new System.Drawing.Size(181, 21);
            this.lblEvalHeader.TabIndex = 18;
            this.lblEvalHeader.Text = "EVALUATION SUMMARY";
            // 
            // lblInterviewScore
            // 
            this.lblInterviewScore.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInterviewScore.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInterviewScore.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblInterviewScore.Location = new System.Drawing.Point(20, 102);
            this.lblInterviewScore.Name = "lblInterviewScore";
            this.lblInterviewScore.Size = new System.Drawing.Size(464, 55);
            this.lblInterviewScore.TabIndex = 19;
            this.lblInterviewScore.Text = "Evaluation: Select an applicant to see scores.";
            // 
            // lblMissingRequirements
            // 
            this.lblMissingRequirements.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMissingRequirements.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMissingRequirements.ForeColor = System.Drawing.Color.Gray;
            this.lblMissingRequirements.Location = new System.Drawing.Point(20, 160);
            this.lblMissingRequirements.Name = "lblMissingRequirements";
            this.lblMissingRequirements.Size = new System.Drawing.Size(464, 45);
            this.lblMissingRequirements.TabIndex = 20;
            this.lblMissingRequirements.Text = "Missing: Select an applicant to compute requirements.";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(245, 432);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(130, 35);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "Save Decision";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnBack
            // 
            this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBack.Location = new System.Drawing.Point(385, 432);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(95, 35);
            this.btnBack.TabIndex = 17;
            this.btnBack.Text = "Back";
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // txtRemarks
            // 
            this.txtRemarks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRemarks.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRemarks.Location = new System.Drawing.Point(20, 298);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRemarks.Size = new System.Drawing.Size(464, 118);
            this.txtRemarks.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(20, 275);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(163, 20);
            this.label2.TabIndex = 10;
            this.label2.Text = "Remarks / Justification:";
            // 
            // rdoOnHold
            // 
            this.rdoOnHold.AutoSize = true;
            this.rdoOnHold.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoOnHold.Location = new System.Drawing.Point(365, 222);
            this.rdoOnHold.Name = "rdoOnHold";
            this.rdoOnHold.Size = new System.Drawing.Size(85, 24);
            this.rdoOnHold.TabIndex = 9;
            this.rdoOnHold.TabStop = true;
            this.rdoOnHold.Text = "On Hold";
            this.rdoOnHold.UseVisualStyleBackColor = true;
            this.rdoOnHold.CheckedChanged += new System.EventHandler(this.rdoDecision_CheckedChanged);
            // 
            // rdoRejected
            // 
            this.rdoRejected.AutoSize = true;
            this.rdoRejected.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoRejected.Location = new System.Drawing.Point(265, 222);
            this.rdoRejected.Name = "rdoRejected";
            this.rdoRejected.Size = new System.Drawing.Size(88, 24);
            this.rdoRejected.TabIndex = 8;
            this.rdoRejected.TabStop = true;
            this.rdoRejected.Text = "Rejected";
            this.rdoRejected.UseVisualStyleBackColor = true;
            this.rdoRejected.CheckedChanged += new System.EventHandler(this.rdoDecision_CheckedChanged);
            // 
            // rdoAccepted
            // 
            this.rdoAccepted.AutoSize = true;
            this.rdoAccepted.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoAccepted.Location = new System.Drawing.Point(165, 222);
            this.rdoAccepted.Name = "rdoAccepted";
            this.rdoAccepted.Size = new System.Drawing.Size(93, 24);
            this.rdoAccepted.TabIndex = 7;
            this.rdoAccepted.TabStop = true;
            this.rdoAccepted.Text = "Accepted";
            this.rdoAccepted.UseVisualStyleBackColor = true;
            this.rdoAccepted.CheckedChanged += new System.EventHandler(this.rdoDecision_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(20, 224);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "Employment State:";
            // 
            // lblApplicant
            // 
            this.lblApplicant.AutoSize = true;
            this.lblApplicant.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblApplicant.Location = new System.Drawing.Point(16, 12);
            this.lblApplicant.Name = "lblApplicant";
            this.lblApplicant.Size = new System.Drawing.Size(122, 28);
            this.lblApplicant.TabIndex = 4;
            this.lblApplicant.Text = "Applicant: —";
            // 
            // lblJob
            // 
            this.lblJob.AutoSize = true;
            this.lblJob.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblJob.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lblJob.Location = new System.Drawing.Point(18, 40);
            this.lblJob.Name = "lblJob";
            this.lblJob.Size = new System.Drawing.Size(57, 21);
            this.lblJob.TabIndex = 5;
            this.lblJob.Text = "Job: —";
            // 
            // HiringDecisionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 565);
            this.Controls.Add(this.pnlRightCard);
            this.Controls.Add(this.pnlLeftCard);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "HiringDecisionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HiringDecisionForm";
            this.Load += new System.EventHandler(this.HiringDecisionForm_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlLeftCard.ResumeLayout(false);
            this.pnlLeftCard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvApplicants)).EndInit();
            this.pnlRightCard.ResumeLayout(false);
            this.pnlRightCard.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblFormTitle;
        private System.Windows.Forms.Label lblFormSubtitle;
        private System.Windows.Forms.Panel pnlLeftCard;
        private System.Windows.Forms.Label lblListTitle;
        private System.Windows.Forms.DataGridView dgvApplicants;
        private System.Windows.Forms.Panel pnlRightCard;
        private System.Windows.Forms.Panel pnlSeparator1;
        private System.Windows.Forms.Panel pnlSeparator2;
        private System.Windows.Forms.Label lblApplicant;
        private System.Windows.Forms.Label lblJob;
        private System.Windows.Forms.RadioButton rdoRejected;
        private System.Windows.Forms.RadioButton rdoAccepted;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rdoOnHold;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Label lblEvalHeader;
        private System.Windows.Forms.Label lblInterviewScore;
        private System.Windows.Forms.Label lblMissingRequirements;
    }
}