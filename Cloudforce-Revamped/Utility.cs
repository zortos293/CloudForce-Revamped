using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Cloudforce_Revamped
{
    public partial class Utility : Form
    {
        string mainpath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Cloudforce\\";
        public Utility()
        {
            InitializeComponent();
            kick_timer.Start();
        }
        #region Waiting GFN 
        Timer kick_timer = new Timer();
        
        #endregion

        #region Theme
        void checktheme()
        {
            if (Main.Dark == true)
            {
                this.BackColor = Color.FromArgb(64, 64, 64);
                changecolorBTN(123, 0, 238, 170, 0, 255);
            }
            if (Main.Light == true)
            {
                this.BackColor = Color.WhiteSmoke;
                changecolorBTN(255, 128, 128, 255, 128, 255);
            }
        }

        private void changecolorBTN(int one, int two, int three, int one1, int two1, int three1)
        {
            foreach (var button in Controls.OfType<Guna.UI2.WinForms.Guna2GradientButton>())
            {
                button.FillColor = Color.FromArgb(one, two, three);
                button.FillColor2 = Color.FromArgb(one1, two1, three1);
            }
        }

        #endregion

        #region Download Stuff
        bool DownloadFinished;
        public void File_Downloader(string URL, string path,Guna.UI2.WinForms.Guna2GradientButton button)
        {
            // download file with progress bar
            DownloadFinished = false;
            WebClient client = new WebClient();
            back.Enabled = false;
            button.Enabled = false;
            guna2ProgressBar1.Value = 0;
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(FileDownloadComplete);
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadChanged);
            client.DownloadFileAsync(new Uri(URL), path);
            while (DownloadFinished == false)
                Application.DoEvents();
        }
        private void ResetButtons(bool Switch)
        {
            // Loop through each control in this container
            foreach (var button in Controls.OfType<Guna.UI2.WinForms.Guna2GradientButton>())
                button.Enabled = Switch;
        }
        private void FileDownloadComplete(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                ResetButtons(true);
                back.Enabled = true;
                DownloadFinished = true;
                guna2ProgressBar1.Value = 0;
                ((WebClient)sender).Dispose();
            }
            else
            {
                MessageBox.Show(e.Error.Message);
            }
        }
        private void DownloadChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            guna2ProgressBar1.Value = e.ProgressPercentage;
        }
        #endregion


        
        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2GradientButton12_Click(object sender, EventArgs e)
        {
            if (File.Exists(mainpath + "\\ProcessHacker\\ProcessHacker.exe"))
            {
                Process.Start(mainpath + "\\ProcessHacker\\\\ProcessHacker.exe");
            }
            else
            {
                File_Downloader("https://picteon.dev/files/ProcessHacker.zip", mainpath + "\\ProcessHacker.zip", guna2GradientButton12);

                ZipFile.ExtractToDirectory(mainpath + "\\ProcessHacker.zip", mainpath + "\\");
                Process.Start(mainpath + "\\ProcessHacker\\\\ProcessHacker.exe"); 
                guna2GradientButton12.Enabled = true;

            }
        }

        private void back_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2GradientButton11_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(mainpath + "\\Discord\\"))
            {
                Process.Start(mainpath + "\\Discord.zip");
            }
            else
            {
                File_Downloader("https://picteon.dev/files/Discord.zip", mainpath + "\\Discord.zip", guna2GradientButton11);
                    
                ZipFile.ExtractToDirectory(mainpath + "\\Discord.zip", mainpath + "\\");
                Process.Start(mainpath + "\\discord2\\discord-portable.exe"); // TODO
                guna2GradientButton1.Enabled = false;

            }
        }

        private void Utility_Shown(object sender, EventArgs e)
        {
            checktheme();
        }

        private void guna2GradientButton14_Click(object sender, EventArgs e) // Firefox
        {
            if (Directory.Exists(mainpath + "\\Firefox\\"))
            {
                Process.Start(mainpath + "\\Firefox\\runthis.exe");
            }
            else
            {
                File_Downloader("https://picteon.dev/files/Firefox.zip", mainpath + "\\Firefox.zip", guna2GradientButton11);

                ZipFile.ExtractToDirectory(mainpath + "\\Firefox.zip", mainpath + "\\");
                Process.Start(mainpath + "\\Firefox\\runthis.exe"); 
                guna2GradientButton1.Enabled = false;

            }
        }

        private void guna2GradientButton4_Click(object sender, EventArgs e)
        {
            if (File.Exists(mainpath + "\\DoraTheExplorer.exe"))
            {
                Process.Start(mainpath + "\\DoraTheExplorer.exe");
            }
            else
            {
                File_Downloader("https://picteon.dev/files/Explorer++.exe", mainpath + "\\DoraTheExplorer.exe", guna2GradientButton4);


                Process.Start(mainpath + "\\ProcessHacker\\\\ProcessHacker.exe"); // TODO
                guna2GradientButton4.Enabled = true;

            }
        }

        private void Utility_Load(object sender, EventArgs e)
        {

        }

        private void guna2GradientButton15_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(mainpath + "\\librewolf-105.0.1\\"))
            {
                Process.Start(mainpath + "\\librewolf-105.0.1\\LibreWolf-Portable.exe");
            }
            else
            {
                File_Downloader("https://gitlab.com/librewolf-community/browser/windows/uploads/a9d86b83d8e66b9c3c75a0c2221aecdd/librewolf-105.0.1-1.en-US.win64-portable.zip", mainpath + "\\librewolf.zip", guna2GradientButton11);

                ZipFile.ExtractToDirectory(mainpath + "\\librewolf.zip", mainpath + "\\");
                Process.Start(mainpath + "\\librewolf-105.0.1\\LibreWolf-Portable.exe");
                guna2GradientButton1.Enabled = false;

            }
        }

        private void guna2GradientButton10_Click(object sender, EventArgs e)
        {
            if (File.Exists(mainpath + "\\ZortosDesktop.exe"))
            {
                Process.Start(mainpath + "\\ZortosDesktop.exe", "");
            }
            else
            {
                File_Downloader("https://github.com/zortos293/ZortosToolBox/raw/main/DesktopOverlay.exe", mainpath + "\\ZortosDesktop.exe", guna2GradientButton4);
                Process.Start(mainpath + "\\ZortosDesktop.exe", "-Desktop"); // TODO
                guna2GradientButton4.Enabled = true;

            }
        }
    }
}
