namespace SSMSPlusSearch.Services
{
    using Microsoft.VisualStudio.Shell.Interop;
    using SSMSPlusCore.Integration;
    using SSMSPlusCore.Integration.Connection;
    using SSMSPlusSearch.UI;
    using System;
    using System.ComponentModel.Design;

    public class SearchUi
    {
        public const int MenuCommandId = 1101;

        private PackageProvider _packageProvider;
        DbConnectionProvider _dbConnectionProvider;

        private bool isRegistred = false;


        public SearchUi(PackageProvider packageProvider, DbConnectionProvider dbConnectionProvider)
        {
            _packageProvider = packageProvider;
            _dbConnectionProvider = dbConnectionProvider;
        }

        public void Register()
        {
            if (isRegistred)
            {
                throw new Exception("SearchUi is already registred");
            }

            isRegistred = true;

            var menuItem = new MenuCommand(this.ExecuteFromMenu, new CommandID(MenuHelper.CommandSet, MenuCommandId));
            _packageProvider.CommandService.AddCommand(menuItem);
        }

        private int id;

        private void ExecuteFromMenu(object sender, EventArgs e)
        {
            var cnx = _dbConnectionProvider.GetFromSelectedDatabase();
            if (cnx == null)
            {
                cnx = _dbConnectionProvider.GetFromActiveConnection();
                if (cnx == null)
                {
                    System.Windows.MessageBox.Show(
@"Please select a user database in object explorer
Or 
Connect to a user database", "SSMS plus");
                    return;
                }
            }

            Launch(cnx);
        }

        private void Launch(DbConnectionString dbConnectionString)
        {
            var toolWindow = _packageProvider.AsyncPackage.FindToolWindow(typeof(SearchToolWindow), id++, true) as SearchToolWindow;
            toolWindow.Intialize(dbConnectionString);

            var frame = (IVsWindowFrame)toolWindow.Frame;
            frame.SetProperty((int)__VSFPROPID.VSFPROPID_FrameMode, VSFRAMEMODE.VSFM_MdiChild);
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(frame.Show());
        }
    }
}
