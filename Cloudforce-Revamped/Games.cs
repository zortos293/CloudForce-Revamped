using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cloudforce_Revamped
{
    public partial class Games : Form
    {
        string mainpath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Cloudforce\\";

        public Games()
        {
            InitializeComponent();
            guna2HtmlLabel1.Text = "Loading";
        }

        #region Waiting GFN 
        public bool afk_timer_Done;
        Timer kick_timer = new Timer();
        int counter = 0;
        void timer1_Tick(object sender, EventArgs e)
        {
            counter++;
            if (!this.Visible == false)
            {
                guna2HtmlLabel1.ForeColor = Color.Black;
                guna2HtmlLabel1.Text = $"{120 - counter} seconds left until you can launch exes.";
            }
            if (counter == 120)  //or whatever your limit is
            {
                kick_timer.Stop();
                afk_timer_Done = true;
                guna2HtmlLabel1.ForeColor = Color.Green;
                guna2HtmlLabel1.Text = "You can now an Game.";
                counter = 0;
            }
            

        }
        bool timercheck()
        {
            if (afk_timer_Done == false)
            {
                guna2HtmlLabel1.ForeColor = Color.Red;
                guna2HtmlLabel1.Text = "You are currently in cooldown, please wait until the timer is done.";
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
        public void File_Downloader(string URL, string path, Guna.UI2.WinForms.Guna2GradientButton button)
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


        private void guna2GradientButton10_Click(object sender, EventArgs e) // Roblox
        {
            if (timercheck() == false) return;
            if (Directory.Exists($"C:\\Users\\{Environment.UserName}\\AppData\\Roaming\\Microsoft\\Windows\\Start Menu\\Programs\\Roblox"))
            {
                Process.Start($@"C:\\Users\\{Environment.UserName}\\AppData\\Roaming\\Microsoft\\Windows\\Start Menu\\Programs\\Roblox\\Roblox Player.lnk");
                wait_Timer();
            }
            else
            {
                File_Downloader("https://picteon.dev/files/Roblox.zip", mainpath + "\\Roblox.zip", guna2GradientButton10);
                ZipFile.ExtractToDirectory(mainpath + "\\Roblox.zip", mainpath + "\\");
                Process.Start(mainpath + "\\Roblox\\Versions\\version-995b3631bc754ce1\\RobloxPlayerLauncher.exe");
                guna2GradientButton10.Enabled = false;
                MessageBox.Show("Relaunch roblox after install ;)");
                wait_Timer();
            }
        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            if (timercheck() == false) return;
            if (Directory.Exists(mainpath + $"C:\\Users\\{Environment.UserName}\\AppData\\Local\\Programs\\lunarclient\\"))
            {
                Process.Start($"C:\\Users\\{Environment.UserName}\\AppData\\Local\\Programs\\lunarclient\\Lunar Client.exe");
                wait_Timer();
            }
            else
            {
                File_Downloader("https://picteon.dev/files/Lunar_Client_v2.12.8.exe", mainpath + "\\Lunar_Install.exe", guna2GradientButton10);
                Process.Start(mainpath + "\\Lunar_Install.exe");
                guna2GradientButton2.Enabled = false;
                wait_Timer();
            }
        }

        private void Games_Load(object sender, EventArgs e)
        {

        }

        private void Games_Shown(object sender, EventArgs e)
        {
            checktheme();


            kick_timer.Interval = 1000;

            kick_timer.Tick += new System.EventHandler(timer1_Tick);
            wait_Timer();
        }

        private void guna2GradientButton3_Click(object sender, EventArgs e)
        {

            if (timercheck() == false) return;
            if (File.Exists(mainpath + "\\rbxfpsunlocker.exe"))
            {
                Process.Start(mainpath + "\\rbxfpsunlocker.exe");
                wait_Timer();
            }
            else
            {
                File_Downloader("https://github.com/axstin/rbxfpsunlocker/releases/download/v4.4.2/rbxfpsunlocker-x64.zip", mainpath + "\\rbxfpsunlocker-x64.zip", guna2GradientButton3);
                ZipFile.ExtractToDirectory(mainpath + "\\rbxfpsunlocker-x64.zip", mainpath + "\\");
                Process.Start(mainpath + "\\rbxfpsunlocker.exe");
                guna2GradientButton3.Enabled = false;
                wait_Timer();
            }
        }

        private void back_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void guna2GradientButton23_Click(object sender, EventArgs e)
        {
            counter = 119;
        }
    }
}
