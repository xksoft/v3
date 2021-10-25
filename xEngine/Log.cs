#region

using System;
using xEngine.Common;

#endregion

namespace xEngine
{
    /// <summary>
    ///     日志记录、输出类
    /// </summary>
    public class Log
    {
        private static readonly object Obj = new object();

        private static readonly char[] Cs =
        {
            'ａ', 'ｂ', 'ｃ', 'ｄ', 'ｅ', 'ｆ', 'ｇ', 'ｈ', 'ｉ', 'ｊ', 'ｋ', 'ｌ', 'ｍ', 'ｎ', 'ｏ',
            'ｐ'
        };

        /// <summary>
        ///     设置当前程序的控制台标题
        /// </summary>
        public static string Title
        {
            get
            {
                try
                {
                    return Console.Title;
                }
                catch
                {
                    return "";
                }
            }
            set { Console.Title = value; }
        }

        /// <summary>
        /// </summary>
        /// <param name="str"></param>
        public static void WriteLine(string str)
        {
            lock (Obj)
            {
                str =
                    str.Replace("[c1]", "ａ")
                        .Replace("[c2]", "ｂ")
                        .Replace("[c3]", "ｃ")
                        .Replace("[c4]", "ｄ")
                        .Replace("[c5]", "ｅ")
                        .Replace("[c6]", "ｆ")
                        .Replace("[c7]", "ｇ")
                        .Replace("[c8]", "ｈ")
                        .Replace("[c9]", "ｉ")
                        .Replace("[c10]", "ｊ")
                        .Replace("[c11]", "ｋ")
                        .Replace("[c12]", "ｌ")
                        .Replace("[c13]", "ｍ")
                        .Replace("[c14]", "ｎ")
                        .Replace("[c15]", "ｏ")
                        .Replace("[/c]", "ｐ");
                var colorindex = 0;
                var index = 0;
                Console.ForegroundColor = ConsoleColor.Gray;
                while (colorindex != -1)
                {
                    colorindex = str.IndexOfAny(Cs, colorindex);

                    if (colorindex > -1)
                    {
                        Console.Write(str.Substring(index, colorindex - index));
                        index = colorindex;
                        var color = str[colorindex];

                        #region 颜色设置

                        switch (color)
                        {
                            case 'ａ':
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            case 'ｂ':
                                Console.ForegroundColor = ConsoleColor.Blue;
                                break;
                            case 'ｃ':
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                break;
                            case 'ｄ':
                                Console.ForegroundColor = ConsoleColor.DarkBlue;
                                break;
                            case 'ｅ':
                                Console.ForegroundColor = ConsoleColor.DarkCyan;
                                break;
                            case 'ｆ':
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                break;
                            case 'ｇ':
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                break;
                            case 'ｈ':
                                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                                break;
                            case 'ｉ':
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                break;
                            case 'ｊ':
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                break;
                            case 'ｋ':
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;
                            case 'ｌ':
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;
                            case 'ｍ':
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                break;
                            case 'ｎ':
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                            case 'ｏ':
                                Console.ForegroundColor = ConsoleColor.Black;
                                break;
                            case 'ｐ':
                                Console.ForegroundColor = ConsoleColor.Gray;
                                break;
                            default:
                                Console.ForegroundColor = ConsoleColor.Gray;
                                break;
                        }

                        #endregion

                        colorindex++;
                        index++;
                    }
                    else
                    {
                        Console.Write(str.Substring(index, str.Length - (index)));
                    }
                }

                Console.WriteLine();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void WriteLine(string format, params object[] args)
        {
            WriteLine(string.Format(format, args));
        }

        /// <summary>
        /// </summary>
        /// <param name="str"></param>
        public static void WriteLineNoLock(string str)
        {
            str =
                str.Replace("[c1]", "ａ")
                    .Replace("[c2]", "ｂ")
                    .Replace("[c3]", "ｃ")
                    .Replace("[c4]", "ｄ")
                    .Replace("[c5]", "ｅ")
                    .Replace("[c6]", "ｆ")
                    .Replace("[c7]", "ｇ")
                    .Replace("[c8]", "ｈ")
                    .Replace("[c9]", "ｉ")
                    .Replace("[c10]", "ｊ")
                    .Replace("[c11]", "ｋ")
                    .Replace("[c12]", "ｌ")
                    .Replace("[c13]", "ｍ")
                    .Replace("[c14]", "ｎ")
                    .Replace("[c15]", "ｏ")
                    .Replace("[/c]", "ｐ");
            var colorindex = 0;
            var index = 0;
            Console.ForegroundColor = ConsoleColor.Gray;
            while (colorindex != -1)
            {
                colorindex = str.IndexOfAny(Cs, colorindex);

                if (colorindex > -1)
                {
                    Console.Write(str.Substring(index, colorindex - index));
                    index = colorindex;
                    var color = str[colorindex];

                    #region 颜色设置

                    switch (color)
                    {
                        case 'ａ':
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                        case 'ｂ':
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;
                        case 'ｃ':
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            break;
                        case 'ｄ':
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            break;
                        case 'ｅ':
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            break;
                        case 'ｆ':
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            break;
                        case 'ｇ':
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            break;
                        case 'ｈ':
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            break;
                        case 'ｉ':
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            break;
                        case 'ｊ':
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            break;
                        case 'ｋ':
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                        case 'ｌ':
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                        case 'ｍ':
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            break;
                        case 'ｎ':
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        case 'ｏ':
                            Console.ForegroundColor = ConsoleColor.Black;
                            break;
                        case 'ｐ':
                            Console.ForegroundColor = ConsoleColor.Gray;
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Gray;
                            break;
                    }

                    #endregion

                    colorindex++;
                    index++;
                }
                else
                {
                    Console.Write(str.Substring(index, str.Length - (index)));
                }
            }

            Console.WriteLine();
        }

        public static void Write(string format, params object[] args)
        {
            Write(string.Format(format, args));
        }

        /// <summary>
        /// </summary>
        /// <param name="str"></param>
        public static void Write(string str)
        {
            lock (Obj)
            {
                str =
                    str.Replace("[c1]", "ａ")
                        .Replace("[c2]", "ｂ")
                        .Replace("[c3]", "ｃ")
                        .Replace("[c4]", "ｄ")
                        .Replace("[c5]", "ｅ")
                        .Replace("[c6]", "ｆ")
                        .Replace("[c7]", "ｇ")
                        .Replace("[c8]", "ｈ")
                        .Replace("[c9]", "ｉ")
                        .Replace("[c10]", "ｊ")
                        .Replace("[c11]", "ｋ")
                        .Replace("[c12]", "ｌ")
                        .Replace("[c13]", "ｍ")
                        .Replace("[c14]", "ｎ")
                        .Replace("[c15]", "ｏ")
                        .Replace("[/c]", "ｐ");
                var colorindex = 0;
                var index = 0;
                Console.ForegroundColor = ConsoleColor.Gray;
                while (colorindex != -1)
                {
                    colorindex = str.IndexOfAny(Cs, colorindex);

                    if (colorindex > -1)
                    {
                        Console.Write(str.Substring(index, colorindex - index));
                        index = colorindex;
                        var color = str[colorindex];

                        #region 颜色设置

                        switch (color)
                        {
                            case 'ａ':
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            case 'ｂ':
                                Console.ForegroundColor = ConsoleColor.Blue;
                                break;
                            case 'ｃ':
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                break;
                            case 'ｄ':
                                Console.ForegroundColor = ConsoleColor.DarkBlue;
                                break;
                            case 'ｅ':
                                Console.ForegroundColor = ConsoleColor.DarkCyan;
                                break;
                            case 'ｆ':
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                break;
                            case 'ｇ':
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                break;
                            case 'ｈ':
                                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                                break;
                            case 'ｉ':
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                break;
                            case 'ｊ':
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                break;
                            case 'ｋ':
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;
                            case 'ｌ':
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;
                            case 'ｍ':
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                break;
                            case 'ｎ':
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                            case 'ｏ':
                                Console.ForegroundColor = ConsoleColor.Black;
                                break;
                            case 'ｐ':
                                Console.ForegroundColor = ConsoleColor.Gray;
                                break;
                            default:
                                Console.ForegroundColor = ConsoleColor.Gray;
                                break;
                        }

                        #endregion

                        colorindex++;
                        index++;
                    }
                    else
                    {
                        Console.Write(str.Substring(index, str.Length - (index)));
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        public static void ShowConsole()
        {
            XConsole.StartForm(Title, true);
        }

        /// <summary>
        /// </summary>
        public static void CloseConsole()
        {
            XConsole.StartForm(Title, false);
        }
    }
}