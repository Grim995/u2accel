using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace u2accel
{
    abstract class GameReader
    {
        GameProcess gameProcess;
        protected int speedAddress;
        protected string name;
        bool initialized;

        public int GameId;

        public GameReader()
        {
            initialized = false;
        }

        /// <summary>
        /// Initialaze async, waiting for the game to be launched if needed
        /// </summary>
        public async void Init()
        {
            gameProcess = await GameProcess.OpenGameProcessAsync(name);
            initialized = true;
        }

        /// <summary>
        /// Return current in-game speed in MPH
        /// </summary>
        /// <returns></returns>
        public float GetSpeed()
        {
            if (!initialized)
                return 0.0f;

            int bytesRead = 0;
            byte[] buffer = new byte[4];


            buffer = gameProcess.ReadMemory(speedAddress, 4, ref bytesRead);
            return BitConverter.ToSingle(buffer, 0);
        }
    }
}
