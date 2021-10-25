using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace XSniffer
{
    static class Program
    {
        public static string []StartArgs=null;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string []args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            StartArgs = args;
            Application.Run(new frmMain());
        }
    }
}
