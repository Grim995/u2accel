using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace u2accel
{
    class U2Reader : GameReader
    {
        public U2Reader()
        {
            speedAddress = 0x007F09E8;
            name = "speed2";
        }
    }
}
