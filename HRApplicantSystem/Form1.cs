using HRApplicantSystem.Database;
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

namespace HRApplicantSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bool connected = DatabaseHelper.IsDatabaseAvailable();

            MessageBox.Show(
                connected ? "Database Ready" : "Database Not Found");
        }


        private void btnTest_Click_1(object sender, EventArgs e)
        {
            OleDbConnection con = DBConnection.GetConnection();

            if (con == null)
                return;

            try
            {
                con.Open();

                MessageBox.Show(
                    "Database Connected Successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Connection Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
