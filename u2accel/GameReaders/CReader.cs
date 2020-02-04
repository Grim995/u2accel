using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace u2accel
{
    class CReader : GameReader
    {
        new public int GameId
        {
            get
            {
                return 2;
            }
        }

        public CReader() : base()
        {
            speedAddress = 0x00A8E610;
            name = "nfsc";
        }
    }
}
