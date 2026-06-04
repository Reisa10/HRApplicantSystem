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
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=HRDatabase.accdb;";
        private int _applicantId = 0;

        private string[] requiredDocs = { "Resume.pdf", "Diploma.pdf", "ValidID.pdf" };

        public DocumentsForm(int applicantId)
        {
            InitializeComponent();
            _applicantId = applicantId;
            this.Load += DocumentsForm_Load;
        }
        private void DocumentsForm_Load(object sender, EventArgs e)
        {
            lblApplicantID.Text = "Applicant ID: " + _applicantId.ToString();
            LoadDocuments();
            CheckMissingRequirements();
        }
        // ============================================
        // LOAD DOCUMENTS
        // ============================================
        private void LoadDocuments()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT DocumentID, DocumentName, Status FROM ApplicantDocuments WHERE ApplicantID =?";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("?", _applicantId);
                    OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvDocuments.DataSource = dt;

                    if (dgvDocuments.Columns ["DocumentID"]!= null)
                        dgvDocuments.Columns ["DocumentID"].Visible = false;
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

            if (DocumentExists(docName))
            {
                MessageBox.Show("A document with this name already exists. Use Update to replace it.");
                return;
            }

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO ApplicantDocuments (ApplicantID, DocumentName, Status) VALUES (?,?,?)";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("?", _applicantId);
                    cmd.Parameters.AddWithValue("?", docName);
                    cmd.Parameters.AddWithValue("?", "Pending");
                    cmd.ExecuteNonQuery();
                }
            }

            txtDocumentName.Clear();
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
                MessageBox.Show("Please select a document to delete.");
                return;
            }

            int docID = Convert.ToInt32(dgvDocuments.CurrentRow.Cells ["DocumentID"].Value);
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

                    var missing = requiredDocs.Where(doc => !existingDocs.Contains(doc, StringComparer.OrdinalIgnoreCase)).ToList();

                    if (missing.Count == 0)
                    {
                        lblMissingRequirements.Text = "All required documents are complete!";
                        lblMissingRequirements.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        lblMissingRequirements.Text = "Missing: " + string.Join(", ", missing);
                        lblMissingRequirements.ForeColor = System.Drawing.Color.Red;
                    }
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
    }
}
