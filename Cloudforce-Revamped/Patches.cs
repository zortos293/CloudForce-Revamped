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

namespace Cloudforce_Revamped
{
    public partial class Patches : Form
    {
        public Patches()
        {
            InitializeComponent();
        }

        #region Download Stuff

        private bool DownloadFinished;

        public void File_Downloader(string URL, string path)
        {
            // download file with progress bar
            DownloadFinished = false;
            WebClient client = new WebClient();
            back.Enabled = false;
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

        private void guna2GradientButton10_Click(object sender, EventArgs e)
        {
            guna2HtmlLabel1.ForeColor = Color.White;
            var username = Environment.UserName;
            //SC2 Kief#2583
            if (File.Exists($"C:\\users\\{username}\\downloads\\SC2\\Support64\\SC2Switcher_x64.exe"))
            {
                new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        WorkingDirectory = $"C:\\users\\{username}\\downloads\\SC2\\Support64\\",
                        WindowStyle = ProcessWindowStyle.Normal,
                        FileName = $"C:\\users\\{username}\\downloads\\SC2\\Support64\\SC2Switcher_x64.exe"
                    }
                }.Start();
            }
            else
            {
                guna2ProgressBar1.Style = ProgressBarStyle.Blocks;
                guna2HtmlLabel1.Text = "[-] Creating Directorys.";
                Directory.CreateDirectory($"C:\\users\\{username}\\downloads\\SC2");
                Directory.CreateDirectory($"C:\\users\\{username}\\downloads\\SC2\\SC2Data");
                Directory.CreateDirectory($"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data");
                Directory.CreateDirectory($"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\config");
                guna2GradientButton10.Enabled = false;
                guna2HtmlLabel1.Text = "[-] Starting Downloads.";
                Thread.Sleep(5000);
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/000000000d.idx", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\000000000d.idx");
                guna2HtmlLabel1.Text = "[-] Downloading: 010000000d";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/010000000d.idx", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\010000000d.idx");
                guna2HtmlLabel1.Text = "[-] Downloading: 020000000d";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/020000000d.idx", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\020000000d.idx");
                guna2HtmlLabel1.Text = "[-] Downloading: 030000000d";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/030000000d.idx", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\030000000d.idx");
                guna2HtmlLabel1.Text = "[-] Downloading: 040000000d";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/040000000d.idx", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\040000000d.idx");
                guna2HtmlLabel1.Text = "[-] Downloading: 050000000d";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/050000000d.idx", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\050000000d.idx");
                guna2HtmlLabel1.Text = "[-] Downloading: 060000000d";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/060000000d.idx", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\060000000d.idx");
                guna2HtmlLabel1.Text = "[-] Downloading: 060000000d";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/070000000d.idx", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\070000000d.idx");
                guna2HtmlLabel1.Text = "[-] Downloading: 070000000d";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/080000000d.idx", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\080000000d.idx");
                guna2HtmlLabel1.Text = "[-] Downloading: 080000000d";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/090000000d.idx", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\090000000d.idx");
                guna2HtmlLabel1.Text = "[-] Downloading: 090000000d";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/0a0000000d.idx", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\0a0000000d.idx");
                guna2HtmlLabel1.Text = "[-] Downloading: 0a0000000d";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/0b0000000d.idx", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\0b0000000d.idx");
                guna2HtmlLabel1.Text = "[-] Downloading: 0b0000000d";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/0c0000000d.idx", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\0c0000000d.idx");
                guna2HtmlLabel1.Text = "[-] Downloading: 0c0000000d";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/0d0000000d.idx", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\0d0000000d.idx");
                guna2HtmlLabel1.Text = "[-] Downloading: 0d0000000d";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/0e0000000d.idx", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\0e0000000d.idx");
                guna2HtmlLabel1.Text = "[-] Downloading: aa0e0000000d";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/0f0000000d.idx", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\0f0000000d.idx");
                guna2HtmlLabel1.Text = "[-] Downloading: 0f0000000d";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.000", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.000");
                guna2HtmlLabel1.Text = "[-] Downloading: data.000 (1/25)";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.001", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.001");
                guna2HtmlLabel1.Text = "[-] Downloading: data.001 (2/25)";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.002", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.002");
                guna2HtmlLabel1.Text = "[-] Downloading: data.002 (3/25)";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.003", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.003");
                guna2HtmlLabel1.Text = "[-] Downloading: data.003 (4/25)";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.004", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.004");
                guna2HtmlLabel1.Text = "[-] Downloading: data.004 (5/25)";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.005", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.005");
                guna2HtmlLabel1.Text = "[-] Downloading: data.005 (6/25)";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.006", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.006");
                guna2HtmlLabel1.Text = "[-] Downloading: data.006 (7/25)";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.007", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.007");
                guna2HtmlLabel1.Text = "[-] Downloading: data.007 (8/25)";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.008", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.008");
                guna2HtmlLabel1.Text = "[-] Downloading: data.008 (9/25)";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.009", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.009");
                guna2HtmlLabel1.Text = "[-] Downloading: data.009 (10/25)";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.010", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.010");
                guna2HtmlLabel1.Text = "[-] Downloading: data.010 (11/25)";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.011", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.011");
                guna2HtmlLabel1.Text = "[-] Downloading: data.011 (12/25)";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.012", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.012");
                guna2HtmlLabel1.Text = "[-] Downloading: data.012 (13/25)";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.013", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.013");
                guna2HtmlLabel1.Text = "[-] Downloading: data.013 (14/25)";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.014", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.014");
                guna2HtmlLabel1.Text = "[-] Downloading: data.014 (15/25)";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.015", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.015");
                guna2HtmlLabel1.Text = "[-] Downloading: data.015 (16/25)";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.016", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.016");
                guna2HtmlLabel1.Text = "[-] Downloading: data.016 (17/25)";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.017", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.017");
                guna2HtmlLabel1.Text = "[-] Downloading: data.017 (18/25)";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.018", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.018");
                guna2HtmlLabel1.Text = "[-] Downloading: data.018 (19/25)";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.019", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.019");
                guna2HtmlLabel1.Text = "[-] Downloading: data.019 (20/25)";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.020", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.020");
                guna2HtmlLabel1.Text = "[-] Downloading: data.020 (21/25)";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.021", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.021");
                guna2HtmlLabel1.Text = "[-] Downloading: data.021 (22/25)";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.022", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.022");
                guna2HtmlLabel1.Text = "[-] Downloading: data.022 (23/25)";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.023", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.023");
                guna2HtmlLabel1.Text = "[-] Downloading: data.023 (24/25)";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.024", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.024");
                guna2HtmlLabel1.Text = "[-] Downloading: data.024 (24/25)";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/shmem", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\shmem");
                guna2HtmlLabel1.Text = "[-] Downloading: shmem";
                File_Downloader("https://cdn.discordapp.com/attachments/1023015575213056050/1023044543148458064/Launcher.db", $"C:\\users\\{username}\\downloads\\SC2\\Launcher.db");
                guna2HtmlLabel1.Text = "[-] Downloading: Launcher.db";
                File_Downloader("https://cdn.discordapp.com/attachments/1023015575213056050/1023044671221538826/product.db", $"C:\\users\\{username}\\downloads\\SC2\\.product.db");
                guna2HtmlLabel1.Text = "[-] Downloading: .product.db";
                File_Downloader("https://cdn.discordapp.com/attachments/1023015575213056050/1023044878323683339/StarCraft_II.exe", $"C:\\users\\{username}\\downloads\\SC2\\StarCraft II.exe");
                guna2HtmlLabel1.Text = "[-] Downloading: StarCraft II.exe";
                File_Downloader("https://cdn.discordapp.com/attachments/1023015575213056050/1023045170884771891/patch.result", $"C:\\users\\{username}\\downloads\\SC2\\.patch.result");
                guna2HtmlLabel1.Text = "[-] Downloading: .patch.result";
                File_Downloader("https://cdn.discordapp.com/attachments/1023015575213056050/1023045257719455825/build.info", $"C:\\users\\{username}\\downloads\\SC2\\.build.info");
                guna2HtmlLabel1.Text = "[-] Downloading: .build.info";
                File_Downloader("https://op-ffa.net/Versions.zip", $"C:\\users\\{username}\\downloads\\SC2\\Versions.zip");
                guna2HtmlLabel1.Text = "[-] Downloading: Versions.zip";
                File_Downloader("https://op-ffa.net/Support.zip", $"C:\\users\\{username}\\downloads\\SC2\\Support.zip");
                guna2HtmlLabel1.Text = "[-] Downloading: Support.zip";
                File_Downloader("https://op-ffa.net/indices.zip", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\indices.zip");
                guna2HtmlLabel1.Text = "[-] Downloading: indices.zip";
                File_Downloader("https://op-ffa.net/Support64.zip", $"C:\\users\\{username}\\downloads\\SC2\\Support64.zip");
                guna2HtmlLabel1.Text = "[-] Downloading: Indices.zip";
                //TODO ASYNC EXTRACT WITHOUT UI BLOCK
                guna2ProgressBar1.Style = ProgressBarStyle.Continuous;
                System.IO.Compression.ZipFile.ExtractToDirectory($@"C:\users\{username}\downloads\SC2\Versions.zip", $@"C:\users\{username}\downloads\SC2\");
                guna2HtmlLabel1.Text = "[-] Extracting: Versions.zip";
                System.IO.Compression.ZipFile.ExtractToDirectory($@"C:\users\{username}\downloads\SC2\Support.zip", $@"C:\users\{username}\downloads\SC2\");
                guna2HtmlLabel1.Text = "[-] Extracting: Support.zip";
                System.IO.Compression.ZipFile.ExtractToDirectory($@"C:\users\{username}\downloads\SC2\Support64.zip", $@"C:\users\{username}\downloads\SC2\");
                guna2HtmlLabel1.Text = "[-] Extracting: Support64.zip";
                System.IO.Compression.ZipFile.ExtractToDirectory($@"C:\users\{username}\downloads\SC2\SC2Data\indices.zip", $@"C:\users\{username}\downloads\SC2\indices");
                guna2HtmlLabel1.Text = "[-] Extracting: indices.zip";
                guna2ProgressBar1.Style = ProgressBarStyle.Blocks;
                guna2HtmlLabel1.Text = "[-] Extracting: Waiting for 60 seconds cause of UadML";
                ResetButtons(true);
                back.Enabled = true;
                guna2HtmlLabel1.Text = "[-] Starting: Starting SC2";
                new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        WorkingDirectory = $"C:\\users\\{username}\\downloads\\SC2\\Support64\\",
                        WindowStyle = ProcessWindowStyle.Normal,
                        FileName = $"C:\\users\\{username}\\downloads\\SC2\\Support64\\SC2Switcher_x64.exe",
                    }
                }.Start();
                guna2HtmlLabel1.Text = "[-] Started: Started SC2";
            }
        }// SC2 Patcher <<

        private void Patches_Shown(object sender, EventArgs e)
        {
            checktheme();
        } // Load Theme <<

        private void back_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public string hosturl;

        private void guna2GradientButton4_Click(object sender, EventArgs e)
        {
            string SERVER1 = "http://188.166.135.171/Files/Overwatch/";
            string SERVER2 = "http://20.19.208.131/Files/Overwatch/";

            guna2HtmlLabel1.ForeColor = Color.White;
            switch (MessageBox.Show("Server 1 (DigitalOcean) : Yes\nServer 2 (Azure) : No", "Server Select", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                case DialogResult.Yes:
                    hosturl = SERVER1;
                    break;

                case DialogResult.No:
                    hosturl = SERVER2;
                    break;
            }
            var username = Environment.UserName;
            //Overwatch 2 Kief#2583 and NotZortos26#6291
            if (File.Exists($"C:\\users\\{username}\\downloads\\OverWatch\\_retail_\\Overwatch.exe"))
            {
                new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        WorkingDirectory = $"C:\\users\\{username}\\downloads\\OverWatch\\_retail_\\",
                        WindowStyle = ProcessWindowStyle.Normal,
                        FileName = $"C:\\users\\{username}\\downloads\\OverWatch\\_retail_\\Overwatch.exe"
                    }
                }.Start();
            }
            else
            {
                guna2ProgressBar1.Style = ProgressBarStyle.Blocks;
                guna2HtmlLabel1.Text = "[-] Creating Directorys.";
                Directory.CreateDirectory($"C:\\users\\{username}\\downloads\\OverWatch");
                Directory.CreateDirectory($"C:\\users\\{username}\\downloads\\OverWatch\\_retail_");
                Directory.CreateDirectory($"C:\\users\\{username}\\downloads\\OverWatch\\data");
                Directory.CreateDirectory($"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc");
                Directory.CreateDirectory($"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data");
                Directory.CreateDirectory($"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\config");
                Directory.CreateDirectory($"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\indices");

                guna2GradientButton4.Enabled = false;
                guna2HtmlLabel1.Text = "[-] Starting Downloads.";
                Thread.Sleep(5000);
                guna2HtmlLabel1.Text = "[-] Downloading: Data Files";
                File_Downloader(hosturl + "data/casc/data/0000000011.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0000000011.idx");
                File_Downloader(hosturl + "data/casc/data/0000000012.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0000000012.idx");
                File_Downloader(hosturl + "data/casc/data/0000000014.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0000000014.idx");
                File_Downloader(hosturl + "data/casc/data/0000000015.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0000000015.idx");
                File_Downloader(hosturl + "data/casc/data/0000000016.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0000000016.idx");
                File_Downloader(hosturl + "data/casc/data/0000000017.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0000000017.idx");

                File_Downloader(hosturl + "data/casc/data/0100000013.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0100000013.idx");
                File_Downloader(hosturl + "data/casc/data/0100000014.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0100000014.idx");
                File_Downloader(hosturl + "data/casc/data/0100000015.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0100000015.idx");

                File_Downloader(hosturl + "data/casc/data/0200000015.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0200000015.idx");
                File_Downloader(hosturl + "data/casc/data/0200000016.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0200000016.idx");
                File_Downloader(hosturl + "data/casc/data/0200000017.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0200000017.idx");

                File_Downloader(hosturl + "data/casc/data/0300000014.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0300000014.idx");
                File_Downloader(hosturl + "data/casc/data/0300000015.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0300000015.idx");
                File_Downloader(hosturl + "data/casc/data/0300000016.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0300000016.idx");

                File_Downloader(hosturl + "data/casc/data/0400000011.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0400000011.idx");
                File_Downloader(hosturl + "data/casc/data/0400000012.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0400000012.idx");
                File_Downloader(hosturl + "data/casc/data/0400000013.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0400000013.idx");

                File_Downloader(hosturl + "data/casc/data/0500000013.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0500000013.idx");
                File_Downloader(hosturl + "data/casc/data/0500000014.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0500000014.idx");
                File_Downloader(hosturl + "data/casc/data/0500000015.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0500000015.idx");

                File_Downloader(hosturl + "data/casc/data/0600000014.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0600000014.idx");
                File_Downloader(hosturl + "data/casc/data/0600000015.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0600000015.idx");
                File_Downloader(hosturl + "data/casc/data/0600000016.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0600000016.idx");

                File_Downloader(hosturl + "data/casc/data/0700000014.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0700000014.idx");
                File_Downloader(hosturl + "data/casc/data/0700000015.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0700000015.idx");
                File_Downloader(hosturl + "data/casc/data/0700000016.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0700000016.idx");

                File_Downloader(hosturl + "data/casc/data/0800000013.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0800000013.idx");
                File_Downloader(hosturl + "data/casc/data/0800000014.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0800000014.idx");
                File_Downloader(hosturl + "data/casc/data/0800000015.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0800000015.idx");

                File_Downloader(hosturl + "data/casc/data/0900000013.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0900000013.idx");
                File_Downloader(hosturl + "data/casc/data/0900000014.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0900000014.idx");
                File_Downloader(hosturl + "data/casc/data/0900000015.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0900000015.idx");

                File_Downloader(hosturl + "data/casc/data/0a00000011.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0a00000011.idx");
                File_Downloader(hosturl + "data/casc/data/0a00000012.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0a00000012.idx");
                File_Downloader(hosturl + "data/casc/data/0a00000013.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0a00000013.idx");

                File_Downloader(hosturl + "data/casc/data/0b00000015.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0b00000015.idx");
                File_Downloader(hosturl + "data/casc/data/0b00000016.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0b00000016.idx");
                File_Downloader(hosturl + "data/casc/data/0b00000017.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0b00000017.idx");

                File_Downloader(hosturl + "data/casc/data/0c00000014.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0c00000014.idx");
                File_Downloader(hosturl + "data/casc/data/0c00000015.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0c00000015.idx");
                File_Downloader(hosturl + "data/casc/data/0c00000016.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0c00000016.idx");

                File_Downloader(hosturl + "data/casc/data/0d00000012.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0d00000012.idx");
                File_Downloader(hosturl + "data/casc/data/0d00000013.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0d00000013.idx");
                File_Downloader(hosturl + "data/casc/data/0d00000014.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0d00000014.idx");

                File_Downloader(hosturl + "data/casc/data/0e00000012.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0e00000012.idx");
                File_Downloader(hosturl + "data/casc/data/0e00000013.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0e00000013.idx");
                File_Downloader(hosturl + "data/casc/data/0e00000014.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0e00000014.idx");

                File_Downloader(hosturl + "data/casc/data/0f00000013.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0f00000013.idx");
                File_Downloader(hosturl + "data/casc/data/0f00000014.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0f00000014.idx");
                File_Downloader(hosturl + "data/casc/data/0f00000015.idx", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\0f00000015.idx");

                guna2HtmlLabel1.Text = "[-] Downloading: data.000";
                File_Downloader(hosturl + "data/casc/data/data.000", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\data.000");
                guna2HtmlLabel1.Text = "[-] Downloading: data.001";
                File_Downloader(hosturl + "data/casc/data/data.001", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\data.001");
                guna2HtmlLabel1.Text = "[-] Downloading: data.002";
                File_Downloader(hosturl + "data/casc/data/data.002", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\data.002");
                guna2HtmlLabel1.Text = "[-] Downloading: data.003";
                File_Downloader(hosturl + "data/casc/data/data.003", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\data.003");
                guna2HtmlLabel1.Text = "[-] Downloading: data.004";
                File_Downloader(hosturl + "data/casc/data/data.004", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\data.004");
                guna2HtmlLabel1.Text = "[-] Downloading: data.005";
                File_Downloader(hosturl + "data/casc/data/data.005", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\data.005");
                guna2HtmlLabel1.Text = "[-] Downloading: data.006";
                File_Downloader(hosturl + "data/casc/data/data.006", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\data.006");
                guna2HtmlLabel1.Text = "[-] Downloading: data.007";
                File_Downloader(hosturl + "data/casc/data/data.007", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\data.007");
                guna2HtmlLabel1.Text = "[-] Downloading: data.008";
                File_Downloader(hosturl + "data/casc/data/data.008", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\data.008");
                guna2HtmlLabel1.Text = "[-] Downloading: data.009";
                File_Downloader(hosturl + "data/casc/data/data.009", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\data.009");
                guna2HtmlLabel1.Text = "[-] Downloading: data.010";
                File_Downloader(hosturl + "data/casc/data/data.010", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\data.010");
                guna2HtmlLabel1.Text = "[-] Downloading: data.011";
                File_Downloader(hosturl + "data/casc/data/data.011", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\data.011");
                guna2HtmlLabel1.Text = "[-] Downloading: data.012";
                File_Downloader(hosturl + "data/casc/data/data.012", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\data.012");
                guna2HtmlLabel1.Text = "[-] Downloading: data.013";
                File_Downloader(hosturl + "data/casc/data/data.013", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\data.013");
                guna2HtmlLabel1.Text = "[-] Downloading: data.014";
                File_Downloader(hosturl + "data/casc/data/data.014", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\data.014");
                guna2HtmlLabel1.Text = "[-] Downloading: data.015";
                File_Downloader(hosturl + "data/casc/data/data.015", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\data.015");
                guna2HtmlLabel1.Text = "[-] Downloading: data.016";
                File_Downloader(hosturl + "data/casc/data/data.016", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\data.016");
                guna2HtmlLabel1.Text = "[-] Downloading: data.017";
                File_Downloader(hosturl + "data/casc/data/data.017", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\data.017");
                guna2HtmlLabel1.Text = "[-] Downloading: data.018";
                File_Downloader(hosturl + "data/casc/data/data.018", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\data.018");
                guna2HtmlLabel1.Text = "[-] Downloading: data.019";
                File_Downloader(hosturl + "data/casc/data/data.019", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\data.019");
                guna2HtmlLabel1.Text = "[-] Downloading: data.020";
                File_Downloader(hosturl + "data/casc/data/data.020", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\data.020");
                guna2HtmlLabel1.Text = "[-] Downloading: data.021";
                File_Downloader(hosturl + "data/casc/data/data.021", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\data.021");
                guna2HtmlLabel1.Text = "[-] Downloading: data.022";
                File_Downloader(hosturl + "data/casc/data/data.022", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\data.022");
                guna2HtmlLabel1.Text = "[-] Downloading: data.023";
                File_Downloader(hosturl + "data/casc/data/shmem", $"C:\\users\\{username}\\downloads\\OverWatch\\data\\casc\\data\\shmem");

                guna2HtmlLabel1.Text = "[-] Downloading and Extracting: indices Files (i will freeze be patient)";
                File_Downloader(hosturl + "indices.zip", $"C:\\users\\{username}\\downloads\\OverWatch\\indices.zip");
                ZipFile.ExtractToDirectory($@"C:\users\{username}\downloads\OverWatch\indices.zip", $@"C:\users\{username}\downloads\OverWatch\data\casc\indices\");
                guna2HtmlLabel1.Text = "[-] Downloading and Extracting: Exe and dll files (i will freeze be patient)";
                File_Downloader(hosturl + "retail.zip", $"C:\\users\\{username}\\downloads\\OverWatch\\retail.zip");
                ZipFile.ExtractToDirectory($@"C:\users\{username}\downloads\OverWatch\retail.zip", $@"C:\users\{username}\downloads\OverWatch\_retail_");
                File_Downloader(hosturl + ".product.db", $"C:\\users\\{username}\\downloads\\OverWatch\\.product.db");
                File_Downloader(hosturl + "Launcher.db", $"C:\\users\\{username}\\downloads\\OverWatch\\Launcher.db");
                File_Downloader(hosturl + ".patch.result", $"C:\\users\\{username}\\downloads\\OverWatch\\.patch.result");
                File_Downloader(hosturl + ".build.info", $"C:\\users\\{username}\\downloads\\OverWatch\\.build.info");
                File_Downloader(hosturl + "Overwatch%20Launcher.exe", $"C:\\users\\{username}\\downloads\\OverWatch\\Overwatch Launcher.exe");
                ResetButtons(true);
                back.Enabled = true;
                guna2HtmlLabel1.Text = "[-] Starting: Starting Overwatch 2";
                new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        WorkingDirectory = $"C:\\users\\{username}\\downloads\\OverWatch\\_retail_\\",
                        WindowStyle = ProcessWindowStyle.Normal,
                        FileName = $"C:\\users\\{username}\\downloads\\OverWatch\\_retail_\\Overwatch.exe"
                    }
                }.Start();
                guna2HtmlLabel1.Text = "[-] Started: Started Overwatch 2 (Takes 1min to launch)";
            }
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
        }
    }
}