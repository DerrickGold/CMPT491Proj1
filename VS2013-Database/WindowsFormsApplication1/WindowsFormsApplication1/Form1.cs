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

        TreeNode makeTypedNode(String name, TypedNode.TYPES type)
        {
            TreeNode all = new TreeNode(name);
            all.Tag = new TypedNode(name, type);
            return all;
        }

        TreeNode makeLocationTree()
        {
            //generate location tree
            // All Countries
            //  Countries
            //      region
            //          province
            //              city
            //              store name

            //start with all countrie            
            TreeNode all = makeTypedNode("All Location", TypedNode.TYPES.ALL_COUNTRY);
            //get all countries
            SqlDataReader reader = theDB.runQuery("SELECT distinct country from " + tableName);
            IDataRecord data = reader;
            while (reader.Read())
            {
                all.Nodes.Add(makeTypedNode(data[0].ToString(), TypedNode.TYPES.COUNTRY));
            }
            reader.Close();
            //forception
            foreach (TreeNode country in all.Nodes) {
                //get all regions for a country
                reader = theDB.runQuery("SELECT distinct region from " + tableName + " where country = '" + country.Text + "';");
                data = reader;
                while (reader.Read())
                {
                    country.Nodes.Add(makeTypedNode(data[0].ToString(), TypedNode.TYPES.REGION));
                }
                reader.Close();
                //for each region, add a province/state
                foreach (TreeNode region in country.Nodes)
                {
                    reader = theDB.runQuery("SELECT distinct province from " + tableName + " where region = '" + region.Text + "' and country = '" + country.Text + "';");
                    data = reader;
                    while (reader.Read())
                    {
                        region.Nodes.Add(makeTypedNode(data[0].ToString(), TypedNode.TYPES.PROVINCE));
                    }
                    reader.Close();

                    //for each province, get a city name
                    foreach (TreeNode province in region.Nodes)
                    {
                        reader = theDB.runQuery("SELECT distinct city from " + tableName + " where province = '" + province.Text + "';");
                        data = reader;
                        while (reader.Read()) 
                        {
                            province.Nodes.Add(makeTypedNode(data[0].ToString(), TypedNode.TYPES.CITY));
                        }
                        reader.Close();

                        //for each City, get store names
                        foreach (TreeNode city in province.Nodes)
                        {
                            reader = theDB.runQuery("SELECT distinct storename from " + tableName + " where city = '" + city.Text + "';");
                            data = reader;
                            while (reader.Read())
                            {
                                city.Nodes.Add(makeTypedNode(data[0].ToString(), TypedNode.TYPES.STORE));
                            }
                            reader.Close();
                        }
                    }
                }
 

            }

            return all;
        }


        TreeNode makeItemTree()
        {
            // All Products
            //  Department
            //      Category
            //          item
            TreeNode all = makeTypedNode("All Products", TypedNode.TYPES.DEPARTMENT);

            //add departments
            SqlDataReader reader = theDB.runQuery("SELECT distinct department from " + tableName + ";");
            IDataRecord data = reader;
            while (reader.Read())
            {
                all.Nodes.Add(makeTypedNode(data[0].ToString(), TypedNode.TYPES.DEPARTMENT));
            }
            reader.Close();

            //for each department, add a catagory
            foreach (TreeNode department in all.Nodes) {
                reader = theDB.runQuery("SELECT distinct catagory from " + tableName + " where department = '" + department.Text + "';");
                data = reader;
                while (reader.Read())
                {
                    department.Nodes.Add(makeTypedNode(data[0].ToString(), TypedNode.TYPES.CATAGORY));
                }
                reader.Close();
                //for each catagory, add items
                foreach (TreeNode catagory in department.Nodes) {
                    reader = theDB.runQuery("SELECT distinct item from " + tableName + " where catagory = '" + catagory.Text + "';");
                    data = reader;
                    while (reader.Read()) {
                        catagory.Nodes.Add(makeTypedNode(data[0].ToString(), TypedNode.TYPES.ITEM));
                    }
                    reader.Close();

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
            productTree.Nodes.Add(makeItemTree());
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
