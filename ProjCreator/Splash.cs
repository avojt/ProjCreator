using System;
using System.Windows.Forms;

namespace ProjCreator
{
    public partial class Splash : Form
    {
        static System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();

        public Splash()
        {
            InitializeComponent();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            myTimer.Stop();

            MainForm mf = new MainForm();

            mf.Show();
            this.Hide();
        }

        private void Splash_Shown(object sender, EventArgs e)
        {
            myTimer = new Timer();

            myTimer.Interval = 8000;
            myTimer.Start();

            myTimer.Tick += Timer1_Tick;
        }
    }
}
