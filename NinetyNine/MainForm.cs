using System;
using System.Drawing;
using System.Windows.Forms;

namespace NinetyNine
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetForm();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {

        }

        private void SetForm()
        {
            WindowState = FormWindowState.Maximized;
        }
    }
}