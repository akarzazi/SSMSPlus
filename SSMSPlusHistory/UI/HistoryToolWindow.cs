using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;

namespace SSMSPlusHistory.UI
{
    [Guid("ebbe0b2f-22b6-4bef-b9fe-f5b695f42be5")]
    public class HistoryToolWindow : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToolWindow1"/> class.
        /// </summary>
        public HistoryToolWindow() : base(null)
        {
            this.Caption = "Execution History";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            this.Content = new HistoryControl();
        }
    }
}
