using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace u2accel
{
    public partial class SettingsForm : Form
    {
        Settings settings;

        public SettingsForm()
        {
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            settings = new Settings();
            textBox1.Text = ((Keys)settings.KeyCode).ToString();
            radioButton1.Checked = settings.IsKmh;
            radioButton2.Checked = !settings.IsKmh;
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            textBox1.Text = e.KeyData.ToString();
            e.Handled = true;
            e.SuppressKeyPress = true;
            settings.KeyCode = (int)e.KeyCode;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            settings.IsKmh = !radioButton2.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            settings.SaveSettings();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
