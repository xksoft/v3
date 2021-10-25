using System;
using System.Collections.Generic;
using System.Text;using System.Net;
using System.IO;
using DevExpress.XtraTreeList.Native;
using DevExpress.XtraTreeList.Nodes;
using xEngine.Plugin.HtmlParsing;

namespace V3.Common
{
    public class HtmlEngine
    {
        public static int count = 0;
  
       static TreeListNode getNode(HtmlNode htmlnode, string keyword, TreeListNodes nodes)
        {
            TreeListNode node=new XtraListNode(nodes);

            string classname = "";
            node.Tag = htmlnode.XPath;
            if (htmlnode.ChildNodes.Count > 0)
            {
                for (int i = 0; i < htmlnode.ChildNodes.Count; i++)
                {
                    if (htmlnode.ChildNodes[i].Name.Contains("#comment") || htmlnode.ChildNodes[i].Name.Contains("#text") || !htmlnode.ChildNodes[i].OuterHtml.Contains(keyword))
                        continue;
                    node.Nodes.Add(getNode(htmlnode.ChildNodes[i], keyword, node.Nodes));
                    count++;
                }
            }
            if (htmlnode.Attributes.Count > 0)
            {
                bool needadd = false;
                TreeListNode nodeattr = new XtraListNode(nodes);
                nodeattr.SetValue(0,htmlnode.Name + "(属性个数：" + htmlnode.Attributes.Count + ")"); 
                nodeattr.Tag = htmlnode.XPath;

                //for (int i = 0; i < htmlnode.Attributes.Count; i++)
                //{
                //    TreeListNode temp = new XtraListNode(nodes);
                //    temp.SetValue(0,"Key:" + htmlnode.Attributes[i].Name + "\r\nValue:" + htmlnode.Attributes[i].Value); 
                //    if ((htmlnode.Attributes[i].Name + htmlnode.Attributes[i].Value + htmlnode.Attributes[i].ToString()).Contains(keyword))
                //        needadd = true;
                //    if (htmlnode.Attributes[i].Name.ToLower() == "class" || htmlnode.Attributes[i].Name.ToLower() == "name" || htmlnode.Attributes[i].Name.ToLower() == "id")
                //        classname += "|" + htmlnode.Attributes[i].Value;
                //    temp.Tag = htmlnode.Attributes[i].XPath;
                //    nodeattr.Nodes.Add(temp);
                //    count++;
                //}
                //if (needadd)
                //    node.Nodes.Add(nodeattr);
            }
           node.SetValue(0,htmlnode.Name + classname + "(长度：" + htmlnode.InnerHtml.Length + ")"); 
            if (htmlnode.Name == "#document")
            {
                node.SetValue(0,"根路径");
               node.Tag = "根路径";
            }
            return node;
        }
        public static HtmlNode findNode( HtmlDocument htmldoc, string path)
        {
            return htmldoc.DocumentNode.SelectSingleNode(path);
        }
    }
}
