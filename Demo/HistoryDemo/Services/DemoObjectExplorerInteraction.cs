using SSMSPlusCore.Integration.ObjectExplorer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Services
{
    public class DemoObjectExplorerInteraction : IObjectExplorerInteraction
    {
        Task IObjectExplorerInteraction.SelectNodeAsync(string server, string dbName, IReadOnlyCollection<string> itemPath)
        {
            return Task.CompletedTask;
        }
    }
}
