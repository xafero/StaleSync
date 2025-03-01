using System;
using System.Threading;
using System.Windows.Forms;
using StaleSync.Core;
using StaleSync.Resources;

// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable

namespace StaleSync
{
    public sealed class NotificationIcon
    {
        private readonly Thread _thread;
        internal NotifyIcon Icon;

        public NotificationIcon()
        {
            Icon = new NotifyIcon();
            var menu = new ContextMenuStrip();
            menu.Items.AddRange(InitializeMenu());

            Icon.DoubleClick += IconDoubleClick;
            Icon.Icon = ResLoader.GetIcon("logo.ico");
            Icon.ContextMenuStrip = menu;

            App.ShowTip = ShowTip;
            _thread = new Thread(App.Run);
            _thread.Start();
        }

        private void ShowTip(TipArg tip)
        {
            var kind = ToolTipIcon.Info;
            Icon.ShowBalloonTip(tip.Timeout, tip.Title, tip.Text, kind);
        }

        private static ToolStripItem[] InitializeMenu()
        {
            ToolStripItem[] menu =
            [
                new ToolStripMenuItem("About", null, MenuAboutClick),
                new ToolStripMenuItem("Exit", null, MenuExitClick)
            ];
            return menu;
        }

        private static void MenuAboutClick(object sender, EventArgs e)
        {
            MessageBox.Show("This is a memory-resident agent.", nameof(StaleSync));
        }

        private static void MenuExitClick(object sender, EventArgs e)
        {
            Quit();
        }

        private static void Quit()
        {
            Client.Instance.Disconnect();
            Application.Exit();
        }

        private static void IconDoubleClick(object sender, EventArgs e)
        {
            var form = new MainForm();
            form.ShowDialog();
        }
    }
}