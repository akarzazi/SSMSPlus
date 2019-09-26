namespace SSMSPlusCore.Integration
{
    using EnvDTE80;
    using Microsoft.VisualStudio.Shell;

    public class PackageProvider
    {
        public DTE2 Dte2 { get; private set; }
        public AsyncPackage AsyncPackage { get; private set; }
        public OleMenuCommandService CommandService { get; private set; }

        public PackageProvider(DTE2 dte2, AsyncPackage asyncPackage, OleMenuCommandService commandService)
        {
            Dte2 = dte2;
            AsyncPackage = asyncPackage;
            CommandService = commandService;
        }
    }
}
