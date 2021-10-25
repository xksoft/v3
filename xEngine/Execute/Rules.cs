#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using xEngine.Common;
using xEngine.Model.Execute.Rules;
using xEngine.Plugin.HtmlParsing;

#endregion

namespace xEngine.Execute
{
    /// <summary>
    ///     提取规则执行器
    /// </summary>
    public class Rules
    {
        /// <summary>
        /// </summary>
        /// <param name="rules"></param>
        /// <returns></returns>
        public List<string> RunRule(RulesChild rules)
        {
            return RunRule(rules.Rulesstr, rules.Method, rules.AttributeName);
        }

        /// <summary>
        ///     执行提取规则
        /// </summary>
        /// <param name="rulesmain"></param>
        /// <returns>返回字符串数组</returns>
        public List<string> RunRules(RulesMain rulesmain)
        {
            var result = new List<string>();
            try
            {
                //判断是提取文本还是链接
                if (rulesmain.IsGetText)
                {
                    foreach (var results in rulesmain.Rules.Select(RunRule))
                    {
                        result.AddRange(results.Where(t => t != null));
                    }
                }
                else
                {
                    var nodes = Htmldoc.DocumentNode.SelectNodes(@"//a[@href]");
                    if (nodes != null)
                    {
                        foreach (
                            var urls in
                                nodes.Select(t => HttpHelper.GetFullUrl(t.Attributes["href"].Value, _url))
                                    .Where(urls => !result.Contains(urls) && urls != null))
                        {
                            result.Add(urls);
                        }
                    }
                }
                var remove = new List<string>();
                //判断必须包含
                if (rulesmain.MustHaveStr.Length > 0)
                {
                    var r = new Regex(rulesmain.MustHaveStr);
                    remove.AddRange(result.Where(t => !r.Match(t).Success));
                }
                foreach (var t in remove)
                {
                    result.Remove(t);
                }
                remove.Clear();
                //判断必须不包含
                if (rulesmain.MustNoHaveStr.Length > 0)
                {
                    var r = new Regex(rulesmain.MustNoHaveStr);
                    remove.AddRange(result.Where(t => r.Match(t).Success));
                }
                foreach (var t in remove)
                {
                    result.Remove(t);
                }
                //结果头部插入
                if (rulesmain.InsertHeadStr.Length > 0)
                {
                    for (var i = 0; i < result.Count; i++)
                    {
                        result[i] = rulesmain.InsertHeadStr + result[i];
                    }
                }
                //结果尾部插入
                if (rulesmain.InsertFootStr.Length > 0)
                {
                    for (var i = 0; i < result.Count; i++)
                    {
                        result[i] = result[i] + rulesmain.InsertFootStr;
                    }
                }

                //处理不同的输出模式
                switch (rulesmain.OutModel)
                {
                    case 1:
                    {
                        var sb = new StringBuilder();
                        for (var i = 0; i < result.Count; i++)
                        {
                            if (i != result.Count - 1)
                            {
                                sb.Append(result[i] + rulesmain.MergerStr);
                            }
                            else
                            {
                                sb.Append(result[i]);
                            }
                        }
                        result.Clear();
                        result.Add(sb.ToString());
                    }
                        break;
                    case 3:
                    {
                        string[] str = {""};
                        foreach (var t in result.Where(t => t.Length >= str[0].Length))
                        {
                            str[0] = t;
                        }
                        result.Clear();
                        result.Add(str[0]);
                    }
                        break;
                }
            }
            catch (Exception ex)
            {
                result.Clear();
                result.Add("提取出错，原因：" + ex.Message);
            }
            return result;
        }

        public List<string> RunRule(string rule, GetMethod method = GetMethod.GetOuter, string attributeName = "")
        {
            var result = new List<string>();
            if (method == GetMethod.Regex)
            {
                result = StrHelper.GetRegex(Htmldoc.HtmlCode, rule);
            }
            else
            {
                HtmlNodeCollection nodes;
                try
                {
                    nodes = Htmldoc.DocumentNode.SelectNodes(rule);
                    if (nodes == null)
                        return result;
                }
                catch (Exception ex)
                {
                    result.Add(ex.Message);
                    return result;
                }
                foreach (var t in nodes)
                {
                    switch (method)
                    {
                        case GetMethod.GetInner:
                            if (!result.Contains(t.InnerHtml))
                            {
                                result.Add(t.InnerHtml);
                            }
                            break;
                        case GetMethod.GetOuter:
                            if (!result.Contains(t.OuterHtml))
                            {
                                result.Add(t.OuterHtml);
                            }
                            break;
                        case GetMethod.GetAttribute:
                            try
                            {
                                if (!result.Contains(t.Attributes[attributeName].Value))
                                {
                                    result.Add(t.Attributes[attributeName].Value);
                                }
                            }
                            catch
                            {
                            }
                            break;
                    }
                }
            }
            return result;
        }

        #region 属性

        private readonly string _htmlcode;

        private readonly string _url;

        /// <summary>
        /// </summary>
        public HtmlDocument Htmldoc;

        /// <summary>
        ///     提取引擎
        /// </summary>
        /// <param name="htmlcode">网页源码</param>
        /// <param name="url">目标地址</param>
        public Rules(string htmlcode, string url)
        {
            _htmlcode = htmlcode;
            _url = url;
            Htmldoc = new HtmlDocument();
            Htmldoc.LoadHtml(_htmlcode);
        }

        #endregion
    }
}