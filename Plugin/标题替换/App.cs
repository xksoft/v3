using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using V3Plugin;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;
namespace 标题替换
{
    public class App : ProcessPlugin
    {
        public  frmMain frm = new frmMain();
        public  Random r = new Random();
        public  string KeyWord = "";
        public UserControl MainControl
        {
            get
            {
                return frm;
            }
        }
        public List<string> KeyWords
        {
            get;
            set;
        }
        public string Id
        {

            get { return "7433-863E-F10E-90CF-67543"; }
        }
        public string ProcessName
        {
            get { return "标题替换"; }
        }
        public Dictionary<int, string> ArticleProcess(Dictionary<int, string> objects)
        {
            string title = GetTitle();
            if (title.Length == 0) { Console.WriteLine("标题替换插件：无可用标题，不替换，请尽快放入标题！"); return objects; }

            objects[0] = title;
           
            return objects;
        }


        public string GetTitle()
        {
            string t = Parameters[0];
            string[] ts = t.Split('\n');
            List<string> tt = new List<string>();
            foreach (string s in ts)
            {
                if (s.Trim().Length > 0 && !tt.Contains(s.Trim()))
                {
                    tt.Add(s.Trim());
                }
            }
            if (tt.Count > 0) 
            {
                string title = tt[r.Next(0, tt.Count)];
                string text="";
                tt.Remove(title);
                foreach(string s in tt)
                {
                   text += s + "\n";
                }
                frm.richTextBox.Text = text;
                return title; 
            }
            else { return ""; }
        }

        public object Clone()
        {
            return new App();
        }

        public string Author
        {
            get { return ""; }
        }

        public string[] Parameters
        {
            get
            {
                string[] s = new string[] { "" };


                s[0] = frm.richTextBox.Text;
               
               
                return s;
            }
            set
            {
                if (value == null) { value = new string[] { "" }; }
                if (value.Length == 1)
                {
                    frm.richTextBox.Text = value[0];
                    
                }

            }
        }



    }
}
