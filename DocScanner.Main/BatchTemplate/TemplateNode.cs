using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.Main
{
    [Serializable]
    public class TemplateNode
    {
        [NonSerialized]
        public object Tag;

        private List<TemplateNode> _children = new List<TemplateNode>();

        public string Name
        {
            get;
            set;
        }

        //public int Count
        //{
        //    get;
        //    set;
        //}

        public List<TemplateNode> Children
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

        public void AddChild(TemplateNode node)
        {
            this._children.Add(node);
        }

        public bool HasChildren()
        {
            if (_children.Count > 0)
            {
                return true;
            }else
            {
                return false;
            }
        }
    }
}
