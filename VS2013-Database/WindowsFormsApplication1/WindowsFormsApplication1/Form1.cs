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
using System.Windows.Forms.DataVisualization.Charting;

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

            chart1.Series.Add("test");
            chart1.Series["test"].ChartType = SeriesChartType.Bar;
            chart1.Series["test"].Points.AddXY(0, 20);

            chart1.Series.Add("test2");
            chart1.Series["test2"].ChartType = SeriesChartType.Bar;
            chart1.Series["test2"].Points.AddXY(1, 30);

            chart1.Series.Add("test3");
            chart1.Series["test3"].ChartType = SeriesChartType.Bar;
            chart1.Series["test3"].Points.AddXY(1, 5);


        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
