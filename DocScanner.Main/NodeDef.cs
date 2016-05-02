using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.Main
{
    [Serializable]
    public class NodeDef
    {
        [NonSerialized]
        public object Tag;

        private List<NodeDef> _children = new List<NodeDef>();

        public string Name
        {
            get;
            set;
        }

        public int Count
        {
            get;
            set;
        }

        public List<NodeDef> Children
        {
            get
            {
                return this._children;
            }
            set
            {
                this._children = value;
            }
        }

        public void AddChild(NodeDef node)
        {
            bool flag = this._children == null;
            if (flag)
            {
                this._children = new List<NodeDef>();
            }
            this._children.Add(node);
        }
    }
}
