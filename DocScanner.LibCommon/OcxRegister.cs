using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.LibCommon
{
    public class OcxRegister
    {
        // Methods
        [DllImport("kernel32.dll")]
        private static extern bool FreeLibrary(IntPtr lib);
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetProcAddress(IntPtr lib, string funcName);
        public Delegate Invoke(string APIName, Type t)
        {
            return Marshal.GetDelegateForFunctionPointer(GetProcAddress(IntPtr.Zero, APIName), t);
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string path);
        private bool Register()
        {
            return false;
        }

        private bool UnRegister()
        {
            return false;
        }

        // Nested Types
        public delegate int Compile(string command, StringBuilder inf);
    }

}
