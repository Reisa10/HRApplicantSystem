using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.OleDb;

namespace HRApplicantSystem.Database
{
    public class DatabaseHelper
    {
        public static bool IsDatabaseAvailable()
        {
            OleDbConnection con = DBConnection.GetConnection();

            if (con == null)
                return false;

            try
            {
                con.Open();
                con.Close();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}