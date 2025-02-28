/*
 * Created by SharpDevelop.
 * User: Hans
 * Date: 2/28/2025
 * Time: 5:31 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms;

namespace AustriaWin
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

            bool isFirstInstance;
            // Please use a unique name for the mutex to prevent conflicts with other programs
            using (Mutex mtx = new Mutex(true, "AustriaTe", out isFirstInstance))
            {
                if (isFirstInstance)
                {
                    NotificationIcon notificationIcon = new NotificationIcon();
                    notificationIcon.notifyIcon.Visible = true;
                    Application.Run();
                    notificationIcon.notifyIcon.Dispose();
                }
                else
                {
                    // The application is already running
                    // TODO: Display message box or change focus to existing application instance
                }
            } // releases the Mutex
        }	
	}
}