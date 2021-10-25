#region

using System;
using System.Text;
using xEngine.Execute;
using xEngine.Model;
using xEngine.Model.Execute.Http;

#endregion

namespace xEngine.Common
{
    /// <summary>
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// </summary>
        /// <param name="url"></param>
        /// <param name="proxy"></param>
        /// <returns></returns>
        public static string GetHtml(string url, string proxy)
        {
            var execute = new Http
            {
                IsDebug = true
            };
            var request = new XRequest
            {
                Url = url
            };
            execute.IsAutoEncoding = true;
            var result = execute.RunRequest(request, "", proxy).BodyString;
            return result;
        }

        /// <summary>
        ///     请求网页源码(智能判断编码)
        /// </summary>
        /// <param name="url">地址</param>
        /// <returns></returns>
        public static string GetHtml(string url)
        {
            var execute = new Http
            {
                IsDebug = true
            };
            var request = new XRequest
            {
                Url = url
            };
            execute.IsAutoEncoding = true;
            var result = execute.RunRequest(request, "").BodyString;
            return result;
        }
        public static XResponse GetResponse(string url)
        {
            var execute = new Http
            {
                IsDebug = true
            };
            var request = new XRequest
            {
                Url = url
            };
            execute.IsAutoEncoding = true;
            return execute.RunRequest(request, "");

        }

        /// <summary>
        ///     请求网页源码
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="encoding">编码 1gbk 2utf8 3自动</param>
        /// <returns></returns>
        public static string GetHtml(string url, int encoding)
        {
            var execute = new Http();
            var request = new XRequest
            {
                Url = url
            };
            switch (encoding)
            {
                case 1:
                    request.Encoding = Encoding.Default.WebName;
                    break;
                case 2:
                    request.Encoding = Encoding.UTF8.WebName;
                    break;
                default:
                    execute.IsAutoEncoding = true;
                    break;
            }
            var result = execute.RunRequest(request, "").BodyString;
            return result;
        }

        /// <summary>
        ///     请求网页源码
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="encoding">编码 1gbk 2utf8 3自动</param>
        /// <param name="hostip">host ip</param>
        /// <returns></returns>
        public static string GetHtml(string url, int encoding, string hostip)
        {
            var execute = new Http();
            var request = new XRequest
            {
                Url = url
            };
            switch (encoding)
            {
                case 1:
                    request.Encoding = Encoding.Default.WebName;
                    break;
                case 2:
                    request.Encoding = Encoding.UTF8.WebName;
                    break;
                default:
                    execute.IsAutoEncoding = true;
                    break;
            }

            var result = execute.RunRequest(request, "", null, hostip).BodyString;
            return result;
        }

        public static byte[] GetData(string url, string hostip = null)
        {
            var execute = new Http();
            var request = new XRequest
            {
                Url = url
            };

            return execute.RunRequest(request, "", null, hostip).BodyData;
        }

        /// <summary>
        ///     请求网页源码
        /// </summary>
        /// <param name="cmd">命令块</param>
        /// <param name="encoding">编码</param>
        /// <param name="arg">参数</param>
        /// <returns></returns>
        public static Command GetHtml(Command cmd, string encoding, string arg)
        {
            if (encoding == null && arg == null)
            {
                if (License.Sessionid != null)
                    cmd.Sessionid = License.Sessionid;
                return SdkHelper.RunCommand(cmd);
            }
            if (encoding != null && arg == null)
            {
                return SdkHelper.RunCommand(cmd, encoding);
            }
            return new Command{Message = "参数不对啊"};
        }

        #region Http请求相关

        /// <summary>
        /// </summary>
        /// <param name="thisurl"></param>
        /// <param name="oldurl"></param>
        /// <returns></returns>
        public static string GetFullUrl(string thisurl, string oldurl)
        {
            if (thisurl.ToLower().StartsWith("javascript") && thisurl.Contains(":"))
                return thisurl;
            if (thisurl.ToLower().StartsWith("http://") || thisurl.ToLower().StartsWith("https://"))
                return thisurl;
            if (thisurl == "")
                return oldurl;
            string url;
            var olduri = new Uri(oldurl);
            if (thisurl.StartsWith("/"))
            {
                if (olduri.Port != 80)
                    url = (oldurl.ToLower().StartsWith("https") ? "https://" : "http://") + olduri.Host + ":" +
                          olduri.Port + thisurl;
                else
                    url = (oldurl.ToLower().StartsWith("https") ? "https://" : "http://") + olduri.Host + thisurl;
            }
            else
            {
                var path = olduri.PathAndQuery;
                var index = path.LastIndexOf("/", StringComparison.Ordinal);
                if (olduri.Port != 80)
                    url = (oldurl.ToLower().StartsWith("https") ? "https://" : "http://") + olduri.Host + ":" +
                          olduri.Port + path.Substring(0, index + 1) + thisurl;
                else
                    url = (oldurl.ToLower().StartsWith("https") ? "https://" : "http://") + olduri.Host +
                          path.Substring(0, index + 1) + thisurl;
            }
            return url;
        }

        #endregion
    }
}