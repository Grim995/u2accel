using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace u2accel
{
    class GameProcess
    {
        /// <summary>
        /// WinAPI constant to read process' memory
        /// </summary>
        const int PROCESS_WM_READ = 0x0010;

        /// <summary>
        /// Private string to know the game process' name
        /// </summary>
        private string processName;

        /// <summary>
        /// Game's process
        /// </summary>
        private Process gameProcess;

        /// <summary>
        /// Game's process' HANDLE
        /// </summary>
        private IntPtr gameProcessHandle;


        /// <summary>
        /// Low level function to get process' handle
        /// </summary>
        /// <param name="dwDesiredAccess">Desired level of access</param>
        /// <param name="bInheritHandle"></param>
        /// <param name="dwProcessId">Process id</param>
        /// <returns>Process HANDLE</returns>
        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        /// <summary>
        /// Low level function to read process memory
        /// </summary>
        /// <param name="hProcess">HANDLE of a process</param>
        /// <param name="lpBaseAddress">An address to read</param>
        /// <param name="lpBuffer">Buffer to read into</param>
        /// <param name="dwSize">Number of bytes to read</param>
        /// <param name="lpNumberOfBytesRead">Total ammount of bytes that were read</param>
        /// <returns>BOOL?</returns>
        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(int hProcess,
          int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        public static async Task<GameProcess> OpenGameProcessAsync(string processName)
        {
            return await Task.Run(() => OpenGameProcess(processName));
        }

        private static GameProcess OpenGameProcess(string processName)
        {
            Process[] processes;
            do
            {
                Thread.Sleep(200);
            } while ((processes = Process.GetProcessesByName(processName)).Length < 1);
            return new GameProcess(processName, processes[0]);
        }

        private GameProcess(string name, Process p)
        {
            processName = name;
            gameProcess = p;
            gameProcessHandle = OpenProcess(PROCESS_WM_READ, false, p.Id);
        }

        public byte[] ReadMemory(int address, int size, ref int dataRead)
        {
            byte[] data = new byte[size];
            ReadProcessMemory((int)gameProcessHandle, address, data, size, ref dataRead);
            return data;
        }
    }
}
