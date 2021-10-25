using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace V3Plugin
{
    public interface ProcessPlugin:ICloneable
    {
        UserControl MainControl
        {
            get;
        }
        string Id { get; }
        string ProcessName { get; }
        string Author { get; }
        Dictionary<int, string> ArticleProcess(Dictionary<int, string> objects);
        List<string> KeyWords { get; set; }
        string[] Parameters { get; set; }
        

    }
}
