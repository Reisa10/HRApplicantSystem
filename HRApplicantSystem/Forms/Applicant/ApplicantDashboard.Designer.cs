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
            this.btnDocuments = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnStatusTracking = new System.Windows.Forms.Button();
            this.btnMyApplications = new System.Windows.Forms.Button();
            this.btnJobVacancies = new System.Windows.Forms.Button();
            this.btnProfile = new System.Windows.Forms.Button();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnDocuments
            // 
            this.btnDocuments.Location = new System.Drawing.Point(68, 582);
            this.btnDocuments.Name = "btnDocuments";
            this.btnDocuments.Size = new System.Drawing.Size(352, 169);
            this.btnDocuments.TabIndex = 31;
            this.btnDocuments.Text = "MY DOCUMENTS\r\n\r\n[Upload and Edit Documents]\r\n";
            this.btnDocuments.UseVisualStyleBackColor = true;
            this.btnDocuments.Click += new System.EventHandler(this.btnDocuments_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(683, 37);
            this.btnLogout.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(135, 46);
            this.btnLogout.TabIndex = 30;
            this.btnLogout.Text = "LOGOUT";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnStatusTracking
            // 
            this.btnStatusTracking.Location = new System.Drawing.Point(466, 376);
            this.btnStatusTracking.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnStatusTracking.Name = "btnStatusTracking";
            this.btnStatusTracking.Size = new System.Drawing.Size(352, 169);
            this.btnStatusTracking.TabIndex = 29;
            this.btnStatusTracking.Text = "STATUS TRACKING\r\n\r\n[Timeline && Schedules]";
            this.btnStatusTracking.UseVisualStyleBackColor = true;
            this.btnStatusTracking.Click += new System.EventHandler(this.btnStatusTracking_Click);
            // 
            // btnMyApplications
            // 
            this.btnMyApplications.Location = new System.Drawing.Point(68, 376);
            this.btnMyApplications.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnMyApplications.Name = "btnMyApplications";
            this.btnMyApplications.Size = new System.Drawing.Size(352, 169);
            this.btnMyApplications.TabIndex = 28;
            this.btnMyApplications.Text = "MY APPLICATIONS\r\n\r\n[Track && Submit Drafts]";
            this.btnMyApplications.UseVisualStyleBackColor = true;
            this.btnMyApplications.Click += new System.EventHandler(this.btnMyApplications_Click);
            // 
            // btnJobVacancies
            // 
            this.btnJobVacancies.Location = new System.Drawing.Point(466, 176);
            this.btnJobVacancies.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnJobVacancies.Name = "btnJobVacancies";
            this.btnJobVacancies.Size = new System.Drawing.Size(352, 169);
            this.btnJobVacancies.TabIndex = 27;
            this.btnJobVacancies.Text = "JOB VACANCIES\r\n\r\n[Browse && Apply for Jobs]";
            this.btnJobVacancies.UseVisualStyleBackColor = true;
            this.btnJobVacancies.Click += new System.EventHandler(this.btnJobVacancies_Click);
            // 
            // btnProfile
            // 
            this.btnProfile.Location = new System.Drawing.Point(68, 176);
            this.btnProfile.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnProfile.Name = "btnProfile";
            this.btnProfile.Size = new System.Drawing.Size(352, 169);
            this.btnProfile.TabIndex = 26;
            this.btnProfile.Text = "MY PROFILE\r\n\r\n[View && Edit Personal Info]";
            this.btnProfile.UseVisualStyleBackColor = true;
            this.btnProfile.Click += new System.EventHandler(this.btnProfile_Click);
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblWelcome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.lblWelcome.Location = new System.Drawing.Point(56, 99);
            this.lblWelcome.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(161, 30);
            this.lblWelcome.TabIndex = 25;
            this.lblWelcome.Text = "Welcome back!";
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblHeader.Location = new System.Drawing.Point(53, 37);
            this.lblHeader.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(357, 48);
            this.lblHeader.TabIndex = 24;
            this.lblHeader.Text = "APPLICANT PORTAL";
            // 
            // ApplicantDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(870, 495);
            this.Controls.Add(this.btnDocuments);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnStatusTracking);
            this.Controls.Add(this.btnMyApplications);
            this.Controls.Add(this.btnJobVacancies);
            this.Controls.Add(this.btnProfile);
            this.Controls.Add(this.lblWelcome);
            this.Controls.Add(this.lblHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "ApplicantDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Applicant Dashboard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ApplicantDashboard_FormClosing);
            this.Load += new System.EventHandler(this.ApplicantDashboard_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDocuments;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnStatusTracking;
        private System.Windows.Forms.Button btnMyApplications;
        private System.Windows.Forms.Button btnJobVacancies;
        private System.Windows.Forms.Button btnProfile;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Label lblHeader;
    }
}