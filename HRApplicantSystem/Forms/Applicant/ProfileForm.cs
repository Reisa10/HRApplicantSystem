using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;
using HRApplicantSystem.Classes;
using HRApplicantSystem.Database;

namespace HRApplicantSystem.Forms.Applicant
{
    public partial class ProfileForm : Form
    {
        // Tracks whether this is a new profile insertion or an update to an existing one
        private bool isNewProfile = true;

        public ProfileForm()
        {
            InitializeComponent();
            ApplyModernUI(); // Programmatically apply clean, modern styles to the form controls

            // Populate Gender dropdown list
            cmbGender.Items.Clear();
            cmbGender.Items.AddRange(new object[] { "Male", "Female", "Other" });
            cmbGender.SelectedIndex = 0;

            dtpBirthday.MaxDate = DateTime.Today;
        }

        private void ProfileForm_Load(object sender, EventArgs e)
        {
            LoadApplicantProfile();
        }

        /// <summary>
        /// Retrieves and displays the current user's profile details if they exist in the database.
        /// </summary>
        private void LoadApplicantProfile()
        {
            int currentUserId = UserSession.UserID;

            if (currentUserId <= 0)
            {
                MessageBox.Show("No active user session detected. Please log in again.",
                                "Session Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            // Mapped to match your exact database columns (e.g. WorkExperience)
            // Note: If you changed 'ContactNum' in your database to 'ContactNumber' in Step 1, update it here too!
            string query = "SELECT FirstName, MiddleName, LastName, Birthday, Gender, Address, ContactNumber, Education, Skills, WorkExperience " +
                           "FROM Applicants WHERE ApplicantID = ?";

            using (OleDbConnection conn = DBConnection.GetConnection())
            {
                if (conn == null) return;

                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("?", currentUserId);

                    try
                    {
                        conn.Open();
                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Profile already exists -> Set fields and prepare for UPDATE
                                isNewProfile = false;

                                txtFirstName.Text = reader["FirstName"]?.ToString();
                                txtMiddleName.Text = reader["MiddleName"]?.ToString();
                                txtLastName.Text = reader["LastName"]?.ToString();

                                if (reader["Birthday"] != DBNull.Value && reader["Birthday"] != null)
                                {
                                    dtpBirthday.Value = Convert.ToDateTime(reader["Birthday"]);
                                }

                                cmbGender.SelectedItem = reader["Gender"]?.ToString() ?? "Male";
                                txtAddress.Text = reader["Address"]?.ToString();
                                txtContactNumber.Text = reader["ContactNumber"]?.ToString();
                                txtEducation.Text = reader["Education"]?.ToString();
                                txtSkills.Text = reader["Skills"]?.ToString();
                                txtWorkExperience.Text = reader["WorkExperience"]?.ToString();

                                btnSave.Text = "Update Profile";
                            }
                            else
                            {
                                // No profile exists yet -> Prepare for INSERT
                                isNewProfile = true;
                                btnSave.Text = "Save Profile";

                                if (!string.IsNullOrEmpty(UserSession.FullName))
                                {
                                    ParseFullName(UserSession.FullName);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading profile details: " + ex.Message,
                                        "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Validates input controls before sending data to the database.
        /// </summary>
        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) || string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("First Name and Last Name are required.",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtContactNumber.Text))
            {
                MessageBox.Show("Please enter a valid Contact Number.",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Save Button click handler (Performs Save or Update).
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            using (OleDbConnection conn = DBConnection.GetConnection())
            {
                if (conn == null) return;

                string query;
                if (isNewProfile)
                {
                    query = "INSERT INTO Applicants (ApplicantID, FirstName, MiddleName, LastName, Birthday, Gender, Address, ContactNumber, Education, Skills, WorkExperience) " +
                            "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
                }
                else
                {
                    query = "UPDATE Applicants SET FirstName = ?, MiddleName = ?, LastName = ?, Birthday = ?, Gender = ?, Address = ?, ContactNumber = ?, Education = ?, Skills = ?, WorkExperience = ? " +
                            "WHERE ApplicantID = ?";
                }

                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    // Access (OLEDB) matches parameters strictly by chronological execution order (?)
                    if (isNewProfile)
                    {
                        cmd.Parameters.AddWithValue("?", UserSession.UserID);
                        cmd.Parameters.AddWithValue("?", txtFirstName.Text.Trim());
                        cmd.Parameters.AddWithValue("?", txtMiddleName.Text.Trim());
                        cmd.Parameters.AddWithValue("?", txtLastName.Text.Trim());
                        cmd.Parameters.AddWithValue("?", dtpBirthday.Value.Date);
                        cmd.Parameters.AddWithValue("?", cmbGender.SelectedItem?.ToString() ?? "");
                        cmd.Parameters.AddWithValue("?", txtAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("?", txtContactNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("?", txtEducation.Text.Trim());
                        cmd.Parameters.AddWithValue("?", txtSkills.Text.Trim());
                        cmd.Parameters.AddWithValue("?", txtWorkExperience.Text.Trim());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("?", txtFirstName.Text.Trim());
                        cmd.Parameters.AddWithValue("?", txtMiddleName.Text.Trim());
                        cmd.Parameters.AddWithValue("?", txtLastName.Text.Trim());
                        cmd.Parameters.AddWithValue("?", dtpBirthday.Value.Date);
                        cmd.Parameters.AddWithValue("?", cmbGender.SelectedItem?.ToString() ?? "");
                        cmd.Parameters.AddWithValue("?", txtAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("?", txtContactNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("?", txtEducation.Text.Trim());
                        cmd.Parameters.AddWithValue("?", txtSkills.Text.Trim());
                        cmd.Parameters.AddWithValue("?", txtWorkExperience.Text.Trim());
                        cmd.Parameters.AddWithValue("?", UserSession.UserID); // WHERE criteria parameter
                    }

                    try
                    {
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show(isNewProfile ? "Profile created successfully!" : "Profile updated successfully!",
                                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            isNewProfile = false;
                            btnSave.Text = "Update Profile";
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error processing profile save action: " + ex.Message,
                                        "Database Action Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to clear all input fields?",
                                                        "Confirm Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                txtFirstName.Clear();
                txtMiddleName.Clear();
                txtLastName.Clear();
                txtAddress.Clear();
                txtContactNumber.Clear();
                txtEducation.Clear();
                txtSkills.Clear();
                txtWorkExperience.Clear();
                cmbGender.SelectedIndex = 0;
                dtpBirthday.Value = DateTime.Today.AddYears(-18);
            }
        }

        private void ParseFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName)) return;

            string[] parts = fullName.Trim().Split(' ');
            if (parts.Length > 1)
            {
                txtFirstName.Text = parts[0];
                txtLastName.Text = parts[parts.Length - 1];
            }
            else
            {
                txtFirstName.Text = fullName;
            }
        }

        /// <summary>
        /// Safely closes this form to seamlessly return to the Applicant Dashboard [3].
        /// </summary>
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ApplyModernUI()
        {
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular);

            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.BackColor = Color.FromArgb(41, 128, 185);
            btnSave.ForeColor = Color.White;
            btnSave.Cursor = Cursors.Hand;
            btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.FlatAppearance.BorderSize = 1;
            btnClear.FlatAppearance.BorderColor = Color.FromArgb(189, 195, 199);
            btnClear.BackColor = Color.White;
            btnClear.ForeColor = Color.FromArgb(127, 140, 141);
            btnClear.Cursor = Cursors.Hand;
            btnClear.Font = new Font("Segoe UI", 10F, FontStyle.Regular);

            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.FlatAppearance.BorderSize = 1;
            btnBack.FlatAppearance.BorderColor = Color.FromArgb(189, 195, 199);
            btnBack.BackColor = Color.White;
            btnBack.ForeColor = Color.FromArgb(127, 140, 141);
            btnBack.Cursor = Cursors.Hand;
            btnBack.Font = new Font("Segoe UI", 10F, FontStyle.Regular);

            txtAddress.Multiline = true;
            txtEducation.Multiline = true;
            txtSkills.Multiline = true;
            txtWorkExperience.Multiline = true;
        }
    }
}