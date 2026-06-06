using HRApplicantSystem.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HRApplicantSystem.Classes;

namespace HRApplicantSystem.Forms.HR
{
    public partial class HiringDecisionForm : Form
    {
        public HiringDecisionForm()
        {
            InitializeComponent();
            
            if (UserSession.Role != "HR Manager" && UserSession.Role != "Admin")
            {
                MessageBox.Show("Access Denied. Only HR Manager or Admin can make hiring decisions.",
                    "Unauthorized", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Load += (s, e) => this.Close();
            }
        }

        private void HiringDecisionForm_Load(object sender, EventArgs e)
        {

        }
    }
}
