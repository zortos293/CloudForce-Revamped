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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace Cloudforce_Revamped
{
    public partial class steamgames : Form
    {
        private string mainpath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Cloudforce\\";

        public steamgames()
        {
            InitializeComponent();
            guna2HtmlLabel1.Text = "Loading";
        }

        private string downloadfolder = $"C:\\users\\{Environment.UserName}\\downloads\\";
        private string SERVER1 = "http://188.166.135.171/Files/Games/";
        private string SERVER2 = "http://20.19.208.131/Files/Games/";

        #region Waiting GFN

        public bool afk_timer_Done;
        private Timer kick_timer1 = new Timer();
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
                kick_timer1.Stop();
                afk_timer_Done = true;
                guna2HtmlLabel1.ForeColor = Color.Green;
                guna2HtmlLabel1.Text = "You can now run an app.";
                counter = 0;
            }
        }

        private bool timercheck()
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

        private void wait_Timer()
        {
            afk_timer_Done = false;
            kick_timer1.Start();
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

        public void Game_Downloader(string URL, string path)
        {
            // download file with progress bar
            DownloadFinished = false;
            WebClient client = new WebClient();
            back.Enabled = false;
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
                ResetButtons(true);
                back.Enabled = true;
            }
        }

        private void DownloadChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            guna2ProgressBar1.Value = e.ProgressPercentage;
        }

        #endregion Download Stuff

        private void Games_Load(object sender, EventArgs e)
        {
        }

        private void Games_Shown(object sender, EventArgs e)
        {
            checktheme();
        }

        private void back_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2GradientButton23_Click(object sender, EventArgs e)
        {
            counter = 119;
        }

        public string serverurl;

        private async void guna2GradientButton10_Click_1(object sender, EventArgs e)
        {
            guna2HtmlLabel1.ForeColor = Color.White;

            switch (MessageBox.Show("Server 1 (DigitalOcean) : Yes\nServer 2 (Azure) : No", "Server Select", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                case DialogResult.Yes:
                    serverurl = SERVER1;
                    break;

                case DialogResult.No:
                    serverurl = SERVER2;
                    break;
            }

            //Resident_Evil_Village  NotZortos26#6291
            if (File.Exists($"{downloadfolder}Resident_Evil_Village\\re8.exe"))
            {
                new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        WorkingDirectory = $"{downloadfolder}Resident_Evil_Village\\",
                        WindowStyle = ProcessWindowStyle.Normal,
                        FileName = $"{downloadfolder}Resident_Evil_Village\\re8.exe"
                    }
                }.Start();
            }
            else
            {
                guna2ProgressBar1.Style = ProgressBarStyle.Blocks;
                guna2HtmlLabel1.Text = "[-] Creating Directorys.";
                Directory.CreateDirectory($"{downloadfolder}Resident_Evil_Village");
                Directory.CreateDirectory($"{downloadfolder}Resident_Evil_Village\\dlc");
                // -----------------------------------------------------------------------
                guna2GradientButton10.Enabled = false;
                guna2HtmlLabel1.Text = "[-] Starting Downloads.";
                guna2HtmlLabel1.Text = "[-] Downloading: exes.zip";
                Game_Downloader(serverurl + "Resident%20Evil%20Village/exes.zip", $"{downloadfolder}Resident_Evil_Village\\exes.zip");
                guna2HtmlLabel1.Text = "[-] Extracting: exes.zip";

                ZipFile.ExtractToDirectory($"{downloadfolder}Resident_Evil_Village\\exes.zip", $"{downloadfolder}Resident_Evil_Village\\");

                guna2HtmlLabel1.Text = "[-] Downloading: re8.exe";
                Game_Downloader(serverurl + "Resident%20Evil%20Village/re8.exe", $"{downloadfolder}Resident_Evil_Village\\re8.exe");
                guna2HtmlLabel1.Text = "[-] Downloading: re_chunk_000.pak (24GB File)";
                Game_Downloader(serverurl + "Resident%20Evil%20Village/re_chunk_000.pak", $"{downloadfolder}Resident_Evil_Village\\re_chunk_000.pak");
                guna2HtmlLabel1.Text = "[-] Downloading: re_chunk_000.pak.patch_001.pak";
                Game_Downloader(serverurl + "Resident%20Evil%20Village/re_chunk_000.pak.patch_001.pak", $"{downloadfolder}Resident_Evil_Village\\re_chunk_000.pak.patch_001.pak");
                guna2HtmlLabel1.Text = "[-] Downloading: re_dlc_stm_1456361.pak";
                Game_Downloader(serverurl + "Resident%20Evil%20Village/dlc/re_dlc_stm_1456361.pak", $"{downloadfolder}Resident_Evil_Village\\dlc\\re_dlc_stm_1456361.pak");

                // -----------------------------------------------------------------------
                ResetButtons(true);
                back.Enabled = true;
                new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        WorkingDirectory = $"{downloadfolder}Resident_Evil_Village\\",
                        WindowStyle = ProcessWindowStyle.Normal,
                        FileName = $"{downloadfolder}Resident_Evil_Village\\re8.exe"
                    }
                }.Start();
                guna2HtmlLabel1.Text = "[-] Starting: Started Resident Evil Village";
            }
        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            Process[] ps = Process.GetProcessesByName("steam");

            foreach (Process p in ps)
                p.Kill();
            Process.Start(@"C:\Program Files (x86)\Steam\steam.exe");
        }
    }
}