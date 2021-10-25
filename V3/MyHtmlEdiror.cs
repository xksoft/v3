using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace V3
{
    public partial class MyHtmlEdiror : UserControl
    {
      public MyHtmlEdiror()
        {
            InitializeComponent();
        }
        public string BodyHtml
        {
            get 
            {
                return myRichTextBox1.Text;
            }
            set
            {
                myRichTextBox1.Text = value;
            }
        }

 
     
    }
}
