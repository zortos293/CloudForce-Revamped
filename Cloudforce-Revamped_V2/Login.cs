using Cloudforce_Revamped_V2;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoginScreen
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        public void Alert(string msg, Form_Alert.enmType type)
        {
            Form_Alert frm = new Form_Alert();
            frm.showAlert(msg, type);
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        public static bool SubExist(string name)
        {
            if (Form1.KeyAuthApp.user_data.subscriptions == null)
                return false;
            if (Form1.KeyAuthApp.user_data.subscriptions.Exists(x => x.subscription == name))
                return true;
            return false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Form1.KeyAuthApp.login(txtUserName.Text, txtpassword.Text);
            if (!Form1.KeyAuthApp.response.success)
            {
                this.Alert("Incorrect user or password", Form_Alert.enmType.Error);
                txtUserName.Clear();
                txtpassword.Clear();
                txtUserName.Focus();
            }
            else
            {
                this.Alert("Logged In!", Form_Alert.enmType.Success);
                this.Close();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            txtUserName.Clear();
            txtpassword.Clear();
            txtUserName.Focus();
        }
        private void label3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
