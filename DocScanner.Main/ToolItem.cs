using DocScanner.LibCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace DocScanner.Main
{
    [Serializable]
    public class ToolItem
    {
        private string _image;

        public string name
        {
            get;
            set;
        }

        public string text
        {
            get;
            set;
        }

        public string tip
        {
            get;
            set;
        }

        [Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
        public string image
        {
            get
            {
                return this._image;
            }
            set
            {
                bool flag = value.StartsWith(SystemHelper.ResourceDir);
                if (flag)
                {
                    this._image = value.Substring(SystemHelper.ResourceDir.Length);
                }
                else
                {
                    this._image = value;
                }
            }
        }

        public bool visable
        {
            get;
            set;
        }

        public string action
        {
            get;
            set;
        }
    }
}
