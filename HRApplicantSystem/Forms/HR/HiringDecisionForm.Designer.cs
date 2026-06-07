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
            this.grpApplicants = new System.Windows.Forms.GroupBox();
            this.dgvApplicants = new System.Windows.Forms.DataGridView();
            this.grpDecision = new System.Windows.Forms.GroupBox();
            this.lblApplicant = new System.Windows.Forms.Label();
            this.lblJob = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.rdoAccepted = new System.Windows.Forms.RadioButton();
            this.rdoRejected = new System.Windows.Forms.RadioButton();
            this.rdoOnHold = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.grpApplicants.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvApplicants)).BeginInit();
            this.grpDecision.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpApplicants
            // 
            this.grpApplicants.Controls.Add(this.dgvApplicants);
            this.grpApplicants.Location = new System.Drawing.Point(12, 10);
            this.grpApplicants.Name = "grpApplicants";
            this.grpApplicants.Size = new System.Drawing.Size(662, 215);
            this.grpApplicants.TabIndex = 0;
            this.grpApplicants.TabStop = false;
            this.grpApplicants.Text = "Applicants For Final Decision";
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
            // grpDecision
            // 
            this.grpDecision.Controls.Add(this.btnSave);
            this.grpDecision.Controls.Add(this.btnBack);
            this.grpDecision.Controls.Add(this.txtRemarks);
            this.grpDecision.Controls.Add(this.label2);
            this.grpDecision.Controls.Add(this.rdoOnHold);
            this.grpDecision.Controls.Add(this.rdoRejected);
            this.grpDecision.Controls.Add(this.rdoAccepted);
            this.grpDecision.Controls.Add(this.label1);
            this.grpDecision.Controls.Add(this.lblApplicant);
            this.grpDecision.Controls.Add(this.lblJob);
            this.grpDecision.Location = new System.Drawing.Point(12, 236);
            this.grpDecision.Name = "grpDecision";
            this.grpDecision.Size = new System.Drawing.Size(662, 265);
            this.grpDecision.TabIndex = 1;
            this.grpDecision.TabStop = false;
            this.grpDecision.Text = "Hiring Decision";
            // 
            // lblApplicant
            // 
            this.lblApplicant.AutoSize = true;
            this.lblApplicant.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblApplicant.Location = new System.Drawing.Point(10, 28);
            this.lblApplicant.Name = "lblApplicant";
            this.lblApplicant.Size = new System.Drawing.Size(122, 25);
            this.lblApplicant.TabIndex = 4;
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
            this.lblJob.TabIndex = 5;
            this.lblJob.Text = "Job: —";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "Decision:";
            // 
            // rdoAccepted
            // 
            this.rdoAccepted.AutoSize = true;
            this.rdoAccepted.Location = new System.Drawing.Point(100, 77);
            this.rdoAccepted.Name = "rdoAccepted";
            this.rdoAccepted.Size = new System.Drawing.Size(86, 20);
            this.rdoAccepted.TabIndex = 7;
            this.rdoAccepted.TabStop = true;
            this.rdoAccepted.Text = "Accepted";
            this.rdoAccepted.UseVisualStyleBackColor = true;
            // 
            // rdoRejected
            // 
            this.rdoRejected.AutoSize = true;
            this.rdoRejected.Location = new System.Drawing.Point(192, 77);
            this.rdoRejected.Name = "rdoRejected";
            this.rdoRejected.Size = new System.Drawing.Size(83, 20);
            this.rdoRejected.TabIndex = 8;
            this.rdoRejected.TabStop = true;
            this.rdoRejected.Text = "Rejected";
            this.rdoRejected.UseVisualStyleBackColor = true;
            // 
            // rdoOnHold
            // 
            this.rdoOnHold.AutoSize = true;
            this.rdoOnHold.Location = new System.Drawing.Point(281, 79);
            this.rdoOnHold.Name = "rdoOnHold";
            this.rdoOnHold.Size = new System.Drawing.Size(77, 20);
            this.rdoOnHold.TabIndex = 9;
            this.rdoOnHold.TabStop = true;
            this.rdoOnHold.Text = "On Hold";
            this.rdoOnHold.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 16);
            this.label2.TabIndex = 10;
            this.label2.Text = "Remarks:";
            // 
            // txtRemarks
            // 
            this.txtRemarks.Location = new System.Drawing.Point(80, 105);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRemarks.Size = new System.Drawing.Size(278, 100);
            this.txtRemarks.TabIndex = 11;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(459, 229);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(105, 30);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "Save Decision\n";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(574, 229);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(82, 30);
            this.btnBack.TabIndex = 17;
            this.btnBack.Text = "Back";
            // 
            // HiringDecisionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 515);
            this.Controls.Add(this.grpDecision);
            this.Controls.Add(this.grpApplicants);
            this.Name = "HiringDecisionForm";
            this.Text = "HiringDecisionForm";
            this.Load += new System.EventHandler(this.HiringDecisionForm_Load);
            this.grpApplicants.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvApplicants)).EndInit();
            this.grpDecision.ResumeLayout(false);
            this.grpDecision.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpApplicants;
        private System.Windows.Forms.DataGridView dgvApplicants;
        private System.Windows.Forms.GroupBox grpDecision;
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
    }
}