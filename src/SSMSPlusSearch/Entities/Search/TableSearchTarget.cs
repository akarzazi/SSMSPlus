using SSMSPlusCore.Ui.Search;
using SSMSPlusCore.Utils;
using SSMSPlusSearch.Entities.Search;
using SSMSPlusSearch.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMSPlusSearch.Entities
{
    public class TableSearchTarget : SearchTargetBase
    {
        public TableSearchTarget(DbObject dbObject, DbColumn[] dbColumns)
        {
            DbObject = dbObject;
            DbColumns = dbColumns;
        }

        public DbColumn[] DbColumns { get; }

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
                var fragments = new TextFragments();
                fragments.AddPrimary("[" + SchemaName + "]." + "[" + Name + "]");
                fragments.AddSecondary("\n\n");

                var struc = CreateTableStruct();
                fragments.Add(CreateTable(struc, GetTableWidths(struc)));
                return fragments;
            }
        }

        private List<(TextFragment, TextFragment, TextFragment)> CreateTableStruct()
        {
            // Column | Datatype | Computed value
            var header = (TextFragment.Secondary("Column"), TextFragment.Secondary("Datatype"), TextFragment.Secondary("Computed value"));
            var list = new List<(TextFragment, TextFragment, TextFragment)>();
            list.Add(header);

            foreach (var dbColumn in DbColumns)
            {
                var col = TextFragment.Primary(dbColumn.Name);
                var type = TextFragment.Secondary(Formatting.FormatDatatype(dbColumn.Datatype, dbColumn.Precision, dbColumn.Scale));
                var def = TextFragment.Secondary(dbColumn.Definition?.RemoveLineReturns());
                list.Add((col, type, def));
            }

            return list;
        }


        private (int, int, int) GetTableWidths(IEnumerable<(TextFragment, TextFragment, TextFragment)> tableStruct)
        {
            var maxColumn = tableStruct.Max(p => p.Item1.Value.Length);
            var maxDatatype = tableStruct.Max(p => p.Item2.Value.Length);
            var maxDef = tableStruct.Max(p => p.Item3.Value.Length);

            return (maxColumn, maxDatatype, maxDef);
        }

        private TextFragments CreateTable(List<(TextFragment, TextFragment, TextFragment)> tableStruct, (int, int, int) widths)
        {
            var frags = new TextFragments();
            for (int i = 0; i < tableStruct.Count; i++)
            {
                if (i == 1)
                {
                    frags.AddSecondary(new string('-', widths.Item1) + "---" + new string('-', widths.Item2) + "---" + new string('-', widths.Item3) + "\n");
                }

                var line = tableStruct[i];
                frags.Add(line.Item1);
                frags.AddSecondary(GetComplement(line.Item1, widths.Item1));
                frags.AddSecondary(" | ");
                frags.Add(line.Item2);
                frags.AddSecondary(GetComplement(line.Item2, widths.Item2));
                frags.AddSecondary(" | ");
                frags.Add(line.Item3);
                frags.AddSecondary(GetComplement(line.Item3, widths.Item3));
                frags.AddSecondary("\n");
            }
            return frags;
        }

        private string GetComplement(TextFragment initial, int maxLength)
        {
            var missing = maxLength - initial.Value.Length;
            return new string(' ', missing);
        }

        public override IReadOnlyCollection<string> DbRealtivePath()
        {
            return new List<string>() { "UserTables", $"{SchemaName}.{Name}" };
        }
    }
}
