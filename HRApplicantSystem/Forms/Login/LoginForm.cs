using HRApplicantSystem.Database;
using HRApplicantSystem.Forms.HR;
using HRApplicantSystem.Forms.Applicant;
using HRApplicantSystem.Classes;
using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace HRApplicantSystem.Forms.Login
{
    public partial class LoginForm : Form
    {
        // Colors extracted from the dashboard design theme
        private static readonly Color ColorSidebarBg = Color.FromArgb(17, 24, 39);       // Slate 900 (Sidebar)
        private static readonly Color ColorMainBg = Color.FromArgb(248, 250, 252);       // Slate 50 (Workspace)
        private static readonly Color ColorTextPrimary = Color.FromArgb(15, 23, 42);     // Slate 900 (Text)
        private static readonly Color ColorTextSecondary = Color.FromArgb(100, 116, 139); // Slate 500 (Subtext)
        private static readonly Color ColorAccentBlue = Color.FromArgb(37, 99, 235);      // Blue 600 (Primary actions)
        private static readonly Color ColorLightButton = Color.FromArgb(241, 245, 249);   // Slate 100 (Secondary buttons)
        private static readonly Color ColorExitButton = Color.FromArgb(226, 232, 240);    // Slate 200 (Neutral actions)

        public LoginForm()
        {
            InitializeComponent();

            // Set default state
            rdoApplicant.Checked = true;
            UpdateUiLayout();
            ApplyThemeStyles();
        }

        /// <summary>
        /// Applies cohesive color palette and typography from the dashboard theme.
        /// </summary>
        private void ApplyThemeStyles()
        {
            this.BackColor = ColorMainBg;
            pnlSidebar.BackColor = ColorSidebarBg;

            // Brand Sidebar Styling
            lblSidebarTitle.ForeColor = Color.White;
            lblSidebarTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblSidebarSub.ForeColor = Color.FromArgb(148, 163, 184); // Slate 400
            lblSidebarSub.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            lblSidebarDesc.ForeColor = Color.FromArgb(100, 116, 139); // Slate 500
            lblSidebarDesc.Font = new Font("Segoe UI", 8F, FontStyle.Regular);

            // Styling form controls recursively
            StyleControlsRecursive(this);

            // Fine-tune specific key actions
            if (lblTitle != null)
            {
                lblTitle.ForeColor = ColorTextPrimary;
                lblTitle.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            }

            if (lnkChangePassword != null)
            {
                lnkChangePassword.LinkColor = ColorAccentBlue;
                lnkChangePassword.ActiveLinkColor = Color.FromArgb(29, 78, 216);
            }

            // Styled buttons with flat properties to match the clean dashboard
            StyleFlatButton(btnLogin, ColorAccentBlue, Color.White);
            StyleFlatButton(btnRegister, ColorLightButton, ColorAccentBlue);
            StyleFlatButton(btnExit, ColorExitButton, Color.FromArgb(71, 85, 105));
        }

        private void StyleControlsRecursive(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl == pnlSidebar) continue; // Skip styling sidebar controls with main form styles

                if (ctrl is Label lbl && lbl.Name != "lblTitle")
                {
                    lbl.ForeColor = ColorTextSecondary;
                    lbl.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
                }
                else if (ctrl is TextBox txt)
                {
                    txt.BackColor = Color.White;
                    txt.ForeColor = ColorTextPrimary;
                    txt.Font = new Font("Segoe UI", 10F);
                    txt.BorderStyle = BorderStyle.FixedSingle;
                }
                else if (ctrl is RadioButton rdo)
                {
                    rdo.ForeColor = ColorTextPrimary;
                    rdo.Font = new Font("Segoe UI", 9.5F);
                }
                else if (ctrl.HasChildren)
                {
                    StyleControlsRecursive(ctrl);
                }
            }
        }

        private void StyleFlatButton(Button btn, Color backColor, Color foreColor)
        {
            if (btn == null) return;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = backColor;
            btn.ForeColor = foreColor;
            btn.Cursor = Cursors.Hand;
            btn.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string usernamePrompt = rdoAdmin.Checked ? "username" : "email address";

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Please enter your " + usernamePrompt + ".", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter your password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            OleDbConnection con = DBConnection.GetConnection();
            if (con == null) return;

            try
            {
                con.Open();

                // 1. Check HR / Manager / Admin Users table
                if (rdoAdmin.Checked)
                {
                    string hrQuery = "SELECT * FROM Users WHERE Username = ? AND [Password] = ?";
                    using (OleDbCommand hrCmd = new OleDbCommand(hrQuery, con))
                    {
                        hrCmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                        hrCmd.Parameters.AddWithValue("@Password", txtPassword.Text);

                        using (OleDbDataReader reader = hrCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                try
                                {
                                    int activeOrdinal = reader.GetOrdinal("IsActive");
                                    if (!reader.IsDBNull(activeOrdinal))
                                    {
                                        bool isActive = Convert.ToBoolean(reader[activeOrdinal]);
                                        if (!isActive)
                                        {
                                            MessageBox.Show("Your staff account is currently inactive. Please contact the System Administrator.", "Account Inactive", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            return;
                                        }
                                    }
                                }
                                catch (IndexOutOfRangeException)
                                {
                                    // Fallback if schema does not have IsActive column yet
                                }

                                UserSession.UserID = Convert.ToInt32(reader["UserID"]);
                                UserSession.Username = reader["Username"].ToString();
                                UserSession.FullName = reader["Full Name"].ToString();
                                UserSession.Role = reader["Role"].ToString();

                                MessageBox.Show("Welcome, " + UserSession.FullName + "!", "Login Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                HRDashboard dashboard = new HRDashboard();
                                dashboard.Show();
                                this.Hide();
                                return;
                            }
                        }
                    }
                }

                // 2. Check ApplicantAccounts table
                if (rdoApplicant.Checked)
                {
                    string appQuery = "SELECT * FROM ApplicantAccounts WHERE Email = ? AND [Password] = ?";
                    using (OleDbCommand appCmd = new OleDbCommand(appQuery, con))
                    {
                        appCmd.Parameters.AddWithValue("@Email", txtUsername.Text.Trim());
                        appCmd.Parameters.AddWithValue("@Password", txtPassword.Text);

                        using (OleDbDataReader reader = appCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (reader["AccountStatus"].ToString() != "Active")
                                {
                                    MessageBox.Show("Your account is currently inactive. Please contact HR.", "Account Inactive", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }

                                UserSession.UserID = Convert.ToInt32(reader["ApplicantID"]);
                                UserSession.Username = reader["Email"].ToString();
                                UserSession.Role = "Applicant";

                                string nameQuery = "SELECT FirstName, LastName FROM Applicants WHERE ApplicantID = ?";
                                using (OleDbCommand nameCmd = new OleDbCommand(nameQuery, con))
                                {
                                    nameCmd.Parameters.AddWithValue("?", UserSession.UserID);
                                    using (OleDbDataReader nameReader = nameCmd.ExecuteReader())
                                    {
                                        if (nameReader.Read())
                                        {
                                            string fName = (nameReader["FirstName"] != DBNull.Value && nameReader["FirstName"] != null) ? nameReader["FirstName"].ToString() : "";
                                            string lName = (nameReader["LastName"] != DBNull.Value && nameReader["LastName"] != null) ? nameReader["LastName"].ToString() : "";
                                            UserSession.FullName = (fName + " " + lName).Trim();
                                        }

                                        if (string.IsNullOrWhiteSpace(UserSession.FullName))
                                        {
                                            UserSession.FullName = "Applicant";
                                        }
                                    }
                                }

                                MessageBox.Show("Login Successful! Welcome, " + UserSession.FullName + ".", "Login Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                ApplicantDashboard dashboard = new ApplicantDashboard();
                                dashboard.Show();
                                this.Hide();
                                return;
                            }
                        }
                    }
                }

                MessageBox.Show("Invalid " + usernamePrompt + " or Password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred during login:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RegisterForm registerForm = new RegisterForm();
            registerForm.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lnkChangePassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            bool isHR = rdoAdmin.Checked;
            ChangePasswordForm cpForm = new ChangePasswordForm(isHR);
            cpForm.ShowDialog();
        }

        private void RoleSelection_CheckedChanged(object sender, EventArgs e)
        {
            UpdateUiLayout();
        }

        /// <summary>
        /// Updates coordinates to match the split-screen design layout when changing roles.
        /// </summary>
        private void UpdateUiLayout()
        {
            if (rdoApplicant.Checked)
            {
                lblUsername.Text = "Email Address:";
                label1.Visible = true;
                btnRegister.Visible = true;
                btnExit.Location = new Point(205, 365);
                this.ClientSize = new Size(520, 420);
            }
            else
            {
                lblUsername.Text = "Username:";
                label1.Visible = false;
                btnRegister.Visible = false;
                btnExit.Location = new Point(205, 295);
                this.ClientSize = new Size(520, 350);
            }
        }

        private void label1_Click(object sender, EventArgs e) { }
        private void label1_Click_1(object sender, EventArgs e) { }
        private void label1_Click_2(object sender, EventArgs e) { }
        private void label1_Click_3(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox1_TextChanged_1(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
    }
}