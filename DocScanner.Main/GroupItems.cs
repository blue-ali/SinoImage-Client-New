using DocScanner.LibCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.Main
{
    [Serializable]
    public class GroupItems
    {
        public List<GroupItem> TopBarItems
        {
            get;
            set;
        }

        public List<GroupItem> RightPaneItems
        {
            get;
            set;
        }

        public void SerializeToXML(string fname)
        {
            SerializeHelper.SerializeToXML<GroupItems>(this, fname);
        }

        public static GroupItems UnSerializeFromXML(string fname)
        {
            return SerializeHelper.DeSerializeFromXML<GroupItems>(fname);
        }
    }
}
