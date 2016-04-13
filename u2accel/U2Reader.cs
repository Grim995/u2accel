using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace u2accel
{
    class U2Reader
    {
        GameProcess speed2;
        private const int speedAddress = 0x007F09E8;
        bool initialized;

        public U2Reader()
        {
            initialized = false;
        }

        public async void Init()
        {
            speed2 = await GameProcess.OpenGameProcessAsync("speed2");
            initialized = true;
        }

        public float GetSpeed()
        {
            if (!initialized)
                return 0.0f;

            int bytesRead = 0;
            byte[] buffer = new byte[4];


            buffer = speed2.ReadMemory(speedAddress, 4, ref bytesRead); // ReadProcessMemory((int)speed2Handle, speedAddress, buffer, buffer.Length, ref bytesRead);
            return BitConverter.ToSingle(buffer, 0);
        }
    }
}
