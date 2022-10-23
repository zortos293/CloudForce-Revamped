using Guna.UI2.WinForms;
using Newtonsoft.Json;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cloudforce_Revamped_V2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private string mainpath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Cloudforce\\";
        public int BtnNumber;
        public static bool ForceExit = false;
        public static bool Done = false;

        public class Game
        {
            public string GameOnedrive { get; set; }
            public string AppExe { get; set; }
        }

        public class Root
        {
            public List<Game> Game { get; set; }
        }
        private bool DownloadFinished;
        public void File_Downloader(string URL, string path, Guna.UI2.WinForms.Guna2Button button)
        {
            // download file with progress bar
            DownloadFinished = false;
            WebClient client = new WebClient();
            button.Enabled = false;
            guna2ProgressBar1.Value = 0;
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(FileDownloadComplete);
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadChanged);
            client.DownloadFileAsync(new Uri(URL), path);
            while (DownloadFinished == false)
                Application.DoEvents();
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

        public void DownloadGame(int JsonNumber) // Download Game Trough Onedrive <<<<
        {
            var username = Environment.UserName;
            Done = false;
            WebClient client = new WebClient();
            string jsonString = File.ReadAllText($"C:\\Users\\{username}\\Downloads\\test.json");  //Need to change
            var results = JsonConvert.DeserializeObject<Root>(jsonString);
            if (!File.Exists(mainpath + "downloader.exe"))
            {
                client.DownloadFile("https://picteon.dev/files/rclone.exe", mainpath + "downloader.exe");
            }
            guna2ProgressBar2.Visible = true;
            guna2HtmlLabel4.Visible = true;
            Process process = new Process();
            process.OutputDataReceived += new DataReceivedEventHandler(process_OutputDataReceived);
            process.StartInfo.FileName = mainpath + "downloader.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.Arguments = "copy -P " + "zortosdrive:\\" + results.Game[JsonNumber].GameOnedrive + " " + mainpath + results.Game[JsonNumber].GameOnedrive ;
            process.Exited += new EventHandler(p_Exited);
            process.EnableRaisingEvents = true;
            process.Start();
            process.BeginOutputReadLine();
        }

        void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            var username = Environment.UserName;
            try
            {
                if (ForceExit)
                {
                    Process[] ps = Process.GetProcessesByName("downloader");

                    foreach (Process p in ps)
                        p.Kill();
                    string jsonString = System.IO.File.ReadAllText($"C:\\Users\\{username}\\Downloads\\test.json");  //Need to change
                    var results = JsonConvert.DeserializeObject<Root>(jsonString);
                    Directory.Delete(mainpath + results.Game[BtnNumber].GameOnedrive, true);

                }

                if (Process.GetProcessesByName("downloader").Length > 0)
                {
                    if (e.Data.Contains("ETA"))
                    {
                        string string1 = e.Data;
                        string string2 = string1.Substring(string1.IndexOf("Transferred:"));
                        int progress = 0;
                        int index = string2.IndexOf('%');
                        string[] split = string2.Split(',');

                        this.guna2ProgressBar2.Invoke(new Action(() => this.guna2HtmlLabel4.Text = split[2] +  " " +split[3]));

                        if (index >= 0)
                        {
                            string sub = string2.Substring(0, index);
                            string sub2 = sub.Substring(sub.IndexOf(",") + 1);
                            int.TryParse(sub2, out progress);
                        }
                        this.guna2ProgressBar2.Invoke(new Action(() => this.guna2ProgressBar2.Value = progress));
                    }


                }
            }
            catch (Exception er)
            {
                MessageBox.Show("An Error occoured\n" + er.Message + "\nPress ok to Continue");

                return;
            }
        }

        void p_Exited(object sender, EventArgs e)
        {
            guna2ProgressBar2.Visible = false;
            guna2HtmlLabel4.Visible = false;
            Done = true;
            try
            {
                this.Invoke(new Action(() => guna2HtmlLabel4.Text = string.Empty));
                this.Invoke(new Action(() => guna2ProgressBar2.Value = 0));
                this.Invoke(new Action(() => guna2ProgressBar2.Hide()));
                

            }
            catch (Exception er)
            {

                return;
            }
        }
        private bool Startgame(int JsonNumber)
        {
            var username = Environment.UserName;
            string jsonString = File.ReadAllText($"C:\\Users\\{username}\\Downloads\\test.json");  //Need to change
            var results = JsonConvert.DeserializeObject<Root>(jsonString);
            if (File.Exists(mainpath + results.Game[JsonNumber].AppExe))
            {
                Process.Start(mainpath + results.Game[JsonNumber].AppExe);
                return true;
            }
            else
            {
                return false;
            }
            
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Epic_Games_Click(object sender, EventArgs e)
        {

        }

        private void Games_Click(object sender, EventArgs e)
        {

        }

        private async void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            Startgame(0);
            DownloadGame(0); // Overwatch
            while (Done == false)
            {
                Application.DoEvents();
            }
            guna2HtmlLabel4.Text = "[/] Extracting Data 01 - 09 (Please Be patient)";
            await Task.Run(() =>
            {
                ZipFile.ExtractToDirectory(mainpath + "overwatch\\data\\casc\\data\\data01-09.zip", mainpath + "overwatch\\data\\casc\\data\\");
            });
            File.Delete(mainpath + "overwatch\\data\\casc\\data\\data01-09.zip");
            guna2HtmlLabel4.Text = "[/] Extracting Data 10 - 13 (Please Be patient)";
            await Task.Run(() =>
            {
                ZipFile.ExtractToDirectory(mainpath + "overwatch\\data\\casc\\data\\data910111213.zip", mainpath + "overwatch\\data\\casc\\data\\");
            });
            File.Delete(mainpath + "overwatch\\data\\casc\\data\\data910111213.zip");
            guna2HtmlLabel4.Text = "[/] Extracting Data 14 - 17 (Please Be patient)";
            await Task.Run(() =>
            {
                ZipFile.ExtractToDirectory(mainpath + "overwatch\\data\\casc\\data\\data14151617.zip", mainpath + "overwatch\\data\\casc\\data\\");
            });
            File.Delete(mainpath + "overwatch\\data\\casc\\data\\data14151617.zip");
            guna2HtmlLabel4.Text = "[/] Extracting Data 18 - 20 (Please Be patient)";
            await Task.Run(() =>
            {
                ZipFile.ExtractToDirectory(mainpath + "overwatch\\data\\casc\\data\\data181920.zip", mainpath + "overwatch\\data\\casc\\data\\");
            });
            File.Delete(mainpath + "overwatch\\data\\casc\\data\\data181920.zip");
            Startgame(0); // Overwatch
        }

        private void guna2Button15_Click(object sender, EventArgs e)
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
                        File_Downloader(download_link, mainpath + $"\\{exe_name}.zip", guna2Button15);
                        guna2HtmlLabel1.Text = $"Downloaded: C:\\users\\{username}\\AppData\\Roaming\\Cloudforce\\{exe_name}.zip";
                        guna2TextBox3.Text = $"C:\\users\\{username}\\AppData\\Roaming\\Cloudforce\\{exe_name}.zip";
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
                        File_Downloader(download_link, mainpath + $"\\{exe_name}.exe", guna2Button15);
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
    }
}
