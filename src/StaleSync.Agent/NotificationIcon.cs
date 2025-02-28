using System;
using System.Windows.Forms;
using StaleSync.Resources;

namespace StaleSync
{
    public sealed class NotificationIcon
    {
        internal NotifyIcon Icon;

        public NotificationIcon()
        {
            Icon = new NotifyIcon();
            var notificationMenu1 = new ContextMenuStrip();
            notificationMenu1.Items.AddRange(InitializeMenu());

            Icon.DoubleClick += IconDoubleClick;
            Icon.Icon = ResLoader.GetIcon("logo.ico");
            Icon.ContextMenuStrip = notificationMenu1;
        }

        private ToolStripItem[] InitializeMenu()
        {
            ToolStripItem[] menu =
            {
                new ToolStripMenuItem("About", null, MenuAboutClick),
                new ToolStripMenuItem("Exit", null, MenuExitClick)
            };
            return menu;
        }

        private static void MenuAboutClick(object sender, EventArgs e)
        {
            MessageBox.Show("This is a memory-resident agent.", nameof(StaleSync));
        }

        private static void MenuExitClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private static void IconDoubleClick(object sender, EventArgs e)
        {
            var form = new MainForm();
            form.ShowDialog();
        }
    }
}