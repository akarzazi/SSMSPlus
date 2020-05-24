namespace SSMSPlusHistory.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    using EnvDTE;

    using Microsoft.Extensions.Logging;
    using Microsoft.SqlServer.Management.Smo.RegSvrEnum;
    using Microsoft.SqlServer.Management.UI.VSIntegration;
    using Microsoft.VisualStudio.Shell;

    using SSMSPlusCore.Integration;

    using SSMSPlusHistory.Entities;
    using SSMSPlusHistory.Repositories;

    using Task = System.Threading.Tasks.Task;

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
