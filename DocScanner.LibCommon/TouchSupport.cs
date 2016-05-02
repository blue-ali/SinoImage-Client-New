using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocScanner.LibCommon
{
    public class TouchSupport : Control
    {
        // Fields
        private const int SM_DIGITIZER = 0x5e;

        // Methods
        [DllImport("user32")]
        private static extern int GetSystemMetrics(int n);
        private bool SupportMultiTouch()
        {
            return ((GetSystemMetrics(0x5e) & 0x40) > 0);
        }

        protected override void WndProc(ref Message m)
        {
        }
    }

}
