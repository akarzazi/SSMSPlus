using Microsoft.SqlServer.Management.UI.VSIntegration.ObjectExplorer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSMSPlusSearch.Menu
{
    public class DbContextMenu : ToolsMenuItemBase, IWinformsMenuHandler
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public DbContextMenu()
        {
            this.Text = "Script Data as INSERT";
        }

        /// <summary>
        /// Invoke
        /// </summary>
        protected override void Invoke()
        {

        }

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new DbContextMenu();
        }


        /// <summary>
        /// Get Menu Items
        /// </summary>
        /// <returns></returns>
        public System.Windows.Forms.ToolStripItem[] GetMenuItems()
        {
            ToolStripMenuItem item = new ToolStripMenuItem("Script Data as");
            ToolStripMenuItem insertItem = new ToolStripMenuItem("INSERT");
            insertItem.Tag = false;
            insertItem.Click += new EventHandler(InsertItem_Click);

            item.DropDownItems.Add(insertItem);


            ToolStripMenuItem scriptIt = new ToolStripMenuItem("Script Full table Schema");
            scriptIt.Click += new EventHandler(scriptIt_Click);

            return new ToolStripItem[] { item, scriptIt };
        }

        void scriptIt_Click(object sender, EventArgs e)
        {


        }


        void InsertItem_Click(object sender, EventArgs e)
        {


        }

    }
}
