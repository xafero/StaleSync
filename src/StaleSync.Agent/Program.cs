using System;
using System.Threading;
using System.Windows.Forms;

// ReSharper disable UseObjectOrCollectionInitializer

namespace StaleSync
{
    internal sealed class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (_ = new Mutex(true, nameof(StaleSync),
                       out var isFirstInstance))
            {
                if (isFirstInstance)
                {
                    var tray = new NotificationIcon();
                    tray.Icon.Visible = true;
                    Application.Run();
                    tray.Icon.Dispose();
                }
                else
                {
                    MessageBox.Show("Application is already running!",
                        "Start", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }
    }
}