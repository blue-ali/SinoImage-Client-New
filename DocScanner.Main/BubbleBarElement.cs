using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Layouts;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace DocScanner.Main
{
    public class BubbleBarElement : RadElement
    {
        private FillPrimitive fill;

        private BorderPrimitive border;

        private StackLayoutPanel panel;

        private RadItemOwnerCollection items;

        public RadItemOwnerCollection Items
        {
            get
            {
                return this.items;
            }
        }

        protected override void InitializeFields()
        {
            base.InitializeFields();
            base.Shape = new RoundRectShape(12);
            this.items = new RadItemOwnerCollection();
            this.items.ItemTypes = new Type[]
            {
                typeof(RadButtonElement)
            };
            this.items.ItemsChanged += new ItemChangedDelegate(this.items_ItemsChanged);
        }

        protected override void CreateChildElements()
        {
            this.border = new BorderPrimitive();
            this.border.GradientStyle = GradientStyles.Solid;
            this.border.ForeColor = Color.FromArgb(0, 0, 0);
            this.border.AutoSizeMode = RadAutoSizeMode.FitToAvailableSize;
            this.Children.Add(this.border);
            this.panel = new StackLayoutPanel();
            this.panel.Orientation = Orientation.Horizontal;
            this.panel.Margin = new Padding(0, 20, 10, 0);
            this.panel.Alignment = ContentAlignment.MiddleCenter;
            this.panel.StretchHorizontally = false;
            this.Children.Add(this.panel);
            this.items.Owner = this.panel;
        }

        public void FillWithColor(Color color)
        {
            this.fill = new FillPrimitive();
            this.fill.BackColor2 = Color.FromArgb(253, 253, 253);
            this.fill.BackColor = color;
            this.fill.NumberOfColors = 2;
            this.fill.GradientStyle = GradientStyles.Linear;
            this.fill.GradientAngle = 90f;
            this.fill.AutoSizeMode = RadAutoSizeMode.FitToAvailableSize;
            this.Children.Insert(0, this.fill);
        }

        private void items_ItemsChanged(RadItemCollection changed, RadItem target, ItemsChangeOperation operation)
        {
            bool flag = operation == ItemsChangeOperation.Inserted || operation == ItemsChangeOperation.Set;
            if (flag)
            {
                target.AddBehavior(new BubbleBarMouseOverBehavior());
            }
        }
    }
}
