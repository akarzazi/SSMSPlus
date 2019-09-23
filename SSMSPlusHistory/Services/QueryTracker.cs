namespace SSMSPlusHistory.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Windows.Controls;
    using System.Windows.Forms;
    using EnvDTE;
    using EnvDTE80;
    using Microsoft.Extensions.Logging;
    using Microsoft.SqlServer.Management.Smo.RegSvrEnum;
    using Microsoft.SqlServer.Management.UI.VSIntegration;
    using Microsoft.SqlServer.Management.UI.VSIntegration.ObjectExplorer;
    using Microsoft.VisualStudio.Shell;
    using SSMSPlusCore.Integration;
    using SSMSPlusHistory.Entities;
    using SSMSPlusHistory.Repositories;
    using Task = System.Threading.Tasks.Task;

    public class tableMenuItem : ToolsMenuItemBase, IWinformsMenuHandler
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public tableMenuItem()
        {
            this.Text = "Script Data as INSERT";
        }

        /// <summary>
        /// Invoke
        /// </summary>
        protected override void Invoke()
        {

        }

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new tableMenuItem();
        }


        /// <summary>
        /// Get Menu Items
        /// </summary>
        /// <returns></returns>
        public System.Windows.Forms.ToolStripItem[] GetMenuItems()
        {
            ToolStripMenuItem item = new ToolStripMenuItem("Script Data as");
            ToolStripMenuItem insertItem = new ToolStripMenuItem("INSERT");
            insertItem.Tag = false;
            insertItem.Click += new EventHandler(InsertItem_Click);

            item.DropDownItems.Add(insertItem);


            ToolStripMenuItem scriptIt = new ToolStripMenuItem("Script Full table Schema");
            scriptIt.Click += new EventHandler(scriptIt_Click);

            return new ToolStripItem[] { item, scriptIt };
        }

        void scriptIt_Click(object sender, EventArgs e)
        {


        }


        void InsertItem_Click(object sender, EventArgs e)
        {


        }

    }
    public class QueryTracker
    {
        private ILogger<QueryTracker> _logger;
        private QueryItemRepository _queryItemRepository;
        private PackageProvider _packageProvider;

        private bool isTracking = false;
        private ConcurrentQueue<QueryItem> itemsQueue = new ConcurrentQueue<QueryItem>();

        public CommandEvents QueryExecuteEvent { get; private set; }

        public QueryTracker(PackageProvider packageProvider, ILogger<QueryTracker> logger, QueryItemRepository queryItemRepository)
        {
            _logger = logger;
            _queryItemRepository = queryItemRepository;
            _packageProvider = packageProvider;
        }


        public void StartTraking()
        {
            if (isTracking)
            {
                throw new Exception("HistoryPlugin is already registred");
            }

            isTracking = true;
            var dte2 = _packageProvider.Dte2;

            var command = dte2.Commands.Item("Query.Execute");

            QueryExecuteEvent = dte2.Events.get_CommandEvents(command.Guid, command.ID);
            QueryExecuteEvent.BeforeExecute += this.CommandEvents_BeforeExecute;
        }

        private void CommandEvents_BeforeExecute(string Guid, int ID, object CustomIn, object CustomOut, ref bool CancelDefault)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            //Microsoft.SqlServer.Management.SqlStudio.
            //var objectExplorerService = (ObjectExplorerService)_package.GetServiceAsync(typeof(IObjectExplorerService)).Result;

            //// var oesTreeProperty = objectExplorerService.GetType().GetProperty("Tree", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase);
            ////// if (oesTreeProperty != null)

            ////     var x = (TreeView)oesTreeProperty.GetValue(objectExplorerService, null);

            //int arraySize;
            //INodeInformation[] array;
            //objectExplorerService.GetSelectedNodes(out arraySize, out array);


            //if (arraySize == 0)
            //    return;

            //var node = array[0];

            //var table = (HierarchyObject)node.GetService(typeof(IMenuHandler));
            //table.AddChild("abc", new tableMenuItem());

            //Microsoft.SqlServer.Management.UI.VSIntegration.ObjectExplorer.FilterOperator

            try
            {
                string queryText = GetQueryText();

                if (string.IsNullOrWhiteSpace(queryText))
                    return;

                // Get Current Connection Information
                UIConnectionInfo connInfo = ServiceCache.ScriptFactory.CurrentlyActiveWndConnectionInfo.UIConnectionInfo;

                var queryItem = new QueryItem()
                {
                    Query = queryText,
                    Server = connInfo.ServerName,
                    Username = connInfo.UserName,
                    Database = connInfo.AdvancedOptions["DATABASE"],
                    ExecutionDateUtc = DateTime.UtcNow
                };

                _logger.LogInformation("Enqueued {@quetyItem}", queryItem.Query);

                itemsQueue.Enqueue(queryItem);

                Task.Delay(1000).ContinueWith((t) => this.SavePendingItems());
            }
            catch (Exception ex)
            {
                _logger.LogError("Error on BeforeExecute tracking", ex);
            }
        }

        private string GetQueryText()
        {
            Document document = _packageProvider.Dte2.ActiveDocument;
            if (document == null)
                return null;


            var textDocument = (TextDocument)document.Object("TextDocument");
            string queryText = textDocument.Selection.Text;

            if (string.IsNullOrEmpty(queryText))
            {
                EditPoint startPoint = textDocument.StartPoint.CreateEditPoint();
                queryText = startPoint.GetText(textDocument.EndPoint);
            }

            return queryText;
        }

        private async Task SavePendingItems()
        {
            List<QueryItem> pendingItems = new List<QueryItem>();
            lock (itemsQueue)
            {
                QueryItem queryItem;
                while (itemsQueue.TryDequeue(out queryItem))
                {
                    pendingItems.Add(queryItem);
                }
            }

            await _queryItemRepository.Insert(pendingItems);
        }
    }
}
