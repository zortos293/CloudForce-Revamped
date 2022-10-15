using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cloudforce_Revamped
{
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }

        private void guna2GradientButton7_Click(object sender, EventArgs e)
        {
            Main.KeyAuthApp.register(guna2TextBox1.Text, guna2TextBox2.Text, guna2TextBox3.Text);
            if (!Main.KeyAuthApp.response.success)
            {
                guna2HtmlLabel1.Text = Main.KeyAuthApp.response.message;
            }
            else
            {
                MessageBox.Show("Registered!");
                this.Close();
            }
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}