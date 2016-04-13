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
    public partial class MainForm : Form
    {
        U2Reader u2reader;
        float prevSpeed = 0;
        float speed = 0;
        int frameCounter = 0;

        Range[] ranges;


        const string speedLabel = "Current speed: {0} km/h";


        private void ReloadRanges()
        {
            listBox1.Items.Clear();
            ranges = Range.LoadRanges("ranges/u2.urf");
            for (int i = 0; i < ranges.Length; i++)
                listBox1.Items.Add(ranges[i]);
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ReloadRanges();
            u2reader = new U2Reader();
            u2reader.Init();
            KeyHook.SharedInstance.OnKeyPressed += OnGlobalKeyDown;
        }

        private void OnGlobalKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.NumPad0)
            {
                foreach (Range r in ranges)
                {
                    r.Reset();
                }
                Reset();
            }
        }

        private void Reset()
        {
            frameCounter = 0;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            frameCounter++;
            prevSpeed = speed;
            speed = u2reader.GetSpeed() * 1.61f;
            foreach(Range range in ranges)
            {
                /*if (speed == 0)
                    range.Reset();*/

                range.Think(speed, frameCounter);

                /*if (speed >= range.LowerBorder)
                    range.EnterFrame = frameCounter;
                if (speed >= range.UpperBorder)
                    range.LeaveFrame = frameCounter;*/
            }
            for(int i=0; i<listBox1.Items.Count; i++)
            {
                listBox1.Items[i] = listBox1.Items[i];
            }

            label1.Text = string.Format(speedLabel, (int)speed);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void editRangesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RangesEditForm f = new RangesEditForm(true);
            f.ShowDialog();
            ReloadRanges();
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
