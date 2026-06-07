namespace HRApplicantSystem.Forms.HR
{
    partial class InterviewEvaluationForm
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
            this.grpApplicants = new System.Windows.Forms.GroupBox();
            this.dgvApplicants = new System.Windows.Forms.DataGridView();
            this.grpEvaluation = new System.Windows.Forms.GroupBox();
            this.rdoFail = new System.Windows.Forms.RadioButton();
            this.rdoPass = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.nudScore = new System.Windows.Forms.NumericUpDown();
            this.Scorelabel = new System.Windows.Forms.Label();
            this.lblInterviewDate = new System.Windows.Forms.Label();
            this.lblApplicant = new System.Windows.Forms.Label();
            this.lblJob = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRecommendation = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.grpApplicants.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvApplicants)).BeginInit();
            this.grpEvaluation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudScore)).BeginInit();
            this.SuspendLayout();
            // 
            // grpApplicants
            // 
            this.grpApplicants.Controls.Add(this.dgvApplicants);
            this.grpApplicants.Location = new System.Drawing.Point(10, 12);
            this.grpApplicants.Name = "grpApplicants";
            this.grpApplicants.Size = new System.Drawing.Size(662, 215);
            this.grpApplicants.TabIndex = 0;
            this.grpApplicants.TabStop = false;
            this.grpApplicants.Text = "Interviewed Applicants";
            // 
            // dgvApplicants
            // 
            this.dgvApplicants.AllowUserToAddRows = false;
            this.dgvApplicants.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvApplicants.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvApplicants.Location = new System.Drawing.Point(3, 18);
            this.dgvApplicants.MultiSelect = false;
            this.dgvApplicants.Name = "dgvApplicants";
            this.dgvApplicants.ReadOnly = true;
            this.dgvApplicants.RowHeadersWidth = 51;
            this.dgvApplicants.RowTemplate.Height = 24;
            this.dgvApplicants.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvApplicants.Size = new System.Drawing.Size(656, 194);
            this.dgvApplicants.TabIndex = 0;
            this.dgvApplicants.SelectionChanged += new System.EventHandler(this.dgvApplicants_SelectionChanged);
            // 
            // grpEvaluation
            // 
            this.grpEvaluation.Controls.Add(this.btnSave);
            this.grpEvaluation.Controls.Add(this.btnBack);
            this.grpEvaluation.Controls.Add(this.txtRecommendation);
            this.grpEvaluation.Controls.Add(this.label3);
            this.grpEvaluation.Controls.Add(this.txtRemarks);
            this.grpEvaluation.Controls.Add(this.label2);
            this.grpEvaluation.Controls.Add(this.rdoFail);
            this.grpEvaluation.Controls.Add(this.rdoPass);
            this.grpEvaluation.Controls.Add(this.label1);
            this.grpEvaluation.Controls.Add(this.nudScore);
            this.grpEvaluation.Controls.Add(this.Scorelabel);
            this.grpEvaluation.Controls.Add(this.lblInterviewDate);
            this.grpEvaluation.Controls.Add(this.lblApplicant);
            this.grpEvaluation.Controls.Add(this.lblJob);
            this.grpEvaluation.Location = new System.Drawing.Point(12, 236);
            this.grpEvaluation.Name = "grpEvaluation";
            this.grpEvaluation.Size = new System.Drawing.Size(662, 265);
            this.grpEvaluation.TabIndex = 1;
            this.grpEvaluation.TabStop = false;
            this.grpEvaluation.Text = "Evaluation Details";
            // 
            // rdoFail
            // 
            this.rdoFail.AutoSize = true;
            this.rdoFail.Location = new System.Drawing.Point(199, 135);
            this.rdoFail.Name = "rdoFail";
            this.rdoFail.Size = new System.Drawing.Size(50, 20);
            this.rdoFail.TabIndex = 9;
            this.rdoFail.TabStop = true;
            this.rdoFail.Text = "Fail";
            this.rdoFail.UseVisualStyleBackColor = true;
            // 
            // rdoPass
            // 
            this.rdoPass.AutoSize = true;
            this.rdoPass.Location = new System.Drawing.Point(133, 135);
            this.rdoPass.Name = "rdoPass";
            this.rdoPass.Size = new System.Drawing.Size(59, 20);
            this.rdoPass.TabIndex = 8;
            this.rdoPass.TabStop = true;
            this.rdoPass.Text = "Pass";
            this.rdoPass.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 135);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 16);
            this.label1.TabIndex = 7;
            this.label1.Text = "Result:";
            // 
            // nudScore
            // 
            this.nudScore.Location = new System.Drawing.Point(133, 106);
            this.nudScore.Name = "nudScore";
            this.nudScore.Size = new System.Drawing.Size(120, 22);
            this.nudScore.TabIndex = 6;
            this.nudScore.Value = new decimal(new int[] {
            75,
            0,
            0,
            0});
            // 
            // Scorelabel
            // 
            this.Scorelabel.AutoSize = true;
            this.Scorelabel.Location = new System.Drawing.Point(12, 106);
            this.Scorelabel.Name = "Scorelabel";
            this.Scorelabel.Size = new System.Drawing.Size(95, 16);
            this.Scorelabel.TabIndex = 5;
            this.Scorelabel.Text = "Score (0 - 100):";
            // 
            // lblInterviewDate
            // 
            this.lblInterviewDate.AutoSize = true;
            this.lblInterviewDate.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblInterviewDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lblInterviewDate.Location = new System.Drawing.Point(11, 75);
            this.lblInterviewDate.Name = "lblInterviewDate";
            this.lblInterviewDate.Size = new System.Drawing.Size(123, 21);
            this.lblInterviewDate.TabIndex = 4;
            this.lblInterviewDate.Text = "Interview Date: -";
            // 
            // lblApplicant
            // 
            this.lblApplicant.AutoSize = true;
            this.lblApplicant.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblApplicant.Location = new System.Drawing.Point(10, 28);
            this.lblApplicant.Name = "lblApplicant";
            this.lblApplicant.Size = new System.Drawing.Size(122, 25);
            this.lblApplicant.TabIndex = 2;
            this.lblApplicant.Text = "Applicant: —";
            // 
            // lblJob
            // 
            this.lblJob.AutoSize = true;
            this.lblJob.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblJob.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lblJob.Location = new System.Drawing.Point(10, 54);
            this.lblJob.Name = "lblJob";
            this.lblJob.Size = new System.Drawing.Size(57, 21);
            this.lblJob.TabIndex = 3;
            this.lblJob.Text = "Job: —";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 169);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 16);
            this.label2.TabIndex = 10;
            this.label2.Text = "Remarks:";
            // 
            // txtRemarks
            // 
            this.txtRemarks.Location = new System.Drawing.Point(133, 166);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRemarks.Size = new System.Drawing.Size(273, 41);
            this.txtRemarks.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 227);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 16);
            this.label3.TabIndex = 12;
            this.label3.Text = "Recommendation:";
            // 
            // txtRecommendation
            // 
            this.txtRecommendation.Location = new System.Drawing.Point(133, 224);
            this.txtRecommendation.Multiline = true;
            this.txtRecommendation.Name = "txtRecommendation";
            this.txtRecommendation.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRecommendation.Size = new System.Drawing.Size(273, 41);
            this.txtRecommendation.TabIndex = 13;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(459, 229);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(105, 30);
            this.btnSave.TabIndex = 14;
            this.btnSave.Text = "Save Evaluation";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(574, 229);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(82, 30);
            this.btnBack.TabIndex = 15;
            this.btnBack.Text = "Back";
            // 
            // InterviewEvaluationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 515);
            this.Controls.Add(this.grpEvaluation);
            this.Controls.Add(this.grpApplicants);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "InterviewEvaluationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Interview Evaluation";
            this.Load += new System.EventHandler(this.InterviewEvaluationForm_Load);
            this.grpApplicants.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvApplicants)).EndInit();
            this.grpEvaluation.ResumeLayout(false);
            this.grpEvaluation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudScore)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpApplicants;
        private System.Windows.Forms.DataGridView dgvApplicants;
        private System.Windows.Forms.GroupBox grpEvaluation;
        private System.Windows.Forms.Label lblApplicant;
        private System.Windows.Forms.Label lblJob;
        private System.Windows.Forms.Label Scorelabel;
        private System.Windows.Forms.Label lblInterviewDate;
        private System.Windows.Forms.NumericUpDown nudScore;
        private System.Windows.Forms.RadioButton rdoFail;
        private System.Windows.Forms.RadioButton rdoPass;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRecommendation;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnBack;
    }
}