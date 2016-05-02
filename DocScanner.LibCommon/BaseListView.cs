using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocScanner.LibCommon
{
    public class BaseListView : ListView
    {
        // Methods
        public void ColorRows(Color diffcolor)
        {
            int num = 0;
            foreach (ListViewItem item in base.Items)
            {
                num++;
                if ((num % 2) == 0)
                {
                    item.BackColor = diffcolor;
                }
            }
        }

        public void FitContentWidth()
        {
            foreach (ColumnHeader header in base.Columns)
            {
                header.Width = -1;
            }
        }
    }

}
