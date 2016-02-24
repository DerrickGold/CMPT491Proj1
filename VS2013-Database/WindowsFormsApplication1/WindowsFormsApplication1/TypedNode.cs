using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    class TypedNode
    {
        public enum TYPES {ALL_COUNTRY, COUNTRY, REGION, PROVINCE, CITY, STORE, ALL_DEPARTMENT, DEPARTMENT,
            CATAGORY, ITEM, ALL_TIME, YEAR, QUARTER, MONTH, DAY};
        private TYPES type;
        private String name;
        private TreeNode parent;
        private TreeNode itself;

        public TypedNode(String name, TYPES type, TreeNode parent)
        {
            this.name = name;
            this.type = type;
            this.parent = parent;
        }

        public void setRefNode(TreeNode reference) {
            this.itself = reference;
        }

        public TreeNode getRefNode()
        {
            return this.itself;
        }

        public String getName()
        {
            return name;
        }

        public TYPES getType()
        {
            return type;
        }

        public TreeNode getImmParent()
        {
            return parent;
        }

        public List<TypedNode> getParents()
        {
            List<TypedNode> list = new List<TypedNode>();
            //first add this node
            list.Add(this);

            //then go for parents
            TreeNode current = this.getImmParent();
            while (current != null)
            {
                TypedNode n = (TypedNode)current.Tag;
                list.Add(n);
                current = n.getImmParent();
            }

            return list;
        }

        public override String ToString()
        {
            return this.getName();
        }
    }
}
