namespace HRApplicantSystem.Forms.HR
{
    partial class HRDashboard
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
            this.components = new System.ComponentModel.Container();
            this.sidebarPanel = new System.Windows.Forms.Panel();
            this.profilePanel = new System.Windows.Forms.Panel();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.lblUserFullName = new System.Windows.Forms.Label();
            this.lblUserRole = new System.Windows.Forms.Label();
            this.btnHome = new System.Windows.Forms.Button();
            this.ApplicantReview = new System.Windows.Forms.Button();
            this.Screening = new System.Windows.Forms.Button();
            this.InterviewSchedule = new System.Windows.Forms.Button();
            this.InterviewEvaluation = new System.Windows.Forms.Button();
            this.HiringDecision = new System.Windows.Forms.Button();
            this.JobVacancyManagement = new System.Windows.Forms.Button();
            this.SystemMaintenance = new System.Windows.Forms.Button();
            this.Reports = new System.Windows.Forms.Button();
            this.Logout = new System.Windows.Forms.Button();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.lblHeaderTitle = new System.Windows.Forms.Label();
            this.lblDateTime = new System.Windows.Forms.Label();
            this.workspaceContainerPanel = new System.Windows.Forms.Panel();
            this.SystemClockTimer = new System.Windows.Forms.Timer(this.components);
            this.sidebarPanel.SuspendLayout();
            this.profilePanel.SuspendLayout();
            this.headerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // sidebarPanel
            // 
            this.sidebarPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(34)))), ((int)(((byte)(45)))));
            this.sidebarPanel.Controls.Add(this.Logout);
            this.sidebarPanel.Controls.Add(this.Reports);
            this.sidebarPanel.Controls.Add(this.SystemMaintenance);
            this.sidebarPanel.Controls.Add(this.JobVacancyManagement);
            this.sidebarPanel.Controls.Add(this.HiringDecision);
            this.sidebarPanel.Controls.Add(this.InterviewEvaluation);
            this.sidebarPanel.Controls.Add(this.InterviewSchedule);
            this.sidebarPanel.Controls.Add(this.Screening);
            this.sidebarPanel.Controls.Add(this.ApplicantReview);
            this.sidebarPanel.Controls.Add(this.btnHome);
            this.sidebarPanel.Controls.Add(this.profilePanel);
            this.sidebarPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidebarPanel.Location = new System.Drawing.Point(0, 0);
            this.sidebarPanel.Name = "sidebarPanel";
            this.sidebarPanel.Size = new System.Drawing.Size(260, 700);
            this.sidebarPanel.TabIndex = 0;
            // 
            // profilePanel
            // 
            this.profilePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(27)))), ((int)(((byte)(36)))));
            this.profilePanel.Controls.Add(this.lblUserRole);
            this.profilePanel.Controls.Add(this.lblUserFullName);
            this.profilePanel.Controls.Add(this.lblWelcome);
            this.profilePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.profilePanel.Location = new System.Drawing.Point(0, 0);
            this.profilePanel.Name = "profilePanel";
            this.profilePanel.Size = new System.Drawing.Size(260, 110);
            this.profilePanel.TabIndex = 0;
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblWelcome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(214)))), ((int)(((byte)(229)))));
            this.lblWelcome.Location = new System.Drawing.Point(15, 18);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(107, 20);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "Welcome back,";
            // 
            // lblUserFullName
            // 
            this.lblUserFullName.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblUserFullName.ForeColor = System.Drawing.Color.White;
            this.lblUserFullName.Location = new System.Drawing.Point(14, 38);
            this.lblUserFullName.Name = "lblUserFullName";
            this.lblUserFullName.Size = new System.Drawing.Size(230, 25);
            this.lblUserFullName.TabIndex = 1;
            this.lblUserFullName.Text = "System User";
            // 
            // lblUserRole
            // 
            this.lblUserRole.AutoSize = true;
            this.lblUserRole.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.lblUserRole.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Bold);
            this.lblUserRole.ForeColor = System.Drawing.Color.White;
            this.lblUserRole.Location = new System.Drawing.Point(16, 68);
            this.lblUserRole.Padding = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.lblUserRole.Name = "lblUserRole";
            this.lblUserRole.Size = new System.Drawing.Size(89, 23);
            this.lblUserRole.TabIndex = 2;
            this.lblUserRole.Text = "SPECIALIST";
            this.lblUserRole.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnHome
            // 
            this.btnHome.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnHome.FlatAppearance.BorderSize = 0;
            this.btnHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHome.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnHome.ForeColor = System.Drawing.Color.White;
            this.btnHome.Location = new System.Drawing.Point(0, 110);
            this.btnHome.Name = "btnHome";
            this.btnHome.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnHome.Size = new System.Drawing.Size(260, 44);
            this.btnHome.TabIndex = 1;
            this.btnHome.Text = "🏠  Dashboard Home";
            this.btnHome.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHome.UseVisualStyleBackColor = true;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // ApplicantReview
            // 
            this.ApplicantReview.Dock = System.Windows.Forms.DockStyle.Top;
            this.ApplicantReview.FlatAppearance.BorderSize = 0;
            this.ApplicantReview.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ApplicantReview.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.ApplicantReview.ForeColor = System.Drawing.Color.Gainsboro;
            this.ApplicantReview.Location = new System.Drawing.Point(0, 154);
            this.ApplicantReview.Name = "ApplicantReview";
            this.ApplicantReview.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.ApplicantReview.Size = new System.Drawing.Size(260, 44);
            this.ApplicantReview.TabIndex = 2;
            this.ApplicantReview.Text = "👥  Applicant Review";
            this.ApplicantReview.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ApplicantReview.UseVisualStyleBackColor = true;
            this.ApplicantReview.Click += new System.EventHandler(this.ApplicantReview_Click);
            // 
            // Screening
            // 
            this.Screening.Dock = System.Windows.Forms.DockStyle.Top;
            this.Screening.FlatAppearance.BorderSize = 0;
            this.Screening.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Screening.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.Screening.ForeColor = System.Drawing.Color.Gainsboro;
            this.Screening.Location = new System.Drawing.Point(0, 198);
            this.Screening.Name = "Screening";
            this.Screening.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.Screening.Size = new System.Drawing.Size(260, 44);
            this.Screening.TabIndex = 3;
            this.Screening.Text = "🔍  Basic Screening";
            this.Screening.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Screening.UseVisualStyleBackColor = true;
            this.Screening.Click += new System.EventHandler(this.Screening_Click);
            // 
            // InterviewSchedule
            // 
            this.InterviewSchedule.Dock = System.Windows.Forms.DockStyle.Top;
            this.InterviewSchedule.FlatAppearance.BorderSize = 0;
            this.InterviewSchedule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.InterviewSchedule.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.InterviewSchedule.ForeColor = System.Drawing.Color.Gainsboro;
            this.InterviewSchedule.Location = new System.Drawing.Point(0, 242);
            this.InterviewSchedule.Name = "InterviewSchedule";
            this.InterviewSchedule.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.InterviewSchedule.Size = new System.Drawing.Size(260, 44);
            this.InterviewSchedule.TabIndex = 4;
            this.InterviewSchedule.Text = "📅  Interview Schedule";
            this.InterviewSchedule.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.InterviewSchedule.UseVisualStyleBackColor = true;
            this.InterviewSchedule.Click += new System.EventHandler(this.InterviewSchedule_Click);
            // 
            // InterviewEvaluation
            // 
            this.InterviewEvaluation.Dock = System.Windows.Forms.DockStyle.Top;
            this.InterviewEvaluation.FlatAppearance.BorderSize = 0;
            this.InterviewEvaluation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.InterviewEvaluation.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.InterviewEvaluation.ForeColor = System.Drawing.Color.Gainsboro;
            this.InterviewEvaluation.Location = new System.Drawing.Point(0, 286);
            this.InterviewEvaluation.Name = "InterviewEvaluation";
            this.InterviewEvaluation.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.InterviewEvaluation.Size = new System.Drawing.Size(260, 44);
            this.InterviewEvaluation.TabIndex = 5;
            this.InterviewEvaluation.Text = "📝  Interview Evaluation";
            this.InterviewEvaluation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.InterviewEvaluation.UseVisualStyleBackColor = true;
            this.InterviewEvaluation.Click += new System.EventHandler(this.InterviewEvaluation_Click);
            // 
            // HiringDecision
            // 
            this.HiringDecision.Dock = System.Windows.Forms.DockStyle.Top;
            this.HiringDecision.FlatAppearance.BorderSize = 0;
            this.HiringDecision.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.HiringDecision.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.HiringDecision.ForeColor = System.Drawing.Color.Gainsboro;
            this.HiringDecision.Location = new System.Drawing.Point(0, 330);
            this.HiringDecision.Name = "HiringDecision";
            this.HiringDecision.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.HiringDecision.Size = new System.Drawing.Size(260, 44);
            this.HiringDecision.TabIndex = 6;
            this.HiringDecision.Text = "⚖️  Hiring Decisions";
            this.HiringDecision.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.HiringDecision.UseVisualStyleBackColor = true;
            this.HiringDecision.Click += new System.EventHandler(this.HiringDecision_Click);
            // 
            // JobVacancyManagement
            // 
            this.JobVacancyManagement.Dock = System.Windows.Forms.DockStyle.Top;
            this.JobVacancyManagement.FlatAppearance.BorderSize = 0;
            this.JobVacancyManagement.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.JobVacancyManagement.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.JobVacancyManagement.ForeColor = System.Drawing.Color.Gainsboro;
            this.JobVacancyManagement.Location = new System.Drawing.Point(0, 374);
            this.JobVacancyManagement.Name = "JobVacancyManagement";
            this.JobVacancyManagement.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.JobVacancyManagement.Size = new System.Drawing.Size(260, 44);
            this.JobVacancyManagement.TabIndex = 7;
            this.JobVacancyManagement.Text = "💼  Job Vacancies";
            this.JobVacancyManagement.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.JobVacancyManagement.UseVisualStyleBackColor = true;
            this.JobVacancyManagement.Click += new System.EventHandler(this.JobVacancyManagement_Click);
            // 
            // SystemMaintenance
            // 
            this.SystemMaintenance.Dock = System.Windows.Forms.DockStyle.Top;
            this.SystemMaintenance.FlatAppearance.BorderSize = 0;
            this.SystemMaintenance.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SystemMaintenance.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.SystemMaintenance.ForeColor = System.Drawing.Color.Gainsboro;
            this.SystemMaintenance.Location = new System.Drawing.Point(0, 418);
            this.SystemMaintenance.Name = "SystemMaintenance";
            this.SystemMaintenance.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.SystemMaintenance.Size = new System.Drawing.Size(260, 44);
            this.SystemMaintenance.TabIndex = 8;
            this.SystemMaintenance.Text = "⚙️  System Maintenance";
            this.SystemMaintenance.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.SystemMaintenance.UseVisualStyleBackColor = true;
            this.SystemMaintenance.Click += new System.EventHandler(this.SystemMaintenance_Click);
            // 
            // Reports
            // 
            this.Reports.Dock = System.Windows.Forms.DockStyle.Top;
            this.Reports.FlatAppearance.BorderSize = 0;
            this.Reports.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Reports.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.Reports.ForeColor = System.Drawing.Color.Gainsboro;
            this.Reports.Location = new System.Drawing.Point(0, 462);
            this.Reports.Name = "Reports";
            this.Reports.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.Reports.Size = new System.Drawing.Size(260, 44);
            this.Reports.TabIndex = 9;
            this.Reports.Text = "📊  Recruitment Reports";
            this.Reports.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Reports.UseVisualStyleBackColor = true;
            this.Reports.Click += new System.EventHandler(this.Reports_Click);
            // 
            // Logout
            // 
            this.Logout.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Logout.FlatAppearance.BorderSize = 0;
            this.Logout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Logout.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.Logout.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.Logout.Location = new System.Drawing.Point(0, 656);
            this.Logout.Name = "Logout";
            this.Logout.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.Logout.Size = new System.Drawing.Size(260, 44);
            this.Logout.TabIndex = 10;
            this.Logout.Text = "🚪  Sign Out";
            this.Logout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Logout.UseVisualStyleBackColor = true;
            this.Logout.Click += new System.EventHandler(this.Logout_Click);
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.White;
            this.headerPanel.Controls.Add(this.lblDateTime);
            this.headerPanel.Controls.Add(this.lblHeaderTitle);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(260, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(840, 75);
            this.headerPanel.TabIndex = 1;
            // 
            // lblHeaderTitle
            // 
            this.lblHeaderTitle.AutoSize = true;
            this.lblHeaderTitle.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold);
            this.lblHeaderTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(38)))), ((int)(((byte)(59)))));
            this.lblHeaderTitle.Location = new System.Drawing.Point(25, 23);
            this.lblHeaderTitle.Name = "lblHeaderTitle";
            this.lblHeaderTitle.Size = new System.Drawing.Size(390, 25);
            this.lblHeaderTitle.TabIndex = 0;
            this.lblHeaderTitle.Text = "HR APPLICANT PROCESSING DASHBOARD";
            // 
            // lblDateTime
            // 
            this.lblDateTime.AutoSize = true;
            this.lblDateTime.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDateTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.lblDateTime.Location = new System.Drawing.Point(550, 27);
            this.lblDateTime.Name = "lblDateTime";
            this.lblDateTime.Size = new System.Drawing.Size(250, 23);
            this.lblDateTime.TabIndex = 1;
            this.lblDateTime.Text = "Live System Clock";
            // 
            // workspaceContainerPanel
            // 
            this.workspaceContainerPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.workspaceContainerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.workspaceContainerPanel.Location = new System.Drawing.Point(260, 75);
            this.workspaceContainerPanel.Name = "workspaceContainerPanel";
            this.workspaceContainerPanel.Size = new System.Drawing.Size(840, 625);
            this.workspaceContainerPanel.TabIndex = 2;
            // 
            // SystemClockTimer
            // 
            this.SystemClockTimer.Interval = 1000;
            this.SystemClockTimer.Tick += new System.EventHandler(this.SystemClockTimer_Tick);
            // 
            // HRDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 700);
            this.Controls.Add(this.workspaceContainerPanel);
            this.Controls.Add(this.headerPanel);
            this.Controls.Add(this.sidebarPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "HRDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Talent Acquisition Operations Portal";
            this.Load += new System.EventHandler(this.HRDashboard_Load);
            this.sidebarPanel.ResumeLayout(false);
            this.profilePanel.ResumeLayout(false);
            this.profilePanel.PerformLayout();
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel sidebarPanel;
        private System.Windows.Forms.Panel profilePanel;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Label lblUserFullName;
        private System.Windows.Forms.Label lblUserRole;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Button ApplicantReview;
        private System.Windows.Forms.Button Screening;
        private System.Windows.Forms.Button InterviewSchedule;
        private System.Windows.Forms.Button InterviewEvaluation;
        private System.Windows.Forms.Button HiringDecision;
        private System.Windows.Forms.Button JobVacancyManagement;
        private System.Windows.Forms.Button SystemMaintenance;
        private System.Windows.Forms.Button Reports;
        private System.Windows.Forms.Button Logout;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label lblHeaderTitle;
        private System.Windows.Forms.Label lblDateTime;
        private System.Windows.Forms.Panel workspaceContainerPanel;
        private System.Windows.Forms.Timer SystemClockTimer;
    }
}