namespace SSMSPlusSearch.Entities
{
    using SSMSPlusCore.Ui.Search;
    using SSMSPlusSearch.Entities.Search;
    using SSMSPlusSearch.Helpers;
    using System;
    using System.Collections.Generic;

    public class ColumnSearchTarget : SearchTargetBase
    {
        public ColumnSearchTarget(DbColumn dbColumn)
        {
            DbColumn = dbColumn;
        }

        public DbColumn DbColumn { get; }

        public override string UniqueIdentifier => Guid.NewGuid().ToString();

        public override string Type => DbObjectType.USER_TABLE_COLUMN.Name;

        public override string SchemaName => DbColumn.Parent.SchemaName;

        public override string Name => DbColumn.Name;


        public override DateTime? ModificationDate => null;

        public override string ModificationDateStr => string.Empty;

        public override TextFragments RichName
        {
            get
            {
                var frags = new TextFragments();
                frags.AddPrimary(DbColumn.Name);
                return frags;
            }
        }

        public override TextFragments RichSmallDefinition
        {
            get
            {
                var frags = new TextFragments();
                frags.AddSecondary(DbColumn.Parent.Name);
                frags.AddSecondary(" | ");
                frags.AddPrimary(DbColumn.Name);
                frags.AddPrimary(" " + Formatting.FormatDatatype(DbColumn.Datatype, DbColumn.Precision));
                if (!string.IsNullOrEmpty(DbColumn.Definition))
                {
                    frags.AddSecondary(" | ");
                    frags.AddPrimary(DbColumn.Definition);
                }
                frags.AddPrimary(DbColumn.Definition);
                return frags;
            }
        }

        public override TextFragments RichFullDefinition
        {
            get
            {
                return RichSmallDefinition;
            }
        }

        public override IReadOnlyCollection<string> DbRealtivePath()
        {
            var parent = DbColumn.Parent;
            return new List<string>() { "UserTables", $"{parent.SchemaName}.{parent.Name}", "Columns", Name };
        }
    }
}
