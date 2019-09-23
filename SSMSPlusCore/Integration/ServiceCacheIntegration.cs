namespace SSMSPlusCore.Integration
{
    using Microsoft.SqlServer.Management.UI.VSIntegration;

    public class ServiceCacheIntegration : IServiceCacheIntegration
    {
        public void OpenScriptInNewWindow(string script)
        {
            ServiceCache.ScriptFactory.CreateNewBlankScript(Microsoft.SqlServer.Management.UI.VSIntegration.Editors.ScriptType.Sql);

            EnvDTE.TextDocument doc = (EnvDTE.TextDocument)ServiceCache.ExtensibilityModel.Application.ActiveDocument.Object(null);
            doc.EndPoint.CreateEditPoint().Insert(script);
        }
    }
}
