using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.Common
{
    internal class SingleApp
    {
        public static void CheckSingleByProcessName()
        {
            string processName = Process.GetCurrentProcess().ProcessName;
            Process[] processesByName = Process.GetProcessesByName(processName);
            bool flag = processesByName != null && processesByName.Count<Process>() > 1;
            if (flag)
            {
                Process[] array = processesByName;
                for (int i = 0; i < array.Length; i++)
                {
                    Process process = array[i];
                    bool flag2 = process.Id != Process.GetCurrentProcess().Id;
                    if (flag2)
                    {
                        SingleApp.HandleRunningInstance(process);
                    }
                }
                Environment.Exit(0);
            }
        }

        private static void HandleRunningInstance(Process instance)
        {
            SingleApp.ShowWindowAsync(instance.MainWindowHandle, 1);
            SingleApp.SetForegroundWindow(instance.MainWindowHandle);
        }

        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}
