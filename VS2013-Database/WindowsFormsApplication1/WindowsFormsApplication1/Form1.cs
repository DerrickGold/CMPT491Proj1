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

        TreeNode makeTypedNode(String name, TypedNode.TYPES type, TreeNode parent)
        {
            TreeNode all = new TreeNode(name);
            TypedNode nodeType = new TypedNode(name, type, parent);
            nodeType.setRefNode(all);
            all.Tag = nodeType;
            
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
            TreeNode all = makeTypedNode("All Location", TypedNode.TYPES.ALL_COUNTRY, null);
            //get all countries
            theDB.runQuery("SELECT distinct country from " + tableName);
            theDB.forEachResult((IDataRecord data) =>
            {
                all.Nodes.Add(makeTypedNode(data[0].ToString(), TypedNode.TYPES.COUNTRY, all));
            });
            //forception
            foreach (TreeNode country in all.Nodes) {
                //get all regions for a country
                theDB.runQuery("SELECT distinct region from " + tableName + " where country = '" + country.Text + "';");
                theDB.forEachResult((IDataRecord data) =>
                {
                    country.Nodes.Add(makeTypedNode(data[0].ToString(), TypedNode.TYPES.REGION, country));
                });
                //for each region, add a province/state
                foreach (TreeNode region in country.Nodes)
                {
                    theDB.runQuery("SELECT distinct province from " + tableName + " where region = '" + region.Text + "' and country = '" + country.Text + "';");
                    theDB.forEachResult((IDataRecord data) =>
                    {
                        region.Nodes.Add(makeTypedNode(data[0].ToString(), TypedNode.TYPES.PROVINCE, region));
                    });
                    //for each province, get a city name
                    foreach (TreeNode province in region.Nodes)
                    {
                        theDB.runQuery("SELECT distinct city from " + tableName + " where province = '" + province.Text + "';");
                        theDB.forEachResult((IDataRecord data) =>
                        {
                            province.Nodes.Add(makeTypedNode(data[0].ToString(), TypedNode.TYPES.CITY, province));
                        });

                        //for each City, get store names
                        foreach (TreeNode city in province.Nodes)
                        {
                            theDB.runQuery("SELECT distinct storename from " + tableName + " where city = '" + city.Text + "';");
                            theDB.forEachResult((IDataRecord data) =>
                            {
                                city.Nodes.Add(makeTypedNode(data[0].ToString(), TypedNode.TYPES.STORE, city));
                            });
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
            TreeNode all = makeTypedNode("All Products", TypedNode.TYPES.ALL_DEPARTMENT, null);

            //add departments
            theDB.runQuery("SELECT distinct department from " + tableName + ";");
            theDB.forEachResult((IDataRecord data) =>
            {
                all.Nodes.Add(makeTypedNode(data[0].ToString(), TypedNode.TYPES.DEPARTMENT, all));
            });

            //for each department, add a catagory
            foreach (TreeNode department in all.Nodes) {
                theDB.runQuery("SELECT distinct catagory from " + tableName + " where department = '" + department.Text + "';");
                theDB.forEachResult((IDataRecord data) =>
                {
                    department.Nodes.Add(makeTypedNode(data[0].ToString(), TypedNode.TYPES.CATAGORY, department));
                });
                //for each catagory, add items
                foreach (TreeNode catagory in department.Nodes) {
                    theDB.runQuery("SELECT distinct item from " + tableName + " where catagory = '" + catagory.Text + "';");
                    theDB.forEachResult((IDataRecord data) =>
                    {
                        catagory.Nodes.Add(makeTypedNode(data[0].ToString(), TypedNode.TYPES.ITEM, catagory));
                    });
                }
            }


            return all;
        }

        public TreeNode makeDateTree()
        {
            TreeNode all = makeTypedNode("All Time", TypedNode.TYPES.ALL_TIME, null);

            //add years
            theDB.runQuery("SELECT distinct year from " + tableName + " ORDER BY year;");
            theDB.forEachResult((IDataRecord data) =>
            {
                all.Nodes.Add(makeTypedNode(data[0].ToString(), TypedNode.TYPES.YEAR, all));
            });

            //for each year, generate a quarter, and month
            foreach (TreeNode year in all.Nodes)
            {
                TreeNode quarter1 = makeTypedNode("Quarter 1", TypedNode.TYPES.QUARTER, year);
                quarter1.Nodes.Add(makeTypedNode("Jan", TypedNode.TYPES.MONTH, quarter1));
                quarter1.Nodes.Add(makeTypedNode("Feb", TypedNode.TYPES.MONTH, quarter1));
                quarter1.Nodes.Add(makeTypedNode("Mar", TypedNode.TYPES.MONTH, quarter1));

                TreeNode quarter2 = makeTypedNode("Quarter 2", TypedNode.TYPES.QUARTER, year);
                quarter2.Nodes.Add(makeTypedNode("Apr", TypedNode.TYPES.MONTH, quarter2));
                quarter2.Nodes.Add(makeTypedNode("May", TypedNode.TYPES.MONTH, quarter2));
                quarter2.Nodes.Add(makeTypedNode("Jun", TypedNode.TYPES.MONTH, quarter2));

                TreeNode quarter3 = makeTypedNode("Quarter 3", TypedNode.TYPES.QUARTER, year);
                quarter3.Nodes.Add(makeTypedNode("Jul", TypedNode.TYPES.MONTH, quarter3));
                quarter3.Nodes.Add(makeTypedNode("Aug", TypedNode.TYPES.MONTH, quarter3));
                quarter3.Nodes.Add(makeTypedNode("Sep", TypedNode.TYPES.MONTH, quarter3));

                TreeNode quarter4 = makeTypedNode("Quarter 4", TypedNode.TYPES.QUARTER, year);
                quarter4.Nodes.Add(makeTypedNode("Oct", TypedNode.TYPES.MONTH, quarter4));
                quarter4.Nodes.Add(makeTypedNode("Nov", TypedNode.TYPES.MONTH, quarter4));
                quarter4.Nodes.Add(makeTypedNode("Dec", TypedNode.TYPES.MONTH, quarter4));

                year.Nodes.Add(quarter1);
                year.Nodes.Add(quarter2);
                year.Nodes.Add(quarter3);
                year.Nodes.Add(quarter4);

                foreach (TreeNode quart in year.Nodes)
                {
                    foreach (TreeNode month in quart.Nodes)
                    {
                        theDB.runQuery("SELECT distinct day from " + tableName + " where year='" + year.Text + "' and month='" + month.Text + "';");
                        theDB.forEachResult((IDataRecord data) => {
                            month.Nodes.Add(makeTypedNode(data[0].ToString(), TypedNode.TYPES.DAY, month));
                        });
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
                //theDB = new DatabaseShiz("CURTIS_PC\\SQLEXPRESS", "CMPT491-Warehouse", "Curtis");
                theDB = new DatabaseShiz("COMPSCI-PC", "CMPT491-Warehouse", "compsci");
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
            //query parameters
            int year = 0, day = 0;
            String month = null;
            String[] monthList = null;
            String department = null, catagory = null, item = null;
            String country = null, region = null, province = null, city = null, store = null;

            //Assigning Date
            //Trace the parents of each selected node to get contextual details
            TreeNode node = dateTree.SelectedNode;
            TypedNode type = (TypedNode)dateTree.SelectedNode.Tag;
            List<TypedNode> dateParents = type.getParents();
            foreach (TypedNode t in dateParents)
            {
                switch (t.getType())
                {
                    case TypedNode.TYPES.YEAR:
                        year = Int32.Parse(t.getName());
                        break;
                    case TypedNode.TYPES.MONTH:
                        month = t.getName();
                        break;
                    case TypedNode.TYPES.QUARTER:
                        //only add quarter months if month is not set
                        if (month != null) break;

                        TreeNode reference = t.getRefNode();
                        //hree months in a quarter, pretty static
                        monthList = new String[3];
                        for (int i = 0; i < 3; i++)
                            monthList[i] = reference.Nodes[i].Text;
                        break;
                    case TypedNode.TYPES.DAY:
                        day = Int32.Parse(t.getName());
                        break;
                }
            }

            //get the selected item details
            
            type = (TypedNode)productTree.SelectedNode.Tag;
            List<TypedNode> itemParents = type.getParents();
            foreach (TypedNode t in itemParents)
            {
                switch (t.getType())
                {
                    case TypedNode.TYPES.DEPARTMENT:
                        department = t.getName();
                        break;
                    case TypedNode.TYPES.CATAGORY:
                        catagory = t.getName();
                        break;
                    case TypedNode.TYPES.ITEM:
                        item = t.getName();
                        break;
                }
            }

            //get location
            type = (TypedNode)locationTree.SelectedNode.Tag;
            List<TypedNode> locationParents = type.getParents();
            foreach (TypedNode t in locationParents)
            {
                switch (t.getType())
                {
                    case TypedNode.TYPES.COUNTRY:
                        country = t.getName();
                        break;
                    case TypedNode.TYPES.REGION:
                        region = t.getName();
                        break;
                    case TypedNode.TYPES.PROVINCE:
                        province = t.getName();
                        break;
                    case TypedNode.TYPES.CITY:
                        city = t.getName();
                        break;
                    case TypedNode.TYPES.STORE:
                        store = t.getName();
                        break;
                }
            }

            String queryStr = "SELECT * from " + tableName + " where ";

            //set time filter
            queryStr += generateTimeFilter(year, month, monthList, day);
            queryStr += generateItemFilter(department, catagory, item);
            queryStr += generateLocationFilter(country, region, province, city, store);
            queryStr = finalizeQuery(queryStr);

            Console.WriteLine(queryStr);
           
        }

        public String finalizeQuery(String query)
        {
            String newQuery = query;
            String delim = " and ";
            //remove trailing 'and'
            if (newQuery.LastIndexOf(delim) == newQuery.Length - delim.Length)
                newQuery = newQuery.Substring(0, newQuery.Length - delim.Length);
                
            newQuery += ";";
            return newQuery;
        }

        public String generateTimeFilter(int year, String month, String[] monthList, int day)
        {
            String filterStr = "";
            //set date filters
            if (year > 0)
                filterStr += "year = " + year + " and ";
            //month won't be set if no year exists
            if (month != null)
                filterStr += "month = '" + month + "' and ";
            else if (monthList != null)
            {
                filterStr += "(";
                for (int i = 0; i < monthList.Length - 1; i++)
                {
                    filterStr += "month = '" + monthList[i] + "' or ";
                }
                filterStr += "month = '" + monthList[monthList.Length - 1] + "') and ";
            }

            return filterStr;
        }

        public String generateItemFilter(String department, String catagory, String item)
        {
            String filterItem = "";
            //set item filters
            if (department != null)
                filterItem += "department = '" + department + "' and ";
            if (catagory != null)
                filterItem += "catagory = '" + catagory + "' and ";
            if (item != null)
                filterItem += "item = '" + item + "' and ";

            return filterItem;
        }

        public String generateLocationFilter(String country, String region, String province, String city, String store)
        {
            String filterLoc = "";
            //set location filter
            if (country != null)
                filterLoc += "country = '" + country + "' and ";
            if (region != null)
                filterLoc += "region = '" + region + "' and ";
            if (province != null)
                filterLoc += "province = '" + province + "' and ";
            if (city != null)
                filterLoc += "city = '" + city + "' and ";
            if (store != null)
                filterLoc += "store = '" + store + "' and ";

            return filterLoc;
        }




        private void dateTree_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

    }
}
