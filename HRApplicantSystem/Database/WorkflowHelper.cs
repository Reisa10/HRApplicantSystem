using HRApplicantSystem.Classes;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRApplicantSystem.Database
{
    public static class WorkflowHelper
    {
        public static void InsertStatusHistory(OleDbConnection con, OleDbTransaction trans, int applicationId, string status)
        {
            string query = @"
                INSERT INTO [ApplicationStatusHistory]
                ([ApplicationID], [Status], [DateChanged])
                VALUES (?, ?, ?)";

            using (OleDbCommand cmd = new OleDbCommand(query, con, trans))
            {
                cmd.Parameters.Add("@ApplicationID", OleDbType.Integer).Value = applicationId;
                cmd.Parameters.Add("@Status", OleDbType.VarWChar).Value = status;
                cmd.Parameters.Add("@DateChanged", OleDbType.Date).Value = DateTime.Now;
                cmd.ExecuteNonQuery();
            }
        }

        public static void InsertAuditTrail(OleDbConnection con, OleDbTransaction trans, string action)
        {
            string query = @"
                INSERT INTO [AuditTrail]
                ([Action], [DateCreated], [UserID])
                VALUES (?, ?, ?)";

            using (OleDbCommand cmd = new OleDbCommand(query, con, trans))
            {
                cmd.Parameters.Add("@Action", OleDbType.VarWChar).Value = action;
                cmd.Parameters.Add("@DateCreated", OleDbType.Date).Value = DateTime.Now;
                cmd.Parameters.Add("@UserID", OleDbType.Integer).Value = UserSession.UserID > 0 ? UserSession.UserID : 1;
                cmd.ExecuteNonQuery();
            }
        }
    }
}
