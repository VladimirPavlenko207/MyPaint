using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ex_Paint.HelperClasses {
    public static class ContextMenuSetting {

        internal static void CreateContextMenu(
            Control control, string[] controlContextMenuItems) {
            control.ContextMenuStrip = new ContextMenuStrip();

            for (int i = 0; i < controlContextMenuItems.Length; i++) {
                ToolStripMenuItem tsmi = new ToolStripMenuItem() {
                    Text = controlContextMenuItems[i]
                };
                control.ContextMenuStrip.Items.Add(tsmi);
            }
        }
        

        internal static void SubScribeTsmiToClick(Control control, EventHandler controlTsmi_Click) {
            for (int i = 0; i < control.ContextMenuStrip.Items.Count; i++) {
                control.ContextMenuStrip.Items[i].Click += controlTsmi_Click;
            }
        }

        internal static void SubScribeContextMenuStripToOpened(Control control, EventHandler controlContextMenuStrip_Opened) {
            control.ContextMenuStrip.Opened += controlContextMenuStrip_Opened;
        }
    }
}
