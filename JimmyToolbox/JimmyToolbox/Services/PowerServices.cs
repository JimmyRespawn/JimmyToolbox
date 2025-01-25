using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace JimmyToolbox.Services
{
    public class PowerServices
    {
        [DllImport("kernel32.dll")]
        public static extern uint SetThreadExecutionState(uint esFlags);

        private const uint ES_CONTINUOUS = 0x80000000;
        private const uint ES_SYSTEM_REQUIRED = 0x00000001;
        private const uint ES_DISPLAY_REQUIRED = 0x00000002;

        public static Task RestartComputer()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "shutdown",
                Arguments = "/r /t 0", // /r: 重启, /t 0: 立即
                UseShellExecute = false,
                CreateNoWindow = true
            });
            return Task.CompletedTask;
        }

        // 执行 CMD 命令
        public static Task ShutdownComputer()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "shutdown",
                Arguments = "/s /t 0", // /s: 关机, /t 0: 立即
                UseShellExecute = false,
                CreateNoWindow = true
            });
            return Task.CompletedTask;
        }

        /// <summary>
        /// Clamshell Mode
        /// </summary>
        public static void PreventSleep()
        {
            SetThreadExecutionState(ES_CONTINUOUS | ES_SYSTEM_REQUIRED | ES_DISPLAY_REQUIRED);
        }

        public static void AllowSleep()
        {
            SetThreadExecutionState(ES_CONTINUOUS);
        }
    }
}
