using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cloudforce_Revamped.Properties;
using KeyAuth;

namespace Cloudforce_Revamped
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
            
        }

        private Register register = new Register();


        public static bool SubExist(string name)
        {
            if (Main.KeyAuthApp.user_data.subscriptions == null)
                return false;
            if (Main.KeyAuthApp.user_data.subscriptions.Exists(x => x.subscription == name))
                return true;
            return false;
        }
        private void guna2GradientButton7_Click(object sender, EventArgs e)
        {
            Main.KeyAuthApp.login(guna2TextBox1.Text, guna2TextBox2.Text);
            if (!Main.KeyAuthApp.response.success)
            {
                guna2HtmlLabel1.Text = Main.KeyAuthApp.response.message;
            }
            else
            {
                MessageBox.Show("Logged in!");
                this.Close();
            }
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            register.ShowDialog();
            this.Show();
        }
    }
}