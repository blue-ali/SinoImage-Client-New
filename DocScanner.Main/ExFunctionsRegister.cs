using DocScanner.LibCommon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.Main
{
    public class ExFunctionsRegister
    {
        public ExFunctionsRegister()
        {
            ExFunctionsRegister.AutoRegisterProcessCmd();
        }

        public static void AboutMe(object param = null)
        {
            FormAboutMe formAboutMe = new FormAboutMe();
            formAboutMe.ShowDialog();
        }

        public static void CheckUpdate(object param = null)
        {
            string text = "升级.exe";
            bool flag = File.Exists(text);
            if (flag)
            {
                Process.Start(text);
            }
        }

        public static void ReportBug(object param = null)
        {
            ExFunctionsRegister.StartProcess("程序反馈.exe");
        }

        public static void AutoRegisterProcessCmd()
        {
            CmdDispatcher val = LibCommon.AppContext.Cur.GetVal<CmdDispatcher>(typeof(CmdDispatcher));
            val.RegisterCMD("testRegisterOcx", new Action<object>(ExFunctionsRegister.testRegisterOcx));
            val.RegisterCMD("testtwain", new Action<object>(ExFunctionsRegister.testtwain));
            val.RegisterCMD("TestNetFunction", new Action<object>(ExFunctionsRegister.TestNetFunction));
        }

        public static void TestNetFunction(object param)
        {
            Process.Start("Test.DocScaner.Network.exe");
        }

        private static void StartProcess(string ppath)
        {
            bool flag = File.Exists(ppath);
            if (flag)
            {
                Process.Start(ppath);
            }
        }

        public static void testRegisterOcx(object param)
        {
            ExFunctionsRegister.StartProcess("注册.exe");
        }

        public static void ShowChangeHistory(object param = null)
        {
            string arguments = "\"" + SystemHelper.GetAssemblesDirectory() + "ChangeHistory.txt\"";
            Process.Start("explorer.exe  ", arguments);
        }

        public static void testtwain(object param)
        {
            ExFunctionsRegister.StartProcess("DocScanner.Adapter.SharpTwainTest.exe");
        }

        public static void ExplorerRootDir(object param = null)
        {
            Process.Start("explorer.exe   ", "\"" + SystemHelper.GetAssemblesDirectory() + "\"");
        }
    }
}
