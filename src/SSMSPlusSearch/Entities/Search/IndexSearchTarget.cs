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
    public class IndexSearchTarget : SearchTargetBase
    {
        public IndexSearchTarget(DbIndex dbIndex)
        {
            DbIndex = dbIndex;
        }

        public DbIndex DbIndex { get; }

        public override string UniqueIdentifier => Guid.NewGuid().ToString();

        public override string Type => DbIndex.Type == "CLUSTERED" ? DbObjectType.INDEX_CLUSTERED.Name : DbObjectType.INDEX_NONCLUSTERED.Name;

        public override string SchemaName => DbIndex.Parent.SchemaName;

        public override string Name => DbIndex.Name;

        public override DateTime? ModificationDate => null;

        public override  string ModificationDateStr => string.Empty;

        public override TextFragments RichName => new TextFragments(TextFragment.Primary(DbIndex.Name));

        public override TextFragments RichSmallDefinition
        {
            get
            {

                return FormatIndex(" ");
            }
        }

        public override TextFragments RichFullDefinition
        {
            get
            {
                return FormatIndex("\n");
            }
        }

        private TextFragments FormatIndex(string lineBreak)
        {
            var frags = new TextFragments();
            if (DbIndex.IsUnique)
                frags.AddPrimary("UNIQUE ");

            frags.AddPrimary(DbIndex.Type + " INDEX ");
            frags.AddPrimary("[" + DbIndex.Name + "]");
            frags.AddSecondary(" ON ");
            frags.AddPrimary("[" + DbIndex.Parent.SchemaName + "].[" + DbIndex.Parent.Name + "]");
            frags.Add(FormatColumns(lineBreak));
            return frags;

        }

        private TextFragments FormatColumns(string lb)
        {
            var frags = new TextFragments();
            frags.AddSecondary("(" + lb);
            var isFirst = true;
            var mainCols = DbIndex.Columns.Where(p => !p.Included).OrderBy(p => p.IndexColumnId);
            foreach (var column in mainCols)
            {
                if (!isFirst)
                {
                    frags.AddSecondary("," + lb);
                }
                isFirst = false;

                frags.AddSecondary("\t");
                frags.AddPrimary("[" + column.ColumnName + "]");
                frags.AddSecondary(column.IsDesc ? " DESC" : " ASC");
            }
            frags.AddSecondary(lb + ")" + lb);


            var includedCols = DbIndex.Columns.Where(p => p.Included).OrderBy(p => p.IndexColumnId);
            if (includedCols.Count() > 0)
            {
                frags.AddSecondary("INCLUDE (" + lb);
                isFirst = true;
                foreach (var column in includedCols)
                {

                    if (!isFirst)
                    {
                        frags.AddSecondary("," + lb);
                    }
                    isFirst = false;

                    frags.AddSecondary("\t");
                    frags.AddPrimary("[" + column.ColumnName + "]");
                }
                frags.AddSecondary(lb + ")" + lb);
            }
            return frags;
        }

        public override IReadOnlyCollection<string> DbRealtivePath()
        {
            var parent = DbIndex.Parent;
            return new List<string>() { "UserTables", $"{parent.SchemaName}.{parent.Name}", "Indexes", Name };
        }
    }
}
