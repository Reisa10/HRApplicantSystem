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

        // Dynamically aligned controls for layout safety and professional styling
        private Button btnViewDocument;

        public DocumentsForm(int applicantId)
        {
            InitializeComponent();
            this.currentApplicantId = applicantId;

            this.txtDocumentName.Click += txtDocumentName_Click;
            this.dgvDocuments.SelectionChanged += dgvDocuments_SelectionChanged;
            this.cmbAppliedJobs.SelectedIndexChanged += cmbAppliedJobs_SelectedIndexChanged;

            InitializeDynamicControls();
            ApplyModernStyles();
        }

        private void DocumentsForm_Load(object sender, EventArgs e)
        {
            lblApplicantID.Text = $"Applicant ID: {currentApplicantId}";

            string applicantDirectory = Path.Combine(uploadsDirectory, currentApplicantId.ToString());
            if (!Directory.Exists(applicantDirectory))
            {
                Directory.CreateDirectory(applicantDirectory);
            }

            SetupDataGridViewColumns();
            LoadAppliedJobs();

            // Adjust layouts dynamically once controls are fully rendered
            AdjustControlsLayout();
        }

        /// <summary>
        /// Safely extracts the active Job ID from the ComboBox during various binding states.
        /// </summary>
        private int GetSelectedJobId()
        {
            if (cmbAppliedJobs.SelectedValue == null) return -1;

            if (cmbAppliedJobs.SelectedValue is int intJobId)
            {
                return intJobId;
            }

            if (cmbAppliedJobs.SelectedValue is DataRowView drv)
            {
                if (drv.Row.Table.Columns.Contains("JobID") && drv["JobID"] != DBNull.Value)
                {
                    if (int.TryParse(drv["JobID"].ToString(), out int id))
                    {
                        return id;
                    }
                }
            }

            if (int.TryParse(cmbAppliedJobs.SelectedValue.ToString(), out int parsedId))
            {
                return parsedId;
            }

            return -1; // Fallback to General Profile Documents
        }

        /// <summary>
        /// Instantiates extra controls programmatically to prevent designer mismatches.
        /// </summary>
        private void InitializeDynamicControls()
        {
            // Setup btnViewDocument programmatically if it doesn't exist
            Control[] btnMatches = this.Controls.Find("btnViewDocument", true);
            if (btnMatches.Length > 0)
            {
                btnViewDocument = (Button)btnMatches[0];
            }
            else
            {
                btnViewDocument = new Button
                {
                    Name = "btnViewDocument",
                    Text = "🔍 View File",
                };
                btnViewDocument.Click += btnViewDocument_Click;

                if (btnDelete.Parent != null)
                {
                    btnDelete.Parent.Controls.Add(btnViewDocument);
                }
                else
                {
                    this.Controls.Add(btnViewDocument);
                }
            }
            StylePrimaryButton(btnViewDocument, Color.FromArgb(142, 68, 173)); // Modern Purple
        }

        /// <summary>
        /// Automatically organizes and snaps all buttons into a clean, unified horizontal row below inputs.
        /// </summary>
        private void AdjustControlsLayout()
        {
            int startX = txtDocumentName.Left;
            int endX = cmbRequirementType.Left + cmbRequirementType.Width;

            Control parentContainer = txtDocumentName.Parent ?? this;

            // Re-parent all buttons onto the same container to avoid coordinate alignment bugs
            Button[] buttons = new Button[] { btnAdd, btnUpdate, btnDelete, btnViewDocument, btnRefresh, btnBack };
            foreach (Button btn in buttons)
            {
                if (btn != null && btn.Parent != parentContainer)
                {
                    btn.Parent?.Controls.Remove(btn);
                    parentContainer.Controls.Add(btn);
                    btn.BringToFront();
                }
            }

            // Position buttons cleanly directly below the main input fields
            int buttonsTop = txtDocumentName.Top + txtDocumentName.Height + 25;
            int btnWidth = 85;
            int btnHeight = 32;
            int spacing = 8;

            int currentLeft = startX;
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i] != null)
                {
                    buttons[i].Size = new Size(btnWidth, btnHeight);

                    // Keep the Back button aligned far right for symmetry
                    if (buttons[i] == btnBack)
                    {
                        buttons[i].Location = new Point(endX - btnWidth, buttonsTop);
                    }
                    else
                    {
                        buttons[i].Location = new Point(currentLeft, buttonsTop);
                        currentLeft += btnWidth + spacing;
                    }
                }
            }
        }

        private void SetupDataGridViewColumns()
        {
            dgvDocuments.Columns.Clear();

            dgvDocuments.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "RequirementName",
                HeaderText = "Requirement Type",
                DataPropertyName = "RequirementName",
                Width = 180,
                ReadOnly = true
            });

            dgvDocuments.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "DocumentName",
                HeaderText = "Submitted File",
                DataPropertyName = "DocumentName",
                Width = 220,
                ReadOnly = true
            });

            dgvDocuments.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Status",
                HeaderText = "Status",
                DataPropertyName = "Status",
                Width = 110,
                ReadOnly = true
            });

            dgvDocuments.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Remarks",
                HeaderText = "Remarks",
                DataPropertyName = "Remarks",
                Width = 200,
                ReadOnly = true
            });

            dgvDocuments.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "DocumentID",
                HeaderText = "Doc ID",
                DataPropertyName = "DocumentID",
                Visible = false
            });

            dgvDocuments.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "RequirementID",
                HeaderText = "Req ID",
                DataPropertyName = "RequirementID",
                Visible = false
            });
        }

        private void LoadAppliedJobs()
        {
            using (OleDbConnection con = DBConnection.GetConnection())
            {
                if (con == null) return;
                try
                {
                    con.Open();
                    string query = @"SELECT a.ApplicationID, a.JobID, p.PositionName AS JobTitle, a.Status 
                                     FROM (Applications a 
                                     INNER JOIN JobVacancies j ON a.JobID = j.JobID) 
                                     INNER JOIN Positions p ON j.PositionID = p.PositionID
                                     WHERE a.ApplicantID = ? AND a.Status NOT IN ('Withdrawn', 'Rejected')
                                     ORDER BY a.DateApplied DESC";

                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("?", currentApplicantId);
                        OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dt.Columns.Add("DisplayString", typeof(string));
                        foreach (DataRow row in dt.Rows)
                        {
                            row["DisplayString"] = $"{row["JobTitle"]} ({row["Status"]})";
                        }

                        // Add Fallback general options
                        DataRow generalRow = dt.NewRow();
                        generalRow["JobID"] = -1;
                        generalRow["DisplayString"] = "-- General Profile Documents --";
                        generalRow["Status"] = "Draft";
                        dt.Rows.InsertAt(generalRow, 0);

                        cmbAppliedJobs.DataSource = dt;
                        cmbAppliedJobs.DisplayMember = "DisplayString";
                        cmbAppliedJobs.ValueMember = "JobID";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading applied jobs: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cmbAppliedJobs_SelectedIndexChanged(object sender, EventArgs e)
        {
            int jobId = GetSelectedJobId();

            CheckLockStatus(jobId);
            LoadRequirementTypes(jobId);
            LoadUploadedDocuments(jobId);
            UpdateMissingRequirementsDisplay();
        }

        private void CheckLockStatus(int jobId)
        {
            string status = "Draft";
            if (cmbAppliedJobs.SelectedItem is DataRowView selectedRow)
            {
                status = selectedRow["Status"]?.ToString() ?? "Draft";
            }

            isEditingLocked = (status != "Draft" && status != "Submitted" && jobId != -1);

            if (isEditingLocked)
            {
                lblMissingRequirements.Text = "🔒 Locked: Under HR review. Changes are disabled.";
                lblMissingRequirements.ForeColor = Color.FromArgb(192, 57, 43);

                txtDocumentName.Enabled = false;
                cmbRequirementType.Enabled = false;

                btnAdd.Enabled = false;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
            }
            else
            {
                txtDocumentName.Enabled = true;
                cmbRequirementType.Enabled = true;

                btnAdd.Enabled = true;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
        }

        private void LoadRequirementTypes(int jobId)
        {
            using (OleDbConnection con = DBConnection.GetConnection())
            {
                if (con == null) return;
                try
                {
                    con.Open();
                    DataTable dtFiltered;

                    if (jobId == -1)
                    {
                        string queryReqs = "SELECT RequirementTypeID, RequirementName FROM RequirementTypes ORDER BY RequirementName ASC";
                        using (OleDbCommand cmd = new OleDbCommand(queryReqs, con))
                        {
                            OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                            dtFiltered = new DataTable();
                            adapter.Fill(dtFiltered);
                        }
                    }
                    else
                    {
                        string queryJob = "SELECT RequiredDocuments FROM JobVacancies WHERE JobID = ?";
                        string requiredDocsCSV = "";
                        using (OleDbCommand jobCmd = new OleDbCommand(queryJob, con))
                        {
                            jobCmd.Parameters.AddWithValue("?", jobId);
                            object result = jobCmd.ExecuteScalar();
                            if (result != null && result != DBNull.Value)
                            {
                                requiredDocsCSV = result.ToString();
                            }
                        }

                        List<int> requiredIds = new List<int>();
                        if (!string.IsNullOrEmpty(requiredDocsCSV))
                        {
                            string[] tokens = requiredDocsCSV.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string t in tokens)
                            {
                                if (int.TryParse(t.Trim(), out int id))
                                {
                                    requiredIds.Add(id);
                                }
                            }
                        }

                        string queryReqs = "SELECT RequirementTypeID, RequirementName FROM RequirementTypes";
                        using (OleDbCommand cmd = new OleDbCommand(queryReqs, con))
                        {
                            OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                            DataTable dtAll = new DataTable();
                            adapter.Fill(dtAll);

                            dtFiltered = dtAll.Clone();
                            foreach (DataRow row in dtAll.Rows)
                            {
                                int id = Convert.ToInt32(row["RequirementTypeID"]);
                                if (requiredIds.Contains(id))
                                {
                                    dtFiltered.ImportRow(row);
                                }
                            }
                        }
                    }

                    cmbRequirementType.DataSource = dtFiltered;
                    cmbRequirementType.DisplayMember = "RequirementName";
                    cmbRequirementType.ValueMember = "RequirementTypeID";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading requirements: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadUploadedDocuments(int jobId)
        {
            using (OleDbConnection con = DBConnection.GetConnection())
            {
                if (con == null) return;
                try
                {
                    con.Open();

                    DataTable dtReqs = new DataTable();
                    if (jobId == -1)
                    {
                        string reqQuery = "SELECT RequirementTypeID, RequirementName FROM RequirementTypes ORDER BY RequirementName ASC";
                        using (OleDbCommand cmd = new OleDbCommand(reqQuery, con))
                        {
                            OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                            adapter.Fill(dtReqs);
                        }
                    }
                    else
                    {
                        string queryJob = "SELECT RequiredDocuments FROM JobVacancies WHERE JobID = ?";
                        string requiredDocsCSV = "";
                        using (OleDbCommand jobCmd = new OleDbCommand(queryJob, con))
                        {
                            jobCmd.Parameters.AddWithValue("?", jobId);
                            object result = jobCmd.ExecuteScalar();
                            if (result != null && result != DBNull.Value)
                            {
                                requiredDocsCSV = result.ToString();
                            }
                        }

                        List<int> requiredIds = new List<int>();
                        if (!string.IsNullOrEmpty(requiredDocsCSV))
                        {
                            string[] tokens = requiredDocsCSV.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string t in tokens)
                            {
                                if (int.TryParse(t.Trim(), out int id))
                                {
                                    requiredIds.Add(id);
                                }
                            }
                        }

                        string queryReqs = "SELECT RequirementTypeID, RequirementName FROM RequirementTypes";
                        using (OleDbCommand cmd = new OleDbCommand(queryReqs, con))
                        {
                            OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                            DataTable dtAll = new DataTable();
                            adapter.Fill(dtAll);

                            dtReqs = dtAll.Clone();
                            foreach (DataRow row in dtAll.Rows)
                            {
                                int id = Convert.ToInt32(row["RequirementTypeID"]);
                                if (requiredIds.Contains(id))
                                {
                                    dtReqs.ImportRow(row);
                                }
                            }
                        }
                    }

                    DataTable dtUploaded = new DataTable();
                    string uploadQuery = "SELECT DocumentID, RequirementTypeID, DocumentName, Status, Remarks FROM ApplicantDocuments WHERE ApplicantID = ?";
                    using (OleDbCommand cmd = new OleDbCommand(uploadQuery, con))
                    {
                        cmd.Parameters.AddWithValue("?", currentApplicantId);
                        OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                        adapter.Fill(dtUploaded);
                    }

                    DataTable dtDisplay = new DataTable();
                    dtDisplay.Columns.Add("DocumentID", typeof(int));
                    dtDisplay.Columns.Add("RequirementID", typeof(int));
                    dtDisplay.Columns.Add("RequirementName", typeof(string));
                    dtDisplay.Columns.Add("DocumentName", typeof(string));
                    dtDisplay.Columns.Add("Status", typeof(string));
                    dtDisplay.Columns.Add("Remarks", typeof(string));

                    foreach (DataRow reqRow in dtReqs.Rows)
                    {
                        int reqId = Convert.ToInt32(reqRow["RequirementTypeID"]);
                        string reqName = reqRow["RequirementName"].ToString();

                        DataRow[] matches = dtUploaded.Select($"RequirementTypeID = {reqId}");
                        if (matches.Length > 0)
                        {
                            DataRow match = matches[0];
                            dtDisplay.Rows.Add(
                                Convert.ToInt32(match["DocumentID"]),
                                reqId,
                                reqName,
                                match["DocumentName"].ToString(),
                                match["Status"].ToString(),
                                match["Remarks"].ToString()
                            );
                        }
                        else
                        {
                            dtDisplay.Rows.Add(
                                DBNull.Value,
                                reqId,
                                reqName,
                                "—",
                                "Missing",
                                ""
                            );
                        }
                    }

                    dgvDocuments.AutoGenerateColumns = false;
                    dgvDocuments.DataSource = dtDisplay;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error merging documents: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UpdateMissingRequirementsDisplay()
        {
            int jobId = GetSelectedJobId();

            if (jobId == -1)
            {
                List<string> missing = GetGlobalMissingRequirements();
                if (missing.Count == 0)
                {
                    lblMissingRequirements.Text = "✅ Profile Requirements Complete!";
                    lblMissingRequirements.ForeColor = Color.FromArgb(39, 174, 96);
                }
                else
                {
                    lblMissingRequirements.Text = "⚠️ Missing Mandatory Profile Items: " + string.Join(", ", missing);
                    lblMissingRequirements.ForeColor = Color.FromArgb(192, 57, 43);
                }
            }
            else
            {
                List<string> missing = DatabaseHelper.GetMissingRequirementsForJob(currentApplicantId, jobId);
                if (missing.Count == 0)
                {
                    lblMissingRequirements.Text = "✅ Job Vacancy Requirements Complete!";
                    lblMissingRequirements.ForeColor = Color.FromArgb(39, 174, 96);
                }
                else
                {
                    lblMissingRequirements.Text = "⚠️ Missing Vacancy Items: " + string.Join(", ", missing);
                    lblMissingRequirements.ForeColor = Color.FromArgb(192, 57, 43);
                }
            }
        }

        private List<string> GetGlobalMissingRequirements()
        {
            List<string> missing = new List<string>();
            using (OleDbConnection con = DBConnection.GetConnection())
            {
                if (con == null) return missing;
                try
                {
                    con.Open();
                    string reqQuery = "SELECT RequirementTypeID, RequirementName FROM RequirementTypes WHERE IsRequired = True";
                    List<int> requiredIds = new List<int>();
                    Dictionary<int, string> idToName = new Dictionary<int, string>();

                    using (OleDbCommand cmd = new OleDbCommand(reqQuery, con))
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["RequirementTypeID"]);
                            string name = reader["RequirementName"].ToString();
                            requiredIds.Add(id);
                            idToName[id] = name;
                        }
                    }

                    List<int> uploadedIds = new List<int>();
                    string uploadQuery = "SELECT RequirementTypeID FROM ApplicantDocuments WHERE ApplicantID = ? AND Status <> 'Missing'";
                    using (OleDbCommand cmd = new OleDbCommand(uploadQuery, con))
                    {
                        cmd.Parameters.AddWithValue("?", currentApplicantId);
                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                uploadedIds.Add(Convert.ToInt32(reader["RequirementTypeID"]));
                            }
                        }
                    }

                    foreach (int reqId in requiredIds)
                    {
                        if (!uploadedIds.Contains(reqId) && idToName.ContainsKey(reqId))
                        {
                            missing.Add(idToName[reqId]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error computing profile requirements: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return missing;
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
                MessageBox.Show("Please browse and select a file first.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
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
                        MessageBox.Show("This requirement type is already uploaded. Please use 'Replace' instead.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (cmbAppliedJobs.SelectedValue is int jobId)
                {
                    LoadUploadedDocuments(jobId);
                }
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
                MessageBox.Show("Please select a new file first.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int docId = 0;
                object docIdVal = dgvDocuments.SelectedRows[0].Cells["DocumentID"].Value;
                if (docIdVal == null || docIdVal == DBNull.Value)
                {
                    MessageBox.Show("This document has not been uploaded yet. Please use the Add option first.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                docId = Convert.ToInt32(docIdVal);

                string oldFileName = dgvDocuments.SelectedRows[0].Cells["DocumentName"].Value?.ToString() ?? "—";

                using (OleDbConnection con = DBConnection.GetConnection())
                {
                    if (con == null) return;
                    con.Open();

                    string originalFileName = Path.GetFileName(selectedFilePath);
                    string applicantDirectory = Path.Combine(uploadsDirectory, currentApplicantId.ToString());
                    string destinationPath = Path.Combine(applicantDirectory, originalFileName);

                    File.Copy(selectedFilePath, destinationPath, true);

                    string updateQuery = "UPDATE ApplicantDocuments SET DocumentName = ?, Status = 'Submitted' WHERE DocumentID = ?";
                    using (OleDbCommand cmd = new OleDbCommand(updateQuery, con))
                    {
                        cmd.Parameters.AddWithValue("?", originalFileName);
                        cmd.Parameters.AddWithValue("?", docId);
                        cmd.ExecuteNonQuery();
                    }

                    if (oldFileName != originalFileName && oldFileName != "—")
                    {
                        string oldFilePath = Path.Combine(applicantDirectory, oldFileName);
                        if (File.Exists(oldFilePath))
                        {
                            try { File.Delete(oldFilePath); } catch { }
                        }
                    }
                }

                MessageBox.Show("Document updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                if (cmbAppliedJobs.SelectedValue is int jobId)
                {
                    LoadUploadedDocuments(jobId);
                }
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
                MessageBox.Show("Please select a document to delete.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            object docIdVal = dgvDocuments.SelectedRows[0].Cells["DocumentID"].Value;
            if (docIdVal == null || docIdVal == DBNull.Value)
            {
                MessageBox.Show("No uploaded file exists to delete.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int docId = Convert.ToInt32(docIdVal);
            string fileName = dgvDocuments.SelectedRows[0].Cells["DocumentName"].Value.ToString();

            DialogResult confirm = MessageBox.Show("Are you sure you want to delete this document?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
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

                string applicantDirectory = Path.Combine(uploadsDirectory, currentApplicantId.ToString());
                string filePath = Path.Combine(applicantDirectory, fileName);
                if (File.Exists(filePath))
                {
                    try { File.Delete(filePath); } catch { }
                }

                MessageBox.Show("Document deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                if (cmbAppliedJobs.SelectedValue is int jobId)
                {
                    LoadUploadedDocuments(jobId);
                }
                UpdateMissingRequirementsDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete record: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnViewDocument_Click(object sender, EventArgs e)
        {
            if (dgvDocuments.SelectedRows.Count == 0) return;

            DataGridViewRow row = dgvDocuments.SelectedRows[0];
            object docIdVal = row.Cells["DocumentID"].Value;
            if (docIdVal == null || docIdVal == DBNull.Value)
            {
                MessageBox.Show("Please select an uploaded file to view.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string fileName = row.Cells["DocumentName"].Value.ToString();
            string applicantDirectory = Path.Combine(uploadsDirectory, currentApplicantId.ToString());
            string filePath = Path.Combine(applicantDirectory, fileName);

            if (File.Exists(filePath))
            {
                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(filePath)
                    {
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not open file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("The physical file could not be found locally.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            int jobId = GetSelectedJobId();
            CheckLockStatus(jobId);
            LoadRequirementTypes(jobId);
            LoadUploadedDocuments(jobId);
            UpdateMissingRequirementsDisplay();
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

                if (row.Cells["RequirementID"].Value != null && row.Cells["RequirementID"].Value != DBNull.Value)
                {
                    cmbRequirementType.SelectedValue = Convert.ToInt32(row.Cells["RequirementID"].Value);
                }

                object docIdVal = row.Cells["DocumentID"].Value;
                bool isUploaded = (docIdVal != null && docIdVal != DBNull.Value);

                if (isUploaded)
                {
                    txtDocumentName.Text = row.Cells["DocumentName"].Value?.ToString() ?? "";

                    if (!isEditingLocked)
                    {
                        btnAdd.Enabled = false;
                        btnUpdate.Enabled = true;
                        btnDelete.Enabled = true;
                        if (btnViewDocument != null) btnViewDocument.Enabled = true;
                    }
                }
                else
                {
                    txtDocumentName.Text = "";

                    if (!isEditingLocked)
                    {
                        btnAdd.Enabled = true;
                        btnUpdate.Enabled = false;
                        btnDelete.Enabled = false;
                        if (btnViewDocument != null) btnViewDocument.Enabled = false;
                    }
                }
            }
        }

        private void dgvDocuments_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvDocuments.Columns[e.ColumnIndex].Name == "Status")
            {
                if (e.Value != null)
                {
                    string statusValue = e.Value.ToString();
                    if (statusValue.Equals("Missing", StringComparison.OrdinalIgnoreCase))
                    {
                        e.CellStyle.ForeColor = Color.FromArgb(192, 57, 43);
                        e.CellStyle.Font = new Font(dgvDocuments.Font, FontStyle.Bold);
                    }
                    else if (statusValue.Equals("Submitted", StringComparison.OrdinalIgnoreCase))
                    {
                        e.CellStyle.ForeColor = Color.FromArgb(39, 174, 96);
                        e.CellStyle.Font = new Font(dgvDocuments.Font, FontStyle.Bold);
                    }
                    else
                    {
                        e.CellStyle.ForeColor = Color.FromArgb(243, 156, 18);
                        e.CellStyle.Font = new Font(dgvDocuments.Font, FontStyle.Bold);
                    }
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
            this.BackColor = Color.FromArgb(244, 246, 249);
            this.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);

            StylePrimaryButton(btnAdd, Color.FromArgb(41, 128, 185));
            StylePrimaryButton(btnUpdate, Color.FromArgb(39, 174, 96));
            StylePrimaryButton(btnDelete, Color.FromArgb(192, 57, 43));

            StyleSecondaryButton(btnRefresh);
            StyleSecondaryButton(btnBack);

            dgvDocuments.BackgroundColor = Color.White;
            dgvDocuments.BorderStyle = BorderStyle.None;
            dgvDocuments.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvDocuments.GridColor = Color.FromArgb(230, 234, 240);

            dgvDocuments.RowHeadersVisible = false;
            dgvDocuments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDocuments.MultiSelect = false;

            dgvDocuments.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
            dgvDocuments.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvDocuments.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            dgvDocuments.ColumnHeadersHeight = 40;
            dgvDocuments.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvDocuments.EnableHeadersVisualStyles = false;

            dgvDocuments.RowTemplate.Height = 35;
            dgvDocuments.DefaultCellStyle.BackColor = Color.White;
            dgvDocuments.DefaultCellStyle.ForeColor = Color.FromArgb(44, 62, 80);
            dgvDocuments.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
            dgvDocuments.DefaultCellStyle.SelectionForeColor = Color.White;

            dgvDocuments.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);

            dgvDocuments.CellFormatting += dgvDocuments_CellFormatting;
        }

        private void StylePrimaryButton(Button btn, Color bg)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = bg;
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
        }

        private void StyleSecondaryButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = Color.FromArgb(127, 140, 141);
            btn.BackColor = Color.White;
            btn.ForeColor = Color.FromArgb(44, 62, 80);
            btn.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
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