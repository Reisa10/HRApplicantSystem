using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;

namespace HRApplicantSystem.Database
{


    public class DBConnection
    {
        private static string dbPath =
            Path.Combine(Application.StartupPath,
            @"Database\HRDatabase.accdb");

        private static string connectionString =
            $@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source={dbPath};";

        public static OleDbConnection GetConnection()
        {
            try
            {
                // Check if database exists
                if (!File.Exists(dbPath))
                {
                    MessageBox.Show(
                        "Database file not found!\n\n" +
                        "Expected Location:\n" +
                        dbPath,
                        "Database Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return null;
                }

                return new OleDbConnection(connectionString);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Database connection error:\n\n" +
                    ex.Message,
                    "Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return null;
            }
        }
    }
}
