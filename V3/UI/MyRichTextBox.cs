using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace xEngine.UI.MyControl
{

    public partial class MyRichTextBox : RichTextBox
    {
        public MyRichTextBox()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | //不擦除背景 ,减少闪烁
                            ControlStyles.OptimizedDoubleBuffer, //使用自定义的重绘事件,减少闪烁
                            true);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
        public const int WM_SETREDRAW = 0x000b;
        public void SetLockWindowUpdate(bool fls)
        {
            if (fls)
            {
                SendMessage((IntPtr)Handle, WM_SETREDRAW, (IntPtr)0, (IntPtr)0);
            }
            else
            {
                SendMessage((IntPtr)Handle, WM_SETREDRAW, (IntPtr)1, (IntPtr)0);
                Refresh();
            }
        }
        public void SetLockWindowUpdate1(bool fls)
        {
            if (fls)
            {
                SendMessage((IntPtr)Handle, WM_SETREDRAW, (IntPtr)0, (IntPtr)0);
            }
            else
            {
                SendMessage((IntPtr)Handle, WM_SETREDRAW, (IntPtr)1, (IntPtr)0);
            }
        }
    }

}