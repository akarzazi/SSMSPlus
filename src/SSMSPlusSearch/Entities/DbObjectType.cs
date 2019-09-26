using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SSMSPlusSearch.Entities
{
    public class DbObjectType
    {
        public static DbObjectType USER_TABLE = new DbObjectType("USER_TABLE", DbSimplifiedType.Table);
        public static DbObjectType USER_TABLE_COLUMN = new DbObjectType("USER_TABLE_COLUMN", DbSimplifiedType.Column); // Not an sql type
        public static DbObjectType VIEW = new DbObjectType("VIEW", DbSimplifiedType.View);

        public static DbObjectType AGGREGATE_FUNCTION = new DbObjectType("AGGREGATE_FUNCTION", DbSimplifiedType.Function);
        public static DbObjectType CLR_SCALAR_FUNCTION = new DbObjectType("CLR_SCALAR_FUNCTION", DbSimplifiedType.Function);
        public static DbObjectType CLR_TABLE_VALUED_FUNCTION = new DbObjectType("CLR_TABLE_VALUED_FUNCTION", DbSimplifiedType.Function);
        public static DbObjectType SQL_INLINE_TABLE_VALUED_FUNCTION = new DbObjectType("SQL_INLINE_TABLE_VALUED_FUNCTION", DbSimplifiedType.Function);
        public static DbObjectType SQL_SCALAR_FUNCTION = new DbObjectType("SQL_SCALAR_FUNCTION", DbSimplifiedType.Function);
        public static DbObjectType SQL_TABLE_VALUED_FUNCTION = new DbObjectType("SQL_TABLE_VALUED_FUNCTION", DbSimplifiedType.Function);

        public static DbObjectType CLR_STORED_PROCEDURE = new DbObjectType("CLR_STORED_PROCEDURE", DbSimplifiedType.Procedure);
        public static DbObjectType EXTENDED_STORED_PROCEDURE = new DbObjectType("EXTENDED_STORED_PROCEDURE", DbSimplifiedType.Procedure);
        public static DbObjectType SQL_STORED_PROCEDURE = new DbObjectType("SQL_STORED_PROCEDURE", DbSimplifiedType.Procedure);

        public static DbObjectType CHECK_CONSTRAINT = new DbObjectType("CHECK_CONSTRAINT", DbSimplifiedType.Constraint);
        public static DbObjectType DEFAULT_CONSTRAINT = new DbObjectType("DEFAULT_CONSTRAINT", DbSimplifiedType.Constraint);
        public static DbObjectType FOREIGN_KEY_CONSTRAINT = new DbObjectType("FOREIGN_KEY_CONSTRAINT", DbSimplifiedType.Constraint);
        public static DbObjectType PRIMARY_KEY_CONSTRAINT = new DbObjectType("PRIMARY_KEY_CONSTRAINT", DbSimplifiedType.Constraint);
        public static DbObjectType UNIQUE_CONSTRAINT = new DbObjectType("UNIQUE_CONSTRAINT", DbSimplifiedType.Constraint);

        public static DbObjectType CLR_TRIGGER = new DbObjectType("CLR_TRIGGER", DbSimplifiedType.Trigger);
        public static DbObjectType SQL_TRIGGER = new DbObjectType("SQL_TRIGGER", DbSimplifiedType.Trigger);

        public static DbObjectType INDEX_CLUSTERED = new DbObjectType("INDEX CLUSTERED", DbSimplifiedType.Index); // Not an sql type
        public static DbObjectType INDEX_NONCLUSTERED = new DbObjectType("INDEX NONCLUSTERED", DbSimplifiedType.Index); // Not an sql type

        public static DbObjectType SYNONYM = new DbObjectType("SYNONYM", DbSimplifiedType.Other);
        public static DbObjectType PLAN_GUIDE = new DbObjectType("PLAN_GUIDE", DbSimplifiedType.Other);
        // any unlisted type will be qualified as "Other"

        public string Name { get; private set; }
        public DbSimplifiedType Category { get; private set; }

        private DbObjectType(string name, DbSimplifiedType category)
        {
            Name = name;
            Category = category;
        }

        public override string ToString()
        {
            return Name;
        }

        private static IReadOnlyDictionary<string, DbObjectType> _allTypescache;

        public static IReadOnlyDictionary<string, DbObjectType> GetAll()
        {
            if (_allTypescache == null)
            {
                var fields = typeof(DbObjectType).GetFields(BindingFlags.Public |
                                                 BindingFlags.Static |
                                                 BindingFlags.DeclaredOnly);

                _allTypescache = fields.Select(f => f.GetValue(null)).Cast<DbObjectType>().ToDictionary(p => p.Name);
            }
            return _allTypescache;
        }


        public static DbObjectType Parse(string sqlType)
        {
            sqlType = sqlType.ToUpperInvariant().Trim();
            GetAll().TryGetValue(sqlType, out DbObjectType match);
            return match ?? new DbObjectType(sqlType, DbSimplifiedType.Other);
        }

        //        AGGREGATE_FUNCTION 

        //CHECK_CONSTRAINT

        //CLR_SCALAR_FUNCTION --

        //CLR_STORED_PROCEDURE

        //CLR_TABLE_VALUED_FUNCTION --

        //CLR_TRIGGER

        //DEFAULT_CONSTRAINT

        //EXTENDED_STORED_PROCEDURE

        //FOREIGN_KEY_CONSTRAINT

        //INTERNAL_TABLE

        //PLAN_GUIDE

        //PRIMARY_KEY_CONSTRAINT

        //REPLICATION_FILTER_PROCEDURE

        //RULE

        //SEQUENCE_OBJECT

        //SERVICE_QUEUE

        //SQL_INLINE_TABLE_VALUED_FUNCTION

        //SQL_SCALAR_FUNCTION

        //SQL_STORED_PROCEDURE

        //SQL_TABLE_VALUED_FUNCTION

        //SQL_TRIGGER

        //SYNONYM

        //SYSTEM_TABLE

        //TABLE_TYPE

        //UNIQUE_CONSTRAINT

        //USER_TABLE

        //VIEW
    }
}
