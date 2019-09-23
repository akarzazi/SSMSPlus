using SSMSPlusCore.App;
using SSMSPlusCore.Settings;
using SSMSPlusCore.Ui;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMSPlusPreferences.UI
{
    public class PreferencesWindowVM : ViewModelBase
    {
        private IVersionProvider _versionProvider;
        private IWorkingDirProvider _workingDirProvider;
        public DistributionSettings DistributionSettings { get; private set; }

        public Command OpenWorkingDirCmd { get; private set; }
        public Command OpenContributeCmd { get; private set; }

        public PreferencesWindowVM(IVersionProvider versionProvider, IWorkingDirProvider workingDirProvider, DistributionSettings distributionSettings)
        {
            _versionProvider = versionProvider;
            _workingDirProvider = workingDirProvider;
            DistributionSettings = distributionSettings;
            OpenWorkingDirCmd = new Command(OnOpenWorkingDir, null, HandleError);
            OpenContributeCmd = new Command(OnOpenContribute, null, HandleError);
            InitDefaults();
        }

        private void OnOpenWorkingDir()
        {
            Process.Start(_workingDirProvider.GetWorkingDir());
        }

        private void OnOpenContribute()
        {
            Process.Start(DistributionSettings.ContributeUrl);
        }

        private void HandleError(Exception ex)
        {
            // nothing for now
        }

        private void InitDefaults()
        {
            Version = string.Join(".", _versionProvider.GetBuildAndRevision());
            WorkingDirPath = _workingDirProvider.GetWorkingDir();
        }

        public string Version { get; private set; }
        public string WorkingDirPath { get; private set; }
    }
}