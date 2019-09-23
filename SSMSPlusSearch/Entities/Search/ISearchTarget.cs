using SSMSPlusCore.Ui.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMSPlusSearch.Entities
{
    public interface ISearchTarget
    {
        string UniqueIdentifier { get; }

        string Type { get; }
        string DisplayType { get; }
        string SchemaName { get; }
        string Name { get; }
        DbObjectType SqlObjectType { get; }
        DbSimplifiedType TypeCategory { get; }
        DateTime? ModificationDate { get; }
        string ModificationDateStr { get; }

        TextFragments RichName { get; }
        TextFragments RichSmallDefinition { get; }
        TextFragments RichFullDefinition { get; }

        TextFragments RichNameHighlight(string search);
        TextFragments RichSmallDefinitionHighlight(string search);
        TextFragments RichFullDefinitionHighlight(string search);

        bool MatchsName(string search);
        bool MatchsDefinition(string search);

        IReadOnlyCollection<string> DbRealtivePath();
    }
}
