using Microsoft.VisualStudio.Shell;
using SSMSPlusCore.Integration.Connection;
using SSMSPlusSearch.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SSMSPlusSearch.UI
{
    [Guid("ebbe0b2f-22b6-4bef-b9fe-f5b695f42be0")]
    public class SearchToolWindow : ToolWindowPane
    {
        public SchemaSearchControl SchemaSearchControl { get { return this.Content as SchemaSearchControl; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolWindow1"/> class.
        /// </summary>
        public SearchToolWindow() : base(null)
        {
            this.Caption = "Schema Search";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            this.Content = new SchemaSearchControl();
        }

        public void Intialize(DbConnectionString cnxStr)
        {
            this.Caption = "Schema Search: " + cnxStr.Database;
            this.SchemaSearchControl.Initialize(cnxStr);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.Content = null;
            GC.Collect();
        }
    }
}
