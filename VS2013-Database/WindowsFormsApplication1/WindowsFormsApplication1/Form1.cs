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
        DatabaseShiz theDB = null;
        String tableName = "dbo.stores";

        TreeNode makeLocationTree()
        {
            //generate location tree
            // All Countries
            //  Countries
            //      region
            //          province
            //              city
            //              store name

            //start with all countries
            TreeNode all = new TreeNode("All Location");


            SqlDataReader reader = theDB.runQuery("SELECT distinct country from " + tableName);
            IDataRecord data = reader;
            while (reader.Read())
            {
                TreeNode temp = new TreeNode(data[0].ToString());
                all.Nodes.Add(temp);
            }

            reader.Close();
            //forception
            foreach (TreeNode country in all.Nodes) {
                //get all regions for a country
                reader = theDB.runQuery("SELECT distinct region from " + tableName + " where country = '" + country.Text + "';");
                data = reader;
                while (reader.Read())
                {
                    country.Nodes.Add(data[0].ToString());
                }
                reader.Close();
                //for each region, add a province/state
                foreach (TreeNode region in country.Nodes)
                {
                    reader = theDB.runQuery("SELECT distinct province from " + tableName + " where region = '" + region.Text + "' and country = '" + country.Text + "';");
                    data = reader;
                    while (reader.Read())
                    {
                        region.Nodes.Add(data[0].ToString());
                    }
                    reader.Close();

                    //for each province, get a city name
                    foreach (TreeNode province in region.Nodes)
                    {
                        reader = theDB.runQuery("SELECT distinct city from " + tableName + " where province = '" + province.Text + "';");
                        data = reader;
                        while (reader.Read()) 
                        {
                            province.Nodes.Add(data[0].ToString());
                        }
                        reader.Close();

                        //for each City, get store names
                        foreach (TreeNode city in province.Nodes)
                        {
                            reader = theDB.runQuery("SELECT distinct storename from " + tableName + " where city = '" + city.Text + "';");
                            data = reader;
                            while (reader.Read())
                            {
                                city.Nodes.Add(data[0].ToString());
                            }
                            reader.Close();
                        }
                    }
                }
 

            }

            return all;
        } 
        public Form1()
        {
            InitializeComponent();

            try
            {
                theDB = new DatabaseShiz("COMPSCI-PC", "CMPT491-Warehouse", "compsci");
                MessageBox.Show("Connected to database.");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                Application.Exit();
            }

            
            locationTree.Nodes.Add(makeLocationTree());
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
