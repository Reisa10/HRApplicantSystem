namespace HRApplicantSystem.Forms.HR
{
    partial class MaintenanceForm
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabDept = new System.Windows.Forms.TabPage();
            this.tabPos = new System.Windows.Forms.TabPage();
            this.tabEmp = new System.Windows.Forms.TabPage();
            this.tabReq = new System.Windows.Forms.TabPage();
            this.tabInt = new System.Windows.Forms.TabPage();
            this.tabAss = new System.Windows.Forms.TabPage();

            this.dgvDepartments = new System.Windows.Forms.DataGridView();
            this.dgvPositions = new System.Windows.Forms.DataGridView();
            this.dgvEmployment = new System.Windows.Forms.DataGridView();
            this.dgvRequirements = new System.Windows.Forms.DataGridView();
            this.dgvInterviews = new System.Windows.Forms.DataGridView();
            this.dgvAssessments = new System.Windows.Forms.DataGridView();

            this.txtDeptName = new System.Windows.Forms.TextBox();
            this.txtDeptDesc = new System.Windows.Forms.TextBox();
            this.chkDeptActive = new System.Windows.Forms.CheckBox();
            this.btnSaveDept = new System.Windows.Forms.Button();
            this.btnClearDept = new System.Windows.Forms.Button();

            this.txtPosName = new System.Windows.Forms.TextBox();
            this.cmbPosDept = new System.Windows.Forms.ComboBox();
            this.chkPosActive = new System.Windows.Forms.CheckBox();
            this.btnSavePos = new System.Windows.Forms.Button();
            this.btnClearPos = new System.Windows.Forms.Button();

            this.txtEmpTypeName = new System.Windows.Forms.TextBox();
            this.chkEmpTypeActive = new System.Windows.Forms.CheckBox();
            this.btnSaveEmp = new System.Windows.Forms.Button();
            this.btnClearEmp = new System.Windows.Forms.Button();

            this.txtReqName = new System.Windows.Forms.TextBox();
            this.chkReqRequired = new System.Windows.Forms.CheckBox();
            this.btnSaveReq = new System.Windows.Forms.Button();
            this.btnClearReq = new System.Windows.Forms.Button();

            this.txtIntTypeName = new System.Windows.Forms.TextBox();
            this.chkIntActive = new System.Windows.Forms.CheckBox();
            this.btnSaveInt = new System.Windows.Forms.Button();
            this.btnClearInt = new System.Windows.Forms.Button();

            this.txtAssTypeName = new System.Windows.Forms.TextBox();
            this.chkAssActive = new System.Windows.Forms.CheckBox();
            this.btnSaveAss = new System.Windows.Forms.Button();
            this.btnClearAss = new System.Windows.Forms.Button();

            this.tabControl.SuspendLayout();
            this.SuspendLayout();

            // Set Tab Page Titles explicitly
            this.tabDept.Text = "Departments";
            this.tabPos.Text = "Positions";
            this.tabEmp.Text = "Employment Types";
            this.tabReq.Text = "Requirement Types";
            this.tabInt.Text = "Interview Types";
            this.tabAss.Text = "Assessment Types";

            // TabControl Size & Alignment Settings
            this.tabControl.Controls.Add(this.tabDept);
            this.tabControl.Controls.Add(this.tabPos);
            this.tabControl.Controls.Add(this.tabEmp);
            this.tabControl.Controls.Add(this.tabReq);
            this.tabControl.Controls.Add(this.tabInt);
            this.tabControl.Controls.Add(this.tabAss);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(984, 611);

            // Configure Datagrid columns programmatically
            ConfigureDepartmentsGrid();
            ConfigurePositionsGrid();
            ConfigureEmploymentGrid();
            ConfigureRequirementsGrid();
            ConfigureInterviewsGrid();
            ConfigureAssessmentsGrid();

            // Setup programmatic layouts on tabs
            SetupProgrammaticTab(this.tabDept, this.dgvDepartments, CreateDeptFormPanel());
            SetupProgrammaticTab(this.tabPos, this.dgvPositions, CreatePosFormPanel());
            SetupProgrammaticTab(this.tabEmp, this.dgvEmployment, CreateEmpFormPanel());
            SetupProgrammaticTab(this.tabReq, this.dgvRequirements, CreateReqFormPanel());
            SetupProgrammaticTab(this.tabInt, this.dgvInterviews, CreateIntFormPanel());
            SetupProgrammaticTab(this.tabAss, this.dgvAssessments, CreateAssFormPanel());

            // Main Form Settings
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.ClientSize = new System.Drawing.Size(984, 611);
            this.Controls.Add(this.tabControl);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MinimumSize = new System.Drawing.Size(1024, 660);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "System Configuration & Maintenance Hub";
            this.Load += new System.EventHandler(this.MaintenanceForm_Load);
            this.tabControl.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void SetupProgrammaticTab(System.Windows.Forms.TabPage tab, System.Windows.Forms.DataGridView dgv, System.Windows.Forms.Panel formPanel)
        {
            // Tab page background color setting
            tab.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);

            System.Windows.Forms.TableLayoutPanel mainLayout = new System.Windows.Forms.TableLayoutPanel
            {
                Dock = System.Windows.Forms.DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new System.Windows.Forms.Padding(15)
            };
            mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58F));
            mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42F));
            mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));

            // LEFT SIDE CARD (Filter Box + Grid View)
            System.Windows.Forms.Panel leftCard = new System.Windows.Forms.Panel
            {
                Dock = System.Windows.Forms.DockStyle.Fill,
                BackColor = System.Drawing.Color.White,
                Padding = new System.Windows.Forms.Padding(15),
                Margin = new System.Windows.Forms.Padding(0, 0, 10, 0)
            };

            // Draw card subtle border
            leftCard.Paint += (s, e) => {
                using (System.Drawing.Pen p = new System.Drawing.Pen(System.Drawing.Color.FromArgb(226, 232, 240), 1))
                {
                    e.Graphics.DrawRectangle(p, 0, 0, leftCard.Width - 1, leftCard.Height - 1);
                }
            };

            // Modern Quick Search Filter
            System.Windows.Forms.Panel searchPanel = new System.Windows.Forms.Panel { Dock = System.Windows.Forms.DockStyle.Top, Height = 45 };
            System.Windows.Forms.Label lblSearch = new System.Windows.Forms.Label
            {
                Text = "🔍 QUICK FILTER:",
                Location = new System.Drawing.Point(0, 12),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.FromArgb(100, 116, 139)
            };
            System.Windows.Forms.TextBox txtSearch = new System.Windows.Forms.TextBox
            {
                Location = new System.Drawing.Point(100, 9),
                Width = 260,
                Font = new System.Drawing.Font("Segoe UI", 9.5F),
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            };

            txtSearch.Tag = dgv;
            txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);

            searchPanel.Controls.Add(lblSearch);
            searchPanel.Controls.Add(txtSearch);

            // Grid Layout Styling
            dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.ReadOnly = true;
            dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgv.BackgroundColor = System.Drawing.Color.White;
            dgv.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgv.RowHeadersVisible = false;
            dgv.GridColor = System.Drawing.Color.FromArgb(241, 245, 249);
            dgv.EnableHeadersVisualStyles = false;
            dgv.RowTemplate.Height = 32;

            // Grid Columns Styling
            dgv.ColumnHeadersHeight = 35;
            dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(44, 62, 80);

            // Grid Content Rows Styling
            dgv.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F);
            dgv.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            dgv.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            dgv.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);

            System.Windows.Forms.Panel dgvWrapper = new System.Windows.Forms.Panel { Dock = System.Windows.Forms.DockStyle.Fill, Padding = new System.Windows.Forms.Padding(0, 5, 0, 0) };
            dgvWrapper.Controls.Add(dgv);

            leftCard.Controls.Add(dgvWrapper);
            leftCard.Controls.Add(searchPanel);

            // RIGHT SIDE CARD (Form Panel wrap)
            formPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            formPanel.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);

            mainLayout.Controls.Add(leftCard, 0, 0);
            mainLayout.Controls.Add(formPanel, 1, 0);
            tab.Controls.Add(mainLayout);
        }

        // --- Column Initializations ---
        private void ConfigureDepartmentsGrid()
        {
            this.dgvDepartments.AutoGenerateColumns = false;
            this.dgvDepartments.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = "DepartmentName", HeaderText = "Department Name", Name = "DepartmentName", FillWeight = 150F });
            this.dgvDepartments.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = "Description", HeaderText = "Description", Name = "Description", FillWeight = 200F });
            this.dgvDepartments.Columns.Add(new System.Windows.Forms.DataGridViewCheckBoxColumn { DataPropertyName = "IsActive", HeaderText = "Active", Name = "IsActive", FillWeight = 50F });

            // Background reference column
            this.dgvDepartments.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = "DepartmentID", Name = "DepartmentID", Visible = false });
        }

        private void ConfigurePositionsGrid()
        {
            this.dgvPositions.AutoGenerateColumns = false;
            this.dgvPositions.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = "PositionName", HeaderText = "Position Name", Name = "PositionName", FillWeight = 150F });
            this.dgvPositions.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = "DepartmentName", HeaderText = "Department", Name = "DepartmentName", FillWeight = 150F });
            this.dgvPositions.Columns.Add(new System.Windows.Forms.DataGridViewCheckBoxColumn { DataPropertyName = "IsActive", HeaderText = "Active", Name = "IsActive", FillWeight = 50F });

            // Background reference columns
            this.dgvPositions.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = "PositionID", Name = "PositionID", Visible = false });
            this.dgvPositions.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = "DepartmentID", Name = "DepartmentID", Visible = false });
        }

        private void ConfigureEmploymentGrid()
        {
            this.dgvEmployment.AutoGenerateColumns = false;
            this.dgvEmployment.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = "TypeName", HeaderText = "Employment Type Name", Name = "TypeName", FillWeight = 200F });
            this.dgvEmployment.Columns.Add(new System.Windows.Forms.DataGridViewCheckBoxColumn { DataPropertyName = "IsActive", HeaderText = "Active", Name = "IsActive", FillWeight = 50F });

            // Background reference column
            this.dgvEmployment.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = "EmploymentTypeID", Name = "EmploymentTypeID", Visible = false });
        }

        private void ConfigureRequirementsGrid()
        {
            this.dgvRequirements.AutoGenerateColumns = false;
            this.dgvRequirements.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = "RequirementName", HeaderText = "Requirement Name", Name = "RequirementName", FillWeight = 200F });
            this.dgvRequirements.Columns.Add(new System.Windows.Forms.DataGridViewCheckBoxColumn { DataPropertyName = "IsRequired", HeaderText = "Is Required", Name = "IsRequired", FillWeight = 80F });

            // Background reference column
            this.dgvRequirements.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = "RequirementTypeID", Name = "RequirementTypeID", Visible = false });
        }

        private void ConfigureInterviewsGrid()
        {
            this.dgvInterviews.AutoGenerateColumns = false;
            this.dgvInterviews.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = "TypeName", HeaderText = "Interview Type Name", Name = "TypeName", FillWeight = 200F });
            this.dgvInterviews.Columns.Add(new System.Windows.Forms.DataGridViewCheckBoxColumn { DataPropertyName = "IsActive", HeaderText = "Active", Name = "IsActive", FillWeight = 50F });

            // Background reference column
            this.dgvInterviews.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = "InterviewTypeID", Name = "InterviewTypeID", Visible = false });
        }

        private void ConfigureAssessmentsGrid()
        {
            this.dgvAssessments.AutoGenerateColumns = false;
            this.dgvAssessments.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = "TypeName", HeaderText = "Assessment Type Name", Name = "TypeName", FillWeight = 200F });
            this.dgvAssessments.Columns.Add(new System.Windows.Forms.DataGridViewCheckBoxColumn { DataPropertyName = "IsActive", HeaderText = "Active", Name = "IsActive", FillWeight = 50F });

            // Background reference column
            this.dgvAssessments.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = "AssessmentTypeID", Name = "AssessmentTypeID", Visible = false });
        }

        private System.Windows.Forms.Label CreateLabel(string text, int x, int y)
        {
            return new System.Windows.Forms.Label
            {
                Text = text,
                Location = new System.Drawing.Point(x, y),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.FromArgb(71, 85, 105)
            };
        }

        private System.Windows.Forms.Label CreateHeaderLabel(string text, int x, int y)
        {
            return new System.Windows.Forms.Label
            {
                Text = text,
                Location = new System.Drawing.Point(x, y),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.FromArgb(30, 41, 59)
            };
        }

        private System.Windows.Forms.Panel CreateDivider(int x, int y, int width)
        {
            return new System.Windows.Forms.Panel
            {
                Location = new System.Drawing.Point(x, y),
                Size = new System.Drawing.Size(width, 1),
                BackColor = System.Drawing.Color.FromArgb(226, 232, 240)
            };
        }

        private void ConfigureButton(System.Windows.Forms.Button btn, string text, int x, int y, System.Drawing.Color color)
        {
            btn.Text = text;
            btn.Location = new System.Drawing.Point(x, y);
            btn.Size = new System.Drawing.Size(95, 34);
            btn.BackColor = color;
            btn.ForeColor = System.Drawing.Color.White;
            btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = System.Windows.Forms.Cursors.Hand;
            btn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
        }

        private System.Windows.Forms.Panel CreateCardBase()
        {
            System.Windows.Forms.Panel panel = new System.Windows.Forms.Panel
            {
                Dock = System.Windows.Forms.DockStyle.Fill,
                Padding = new System.Windows.Forms.Padding(20),
                BackColor = System.Drawing.Color.White
            };

            panel.Paint += (s, e) => {
                using (System.Drawing.Pen p = new System.Drawing.Pen(System.Drawing.Color.FromArgb(226, 232, 240), 1))
                {
                    e.Graphics.DrawRectangle(p, 0, 0, panel.Width - 1, panel.Height - 1);
                }
            };

            return panel;
        }

        private System.Windows.Forms.Panel CreateDeptFormPanel()
        {
            System.Windows.Forms.Panel panel = CreateCardBase();

            panel.Controls.Add(CreateHeaderLabel("DEPARTMENT CONFIGURATION", 20, 15));
            panel.Controls.Add(CreateDivider(20, 42, 300));

            panel.Controls.Add(CreateLabel("Department Name:", 20, 55));
            this.txtDeptName.Location = new System.Drawing.Point(20, 75);
            this.txtDeptName.Size = new System.Drawing.Size(300, 23);
            this.txtDeptName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDeptName.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            panel.Controls.Add(this.txtDeptName);

            panel.Controls.Add(CreateLabel("Description:", 20, 115));
            this.txtDeptDesc.Location = new System.Drawing.Point(20, 135);
            this.txtDeptDesc.Size = new System.Drawing.Size(300, 80);
            this.txtDeptDesc.Multiline = true;
            this.txtDeptDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDeptDesc.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            panel.Controls.Add(this.txtDeptDesc);

            this.chkDeptActive.Text = "Is Active";
            this.chkDeptActive.Location = new System.Drawing.Point(20, 230);
            this.chkDeptActive.Checked = true;
            this.chkDeptActive.AutoSize = true;
            this.chkDeptActive.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkDeptActive.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            panel.Controls.Add(this.chkDeptActive);

            ConfigureButton(this.btnSaveDept, "Save", 20, 275, System.Drawing.Color.FromArgb(37, 99, 235));
            ConfigureButton(this.btnClearDept, "Clear", 122, 275, System.Drawing.Color.FromArgb(100, 116, 139));

            panel.Controls.Add(this.btnSaveDept);
            panel.Controls.Add(this.btnClearDept);

            this.btnSaveDept.Click += new System.EventHandler(this.btnSaveDept_Click);
            this.btnClearDept.Click += new System.EventHandler(this.btnClearDept_Click);
            this.dgvDepartments.SelectionChanged += new System.EventHandler(this.dgvDepartments_SelectionChanged);

            return panel;
        }

        private System.Windows.Forms.Panel CreatePosFormPanel()
        {
            System.Windows.Forms.Panel panel = CreateCardBase();

            panel.Controls.Add(CreateHeaderLabel("POSITION CONFIGURATION", 20, 15));
            panel.Controls.Add(CreateDivider(20, 42, 300));

            panel.Controls.Add(CreateLabel("Position Name:", 20, 55));
            this.txtPosName.Location = new System.Drawing.Point(20, 75);
            this.txtPosName.Size = new System.Drawing.Size(300, 23);
            this.txtPosName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPosName.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            panel.Controls.Add(this.txtPosName);

            panel.Controls.Add(CreateLabel("Department:", 20, 115));
            this.cmbPosDept.Location = new System.Drawing.Point(20, 135);
            this.cmbPosDept.Size = new System.Drawing.Size(300, 23);
            this.cmbPosDept.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPosDept.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbPosDept.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            panel.Controls.Add(this.cmbPosDept);

            this.chkPosActive.Text = "Is Active";
            this.chkPosActive.Location = new System.Drawing.Point(20, 180);
            this.chkPosActive.Checked = true;
            this.chkPosActive.AutoSize = true;
            this.chkPosActive.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkPosActive.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            panel.Controls.Add(this.chkPosActive);

            ConfigureButton(this.btnSavePos, "Save", 20, 225, System.Drawing.Color.FromArgb(37, 99, 235));
            ConfigureButton(this.btnClearPos, "Clear", 122, 225, System.Drawing.Color.FromArgb(100, 116, 139));

            panel.Controls.Add(this.btnSavePos);
            panel.Controls.Add(this.btnClearPos);

            this.btnSavePos.Click += new System.EventHandler(this.btnSavePos_Click);
            this.btnClearPos.Click += new System.EventHandler(this.btnClearPos_Click);
            this.dgvPositions.SelectionChanged += new System.EventHandler(this.dgvPositions_SelectionChanged);

            return panel;
        }

        private System.Windows.Forms.Panel CreateEmpFormPanel()
        {
            System.Windows.Forms.Panel panel = CreateCardBase();

            panel.Controls.Add(CreateHeaderLabel("EMPLOYMENT CONFIGURATION", 20, 15));
            panel.Controls.Add(CreateDivider(20, 42, 300));

            panel.Controls.Add(CreateLabel("Employment Type Name:", 20, 55));
            this.txtEmpTypeName.Location = new System.Drawing.Point(20, 75);
            this.txtEmpTypeName.Size = new System.Drawing.Size(300, 23);
            this.txtEmpTypeName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmpTypeName.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            panel.Controls.Add(this.txtEmpTypeName);

            this.chkEmpTypeActive.Text = "Is Active";
            this.chkEmpTypeActive.Location = new System.Drawing.Point(20, 120);
            this.chkEmpTypeActive.Checked = true;
            this.chkEmpTypeActive.AutoSize = true;
            this.chkEmpTypeActive.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkEmpTypeActive.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            panel.Controls.Add(this.chkEmpTypeActive);

            ConfigureButton(this.btnSaveEmp, "Save", 20, 165, System.Drawing.Color.FromArgb(37, 99, 235));
            ConfigureButton(this.btnClearEmp, "Clear", 122, 165, System.Drawing.Color.FromArgb(100, 116, 139));

            panel.Controls.Add(this.btnSaveEmp);
            panel.Controls.Add(this.btnClearEmp);

            this.btnSaveEmp.Click += new System.EventHandler(this.btnSaveEmp_Click);
            this.btnClearEmp.Click += new System.EventHandler(this.btnClearEmp_Click);
            this.dgvEmployment.SelectionChanged += new System.EventHandler(this.dgvEmployment_SelectionChanged);

            return panel;
        }

        private System.Windows.Forms.Panel CreateReqFormPanel()
        {
            System.Windows.Forms.Panel panel = CreateCardBase();

            panel.Controls.Add(CreateHeaderLabel("REQUIREMENT CONFIGURATION", 20, 15));
            panel.Controls.Add(CreateDivider(20, 42, 300));

            panel.Controls.Add(CreateLabel("Requirement Name:", 20, 55));
            this.txtReqName.Location = new System.Drawing.Point(20, 75);
            this.txtReqName.Size = new System.Drawing.Size(300, 23);
            this.txtReqName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtReqName.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            panel.Controls.Add(this.txtReqName);

            this.chkReqRequired.Text = "Is Required";
            this.chkReqRequired.Location = new System.Drawing.Point(20, 120);
            this.chkReqRequired.Checked = true;
            this.chkReqRequired.AutoSize = true;
            this.chkReqRequired.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkReqRequired.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            panel.Controls.Add(this.chkReqRequired);

            ConfigureButton(this.btnSaveReq, "Save", 20, 165, System.Drawing.Color.FromArgb(37, 99, 235));
            ConfigureButton(this.btnClearReq, "Clear", 122, 165, System.Drawing.Color.FromArgb(100, 116, 139));

            panel.Controls.Add(this.btnSaveReq);
            panel.Controls.Add(this.btnClearReq);

            this.btnSaveReq.Click += new System.EventHandler(this.btnSaveReq_Click);
            this.btnClearReq.Click += new System.EventHandler(this.btnClearReq_Click);
            this.dgvRequirements.SelectionChanged += new System.EventHandler(this.dgvRequirements_SelectionChanged);

            return panel;
        }

        private System.Windows.Forms.Panel CreateIntFormPanel()
        {
            System.Windows.Forms.Panel panel = CreateCardBase();

            panel.Controls.Add(CreateHeaderLabel("INTERVIEW CONFIGURATION", 20, 15));
            panel.Controls.Add(CreateDivider(20, 42, 300));

            panel.Controls.Add(CreateLabel("Interview Type Name:", 20, 55));
            this.txtIntTypeName.Location = new System.Drawing.Point(20, 75);
            this.txtIntTypeName.Size = new System.Drawing.Size(300, 23);
            this.txtIntTypeName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtIntTypeName.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            panel.Controls.Add(this.txtIntTypeName);

            this.chkIntActive.Text = "Is Active";
            this.chkIntActive.Location = new System.Drawing.Point(20, 120);
            this.chkIntActive.Checked = true;
            this.chkIntActive.AutoSize = true;
            this.chkIntActive.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkIntActive.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            panel.Controls.Add(this.chkIntActive);

            ConfigureButton(this.btnSaveInt, "Save", 20, 165, System.Drawing.Color.FromArgb(37, 99, 235));
            ConfigureButton(this.btnClearInt, "Clear", 122, 165, System.Drawing.Color.FromArgb(100, 116, 139));

            panel.Controls.Add(this.btnSaveInt);
            panel.Controls.Add(this.btnClearInt);

            this.btnSaveInt.Click += new System.EventHandler(this.btnSaveInt_Click);
            this.btnClearInt.Click += new System.EventHandler(this.btnClearInt_Click);
            this.dgvInterviews.SelectionChanged += new System.EventHandler(this.dgvInterviews_SelectionChanged);

            return panel;
        }

        private System.Windows.Forms.Panel CreateAssFormPanel()
        {
            System.Windows.Forms.Panel panel = CreateCardBase();

            panel.Controls.Add(CreateHeaderLabel("ASSESSMENT CONFIGURATION", 20, 15));
            panel.Controls.Add(CreateDivider(20, 42, 300));

            panel.Controls.Add(CreateLabel("Assessment Type Name:", 20, 55));
            this.txtAssTypeName.Location = new System.Drawing.Point(20, 75);
            this.txtAssTypeName.Size = new System.Drawing.Size(300, 23);
            this.txtAssTypeName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAssTypeName.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            panel.Controls.Add(this.txtAssTypeName);

            this.chkAssActive.Text = "Is Active";
            this.chkAssActive.Location = new System.Drawing.Point(20, 120);
            this.chkAssActive.Checked = true;
            this.chkAssActive.AutoSize = true;
            this.chkAssActive.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkAssActive.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            panel.Controls.Add(this.chkAssActive);

            ConfigureButton(this.btnSaveAss, "Save", 20, 165, System.Drawing.Color.FromArgb(37, 99, 235));
            ConfigureButton(this.btnClearAss, "Clear", 122, 165, System.Drawing.Color.FromArgb(100, 116, 139));

            panel.Controls.Add(this.btnSaveAss);
            panel.Controls.Add(this.btnClearAss);

            this.btnSaveAss.Click += new System.EventHandler(this.btnSaveAss_Click);
            this.btnClearAss.Click += new System.EventHandler(this.btnClearAss_Click);
            this.dgvAssessments.SelectionChanged += new System.EventHandler(this.dgvAssessments_SelectionChanged);

            return panel;
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabDept;
        private System.Windows.Forms.TabPage tabPos;
        private System.Windows.Forms.TabPage tabEmp;
        private System.Windows.Forms.TabPage tabReq;
        private System.Windows.Forms.TabPage tabInt;
        private System.Windows.Forms.TabPage tabAss;

        private System.Windows.Forms.DataGridView dgvDepartments;
        private System.Windows.Forms.DataGridView dgvPositions;
        private System.Windows.Forms.DataGridView dgvEmployment;
        private System.Windows.Forms.DataGridView dgvRequirements;
        private System.Windows.Forms.DataGridView dgvInterviews;
        private System.Windows.Forms.DataGridView dgvAssessments;

        private System.Windows.Forms.TextBox txtDeptName;
        private System.Windows.Forms.TextBox txtDeptDesc;
        private System.Windows.Forms.CheckBox chkDeptActive;
        private System.Windows.Forms.Button btnSaveDept;
        private System.Windows.Forms.Button btnClearDept;

        private System.Windows.Forms.TextBox txtPosName;
        private System.Windows.Forms.ComboBox cmbPosDept;
        private System.Windows.Forms.CheckBox chkPosActive;
        private System.Windows.Forms.Button btnSavePos;
        private System.Windows.Forms.Button btnClearPos;

        private System.Windows.Forms.TextBox txtEmpTypeName;
        private System.Windows.Forms.CheckBox chkEmpTypeActive;
        private System.Windows.Forms.Button btnSaveEmp;
        private System.Windows.Forms.Button btnClearEmp;

        private System.Windows.Forms.TextBox txtReqName;
        private System.Windows.Forms.CheckBox chkReqRequired;
        private System.Windows.Forms.Button btnSaveReq;
        private System.Windows.Forms.Button btnClearReq;

        private System.Windows.Forms.TextBox txtIntTypeName;
        private System.Windows.Forms.CheckBox chkIntActive;
        private System.Windows.Forms.Button btnSaveInt;
        private System.Windows.Forms.Button btnClearInt;

        private System.Windows.Forms.TextBox txtAssTypeName;
        private System.Windows.Forms.CheckBox chkAssActive;
        private System.Windows.Forms.Button btnSaveAss;
        private System.Windows.Forms.Button btnClearAss;
    }
}