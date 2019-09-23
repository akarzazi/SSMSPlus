namespace SSMSPlusCore.Integration.ObjectExplorer
{
    using Microsoft.SqlServer.Management.Common;
    using Microsoft.SqlServer.Management.UI.VSIntegration.ObjectExplorer;

    public class ObjectExplorerServer
    {
        public SqlConnectionInfo ConnectionInfo => NodeInformation.Connection as SqlConnectionInfo;
        public INodeInformation NodeInformation { get; private set; }
        public IExplorerHierarchy Hierarchy { get; private set; }
        public HierarchyTreeNode Root => Hierarchy.Root;

        public ObjectExplorerServer(INodeInformation nodeInformation, IExplorerHierarchy hierarchy)
        {
            NodeInformation = nodeInformation;
            Hierarchy = hierarchy;
        }
    }
}
