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
using System.Windows.Forms;

namespace Cloudforce_Revamped
{
    public partial class Main : Form
    {
        public static bool Dark = true;
        public static bool Light;
        private Settings settings = new Settings();
        private Utility utility = new Utility();
        private Patches patches = new Patches();
        private Games games = new Games();
        private Extra extra = new Extra();

        public Main()
        {
            InitializeComponent();
            checktheme();
            this.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2,
                          (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2);
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

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            settings.ShowDialog();
            this.Show();
            checktheme();
        }

        private void guna2GradientButton5_Click(object sender, EventArgs e)
        {
            this.Hide();
            patches.ShowDialog();
            this.Show();
            checktheme();
        }

        private void guna2GradientButton4_Click(object sender, EventArgs e)
        {
            this.Hide();
            utility.ShowDialog();
            this.Show();
            checktheme();
        }

        private void Main_Shown(object sender, EventArgs e)
        {
        }

        private void guna2GradientButton3_Click(object sender, EventArgs e)
        {
            this.Hide();
            games.ShowDialog();
            this.Show();
            checktheme();
        }

        private void guna2GradientButton6_Click(object sender, EventArgs e)
        {
            this.Hide();
            extra.ShowDialog();
            this.Show();
            checktheme();
        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }
    }
}