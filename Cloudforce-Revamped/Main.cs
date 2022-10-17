/**************************************************************
 * Cloud-Force by Zortos293 and Kief
 *
 * (c) 2022. All rights reserved.
 * You may not distrobute app in anyway if the credits are removed, nor sell it.
 *
 * 9/30/22 11:45AM PDT
 */

using KeyAuth;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Linq;
using System.Net;
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
        private steamgames steam = new steamgames();
        private login Login = new login();

        public Main()
        {
            InitializeComponent();
            checktheme();
            KeyAuthApp.init();
            if (!KeyAuthApp.response.success)
            {
                MessageBox.Show(KeyAuthApp.response.message);
            }
            WebClient a = new WebClient();
            string json = a.DownloadString("https://keyauth.win/api/seller/?sellerkey=84e4776b79c0528d2d3246b4f2bd8178&type=fetchallsessions");
            dynamic array = JsonConvert.DeserializeObject(json);
            guna2HtmlLabel3.Text = $"Number of users Online : {array.sessions.Count}";
        }

        public static api KeyAuthApp = new api(
            name: "CF Early",
            ownerid: "0t0Sr0pLaB",
            secret: "c52ed8ebcefc829ffed9a73e9c85b73fd5a8e244abec5531ef1cf87628d181e0",
            version: "1.0"
        );

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

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            this.Hide();
            steam.ShowDialog();
            this.Show();
            checktheme();
            //if (login.SubExist("premium"))
            //{
            //    this.Hide();
            //    steam.ShowDialog();
            //    this.Show();
            //    checktheme();
            //}
            //else
            //{
            //    MessageBox.Show("You need to have Cloudforce Early Access to use this feature");
            //}
        }

        private void guna2GradientButton7_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login.ShowDialog();
            this.Show();
            checktheme();
            guna2HtmlLabel3.Text = $"Number of users Online : {KeyAuthApp.app_data.numOnlineUsers}";
        }

        private void guna2HtmlLabel3_Click(object sender, EventArgs e)
        {
        }
    }
}