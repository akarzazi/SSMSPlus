using System;
using System.Collections.Generic;
using System.Text;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using SSMSPlusHistory.Repositories;
using SSMSPlusHistory.Services;

namespace SSMSPlusHistory
{
    public class HistoryPlugin
    {
        private bool isRegistred = false;
        private QueryTracker _queryTracker;
        private HistoryUi _historyUi;

        public HistoryPlugin(QueryTracker queryTracker, HistoryUi historyUi)
        {
            _queryTracker = queryTracker;
            _historyUi = historyUi;
        }


        public void Register()
        {
            if (isRegistred)
            {
                throw new Exception("HistoryPlugin is already registred");
            }

            isRegistred = true;
            _queryTracker.StartTraking();
            _historyUi.Register();
        }
    }
}
