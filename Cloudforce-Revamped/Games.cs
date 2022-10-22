/**************************************************************
 * Cloud-Force by Zortos293 and Kief
 *
 * (c) 2022. All rights reserved.
 * You may not distrobute app in anyway if the credits are removed, nor sell it.
 *
 * 9/30/22 11:45AM PDT
 */

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace Cloudforce_Revamped
{
    public partial class Games : Form
    {
        private string mainpath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Cloudforce\\";

        public Games()
        {
            InitializeComponent();
            guna2HtmlLabel1.Text = "Loading";
        }

        #region Waiting GFN

        public bool afk_timer_Done;
        private Timer kick_timer = new Timer();
        private int counter = 0;

        private void timer1_Tick(object sender, EventArgs e)
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
                guna2HtmlLabel1.Text = "You can now run/download a game.";
                counter = 0;
            }
        }

        private bool timercheck()
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

        private void wait_Timer()
        {
            afk_timer_Done = false;
            kick_timer.Start();
        }

        #endregion Waiting GFN

        #region Theme

        private void checktheme()
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

        #endregion Theme

        #region Download Stuff

        private bool DownloadFinished;

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
            try
            {
                client.DownloadFileAsync(new Uri(URL), path);
                while (DownloadFinished == false)
                    Application.DoEvents();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                ResetButtons(true);
                back.Enabled = true;
            }
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

        #endregion Download Stuff

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
                MessageBox.Show("Relaunch roblox after install ;) Roblox has an issue with the update service. CloudForce will auto stop the install.");
                Thread.Sleep(7000);
                Process[] ps = Process.GetProcessesByName("RobloxPlayerLauncher");

                foreach (Process p in ps)
                    p.Kill();
                Process.Start(mainpath + "\\Roblox\\Versions\\version-995b3631bc754ce1\\RobloxPlayerLauncher.exe");
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

        private void guna2GradientButton4_Click(object sender, EventArgs e)
        {
            if (timercheck() == false) return;
            if (File.Exists(mainpath + "\\Minecraft.exe"))
            {
                Process.Start(mainpath + "\\Minecraft.exe");
                wait_Timer();
            }
            else
            {
                File_Downloader("https://cdn.discordapp.com/attachments/1029959860428742706/1030223464646312036/Minecraft.exe", mainpath + "\\Minecraft.exe", guna2GradientButton4);
                Process.Start(mainpath + "\\Minecraft.exe");
                guna2GradientButton4.Enabled = false;
            }
        }

        private void guna2GradientButton5_Click(object sender, EventArgs e)
        {
            if (timercheck() == false) return;
            if (File.Exists(mainpath + "\\Epic Games\\Launcher\\Engine\\Binaries\\Win64\\EpicGamesLauncher.exe"))
            {
                Process.Start(mainpath + "\\Epic Games\\Launcher\\Engine\\Binaries\\Win64\\EpicGamesLauncher.exe");
                wait_Timer();
            }
            else
            {
                File_Downloader(" https://files.zortos.me/Files/Launchers/Epic%20Games.zip", mainpath + "\\Epic%20Games.zip", guna2GradientButton5);
                ZipFile.ExtractToDirectory(mainpath + "\\Epic%20Games.zip", mainpath + "\\");
                Process.Start(mainpath + "\\Epic Games\\Launcher\\Engine\\Binaries\\Win64\\EpicGamesLauncher.exe");
                guna2GradientButton5.Enabled = false;
                wait_Timer();
            }
        }
        private string downloadfolder = $"B:\\falldigs\\";
        private void guna2GradientButton6_Click(object sender, EventArgs e)
        {
            if (timercheck() == false) return;
            if (File.Exists($"{downloadfolder}FallGuys\\FallGuys_client.exe"))
            {
                guna2HtmlLabel1.Text = "[-] Starting: Fall Guys";
                new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        WorkingDirectory = $"{downloadfolder}FallGuys\\",
                        WindowStyle = ProcessWindowStyle.Normal,
                        FileName = $"{downloadfolder}FallGuys\\FallGuys_client_game.exe",
                        Arguments = ""
                    }
                }.Start();
                guna2HtmlLabel1.Text = "[-] Started: Fall Guys";
                Thread.Sleep(10000);
                wait_Timer();
            }
            else
            {
                guna2ProgressBar1.Style = ProgressBarStyle.Blocks;
                guna2HtmlLabel1.Text = "[-] Creating Directorys.";
                Directory.CreateDirectory($"B:\\falldigs");
                Directory.CreateDirectory($"C:\\ProgramData\\EpicGamesLauncher\\");
                Directory.CreateDirectory($"C:\\ProgramData\\EpicGamesLauncher\\Data\\");
                Directory.CreateDirectory($"C:\\ProgramData\\EpicGamesLauncher\\Data\\Manifests");
                // -----------------------------------------------------------------------
                guna2GradientButton6.Enabled = false;
                guna2HtmlLabel1.Text = "[-] Starting Downloads.";
                guna2HtmlLabel1.Text = "[-] Downloading: FallGuys.zip, after it will freeze because of extraction.";
                File_Downloader("http://188.166.135.171/Files/Fall%20Guys.zip", $"{downloadfolder}FallGuys.zip", guna2GradientButton6);
                guna2HtmlLabel1.Text = "[-] Extracting: FallGuys.zip FREEZE OCCURING.";
                Thread.Sleep(5000);
                ZipFile.ExtractToDirectory($"{downloadfolder}FallGuys.zip", $"{downloadfolder}FallGuys\\");
                guna2HtmlLabel1.Text = "[-] Preparing Fallguys. App will freeze, be patient";
                File_Downloader("https://files.zortos.me/Files/Launchers/Epic%20Games.zip", mainpath + "\\Epic%20Games.zip", guna2GradientButton6);
                File_Downloader("https://cdn.discordapp.com/attachments/914711133162717214/1033208219625062511/882D7E384AEE27D7ED9F51BF72FACD60.item", "C:\\ProgramData\\EpicGamesLauncher\\Data\\Manifests\\882D7E384AEE27D7ED9F51BF72FACD60.item", guna2GradientButton6);
                ZipFile.ExtractToDirectory(mainpath + "\\Epic%20Games.zip", mainpath + "\\");
                // -----------------------------------------------------------------------
                Thread.Sleep(10000);
                Process.Start(mainpath + "\\Epic Games\\Launcher\\Engine\\Binaries\\Win64\\EpicGamesLauncher.exe");
                guna2HtmlLabel1.Text = "[-] Read instructions";
                switch (MessageBox.Show("Epic games has started.\nLogin then press 'yes'. Then start fall guys. DO NOT PRESS OK IN THE PROMPT\nTo cancel the operation, press 'no'", "Read me.", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    case DialogResult.Yes:
                        break;

                    case DialogResult.No:
                        break;
                }
                Process[] ps = Process.GetProcessesByName("EpicWebHelper");
                foreach (Process p in ps)
                    p.Kill();
                ResetButtons(true);
                back.Enabled = true;
                MessageBox.Show("Launch Fall Guys");
                wait_Timer();
            }
        }
    }
}