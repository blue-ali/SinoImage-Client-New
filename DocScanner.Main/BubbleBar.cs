using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls;

namespace DocScanner.Main
{
    public class BubbleBar : RadControl
    {
        private BubbleBarElement element;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public BubbleBarElement Element
        {
            get
            {
                return this.element;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RadEditItemsAction]
        public RadItemOwnerCollection Items
        {
            get
            {
                return this.element.Items;
            }
        }

        protected override void CreateChildItems(RadElement parent)
        {
            this.element = new BubbleBarElement();
            this.element.AutoSizeMode = RadAutoSizeMode.FitToAvailableSize;
            parent.Children.Add(this.element);
        }
    }
}
