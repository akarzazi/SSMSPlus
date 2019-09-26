using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSMSPlusCore.Ui.Search;

namespace SSMSPlusSearch.Entities.Search
{
    public abstract class SearchTargetBase : ISearchTarget
    {
        public abstract string UniqueIdentifier { get; }
        public abstract string Type { get; }
        public virtual string DisplayType => TypeCategory.Name;
        public abstract string SchemaName { get; }
        public abstract string Name { get; }
        public abstract DateTime? ModificationDate { get; }
        public abstract string ModificationDateStr { get; }
        public abstract TextFragments RichName { get; }
        public abstract TextFragments RichSmallDefinition { get; }
        public abstract TextFragments RichFullDefinition { get; }


        private DbObjectType _sqlObjectType;
        public DbObjectType SqlObjectType => _sqlObjectType ?? (_sqlObjectType = DbObjectType.Parse(Type));

        public DbSimplifiedType TypeCategory => SqlObjectType.Category;

        public bool MatchsName(string search)
        {
            return RichName.PrimaryContainsText(search);
        }

        public bool MatchsDefinition(string search)
        {
            return RichFullDefinition.PrimaryContainsText(search);
        }

        public TextFragments RichNameHighlight(string search)
        {
            return RichName.HighlightPrimary(search);
        }

        public TextFragments RichSmallDefinitionHighlight(string search)
        {
            return RichSmallDefinition.HighlightPrimary(search);
        }

        public TextFragments RichFullDefinitionHighlight(string search)
        {
            return RichFullDefinition.HighlightPrimary(search);
        }

        public virtual IReadOnlyCollection<string> DbRealtivePath()
        {
            return new List<string>() {  };
        }
    }
}
