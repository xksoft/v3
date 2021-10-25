using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Library
{
    public class RegexHelper
    {
        public static ArrayList GetArrayList(string html, string regex)
        {
          
            ArrayList result = new ArrayList();
            try
            {
                MatchCollection mccc = Regex.Matches(html, regex, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                for (int i = 0; i < mccc.Count; i++)
                {
                    result.Add(mccc[i].Groups[0].Value);
                }
               
                return result;
            }
            catch (Exception ex)
            {
               
                result = new ArrayList();
                result.Add("正则表达式有误：" + ex.Message);
                return result;
            }
        }
        public static List<string> GetList(string html, string reg)
        {
            List<string> list = new List<string>();
            Regex r = new Regex(reg);
            MatchCollection mc = r.Matches(html);
            string groupname = GetGroupName(reg);
            for (int i = 0; i < mc.Count; i++)
            {
                string v = "";
                if (groupname.Length > 0)
                {
                    v = mc[i].Groups[groupname].Value;
                }
                else
                {
                    v = mc[i].Value;
                }
                if (!list.Contains(v)) { list.Add(v); }

            }
            return list;
        }

        public static string GetGroupName(string regex)
        {
            if (regex.Contains("(?<"))
            {

                regex = regex.Substring(regex.IndexOf("(?<") + 3);

            }
            else { return ""; }
            if (regex.Contains(">")) { regex = regex.Remove(regex.IndexOf(">")); }
            return regex;

        }
      
    }
}
