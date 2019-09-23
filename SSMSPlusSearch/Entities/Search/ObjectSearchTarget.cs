using SSMSPlusCore.Ui.Search;
using SSMSPlusCore.Utils;
using SSMSPlusSearch.Entities.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMSPlusSearch.Entities
{
    public class ObjectSearchTarget : SearchTargetBase
    {
        public ObjectSearchTarget(DbObject dbObject)
        {
            DbObject = dbObject;
        }

        public DbObject DbObject { get; }

        public override string UniqueIdentifier => Guid.NewGuid().ToString();

        public override string Type => DbObject.Type;

        public override string SchemaName => DbObject.SchemaName;

        public override string Name => DbObject.Name;

        public override DateTime? ModificationDate => DbObject.ModificationDate;

        public override string ModificationDateStr => DbObject.ModificationDate.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");

        public override TextFragments RichName => new TextFragments(TextFragment.Primary(DbObject.Name));

        public override TextFragments RichSmallDefinition
        {
            get
            {
                var smallDef = DbObject.Definition?.Trim().Truncate(300).RemoveLineReturns();
                return new TextFragments(TextFragment.Primary(smallDef));
            }
        }

        public override TextFragments RichFullDefinition
        {
            get
            {
                return new TextFragments(TextFragment.Primary(DbObject.Definition));
            }
        }

        public override IReadOnlyCollection<string> DbRealtivePath()
        {
            if (SqlObjectType == DbObjectType.SQL_SCALAR_FUNCTION)
            {
                return FuncPath("Scalar-valuedFunctions");
            }

            if (SqlObjectType == DbObjectType.SQL_TABLE_VALUED_FUNCTION)
            {
                return FuncPath("Table-valuedFunctions");
            }

            if (SqlObjectType == DbObjectType.AGGREGATE_FUNCTION)
            {
                return FuncPath("AggregateFunctions");
            }

            if (SqlObjectType == DbObjectType.SQL_STORED_PROCEDURE)
            {
                return ProcPath();
            }

            if (SqlObjectType == DbObjectType.SQL_TRIGGER)
            {
                return TriggerPath();
            }

            if (SqlObjectType == DbObjectType.VIEW)
            {
                return ViewPath();
            }

            return base.DbRealtivePath(); ;
        }

        public IReadOnlyCollection<string> FuncPath(string folder)
        {
            return new List<string>() { "UserProgrammability", "UsrDbFunctions", folder, $"{SchemaName}.{Name}" };
        }

        public IReadOnlyCollection<string> ProcPath()
        {
            return new List<string>() { "UserProgrammability", "StoredProcedures", $"{SchemaName}.{Name}" };
        }

        public IReadOnlyCollection<string> TriggerPath()
        {
            var parent = DbObject.Parent;
            return new List<string>() { "UserTables", $"{parent.SchemaName}.{parent.Name}", "Triggers", Name };
        }

        public IReadOnlyCollection<string> ViewPath()
        {
            var parent = DbObject.Parent;
            return new List<string>() { "Views", $"{SchemaName}.{Name}" };
        }
    }
}
