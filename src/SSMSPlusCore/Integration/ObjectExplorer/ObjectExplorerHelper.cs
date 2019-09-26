using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.UI.VSIntegration.ObjectExplorer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SSMSPlusCore.Integration.ObjectExplorer
{
    public static class ObjectExplorerHelper
    {
        public static string DB_URNPATH = "Server/Database";
        public static string SERVER_URNPATH = "Server";

        public static INodeInformation FindSelectedNode(IObjectExplorerService explorerService)
        {
            int arraySize;
            INodeInformation[] array;
            explorerService.GetSelectedNodes(out arraySize, out array);

            if (arraySize == 0)
                return null;

            return array[0];
        }

        public static object FollowPropertyPath(object value, string path)
        {
            Type currentType = value.GetType();

            foreach (string propertyName in path.Split('.'))
            {
                PropertyInfo property = currentType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                value = property.GetValue(value, null);
                currentType = property.PropertyType;
            }
            return value;
        }

        public static INodeInformation FindSelectedDatabaseNode(IObjectExplorerService explorerService)
        {
            var selectedNode = FindSelectedNode(explorerService);
            if (selectedNode == null)
                return null;

            return FindParentDatabaseNode(selectedNode);
        }

        public static INodeInformation FindParentDatabaseNode(INodeInformation node)
        {
            return FindParentNode(node, DB_URNPATH);
        }

        public static INodeInformation FindParentServerNode(INodeInformation node)
        {
            return FindParentNode(node, SERVER_URNPATH);
        }

        public static INodeInformation FindParentNode(INodeInformation node, string searchUrnPath)
        {
            do
            {
                if (node.UrnPath == searchUrnPath)
                    return node;

                node = node.Parent;
            } while (node != null);

            return null;
        }

        public static IReadOnlyCollection<ObjectExplorerServer> GetServersConnection(IObjectExplorerService explorerService)
        {
            List<ObjectExplorerServer> servers = new List<ObjectExplorerServer>();

            var hierarchies = FollowPropertyPath(explorerService, "Tree.Hierarchies") as IEnumerable<KeyValuePair<string, IExplorerHierarchy>>;
            foreach (KeyValuePair<string, IExplorerHierarchy> srvHierarchy in hierarchies)
            {
                IServiceProvider provider = srvHierarchy.Value.Root as IServiceProvider;

                if (provider != null)
                {
                    INodeInformation nodeInformation = provider.GetService(typeof(INodeInformation)) as INodeInformation;
                    servers.Add(new ObjectExplorerServer(nodeInformation, srvHierarchy.Value));
                }
            }
            return servers;
        }

        public static void EnumerateChildrenSynchronously(HierarchyTreeNode node)
        {
            Type t = node.GetType();
            MethodInfo method = t.GetMethod("EnumerateChildren", new Type[] { typeof(Boolean) });

            if (method != null)
            {
                method.Invoke(node, new Object[] { false });
            }
            else
            {
                node.EnumerateChildren();
            }
        }

        public static string GetInvariantPath(this HierarchyTreeNode node)
        {
            return FollowPropertyPath(node, "InvariantPath") as string;
        }

        public static ObjectExplorerServer GetServerHierarchyNode(IObjectExplorerService explorerService, string serverName)
        {
            var cnx = GetServersConnection(explorerService);
            return cnx.SingleOrDefault(p => string.Compare(p.ConnectionInfo.ServerName, serverName, true) == 0);
        }

        public static HierarchyTreeNode GetObjectHierarchyNode(IObjectExplorerService explorerService, string serverName, string dbName, IReadOnlyCollection<string> dbRelativePath)
        {
            var server = GetServerHierarchyNode(explorerService, serverName);
            var rootNode = server.Hierarchy.Root;

            var pathSegments = new List<string> { "Databases", dbName }.Concat(dbRelativePath);

            foreach (var pathSegment in pathSegments)
            {
                EnumerateChildrenSynchronously(rootNode);
                var parentPath = rootNode.GetInvariantPath();
                var currentPath = parentPath + "/" + pathSegment;

                rootNode = rootNode.Nodes.OfType<HierarchyTreeNode>().SingleOrDefault(p => p.GetInvariantPath() == currentPath);
                if (rootNode == null)
                {
                    throw new Exception($"Could not unfold path: {currentPath}");
                }
            }

            return rootNode;
        }

        public static void SelectNode(IObjectExplorerService explorerService, HierarchyTreeNode node)
        {
            IServiceProvider provider = node as IServiceProvider;

            if (provider != null)
            {
                INodeInformation containedItem = provider.GetService(typeof(INodeInformation)) as INodeInformation;

                if (containedItem != null)
                {
                    explorerService.SynchronizeTree(containedItem);
                }
            }
        }
    }
}
