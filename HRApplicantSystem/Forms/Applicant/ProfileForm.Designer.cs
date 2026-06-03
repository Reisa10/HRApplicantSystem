namespace ProfileForm
{
    partial class ProfileForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtFirstName = new TextBox();
            CreateProfLabel = new Label();
            txtLastName = new TextBox();
            dtpBirthdate = new DateTimePicker();
            cmbGender = new ComboBox();
            txtPhoneNum = new TextBox();
            txtAddress = new TextBox();
            labelEduc = new Label();
            labelSkills = new Label();
            txtEduc = new RichTextBox();
            txtSkills = new RichTextBox();
            btnSave = new Button();
            btnUpdate = new Button();
            btnClear = new Button();
            txtApplicantID = new TextBox();
            labelExp = new Label();
            txtWorkExp = new RichTextBox();
            SuspendLayout();
            // 
            // txtFirstName
            // 
            txtFirstName.ForeColor = SystemColors.GrayText;
            txtFirstName.Location = new Point(256, 87);
            txtFirstName.Name = "txtFirstName";
            txtFirstName.Size = new Size(234, 31);
            txtFirstName.TabIndex = 0;
            txtFirstName.Text = "First Name";
            // 
            // CreateProfLabel
            // 
            CreateProfLabel.AutoSize = true;
            CreateProfLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            CreateProfLabel.Location = new Point(246, 9);
            CreateProfLabel.Name = "CreateProfLabel";
            CreateProfLabel.Size = new Size(264, 38);
            CreateProfLabel.TabIndex = 1;
            CreateProfLabel.Text = "Create your Profile";
            CreateProfLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtLastName
            // 
            txtLastName.ForeColor = SystemColors.GrayText;
            txtLastName.Location = new Point(256, 124);
            txtLastName.Name = "txtLastName";
            txtLastName.Size = new Size(234, 31);
            txtLastName.TabIndex = 2;
            txtLastName.Text = "Last Name";
            // 
            // dtpBirthdate
            // 
            dtpBirthdate.CalendarForeColor = SystemColors.GrayText;
            dtpBirthdate.Location = new Point(137, 162);
            dtpBirthdate.Name = "dtpBirthdate";
            dtpBirthdate.Size = new Size(300, 31);
            dtpBirthdate.TabIndex = 4;
            // 
            // cmbGender
            // 
            cmbGender.ForeColor = SystemColors.GrayText;
            cmbGender.FormattingEnabled = true;
            cmbGender.Items.AddRange(new object[] { "Male", "Female" });
            cmbGender.Location = new Point(452, 160);
            cmbGender.Name = "cmbGender";
            cmbGender.Size = new Size(146, 33);
            cmbGender.TabIndex = 6;
            cmbGender.Text = "Gender";
            // 
            // txtPhoneNum
            // 
            txtPhoneNum.ForeColor = SystemColors.GrayText;
            txtPhoneNum.Location = new Point(256, 199);
            txtPhoneNum.Name = "txtPhoneNum";
            txtPhoneNum.Size = new Size(234, 31);
            txtPhoneNum.TabIndex = 9;
            txtPhoneNum.Text = "Phone Number";
            // 
            // txtAddress
            // 
            txtAddress.ForeColor = SystemColors.GrayText;
            txtAddress.Location = new Point(256, 236);
            txtAddress.Name = "txtAddress";
            txtAddress.Size = new Size(234, 31);
            txtAddress.TabIndex = 11;
            txtAddress.Text = "Address";
            // 
            // labelEduc
            // 
            labelEduc.AutoSize = true;
            labelEduc.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelEduc.Location = new Point(48, 286);
            labelEduc.Name = "labelEduc";
            labelEduc.Size = new Size(147, 38);
            labelEduc.TabIndex = 16;
            labelEduc.Text = "Education";
            labelEduc.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // labelSkills
            // 
            labelSkills.AutoSize = true;
            labelSkills.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelSkills.Location = new Point(638, 286);
            labelSkills.Name = "labelSkills";
            labelSkills.Size = new Size(85, 38);
            labelSkills.TabIndex = 17;
            labelSkills.Text = "Skills";
            labelSkills.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtEduc
            // 
            txtEduc.Location = new Point(12, 339);
            txtEduc.Name = "txtEduc";
            txtEduc.Size = new Size(215, 144);
            txtEduc.TabIndex = 18;
            txtEduc.Text = "";
            // 
            // txtSkills
            // 
            txtSkills.Location = new Point(569, 339);
            txtSkills.Name = "txtSkills";
            txtSkills.Size = new Size(219, 144);
            txtSkills.TabIndex = 19;
            txtSkills.Text = "";
            // 
            // btnSave
            // 
            btnSave.Location = new Point(220, 517);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(112, 34);
            btnSave.TabIndex = 20;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(338, 517);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(112, 34);
            btnUpdate.TabIndex = 21;
            btnUpdate.Text = "Update";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(456, 517);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(112, 34);
            btnClear.TabIndex = 22;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // txtApplicantID
            // 
            txtApplicantID.ForeColor = SystemColors.GrayText;
            txtApplicantID.Location = new Point(256, 50);
            txtApplicantID.Name = "txtApplicantID";
            txtApplicantID.Size = new Size(234, 31);
            txtApplicantID.TabIndex = 23;
            txtApplicantID.Text = "Applicant ID";
            // 
            // labelExp
            // 
            labelExp.AutoSize = true;
            labelExp.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelExp.Location = new Point(273, 286);
            labelExp.Name = "labelExp";
            labelExp.Size = new Size(237, 38);
            labelExp.TabIndex = 24;
            labelExp.Text = "Work Experience";
            labelExp.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtWorkExp
            // 
            txtWorkExp.Location = new Point(282, 339);
            txtWorkExp.Name = "txtWorkExp";
            txtWorkExp.Size = new Size(219, 144);
            txtWorkExp.TabIndex = 25;
            txtWorkExp.Text = "";
            // 
            // ProfileForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 964);
            Controls.Add(txtWorkExp);
            Controls.Add(labelExp);
            Controls.Add(txtApplicantID);
            Controls.Add(btnClear);
            Controls.Add(btnUpdate);
            Controls.Add(btnSave);
            Controls.Add(txtSkills);
            Controls.Add(txtEduc);
            Controls.Add(labelSkills);
            Controls.Add(labelEduc);
            Controls.Add(txtAddress);
            Controls.Add(txtPhoneNum);
            Controls.Add(cmbGender);
            Controls.Add(dtpBirthdate);
            Controls.Add(txtLastName);
            Controls.Add(CreateProfLabel);
            Controls.Add(txtFirstName);
            Name = "ProfileForm";
            Text = "ProfileForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtFirstName;
        private Label CreateProfLabel;
        private TextBox txtLastName;
        private TextBox txtMiddleName;
        private DateTimePicker dtpBirthdate;
        private ComboBox cmbGender;
        private TextBox txtPhoneNum;
        private Label labelAddress;
        private TextBox txtProvince;
        private TextBox txtCity;
        private TextBox txtAddress;
        private TextBox txtCountry;
        private TextBox txtZipCode;
        private Label labelEduc;
        private Label labelSkills;
        private RichTextBox txtEduc;
        private RichTextBox txtSkills;
        private Button btnSave;
        private Button btnUpdate;
        private Button btnClear;
        private TextBox txtApplicantID;
        private Label labelExp;
        private RichTextBox txtWorkExp;
    }
}
