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

            

        }

        private void logQuery(String results) {
            var dateInfo = getSelectedDateTree();
            var itemInfo = getSelectedItemTree();
            var locInfo = getSelectedLocationTree();


            String output = "\n---" + 
                results + "\n|" + timeInfoString(dateInfo) +
                "\n|" + itemInfoString(itemInfo) +
                "\n---";
            richTextBox1.Text += output;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //run filterd query with this select statement
            runFilteredQuery("SELECT COUNT(sale_id), SUM(price)", (IDataRecord data) =>
            {
                //for each row of the result (only one for this query),
                //populate some text boxes
                String unitsSold = data[0].ToString();
                String dollars = data[1].ToString();
                logQuery("Dollars In: " + dollars + " Units Out: " + unitsSold);
            });

        }


        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


        private void runFilteredQuery(String selectStmt, DatabaseShiz.ForEachResult onResult)
        {
            //get the selected item details
            var dateInfo = getSelectedDateTree();
            var itemInfo = getSelectedItemTree();
            var locInfo = getSelectedLocationTree();


            //Get total sales
            String queryStr = selectStmt + " from " + tableName + " where ";
            //set time filter
            queryStr += generateTimeFilter(dateInfo);
            queryStr += generateItemFilter(itemInfo);
            queryStr += generateLocationFilter(locInfo);
            queryStr = finalizeQuery(queryStr);

            Console.WriteLine(queryStr);
            theDB.runQuery(queryStr);
            theDB.forEachResult(onResult);
        }

        private String removeTrailingDelim(String delim, String input)
        {
            String output = input;
            if (output.LastIndexOf(delim) == output.Length - delim.Length)
                output = output.Substring(0, output.Length - delim.Length);

            return output;
        }

        public String finalizeQuery(String query)
        {
            String newQuery = query;
            //remove trailing 'and'
            newQuery = removeTrailingDelim(" and ", newQuery);
            //remove trailling 'where'
            newQuery = removeTrailingDelim(" where ", newQuery);
            //remove trailing 'or'
            newQuery = removeTrailingDelim(" or ", newQuery);
            newQuery += ";";
            return newQuery;
        }

        public String generateTimeFilter(Tuple<int, String, String[], int> info)
        {
            int year=info.Item1; 
            String month = info.Item2;
            String[] monthList = info.Item3;
            int day = info.Item4;

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
                for (int i = 1; i < monthList.Length - 1; i++)
                {
                    filterStr += "month = '" + monthList[i] + "' or ";
                }
                filterStr += "month = '" + monthList[monthList.Length - 1] + "') and ";
            }
            if (day > 0)
                filterStr += "day = " + day + " and ";
            

            return filterStr;
        }

        public String generateItemFilter(Tuple<String, String, String> info)
        {
            String department = info.Item1;
            String catagory = info.Item2;
            String item = info.Item3;

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

        public String generateLocationFilter(Tuple<String, String, String, String, String> info)
        {
            String country = info.Item1; 
            String region = info.Item2; 
            String province = info.Item3;
            String city = info.Item4;
            String store = info.Item5;

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
                filterLoc += "storename = '" + store + "' and ";

            return filterLoc;
        }


        private Tuple<int, String, String[], int> getSelectedDateTree()
        {
            //tuple data
            int year = 0;
            String month = null;
            String[] monthList = null;
            int day = 0;

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
                        monthList = new String[4];
                        for (int i = 1; i < 4; i++)
                            monthList[i] = reference.Nodes[i - 1].Text;
                        //store quarter name in pos 0
                        monthList[0] = t.getName();
                        break;
                    case TypedNode.TYPES.DAY:
                        day = Int32.Parse(t.getName());
                        break;
                }
            }
            return new Tuple<int, String, String[], int>(year, month, monthList, day);
        }

        public String timeInfoString(Tuple<int, String, String[], int> info)
        {
            int year = info.Item1;
            if (year == 0)
                return "All Time";

            String output = "(" + year + "/";

            String month = info.Item2;
            String[] monthList = info.Item3;

            if (month != null)
                output += month + "/";
            else if (monthList != null)
                output += monthList[0] + "/";

            int day = info.Item4;
            if (day > 0)
                output += day;

            output += ")";
            return output;
        }

        private Tuple<String, String, String> getSelectedItemTree()
        {
            //tuple data
            String department = null, catagory = null, item = null;

            TypedNode type = (TypedNode)productTree.SelectedNode.Tag;
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

            return new Tuple<String, String, String>(department, catagory, item);
        }

        public String itemInfoString( Tuple<String, String, String> info)
        {
            String output = "";

            String item = info.Item3;
            if (item != null)
                output += "Item: " + item;

            String dept = info.Item1;
            if (dept != null)
                output += " of " + dept + " department ";

            String catagory = info.Item2;
            if (catagory != null)
                output += " for " + catagory + " catagory ";


            return output;
        }

        private Tuple<String, String, String, String, String> getSelectedLocationTree()
        {
            String country = null, region = null, province = null, city = null, store = null;

            TypedNode type = (TypedNode)locationTree.SelectedNode.Tag;
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

            return new Tuple<String, String, String, String, String>(country, region, province, city, store);
        }

        private void dateTree_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}
