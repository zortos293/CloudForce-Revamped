using Guna.UI2.WinForms;
using Newtonsoft.Json;
using Sentry;
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
using KeyAuth;
using LoginScreen;


namespace Cloudforce_Revamped_V2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            KeyAuthApp.init();  
            if (!KeyAuthApp.response.success)
            {
                MessageBox.Show(KeyAuthApp.response.message);
            }
            WebClient a = new WebClient();

            a.DownloadFile("https://raw.githubusercontent.com/zortos293/CloudForce-Revamped/master/games.json",mainpath + "games.json");
            jsonString = File.ReadAllText(mainpath + "games.json");
            string json = a.DownloadString("https://keyauth.win/api/seller/?sellerkey=84e4776b79c0528d2d3246b4f2bd8178&type=fetchallsessions");
            dynamic array = JsonConvert.DeserializeObject(json);
            guna2HtmlLabel9.Text = $"CloudForce Users Online: {array.sessions.Count}";
        }


        #region Login Stuff
        public static api KeyAuthApp = new api(
            name: "CF Early",
            ownerid: "0t0Sr0pLaB",
            secret: "c52ed8ebcefc829ffed9a73e9c85b73fd5a8e244abec5531ef1cf87628d181e0",
            version: "1.0"
        );

        public static bool SubExist(string name)
        {
            if (KeyAuthApp.user_data.subscriptions == null)
                return false;
            if (KeyAuthApp.user_data.subscriptions.Exists(x => x.subscription == name))
                return true;
            return false;
        }
        #endregion
        #region Download Stuff
        public static string mainpath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Cloudforce\\";
        public string jsonString;  //Need to change
        public static bool ForceExit = false;
        public static bool Done = false;
        public int BtnNumber;
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
        public void File_Downloader(string URL, string path, Guna2Button button)
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

        public void DownloadGame(int JsonNumber, string downloadpath) // Download Game Trough Onedrive <<<<
        {
            var username = Environment.UserName;
            Done = false;
            WebClient client = new WebClient(); 
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
            BtnNumber = JsonNumber;
            process.OutputDataReceived += new DataReceivedEventHandler(process_OutputDataReceived);
            process.StartInfo.FileName = mainpath + "downloader.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            if (SubExist("premium"))
            {
                MessageBox.Show("Using Google Drive (Premium)");
                process.StartInfo.Arguments = "copy -P --transfers=10 --checkers=16 " + "zortosgdrive:\\" + results.Game[JsonNumber].GameOnedrive + " " + downloadpath + results.Game[JsonNumber].GameOnedrive;

            }
            else
            {
                process.StartInfo.Arguments = "copy -P --transfers=4 --checkers=16 " + "zortosdrive:\\" + results.Game[JsonNumber].GameOnedrive + " " + downloadpath + results.Game[JsonNumber].GameOnedrive;

            }
            process.Exited += new EventHandler(p_Exited);
            process.EnableRaisingEvents = true;
            process.Start();
            process.BeginOutputReadLine();
        }
        public void ExtractZipFileToDirectory(string sourceZipFilePath, string destinationDirectoryName, bool overwrite)
        {
            using (var archive = ZipFile.Open(sourceZipFilePath, ZipArchiveMode.Read))
            {
                if (!overwrite)
                {
                    archive.ExtractToDirectory(destinationDirectoryName);
                    return;
                }

                DirectoryInfo di = Directory.CreateDirectory(destinationDirectoryName);
                string destinationDirectoryFullPath = di.FullName;

                foreach (ZipArchiveEntry file in archive.Entries)
                {
                    string completeFileName = Path.GetFullPath(Path.Combine(destinationDirectoryFullPath, file.FullName));

                    if (!completeFileName.StartsWith(destinationDirectoryFullPath, StringComparison.OrdinalIgnoreCase))
                    {
                        throw new IOException("Trying to extract file outside of destination directory. See this link for more info: https://snyk.io/research/zip-slip-vulnerability");
                    }

                    if (file.Name == "")
                    {// Assuming Empty for Directory
                        Directory.CreateDirectory(Path.GetDirectoryName(completeFileName));
                        continue;
                    }
                    file.ExtractToFile(completeFileName, true);
                }
            }
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

                        this.guna2ProgressBar2.Invoke(new Action(() => this.guna2HtmlLabel4.Text = split[2] + " " + split[3]));

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
                // if exeception is object refrence
                if (er.InnerException is NullReferenceException)
                {
                    return;
                }
                else
                {
                    MessageBox.Show("An Error occoured\n" + er.Message + "\nPress ok to Continue");

                    return;
                }
               
            }
        }

        void p_Exited(object sender, EventArgs e)
        {

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
            Done = true;
        }
        public void Alert(string msg, Form_Alert.enmType type)
        {
            Form_Alert frm = new Form_Alert();
            frm.showAlert(msg, type);
        }
        private bool Startgame(int JsonNumber)
        {
            var username = Environment.UserName;
            
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
        #endregion
        private async void Form1_Load(object sender, EventArgs e)
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
            if (Startgame(0) == false)
            {
                DownloadGame(0, mainpath); // Overwatch
                while (Done == false)
                {
                    Application.DoEvents();
                }
                guna2HtmlLabel4.Visible = true;
                try
                {
                    guna2HtmlLabel4.Text = "[/] Extracting Data 01 - 09 (Please Be patient)";
                    await Task.Run(() =>
                    {
                        ExtractZipFileToDirectory(mainpath + "overwatch\\data\\casc\\data\\data01-09.zip", mainpath + "overwatch\\data\\casc\\data\\", true);
                    });
                    File.Delete(mainpath + "overwatch\\data\\casc\\data\\data01-09.zip");
                    guna2HtmlLabel4.Text = "[/] Extracting Data 10 - 13 (Please Be patient)";
                    await Task.Run(() =>
                    {
                        ExtractZipFileToDirectory(mainpath + "overwatch\\data\\casc\\data\\data910111213.zip", mainpath + "overwatch\\data\\casc\\data\\", true);
                    });
                    File.Delete(mainpath + "overwatch\\data\\casc\\data\\data910111213.zip");
                    guna2HtmlLabel4.Text = "[/] Extracting Data 14 - 17 (Please Be patient)";
                    await Task.Run(() =>
                    {
                        ExtractZipFileToDirectory(mainpath + "overwatch\\data\\casc\\data\\data14151617.zip", mainpath + "overwatch\\data\\casc\\data\\", true);
                    });
                    File.Delete(mainpath + "overwatch\\data\\casc\\data\\data14151617.zip");
                    guna2HtmlLabel4.Text = "[/] Extracting Data 18 - 20 (Please Be patient)";
                    await Task.Run(() =>
                    {
                        ExtractZipFileToDirectory(mainpath + "overwatch\\data\\casc\\data\\data181920.zip", mainpath + "overwatch\\data\\casc\\data\\", true);
                    });
                    File.Delete(mainpath + "overwatch\\data\\casc\\data\\data181920.zip");
                    guna2HtmlLabel4.Text = "[/] Extracting indicies (Please Be patient)";
                    await Task.Run(() =>
                    {
                        ExtractZipFileToDirectory(mainpath + "overwatch\\data\\casc\\indices\\indices.zip", mainpath + "overwatch\\data\\casc\\indices\\", true);
                    });
                    File.Delete(mainpath + "overwatch\\data\\casc\\indices\\indices.zip");
                    guna2HtmlLabel4.Visible = false;
                    if (Startgame(0)) // Overwatch
                    {
                        this.Alert("Starting overwatch, please wait.", Form_Alert.enmType.Success);
                    }
                    
                }
                catch (Exception ex)
                {
                    SentrySdk.CaptureException(ex);
                }
                
            }
            
            

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
                        this.Alert("Downloading success!", Form_Alert.enmType.Success);
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
                        this.Alert("Downloading success!", Form_Alert.enmType.Success);
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
            guna2Button15.Enabled = true;
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
                    this.Alert("Extracting success!", Form_Alert.enmType.Success);
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
            guna2Button16.Enabled = true;
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
                    this.Alert("Starting exe success!", Form_Alert.enmType.Success);
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
            guna2Button17.Enabled = true;
        }

        private void guna2Button18_Click(object sender, EventArgs e) //Discord
        {
            Clipboard.SetText("https://discord.com/invite/znCq8VjghQ");
            this.Alert("Invite link copied to clipboard", Form_Alert.enmType.Info);
        }

        private void guna2Button2_Click(object sender, EventArgs e) // Explorer ++
        {
            if (File.Exists(mainpath + "\\DoraTheExplorer.exe"))
            {
                Process.Start(mainpath + "\\DoraTheExplorer.exe");
                this.Alert("Started Explorer++", Form_Alert.enmType.Success);
            }
            else
            {
                File_Downloader("https://picteon.dev/files/Explorer++.exe", mainpath + "\\DoraTheExplorer.exe", guna2Button2);

                Process.Start(mainpath + "\\DoraTheExplorer.exe"); // TODO
                guna2Button2.Enabled = true;
                this.Alert("Started Explorer++", Form_Alert.enmType.Success);
            }
        }

        private void guna2Button19_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button5_Click(object sender, EventArgs e) //PH64
        {
            if (File.Exists(mainpath + "\\ProcessHacker\\ProcessHacker.exe"))
            {
                Process.Start(mainpath + "\\ProcessHacker\\\\ProcessHacker.exe");
                this.Alert("Started ProcessHacker", Form_Alert.enmType.Success);
            }
            else
            {
                File_Downloader("https://picteon.dev/files/ProcessHacker.zip", mainpath + "\\ProcessHacker.zip", guna2Button5);

                ZipFile.ExtractToDirectory(mainpath + "\\ProcessHacker.zip", mainpath + "\\");
                Process.Start(mainpath + "\\ProcessHacker\\\\ProcessHacker.exe");
                this.Alert("Started ProcessHacker", Form_Alert.enmType.Success);
                guna2Button5.Enabled = true;
            }
        }

        private void guna2Button20_Click(object sender, EventArgs e) //Discord
        {
            File_Downloader("https://discord.com/api/downloads/distributions/app/installers/latest?channel=stable&platform=win&arch=x86", mainpath + "\\shardshardup.exe", guna2Button20);
            Process.Start(mainpath + "\\shardshardup.exe");
            this.Alert("Started Discord installer", Form_Alert.enmType.Success);
            guna2Button20.Enabled = true;
        }

        private void guna2Button3_Click(object sender, EventArgs e) //Zorots Unpwetter
        {
            if (File.Exists(mainpath + "\\ZortosUnpwetterA.exe"))
            {
                Process.Start(mainpath + "\\ZortosUnpwetterA.exe");
                this.Alert("Started Zortos Unpwetter", Form_Alert.enmType.Success);
            }
            else
            {
                File_Downloader("https://op-ffa.net/gfn/ZortosUnpwetterA.exe", mainpath + "\\ZortosUnpwetterA.exe", guna2Button3);

                Process.Start(mainpath + "\\ZortosUnpwetterA.exe"); // TODO
                this.Alert("Started Zortos Unpwetter", Form_Alert.enmType.Success);
                guna2Button3.Enabled = true;
            }
        }

        private void guna2Button6_Click(object sender, EventArgs e) //Anypwet
        {
            if (File.Exists(mainpath + "\\AnyPwet.exe"))
            {
                Process.Start(mainpath + "\\AnyPwet.exe");
                this.Alert("Started AnyPwet", Form_Alert.enmType.Success);
            }
            else
            {
                File_Downloader("https://picteon.dev/files/AnyDesk.exe", mainpath + "\\AnyPwet.exe", guna2Button6);
                Process.Start(mainpath + "\\AnyPwet.exe");
                this.Alert("Started AnyPwet", Form_Alert.enmType.Success);
                guna2Button6.Enabled = true;
            }
        }

        private void guna2Button12_Click(object sender, EventArgs e) //Cmd
        {
            if (Directory.Exists(mainpath + "\\cmdpwet.exe"))
            {
                Process.Start(mainpath + "\\cmdpwetaa.exe");
                this.Alert("Started CommpandPrompt", Form_Alert.enmType.Success);
            }
            else
            {
                File_Downloader("https://picteon.dev/files/NotCMDNvidia.exe", mainpath + "\\cmdpwetaa.exe", guna2Button12);
                Process.Start(mainpath + "\\cmdpwetaa.exe");
                guna2Button12.Enabled = true;
                this.Alert("Started CommandPrompt", Form_Alert.enmType.Success);
            }
        }

        private void guna2Button10_Click(object sender, EventArgs e) //parsec
        {
            if (Directory.Exists(mainpath + "\\Parsec\\"))
            {
                new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        WorkingDirectory = mainpath + "\\Parsec\\",
                        WindowStyle = ProcessWindowStyle.Normal,
                        FileName = mainpath + "\\Parsec\\parsecd.exe"
                    }
                }.Start();
                this.Alert("Started Parsec", Form_Alert.enmType.Success);
            }
            else
            {
                File_Downloader("https://picteon.dev/files/Parsec%20%282%29.zip", mainpath + "\\Parsec.zip", guna2Button10);
                Directory.CreateDirectory(mainpath + "\\Parsec\\");
                ZipFile.ExtractToDirectory(mainpath + "\\Parsec.zip", mainpath + "\\Parsec");
                new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        WorkingDirectory = mainpath + "\\Parsec\\",
                        WindowStyle = ProcessWindowStyle.Normal,
                        FileName = mainpath + "\\Parsec\\parsecd.exe"
                    }
                }.Start();
                guna2Button10.Enabled = true;
                this.Alert("Started Parsec", Form_Alert.enmType.Success);
            }
        }

        private void guna2Button13_Click(object sender, EventArgs e) //OBS
        {
            MessageBox.Show("If you already installed, open ir via explorer++\n If not install it to C:\\OBS");
            File_Downloader("https://cdn-fastly.obsproject.com/downloads/OBS-Studio-28.0.3-Full-Installer-x64.exe", mainpath + "\\ops.exe", guna2Button13);
            Process.Start(mainpath + "\\ops.exe");
            guna2Button13.Enabled = true;
            this.Alert("Started OBS Installer", Form_Alert.enmType.Success);
        }

        private void guna2Button8_Click(object sender, EventArgs e) //Firefox
        {
            if (Directory.Exists(mainpath + "\\Firefox\\"))
            {
                Process.Start(mainpath + "\\Firefox\\runthis.exe");
                this.Alert("Started FireFox", Form_Alert.enmType.Success);
            }
            else
            {
                File_Downloader("https://picteon.dev/files/Firefox.zip", mainpath + "\\Firefox.zip", guna2Button8);

                ZipFile.ExtractToDirectory(mainpath + "\\Firefox.zip", mainpath + "\\");
                Process.Start(mainpath + "\\Firefox\\runthis.exe");
                guna2Button8.Enabled = true;
                this.Alert("Started FireFox", Form_Alert.enmType.Success);
            }
        }

        private void guna2Button7_Click(object sender, EventArgs e) //LibreWolf
        {
            if (Directory.Exists(mainpath + "\\librewolf-105.0.1\\"))
            {
                Process.Start(mainpath + "\\librewolf-105.0.1\\LibreWolf-Portable.exe");
                this.Alert("Started FireFox", Form_Alert.enmType.Success);
            }
            else
            {
                File_Downloader("https://gitlab.com/librewolf-community/browser/windows/uploads/a9d86b83d8e66b9c3c75a0c2221aecdd/librewolf-105.0.1-1.en-US.win64-portable.zip", mainpath + "\\librewolf.zip", guna2Button7);

                ZipFile.ExtractToDirectory(mainpath + "\\librewolf.zip", mainpath + "\\");
                Process.Start(mainpath + "\\librewolf-105.0.1\\LibreWolf-Portable.exe");
                this.Alert("Started FireFox", Form_Alert.enmType.Success);
                guna2Button7.Enabled = true;
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e) // Notepad
        {
            if (File.Exists(mainpath + "\\Notepad2x64.exe"))
            {
                Process.Start(mainpath + "\\Notepad2x64.exe");
                this.Alert("Started Notepad", Form_Alert.enmType.Success);
            }
            else
            {
                File_Downloader("https://picteon.dev/files/Notepad2x64.exe", mainpath + "\\Notepad2x64.exe", guna2Button4);
                Process.Start(mainpath + "\\Notepad2x64.exe");
                guna2Button4.Enabled = true;
                this.Alert("Started Notepad", Form_Alert.enmType.Success);
            }
        }

        private void guna2Button11_Click(object sender, EventArgs e)// Spotify
        {
            if (Directory.Exists(mainpath + "\\Spotify\\"))
            {
                Process.Start(mainpath + "\\Spotify\\Spotify.exe");
                this.Alert("Started Spotify", Form_Alert.enmType.Success);
            }
            else
            {
                File_Downloader("https://picteon.dev/files/Spotify.zip", mainpath + "\\Spotify.zip", guna2Button11);
                ZipFile.ExtractToDirectory(mainpath + "\\Spotify.zip", mainpath + "\\");
                Process.Start(mainpath + "\\Spotify\\Spotify.exe");
                guna2Button11.Enabled = true;
                this.Alert("Started Spotify", Form_Alert.enmType.Success);
            }
        }

        private void guna2Button1_Click_1(object sender, EventArgs e) // Desktop
        {
            if (File.Exists(mainpath + "\\ZortosDesktop.exe"))
            {
                Process.Start(mainpath + "\\ZortosDesktop.exe", "");
            }
            else
            {
                File_Downloader("https://github.com/zortos293/ZortosToolBox/raw/main/DesktopOverlay.exe", mainpath + "\\ZortosDesktop.exe", guna2Button1);
                File_Downloader("https://github.com/zortos293/ZortosToolBox/raw/main/1.png", mainpath + "\\1.png", guna2Button1);
                File_Downloader("https://github.com/zortos293/ZortosToolBox/raw/main/WinXShell.jcfg", mainpath + "\\WinXShell.jcfg", guna2Button1);

                Process[] ps = Process.GetProcessesByName("explorer");

                foreach (Process p in ps)
                    p.Kill();
                Process.Start(mainpath + "\\ZortosDesktop.exe", "-Desktop");
                guna2Button1.Enabled = true;
                this.Alert("Launched Desktop", Form_Alert.enmType.Success);
            }
        }
        private void guna2Button14_Click(object sender, EventArgs e) // rpcs3
        {

            if (Directory.Exists(mainpath + "\\rpcs3\\"))
            {
                Process.Start(mainpath + "\\rpcs3\\rpcs3.exe");
                this.Alert("Started rpcs3", Form_Alert.enmType.Success);
            }
            else
            {
                File_Downloader("https://files.zortos.me/Files/Launchers/rpcs3.zip", mainpath + "\\rpcs3.zip", guna2Button14);
                ZipFile.ExtractToDirectory(mainpath + "\\rpcs3.zip", mainpath + "\\");
                Process.Start(mainpath + "\\rpcs3\\rpcs3.exe");
                guna2Button14.Enabled = true;
                this.Alert("Started rpcs3", Form_Alert.enmType.Success);
            }
        }
        private void Home_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Shown(object sender, EventArgs e)
        {

        }

        private async void guna2ImageButton2_Click(object sender, EventArgs e)
        {
            if (!Login.SubExist("premium"))
            {
                MessageBox.Show("You need to have Cloudforce Early Access to Play Fall Guys. Join the discrod located in the main form to purchase");
                return;
            }
            if (File.Exists(mainpath + "\\Epic Games\\Launcher\\Engine\\Binaries\\Win64\\EpicGamesLauncher.exe"))
            {
                Process.Start(mainpath + "\\Epic Games\\Launcher\\Engine\\Binaries\\Win64\\EpicGamesLauncher.exe");
                this.Alert("Login then return to CF", Form_Alert.enmType.Info);
                MessageBox.Show("After login Close this messagebox");
                new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        WorkingDirectory = $"{mainpath}\\",
                        WindowStyle = ProcessWindowStyle.Hidden,
                        FileName = $"{mainpath}\\shard.bat"
                    }
                }.Start();
                DialogResult dialogResult = MessageBox.Show("Did you get in?", "Quesiton", MessageBoxButtons.YesNo);
                this.Alert("CF has a prompt for you.", Form_Alert.enmType.Info);
                if (dialogResult == DialogResult.No)
                {
                    new Process()
                    {
                        StartInfo = new ProcessStartInfo()
                        {
                            WorkingDirectory = $"{mainpath}\\",
                            WindowStyle = ProcessWindowStyle.Hidden,
                            FileName = $"{mainpath}\\shard.bat"
                        }
                    }.Start();
                }
                guna2HtmlLabel4.Text = "[-] Started: Fall Guys";
                this.Alert("Started Fall Guys", Form_Alert.enmType.Success);
            }
            else
            {
                guna2ProgressBar2.Style = ProgressBarStyle.Blocks;
                guna2HtmlLabel4.Text = "[-] Creating Directorys.";
                this.Alert("Creating Directorys", Form_Alert.enmType.Info);
                Directory.CreateDirectory(@"c:\ProgramData\Epic");
                Directory.CreateDirectory(@"c:\ProgramData\Epic\EpicGamesLauncher");
                Directory.CreateDirectory(@"c:\ProgramData\Epic\EpicGamesLauncher\Data");
                Directory.CreateDirectory(@"c:\ProgramData\Epic\EpicGamesLauncher\Data\Manifests");
                byte[] pwet = KeyAuthApp.download("041744");
                File.WriteAllBytes(@"c:\ProgramData\Epic\EpicGamesLauncher\Data\Manifests\882D7E384AEE27D7ED9F51BF72FACD60.item", pwet);
                // ----------------------------------------------------------------------- < Installing FallGuys
                guna2HtmlLabel4.Text = "[-] Downloading: FallGuys.zip";
                this.Alert("Downloading Fall Guys", Form_Alert.enmType.Info);
                guna2HtmlLabel4.Visible = true;
                DownloadGame(1, "b:\\");
                while (Done == false)
                {
                    Application.DoEvents();
                }
                guna2HtmlLabel4.Visible = true;
                // ----------------------------------------------------------------------- < Installing Epic Games Launcher
                this.Alert("Preparing Fall guys", Form_Alert.enmType.Info);
                guna2HtmlLabel4.Text = "[-] Preparing Fall Guys.";
                File_Downloader("https://files.zortos.me/Files/Launchers/Epic%20Games.zip", mainpath + "\\Epic%20Games.zip", guna2Button10);
                byte[] shard = KeyAuthApp.download("108975");
                File.WriteAllBytes(mainpath + "shard.bat", shard);
                await Task.Run(() =>
                {
                    ZipFile.ExtractToDirectory(mainpath + "\\Epic%20Games.zip", mainpath + "\\");
                });
                guna2Button10.Enabled = true;
                guna2HtmlLabel4.Visible = false;
                Process.Start(mainpath + "\\Epic Games\\Launcher\\Engine\\Binaries\\Win64\\EpicGamesLauncher.exe");
                this.Alert("After login return to CF", Form_Alert.enmType.Info); 
                MessageBox.Show("After login Close this messagebox");
                new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        WorkingDirectory = $"{mainpath}\\",
                        WindowStyle = ProcessWindowStyle.Hidden,
                        FileName = $"{mainpath}\\shard.bat"
                    }
                }.Start();
                Thread.Sleep(10000);
                this.Alert("CF has a prompt for you", Form_Alert.enmType.Info);
                DialogResult dialogResult = MessageBox.Show("Did you get in?", "Quesiton", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    new Process()
                    {
                        StartInfo = new ProcessStartInfo()
                        {
                            WorkingDirectory = $"{mainpath}\\",
                            WindowStyle = ProcessWindowStyle.Hidden,
                            FileName = $"{mainpath}\\shard.bat"
                        }
                    }.Start();
                }
                // -----------------------------------------------------------------------
                this.Alert("Wait for epic games to refresh", Form_Alert.enmType.Info);
                Thread.Sleep(3000);
                this.Alert("Refreshed epic games.", Form_Alert.enmType.Success);
                guna2HtmlLabel4.Text = "[-] Started: EpicGames.";
                this.Alert("Launch Fall guys now!", Form_Alert.enmType.Info);
            }
        }
        private Login login = new Login();
        public void guna2Button19_Click_1(object sender, EventArgs e) //login
        {
            if (Login.SubExist("premium")) return;
            else
            {
                this.Hide();
                login.ShowDialog();
                if (Login.SubExist("premium"))
                {
                    guna2Button19.Text = "Logged in";
                    guna2HtmlLabel10.Text = "Number of Early Access users : " + KeyAuthApp.app_data.numOnlineUsers;
                    guna2Button19.Image = global::Cloudforce_Revamped_V2.Properties.Resources.checked_user_male_208px;
                }
                this.Show();
            }
        }

        private void guna2ImageButton3_Click(object sender, EventArgs e) // Lunar Client
        {
            if (Directory.Exists(mainpath + $"C:\\Users\\{Environment.UserName}\\AppData\\Local\\Programs\\lunarclient\\"))
            {
                Process.Start($"C:\\Users\\{Environment.UserName}\\AppData\\Local\\Programs\\lunarclient\\Lunar Client.exe");
                this.Alert("Launched Lunar Client", Form_Alert.enmType.Success);
            }
            else
            {
                File_Downloader("https://launcherupdates.lunarclientcdn.com/Lunar%20Client%20v2.13.0.exe", mainpath + "\\Lunar_Install.exe", guna2Button2);
                Process.Start(mainpath + "\\Lunar_Install.exe");
                guna2Button2.Enabled = true;
                this.Alert("Launched Lunar Client", Form_Alert.enmType.Success);
            }
        }

        private void guna2ImageButton4_Click(object sender, EventArgs e) //Minecraft launcher
        {
            if (File.Exists(mainpath + "\\Minecraft.exe"))
            {
                Process.Start(mainpath + "\\Minecraft.exe");

            }
            else
            {
                File_Downloader("https://cdn.discordapp.com/attachments/1029959860428742706/1030223464646312036/Minecraft.exe", mainpath + "\\Minecraft.exe", guna2Button2);
                Process.Start(mainpath + "\\Minecraft.exe");
                guna2Button2.Enabled = true;
            }
        }

        private async void guna2ImageButton5_Click(object sender, EventArgs e)
        {

            if (Directory.Exists($"C:\\Users\\{Environment.UserName}\\AppData\\Roaming\\Microsoft\\Windows\\Start Menu\\Programs\\Roblox"))
            {
                Process.Start($@"C:\\Users\\{Environment.UserName}\\AppData\\Roaming\\Microsoft\\Windows\\Start Menu\\Programs\\Roblox\\Roblox Player.lnk");
            }
            else
            {
                File_Downloader("https://picteon.dev/files/Roblox.zip", mainpath + "\\Roblox.zip", guna2Button2);
                await Task.Run(() =>
                {
                    ZipFile.ExtractToDirectory(mainpath + "\\Roblox.zip", mainpath + "\\");
                });
                Process.Start(mainpath + "\\Roblox\\Versions\\version-995b3631bc754ce1\\RobloxPlayerLauncher.exe");
                guna2Button2.Enabled = true;
                MessageBox.Show("Relaunch roblox after install ;) Roblox installer will have error, we will kill it.");
                await Task.Delay(10000);
                foreach (var process in Process.GetProcessesByName("RobloxPlayerLauncher"))
                {
                    process.Kill();
                }
            }
        }

        private void guna2Button21_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(mainpath + "\\7-Zip\\"))
            {
                Process.Start(mainpath + "\\7-Zip\\7zFM.exe");
                this.Alert("Started 7-Zip", Form_Alert.enmType.Success);
            }
            else
            {
                File_Downloader("https://picteon.dev/files/7-Zip.zip", mainpath + "\\7-Zip.zip", guna2Button21);
                ZipFile.ExtractToDirectory(mainpath + "\\7-Zip.zip", mainpath + "\\");
                Process.Start(mainpath + "\\7-Zip\\7zFM.exe");
                guna2Button21.Enabled = true;
                this.Alert("Started 7-Zip", Form_Alert.enmType.Success);
            }
        }
    }
}
