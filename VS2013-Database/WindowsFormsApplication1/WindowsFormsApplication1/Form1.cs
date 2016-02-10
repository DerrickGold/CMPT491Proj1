using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

            TreeNode parent = new TreeNode("top level");
            TreeNode child = new TreeNode("öption 1");
            parent.Nodes.Add(child);
            parent.Nodes.Add(new TreeNode("öption 2"));
            TreeNode something = new TreeNode("level2");
            child.Nodes.Add(something);

            treeView1.Nodes.Add(parent);
            treeView1.Nodes.Add("something2");
           
        }

    }
}
