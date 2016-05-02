using DocScanner.LibCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocScanner.Main
{
    static class Program
    {
        // Methods
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            new TestFormMain().ShowDialog();
            TmpGC.EmptyRubbish();
        }

        [STAThread]
        public static DialogResult ThreadSafeWay(object msg, object form)
        {
            Program.FormDelegate method = new Program.FormDelegate(Program.ThreadSafeWay);
            bool invokeRequired = (form as Form).InvokeRequired;
            DialogResult result;
            if (invokeRequired)
            {
                result = (DialogResult)(form as Form).Invoke(method, new object[]
                {
                    msg,
                    form
                });
            }
            else
            {
                result = (form as Form).ShowDialog();
            }
            return result;
        }

        // Nested Types
        private delegate DialogResult FormDelegate(object msg, object form);

    }
}
