using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using HRApplicantSystem.Database;

namespace HRApplicantSystem.Forms.Applicant
{
    public partial class DocumentsForm : Form
    {
        private int currentApplicantId;
        private bool isEditingLocked = false;
        private string selectedFilePath = string.Empty;
        private string uploadsDirectory = Path.Combine(Application.StartupPath, "UploadedDocuments");

        public DocumentsForm(int applicantId)
        {
            InitializeComponent();
            this.currentApplicantId = applicantId;

            // Programmatically wire help events to keep designer file clean
            this.txtDocumentName.Click += txtDocumentName_Click;
            this.dgvDocuments.SelectionChanged += dgvDocuments_SelectionChanged;

            ApplyModernStyles();
        }

        private void DocumentsForm_Load(object sender, EventArgs e)
        {
            lblApplicantID.Text = $"Applicant ID: {currentApplicantId}";

            // Ensure our applicant-specific subfolder directory exists locally [2]
            string applicantDirectory = Path.Combine(uploadsDirectory, currentApplicantId.ToString());
            if (!Directory.Exists(applicantDirectory))
            {
                Directory.CreateDirectory(applicantDirectory);
            }

            CheckLockStatus();
            LoadRequirementTypes();
            LoadUploadedDocuments();
        }

        private void CheckLockStatus()
        {
            isEditingLocked = DatabaseHelper.IsApplicationLocked(currentApplicantId);

            if (isEditingLocked)
            {
                lblMissingRequirements.Text = "🔒 Locked: Application is under HR review. Changes are disabled.";
                lblMissingRequirements.ForeColor = Color.FromArgb(192, 57, 43); // Dark red alert

                // Disable input fields and buttons
                txtDocumentName.Enabled = false;
                cmbRequirementType.Enabled = false;
                btnAdd.Enabled = false;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
            }
            else
            {
                UpdateMissingRequirementsDisplay();
            }
        }

