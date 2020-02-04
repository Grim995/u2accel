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
    public partial class MainForm : Form
    {
        GameReader u2reader;
        Settings settings;
        float prevSpeed = 0;
        float speed = 0;
        int frameCounter = 0;

        float maxSpeed = 0.0f;

        Range[] ranges;
        Report report;

        DateTime last = DateTime.Now;


        const string speedLabel = "Current speed: {0:0.00} km/h";
        const string speedLabelMph = "Current speed: {0:0.00} MPH";

        const string maxSpeedLabel = "Max speed: {0:0.00} km/h";
        const string maxSpeedLabelMph = "Max speed: {0:0.00} MPH";

        private void ReloadRanges()
        {
            listBox1.Items.Clear();
            ranges = Range.LoadRanges("ranges/u2.urf", 10, settings.IsKmh);
            for (int i = 0; i < ranges.Length; i++)
                listBox1.Items.Add(ranges[i]);
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            report = new Report();
            ReloadSettings();
            ReloadRanges();
            u2reader = new U2Reader();
            u2reader.Init();
            KeyHook.SharedInstance.OnKeyPressed += OnGlobalKeyDown;
        }

        private void OnGlobalKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == (Keys)settings.KeyCode)
            {
                Reset();
                bool shouldReport = true;
                foreach(Range range in ranges)
                {
                    if (range.Time != 0.0f)
                        continue;
                    shouldReport = false;
                }
                if(shouldReport)
                    report.AddSection(ranges);
                foreach (Range r in ranges)
                {
                    r.Reset();
                }
            }
        }

        private void Reset()
        {
            frameCounter = 0;
            maxSpeed = 0;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            frameCounter++;
            prevSpeed = speed;
            speed = u2reader.GetSpeed() * 1.61f;
            maxSpeed = Math.Max(maxSpeed, speed);
            foreach(Range range in ranges)
            {
                range.Think(speed, frameCounter);
            }
            for(int i=0; i<listBox1.Items.Count; i++)
            {
                listBox1.Items[i] = listBox1.Items[i];
            }

            var lat = (DateTime.Now - last).TotalMilliseconds;
            label1.Text = string.Format(settings.IsKmh ? speedLabel : speedLabelMph, speed);
            label2.Text = string.Format(settings.IsKmh ? maxSpeedLabel : maxSpeedLabelMph, maxSpeed);
            latencyLabel.Text = string.Format("Latency {0:0.00}ms", lat);
            latencyLabel.ForeColor = lat > 50 ? Color.Red : Color.Black;
            last = DateTime.Now;
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
            if(saveFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            StreamWriter writer = new StreamWriter(saveFileDialog1.FileName);
            writer.Write(report.ReportString);
            writer.Close();
        }

        private void settingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SettingsForm settings = new SettingsForm();
            settings.ShowDialog();
            ReloadSettings();
            ReloadRanges();
        }

        private void ReloadSettings()
        {
            settings = new Settings();
        }

        private void needForSpeedMostWantedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            u2reader = new MWReader();
            u2reader.Init();
        }

        private void needForSpeedCarbonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            u2reader = new CReader();
            u2reader.Init();
        }
    }
}
