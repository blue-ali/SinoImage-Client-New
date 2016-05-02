using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls;

namespace DocScanner.Main
{
    public class BubbleBarMouseOverBehavior : PropertyChangeBehavior
    {
        public BubbleBarMouseOverBehavior() : base(RadElement.IsMouseOverProperty)
        {
        }

        public override void OnPropertyChange(RadElement element, RadPropertyChangedEventArgs e)
        {
            bool flag = (bool)e.NewValue;
            if (flag)
            {
                element.ResetValue(RadElement.ScaleTransformProperty);
                AnimatedPropertySetting animatedPropertySetting = new AnimatedPropertySetting(RadElement.ScaleTransformProperty, new SizeF(0.65f, 0.65f), new SizeF(1f, 1f), 5, 30);
                animatedPropertySetting.ApplyValue(element);
            }
            else
            {
                AnimatedPropertySetting animatedPropertySetting2 = new AnimatedPropertySetting(RadElement.ScaleTransformProperty, new SizeF(0.65f, 0.65f), 5, 30);
                animatedPropertySetting2.ApplyValue(element);
            }
        }
    }
}
