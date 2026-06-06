namespace HRApplicantSystem.Forms.Login
{
    partial class LoginForm
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.rdoApplicant = new System.Windows.Forms.RadioButton();
            this.rdoAdmin = new System.Windows.Forms.RadioButton();
            this.lnkChangePassword = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(183, 26);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "LOGIN PORTAL";
            this.lblTitle.Click += new System.EventHandler(this.label1_Click);
            // 
            // rdoApplicant
            // 
            this.rdoApplicant.AutoSize = true;
            this.rdoApplicant.Checked = true;
            this.rdoApplicant.Location = new System.Drawing.Point(15, 50);
            this.rdoApplicant.Name = "rdoApplicant";
            this.rdoApplicant.Size = new System.Drawing.Size(117, 20);
            this.rdoApplicant.TabIndex = 1;
            this.rdoApplicant.TabStop = true;
            this.rdoApplicant.Text = "Applicant Portal";
            this.rdoApplicant.UseVisualStyleBackColor = true;
            this.rdoApplicant.CheckedChanged += new System.EventHandler(this.RoleSelection_CheckedChanged);
            // 
            // rdoAdmin
            // 
            this.rdoAdmin.AutoSize = true;
            this.rdoAdmin.Location = new System.Drawing.Point(160, 50);
            this.rdoAdmin.Name = "rdoAdmin";
            this.rdoAdmin.Size = new System.Drawing.Size(127, 20);
            this.rdoAdmin.TabIndex = 2;
            this.rdoAdmin.Text = "HR / Management";
            this.rdoAdmin.UseVisualStyleBackColor = true;
            this.rdoAdmin.CheckedChanged += new System.EventHandler(this.RoleSelection_CheckedChanged);
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(12, 90);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(94, 16);
            this.lblUsername.TabIndex = 3;
            this.lblUsername.Text = "Email Address:";
            this.lblUsername.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(15, 110);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(315, 22);
            this.txtUsername.TabIndex = 4;
            this.txtUsername.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(12, 155);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(70, 16);
            this.lblPassword.TabIndex = 5;
            this.lblPassword.Text = "Password:";
            this.lblPassword.Click += new System.EventHandler(this.label1_Click_2);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(12, 175);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(318, 22);
            this.txtPassword.TabIndex = 6;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.TextChanged += new System.EventHandler(this.textBox1_TextChanged_1);
            // 
            // lnkChangePassword
            // 
            this.lnkChangePassword.AutoSize = true;
            this.lnkChangePassword.Location = new System.Drawing.Point(210, 200);
            this.lnkChangePassword.Name = "lnkChangePassword";
            this.lnkChangePassword.TabIndex = 11;
            this.lnkChangePassword.Text = "Change Password";
            this.lnkChangePassword.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkChangePassword_LinkClicked);
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(10, 220);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(320, 30);
            this.btnLogin.TabIndex = 7;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 265);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(146, 16);
            this.label1.TabIndex = 8;
            this.label1.Text = "Don\'t have an account?";
            this.label1.Click += new System.EventHandler(this.label1_Click_3);
            // 
            // btnRegister
            // 
            this.btnRegister.Location = new System.Drawing.Point(10, 290);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(320, 30);
            this.btnRegister.TabIndex = 9;
            this.btnRegister.Text = "Register";
            this.btnRegister.UseVisualStyleBackColor = true;
            this.btnRegister.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(10, 340);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(320, 30);
            this.btnExit.TabIndex = 10;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 385);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lnkChangePassword);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.rdoAdmin);
            this.Controls.Add(this.rdoApplicant);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Applicant System Login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rdoApplicant;
        private System.Windows.Forms.RadioButton rdoAdmin;
        private System.Windows.Forms.LinkLabel lnkChangePassword;
    }
}