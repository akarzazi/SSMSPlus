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
    public class OtherSearchTarget : SearchTargetBase
    {
        public OtherSearchTarget(DbObject dbObject)
        {
            DbObject = dbObject;
        }

        public DbObject DbObject { get; }

        public override string UniqueIdentifier => Guid.NewGuid().ToString();

        public override string Type => DbObject.Type;

        public override string DisplayType => TypeCategory.Name + " (" + DbObject.Type + ")";

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
                var frags = new TextFragments();
                frags.AddPrimary(smallDef);
                return frags;
            }
        }

        public override TextFragments RichFullDefinition
        {
            get
            {
                var frags = new TextFragments();
                frags.AddPrimary(DbObject.Definition);
                return frags;
            }
        }
        public override IReadOnlyCollection<string> DbRealtivePath()
        {
            if (SqlObjectType == DbObjectType.SYNONYM)
            {
                return SynonymPath();
            }

            return base.DbRealtivePath();
        }

        public IReadOnlyCollection<string> SynonymPath()
        {
            return new List<string>() { "Synonyms", $"{SchemaName}.{Name}" };
        }
    }
}
