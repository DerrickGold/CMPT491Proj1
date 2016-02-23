using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace WindowsFormsApplication1
{
    class DatabaseShiz
    {
        private const String databaseName = "CMPT491"; 
        private SqlConnection dbCon = null;


        public DatabaseShiz(String server, String username) {
            if (!connectDb(server, username)) throw new Exception("Failed to connect to db");
        }

        public SqlDataReader runQuery(String query) {

            SqlCommand cmd = dbCon.CreateCommand();
            cmd.CommandText = query;
            return cmd.ExecuteReader();
        }


        private bool connectDb(String server, String username) {
            String conStr = "server="+ server +";" +
                "Trusted_Connection=yes;" +
                "User ID="+username+";" +
                "database="+databaseName+";";

            dbCon = new SqlConnection(conStr);
            try
            {

                dbCon.Open();
                //MessageBox.Show("db successfully connected");
                return true;
            }
            catch (SqlException e)
            {
                //MessageBox.Show("failed to connect to db: " + e.Message);

            }
            return false; 
        }
    }


}
