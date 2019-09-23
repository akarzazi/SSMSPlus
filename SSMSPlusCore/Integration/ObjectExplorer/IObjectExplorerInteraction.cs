using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSMSPlusCore.Integration.ObjectExplorer
{
    public interface IObjectExplorerInteraction
    {
        Task SelectNodeAsync(string server, string dbName, IReadOnlyCollection<string> itemPath);
    }
}