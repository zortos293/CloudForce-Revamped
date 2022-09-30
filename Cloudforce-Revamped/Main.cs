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
    public partial class Main : Form
    {
        public static bool Dark;
        public static bool Light;
        Settings settings = new Settings();
        Utility utility = new Utility();
        public Main()
        {
            InitializeComponent();
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
        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            
            this.Hide();
            settings.ShowDialog();
            this.Show();
            checktheme();

        }

        private void guna2GradientButton5_Click(object sender, EventArgs e)
        {

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
    }
}
