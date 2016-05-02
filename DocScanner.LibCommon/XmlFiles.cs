namespace Logos.DocScaner.Common
{
    using System;
    using System.Xml;

    /// <summary>
    /// XmlFiles 的摘要说明。
    /// </summary>
    public class XmlFiles : XmlDocument
    {
        // Fields
        private string _xmlFileName;

        // Methods
        public XmlNode FindNode(string xPath)
        {
            return base.SelectSingleNode(xPath);
        }

        public XmlNodeList GetNodeList(params string[] datas)
        {
            XmlNode node = null;
            foreach (string str in datas)
            {
                if (node == null)
                {
                    node = base.SelectSingleNode(str);
                }
                else
                {
                    node = node.SelectSingleNode(str);
                }
            }
            return node.ChildNodes;
        }

        public static string GetNodeValue(XmlNode node)
        {
            return node.InnerText;
        }

        public string GetNodeValue(params string[] datas)
        {
            XmlNode node = null;
            foreach (string str in datas)
            {
                if (node == null)
                {
                    node = base.SelectSingleNode(str);
                }
                else
                {
                    node = node.SelectSingleNode(str);
                }
            }
            return node.InnerText;
        }

        public T GetNodeValue<T>(params string[] xmlpath)
        {
            return (T)Convert.ChangeType(this.GetNodeValue(xmlpath), typeof(T));
        }

        public bool Open(string xmlFile)
        {
            this._xmlFileName = xmlFile;
            this.Load(this._xmlFileName);
            return true;
        }

        public void SetNodeValue(string xPath, string newVlaue)
        {
            base.SelectSingleNode(xPath).InnerText = newVlaue;
        }
    }

}