namespace HRApplicantSystem.Forms.HR
{
    partial class ReportsForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.grpReportType = new System.Windows.Forms.GroupBox();
            this.rdoAllApplicants = new System.Windows.Forms.RadioButton();
            this.rdoPending = new System.Windows.Forms.RadioButton();
            this.rdoInterviews = new System.Windows.Forms.RadioButton();
            this.rdoHired = new System.Windows.Forms.RadioButton();
            this.rdoMissingDocs = new System.Windows.Forms.RadioButton();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnExportCSV = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblReportTitle = new System.Windows.Forms.Label();
            this.dgvReport = new System.Windows.Forms.DataGridView();
            this.lblStatus = new System.Windows.Forms.Label();
            this.grpReportType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
            this.SuspendLayout();

            // ── grpReportType ──────────────────────────────────
            this.grpReportType.Controls.Add(this.rdoAllApplicants);
            this.grpReportType.Controls.Add(this.rdoPending);
            this.grpReportType.Controls.Add(this.rdoInterviews);
            this.grpReportType.Controls.Add(this.rdoHired);
            this.grpReportType.Controls.Add(this.rdoMissingDocs);
            this.grpReportType.Location = new System.Drawing.Point(12, 12);
            this.grpReportType.Name = "grpReportType";
            this.grpReportType.Size = new System.Drawing.Size(205, 175);
            this.grpReportType.TabIndex = 0;
            this.grpReportType.TabStop = false;
            this.grpReportType.Text = "Select Report Type";

            // ── rdoAllApplicants ───────────────────────────────
            this.rdoAllApplicants.AutoSize = true;
            this.rdoAllApplicants.Location = new System.Drawing.Point(12, 25);
            this.rdoAllApplicants.Name = "rdoAllApplicants";
            this.rdoAllApplicants.TabIndex = 0;
            this.rdoAllApplicants.Text = "All Applicants";

            // ── rdoPending ─────────────────────────────────────
            this.rdoPending.AutoSize = true;
            this.rdoPending.Location = new System.Drawing.Point(12, 55);
            this.rdoPending.Name = "rdoPending";
            this.rdoPending.TabIndex = 1;
            this.rdoPending.Text = "Pending Applications";

            // ── rdoInterviews ──────────────────────────────────
            this.rdoInterviews.AutoSize = true;
            this.rdoInterviews.Location = new System.Drawing.Point(12, 85);
            this.rdoInterviews.Name = "rdoInterviews";
            this.rdoInterviews.TabIndex = 2;
            this.rdoInterviews.Text = "Scheduled Interviews";

            // ── rdoHired ───────────────────────────────────────
            this.rdoHired.AutoSize = true;
            this.rdoHired.Location = new System.Drawing.Point(12, 115);
            this.rdoHired.Name = "rdoHired";
            this.rdoHired.TabIndex = 3;
            this.rdoHired.Text = "Accepted / Rejected";

            // ── rdoMissingDocs ─────────────────────────────────
            this.rdoMissingDocs.AutoSize = true;
            this.rdoMissingDocs.Location = new System.Drawing.Point(12, 145);
            this.rdoMissingDocs.Name = "rdoMissingDocs";
            this.rdoMissingDocs.TabIndex = 4;
            this.rdoMissingDocs.Text = "Missing Requirements";

            // ── btnGenerate ────────────────────────────────────
            this.btnGenerate.Location = new System.Drawing.Point(12, 200);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(205, 35);
            this.btnGenerate.TabIndex = 5;
            this.btnGenerate.Text = "Generate Report";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);

            // ── btnExportCSV ───────────────────────────────────
            this.btnExportCSV.Location = new System.Drawing.Point(12, 245);
            this.btnExportCSV.Name = "btnExportCSV";
            this.btnExportCSV.Size = new System.Drawing.Size(205, 35);
            this.btnExportCSV.TabIndex = 6;
            this.btnExportCSV.Text = "Export to CSV";
            this.btnExportCSV.UseVisualStyleBackColor = true;
            this.btnExportCSV.Click += new System.EventHandler(this.btnExportCSV_Click);

            // ── btnClose ───────────────────────────────────────
            this.btnClose.Location = new System.Drawing.Point(12, 290);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(205, 35);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);

            // ── lblReportTitle ─────────────────────────────────
            this.lblReportTitle.AutoSize = false;
            this.lblReportTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblReportTitle.Location = new System.Drawing.Point(228, 12);
            this.lblReportTitle.Name = "lblReportTitle";
            this.lblReportTitle.Size = new System.Drawing.Size(720, 30);
            this.lblReportTitle.TabIndex = 8;
            this.lblReportTitle.Text = "Select a report type and click Generate Report";

            // ── dgvReport ──────────────────────────────────────
            this.dgvReport.AllowUserToAddRows = false;
            this.dgvReport.AllowUserToDeleteRows = false;
            this.dgvReport.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvReport.ColumnHeadersHeightSizeMode =
                System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReport.Location = new System.Drawing.Point(228, 50);
            this.dgvReport.Name = "dgvReport";
            this.dgvReport.ReadOnly = true;
            this.dgvReport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvReport.Size = new System.Drawing.Size(720, 490);
            this.dgvReport.TabIndex = 9;

            // ── lblStatus ──────────────────────────────────────
            this.lblStatus.AutoSize = true;
            this.lblStatus.ForeColor = System.Drawing.Color.Gray;
            this.lblStatus.Location = new System.Drawing.Point(228, 548);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.TabIndex = 10;
            this.lblStatus.Text = "No report generated yet.";

            // ── ReportsForm ────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(964, 575);
            this.Controls.Add(this.grpReportType);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.btnExportCSV);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblReportTitle);
            this.Controls.Add(this.dgvReport);
            this.Controls.Add(this.lblStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ReportsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HR Reports";
            this.Load += new System.EventHandler(this.ReportsForm_Load);

            this.grpReportType.ResumeLayout(false);
            this.grpReportType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        // ── Field declarations ─────────────────────────────────
        private System.Windows.Forms.GroupBox grpReportType;
        private System.Windows.Forms.RadioButton rdoAllApplicants;
        private System.Windows.Forms.RadioButton rdoPending;
        private System.Windows.Forms.RadioButton rdoInterviews;
        private System.Windows.Forms.RadioButton rdoHired;
        private System.Windows.Forms.RadioButton rdoMissingDocs;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button btnExportCSV;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblReportTitle;
        private System.Windows.Forms.DataGridView dgvReport;
        private System.Windows.Forms.Label lblStatus;
    }
}
