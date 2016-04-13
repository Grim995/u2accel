using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace u2accel
{
    public partial class RangesEditForm : Form
    {
        private float divider;

        public RangesEditForm(bool isKmh)
        {
            divider = isKmh ? 1.0f : 1.61f;
            InitializeComponent();
        }

        private void RangesEditForm_Load(object sender, EventArgs e)
        {
            Range[] ranges = Range.LoadRanges("ranges/u2.urf");
            for (int i = 0; i < ranges.Length; i++)
                listBox1.Items.Add(ranges[i]);
        }

        private void listBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            MessageBox.Show(e.KeyChar + " - " + (int)e.KeyChar);
        }

        private void listBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData != Keys.Delete)
                return;
            if (listBox1.SelectedIndex < 0 || listBox1.Items.Count < listBox1.SelectedIndex)
                return;

            int prevIndex = listBox1.SelectedIndex;
            listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            if (listBox1.Items.Count > 0)
                listBox1.SelectedIndex = prevIndex >= listBox1.Items.Count ? listBox1.Items.Count - 1 : prevIndex;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int a, b;
            if(!int.TryParse(textBox1.Text, out a) || !int.TryParse(textBox2.Text, out b))
            {
                MessageBox.Show("Invalid range arguments!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Clear();
                textBox2.Clear();
                return;
            }
            if( a == b || a > b || b == 0)
            {
                MessageBox.Show("There's nothing to measure this way!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Clear();
                textBox2.Clear();
                return;
            }
            Range r = new Range(a, b, 10, true);
            listBox1.Items.Add(r);
        }

        private void RangesEditForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void RangesEditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Range[] ranges = new Range[listBox1.Items.Count];
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    ranges[i] = (Range)listBox1.Items[i];
                }
                Range.SaveRanges(ranges, "ranges/u2.urf");
            }
            catch(Exception ex)
            {
                if (MessageBox.Show("Something went wrong and I couldn't save your ranges. Quit?", "Error...", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.No)
                    e.Cancel = true;
            }
        }
    }
}
