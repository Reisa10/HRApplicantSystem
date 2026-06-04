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
            this.lblHeader = new System.Windows.Forms.Label();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.btnProfile = new System.Windows.Forms.Button();
            this.btnJobVacancies = new System.Windows.Forms.Button();
            this.btnMyApplications = new System.Windows.Forms.Button();
            this.btnStatusTracking = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblHeader.Location = new System.Drawing.Point(30, 25);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(248, 32);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "APPLICANT PORTAL";
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblWelcome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.lblWelcome.Location = new System.Drawing.Point(32, 65);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(110, 20);
            this.lblWelcome.TabIndex = 1;
            this.lblWelcome.Text = "Welcome back!";
            // 
            // btnProfile
            // 
            this.btnProfile.Location = new System.Drawing.Point(40, 115);
            this.btnProfile.Name = "btnProfile";
            this.btnProfile.Size = new System.Drawing.Size(235, 110);
            this.btnProfile.TabIndex = 2;
            this.btnProfile.Text = "MY PROFILE\r\n\r\n[View && Edit Personal Info]";
            this.btnProfile.UseVisualStyleBackColor = true;
            this.btnProfile.Click += new System.EventHandler(this.btnProfile_Click);
            // 
            // btnJobVacancies
            // 
            this.btnJobVacancies.Location = new System.Drawing.Point(305, 115);
            this.btnJobVacancies.Name = "btnJobVacancies";
            this.btnJobVacancies.Size = new System.Drawing.Size(235, 110);
            this.btnJobVacancies.TabIndex = 3;
            this.btnJobVacancies.Text = "JOB VACANCIES\r\n\r\n[Browse && Apply for Jobs]";
            this.btnJobVacancies.UseVisualStyleBackColor = true;
            this.btnJobVacancies.Click += new System.EventHandler(this.btnJobVacancies_Click);
            // 
            // btnMyApplications
            // 
            this.btnMyApplications.Location = new System.Drawing.Point(40, 245);
            this.btnMyApplications.Name = "btnMyApplications";
            this.btnMyApplications.Size = new System.Drawing.Size(235, 110);
            this.btnMyApplications.TabIndex = 4;
            this.btnMyApplications.Text = "MY APPLICATIONS\r\n\r\n[Track && Submit Drafts]";
            this.btnMyApplications.UseVisualStyleBackColor = true;
            this.btnMyApplications.Click += new System.EventHandler(this.btnMyApplications_Click);
            // 
            // btnStatusTracking
            // 
            this.btnStatusTracking.Location = new System.Drawing.Point(305, 245);
            this.btnStatusTracking.Name = "btnStatusTracking";
            this.btnStatusTracking.Size = new System.Drawing.Size(235, 110);
            this.btnStatusTracking.TabIndex = 5;
            this.btnStatusTracking.Text = "STATUS TRACKING\r\n\r\n[Timeline && Schedules]";
            this.btnStatusTracking.UseVisualStyleBackColor = true;
            this.btnStatusTracking.Click += new System.EventHandler(this.btnStatusTracking_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(450, 25);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(90, 30);
            this.btnLogout.TabIndex = 6;
            this.btnLogout.Text = "LOGOUT";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // ApplicantDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 395);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnStatusTracking);
            this.Controls.Add(this.btnMyApplications);
            this.Controls.Add(this.btnJobVacancies);
            this.Controls.Add(this.btnProfile);
            this.Controls.Add(this.lblWelcome);
            this.Controls.Add(this.lblHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
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

        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Button btnProfile;
        private System.Windows.Forms.Button btnJobVacancies;
        private System.Windows.Forms.Button btnMyApplications;
        private System.Windows.Forms.Button btnStatusTracking;
        private System.Windows.Forms.Button btnLogout;
    }
}