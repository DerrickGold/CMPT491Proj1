using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class TypedNode
    {
        public enum TYPES {ALL_COUNTRY, COUNTRY, REGION, PROVINCE, CITY, STORE, DEPARTMENT, CATAGORY, ITEM};
        private TYPES type;
        private String name;

        public TypedNode(String name, TYPES type)
        {
            this.name = name;
            this.type = type;
        }

        public String getName()
        {
            return name;
        }

        public TYPES getType()
        {
            return type;
        }

        public String ToString()
        {
            return this.getName();
        }
    }
}
