using AutoUpdaterDotNET;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cloudforce_Revamped
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Creating Cloudforce Directory's before startup
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Cloudforce\\");
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Cloudforce\\Patches\\");
            AutoUpdater.ShowRemindLaterButton = false;
            AutoUpdater.RunUpdateAsAdmin = false;
            AutoUpdater.DownloadPath = Environment.SpecialFolder.ApplicationData + "\\Cloudforce\\";
            AutoUpdater.Start("https://raw.githubusercontent.com/zortos293/CloudForce-Revamped/master/cfupdate.xml");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}
