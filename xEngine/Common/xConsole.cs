#region

using System;
using System.Runtime.InteropServices;

#endregion

namespace xEngine.Common
{
    internal class XConsole
    {
        internal static bool MIsInitialized;
        internal static IntPtr MOutputHandle = IntPtr.Zero;

        internal static void StartForm(string title, bool isopen)
        {
            lock ("inits")
            {
                if (isopen)
                {
                    if (!MIsInitialized)
                    {
                        try
                        {
                            MIsInitialized = AllocConsole();

                            MOutputHandle = GetStdHandle(StdOutputHandle);
                            SetConsoleTitleW(title);

                            var wndHandle = GetConsoleWindow();
                            var menuHandle = GetSystemMenu(wndHandle, false);
                            RemoveMenu(menuHandle, ScClose, MfBycommand);
                            Console.TreatControlCAsInput = true;
                        }
                        catch
                        {
                        }
                    }
                }
                else
                {
                    MIsInitialized = false;
                    FreeConsole();
                }
            }
        }

        #region DllImport

        private const int StdOutputHandle = -11;
        private const uint ScClose = 0x0000F060;
        private const uint MfBycommand = 0x00000000;


        [DllImport("kernel32.dll", SetLastError = false)]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = false, ExactSpelling = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("user32.dll", SetLastError = false, ExactSpelling = true)]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", SetLastError = false, ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll", SetLastError = false, ExactSpelling = true)]
        private static extern bool RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = false, ExactSpelling = true)]
        private static extern bool SetConsoleTitleW(string lpConsoleTitle);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        private static extern bool FreeConsole();

        #endregion
    }
}