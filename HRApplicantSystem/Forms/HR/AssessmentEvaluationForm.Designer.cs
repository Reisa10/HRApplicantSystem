namespace HRApplicantSystem.Forms.HR
{
    partial class AssessmentEvaluationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            this.grpApplicants = new System.Windows.Forms.GroupBox();
            this.dgvApplicants = new System.Windows.Forms.DataGridView();
            this.grpAssessment = new System.Windows.Forms.GroupBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.lblRemarks = new System.Windows.Forms.Label();
            this.txtAssessmentType = new System.Windows.Forms.TextBox();
            this.lblAssessmentType = new System.Windows.Forms.Label();
            this.rdoFail = new System.Windows.Forms.RadioButton();
            this.rdoPass = new System.Windows.Forms.RadioButton();
            this.lblResult = new System.Windows.Forms.Label();
            this.nudScore = new System.Windows.Forms.NumericUpDown();
            this.lblScore = new System.Windows.Forms.Label();
            this.lblApplicant = new System.Windows.Forms.Label();
            this.lblJob = new System.Windows.Forms.Label();
            this.grpApplicants.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvApplicants)).BeginInit();
            this.grpAssessment.SuspendLayout();
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
            this.grpApplicants.Text = "Applicants For Assessment";
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
            // grpAssessment
            // 
            this.grpAssessment.Controls.Add(this.btnSave);
            this.grpAssessment.Controls.Add(this.btnBack);
            this.grpAssessment.Controls.Add(this.txtRemarks);
            this.grpAssessment.Controls.Add(this.lblRemarks);
            this.grpAssessment.Controls.Add(this.txtAssessmentType);
            this.grpAssessment.Controls.Add(this.lblAssessmentType);
            this.grpAssessment.Controls.Add(this.rdoFail);
            this.grpAssessment.Controls.Add(this.rdoPass);
            this.grpAssessment.Controls.Add(this.lblResult);
            this.grpAssessment.Controls.Add(this.nudScore);
            this.grpAssessment.Controls.Add(this.lblScore);
            this.grpAssessment.Controls.Add(this.lblApplicant);
            this.grpAssessment.Controls.Add(this.lblJob);
            this.grpAssessment.Location = new System.Drawing.Point(12, 236);
            this.grpAssessment.Name = "grpAssessment";
            this.grpAssessment.Size = new System.Drawing.Size(662, 290);
            this.grpAssessment.TabIndex = 1;
            this.grpAssessment.TabStop = false;
            this.grpAssessment.Text = "Assessment Details";
            // 
            // lblApplicant
            // 
            this.lblApplicant.AutoSize = true;
            this.lblApplicant.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblApplicant.Location = new System.Drawing.Point(10, 25);
            this.lblApplicant.Name = "lblApplicant";
            this.lblApplicant.Size = new System.Drawing.Size(122, 25);
            this.lblApplicant.TabIndex = 0;
            this.lblApplicant.Text = "Applicant: \u2014";
            // 
            // lblJob
            // 
            this.lblJob.AutoSize = true;
            this.lblJob.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblJob.ForeColor = System.Drawing.Color.FromArgb(100, 100, 100);
            this.lblJob.Location = new System.Drawing.Point(10, 52);
            this.lblJob.Name = "lblJob";
            this.lblJob.Size = new System.Drawing.Size(57, 21);
            this.lblJob.TabIndex = 1;
            this.lblJob.Text = "Job: \u2014";
            // 
            // lblAssessmentType
            // 
            this.lblAssessmentType.AutoSize = true;
            this.lblAssessmentType.Location = new System.Drawing.Point(11, 88);
            this.lblAssessmentType.Name = "lblAssessmentType";
            this.lblAssessmentType.Size = new System.Drawing.Size(108, 16);
            this.lblAssessmentType.TabIndex = 2;
            this.lblAssessmentType.Text = "Assessment Type:";
            // 
            // txtAssessmentType
            // 
            this.txtAssessmentType.Location = new System.Drawing.Point(133, 85);
            this.txtAssessmentType.Name = "txtAssessmentType";
            this.txtAssessmentType.Size = new System.Drawing.Size(273, 22);
            this.txtAssessmentType.TabIndex = 3;
            // 
            // lblScore
            // 
            this.lblScore.AutoSize = true;
            this.lblScore.Location = new System.Drawing.Point(11, 120);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(95, 16);
            this.lblScore.TabIndex = 4;
            this.lblScore.Text = "Score (0 - 100):";
            // 
            // nudScore
            // 
            this.nudScore.Location = new System.Drawing.Point(133, 117);
            this.nudScore.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            this.nudScore.Name = "nudScore";
            this.nudScore.Size = new System.Drawing.Size(120, 22);
            this.nudScore.TabIndex = 5;
            this.nudScore.Value = new decimal(new int[] { 75, 0, 0, 0 });
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(11, 150);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(48, 16);
            this.lblResult.TabIndex = 6;
            this.lblResult.Text = "Result:";
            // 
            // rdoPass
            // 
            this.rdoPass.AutoSize = true;
            this.rdoPass.Location = new System.Drawing.Point(133, 148);
            this.rdoPass.Name = "rdoPass";
            this.rdoPass.Size = new System.Drawing.Size(59, 20);
            this.rdoPass.TabIndex = 7;
            this.rdoPass.TabStop = true;
            this.rdoPass.Text = "Pass";
            this.rdoPass.UseVisualStyleBackColor = true;
            // 
            // rdoFail
            // 
            this.rdoFail.AutoSize = true;
            this.rdoFail.Location = new System.Drawing.Point(210, 148);
            this.rdoFail.Name = "rdoFail";
            this.rdoFail.Size = new System.Drawing.Size(50, 20);
            this.rdoFail.TabIndex = 8;
            this.rdoFail.TabStop = true;
            this.rdoFail.Text = "Fail";
            this.rdoFail.UseVisualStyleBackColor = true;
            // 
            // lblRemarks
            // 
            this.lblRemarks.AutoSize = true;
            this.lblRemarks.Location = new System.Drawing.Point(11, 184);
            this.lblRemarks.Name = "lblRemarks";
            this.lblRemarks.Size = new System.Drawing.Size(65, 16);
            this.lblRemarks.TabIndex = 9;
            this.lblRemarks.Text = "Remarks:";
            // 
            // txtRemarks
            // 
            this.txtRemarks.Location = new System.Drawing.Point(133, 181);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRemarks.Size = new System.Drawing.Size(273, 55);
            this.txtRemarks.TabIndex = 10;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(459, 249);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(115, 30);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "Save Assessment";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(580, 249);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(76, 30);
            this.btnBack.TabIndex = 12;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // AssessmentEvaluationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 545);
            this.Controls.Add(this.grpAssessment);
            this.Controls.Add(this.grpApplicants);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "AssessmentEvaluationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Assessment Evaluation";
            this.Load += new System.EventHandler(this.AssessmentEvaluationForm_Load);
            this.grpApplicants.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvApplicants)).EndInit();
            this.grpAssessment.ResumeLayout(false);
            this.grpAssessment.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudScore)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox grpApplicants;
        private System.Windows.Forms.DataGridView dgvApplicants;
        private System.Windows.Forms.GroupBox grpAssessment;
        private System.Windows.Forms.Label lblApplicant;
        private System.Windows.Forms.Label lblJob;
        private System.Windows.Forms.Label lblAssessmentType;
        private System.Windows.Forms.TextBox txtAssessmentType;
        private System.Windows.Forms.Label lblScore;
        private System.Windows.Forms.NumericUpDown nudScore;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.RadioButton rdoPass;
        private System.Windows.Forms.RadioButton rdoFail;
        private System.Windows.Forms.Label lblRemarks;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnBack;
    }
}
