namespace SSMSPlusCore.Ui
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Automation.Peers;

    public class EmptyAutomationPeer : FrameworkElementAutomationPeer
    {
        public EmptyAutomationPeer(FrameworkElement owner) : base(owner) { }

        protected override string GetNameCore()
        {
            return "EmptyAutomationPeer";
        }

        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Window;
        }

        protected override List<AutomationPeer> GetChildrenCore()
        {
            return new List<AutomationPeer>();
        }
    }
}
