using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SSMSPlusSearch.Entities
{
    public class DbSimplifiedType
    {
        public static DbSimplifiedType Table = new DbSimplifiedType("Table");
        public static DbSimplifiedType Column = new DbSimplifiedType("Column");
        public static DbSimplifiedType View = new DbSimplifiedType("View");
        public static DbSimplifiedType Procedure = new DbSimplifiedType("Procedure");
        public static DbSimplifiedType Function = new DbSimplifiedType("Function");
        public static DbSimplifiedType Trigger = new DbSimplifiedType("Trigger");
        public static DbSimplifiedType Constraint = new DbSimplifiedType("Constraint");
        public static DbSimplifiedType Index = new DbSimplifiedType("Index");
        public static DbSimplifiedType Other = new DbSimplifiedType("Other");

        public string Name { get; private set; }
        private DbSimplifiedType(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        public static IEnumerable<DbSimplifiedType> GetAll()
        {
            var fields = typeof(DbSimplifiedType).GetFields(BindingFlags.Public |
                                             BindingFlags.Static |
                                             BindingFlags.DeclaredOnly);

            return fields.Select(f => f.GetValue(null)).Cast<DbSimplifiedType>();
        }
    }
}
