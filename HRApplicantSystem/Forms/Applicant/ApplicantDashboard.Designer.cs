namespace HRApplicantSystem.Forms.Applicant
{
    partial class ApplicantDashboard
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
            this.panelSidebar = new System.Windows.Forms.Panel();
            this.lblPortalTitle = new System.Windows.Forms.Label();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.lblRoleBadge = new System.Windows.Forms.Label();
            this.btnProfile = new System.Windows.Forms.Button();
            this.btnJobVacancies = new System.Windows.Forms.Button();
            this.btnMyApplications = new System.Windows.Forms.Button();
            this.btnStatusTracking = new System.Windows.Forms.Button();
            this.btnDocuments = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblHeader = new System.Windows.Forms.Label();
            this.lblDateTime = new System.Windows.Forms.Label();
            this.panelContent = new System.Windows.Forms.Panel();
            this.panelCard1 = new System.Windows.Forms.Panel();
            this.panelCard1Accent = new System.Windows.Forms.Panel();
            this.lblCard1Title = new System.Windows.Forms.Label();
            this.lblCard1Value = new System.Windows.Forms.Label();
            this.panelCard2 = new System.Windows.Forms.Panel();
            this.panelCard2Accent = new System.Windows.Forms.Panel();
            this.lblCard2Title = new System.Windows.Forms.Label();
            this.lblCard2Value = new System.Windows.Forms.Label();
            this.panelCard3 = new System.Windows.Forms.Panel();
            this.panelCard3Accent = new System.Windows.Forms.Panel();
            this.lblCard3Title = new System.Windows.Forms.Label();
            this.lblCard3Value = new System.Windows.Forms.Label();
            this.lblMainTitle = new System.Windows.Forms.Label();
            this.lblMainDesc = new System.Windows.Forms.Label();
            this.timerDateTime = new System.Windows.Forms.Timer(this.components);
            this.panelSidebar.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.panelCard1.SuspendLayout();
            this.panelCard2.SuspendLayout();
            this.panelCard3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelSidebar
            // 
            this.panelSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(34)))), ((int)(((byte)(45)))));
            this.panelSidebar.Controls.Add(this.lblPortalTitle);
            this.panelSidebar.Controls.Add(this.lblWelcome);
            this.panelSidebar.Controls.Add(this.lblRoleBadge);
            this.panelSidebar.Controls.Add(this.btnProfile);
            this.panelSidebar.Controls.Add(this.btnJobVacancies);
            this.panelSidebar.Controls.Add(this.btnMyApplications);
            this.panelSidebar.Controls.Add(this.btnStatusTracking);
            this.panelSidebar.Controls.Add(this.btnDocuments);
            this.panelSidebar.Controls.Add(this.btnLogout);
            this.panelSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSidebar.Location = new System.Drawing.Point(0, 0);
            this.panelSidebar.Name = "panelSidebar";
            this.panelSidebar.Size = new System.Drawing.Size(260, 700);
            this.panelSidebar.TabIndex = 0;
            // 
            // lblPortalTitle
            // 
            this.lblPortalTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblPortalTitle.ForeColor = System.Drawing.Color.White;
            this.lblPortalTitle.Location = new System.Drawing.Point(15, 20);
            this.lblPortalTitle.Name = "lblPortalTitle";
            this.lblPortalTitle.Size = new System.Drawing.Size(230, 55);
            this.lblPortalTitle.TabIndex = 0;
            this.lblPortalTitle.Text = "Talent Acquisition\r\nOperations Portal";
            // 
            // lblWelcome
            // 
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblWelcome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(214)))), ((int)(((byte)(229)))));
            this.lblWelcome.Location = new System.Drawing.Point(15, 85);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(230, 50);
            this.lblWelcome.TabIndex = 1;
            this.lblWelcome.Text = "Welcome back,\r\nApplicant!";
            // 
            // lblRoleBadge
            // 
            this.lblRoleBadge.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.lblRoleBadge.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblRoleBadge.ForeColor = System.Drawing.Color.White;
            this.lblRoleBadge.Location = new System.Drawing.Point(15, 142);
            this.lblRoleBadge.Name = "lblRoleBadge";
            this.lblRoleBadge.Size = new System.Drawing.Size(110, 24);
            this.lblRoleBadge.TabIndex = 2;
            this.lblRoleBadge.Text = "APPLICANT";
            this.lblRoleBadge.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnProfile
            // 
            this.btnProfile.Location = new System.Drawing.Point(10, 180);
            this.btnProfile.Name = "btnProfile";
            this.btnProfile.Size = new System.Drawing.Size(240, 45);
            this.btnProfile.TabIndex = 3;
            this.btnProfile.Click += new System.EventHandler(this.btnProfile_Click);
            // 
            // btnJobVacancies
            // 
            this.btnJobVacancies.Location = new System.Drawing.Point(10, 230);
            this.btnJobVacancies.Name = "btnJobVacancies";
            this.btnJobVacancies.Size = new System.Drawing.Size(240, 45);
            this.btnJobVacancies.TabIndex = 4;
            this.btnJobVacancies.Click += new System.EventHandler(this.btnJobVacancies_Click);
            // 
            // btnMyApplications
            // 
            this.btnMyApplications.Location = new System.Drawing.Point(10, 280);
            this.btnMyApplications.Name = "btnMyApplications";
            this.btnMyApplications.Size = new System.Drawing.Size(240, 45);
            this.btnMyApplications.TabIndex = 5;
            this.btnMyApplications.Click += new System.EventHandler(this.btnMyApplications_Click);
            // 
            // btnStatusTracking
            // 
            this.btnStatusTracking.Location = new System.Drawing.Point(10, 330);
            this.btnStatusTracking.Name = "btnStatusTracking";
            this.btnStatusTracking.Size = new System.Drawing.Size(240, 45);
            this.btnStatusTracking.TabIndex = 6;
            this.btnStatusTracking.Click += new System.EventHandler(this.btnStatusTracking_Click);
            // 
            // btnDocuments
            // 
            this.btnDocuments.Location = new System.Drawing.Point(10, 380);
            this.btnDocuments.Name = "btnDocuments";
            this.btnDocuments.Size = new System.Drawing.Size(240, 45);
            this.btnDocuments.TabIndex = 7;
            this.btnDocuments.Click += new System.EventHandler(this.btnDocuments_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(10, 630);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(240, 45);
            this.btnLogout.TabIndex = 8;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.White;
            this.panelHeader.Controls.Add(this.lblHeader);
            this.panelHeader.Controls.Add(this.lblDateTime);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(260, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(840, 75);
            this.panelHeader.TabIndex = 1;
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold); // Scaled down to match administrative portal template
            this.lblHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(38)))), ((int)(((byte)(59))))); // Clean professional dark navy
            this.lblHeader.Location = new System.Drawing.Point(25, 25);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(437, 32);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "HR APPLICANT PROCESSING DASHBOARD";
            // 
            // lblDateTime
            // 
            this.lblDateTime.AutoSize = true;
            this.lblDateTime.Font = new System.Drawing.Font("Segoe UI", 9F); // Standard sizing matching template exactly
            this.lblDateTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.lblDateTime.Location = new System.Drawing.Point(550, 27);
            this.lblDateTime.Name = "lblDateTime";
            this.lblDateTime.Size = new System.Drawing.Size(250, 25);
            this.lblDateTime.TabIndex = 1;
            this.lblDateTime.Text = "Tuesday, June 09, 2026 7:41:57 AM";
            // 
            // panelContent
            // 
            this.panelContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.panelContent.Controls.Add(this.panelCard1);
            this.panelContent.Controls.Add(this.panelCard2);
            this.panelContent.Controls.Add(this.panelCard3);
            this.panelContent.Controls.Add(this.lblMainTitle);
            this.panelContent.Controls.Add(this.lblMainDesc);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(260, 75);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(840, 625);
            this.panelContent.TabIndex = 2;
            // 
            // panelCard1
            // 
            this.panelCard1.BackColor = System.Drawing.Color.White;
            this.panelCard1.Controls.Add(this.panelCard1Accent);
            this.panelCard1.Controls.Add(this.lblCard1Title);
            this.panelCard1.Controls.Add(this.lblCard1Value);
            this.panelCard1.Location = new System.Drawing.Point(25, 30);
            this.panelCard1.Name = "panelCard1";
            this.panelCard1.Size = new System.Drawing.Size(240, 95);
            this.panelCard1.TabIndex = 0;
            // 
            // panelCard1Accent
            // 
            this.panelCard1Accent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.panelCard1Accent.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelCard1Accent.Location = new System.Drawing.Point(0, 0);
            this.panelCard1Accent.Name = "panelCard1Accent";
            this.panelCard1Accent.Size = new System.Drawing.Size(6, 95);
            this.panelCard1Accent.TabIndex = 0;
            // 
            // lblCard1Title
            // 
            this.lblCard1Title.AutoSize = true;
            this.lblCard1Title.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblCard1Title.ForeColor = System.Drawing.Color.DarkGray;
            this.lblCard1Title.Location = new System.Drawing.Point(15, 12);
            this.lblCard1Title.Name = "lblCard1Title";
            this.lblCard1Title.Size = new System.Drawing.Size(161, 19);
            this.lblCard1Title.TabIndex = 1;
            this.lblCard1Title.Text = "TOTAL APPLICATIONS";
            // 
            // lblCard1Value
            // 
            this.lblCard1Value.AutoSize = true;
            this.lblCard1Value.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblCard1Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblCard1Value.Location = new System.Drawing.Point(12, 30);
            this.lblCard1Value.Name = "lblCard1Value";
            this.lblCard1Value.Size = new System.Drawing.Size(46, 54);
            this.lblCard1Value.TabIndex = 2;
            this.lblCard1Value.Text = "2";
            // 
            // panelCard2
            // 
            this.panelCard2.BackColor = System.Drawing.Color.White;
            this.panelCard2.Controls.Add(this.panelCard2Accent);
            this.panelCard2.Controls.Add(this.lblCard2Title);
            this.panelCard2.Controls.Add(this.lblCard2Value);
            this.panelCard2.Location = new System.Drawing.Point(290, 30);
            this.panelCard2.Name = "panelCard2";
            this.panelCard2.Size = new System.Drawing.Size(240, 95);
            this.panelCard2.TabIndex = 1;
            // 
            // panelCard2Accent
            // 
            this.panelCard2Accent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(126)))), ((int)(((byte)(34)))));
            this.panelCard2Accent.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelCard2Accent.Location = new System.Drawing.Point(0, 0);
            this.panelCard2Accent.Name = "panelCard2Accent";
            this.panelCard2Accent.Size = new System.Drawing.Size(6, 95);
            this.panelCard2Accent.TabIndex = 0;
            // 
            // lblCard2Title
            // 
            this.lblCard2Title.AutoSize = true;
            this.lblCard2Title.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblCard2Title.ForeColor = System.Drawing.Color.DarkGray;
            this.lblCard2Title.Location = new System.Drawing.Point(15, 12);
            this.lblCard2Title.Name = "lblCard2Title";
            this.lblCard2Title.Size = new System.Drawing.Size(167, 19);
            this.lblCard2Title.TabIndex = 1;
            this.lblCard2Title.Text = "ACTIVE APPLICATIONS";
            // 
            // lblCard2Value
            // 
            this.lblCard2Value.AutoSize = true;
            this.lblCard2Value.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblCard2Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblCard2Value.Location = new System.Drawing.Point(12, 30);
            this.lblCard2Value.Name = "lblCard2Value";
            this.lblCard2Value.Size = new System.Drawing.Size(46, 54);
            this.lblCard2Value.TabIndex = 2;
            this.lblCard2Value.Text = "1";
            // 
            // panelCard3
            // 
            this.panelCard3.BackColor = System.Drawing.Color.White;
            this.panelCard3.Controls.Add(this.panelCard3Accent);
            this.panelCard3.Controls.Add(this.lblCard3Title);
            this.panelCard3.Controls.Add(this.lblCard3Value);
            this.panelCard3.Location = new System.Drawing.Point(555, 30);
            this.panelCard3.Name = "panelCard3";
            this.panelCard3.Size = new System.Drawing.Size(240, 95);
            this.panelCard3.TabIndex = 2;
            // 
            // panelCard3Accent
            // 
            this.panelCard3Accent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.panelCard3Accent.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelCard3Accent.Location = new System.Drawing.Point(0, 0);
            this.panelCard3Accent.Name = "panelCard3Accent";
            this.panelCard3Accent.Size = new System.Drawing.Size(6, 95);
            this.panelCard3Accent.TabIndex = 0;
            // 
            // lblCard3Title
            // 
            this.lblCard3Title.AutoSize = true;
            this.lblCard3Title.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblCard3Title.ForeColor = System.Drawing.Color.DarkGray;
            this.lblCard3Title.Location = new System.Drawing.Point(15, 12);
            this.lblCard3Title.Name = "lblCard3Title";
            this.lblCard3Title.Size = new System.Drawing.Size(188, 19);
            this.lblCard3Title.TabIndex = 1;
            this.lblCard3Title.Text = "APPROVED APPLICATIONS";
            // 
            // lblCard3Value
            // 
            this.lblCard3Value.AutoSize = true;
            this.lblCard3Value.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblCard3Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblCard3Value.Location = new System.Drawing.Point(12, 30);
            this.lblCard3Value.Name = "lblCard3Value";
            this.lblCard3Value.Size = new System.Drawing.Size(46, 54);
            this.lblCard3Value.TabIndex = 2;
            this.lblCard3Value.Text = "0";
            // 
            // lblMainTitle
            // 
            this.lblMainTitle.AutoSize = true;
            this.lblMainTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblMainTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblMainTitle.Location = new System.Drawing.Point(25, 170);
            this.lblMainTitle.Name = "lblMainTitle";
            this.lblMainTitle.Size = new System.Drawing.Size(564, 41);
            this.lblMainTitle.TabIndex = 3;
            this.lblMainTitle.Text = "Talent Acquisition Recruitment Portal";
            // 
            // lblMainDesc
            // 
            this.lblMainDesc.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblMainDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.lblMainDesc.Location = new System.Drawing.Point(25, 225);
            this.lblMainDesc.Name = "lblMainDesc";
            this.lblMainDesc.Size = new System.Drawing.Size(770, 150);
            this.lblMainDesc.TabIndex = 4;
            this.lblMainDesc.Text = "Select an operation from the sidebar navigation panel on the left to launch the respective module. The Dashboard will temporarily close and return once you exit the selected module.";
            // 
            // timerDateTime
            // 
            this.timerDateTime.Interval = 1000;
            this.timerDateTime.Tick += new System.EventHandler(this.timerDateTime_Tick);
            // 
            // ApplicantDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1100, 700);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.panelSidebar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ApplicantDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Applicant Dashboard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ApplicantDashboard_FormClosing);
            this.Load += new System.EventHandler(this.ApplicantDashboard_Load);
            this.panelSidebar.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelContent.ResumeLayout(false);
            this.panelContent.PerformLayout();
            this.panelCard1.ResumeLayout(false);
            this.panelCard1.PerformLayout();
            this.panelCard2.ResumeLayout(false);
            this.panelCard2.PerformLayout();
            this.panelCard3.ResumeLayout(false);
            this.panelCard3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelSidebar;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Label lblPortalTitle;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Label lblRoleBadge;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblDateTime;
        private System.Windows.Forms.Button btnProfile;
        private System.Windows.Forms.Button btnJobVacancies;
        private System.Windows.Forms.Button btnMyApplications;
        private System.Windows.Forms.Button btnStatusTracking;
        private System.Windows.Forms.Button btnDocuments;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Panel panelCard1;
        private System.Windows.Forms.Panel panelCard1Accent;
        private System.Windows.Forms.Label lblCard1Title;
        private System.Windows.Forms.Label lblCard1Value;
        private System.Windows.Forms.Panel panelCard2;
        private System.Windows.Forms.Panel panelCard2Accent;
        private System.Windows.Forms.Label lblCard2Title;
        private System.Windows.Forms.Label lblCard2Value;
        private System.Windows.Forms.Panel panelCard3;
        private System.Windows.Forms.Panel panelCard3Accent;
        private System.Windows.Forms.Label lblCard3Title;
        private System.Windows.Forms.Label lblCard3Value;
        private System.Windows.Forms.Label lblMainTitle;
        private System.Windows.Forms.Label lblMainDesc;
        private System.Windows.Forms.Timer timerDateTime;
    }
}