        private void LoadRequirementTypes()
        {
            using (OleDbConnection con = DBConnection.GetConnection())
            {
                if (con == null) return;
                try
                {
                    con.Open();
                    string query = "SELECT RequirementTypeID, RequirementName FROM RequirementTypes";
                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    {
                        OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        cmbRequirementType.DataSource = dt;
                        cmbRequirementType.DisplayMember = "RequirementName";
                        cmbRequirementType.ValueMember = "RequirementTypeID";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading requirement types: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadUploadedDocuments()
        {
            using (OleDbConnection con = DBConnection.GetConnection())
            {
                if (con == null) return;
                try
                {
                    con.Open();
                    string query = "SELECT d.DocumentID, d.RequirementTypeID AS RequirementID, d.DocumentName, r.RequirementName, d.Status, d.Remarks AS HRRemarks " +
                                   "FROM ApplicantDocuments d " +
                                   "INNER JOIN RequirementTypes r ON d.RequirementTypeID = r.RequirementTypeID " +
                                   "WHERE d.ApplicantID = ?";

                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("?", currentApplicantId);
                        OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvDocuments.AutoGenerateColumns = false;
                        dgvDocuments.DataSource = dt;

                        // Programmatic fix to format both column headers dynamically [1]
                        if (dgvDocuments.Columns["DocumentName"] != null)
                        {
                            dgvDocuments.Columns["DocumentName"].HeaderText = "Document Name";
                        }
                        if (dgvDocuments.Columns["RequirementName"] != null)
                        {
                            dgvDocuments.Columns["RequirementName"].HeaderText = "Requirement Type";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error displaying uploaded files: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UpdateMissingRequirementsDisplay()
        {
            List<string> missing = DatabaseHelper.GetMissingRequirements(currentApplicantId);
            if (missing.Count == 0)
            {
                lblMissingRequirements.Text = "✅ Missing Requirements: None (All uploaded!)";
                lblMissingRequirements.ForeColor = Color.FromArgb(39, 174, 96); // Clean Green
            }
            else
            {
                lblMissingRequirements.Text = "⚠️ Missing Requirements: " + string.Join(", ", missing);
                lblMissingRequirements.ForeColor = Color.FromArgb(192, 57, 43); // Notice Red
            }
        }

        private void txtDocumentName_Click(object sender, EventArgs e)
        {
            if (isEditingLocked) return;

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Documents (*.pdf;*.docx;*.jpg;*.png)|*.pdf;*.docx;*.jpg;*.png|All Files (*.*)|*.*";
                ofd.Title = "Select File to Upload";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    selectedFilePath = ofd.FileName;
                    txtDocumentName.Text = Path.GetFileName(selectedFilePath);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (isEditingLocked) return;

            if (cmbRequirementType.SelectedValue == null)
            {
                MessageBox.Show("Please select a requirement type.", "Validation Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int reqTypeId = Convert.ToInt32(cmbRequirementType.SelectedValue);

            if (string.IsNullOrEmpty(selectedFilePath) || !File.Exists(selectedFilePath))
            {
                MessageBox.Show("Please select a file first by clicking inside the Document Name field.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Preserve exact filename [2]
                string originalFileName = Path.GetFileName(selectedFilePath);
                string applicantDirectory = Path.Combine(uploadsDirectory, currentApplicantId.ToString());
                string destinationPath = Path.Combine(applicantDirectory, originalFileName);

                File.Copy(selectedFilePath, destinationPath, true);

                using (OleDbConnection con = DBConnection.GetConnection())
                {
                    if (con == null) return;
                    con.Open();

                    string checkQuery = "SELECT DocumentID FROM ApplicantDocuments WHERE ApplicantID = ? AND RequirementTypeID = ?";
                    bool exists = false;
                    using (OleDbCommand checkCmd = new OleDbCommand(checkQuery, con))
                    {
                        checkCmd.Parameters.AddWithValue("?", currentApplicantId);
                        checkCmd.Parameters.AddWithValue("?", reqTypeId);
                        object result = checkCmd.ExecuteScalar();
                        exists = (result != null);
                    }

                    if (exists)
                    {
                        MessageBox.Show("This requirement type is already uploaded. Please use 'Update Document' instead.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    string insertQuery = "INSERT INTO ApplicantDocuments (ApplicantID, RequirementTypeID, DocumentName, Status, Remarks) VALUES (?, ?, ?, 'Submitted', '')";
                    using (OleDbCommand insertCmd = new OleDbCommand(insertQuery, con))
                    {
                        insertCmd.Parameters.AddWithValue("?", currentApplicantId);
                        insertCmd.Parameters.AddWithValue("?", reqTypeId);
                        insertCmd.Parameters.AddWithValue("?", originalFileName);
                        insertCmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Document added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadUploadedDocuments();
                UpdateMissingRequirementsDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (isEditingLocked) return;

            if (dgvDocuments.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a document from the grid to update.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrEmpty(selectedFilePath) || !File.Exists(selectedFilePath))
            {
                MessageBox.Show("Please select a new file first by clicking inside the Document Name field.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int docId = Convert.ToInt32(dgvDocuments.SelectedRows[0].Cells["DocumentID"].Value);
                int reqTypeId = Convert.ToInt32(cmbRequirementType.SelectedValue);
                string oldFileName = dgvDocuments.SelectedRows[0].Cells["DocumentName"].Value.ToString();

                // Preserve exact filename [2]
                string originalFileName = Path.GetFileName(selectedFilePath);
                string applicantDirectory = Path.Combine(uploadsDirectory, currentApplicantId.ToString());
                string destinationPath = Path.Combine(applicantDirectory, originalFileName);

                File.Copy(selectedFilePath, destinationPath, true);

                using (OleDbConnection con = DBConnection.GetConnection())
                {
                    if (con == null) return;
                    con.Open();

                    string updateQuery = "UPDATE ApplicantDocuments SET DocumentName = ?, Status = 'Submitted' WHERE DocumentID = ?";
                    using (OleDbCommand cmd = new OleDbCommand(updateQuery, con))
                    {
                        cmd.Parameters.AddWithValue("?", originalFileName);
                        cmd.Parameters.AddWithValue("?", docId);
                        cmd.ExecuteNonQuery();
                    }
                }

                // Delete the old file safely if names are different [2]
                if (oldFileName != originalFileName)
                {
                    string oldFilePath = Path.Combine(applicantDirectory, oldFileName);
                    if (File.Exists(oldFilePath))
                    {
                        try { File.Delete(oldFilePath); } catch { }
                    }
                }

                MessageBox.Show("Document updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadUploadedDocuments();
                UpdateMissingRequirementsDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (isEditingLocked) return;

            if (dgvDocuments.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a document from the list to delete.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult confirm = MessageBox.Show("Are you sure you want to delete this document?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                int docId = Convert.ToInt32(dgvDocuments.SelectedRows[0].Cells["DocumentID"].Value);
                string fileName = dgvDocuments.SelectedRows[0].Cells["DocumentName"].Value.ToString();

                using (OleDbConnection con = DBConnection.GetConnection())
                {
                    if (con == null) return;
                    con.Open();

                    string deleteQuery = "DELETE FROM ApplicantDocuments WHERE DocumentID = ?";
                    using (OleDbCommand cmd = new OleDbCommand(deleteQuery, con))
                    {
                        cmd.Parameters.AddWithValue("?", docId);
                        cmd.ExecuteNonQuery();
                    }
                }

                // Delete local file from applicant-specific subfolder [2]
                string applicantDirectory = Path.Combine(uploadsDirectory, currentApplicantId.ToString());
                string filePath = Path.Combine(applicantDirectory, fileName);
                if (File.Exists(filePath))
                {
                    try { File.Delete(filePath); } catch { }
                }

                MessageBox.Show("Document deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadUploadedDocuments();
                UpdateMissingRequirementsDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete record: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            CheckLockStatus();
            LoadUploadedDocuments();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvDocuments_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDocuments.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvDocuments.SelectedRows[0];
                if (row.Cells["DocumentName"].Value != null)
                {
                    txtDocumentName.Text = row.Cells["DocumentName"].Value.ToString();
                }
                if (row.Cells["RequirementID"].Value != null)
                {
                    cmbRequirementType.SelectedValue = row.Cells["RequirementID"].Value;
                }
            }
        }

        private void ClearForm()
        {
            selectedFilePath = string.Empty;
            txtDocumentName.Clear();
        }

        private void ApplyModernStyles()
        {
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular);

            // Style standard functional buttons
            StylePrimaryButton(btnAdd, Color.FromArgb(41, 128, 185)); // Deep Blue
            StylePrimaryButton(btnUpdate, Color.FromArgb(39, 174, 96)); // Green
            StylePrimaryButton(btnDelete, Color.FromArgb(192, 57, 43)); // Soft Red

            StyleSecondaryButton(btnRefresh);
            StyleSecondaryButton(btnBack);

            // Setup DataGridView styling
            dgvDocuments.BackgroundColor = Color.White;
            dgvDocuments.BorderStyle = BorderStyle.None;
            dgvDocuments.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94);
            dgvDocuments.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvDocuments.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvDocuments.EnableHeadersVisualStyles = false;
            dgvDocuments.GridColor = Color.FromArgb(220, 224, 230);
            dgvDocuments.RowHeadersVisible = false;
            dgvDocuments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDocuments.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
        }

        private void StylePrimaryButton(Button btn, Color bg)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = bg;
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
        }

        private void StyleSecondaryButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = Color.FromArgb(127, 140, 141);
            btn.BackColor = Color.White;
            btn.ForeColor = Color.FromArgb(127, 140, 141);
            btn.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
        }

        private void lblApplicantID_Click(object sender, EventArgs e) { }
        private void dgvDocuments_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void lblMissingRequirements_Click(object sender, EventArgs e) { }
        private void txtDocumentName_TextChanged(object sender, EventArgs e) { }
        private void lblDocumentName_Click(object sender, EventArgs e) { }
        private void lblRequirementType_Click(object sender, EventArgs e) { }
    }
}