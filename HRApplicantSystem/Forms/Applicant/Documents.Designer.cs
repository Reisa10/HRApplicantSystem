namespace HRApplicantSystem.Forms.Applicant
{
    partial class DocumentsForm
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
            this.lblApplicantID = new System.Windows.Forms.Label();
            this.dgvDocuments = new System.Windows.Forms.DataGridView();
            this.DocumentID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DocumentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblMissingRequirements = new System.Windows.Forms.Label();
            this.txtDocumentName = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.lblDocumentName = new System.Windows.Forms.Label();
            this.lblRequirementType = new System.Windows.Forms.Label();
            this.cmbRequirementType = new System.Windows.Forms.ComboBox();
            this.RequirementID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RequirementName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HRRemarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDocuments)).BeginInit();
            this.SuspendLayout();
            // 
            // lblApplicantID
            // 
            this.lblApplicantID.AutoSize = true;
            this.lblApplicantID.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApplicantID.Location = new System.Drawing.Point(20, 20);
            this.lblApplicantID.Name = "lblApplicantID";
            this.lblApplicantID.Size = new System.Drawing.Size(154, 25);
            this.lblApplicantID.TabIndex = 0;
            this.lblApplicantID.Text = "Applicant ID: 0";
            this.lblApplicantID.Click += new System.EventHandler(this.lblApplicantID_Click);
            // 
            // dgvDocuments
            // 
            this.dgvDocuments.AllowUserToAddRows = false;
            this.dgvDocuments.AllowUserToDeleteRows = false;
            this.dgvDocuments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDocuments.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DocumentID,
            this.RequirementID,
            this.DocumentName,
            this.RequirementName,
            this.Status,
            this.HRRemarks});
            this.dgvDocuments.Location = new System.Drawing.Point(20, 50);
            this.dgvDocuments.Name = "dgvDocuments";
            this.dgvDocuments.ReadOnly = true;
            this.dgvDocuments.RowHeadersWidth = 62;
            this.dgvDocuments.RowTemplate.Height = 28;
            this.dgvDocuments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDocuments.Size = new System.Drawing.Size(640, 200);
            this.dgvDocuments.TabIndex = 1;
            this.dgvDocuments.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDocuments_CellContentClick);
            // 
            // DocumentID
            // 
            this.DocumentID.HeaderText = "DocumentID";
            this.DocumentID.MinimumWidth = 8;
            this.DocumentID.Name = "DocumentID";
            this.DocumentID.ReadOnly = true;
            this.DocumentID.Visible = false;
            this.DocumentID.Width = 150;
            // 
            // DocumentName
            // 
            this.DocumentName.HeaderText = "Document Name"; // Updated column header visually [1]
            this.DocumentName.MinimumWidth = 8;
            this.DocumentName.Name = "DocumentName";
            this.DocumentName.ReadOnly = true;
            this.DocumentName.Width = 150;
            // 
            // Status
            // 
            this.Status.HeaderText = "Status";
            this.Status.MinimumWidth = 8;
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.Width = 150;
            // 
            // lblMissingRequirements
            // 
            this.lblMissingRequirements.AutoSize = true;
            this.lblMissingRequirements.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMissingRequirements.ForeColor = System.Drawing.Color.Red;
            this.lblMissingRequirements.Location = new System.Drawing.Point(20, 270);
            this.lblMissingRequirements.Name = "lblMissingRequirements";
            this.lblMissingRequirements.Size = new System.Drawing.Size(288, 25);
            this.lblMissingRequirements.TabIndex = 2;
            this.lblMissingRequirements.Text = "Missing Requirements: None";
            this.lblMissingRequirements.Click += new System.EventHandler(this.lblMissingRequirements_Click);
            // 
            // txtDocumentName
            // 
            this.txtDocumentName.Location = new System.Drawing.Point(20, 346);
            this.txtDocumentName.Name = "txtDocumentName";
            this.txtDocumentName.Size = new System.Drawing.Size(300, 26);
            this.txtDocumentName.TabIndex = 3;
            this.txtDocumentName.TextChanged += new System.EventHandler(this.txtDocumentName_TextChanged);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(20, 435);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(120, 30);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "Add Document";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(130, 435);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(120, 30);
            this.btnUpdate.TabIndex = 5;
            this.btnUpdate.Text = "Update Document";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(280, 435);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(120, 30);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "Delete Document";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(20, 485);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 30);
            this.btnRefresh.TabIndex = 8;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(560, 12);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(100, 30);
            this.btnBack.TabIndex = 9;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // lblDocumentName
            // 
            this.lblDocumentName.AutoSize = true;
            this.lblDocumentName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocumentName.Location = new System.Drawing.Point(20, 318);
            this.lblDocumentName.Name = "lblDocumentName";
            this.lblDocumentName.Size = new System.Drawing.Size(164, 25);
            this.lblDocumentName.TabIndex = 10;
            this.lblDocumentName.Text = "Document Name:";
            this.lblDocumentName.Click += new System.EventHandler(this.lblDocumentName_Click);
            // 
            // lblRequirementType
            // 
            this.lblRequirementType.AutoSize = true;
            this.lblRequirementType.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRequirementType.Location = new System.Drawing.Point(359, 318);
            this.lblRequirementType.Name = "lblRequirementType";
            this.lblRequirementType.Size = new System.Drawing.Size(178, 25);
            this.lblRequirementType.TabIndex = 11;
            this.lblRequirementType.Text = "Requirement Type:";
            this.lblRequirementType.Click += new System.EventHandler(this.lblRequirementType_Click);
            // 
            // cmbRequirementType
            // 
            this.cmbRequirementType.FormattingEnabled = true;
            this.cmbRequirementType.Location = new System.Drawing.Point(364, 346);
            this.cmbRequirementType.Name = "cmbRequirementType";
            this.cmbRequirementType.Size = new System.Drawing.Size(173, 28);
            this.cmbRequirementType.TabIndex = 12;
            // 
            // DocumentID
            // 
            this.DocumentID.DataPropertyName = "DocumentID";
            this.DocumentID.HeaderText = "DocumentID";
            this.DocumentID.MinimumWidth = 8;
            this.DocumentID.Name = "DocumentID";
            this.DocumentID.ReadOnly = true;
            this.DocumentID.Visible = false;
            this.DocumentID.Width = 150;
            // 
            // RequirementID
            // 
            this.RequirementID.DataPropertyName = "RequirementID";
            this.RequirementID.HeaderText = "RequirementID";
            this.RequirementID.MinimumWidth = 8;
            this.RequirementID.Name = "RequirementID";
            this.RequirementID.ReadOnly = true;
            this.RequirementID.Visible = false;
            this.RequirementID.Width = 150;
            // 
            // DocumentName
            // 
            this.DocumentName.DataPropertyName = "DocumentName";
            this.DocumentName.HeaderText = "Document Name"; // Updated column header visually [1]
            this.DocumentName.MinimumWidth = 8;
            this.DocumentName.Name = "DocumentName";
            this.DocumentName.ReadOnly = true;
            this.DocumentName.Width = 150;
            // 
            // RequirementName
            // 
            this.RequirementName.DataPropertyName = "RequirementName";
            this.RequirementName.HeaderText = "Requirement Type"; // Updated column header visually [1]
            this.RequirementName.MinimumWidth = 8;
            this.RequirementName.Name = "RequirementName";
            this.RequirementName.ReadOnly = true;
            this.RequirementName.Width = 150;
            // 
            // Status
            // 
            this.Status.DataPropertyName = "Status";
            this.Status.HeaderText = "Status";
            this.Status.MinimumWidth = 8;
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.Width = 150;
            // 
            // HRRemarks
            // 
            this.HRRemarks.DataPropertyName = "HRRemarks";
            this.HRRemarks.HeaderText = "HR Remarks";
            this.HRRemarks.MinimumWidth = 8;
            this.HRRemarks.Name = "HRRemarks";
            this.HRRemarks.ReadOnly = true;
            this.HRRemarks.Width = 150;
            // 
            // DocumentsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(678, 544); // Form height slightly adjusted
            this.Controls.Add(this.cmbRequirementType);
            this.Controls.Add(this.lblRequirementType);
            this.Controls.Add(this.lblDocumentName);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtDocumentName);
            this.Controls.Add(this.lblMissingRequirements);
            this.Controls.Add(this.dgvDocuments);
            this.Controls.Add(this.lblApplicantID);
            this.Name = "DocumentsForm";
            this.Text = "Documents";
            this.Load += new System.EventHandler(this.DocumentsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDocuments)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblApplicantID;
        private System.Windows.Forms.DataGridView dgvDocuments;
        private System.Windows.Forms.Label lblMissingRequirements;
        private System.Windows.Forms.TextBox txtDocumentName;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Label lblDocumentName;
        private System.Windows.Forms.Label lblRequirementType;
        private System.Windows.Forms.ComboBox cmbRequirementType;
        private System.Windows.Forms.DataGridViewTextBoxColumn DocumentID;
        private System.Windows.Forms.DataGridViewTextBoxColumn RequirementID;
        private System.Windows.Forms.DataGridViewTextBoxColumn DocumentName;
        private System.Windows.Forms.DataGridViewTextBoxColumn RequirementName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn HRRemarks;
    }
}