/**************************************************************
 * Cloud-Force by Zortos293 and Kief
 * 
 * (c) 2022. All rights reserved.
 * You may not distrobute app in anyway if the credits are removed, nor sell it.
 * 
 * 9/30/22 11:45AM PDT
 */
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Cloudforce_Revamped
{
    public partial class Utility : Form
    {
        string mainpath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Cloudforce\\";
        public Utility()
        {
            InitializeComponent();

            //DownloadPath = Environment.SpecialFolder.ApplicationData + "\\Cloudforce\\";


        }
        
        #region Waiting GFN 
        public bool afk_timer_Done; 
        Timer kick_timer = new Timer();
        int counter = 0;
        void timer1_Tick(object sender, EventArgs e)
        {
            counter++;
            guna2HtmlLabel1.ForeColor = Color.Black;
            guna2HtmlLabel1.Text = $"{120 - counter} seconds left until you can launch exes.";
            if (counter == 120)  //or whatever your limit is
            {
                kick_timer.Stop();
                afk_timer_Done = true;
                guna2HtmlLabel1.ForeColor = Color.Green;
                guna2HtmlLabel1.Text = "You can now an app.";
                counter = 0;
            }
            
        }
        bool timercheck()
        {
            if (afk_timer_Done == false)
            {
                guna2HtmlLabel1.ForeColor = Color.Red;
                guna2HtmlLabel1.Text = "You are currently in Cooldown, please wait until the timer is done.";
                return false;
            }
            else
            {
                return true;
            }
        }
        void wait_Timer()
        {
            afk_timer_Done = false;
            kick_timer.Start();
        }
        #endregion

        #region Theme
        void checktheme()
        {
            if (Main.Dark == true)
            {
                this.BackColor = Color.FromArgb(64, 64, 64);
                changecolorBTN(123, 0, 238, 170, 0, 255);
                guna2GradientPanel1.FillColor = Color.FromArgb(97, 67, 133);
                guna2GradientPanel1.FillColor2 = Color.FromArgb(81, 99, 149);
            }
            if (Main.Light == true)
            {
                this.BackColor = Color.WhiteSmoke;
                changecolorBTN(255, 128, 128, 255, 128, 255);
                guna2GradientPanel1.FillColor = Color.FromArgb(255, 192, 255);
                guna2GradientPanel1.FillColor2 = Color.FromArgb(255, 192, 192);
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

        private void guna2GradientButton12_Click(object sender, EventArgs e) // Process Hacker
        {
            if (timercheck() == false) return;
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
                wait_Timer();
            }
        }

        private void back_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2GradientButton11_Click(object sender, EventArgs e) // Discord
        {
            if (timercheck() == false) return;
            if (Directory.Exists(mainpath + "\\Discord\\"))
            {
                Process.Start(mainpath + "\\Discord.zip");
                wait_Timer();
            }
            else
            {
                File_Downloader("https://picteon.dev/files/Discord.zip", mainpath + "\\Discord.zip", guna2GradientButton11);
                    
                ZipFile.ExtractToDirectory(mainpath + "\\Discord.zip", mainpath + "\\");
                Process.Start(mainpath + "\\discord2\\discord-portable.exe"); 
                guna2GradientButton11.Enabled = false;
                wait_Timer();
            }
        }

        private void Utility_Shown(object sender, EventArgs e)
        {
            checktheme();
            kick_timer.Interval = 1000;

            kick_timer.Tick += new System.EventHandler(timer1_Tick);
            kick_timer.Start();
        }

        private void guna2GradientButton14_Click(object sender, EventArgs e) // Firefox
        {
            if (timercheck() == false) return;
            if (Directory.Exists(mainpath + "\\Firefox\\"))
            {
                Process.Start(mainpath + "\\Firefox\\runthis.exe");
                wait_Timer();
            }
            else
            {
                File_Downloader("https://picteon.dev/files/Firefox.zip", mainpath + "\\Firefox.zip", guna2GradientButton14);

                ZipFile.ExtractToDirectory(mainpath + "\\Firefox.zip", mainpath + "\\");
                Process.Start(mainpath + "\\Firefox\\runthis.exe"); 
                guna2GradientButton14.Enabled = false;
                wait_Timer();
            }
        }

        private void guna2GradientButton4_Click(object sender, EventArgs e) // Explorer ++ 
        {
            if (timercheck() == false) return;
            if (File.Exists(mainpath + "\\DoraTheExplorer.exe"))
            {
                Process.Start(mainpath + "\\DoraTheExplorer.exe");
            }
            else
            {
                File_Downloader("https://picteon.dev/files/Explorer++.exe", mainpath + "\\DoraTheExplorer.exe", guna2GradientButton4);


                Process.Start(mainpath + "\\DoraTheExplorer.exe"); // TODO
                guna2GradientButton4.Enabled = true;
                wait_Timer();
            }
        }

        private void Utility_Load(object sender, EventArgs e)
        {

        }

        private void guna2GradientButton15_Click(object sender, EventArgs e) // LibreWolf
        {
            if (timercheck() == false) return;
            if (Directory.Exists(mainpath + "\\librewolf-105.0.1\\"))
            {
                Process.Start(mainpath + "\\librewolf-105.0.1\\LibreWolf-Portable.exe");
                wait_Timer();
            }
            else
            {
                File_Downloader("https://gitlab.com/librewolf-community/browser/windows/uploads/a9d86b83d8e66b9c3c75a0c2221aecdd/librewolf-105.0.1-1.en-US.win64-portable.zip", mainpath + "\\librewolf.zip", guna2GradientButton15);

                ZipFile.ExtractToDirectory(mainpath + "\\librewolf.zip", mainpath + "\\");
                Process.Start(mainpath + "\\librewolf-105.0.1\\LibreWolf-Portable.exe");
                guna2GradientButton15.Enabled = false;
                wait_Timer();
            }
        }

        private void guna2GradientButton10_Click(object sender, EventArgs e) // Desktop
        {
            if (timercheck() == false) return;
            if (File.Exists(mainpath + "\\ZortosDesktop.exe"))
            {
                Process.Start(mainpath + "\\ZortosDesktop.exe", "");
                wait_Timer();
            }
            else
            {
                File_Downloader("https://github.com/zortos293/ZortosToolBox/raw/main/DesktopOverlay.exe", mainpath + "\\ZortosDesktop.exe", guna2GradientButton10);
                Process.Start(mainpath + "\\ZortosDesktop.exe", "-Desktop"); // TODO
                guna2GradientButton10.Enabled = true;
                wait_Timer();
            }
        }

        private void guna2GradientButton13_Click(object sender, EventArgs e) // Anydesk
        {
            if (timercheck() == false) return;
            if (File.Exists(mainpath + "\\AnyPwet.exe"))
            {
                Process.Start(mainpath + "\\AnyPwet.exe");
                wait_Timer();
            }
            else
            {
                File_Downloader("https://picteon.dev/files/AnyDesk.exe", mainpath + "\\AnyPwet.exe", guna2GradientButton13);
                Process.Start(mainpath + "\\AnyPwet.exe"); 
                guna2GradientButton13.Enabled = true;
                wait_Timer();
            }
        }

        private void guna2GradientButton16_Click(object sender, EventArgs e) // Spotify
        {
            if (timercheck() == false) return;
            if (Directory.Exists(mainpath + "\\Spotify\\"))
            {
                Process.Start(mainpath + "\\Spotify\\Spotify.exe");
                wait_Timer();
            }
            else
            {
                File_Downloader("https://picteon.dev/files/Spotify.zip", mainpath + "\\Spotify.zip", guna2GradientButton16);
                ZipFile.ExtractToDirectory(mainpath + "\\Spotify.zip", mainpath + "\\");
                Process.Start(mainpath + "\\Spotify\\Spotify.exe");
                guna2GradientButton16.Enabled = false;
                wait_Timer();
            }
        }
        private void guna2GradientButton17_Click(object sender, EventArgs e) // Command Prompt
        {
            if (timercheck() == false) return;
            if (Directory.Exists(mainpath + "\\cmdpwet.exe"))
            {
                Process.Start(mainpath + "\\cmdpwetaa.exe");
                wait_Timer();
            }
            else
            {
                File_Downloader("https://picteon.dev/files/NotCMDNvidia.exe", mainpath + "\\cmdpwetaa.exe", guna2GradientButton17);
                Process.Start(mainpath + "\\cmdpwetaa.exe");
                guna2GradientButton17.Enabled = false;
                wait_Timer();
            }
        }

        private void guna2GradientButton22_Click(object sender, EventArgs e)
        {
            // warning about that developer is not responsible for any damage
            MessageBox.Show("Warning: This is a ILLEGAL App, the developer is not responsible for any damage caused by this App.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (timercheck() == false) return;
            if (Directory.Exists(mainpath + "\\SilverBullet.v1.1.2\\"))
            {
                new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        WorkingDirectory = mainpath + "\\SilverBullet.v1.1.2\\",
                        WindowStyle = ProcessWindowStyle.Normal,
                        FileName = mainpath + "\\SilverBullet.v1.1.2\\SilverBullet.exe"
                    }
                }.Start();
                
                wait_Timer();
            }
            else
            {
                File_Downloader("https://github.com/zortos293/ZortosCDN/releases/download/Mirror/SilverBullet.v1.1.2.zip", mainpath + "\\SilverBullet.zip", guna2GradientButton22);
                ZipFile.ExtractToDirectory(mainpath + "\\SilverBullet.zip", mainpath + "\\");
                new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        WorkingDirectory = mainpath + "\\SilverBullet.v1.1.2\\",
                        WindowStyle = ProcessWindowStyle.Normal,
                        FileName = mainpath + "\\SilverBullet.v1.1.2\\SilverBullet.exe"
                    }
                }.Start();
                guna2GradientButton22.Enabled = false;
                wait_Timer();
            }
        }

        private void guna2GradientButton23_Click(object sender, EventArgs e)
        {
            counter = 119;
        }
    }
}
