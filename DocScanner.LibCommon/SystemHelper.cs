using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace DocScanner.LibCommon
{

    public static class SystemHelper
    {
        private static string _appdirectory = null;

        public static string ResourceDir
        {
            get
            {
                return SystemHelper.GetAssemblesDirectory() + "Resources\\";
            }
        }

        public static EControlMode GetConponentMode(this Component con)
        {
            MethodInfo namedMethod = ReflectHelper.GetNamedMethod(con.GetType(), "GetService");
            object obj = null;
            bool flag = namedMethod != null;
            if (flag)
            {
                obj = namedMethod.Invoke(con, new object[]
                {
                    typeof(IDesignerHost)
                });
            }
            bool flag2 = obj != null || LicenseManager.UsageMode == LicenseUsageMode.Designtime;
            EControlMode result;
            if (flag2)
            {
                result = EControlMode.DesignMode;
            }
            else
            {
                result = EControlMode.RuntimeMode;
            }
            return result;
        }

        public static void SetAssemblesDirectory(Type mainmoudle)
        {
            string location = mainmoudle.Assembly.Location;
            string text = location.Substring(0, location.LastIndexOf("\\")) + "\\";
            Directory.SetCurrentDirectory(text);
            SystemHelper._appdirectory = text;
        }

        public static void SetAssemblesDirectory(string dir)
        {
            SystemHelper._appdirectory = dir;
        }

        public static string GetAssemblesDirectory()
        {
            bool flag = SystemHelper._appdirectory == null;
            if (flag)
            {
                SystemHelper.SetAssemblesDirectory(typeof(SystemHelper));
            }
            return SystemHelper._appdirectory;
        }

        public static string GetDesignDirectory()
        {
            return "D:\\tigera\\imgmgr\\TigEra.DocScaner.Main\\";
        }

        public static string GetCurInstanceExe()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            return location.Substring(location.LastIndexOf("\\") + 1, location.Length - location.LastIndexOf("\\") - 1);
        }

        public static void ExplorerFile(string fname)
        {
            Process.Start("explorer.exe", "\"" + fname + "\"");
        }

        [DllImport("Kernel32")]
        public static extern int GetThreadId(IntPtr handler);
    }
}