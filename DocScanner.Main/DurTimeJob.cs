using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.Main
{
    public class DurTimeJob : IDisposable
    {
        [ThreadStatic]
        private static UCStatusBar _statusbar;

        public DurTimeJob(string msg)
        {
            bool flag = DurTimeJob._statusbar != null;
            if (flag)
            {
                DurTimeJob._statusbar.BeginJob(msg);
            }
        }

        public static void SetStatusBar(UCStatusBar bar)
        {
            DurTimeJob._statusbar = bar;
        }

        public void Dispose()
        {
            bool flag = DurTimeJob._statusbar != null;
            if (flag)
            {
                DurTimeJob._statusbar.EndJob();
            }
        }
    }
}
