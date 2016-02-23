using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace WindowsFormsApplication1
{
    class DatabaseShiz
    {
        private SqlConnection dbCon = null;
        private SqlDataReader reader = null;

        public delegate void  ForEachResult(IDataRecord data);

        public DatabaseShiz(String server, String dbName, String username) {
            if (!connectDb(server, dbName, username)) throw new Exception("Failed to connect to db");
        }

        public SqlDataReader runQuery(String query) {

            SqlCommand cmd = dbCon.CreateCommand();
            cmd.CommandText = query;
            reader = cmd.ExecuteReader();
            return reader;
        }

        public void forEachResult( ForEachResult fn) {
            IDataRecord data = reader;
            while (reader.Read())
            {
                fn(data);
            }

            reader.Close();
        }


        private bool connectDb(String server,String dbName, String username) {
            String conStr = "server="+ server +";" +
                "Trusted_Connection=yes;" +
                "User ID="+username+";" +
                "database="+dbName+";";

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
