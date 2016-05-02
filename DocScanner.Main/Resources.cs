using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

//111
namespace DocScanner.Main
{
    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"), DebuggerNonUserCode, CompilerGenerated]
    internal class Resources
    {
        private static ResourceManager resourceMan;

        private static CultureInfo resourceCulture;

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static ResourceManager ResourceManager
        {
            get
            {
                bool flag = Resources.resourceMan == null;
                if (flag)
                {
                    ResourceManager resourceManager = new ResourceManager("DocScanner.Main.Properties.Resources", typeof(Resources).Assembly);
                    Resources.resourceMan = resourceManager;
                }
                return Resources.resourceMan;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture
        {
            get
            {
                return Resources.resourceCulture;
            }
            set
            {
                Resources.resourceCulture = value;
            }
        }

        internal static Bitmap del
        {
            get
            {
                object @object = Resources.ResourceManager.GetObject("del.jpg", Resources.resourceCulture);
                return (Bitmap)@object;
            }
        }

        internal static Bitmap Filter
        {
            get
            {
                object @object = Resources.ResourceManager.GetObject("Filter.jpg", Resources.resourceCulture);
                return (Bitmap)@object;
            }
        }

        internal static Bitmap newbatch
        {
            get
            {
                object @object = Resources.ResourceManager.GetObject("newbatch.jpg", Resources.resourceCulture);
                return (Bitmap)@object;
            }
        }

        internal static Bitmap view
        {
            get
            {
                object @object = Resources.ResourceManager.GetObject("view.jpg", Resources.resourceCulture);
                return (Bitmap)@object;
            }
        }

        internal Resources()
        {
        }
    }
}
