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
using Timer = System.Windows.Forms.Timer;

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
        public bool afk_timer_Done;
        private Timer kick_timer1 = new Timer();
        private int counter = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            counter++;
            if (!this.Visible == false)
            {
                guna2HtmlLabel3.ForeColor = Color.White;
                guna2HtmlLabel3.Text = $"{120 - counter} seconds left until you can launch exes.";
            }
            if (counter == 120)  //or whatever your limit is
            {
                kick_timer1.Stop();
                afk_timer_Done = true;
                guna2HtmlLabel3.ForeColor = Color.White;
                guna2HtmlLabel3.Text = "You can now run an app.";
                counter = 0;
            }
        }

        private bool timercheck()
        {
            if (afk_timer_Done == false)
            {
                guna2HtmlLabel3.ForeColor = Color.MediumVioletRed;
                guna2HtmlLabel3.Text = "You are currently in Cooldown, please wait until the timer is done.";
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
                Directory.CreateDirectory($"C:\\Users\\{username}\\.config\\");
                Directory.CreateDirectory($"C:\\Users\\{username}\\.config\\rclone\\");
                client.DownloadFile("https://raw.githubusercontent.com/zortos293/ZortosCDN/master/rclone.conf", $"C:\\Users\\{username}\\.config\\rclone\\rclone.conf");
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
            Done = true;
            try
            {
                this.Invoke(new Action(() => guna2ProgressBar2.Visible = false));
                this.Invoke(new Action(() => guna2HtmlLabel4.Visible = false));
                this.Invoke(new Action(() => guna2HtmlLabel4.Text = string.Empty));
                this.Invoke(new Action(() => guna2ProgressBar2.Value = 0));
                this.Invoke(new Action(() => guna2ProgressBar2.Hide()));
                

            }
            catch (Exception er)
            {

                return;
            }
        }
        public void Alert(string msg, Form_Alert.enmType type)
        {
            Form_Alert frm = new Form_Alert();
            frm.showAlert(msg, type);
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
            this.Alert("Downloading overwatch, please wait.", Form_Alert.enmType.Info);
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
                guna2HtmlLabel8.Text = "Please put a valid download link in the input field.";
                guna2HtmlLabel8.ForeColor = Color.Red;
                guna2TextBox3.PlaceholderForeColor = Color.Red;
                guna2TextBox3.PlaceholderText = "Invalid link.";
                guna2TextBox3.Text = "";
                guna2HtmlLabel7.Text = "Error | Ready for next operation.";
                this.Alert("Invalid Download Link", Form_Alert.enmType.Error);
            }
            else
            {
                Boolean dresult = download_link.Contains(".zip");
                if (dresult == true)
                {
                    try
                    {
                        File_Downloader(download_link, mainpath + $"\\{exe_name}.zip", guna2Button15);
                        guna2HtmlLabel8.Text = $"Downloaded: C:\\users\\{username}\\AppData\\Roaming\\Cloudforce\\{exe_name}.zip";
                        guna2TextBox3.Text = $"C:\\users\\{username}\\AppData\\Roaming\\Cloudforce\\{exe_name}.zip";
                        guna2HtmlLabel7.Text = "Status: Ready.";
                    }
                    catch
                    {
                        guna2HtmlLabel8.Text = "An error occured while trying to download.";
                        guna2TextBox3.Text = "";
                        guna2HtmlLabel8.ForeColor = Color.Red;
                        guna2TextBox3.ForeColor = Color.Red;
                        guna2TextBox3.PlaceholderText = "Invalid link.";
                        guna2TextBox3.Text = "";
                        guna2HtmlLabel7.Text = "Error | Ready for next operation.";
                        this.Alert("Error while downloading", Form_Alert.enmType.Error);
                    }
                }
                else
                {
                    try
                    {
                        File_Downloader(download_link, mainpath + $"\\{exe_name}.exe", guna2Button15);
                        guna2HtmlLabel8.Text = $"Downloaded: {mainpath}\\{exe_name}.exe";
                        guna2TextBox2.Text = $"C:\\users\\{username}\\AppData\\Roaming\\Cloudforce\\{exe_name}.exe";
                        guna2HtmlLabel7.Text = "Status: Ready.";
                    }
                    catch
                    {
                        guna2HtmlLabel8.Text = "An error occured while trying to download.";
                        guna2TextBox3.Text = "";
                        guna2HtmlLabel8.ForeColor = Color.Red;
                        guna2TextBox3.PlaceholderForeColor = Color.Red;
                        guna2TextBox3.PlaceholderText = "Invalid link.";
                        guna2TextBox3.Text = "";
                        guna2HtmlLabel7.Text = "Error | Ready for next operation.";
                        this.Alert("Error while downloading", Form_Alert.enmType.Error);
                    }
                }
                guna2TextBox3.PlaceholderText = "Link here";
                guna2TextBox3.Text = "";
            }
        }

        private async void guna2Button16_Click(object sender, EventArgs e)
        {
            var username = Environment.UserName;
            var extract_path = guna2TextBox1.Text;
            if (extract_path == "" || extract_path == " " || File.Exists(extract_path) == false || extract_path.Contains(".zip") == false)
            {
                guna2HtmlLabel8.Text = "Please put a valid path in the input field.";
                guna2HtmlLabel8.ForeColor = Color.Red;
                guna2TextBox1.PlaceholderForeColor = Color.Red;
                guna2TextBox1.PlaceholderText = "Invalid path.";
                guna2TextBox1.Text = "";
                guna2HtmlLabel7.Text = "Error | Ready for next operation.";
                this.Alert("Invalid Path", Form_Alert.enmType.Error);
            }
            else
            {
                try
                {
                    await Task.Run(() =>
                    {
                        ZipFile.ExtractToDirectory($@"{extract_path}", $@"C:\users\{username}\AppData\Roaming\Cloudforce");
                    });
                    guna2HtmlLabel8.Text = $"[-] Extracted: {extract_path}";
                    guna2TextBox2.Text = $"C:\\users\\{username}\\AppData\\Roaming\\Cloudforce\\";
                    guna2HtmlLabel7.Text = "Status: Ready.";
                    guna2TextBox1.Text = "";
                }
                catch
                {
                    guna2HtmlLabel8.Text = "Failed to extract zip.";
                    guna2HtmlLabel8.ForeColor = Color.Red;
                    guna2TextBox1.PlaceholderForeColor = Color.Red;
                    guna2TextBox1.PlaceholderText = "Failed to extract";
                    guna2TextBox1.Text = "";
                    guna2HtmlLabel7.Text = "Error | Ready for next operation.";
                    this.Alert("Extracting Failed.", Form_Alert.enmType.Error);
                }
            }
        }

        private void guna2Button17_Click(object sender, EventArgs e)
        {
            var run_path = guna2TextBox2.Text;
            if (run_path == "" || run_path == " " || File.Exists(run_path) == false || run_path.Contains(".exe") == false)
            {
                guna2HtmlLabel8.Text = "Please put a valid path in the input field.";
                guna2HtmlLabel8.ForeColor = Color.Red;
                guna2TextBox2.PlaceholderForeColor = Color.Red;
                guna2TextBox2.PlaceholderText = "Invalid path.";
                guna2TextBox2.Text = "";
                guna2HtmlLabel7.Text = "Error | Ready for next operation.";
                this.Alert("Invalid Path", Form_Alert.enmType.Error);
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
                    guna2HtmlLabel8.Text = $"[-] Started {run_path}";
                    guna2HtmlLabel7.Text = "Status: Ready.";
                }
                catch
                {
                    guna2HtmlLabel8.Text = "Failed to start exe.";
                    guna2HtmlLabel8.ForeColor = Color.Red;
                    guna2TextBox2.PlaceholderForeColor = Color.Red;
                    guna2TextBox2.PlaceholderText = "Failed to start";
                    guna2TextBox2.Text = "";
                    guna2HtmlLabel7.Text = "Error | Ready for next operation.";
                    this.Alert("Failed to start exe.", Form_Alert.enmType.Error);
                }
            }
        }

        private void guna2Button18_Click(object sender, EventArgs e) //Discord
        {
            System.Windows.Forms.Clipboard.SetText("https://discord.com/invite/znCq8VjghQ");
            this.Alert("Invite link copied to clipboard", Form_Alert.enmType.Info);
        }

        private void guna2Button2_Click(object sender, EventArgs e) // Explorer ++
        {

        }
    }
}
