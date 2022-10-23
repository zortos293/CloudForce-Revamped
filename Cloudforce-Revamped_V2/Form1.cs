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

        public void DownloadGame(int JsonNumber) // Download Game Trough Onedrive <<<<
        {
            Done = false;
            WebClient client = new WebClient();
            string jsonString = File.ReadAllText("C:\\Users\\Zortos\\Downloads\\test.json");  //Need to change
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
            try
            {
                if (ForceExit)
                {
                    Process[] ps = Process.GetProcessesByName("downloader");

                    foreach (Process p in ps)
                        p.Kill();
                    string jsonString = System.IO.File.ReadAllText("C:\\Users\\Zortos\\Downloads\\test.json");  //Need to change
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

                        this.guna2ProgressBar2.Invoke(new Action(() => this.guna2HtmlLabel4.Text = split[3]));

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
        private void Startgame(int JsonNumber)
        {
            string jsonString = File.ReadAllText("C:\\Users\\Zortos\\Downloads\\test.json");  //Need to change
            var results = JsonConvert.DeserializeObject<Root>(jsonString);
            Process.Start(mainpath + results.Game[JsonNumber].AppExe);
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
            Startgame(0); // Overwatche
        }
    }
}
