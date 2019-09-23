using Microsoft.SqlServer.Management.Smo.RegSvrEnum;
using Microsoft.SqlServer.Management.UI.VSIntegration;
using Microsoft.SqlServer.Management.UI.VSIntegration.ObjectExplorer;
using Microsoft.VisualStudio.Shell;
using SSMSPlusCore.Integration.ObjectExplorer;
using SSMSPlusCore.SqlServer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMSPlusCore.Integration.Connection
{
    public class DbConnectionProvider
    {
        PackageProvider _packageProvider;

        public DbConnectionProvider(PackageProvider packageProvider)
        {
            _packageProvider = packageProvider;
        }

        public DbConnectionString GetFromActiveConnection()
        {
            UIConnectionInfo connInfo = ServiceCache.ScriptFactory.CurrentlyActiveWndConnectionInfo?.UIConnectionInfo;
            if (connInfo == null || connInfo.AdvancedOptions["DATABASE"] == null || DbHelper.IsSystemDb(connInfo.AdvancedOptions["DATABASE"]))
            {
                return null;
            }

            return new DbConnectionString(GetConnectionString(connInfo), connInfo.AdvancedOptions["DATABASE"]);
        }

        public DbConnectionString GetFromSelectedDatabase()
        {
            var objectExplorerService = _packageProvider.AsyncPackage.GetServiceAsync(typeof(IObjectExplorerService)).Result as IObjectExplorerService;

            var dbNode = ObjectExplorerHelper.FindSelectedDatabaseNode(objectExplorerService);
            if (dbNode == null)
            {
                return null;
            }
            return new DbConnectionString(dbNode.Connection.ConnectionString, dbNode.InvariantName);
        }

        internal static string GetConnectionString(UIConnectionInfo connection)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.DataSource = connection.ServerName;
            builder.IntegratedSecurity = string.IsNullOrEmpty(connection.Password);
            builder.Password = connection.Password;
            builder.UserID = connection.UserName;
            builder.InitialCatalog = connection.AdvancedOptions["DATABASE"];
            builder.ApplicationName = "SSMS Plus";

            return builder.ToString();
        }
    }
}
