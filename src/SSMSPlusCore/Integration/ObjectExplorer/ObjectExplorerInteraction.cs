using Microsoft.SqlServer.Management.UI.VSIntegration.ObjectExplorer;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMSPlusCore.Integration.ObjectExplorer
{
    public class ObjectExplorerInteraction : IObjectExplorerInteraction
    {
        PackageProvider _packageProvider;

        public ObjectExplorerInteraction(PackageProvider packageProvider)
        {
            _packageProvider = packageProvider;
        }

        public async System.Threading.Tasks.Task SelectNodeAsync(string server, string dbName, IReadOnlyCollection<string> itemPath)
        {
            var objectExplorer = (await _packageProvider.AsyncPackage.GetServiceAsync(typeof(IObjectExplorerService))) as IObjectExplorerService;
            var objNode = ObjectExplorerHelper.GetObjectHierarchyNode(objectExplorer, server, dbName, itemPath);
            ObjectExplorerHelper.SelectNode(objectExplorer, objNode);
        }
    }
}
