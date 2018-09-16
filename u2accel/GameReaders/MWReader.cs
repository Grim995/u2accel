using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace u2accel
{
    class MWReader : GameReader
    {
        new public int GameId
        {
            get
            {
                return 3;
            }
        }

        public MWReader() : base()
        {
            speedAddress = 0x00914798;
            name = "speed";
        }
    }
}
