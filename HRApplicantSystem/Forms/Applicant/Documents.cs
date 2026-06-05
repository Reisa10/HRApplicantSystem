using HRApplicantSystem.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HRApplicantSystem.Forms.Applicant
{
    public partial class DocumentsForm : Form
    {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Database/HRDatabase.accdb;";
        private int _applicantId = 0;

        //private string[] requiredDocs = { "Resume.pdf", "Diploma.pdf", "ValidID.pdf" };

        public DocumentsForm(int applicantId)
        {
            InitializeComponent();
            _applicantId = applicantId;
            this.Load += DocumentsForm_Load;
        }
        private void DocumentsForm_Load(object sender, EventArgs e)
        {
            lblApplicantID.Text = "Applicant ID: " + _applicantId.ToString();
            EnsureApplicantExists();
            LoadRequirementTypes();
            LoadRequirementTypeCombo();
            LoadDocuments();
            CheckMissingRequirements();
            dgvDocuments.AllowUserToAddRows = false;
        }
        private void LoadRequirementTypeCombo()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT RequirementTypeID, RequirementName FROM RequirementTypes ORDER BY RequirementName";
                OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                cmbRequirementType.DisplayMember = "RequirementName";
                cmbRequirementType.ValueMember = "RequirementTypeID";
                cmbRequirementType.DataSource = dt;
        }
        }
        private void EnsureApplicantExists()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();

                // Check if applicant exists in Applicants already
                string checkQuery = "SELECT COUNT(*) FROM Applicants WHERE ApplicantID =?";
                using (OleDbCommand checkCmd = new OleDbCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("?", _applicantId);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count == 0)
                    {
                        // Check if ApplicantID exists in ApplicantAccounts
                        string checkAccount = "SELECT COUNT(*) FROM ApplicantAccounts WHERE ApplicantID =?";
                        using (OleDbCommand checkAcctCmd = new OleDbCommand(checkAccount, conn))
                        {
                            checkAcctCmd.Parameters.AddWithValue("?", _applicantId);
                            int acctCount = (int)checkAcctCmd.ExecuteScalar();

                            if (acctCount == 0)
                            {
                                // Insert into ApplicantAccounts first (let ID auto-generate)
                                string insertAccount = "INSERT INTO ApplicantAccounts (Email, [Password], AccountStatus) VALUES (?,?,?)";
                                using (OleDbCommand cmd = new OleDbCommand(insertAccount, conn))
                                {
                                    cmd.Parameters.AddWithValue("?", "temp@email.com"); // placeholder
                                    cmd.Parameters.AddWithValue("?", "temp"); // placeholder
                                    cmd.Parameters.AddWithValue("?", "Active");
                                    cmd.ExecuteNonQuery();
                                }

                                // Get the auto-generated ID
                                string getIdQuery = "SELECT @@IDENTITY";
                                using (OleDbCommand getIdCmd = new OleDbCommand(getIdQuery, conn))
                                {
                                    _applicantId = Convert.ToInt32(getIdCmd.ExecuteScalar());
                                }
                            }
                        }

                        // Now insert into Applicants
                        string fullName = UserSession.FullName ?? "Applicant";
                        string[] nameParts = fullName.Split(' ');

                        string firstName = nameParts[0];
                        string middleName = nameParts.Length > 2 ? string.Join(" ", nameParts.Skip(1).Take(nameParts.Length - 2)) : "";
                        string lastName = nameParts.Length > 1 ? nameParts.Last() : "";

                        string insertQuery = "INSERT INTO Applicants (ApplicantID, FirstName, MiddleName, LastName) VALUES (?,?,?,?)";
                        using (OleDbCommand insertCmd = new OleDbCommand(insertQuery, conn))
                        {
                            insertCmd.Parameters.AddWithValue("?", _applicantId);
                            insertCmd.Parameters.AddWithValue("?", string.IsNullOrEmpty(firstName) ? DBNull.Value : (object)firstName);
                            insertCmd.Parameters.AddWithValue("?", string.IsNullOrEmpty(middleName) ? DBNull.Value : (object)middleName);
                            insertCmd.Parameters.AddWithValue("?", string.IsNullOrEmpty(lastName) ? DBNull.Value : (object)lastName);
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
        private void LoadRequirementTypes()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT RequirementTypeID, RequirementName FROM RequirementTypes";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    requirementTypes.Clear();
                    while (reader.Read())
                    {
                        requirementTypes.Add(
                        Convert.ToInt32(reader ["RequirementTypeID"]),
                        reader ["RequirementName"].ToString()
                        );
                    }
                }
            }
        }
        // ============================================
        // LOAD DOCUMENTS
        // ============================================
        private void LoadDocuments()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string query = @"
                SELECT ad.DocumentID, ad.RequirementTypeID, ad.DocumentName, ad.Status, ad.Remarks, 
                rt.RequirementName FROM ApplicantDocuments ad INNER JOIN RequirementTypes rt ON ad.RequirementTypeID = rt.RequirementTypeID WHERE ad.ApplicantID =?";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("?", _applicantId);
                    OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvDocuments.AutoGenerateColumns = false;
                    dgvDocuments.DataSource = dt;

                    if (dgvDocuments.Columns ["DocumentID"]!= null)
                        dgvDocuments.Columns ["DocumentID"].Visible = false;

                    if (dgvDocuments.Columns ["RequirementTypeID"]!= null)
                        dgvDocuments.Columns ["RequirementTypeID"].Visible = false;
                }
            }
        }
        // ============================================
        // ADD DOCUMENT
        // ============================================
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string docName = txtDocumentName.Text.Trim();

            if (string.IsNullOrWhiteSpace(docName))
            {
                MessageBox.Show("Please enter a document name (e.g., Resume.pdf).");
                return;
            }

            if (!docName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Document name must end with.pdf");
                return;
            }

            if (cmbRequirementType.SelectedValue == null)
            {
                MessageBox.Show("Please select a requirement type.");
                return;
            }

            if (DocumentExists(docName))
            {
                MessageBox.Show("A document with this name already exists. Use Update to replace it.");
                return;
            }

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO ApplicantDocuments (ApplicantID, RequirementTypeID, DocumentName, Status, Remarks) VALUES (?,?,?,?,?)";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("?", _applicantId);
                    cmd.Parameters.AddWithValue("?", cmbRequirementType.SelectedValue);
                    cmd.Parameters.AddWithValue("?", docName);
                    cmd.Parameters.AddWithValue("?", "Pending");
                    cmd.Parameters.AddWithValue("?", "");

                    cmd.ExecuteNonQuery();
                }
            }

            txtDocumentName.Clear();
            cmbRequirementType.SelectedIndex = -1; // reset combo
            LoadDocuments();
            CheckMissingRequirements();
            MessageBox.Show("Document added successfully!");
        }
        // ============================================
        // UPDATE / REPLACE DOCUMENT
        // ============================================
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string newDocName = txtDocumentName.Text.Trim();

            if (string.IsNullOrWhiteSpace(newDocName))
            {
                MessageBox.Show("Please enter a new document name.");
                return;
            }

            if (!newDocName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Document name must end with.pdf");
                return;
            }

            if (dgvDocuments.CurrentRow == null)
            {
                MessageBox.Show("Please select a document from the list to update.");
                return;
            }

            int docID = Convert.ToInt32(dgvDocuments.CurrentRow.Cells ["DocumentID"].Value);
            string currentStatus = dgvDocuments.CurrentRow.Cells ["Status"].Value.ToString();

            if (currentStatus != "Pending")
            {
                MessageBox.Show("Only documents with 'Pending' status can be updated.");
                return;
            }

            // Check if new name already exists (excluding the current document)
            if (DocumentExistsExcluding(newDocName, docID))
            {
                MessageBox.Show("Another document with this name already exists.");
                return;
            }

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE ApplicantDocuments SET DocumentName =? WHERE DocumentID =?";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("?", newDocName);
                    cmd.Parameters.AddWithValue("?", docID);
                    cmd.Parameters.AddWithValue("?", ""); // Remarks
                    cmd.ExecuteNonQuery();
                }
            }

            txtDocumentName.Clear();
            LoadDocuments();
            CheckMissingRequirements();
            MessageBox.Show("Document updated successfully!");
        }
        // ============================================
        // DELETE DOCUMENT (Only if Pending)
        // ============================================
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDocuments.CurrentRow == null)
            {
                MessageBox.Show("No row selected.");
                return;
            }

            if (dgvDocuments.CurrentRow.Cells ["DocumentID"].Value == null)
            {
                MessageBox.Show("Cannot delete the new row placeholder. Select an existing document.");
                return;
            }

            int docID = (int)dgvDocuments.CurrentRow.Cells ["DocumentID"].Value;
            string currentStatus = dgvDocuments.CurrentRow.Cells ["Status"].Value.ToString();
            string docName = dgvDocuments.CurrentRow.Cells ["DocumentName"].Value.ToString();

            if (currentStatus != "Pending")
            {
                MessageBox.Show("Cannot delete a document that is already submitted/under review.");
                return;
            }

            DialogResult result = MessageBox.Show("Delete \"" + docName + "\"?", "Confirm Delete", MessageBoxButtons.YesNo);
            if (result != DialogResult.Yes) return;

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM ApplicantDocuments WHERE DocumentID =?";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("?", docID);
                    cmd.ExecuteNonQuery();
                }
            }

            LoadDocuments();
            CheckMissingRequirements();
            MessageBox.Show("Document deleted successfully!");
        }
        // ============================================
        // SUBMIT FOR REVIEW
        // ============================================
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Submit all pending documents for review?\n\nOnce submitted, documents cannot be deleted.", "Confirm Submit", MessageBoxButtons.YesNo);
            if (result != DialogResult.Yes) return;

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE ApplicantDocuments SET Status = 'Submitted' WHERE ApplicantID =? AND Status = 'Pending'";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("?", _applicantId);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show(rowsAffected + " document(s) submitted for review!");
                    }
                    else
                    {
                        MessageBox.Show("No pending documents to submit.");
                    }
                }
            }

            LoadDocuments();
        }
        // ============================================
        // CHECK MISSING REQUIREMENTS
        // ============================================
        private void CheckMissingRequirements()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT DocumentName FROM ApplicantDocuments WHERE ApplicantID =?";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("?", _applicantId);
                    OleDbDataReader reader = cmd.ExecuteReader();

                    var existingDocs = new System.Collections.Generic.List<string>();
                    while (reader.Read())
                    {
                        existingDocs.Add(reader ["DocumentName"].ToString());
                    }
                    reader.Close();

                    /*var missing = requiredDocs.Where(doc => !existingDocs.Contains(doc, StringComparer.OrdinalIgnoreCase)).ToList();

                    if (missing.Count == 0)
                    {
                        lblMissingRequirements.Text = "All required documents are complete!";
                        lblMissingRequirements.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        lblMissingRequirements.Text = "Missing: " + string.Join(", ", missing);
                        lblMissingRequirements.ForeColor = System.Drawing.Color.Red;
                    }*/
                }
            }
        }
        // ============================================
        // HELPER: Check if document name exists
        // ============================================
        private bool DocumentExists(string docName)
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM ApplicantDocuments WHERE ApplicantID =? AND DocumentName =?";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("?", _applicantId);
                    cmd.Parameters.AddWithValue("?", docName);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        // ============================================
        // HELPER: Check if name exists (excluding a specific ID)
        // ============================================
        private bool DocumentExistsExcluding(string docName, int excludeDocID)
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM ApplicantDocuments WHERE ApplicantID =? AND DocumentName =? AND DocumentID <>?";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("?", _applicantId);
                    cmd.Parameters.AddWithValue("?", docName);
                    cmd.Parameters.AddWithValue("?", excludeDocID);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        // ============================================
        // REFRESH
        // ============================================
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDocuments();
            CheckMissingRequirements();
        }

        private void lblApplicantID_Click(object sender, EventArgs e)
        {

        }

        private void dgvDocuments_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lblMissingRequirements_Click(object sender, EventArgs e)
        {

        }

        private void txtDocumentName_TextChanged(object sender, EventArgs e)
        {

        }
        private Dictionary<int, string> requirementTypes = new Dictionary<int, string>();

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblDocumentName_Click(object sender, EventArgs e)
        {

        }

        private void lblRequirementType_Click(object sender, EventArgs e)
        {

        }
    }
}
