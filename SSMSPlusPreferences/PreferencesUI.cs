using SSMSPlusCore.Integration;
using SSMSPlusCore.Ui.Extensions;
using SSMSPlusPreferences.UI;
using System;
using System.ComponentModel.Design;

namespace SSMSPlusPreferences
{
    public class PreferencesUI
    {
        public const int MenuCommandId = 2001;

        private bool isRegistred = false;
        private PreferencesWindow _window;

        private PackageProvider _packageProvider;

        public PreferencesUI(PackageProvider packageProvider)
        {
            _packageProvider = packageProvider;
        }

        public void Register()
        {
            if (isRegistred)
            {
                throw new Exception("PreferencesUI is already registred");
            }

            isRegistred = true;

            var menuItem = new MenuCommand(this.ExecuteFromMenu, new CommandID(MenuHelper.CommandSet, MenuCommandId));
            _packageProvider.CommandService.AddCommand(menuItem);
        }

        private void ExecuteFromMenu(object sender, EventArgs e)
        {
            if (_window == null)
                _window = new PreferencesWindow();

            _window.ShowAndActivate();
        }
    }
}
