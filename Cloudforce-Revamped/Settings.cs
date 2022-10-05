/**************************************************************
 * Cloud-Force by Zortos293 and Kief
 *
 * (c) 2022. All rights reserved.
 * You may not distrobute app in anyway if the credits are removed, nor sell it.
 *
 * 9/30/22 11:45AM PDT
 */

using System;
using System.Drawing;
using System.Linq;
using System.Management;
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

        private void back_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guna2ComboBox1.SelectedItem.ToString() == "Light Theme") //TODO CHECK If theme is applied
            {
                pictureBox1.Image = null;
                this.BackColor = Color.WhiteSmoke;
                Main.Dark = false;
                Main.Light = true;
                changecolorBTN(123, 0, 238, 170, 0, 255);
                if (Main.Light.Equals(true))
                {
                    MessageBox.Show("Changed to Light mode !");
                }
            }
            if (guna2ComboBox1.SelectedItem.ToString() == "Dark Theme")
            {
                this.BackColor = Color.FromArgb(64, 64, 64);
                Main.Dark = true;
                Main.Light = false;
                changecolorBTN(123, 0, 238, 170, 0, 255);
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