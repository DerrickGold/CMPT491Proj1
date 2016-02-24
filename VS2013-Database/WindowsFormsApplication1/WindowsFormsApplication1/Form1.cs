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

        public TreeNode makeDateTree()
        {
            TreeNode all = makeTypedNode("All Time", TypedNode.TYPES.ALL_TIME);

            //add years
            SqlDataReader reader = theDB.runQuery("SELECT distinct year from " + tableName + " ORDER BY year;");
            IDataRecord data = reader;
            while (reader.Read())
            {
                all.Nodes.Add(makeTypedNode(data[0].ToString(), TypedNode.TYPES.YEAR));
            }
            reader.Close();

            //for each year, generate a quarter, and month
            foreach (TreeNode year in all.Nodes)
            {
                TreeNode quarter1 = makeTypedNode("Quarter 1", TypedNode.TYPES.QUARTER);
                quarter1.Nodes.Add(makeTypedNode("Jan", TypedNode.TYPES.MONTH));
                quarter1.Nodes.Add(makeTypedNode("Feb", TypedNode.TYPES.MONTH));
                quarter1.Nodes.Add(makeTypedNode("Mar", TypedNode.TYPES.MONTH));

                TreeNode quarter2 = makeTypedNode("Quarter 2", TypedNode.TYPES.QUARTER);
                quarter2.Nodes.Add(makeTypedNode("Apr", TypedNode.TYPES.MONTH));
                quarter2.Nodes.Add(makeTypedNode("May", TypedNode.TYPES.MONTH));
                quarter2.Nodes.Add(makeTypedNode("Jun", TypedNode.TYPES.MONTH));

                TreeNode quarter3 = makeTypedNode("Quarter 3", TypedNode.TYPES.QUARTER);
                quarter3.Nodes.Add(makeTypedNode("Jul", TypedNode.TYPES.MONTH));
                quarter3.Nodes.Add(makeTypedNode("Aug", TypedNode.TYPES.MONTH));
                quarter3.Nodes.Add(makeTypedNode("Sep", TypedNode.TYPES.MONTH));

                TreeNode quarter4 = makeTypedNode("Quarter 4", TypedNode.TYPES.QUARTER);
                quarter4.Nodes.Add(makeTypedNode("Oct", TypedNode.TYPES.MONTH));
                quarter4.Nodes.Add(makeTypedNode("Nov", TypedNode.TYPES.MONTH));
                quarter4.Nodes.Add(makeTypedNode("Dec", TypedNode.TYPES.MONTH));

                year.Nodes.Add(quarter1);
                year.Nodes.Add(quarter2);
                year.Nodes.Add(quarter3);
                year.Nodes.Add(quarter4);

                foreach (TreeNode quart in year.Nodes)
                {
                    foreach (TreeNode month in quart.Nodes)
                    {
                        reader = theDB.runQuery("SELECT distinct day from " + tableName + " where year='" + year.Text + "' and month='" + month.Text + "';");
                        data = reader;
                        while (reader.Read())
                        {
                            month.Nodes.Add(makeTypedNode(data[0].ToString(), TypedNode.TYPES.DAY));
                        }
                        reader.Close();
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
                theDB = new DatabaseShiz("CURTIS_PC\\SQLEXPRESS", "CMPT491-Warehouse", "Curtis");
                MessageBox.Show("Connected to database.");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                Application.Exit();
            }

            TreeNode firstLoc = makeLocationTree();
            TreeNode firstDate = makeDateTree();
            TreeNode firstItem = makeItemTree();
            locationTree.Nodes.Add(firstLoc);
            dateTree.Nodes.Add(firstDate);
            productTree.Nodes.Add(firstItem);
            locationTree.SelectedNode = firstLoc;
            dateTree.SelectedNode = firstDate;
            productTree.SelectedNode = firstItem;
            //TreeNode test = locationTree.SelectedNode;
            //Console.Write(test.Text);
            //productTree.Nodes.Add(makeItemTree());
            //dateTree.Nodes.Add(makeDateTree());
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

        private void button1_Click(object sender, EventArgs e)
        {
            //Assigning Date
            int year = 0;
            string month;
            TreeNode node = dateTree.SelectedNode;
            if (node.Text.Length == 3){
                month = node.Text;
                year = Int32.Parse(node.Parent.Parent.Text);
            }
            if (node.Text.Length == 4)
            {
                year = Int32.Parse(node.Text);
            }
            if (node.Text.Length == 9){
                if (node.Text == "Quarter 1")
                {
                    month = "Jan Feb Mar";
                }
                if (node.Text == "Quarter 2")
                {
                    month = "Apr May Jun";
                }
                if (node.Text == "Quarter 3")
                {
                    month = "Jul Aug Sep";
                }
                if (node.Text == "Quarter 4")
                {
                    month = "Oct Nov Dec";
                }
                year = Int32.Parse(node.Parent.Text);
            }
            
            //assigning Items
            string department = "no";
            string cat = "no";
            string item = "no";
            TreeNode node1 = productTree.SelectedNode;
            if (node1.Level == 1)
            {
                node1.Text = department;
            }
            if (node1.Level == 2)
            {
                node1.Text = cat;
                node1.Parent.Text = department;
            } 
            if (node1.Level == 3)
            {
                node1.Text = item;
                node1.Parent.Text = cat;
                node1.Parent.Parent.Text = department;
            }
            string country = "no";
            string region = "no";
            string prov = "no";
            string city = "no";
            string store = "no";
            TreeNode node2 = productTree.SelectedNode;
            if (node2.Level == 1)
            {
                country = node2.Text;
            }
            if (node2.Level == 2)
            {
                region = node2.Text;
                country = node2.Parent.Text;
            }
            if (node2.Level == 3)
            {
                prov = node2.Text;
                region = node2.Parent.Text;
                country = node2.Parent.Parent.Text;
            }
            if (node2.Level == 4)
            {
                city = node2.Text;
                prov = node2.Parent.Text;
                region = node2.Parent.Parent.Text;
                country = node2.Parent.Parent.Parent.Text;
            }
            if (node2.Level == 5)
            {
                store = node2.Text;
                city = node2.Parent.Text;
                prov = node2.Parent.Parent.Text;
                region = node2.Parent.Parent.Parent.Text;
                country = node2.Parent.Parent.Parent.Parent.Text;
            }

        }

        private void dateTree_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

    }
}
