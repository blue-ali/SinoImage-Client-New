using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls;
using Telerik.WinControls.Layouts;

namespace DocScanner.Main
{
    public class BubbleBarLayout : LayoutPanel
    {
        private Size prefferedSize;

        public override void PerformLayoutCore(RadElement affectedElement)
        {
            this.prefferedSize = Size.Empty;
            foreach (RadElement current in this.Children)
            {
                Size preferredSize = current.GetPreferredSize(this.AvailableSize);
                preferredSize.Width = (int)((float)preferredSize.Width * current.ScaleTransform.Width);
                this.prefferedSize.Width = this.prefferedSize.Width + preferredSize.Width;
                this.prefferedSize.Height = Math.Max(this.prefferedSize.Height, preferredSize.Height);
            }
            int num = 0;
            foreach (RadElement current2 in this.Children)
            {
                Size preferredSize2 = current2.GetPreferredSize(this.AvailableSize);
                current2.Bounds = new Rectangle(new Point(num, this.prefferedSize.Height - (int)((float)preferredSize2.Height * current2.ScaleTransform.Height)), preferredSize2);
                num += (int)((float)preferredSize2.Width * current2.ScaleTransform.Width);
            }
        }

        public override Size GetPreferredSizeCore(Size proposedSize)
        {
            return this.prefferedSize;
        }
    }
}
