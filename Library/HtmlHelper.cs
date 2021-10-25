using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Library
{
    public class HtmlHelper
    {
        public static string GetHtmlFromByte(byte[] b, string ContentType)
        {
            if (b == null) { return ""; }
            Encoding encoding = Encoding.UTF8;
            string DefaultStr = Encoding.Default.GetString(b).ToLower();
            if (b == null || b.Length == 0) return "";
            //从这里开始我们要无视编码了
            if (ContentType == null)
            {


                Match meta = Regex.Match(DefaultStr, "<meta.+?charset=(?<enc>(\"|'|\\S+|).*?(\"|\'|\\S+|))", RegexOptions.IgnoreCase);
                string c = string.Empty;
                if (meta != null && meta.Groups.Count > 0)
                {
                    c = meta.Groups["enc"].Value.ToLower().Trim().Replace("\"", "").Replace("\'", "").Replace(">", ""); ;
                }
                if (c.Length > 2)
                {
                    try
                    {
                        encoding = Encoding.GetEncoding(c.Replace("\"", string.Empty).Replace("'", "").Replace(";", "").Replace("iso-8859-1", "gbk").Trim());
                    }
                    catch
                    {
                        if (c.Contains("gb2312") || c.Contains("gbk"))
                        {
                            encoding = Encoding.GetEncoding("gbk");
                        }
                        else
                        {
                            encoding = Encoding.UTF8;
                        }
                    }
                }
                else
                {
                    if (DefaultStr.Contains("gb2312") || DefaultStr.Contains("gbk"))
                    {
                        encoding = Encoding.GetEncoding("gbk");
                    }
                    else
                    {
                        encoding = Encoding.UTF8;
                    }
                }
            }
            else if (ContentType.ToLower().Contains("utf-8") || ContentType.ToLower().Contains("utf8"))
            {
                encoding = Encoding.UTF8;
            }
            else if (ContentType.ToLower().Contains("gbk") || ContentType.ToLower().Contains("gb2312"))
            {
                encoding = Encoding.GetEncoding("gbk");
            }
            else
            {
                Match meta = Regex.Match(DefaultStr, "<meta.+?charset=(?<enc>(\"|'|\\S+|).*?(\"|\'|\\S+|))", RegexOptions.IgnoreCase);
                string c = string.Empty;
                if (meta != null && meta.Groups.Count > 0)
                {
                    c = meta.Groups["enc"].Value.ToLower().Trim().Replace("\"", "").Replace("\'", "").Replace(">", "");
                }
                if (c.Length > 2)
                {
                    try
                    {
                        encoding = Encoding.GetEncoding(c.Replace("\"", string.Empty).Replace("'", "").Replace(";", "").Replace("iso-8859-1", "gbk").Trim());
                    }
                    catch
                    {
                        if (c.Contains("gb2312") || c.Contains("gbk"))
                        {
                            encoding = Encoding.GetEncoding("gbk");
                        }
                        else
                        {
                            encoding = Encoding.UTF8;
                        }
                    }
                }
                else
                {
                    //直接判断html中的编码
                    if (DefaultStr.Contains("gb2312") || DefaultStr.Contains("gbk"))
                    {
                        encoding = Encoding.GetEncoding("gbk");
                    }
                    else
                    {
                        if (Encoding.UTF8.GetString(b).Contains("�")) { encoding = Encoding.GetEncoding("gbk"); }
                        else
                        {
                            encoding = Encoding.UTF8;
                        }
                    }
                }
            }
            return encoding.GetString(b);
        }

       

        /// <summary>
        /// 文章正文数据模型
        /// </summary>
        public class Article
        {
            /// <summary>
            /// 文章标题
            /// </summary>
            public string Title { get; set; }
            /// <summary>
            /// 正文文本
            /// </summary>
            public string Content { get; set; }
            /// <summary>
            /// 带标签正文
            /// </summary>
            public string ContentWithTags { get; set; }

        }

        /// <summary>
        /// 解析Html页面的文章正文内容,基于文本密度的HTML正文提取类
        /// Date:   2012/12/30
        /// Update: 
        ///     2013/7/10   优化文章头部分析算法，优化
        ///     2014/4/25   添加Html代码中注释过滤的正则
        ///         
        /// </summary>
        public class HtmlToArticle
        {
            #region 参数设置

            // 正则表达式过滤：正则表达式，要替换成的文本
            private static readonly string[][] Filters =
        {
            new[] { @"(?is)<script.*?>.*?</script>", "" },
            new[] { @"(?is)<style.*?>.*?</style>", "" },
            new[] { @"(?is)<!--.*?-->", "" },    // 过滤Html代码中的注释
            // 针对链接密集型的网站的处理，主要是门户类的网站，降低链接干扰
            new[] { @"(?is)</a>", "</a>\n"}                 
        };

            private static bool _appendMode = false;
            /// <summary>
            /// 是否使用追加模式，默认为false
            /// 使用追加模式后，会将符合过滤条件的所有文本提取出来
            /// </summary>
            public static bool AppendMode
            {
                get { return _appendMode; }
                set { _appendMode = value; }
            }

            private static int _depth = 6;
            /// <summary>
            /// 按行分析的深度，默认为6
            /// </summary>
            public static int Depth
            {
                get { return _depth; }
                set { _depth = value; }
            }

            private static int _limitCount = 180;
            /// <summary>
            /// 字符限定数，当分析的文本数量达到限定数则认为进入正文内容
            /// 默认180个字符数
            /// </summary>
            public static int LimitCount
            {
                get { return _limitCount; }
                set { _limitCount = value; }
            }

            // 确定文章正文头部时，向上查找，连续的空行到达_headEmptyLines，则停止查找
            private static int _headEmptyLines = 2;
            // 用于确定文章结束的字符数
            private static int _endLimitCharCount = 20;

            #endregion

            /// <summary>
            /// 从给定的Html原始文本中获取正文信息
            /// </summary>
            /// <param name="html"></param>
            /// <returns></returns>
            public static Article GetArticle(string html)
            {
                // 如果换行符的数量小于10，则认为html为压缩后的html
                // 由于处理算法是按照行进行处理，需要为html标签添加换行符，便于处理
                if (html.Count(c => c == '\n') < 10)
                {
                    html = html.Replace(">", ">\n");
                }

                // 获取html，body标签内容
                string body = "";
                string bodyFilter = @"(?is)<body.*?</body>";
                Match m = Regex.Match(html, bodyFilter);
                if (m.Success)
                {
                    body = m.ToString();
                }
                // 过滤样式，脚本等不相干标签
                foreach (var filter in Filters)
                {
                    body = Regex.Replace(body, filter[0], filter[1]);
                }
                // 标签规整化处理，将标签属性格式化处理到同一行
                // 处理形如以下的标签：
                //  <a 
                //   href='http://www.baidu.com'
                //   class='test'
                // 处理后为
                //  <a href='http://www.baidu.com' class='test'>
                body = Regex.Replace(body, @"(<[^<>]+)\s*\n\s*", FormatTag);

                string content;
                string contentWithTags;
                GetContent(body, out content, out contentWithTags);

                Article article = new Article
                {
                    Title = GetTitle(html),
                    Content = content,
                    ContentWithTags = contentWithTags
                };

                return article;
            }

            /// <summary>
            /// 格式化标签，剔除匹配标签中的回车符
            /// </summary>
            /// <param name="match"></param>
            /// <returns></returns>
            private static string FormatTag(Match match)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var ch in match.Value)
                {
                    if (ch == '\r' || ch == '\n')
                    {
                        continue;
                    }
                    sb.Append(ch);
                }
                return sb.ToString();
            }

            /// <summary>
            /// 获取时间
            /// </summary>
            /// <param name="html"></param>
            /// <returns></returns>
            private static string GetTitle(string html)
            {
                string titleFilter = @"<title>[\s\S]*?</title>";
                //string h1Filter = @"<h\d+.*?>[\s\S].*?[\s\S]</h\d+>";
                string h1Filter = @"<.*?>[\s\S]*?</.*?>";
                string clearFilter = @"<.*?>";

                string title = "";
                Match match = Regex.Match(html, titleFilter, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    title = Regex.Replace(match.Groups[0].Value, clearFilter, "");
                }

                // 正文的标题一般在h1中，比title中的标题更干净
                MatchCollection mc = Regex.Matches(html, h1Filter, RegexOptions.IgnoreCase);
                if (mc.Count > 0)
                {
                    for (int i = 0; i < mc.Count; i++)
                    {
                        if (!mc[i].Value.Contains("<title>"))
                        {
                            string h1 = Regex.Replace(mc[i].Value, clearFilter, "");
                            h1 = h1.Replace("\r", "").Replace("\n", "").Trim();
                            if (!String.IsNullOrEmpty(h1) && title.StartsWith(h1))
                            {
                                title = h1;
                            }
                        }

                    }

                }
                return title;
            }



            /// <summary>
            /// 从body标签文本中分析正文内容
            /// </summary>
            /// <param name="bodyText">只过滤了script和style标签的body文本内容</param>
            /// <param name="content">返回文本正文，不包含标签</param>
            /// <param name="contentWithTags">返回文本正文包含标签</param>
            private static void GetContent(string bodyText, out string content, out string contentWithTags)
            {
                string[] orgLines = null;   // 保存原始内容，按行存储
                string[] lines = null;      // 保存干净的文本内容，不包含标签

                orgLines = bodyText.Split('\n');
                lines = new string[orgLines.Length];
                // 去除每行的空白字符,剔除标签
                for (int i = 0; i < orgLines.Length; i++)
                {
                    string lineInfo = orgLines[i];
                    // 处理回车，使用[crlf]做为回车标记符，最后统一处理
                    lineInfo = Regex.Replace(lineInfo, "(?is)</p>|<br.*?/>", "[crlf]");
                    lines[i] = Regex.Replace(lineInfo, "(?is)<.*?>", "").Trim();
                }

                StringBuilder sb = new StringBuilder();
                StringBuilder orgSb = new StringBuilder();

                StringBuilder sbDebug = new StringBuilder();
                for (int i = 0; i < lines.Length; i++)
                {
                    sbDebug.Append(i);
                    sbDebug.Append(',');
                    sbDebug.Append(lines[i].Length);
                    sbDebug.Append("\r\n");
                }
                //File.WriteAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.txt"), lines, Encoding.Default);
                //File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "stat.csv"), sbDebug.ToString());

                int preTextLen = 0;         // 记录上一次统计的字符数量
                int startPos = -1;          // 记录文章正文的起始位置
                for (int i = 0; i < lines.Length - _depth; i++)
                {
                    int len = 0;
                    for (int j = 0; j < _depth; j++)
                    {
                        len += lines[i + j].Length;
                    }

                    if (startPos == -1)     // 还没有找到文章起始位置，需要判断起始位置
                    {
                        if (preTextLen > _limitCount && len > 0)    // 如果上次查找的文本数量超过了限定字数，且当前行数字符数不为0，则认为是开始位置
                        {
                            // 查找文章起始位置, 如果向上查找，发现2行连续的空行则认为是头部
                            int emptyCount = 0;
                            for (int j = i - 1; j > 0; j--)
                            {
                                if (String.IsNullOrEmpty(lines[j]))
                                {
                                    emptyCount++;
                                }
                                else
                                {
                                    emptyCount = 0;
                                }
                                if (emptyCount == _headEmptyLines)
                                {
                                    startPos = j + _headEmptyLines;
                                    break;
                                }
                            }
                            // 如果没有定位到文章头，则以当前查找位置作为文章头
                            if (startPos == -1)
                            {
                                startPos = i;
                            }
                            // 填充发现的文章起始部分
                            for (int j = startPos; j <= i; j++)
                            {
                                sb.Append(lines[j]);
                                orgSb.Append(orgLines[j]);
                            }
                        }
                    }
                    else
                    {
                        //if (len == 0 && preTextLen == 0)    // 当前长度为0，且上一个长度也为0，则认为已经结束
                        if (len <= _endLimitCharCount && preTextLen < _endLimitCharCount)    // 当前长度为0，且上一个长度也为0，则认为已经结束
                        {
                            if (!_appendMode)
                            {
                                break;
                            }
                            startPos = -1;
                        }
                        sb.Append(lines[i]);
                        orgSb.Append(orgLines[i]);
                    }
                    preTextLen = len;
                }

                string result = sb.ToString();
                // 处理回车符，更好的将文本格式化输出
                content = result.Replace("[crlf]", Environment.NewLine);
                content = System.Web.HttpUtility.HtmlDecode(content);
                // 输出带标签文本
                contentWithTags = orgSb.ToString();
            }
        }
    }
}
