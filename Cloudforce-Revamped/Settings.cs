using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cloudforce_Revamped
{
    public partial class Settings : Form
    {

        public Settings()
        {
            InitializeComponent();
            ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("select * from Win32_Processor");
            ManagementObjectSearcher managementObjectSearcher2 = new ManagementObjectSearcher("select * from Win32_VideoController");
            foreach (ManagementObject item in managementObjectSearcher2.Get())
            {
                guna2HtmlLabel12.Text = ("GPU: " + item["Name"]);
            }
            foreach (ManagementObject item2 in managementObjectSearcher.Get())
            {
                guna2HtmlLabel11.Text = ("CPU: " + item2["Name"]);
                guna2HtmlLabel13.Text = ("Threads: " + item2["NumberOfLogicalProcessors"]?.ToString() + ", Cores: " + item2["NumberOfCores"]);
            }
        }
        #region Theme
        void checktheme()
        {
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
        private void back_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guna2ComboBox1.SelectedItem.ToString() == "Light Theme")
            {
                pictureBox1.Image = null;
                this.BackColor = Color.WhiteSmoke;
                Main.Dark = false;
                Main.Light = true;
                if (Main.Light.Equals(true))
                {
                    MessageBox.Show("Changed to Light mode !");
                }
            }
            if (guna2ComboBox1.SelectedItem.ToString() == "Dark Theme")
            {
                this.BackColor = Color.DimGray;
                Main.Dark = true;
                Main.Light = false;
                if (Main.Dark.Equals(true))
                {
                    MessageBox.Show("Changed to Dark mode !");
                }
            }
        }

        private void Settings_Shown(object sender, EventArgs e)
        {
            checktheme();
        }
    }
}
