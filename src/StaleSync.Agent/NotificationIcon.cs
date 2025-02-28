/*
 * Created by SharpDevelop.
 * User: Hans
 * Date: 2/28/2025
 * Time: 12:51 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace AustriaTe
{
    public sealed class NotificationIcon
    {
        private NotifyIcon notifyIcon;
        private ContextMenu notificationMenu;

        #region Initialize icon and menu

        public NotificationIcon()
        {
            notifyIcon = new NotifyIcon();
            notificationMenu = new ContextMenu(InitializeMenu());

            notifyIcon.DoubleClick += IconDoubleClick;
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(NotificationIcon));
            notifyIcon.Icon = (Icon)resources.GetObject("$this.Icon");
            notifyIcon.ContextMenu = notificationMenu;
        }

        private MenuItem[] InitializeMenu()
        {
            MenuItem[] menu = new MenuItem[]
            {
                new MenuItem("About", menuAboutClick),
                new MenuItem("Exit", menuExitClick)
            };
            return menu;
        }

        #endregion

        #region Main - Program entry point

        

        #endregion

        #region Event Handlers

        private void menuAboutClick(object sender, EventArgs e)
        {
            MessageBox.Show("About This Application");
        }

        private void menuExitClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void IconDoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show("The icon was double clicked");
        }

        #endregion
    }
}