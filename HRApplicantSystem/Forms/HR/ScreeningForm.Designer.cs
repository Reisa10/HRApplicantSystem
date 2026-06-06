namespace HRApplicantSystem.Forms.HR
{
    partial class ScreeningForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.grpApplications = new System.Windows.Forms.GroupBox();
            this.dgvApplications = new System.Windows.Forms.DataGridView();
            this.grpDecision = new System.Windows.Forms.GroupBox();
            this.lblSelectedApplicant = new System.Windows.Forms.Label();
            this.lblSelectedJob = new System.Windows.Forms.Label();
            this.lblDecisionStatic = new System.Windows.Forms.Label();
            this.rdoQualified = new System.Windows.Forms.RadioButton();
            this.rdoNotQualified = new System.Windows.Forms.RadioButton();
            this.lblRemarksStatic = new System.Windows.Forms.Label();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();

            this.grpApplications.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvApplications)).BeginInit();
            this.grpDecision.SuspendLayout();
            this.SuspendLayout();

            // ── grpApplications ──────────────────────────────────────────
            this.grpApplications.Controls.Add(this.dgvApplications);
            this.grpApplications.Location = new System.Drawing.Point(12, 10);
            this.grpApplications.Name = "grpApplications";
            this.grpApplications.Size = new System.Drawing.Size(662, 215);
            this.grpApplications.TabIndex = 0;
            this.grpApplications.TabStop = false;
            this.grpApplications.Text = "Applications Under Review";

            // ── dgvApplications ──────────────────────────────────────────
            this.dgvApplications.AllowUserToAddRows = false;
            this.dgvApplications.AllowUserToDeleteRows = false;
            this.dgvApplications.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvApplications.Location = new System.Drawing.Point(10, 22);
            this.dgvApplications.MultiSelect = false;
            this.dgvApplications.Name = "dgvApplications";
            this.dgvApplications.ReadOnly = true;
            this.dgvApplications.RowHeadersVisible = false;
            this.dgvApplications.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvApplications.Size = new System.Drawing.Size(642, 183);
            this.dgvApplications.TabIndex = 0;
            this.dgvApplications.SelectionChanged += new System.EventHandler(this.dgvApplications_SelectionChanged);

            // ── grpDecision ──────────────────────────────────────────────
            this.grpDecision.Controls.Add(this.lblSelectedApplicant);
            this.grpDecision.Controls.Add(this.lblSelectedJob);
            this.grpDecision.Controls.Add(this.lblDecisionStatic);
            this.grpDecision.Controls.Add(this.rdoQualified);
            this.grpDecision.Controls.Add(this.rdoNotQualified);
            this.grpDecision.Controls.Add(this.lblRemarksStatic);
            this.grpDecision.Controls.Add(this.txtRemarks);
            this.grpDecision.Controls.Add(this.btnSave);
            this.grpDecision.Controls.Add(this.btnBack);
            this.grpDecision.Enabled = false;
            this.grpDecision.Location = new System.Drawing.Point(12, 236);
            this.grpDecision.Name = "grpDecision";
            this.grpDecision.Size = new System.Drawing.Size(662, 265);
            this.grpDecision.TabIndex = 1;
            this.grpDecision.TabStop = false;
            this.grpDecision.Text = "Screening Decision";

            // ── lblSelectedApplicant ─────────────────────────────────────
            this.lblSelectedApplicant.AutoSize = true;
            this.lblSelectedApplicant.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblSelectedApplicant.Location = new System.Drawing.Point(10, 28);
            this.lblSelectedApplicant.Name = "lblSelectedApplicant";
            this.lblSelectedApplicant.TabIndex = 0;
            this.lblSelectedApplicant.Text = "Applicant: —";

            // ── lblSelectedJob ───────────────────────────────────────────
            this.lblSelectedJob.AutoSize = true;
            this.lblSelectedJob.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblSelectedJob.ForeColor = System.Drawing.Color.FromArgb(100, 100, 100);
            this.lblSelectedJob.Location = new System.Drawing.Point(10, 54);
            this.lblSelectedJob.Name = "lblSelectedJob";
            this.lblSelectedJob.TabIndex = 1;
            this.lblSelectedJob.Text = "Job: —";

            // ── lblDecisionStatic ────────────────────────────────────────
            this.lblDecisionStatic.AutoSize = true;
            this.lblDecisionStatic.Location = new System.Drawing.Point(10, 93);
            this.lblDecisionStatic.Name = "lblDecisionStatic";
            this.lblDecisionStatic.TabIndex = 2;
            this.lblDecisionStatic.Text = "Decision:";

            // ── rdoQualified ─────────────────────────────────────────────
            this.rdoQualified.AutoSize = true;
            this.rdoQualified.Checked = true;
            this.rdoQualified.Location = new System.Drawing.Point(85, 91);
            this.rdoQualified.Name = "rdoQualified";
            this.rdoQualified.Size = new System.Drawing.Size(90, 21);
            this.rdoQualified.TabIndex = 3;
            this.rdoQualified.TabStop = true;
            this.rdoQualified.Text = "Qualified";

            // ── rdoNotQualified ──────────────────────────────────────────
            this.rdoNotQualified.AutoSize = true;
            this.rdoNotQualified.Location = new System.Drawing.Point(185, 91);
            this.rdoNotQualified.Name = "rdoNotQualified";
            this.rdoNotQualified.Size = new System.Drawing.Size(110, 21);
            this.rdoNotQualified.TabIndex = 4;
            this.rdoNotQualified.Text = "Not Qualified";

            // ── lblRemarksStatic ─────────────────────────────────────────
            this.lblRemarksStatic.AutoSize = true;
            this.lblRemarksStatic.Location = new System.Drawing.Point(10, 126);
            this.lblRemarksStatic.Name = "lblRemarksStatic";
            this.lblRemarksStatic.TabIndex = 5;
            this.lblRemarksStatic.Text = "Remarks / HR Notes:";

            // ── txtRemarks ───────────────────────────────────────────────
            this.txtRemarks.Location = new System.Drawing.Point(10, 146);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRemarks.Size = new System.Drawing.Size(642, 72);
            this.txtRemarks.TabIndex = 6;

            // ── btnSave ──────────────────────────────────────────────────
            this.btnSave.Location = new System.Drawing.Point(455, 228);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(105, 30);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save Decision";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // ── btnBack ──────────────────────────────────────────────────
            this.btnBack.Location = new System.Drawing.Point(570, 228);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(82, 30);
            this.btnBack.TabIndex = 8;
            this.btnBack.Text = "Back";
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);

            // ── ScreeningForm ────────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 515);
            this.Controls.Add(this.grpApplications);
            this.Controls.Add(this.grpDecision);
            this.Name = "ScreeningForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Screening — HR Applicant System";
            this.Load += new System.EventHandler(this.ScreeningForm_Load);

            this.grpApplications.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvApplications)).EndInit();
            this.grpDecision.ResumeLayout(false);
            this.grpDecision.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox grpApplications;
        private System.Windows.Forms.DataGridView dgvApplications;
        private System.Windows.Forms.GroupBox grpDecision;
        private System.Windows.Forms.Label lblSelectedApplicant;
        private System.Windows.Forms.Label lblSelectedJob;
        private System.Windows.Forms.Label lblDecisionStatic;
        private System.Windows.Forms.RadioButton rdoQualified;
        private System.Windows.Forms.RadioButton rdoNotQualified;
        private System.Windows.Forms.Label lblRemarksStatic;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnBack;
    }
}
