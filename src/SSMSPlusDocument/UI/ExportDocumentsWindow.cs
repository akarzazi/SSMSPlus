namespace SSMSPlusDocument.UI
{
    using Microsoft.VisualStudio.Shell;
    using SSMSPlusCore.Integration.Connection;
    using System;
    using System.Runtime.InteropServices;

    [Guid("ebbe0b2f-22b6-4bef-b9fe-f5b695f42be1")]
    public class ExportDocumentsWindow : ToolWindowPane
    {
        public ExportDocumentsControl Control { get { return this.Content as ExportDocumentsControl; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolWindow1"/> class.
        /// </summary>
        public ExportDocumentsWindow() : base(null)
        {
            this.Caption = "Schema Search";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            this.Content = new ExportDocumentsControl();
        }

        public void Intialize(DbConnectionString cnxStr)
        {
            this.Caption = "Export documents: " + cnxStr.Database;
            this.Control.Initialize(cnxStr);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.Content = null;
            GC.Collect();
        }
    }
}
