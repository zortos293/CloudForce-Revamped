using Guna.UI2.WinForms;
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
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Cloudforce_Revamped
{
    public partial class Utility : Form
    {
        string mainpath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Cloudforce\\";
        public Utility()
        {
            InitializeComponent();
            
        }

        #region Theme
        void checktheme()
        {
            Debug.WriteLine("Checking theme...");
            Debug.WriteLine("Dark: " + Main.Dark);
            Debug.WriteLine("Light: " + Main.Light);
            if (Main.Dark == true)
            {
                this.BackColor = Color.DimGray;
            }
            if (Main.Light == true)
            {
                this.BackColor = Color.WhiteSmoke;
            }
        }
        #endregion

        #region Download Stuff
        bool DownloadFinished;
        public void File_Downloader(string URL, string path,Guna.UI2.WinForms.Guna2GradientButton button)
        {
            // download file with progress bar
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
        #endregion


        
        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2GradientButton12_Click(object sender, EventArgs e)
        {
            if (File.Exists(mainpath + "\\ProcessHacker\\ProcessHacker.exe"))
            {
                Process.Start(mainpath + "\\ProcessHacker\\\\ProcessHacker.exe");
            }
            else
            {
                File_Downloader("https://picteon.dev/files/ProcessHacker.zip", mainpath + "\\ProcessHacker.zip", guna2GradientButton12);

                ZipFile.ExtractToDirectory(mainpath + "\\ProcessHacker.zip", mainpath + "\\");
                Process.Start(mainpath + "\\ProcessHacker\\\\ProcessHacker.exe"); // TODO
                guna2GradientButton12.Enabled = true;

            }
        }

        private void back_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2GradientButton11_Click(object sender, EventArgs e)
        {
            if (File.Exists(mainpath + "\\Discord.zip"))
            {
                Process.Start(mainpath + "\\Discord.zip");
            }
            else
            {
                File_Downloader("https://picteon.dev/files/Discord.zip", mainpath + "\\Discord.zip", guna2GradientButton11);
                    
                ZipFile.ExtractToDirectory(mainpath + "\\Discord.zip", mainpath + "\\");
                Process.Start(mainpath + "\\Discord"); // TODO
                guna2GradientButton1.Enabled = false;

            }
        }

        private void Utility_Shown(object sender, EventArgs e)
        {
            checktheme();
        }

        private void guna2GradientButton14_Click(object sender, EventArgs e) // Firefox
        {
            if (File.Exists(mainpath + "\\Firefox.zip"))
            {
                Process.Start(mainpath + "\\Firefox.zip");
            }
            else
            {
                File_Downloader("https://picteon.dev/files/Firefox.zip", mainpath + "\\Firefox.zip", guna2GradientButton11);

                ZipFile.ExtractToDirectory(mainpath + "\\Firefox.zip", mainpath + "\\");
                Process.Start(mainpath + "\\Discord"); // TODO
                guna2GradientButton1.Enabled = false;

            }
        }

        private void guna2GradientButton4_Click(object sender, EventArgs e)
        {
            if (File.Exists(mainpath + "\\DoraTheExplorer.exe"))
            {
                Process.Start(mainpath + "\\DoraTheExplorer.exe");
            }
            else
            {
                File_Downloader("https://picteon.dev/files/Explorer++.exe", mainpath + "\\DoraTheExplorer.exe", guna2GradientButton4);


                Process.Start(mainpath + "\\ProcessHacker\\\\ProcessHacker.exe"); // TODO
                guna2GradientButton4.Enabled = true;

            }
        }
    }
}
