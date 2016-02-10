using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsApplication1
{

    public partial class Form1 : Form
    {
        public void connectDB()
        {
            
        }
        
        public Form1()
        {
            InitializeComponent();

            DatabaseShiz theDB = null;

            try
            {
                theDB = new DatabaseShiz("COMPSCI-PC", "compsci");
                MessageBox.Show("Connected to database.");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                Application.Exit();
            }

            SqlDataReader reader = theDB.runQuery("SELECT * from dbo.Data$");


            TreeNode parent = new TreeNode("top level");
            for (int i = 0; i < 20; i++)
            {
                if (reader.Read()) {
                    IDataRecord data = reader;

                    TreeNode temp = new TreeNode(data[2].ToString());
                    parent.Nodes.Add(temp);
                }

            }

                treeView1.Nodes.Add(parent);
            treeView1.Nodes.Add("something2");
           
        }

    }
}
