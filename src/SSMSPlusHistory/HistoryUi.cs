using System;
using System.ComponentModel.Design;

using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

using SSMSPlusCore.Integration;

using SSMSPlusHistory.UI;

namespace SSMSPlusHistory
{
    public class HistoryUi
    {
        public const int CommandId = 1001;

        private PackageProvider _packageProvider;

        private IVsWindowFrame _window;
        private bool isRegistred = false;

        public HistoryUi(PackageProvider packageProvider)
        {
            _packageProvider = packageProvider;
        }

        public void Register()
        {
            if (isRegistred)
            {
                throw new Exception("HistoryUi is already registred");
            }

            isRegistred = true;

            var menuCommandID = new CommandID(MenuHelper.CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);

            _packageProvider.CommandService.AddCommand(menuItem);
        }

        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var toolWindow = _packageProvider.AsyncPackage.FindToolWindow(typeof(HistoryToolWindow), 0, true);
            _window = (IVsWindowFrame)toolWindow.Frame;
            _window.SetProperty((int)__VSFPROPID.VSFPROPID_FrameMode, VSFRAMEMODE.VSFM_MdiChild);
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(_window.Show());
        }
    }
}
