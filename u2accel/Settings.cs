using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace u2accel
{
    class Settings
    {
        private const string settingsPath = "config.cfg";

        public int KeyCode;
        public bool IsKmh;


        private void ResetSettings()
        {
            KeyCode = (int)Keys.NumPad0;
            IsKmh = true;
        }

        public Settings()
        {
            ReadSettings();
        }

        public void ReadSettings()
        {
            try
            {
                StreamReader sr = new StreamReader(settingsPath);
                KeyCode = int.Parse(sr.ReadLine());
                IsKmh = bool.Parse(sr.ReadLine());
            }
            catch
            {
                ResetSettings();
                SaveSettings();
            }
        }

        public void SaveSettings()
        {
            StreamWriter sw = new StreamWriter(settingsPath);
            sw.WriteLine(KeyCode);
            sw.WriteLine(IsKmh);
            sw.Close();
        }
    }
}
