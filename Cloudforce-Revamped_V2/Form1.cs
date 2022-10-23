﻿using Guna.UI2.WinForms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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

        public void DownloadGame(int JsonNumber)
        {
            WebClient client = new WebClient();
            string jsonString = File.ReadAllText("D:\\GameJSON.json");  //Need to change
            var results = JsonConvert.DeserializeObject<Root>(jsonString);
            if (!File.Exists(mainpath + "downloader.exe"))
            {
                client.DownloadFile("https://picteon.dev/files/rclone.exe", mainpath + "downloader.exe");
            }

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
                    string jsonString = System.IO.File.ReadAllText("D:\\GameJSON.json");  //Need to change
                    var results = JsonConvert.DeserializeObject<Root>(jsonString);
                    Directory.Delete(mainpath + results.Games[BtnNumber].GameOnedrive, true);

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

                        DownloadForm.TimeLBL.Invoke(new Action(() => DownloadForm.TimeLBL.Text = split[3]));

                        if (index >= 0)
                        {
                            string sub = string2.Substring(0, index);
                            string sub2 = sub.Substring(sub.IndexOf(",") + 1);
                            int.TryParse(sub2, out progress);
                        }
                        DownloadForm.Progress.Invoke(new Action(() => DownloadForm.Progress.Value = progress));
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

            Done = true;
            try
            {
                DownloadForm.Invoke(new Action(() => DownloadForm.FilenameLB.Text = string.Empty));
                DownloadForm.Invoke(new Action(() => DownloadForm.Progress.Value = 0));
                DownloadForm.Invoke(new Action(() => DownloadForm.Hide()));

            }
            catch (Exception er)
            {

                return;
            }
        }
        private void Startgame(int JsonNumber)
        {
            string jsonString = System.IO.File.ReadAllText("D:\\GameJSON.json");  //Need to change
            var results = JsonConvert.DeserializeObject<GamesRoot>(jsonString);
            if (!guna2ImageCheckBox2.Checked)
            {
                return;
            }


            Process pr = new Process();
            ProcessStartInfo prs = new ProcessStartInfo();
            prs.FileName = DriveTXTBOX.Text + results.Games[JsonNumber].GameOnedrive + results.Games[JsonNumber].GameStartLocation;
            pr.StartInfo = prs;

            ThreadStart ths = new ThreadStart(() => pr.Start());
            Thread th = new Thread(ths);
            th.Start();
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
    }
}
