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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace Cloudforce_Revamped
{
    public partial class Patches : Form
    {
        public Patches()
        {
            InitializeComponent();
        }
        #region Download Stuff
        bool DownloadFinished;
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
        private void guna2GradientButton10_Click(object sender, EventArgs e)
        {
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
                guna2HtmlLabel1.Text = "[-] Downloading: data.000";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.001", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.001");
                guna2HtmlLabel1.Text = "[-] Downloading: data.001";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.002", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.002");
                guna2HtmlLabel1.Text = "[-] Downloading: data.002";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.003", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.003");
                guna2HtmlLabel1.Text = "[-] Downloading: data.003";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.004", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.004");
                guna2HtmlLabel1.Text = "[-] Downloading: data.004";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.005", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.005");
                guna2HtmlLabel1.Text = "[-] Downloading: data.005";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.006", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.006");
                guna2HtmlLabel1.Text = "[-] Downloading: data.006";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.007", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.007");
                guna2HtmlLabel1.Text = "[-] Downloading: data.007";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.008", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.008");
                guna2HtmlLabel1.Text = "[-] Downloading: data.008";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.009", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.009");
                guna2HtmlLabel1.Text = "[-] Downloading: data.009";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.010", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.010");
                guna2HtmlLabel1.Text = "[-] Downloading: data.010";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.011", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.011");
                guna2HtmlLabel1.Text = "[-] Downloading: data.011";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.012", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.012");
                guna2HtmlLabel1.Text = "[-] Downloading: data.012";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.013", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.013");
                guna2HtmlLabel1.Text = "[-] Downloading: data.013";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.014", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.014");
                guna2HtmlLabel1.Text = "[-] Downloading: data.014";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.015", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.015");
                guna2HtmlLabel1.Text = "[-] Downloading: data.015";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.016", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.016");
                guna2HtmlLabel1.Text = "[-] Downloading: data.016";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.017", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.017");
                guna2HtmlLabel1.Text = "[-] Downloading: data.017";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.018", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.018");
                guna2HtmlLabel1.Text = "[-] Downloading: data.018";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.019", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.019");
                guna2HtmlLabel1.Text = "[-] Downloading: data.019";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.020", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.020");
                guna2HtmlLabel1.Text = "[-] Downloading: data.020";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.021", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.021");
                guna2HtmlLabel1.Text = "[-] Downloading: data.021";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.022", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.022");
                guna2HtmlLabel1.Text = "[-] Downloading: data.022";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.023", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.023");
                guna2HtmlLabel1.Text = "[-] Downloading: data.023";
                File_Downloader("https://picteon.dev/files/StarCraft%20II/SC2Data/data/data.024", $"C:\\users\\{username}\\downloads\\SC2\\SC2Data\\data\\data.024");
                guna2HtmlLabel1.Text = "[-] Downloading: data.024";
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
                guna2HtmlLabel1.Text = "";
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
    }
}
