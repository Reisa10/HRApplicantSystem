namespace HRApplicantSystem.Forms.HR
{
    partial class ApplicantReviewForm
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.dgvApplications = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtReviewSkills = new System.Windows.Forms.TextBox();
            this.txtReviewEducation = new System.Windows.Forms.TextBox();
            this.txtReviewContact = new System.Windows.Forms.TextBox();
            this.txtReviewLastName = new System.Windows.Forms.TextBox();
            this.txtReviewFirstName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFirstName = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lstApplicantDocuments = new System.Windows.Forms.ListBox();
            this.btnLoadProfile = new System.Windows.Forms.Button();
            this.btnLoadDocuments = new System.Windows.Forms.Button();
            this.btnLockApplication = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvApplications)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(249, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(388, 31);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "APPLICANT REVIEW FORM";
            this.lblTitle.Click += new System.EventHandler(this.lblTitle_Click);
            // 
            // dgvApplications
            // 
            this.dgvApplications.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvApplications.Location = new System.Drawing.Point(69, 63);
            this.dgvApplications.Name = "dgvApplications";
            this.dgvApplications.RowHeadersWidth = 51;
            this.dgvApplications.RowTemplate.Height = 24;
            this.dgvApplications.Size = new System.Drawing.Size(755, 150);
            this.dgvApplications.TabIndex = 1;
            this.dgvApplications.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvApplications_CellClick);
            this.dgvApplications.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvApplications_CellContentClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtReviewSkills);
            this.groupBox1.Controls.Add(this.txtReviewEducation);
            this.groupBox1.Controls.Add(this.txtReviewContact);
            this.groupBox1.Controls.Add(this.txtReviewLastName);
            this.groupBox1.Controls.Add(this.txtReviewFirstName);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtFirstName);
            this.groupBox1.Location = new System.Drawing.Point(69, 242);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(337, 178);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Applicant Profile";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // txtReviewSkills
            // 
            this.txtReviewSkills.Location = new System.Drawing.Point(124, 139);
            this.txtReviewSkills.Name = "txtReviewSkills";
            this.txtReviewSkills.ReadOnly = true;
            this.txtReviewSkills.Size = new System.Drawing.Size(100, 22);
            this.txtReviewSkills.TabIndex = 9;
            // 
            // txtReviewEducation
            // 
            this.txtReviewEducation.Location = new System.Drawing.Point(124, 110);
            this.txtReviewEducation.Name = "txtReviewEducation";
            this.txtReviewEducation.ReadOnly = true;
            this.txtReviewEducation.Size = new System.Drawing.Size(100, 22);
            this.txtReviewEducation.TabIndex = 8;
            // 
            // txtReviewContact
            // 
            this.txtReviewContact.Location = new System.Drawing.Point(124, 81);
            this.txtReviewContact.Name = "txtReviewContact";
            this.txtReviewContact.ReadOnly = true;
            this.txtReviewContact.Size = new System.Drawing.Size(100, 22);
            this.txtReviewContact.TabIndex = 7;
            // 
            // txtReviewLastName
            // 
            this.txtReviewLastName.Location = new System.Drawing.Point(124, 51);
            this.txtReviewLastName.Name = "txtReviewLastName";
            this.txtReviewLastName.ReadOnly = true;
            this.txtReviewLastName.Size = new System.Drawing.Size(100, 22);
            this.txtReviewLastName.TabIndex = 6;
            // 
            // txtReviewFirstName
            // 
            this.txtReviewFirstName.Location = new System.Drawing.Point(124, 22);
            this.txtReviewFirstName.Name = "txtReviewFirstName";
            this.txtReviewFirstName.ReadOnly = true;
            this.txtReviewFirstName.Size = new System.Drawing.Size(100, 22);
            this.txtReviewFirstName.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 145);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 16);
            this.label5.TabIndex = 4;
            this.label5.Text = "Skills: ";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 116);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "Education: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Contact Number: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Last Name:";
            // 
            // txtFirstName
            // 
            this.txtFirstName.AutoSize = true;
            this.txtFirstName.Location = new System.Drawing.Point(6, 25);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(78, 16);
            this.txtFirstName.TabIndex = 0;
            this.txtFirstName.Text = "First Name: ";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lstApplicantDocuments);
            this.groupBox2.Location = new System.Drawing.Point(437, 254);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(364, 166);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Documents";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // lstApplicantDocuments
            // 
            this.lstApplicantDocuments.FormattingEnabled = true;
            this.lstApplicantDocuments.ItemHeight = 16;
            this.lstApplicantDocuments.Location = new System.Drawing.Point(7, 22);
            this.lstApplicantDocuments.Name = "lstApplicantDocuments";
            this.lstApplicantDocuments.Size = new System.Drawing.Size(351, 132);
            this.lstApplicantDocuments.TabIndex = 0;
            // 
            // btnLoadProfile
            // 
            this.btnLoadProfile.Location = new System.Drawing.Point(69, 444);
            this.btnLoadProfile.Name = "btnLoadProfile";
            this.btnLoadProfile.Size = new System.Drawing.Size(146, 23);
            this.btnLoadProfile.TabIndex = 4;
            this.btnLoadProfile.Text = "View Profile";
            this.btnLoadProfile.UseVisualStyleBackColor = true;
            this.btnLoadProfile.Click += new System.EventHandler(this.btnLoadProfile_Click);
            // 
            // btnLoadDocuments
            // 
            this.btnLoadDocuments.Location = new System.Drawing.Point(69, 474);
            this.btnLoadDocuments.Name = "btnLoadDocuments";
            this.btnLoadDocuments.Size = new System.Drawing.Size(146, 23);
            this.btnLoadDocuments.TabIndex = 5;
            this.btnLoadDocuments.Text = "View Documents";
            this.btnLoadDocuments.UseVisualStyleBackColor = true;
            this.btnLoadDocuments.Click += new System.EventHandler(this.btnLoadDocuments_Click);
            // 
            // btnLockApplication
            // 
            this.btnLockApplication.Location = new System.Drawing.Point(69, 504);
            this.btnLockApplication.Name = "btnLockApplication";
            this.btnLockApplication.Size = new System.Drawing.Size(146, 23);
            this.btnLockApplication.TabIndex = 6;
            this.btnLockApplication.Text = "Lock Application";
            this.btnLockApplication.UseVisualStyleBackColor = true;
            this.btnLockApplication.Click += new System.EventHandler(this.btnLockApplication_Click);
            // 
            // ApplicantReviewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(877, 573);
            this.Controls.Add(this.btnLockApplication);
            this.Controls.Add(this.btnLoadDocuments);
            this.Controls.Add(this.btnLoadProfile);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dgvApplications);
            this.Controls.Add(this.lblTitle);
            this.Name = "ApplicantReviewForm";
            this.Text = "ApplicantReviewForm";
            this.Load += new System.EventHandler(this.ApplicantReviewForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvApplications)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DataGridView dgvApplications;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label txtFirstName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtReviewSkills;
        private System.Windows.Forms.TextBox txtReviewEducation;
        private System.Windows.Forms.TextBox txtReviewContact;
        private System.Windows.Forms.TextBox txtReviewLastName;
        private System.Windows.Forms.TextBox txtReviewFirstName;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox lstApplicantDocuments;
        private System.Windows.Forms.Button btnLoadProfile;
        private System.Windows.Forms.Button btnLoadDocuments;
        private System.Windows.Forms.Button btnLockApplication;
    }
}