using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XSniffer
{
   
        /// <summary>
        ///双缓冲ListView ，解决闪烁
        /// </summary>
        public class DoubleBufferListView : ListView
        {
            public DoubleBufferListView()
            {
                SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
                UpdateStyles();

            }
        }
    
}
