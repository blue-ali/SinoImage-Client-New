using DocScanner.Bean;
using DocScanner.Bean.pb;
using DocScanner.LibCommon.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using Telerik.WinControls.UI;

namespace DocScanner.LibCommon
{
    public static class TreeHelper
    {

        

        public static RadTreeNode GetNode(RadTreeNode rootNode, string name)
        {
            foreach (RadTreeNode node in rootNode.Nodes)
            {
                if (node.Name.Equals(name))
                    return node;
                RadTreeNode next = GetNode(node, name);
                if (next != null)
                    return next;
            }
            return null;
        }

        public static void SetImageIcon(this RadTreeNode node, string fname, int height, int width)
        {
            if (ImgUtils.ImageHelper.IsImgExt(fname))
            {
                node.Image = ImgUtils.ImageHelper.LoadSizedImage(fname, width, height, "");
            }
            else
            {
                node.Image = FileHelper.GetFilesIcon(fname);
            }
        }

        public static List<RadTreeNode> GetChildren(this RadTreeNode node)
        {
            List<RadTreeNode> list = new List<RadTreeNode>();
            if (node.Nodes != null && node.Nodes.Count > 0)
            {
                foreach (RadTreeNode current in node.Nodes)
                {
                    list.Add(current);
                    list.AddRange(current.GetChildren());
                }
            }
            return list;
        }
    }
}
