using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.Main
{
    [Serializable]
    public class GroupItem
    {
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

        public bool visable
        {
            get;
            set;
        }

        [Browsable(false)]
        public List<ToolItem> ToolItems
        {
            get;
            set;
        }
    }
}
