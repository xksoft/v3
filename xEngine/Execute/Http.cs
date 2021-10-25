#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using xEngine.Common;
using xEngine.Model;
using xEngine.Model.Execute.Http;
using xEngine.Plugin.HtmlParsing;

#endregion

namespace xEngine.Execute
{
    /// <summary>
    ///     HTTP脚本执行器
    /// </summary>
    public class Http
    {
        #region 属性

        /// <summary>
        ///     是否重定向
        /// </summary>
        public bool IsRedirect = true;

        private readonly Random _random = new Random();

        /// <summary>
        ///     账号模型
        /// </summary>
        public XAccount Account = new XAccount();

        /// <summary>
        ///     二进制数据
        /// </summary>
        public ByteData ByteData = new ByteData();

        /// <summary>
        ///     xCookie管理容器
        /// </summary>
        public XCookieManager CookieManager = new XCookieManager();

        /// <summary>
        ///     是否使用自动识别内容编码
        /// </summary>
        public bool IsAutoEncoding;

        /// <summary>
        ///     是否使用自动识别验证码
        /// </summary>
        public bool IsAutoVerify;

        internal bool IsDebug;


        /// <summary>
        ///     本地绑定的IP
        /// </summary>
        public IPEndPoint LoaclEndPoin;

        /// <summary>
        ///     变量标签
        /// </summary>
        public XTag Tag = new XTag();


        /// <summary>
        ///     实例化组件
        /// </summary>
        public Http()
        {
            IsAutoVerify = false;
        }

        /// <summary>
        ///     所有脚本
        /// </summary>
        public List<XRequest> Scripts = new List<XRequest>();

        #endregion

        #region Cookie操作

        private void CookieAdd(XCookie xCookie)
        {
            lock ("xCookie")
            {
                if (CookieManager.Cookies.ContainsKey(xCookie.Name + xCookie.Path + xCookie.Domain))
                {
                    if (xCookie.Value.ToLower().Trim() != "deleted")
                    {
                        CookieManager.Cookies[xCookie.Name + xCookie.Path + xCookie.Domain] = xCookie;
                    }
                    else
                    {
                        CookieManager.Cookies.Remove(xCookie.Name + xCookie.Path + xCookie.Domain);
                    }
                }
                else
                {
                    if (xCookie.Value.ToLower().Trim() != "deleted")
                    {
                        CookieManager.Cookies.Add(xCookie.Name + xCookie.Path + xCookie.Domain, xCookie);
                    }
                }
            }
        }

        /// <summary>
        ///     获取文本形式的cookie
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public string GetCookieStr(Uri uri)
        {
            var result = "";
            lock ("xCookie")
            {
                if (CookieManager == null)
                    CookieManager = new XCookieManager();

                result = (from s in CookieManager.Cookies
                          let flag1 = uri.Host.Contains(s.Value.Domain) || ("." + uri.Host).Contains(s.Value.Domain)
                          let flag2 = uri.AbsolutePath.Contains(s.Value.Path)
                          where flag1 && flag2
                          select s).Aggregate(result, (current, s) => current + (s.Value.Name + "=" + s.Value.Value + ";"));
            }
            return result != "" ? result.Remove(result.Length - 1) : result;
        }
        public CookieContainer GetCookieCookieContainer(Uri uri)
        {
            var cookieContainer = new CookieContainer();
            lock ("xCookie")
            {
                if (CookieManager == null)
                    CookieManager = new XCookieManager();
                var result = (from s in CookieManager.Cookies
                              let flag1 = uri.Host.Contains(s.Value.Domain) || ("." + uri.Host).Contains(s.Value.Domain)
                              let flag2 = uri.PathAndQuery.Contains(s.Value.Path)
                              where flag1 && flag2
                              select s).ToArray();

                foreach (var kv in result)
                {
                    cookieContainer.Add(uri, new Cookie(kv.Value.Name, kv.Value.Value, kv.Value.Path));
                }
            }
            return cookieContainer;
        }

        /// <summary>
        ///     清空cookie
        /// </summary>
        public void CookieClear()
        {
            CookieManager.Cookies.Clear();
        }

        private string CookieGetKey(string key)
        {
            lock ("find")
            {
                foreach (var entry in CookieManager.Cookies.Where(entry => entry.Value.Name == key))
                {
                    return entry.Value.Value;
                }
            }
            return "";
        }

        /// <summary>
        ///     获取xCookie数量
        /// </summary>
        /// <returns></returns>
        public int CookieGetCount()
        {
            return CookieManager.Cookies.Count;
        }

        /// <summary>
        ///     返回所有xCookie值
        /// </summary>
        /// <returns></returns>
        public string CookieGetAllKeyValue()
        {
            var sb = new StringBuilder();
            sb.Append("\r\nCookie总数：" + CookieManager.Cookies.Count + "\r\n=======================================\r\n");
            var ids = new string[CookieManager.Cookies.Count];
            CookieManager.Cookies.Keys.CopyTo(ids, 0);
            foreach (var t in ids)
            {
                sb.Append("Name:" + CookieManager.Cookies[t].Name + " Value:" + CookieManager.Cookies[t].Value +
                          " Path:" + CookieManager.Cookies[t].Path + " Domain:" +
                          CookieManager.Cookies[t].Domain + "\r\n");
            }
            sb.Append("=======================================\r\n");
            return sb.ToString();
        }

        /// <summary>
        ///     单独设置xCookie
        /// </summary>
        public void CookieAddStr(string xCookiestr)
        {
            var strs = xCookiestr.Split(';');
            {
                foreach (var t in strs)
                {
                    var index = t.IndexOf('=');
                    try
                    {
                        var xCookie = new XCookie
                        {
                            Name = t.Substring(0, index).Trim(),
                            Value = t.Substring(index + 1, t.Length - index - 1),
                            Path = "/",
                            Domain = ""
                        };
                        CookieAdd(xCookie);
                    }
                    catch
                    {
                    }
                }
            }
        }

        //xCookie获取方法
        private static XCookie XCookieParser(string xCookiestr, Uri uri)
        {
            var co = new XCookie
            {
                Name = "woshiyigewudicookie"
            };
            var xCookiestrs = xCookiestr.Split(';');
            foreach (var result in xCookiestrs.Select(t => t.Split('=')).Where(result => result.Length > 1))
            {
                switch (result[0].ToLower().Trim())
                {
                    case "path":
                        co.Path = result[1].Trim();
                        break;
                    case "domain":
                        co.Domain = result[1].Trim();
                        break;
                    default:
                        if (result[0].ToLower().Trim() != "expires" && result[0].ToLower().Trim() != "version" &&
                            result[0].ToLower().Trim() != "max-age")
                        {
                            var value = "";
                            for (var ii = 1; ii < result.Length; ii++)
                            {
                                if (ii != result.Length - 1)
                                    value += result[ii] + "=";
                                else
                                    value += result[ii];
                            }
                            co.Name = result[0].Trim();
                            co.Value = value;
                        }
                        break;
                }
            }
            if (string.IsNullOrEmpty(co.Path))
                co.Path = "/";
            if (co.Path[co.Path.Length - 1] != '/')
                co.Path += "/";
            if (co.Domain == null)
                co.Domain = uri.Host;
            if (co.Path == null)
                co.Path = "/";
            return co.Name != "woshiyigewudicookie" ? co : null;
        }

        #endregion

        #region 私有方法

        private static byte[] ReadStreamToBytes(Stream stream)
        {
            var buffer = new byte[64 * 1024];
            int r;
            var l = 0;
            long position = -1;
            if (stream.CanSeek)
            {
                position = stream.Position;
                stream.Position = 0;
            }
            var ms = new MemoryStream();
            while (true)
            {
                r = stream.Read(buffer, 0, buffer.Length);
                if (r > 0)
                {
                    l += r;
                    ms.Write(buffer, 0, r);
                }
                else
                {
                    break;
                }
            }
            var bytes = new byte[l];
            ms.Position = 0;
            ms.Read(bytes, 0, l);
            ms.Close();
            if (position >= 0)
            {
                stream.Position = position;
            }
            return bytes;
        }

        private static byte[] Decompress(byte[] data, string encoding)
        {
            if (encoding != "")
            {
                if (encoding.Contains("gzip"))
                {
                    var ms = new MemoryStream(data);
                    var stream = new GZipStream(ms, CompressionMode.Decompress);
                    data = ReadStreamToBytes(stream);
                    ms.Close();
                    ms.Dispose();
                    stream.Close();
                    stream.Dispose();
                }
                else if (encoding.Contains("deflate"))
                {
                    var ms = new MemoryStream(data);
                    var stream = new DeflateStream(ms, CompressionMode.Decompress);
                    data = ReadStreamToBytes(stream);
                    ms.Close();
                    ms.Dispose();
                    stream.Close();
                    stream.Dispose();
                }
            }
            return data;
        }

        private static bool TryHexParse(string sInput, out int iOutput)
        {
            return int.TryParse(sInput, NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo, out iOutput);
        }

        private static byte[] DoUnchunk(byte[] writeData)
        {
            if ((writeData == null) || (writeData.Length == 0))
            {
                return new byte[0];
            }
            var stream = new MemoryStream(writeData.Length);
            var index = 0;
            var flag = false;
            while (!flag && (index <= (writeData.Length - 3)))
            {
                int num3;
                var sInput = Encoding.ASCII.GetString(writeData, index, Math.Min(0x40, writeData.Length - index));
                var length = sInput.IndexOf("\r\n", StringComparison.Ordinal);
                if (length <= 0)
                {
                    throw new InvalidDataException(
                        "HTTP Error: The chunked content is corrupt. Cannot find Chunk-Length in expected location. Offset: " +
                        index);
                }
                index += length + 2;
                sInput = sInput.Substring(0, length);
                length = sInput.IndexOf(';');
                if (length > 0)
                {
                    sInput = sInput.Substring(0, length);
                }
                if (!TryHexParse(sInput, out num3))
                {
                    throw new InvalidDataException(
                        "HTTP Error: The chunked content is corrupt. Chunk-Length was malformed." + index);
                }
                if (num3 == 0)
                {
                    flag = true;
                }
                else
                {
                    if (writeData.Length < (num3 + index))
                    {
                        throw new InvalidDataException(
                            "HTTP Error: The chunked entity body is corrupt. The final chunk length is greater than the number of bytes remaining.");
                    }
                    stream.Write(writeData, index, num3);
                    index += num3 + 2;
                }
            }
            if (!flag)
            {
                //FiddlerApplication.DoNotifyUser("Chunked body did not terminate properly with 0-sized chunk.", "HTTP Protocol Violation");       
            }
            var dst = new byte[stream.Length];
            Buffer.BlockCopy(stream.GetBuffer(), 0, dst, 0, dst.Length);
            return dst;
        }

        #endregion

        #region http请求方法

        private static byte[] GetBodyData(ref int contentlength, ref bool ischunked, ref string contentencoding,
            ref StringBuilder responseHeaderBuilder, XResponse response, List<string> setcookies, byte[] data)
        {
            var headdata = new byte[0];
            var bodydata = new byte[0];
            for (var i = 0; i < data.Length; i++)
            {
                if (data[i] == 13 && data[i + 1] == 10 & data[i + 2] == 13 & data[i + 3] == 10)
                {
                    headdata = new byte[i + 4];
                    Array.Copy(data, 0, headdata, 0, headdata.Length);
                    bodydata = new byte[data.Length - headdata.Length];
                    Array.Copy(data, headdata.Length, bodydata, 0, bodydata.Length);
                    break;
                }
            }

            if (headdata.Length > 0)
            {
                responseHeaderBuilder = new StringBuilder();

                var smReader = new StreamReader(new MemoryStream(headdata));
                string str;

                do
                {
                    str = smReader.ReadLine();
                    if (response.StatusCode == 0 && str.StartsWith("HTTP/"))
                    {
                        var status = str.Split(' ');
                        if (status.Length > 1)
                        {
                            int.TryParse(status[1], out response.StatusCode);
                        }
                    }
                    var kv = str.Split(':');
                    if (kv.Length > 2)
                    {
                        kv[1] = str.Substring(str.IndexOf(':') + 1).Trim();
                    }

                    switch (kv[0].ToLower())
                    {
                        case "content-length":
                            int.TryParse(kv[1].Trim(), out contentlength);
                            break;
                        case "transfer-encoding":
                            ischunked = true;
                            break;
                        case "content-encoding":
                            contentencoding = kv[1].Trim();
                            break;
                        case "set-cookie":
                            setcookies.Add(kv[1].Trim());
                            break;
                        case "content-type":
                            response.ContentType = kv[1].Trim();
                            break;
                        case "location":
                            response.Location = kv[1].Trim();
                            break;
                    }
                    responseHeaderBuilder.AppendLine(str);
                } while (str != "");
            }
            return bodydata;
        }


        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private static Stream SendData(XRequest xrequest, TcpClient client, Uri uri, MemoryStream requestMs)
        {
            Stream clientStream;
            if (xrequest.Url.StartsWith("https://"))
            {
                var ssl = new SslStream(client.GetStream(), false, ValidateServerCertificate, null);
                ssl.AuthenticateAsClient(uri.Host);
                clientStream = ssl;
            }
            else
            {
                clientStream = client.GetStream();
            }
            clientStream.Write(requestMs.ToArray(), 0, (int)requestMs.Length);
            clientStream.Flush();


            return clientStream;
        }

        private static StringBuilder ReceiveData(TcpClient client, Stream clientStream, XResponse response,
            out List<string> setcookies)
        {
            var responseBuffer = new byte[8192];
            var memStream = new MemoryStream();

            client.Client.NoDelay = true;


            var contentLength = -1;
            var ischunked = false;


            StringBuilder responseHeader = null;
            var contentencoding = "";
            setcookies = new List<string>();

            int bytesRead;

            do
            {
                bytesRead = clientStream.Read(responseBuffer, 0, responseBuffer.Length);
                if (responseHeader == null)
                {
                    var data = new byte[bytesRead];
                    Array.Copy(responseBuffer, 0, data, 0, bytesRead);
                    var bodydata = GetBodyData(ref contentLength, ref ischunked, ref contentencoding,
                        ref responseHeader,
                        response, setcookies, data);
                    response.ResponseHeader = responseHeader.ToString();
                    memStream.Write(bodydata, 0, bodydata.Length);
                }
                else
                {
                    memStream.Write(responseBuffer, 0, bytesRead);
                }

                if (ischunked)
                {
                    if (bytesRead >= 5)
                    {
                        if (responseBuffer[bytesRead - 5] == 48 && responseBuffer[bytesRead - 4] == 13 &&
                            responseBuffer[bytesRead - 3] == 10 && responseBuffer[bytesRead - 2] == 13 &&
                            responseBuffer[bytesRead - 1] == 10)
                        {
                            break;
                        }
                    }
                }
            } while (bytesRead > 0 && memStream.Length != contentLength);

            memStream.Position = 0;
            response.BodyData = ischunked ? DoUnchunk(memStream.ToArray()) : memStream.ToArray();
            memStream.Close();
            response.BodyData = Decompress(response.BodyData, contentencoding);


            return responseHeader;
        }

        private byte[] GetRequestBody(string useragent, string referer, int method, Encoding encoding, Uri uri,
            string cookiestr, MemoryStream requestMs,
            out StringBuilder requestHeader, List<KeyValuePair<string, string>> Header,
            List<KeyValuePair<string, string>> PostData, string PostString, bool isproxy)
        {
            //强名称校验
            Crc32.C32();

            var contenttype = "";
            var postBody = new byte[0];
            if (method > 0)
            {
                switch (method)
                {
                    case 1:
                        contenttype = "application/x-www-form-urlencoded; charset=" + encoding.WebName;
                        var sb1 = new StringBuilder();
                        for (var index = 0; index < PostData.Count; index++)
                        {
                            sb1.Append(PostData[index].Key + "=" +
                                       StrHelper.UrlEncode(PostData[index].Value,
                                           encoding));
                            if (index != PostData.Count - 1)
                                sb1.Append("&");
                        }

                        postBody = encoding.GetBytes(sb1.ToString());
                        break;
                    case 2:
                        var boundar = RandStr.GetEngStr(10, false);
                        var bundarystr = "----WebKitFormBoundary" + boundar;
                        var sb2 = new StringBuilder();
                        var ms = new MemoryStream();
                        foreach (var v in PostData)
                        {
                            sb2.Clear();
                            if (v.Value.StartsWith("{文件数据"))
                            {
                                sb2.AppendLine("--" + bundarystr);
                                var kv = v.Value.Split('=');
                                sb2.AppendLine("Content-Disposition: form-data; name=\"" + v.Key + "\"; filename=\"" +
                                               kv[1].Trim('}') + "\"");
                                sb2.AppendLine("Content-Type: " + MimeMapping.GetMimeMapping(kv[1].Trim('}')));
                                sb2.AppendLine();
                                var tempdata = encoding.GetBytes(sb2.ToString());
                                ms.Write(tempdata, 0, tempdata.Length);

                                var filedata = new byte[0];
                                switch (kv[0])
                                {
                                    case "{文件数据1":
                                        filedata = ByteData.Data1;
                                        break;
                                    case "{文件数据2":
                                        filedata = ByteData.Data2;
                                        break;
                                    case "{文件数据3":
                                        filedata = ByteData.Data1;
                                        break;
                                    case "{文件数据4":
                                        ms.Write(ByteData.Data1, 0, ByteData.Data4.Length);
                                        break;
                                    case "{文件数据5":
                                        ms.Write(ByteData.Data1, 0, ByteData.Data5.Length);
                                        break;
                                }
                                ms.Write(filedata, 0, filedata.Length);
                                sb2.Clear();
                                sb2.AppendLine();
                                tempdata = encoding.GetBytes(sb2.ToString());
                                ms.Write(tempdata, 0, tempdata.Length);
                            }
                            else
                            {
                                sb2.AppendLine("--" + bundarystr);
                                sb2.AppendLine("Content-Disposition: form-data; name=\"" + v.Key + "\"");
                                sb2.AppendLine();
                                sb2.AppendLine(v.Value);
                                var tempdata2 = encoding.GetBytes(sb2.ToString());
                                ms.Write(tempdata2, 0, tempdata2.Length);
                            }
                        }
                        sb2.Clear();
                        sb2.AppendLine("--" + bundarystr + "--");
                        var tempdata3 = encoding.GetBytes(sb2.ToString());
                        ms.Write(tempdata3, 0, tempdata3.Length);
                        postBody = ms.ToArray();
                        contenttype = "multipart/form-data; boundary=" + bundarystr;
                        break;
                    case 3:
                        if (PostString.StartsWith("{文件数据"))
                        {
                            var kv = PostString.Split('-');
                            switch (kv[0])
                            {
                                case "{文件数据1":
                                    postBody = ByteData.Data1;
                                    break;
                                case "{文件数据2":
                                    postBody = ByteData.Data2;
                                    break;
                                case "{文件数据3":
                                    postBody = ByteData.Data3;
                                    break;
                                case "{文件数据4":
                                    postBody = ByteData.Data4;
                                    break;
                                case "{文件数据5":
                                    postBody = ByteData.Data5;
                                    break;
                            }
                        }
                        else
                        {
                            postBody = encoding.GetBytes(PostString);
                        }
                        break;
                }
            }


            requestHeader = new StringBuilder();

            requestHeader.AppendLine(string.Format("{0} {1} HTTP/1.1", method == 0 ? "GET" : "POST",
                isproxy ? uri.AbsoluteUri : uri.PathAndQuery));
            requestHeader.AppendLine(string.Format("Host: {0}",
                uri.Host + ((uri.Port != 80 && uri.Port != 443) ? (":" + uri.Port) : "")));
            requestHeader.AppendLine("Connection: close");
            requestHeader.AppendLine("Accept-Encoding: gzip, deflate");


            if (useragent != "")
                requestHeader.AppendLine(string.Format("User-Agent: {0}", useragent));
            if (referer != "")
                requestHeader.AppendLine(string.Format("Referer: {0}", referer));


            if (postBody.Length > 0 || PostString.Length > 0)
            {
                requestHeader.AppendLine(string.Format("Content-Length: {0}", postBody.Length));
            }

            if (postBody.Length > 0 && contenttype != "")
            {
                requestHeader.AppendLine(string.Format("Content-Type: {0}", contenttype));
            }

            foreach (var header in Header.Where(header => header.Key != "Content-Type" || contenttype == ""))
            {
                requestHeader.AppendLine(string.Format("{0}: {1}", header.Key, header.Value));
            }

            if (cookiestr != "")
                requestHeader.AppendLine(string.Format("Cookie: {0}", cookiestr));

            requestHeader.AppendLine();
            var headData = encoding.GetBytes(requestHeader.ToString());

            requestMs.Write(headData, 0, headData.Length);

            requestMs.Write(postBody, 0, postBody.Length);


            return postBody;
        }

        #endregion

        #region 公开方法

        /// <summary>
        ///     执行单条请求（http代理模式)
        /// </summary>
        /// <param name="xrequest">请求体</param>
        /// <param name="oldhtml">上一步源码</param>
        /// <param name="proxystr">http代理，格式：127.0.0.1:8080</param>
        /// <param name="hostip"></param>
        /// <returns></returns>
        public XResponse RunRequest(XRequest xrequest, string oldhtml = null, string proxystr = null, string hostip = null)
        {
            var response = new XResponse();

            try
            {

                var redirectCount = 0;
                Redirect:

                var encoding = Encoding.GetEncoding(xrequest.Encoding);


                #region 处理标签

                var uri = new Uri(ReplaceTag(xrequest.Url, oldhtml));
                var referer = ReplaceTag(xrequest.Referer, oldhtml);


                var headerlist =
                    xrequest.Header.Select(
                        t => new KeyValuePair<string, string>(ReplaceTag(t.Key, oldhtml), ReplaceTag(t.Value, oldhtml)))
                        .ToList();
                var postlist =
                   xrequest.PostData.Select(
                       t => new KeyValuePair<string, string>(ReplaceTag(t.Key, oldhtml), ReplaceTag(t.Value, oldhtml)))
                       .ToList();
                var poststring = ReplaceTag(xrequest.PostString, oldhtml);

                #endregion

                if (Config.IsUseHttpClient)
                {
                    #region 使用新版内核
                    #region 构造头部
                    ServicePointManager.DefaultConnectionLimit = 512;
                    ServicePointManager.Expect100Continue = false;
                    ServicePointManager.SetTcpKeepAlive(true, 30000, 3000);


                    var handler = new HttpClientHandler
                    {
                        AutomaticDecompression = DecompressionMethods.GZip,
                        CookieContainer = GetCookieCookieContainer(uri),
                        UseCookies = true,
                        AllowAutoRedirect = false,
                        ClientCertificateOptions = ClientCertificateOption.Automatic
                    };

                    //  handler.UseProxy = false;

                    var httpClient = new HttpClient(handler);

                    var httprequest = new HttpRequestMessage();


                    httpClient.Timeout = new TimeSpan(0, 0, 0, 60);
                    //最大内容缓存8MB
                    httpClient.MaxResponseContentBufferSize = 1024 * 1024 * 8;


                    httprequest.Headers.Referrer = referer.ToLower().StartsWith("http") ? new Uri(referer) : null;
                    httprequest.RequestUri = uri;
                    if (xrequest.UserAgent != "")
                        httprequest.Headers.Add("User-Agent", xrequest.UserAgent);

                    foreach (var keyValuePair in headerlist)
                    {
                        try
                        {
                            httprequest.Headers.Add(keyValuePair.Key, keyValuePair.Value);
                        }
                        catch (Exception ex)
                        {

                            throw new Exception(ex.Message + " key:" + keyValuePair.Key + " value:" + keyValuePair.Value);
                        }

                    }

                    #endregion

                    #region 请求方法

                    if (xrequest.Method == 0)
                    {
                        httprequest.Method = HttpMethod.Get;
                    }
                    else
                    {

                        switch (xrequest.Method)
                        {
                            case 1:
                                var sb1 = new StringBuilder();
                                for (var index = 0; index < postlist.Count; index++)
                                {
                                    sb1.Append(postlist[index].Key + "=" +
                                               StrHelper.UrlEncode(postlist[index].Value,
                                                   encoding));
                                    if (index != postlist.Count - 1)
                                        sb1.Append("&");
                                }
                                httprequest.Content = new ByteArrayContent(encoding.GetBytes(sb1.ToString()));
                                httprequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                                break;
                            case 2:
                                var boundar = RandStr.GetEngStr(10, false);
                                var multipartContent = new MultipartContent("form-data", "WebKitFormBoundary" + boundar);

                                foreach (var v in postlist)
                                {

                                    if (v.Value.StartsWith("{文件数据"))
                                    {
                                        var kv = v.Value.Split('=');
                                        var filedata = new byte[0];
                                        switch (kv[0])
                                        {
                                            case "{文件数据1":
                                                filedata = ByteData.Data1;
                                                break;
                                            case "{文件数据2":
                                                filedata = ByteData.Data2;
                                                break;
                                            case "{文件数据3":
                                                filedata = ByteData.Data3;
                                                break;
                                            case "{文件数据4":
                                                filedata = ByteData.Data4;
                                                break;
                                            case "{文件数据5":
                                                filedata = ByteData.Data5;
                                                break;
                                        }
                                        var bytecontent = new ByteArrayContent(filedata);
                                        bytecontent.Headers.ContentDisposition =
                                            new ContentDispositionHeaderValue("form-data")
                                            {
                                                Name = v.Key,
                                                FileName = kv[1].Trim('}')
                                            };
                                        bytecontent.Headers.ContentType =
                                            new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(kv[1].Trim('}')));
                                        multipartContent.Add(bytecontent);
                                    }
                                    else
                                    {
                                        var stringcontent = new StringContent(v.Value);
                                        stringcontent.Headers.ContentDisposition =
                                            new ContentDispositionHeaderValue("form-data") { Name = v.Key };
                                        multipartContent.Add(stringcontent);
                                    }
                                }
                                httprequest.Content = multipartContent;
                                break;
                            case 3:

                                byte[] postBody = new byte[0];
                                if (xrequest.PostString.StartsWith("{文件数据"))
                                {

                                    var kv = xrequest.PostString.Split('-');
                                    switch (kv[0])
                                    {
                                        case "{文件数据1":
                                            postBody = ByteData.Data1;
                                            break;
                                        case "{文件数据2":
                                            postBody = ByteData.Data2;
                                            break;
                                        case "{文件数据3":
                                            postBody = ByteData.Data3;
                                            break;
                                        case "{文件数据4":
                                            postBody = ByteData.Data4;
                                            break;
                                        case "{文件数据5":
                                            postBody = ByteData.Data5;
                                            break;
                                    }
                                }
                                else
                                {
                                    postBody = encoding.GetBytes(xrequest.PostString);
                                }
                                var bytecontent3 = new ByteArrayContent(postBody);
                                httprequest.Content = bytecontent3;
                                break;
                        }

                        httprequest.Method = HttpMethod.Post;

                    }

                    #endregion

                    #region 获取返回包

                    var myresponse = httpClient.SendAsync(httprequest).Result;

                    //设置返回内容编码
                    var contentType = myresponse.Content.Headers.ContentType ?? new MediaTypeHeaderValue("application/xml");
                    if (string.IsNullOrEmpty(contentType.CharSet))
                    {
                        contentType.CharSet = xrequest.Encoding;
                    }

                    var content = myresponse.Content;

                    response.Location = myresponse.Headers.Location != null
                        ? myresponse.Headers.Location.ToString()
                        : "";
                    response.BodyData = content.ReadAsByteArrayAsync().Result;
                    response.BodyString = encoding.GetString(response.BodyData);
                    response.StatusCode = Convert.ToInt32(myresponse.StatusCode);


                    IEnumerable<string> cookies;
                    myresponse.Headers.TryGetValues("Set-Cookie", out cookies);

                    if (cookies != null)
                    {
                        foreach (
                            var cookie in
                                cookies.Select(setcooky => XCookieParser(setcooky, uri)).Where(cookie => cookie != null)
                            )
                        {
                            CookieAdd(cookie);
                        }
                    }

                    response.Header = myresponse.RequestMessage.Method + "：" + myresponse.RequestMessage.RequestUri + "\r\n" + myresponse.RequestMessage.Headers + "\r\nBody Length:" + (myresponse.RequestMessage.Content != null ? myresponse.RequestMessage.Content.Headers.ContentLength : 0) + "\r\n\r\n" + "Status：" + myresponse.StatusCode + "\r\n" + myresponse.Headers + "\r\n\r\n";
                    response.ContentType = contentType.MediaType;


                    if ((response.StatusCode == 301 || response.StatusCode == 302) && response.Location != null && IsRedirect)
                    {
                        redirectCount++;
                        if (redirectCount < 5)
                        {
                            var nrequest = XSerializable.BytesToObject<XRequest>(XSerializable.ObjectToBytes(xrequest));
                            nrequest.Url = HttpHelper.GetFullUrl(response.Location, ReplaceTag(xrequest.Url));
                            response.StatusCode = 0;
                            response.Location = null;
                            xrequest = nrequest;
                            goto Redirect;
                        }
                    }

                    return response;

                    #endregion
                    #endregion
                }
                else
                {
                    #region 使用老版内核
                    var requestMs = new MemoryStream();

                    StringBuilder requestHeader;
                    List<string> setcookies;



                    if (proxystr != null)
                    {
                        var ps = proxystr.Split('|');
                        if (ps.Length == 2)
                        {
                            return RemoteRun(ps[0], ps[1], ref CookieManager, xrequest);
                        }
                    }


                    TcpClient client;

                    if (proxystr != null && proxystr.Split(':').Length == 2)
                    {
                        client = new TimeOutSocket().Connect(proxystr.Split(':')[0],
                            Convert.ToInt32(proxystr.Split(':')[1]), Config.ConnectTimeout, Config.SendTimeout,
                            Config.ReceiveTimeout, null);
                    }
                    else
                    {
                        client = new TimeOutSocket().Connect(hostip ?? uri.Host, uri.Port, Config.ConnectTimeout,
                            Config.SendTimeout, Config.ReceiveTimeout, LoaclEndPoin);
                    }


                    client.ReceiveBufferSize = 4096;
                    client.SendBufferSize = 4096;
                    client.NoDelay = true;

                    #region 合成请求包

                    var requestData = GetRequestBody(xrequest.UserAgent, referer, xrequest.Method, encoding, uri,
                        GetCookieStr(uri), requestMs, out requestHeader, headerlist, postlist, poststring,
                        (proxystr != null && proxystr.Split(':').Length == 2));

                    #endregion

                    #region 发送数据

                    var clientStream = SendData(xrequest, client, uri, requestMs);

                    #endregion

                    #region 接收数据

                    var responseHeader = ReceiveData(client, clientStream, response, out setcookies);

                    #endregion

                    response.BodyString = StrHelper.ByteToString(response.BodyData, encoding);

                    clientStream.Close();
                    client.Close();

                    foreach (
                        var cookie in
                            setcookies.Select(setcooky => XCookieParser(setcooky, uri)).Where(cookie => cookie != null))
                    {
                        CookieAdd(cookie);
                    }

                    response.Header += requestHeader +
                                       (xrequest.Method == 1
                                           ? StrHelper.ByteToString(requestData, encoding)
                                           : (requestData.Length > 0 ? "[More Data]" : "")) + "\r\n\r\n" + responseHeader +
                                       "\r\n\r\n";

                    response.RequestEncoding = encoding.WebName;

                    if ((response.StatusCode == 301 || response.StatusCode == 302) && response.Location != null && IsRedirect)
                    {
                        redirectCount++;
                        if (redirectCount < 5)
                        {
                            var nrequest = XSerializable.BytesToObject<XRequest>(XSerializable.ObjectToBytes(xrequest));
                            nrequest.Url = HttpHelper.GetFullUrl(response.Location, ReplaceTag(xrequest.Url));
                            response.StatusCode = 0;
                            response.Location = null;
                            xrequest = nrequest;
                            goto Redirect;
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                response.Header = "try catch";
                response.BodyString = "出错了，原因：" + (ex.InnerException != null ? ex.InnerException.Message + ex.InnerException.StackTrace : ex.Message + ex.StackTrace);
            }
            return response;
        }

        public XResponse RunRequest(string xrequest, string oldhtml = null, string proxystr = null, string hostip = null)
        {
            var response = new XResponse();
            try
            {
                var model = FileHelper.LoadModel<XRequest>(Base32.FromBase32String(xrequest));
                if (model != null)
                {
                    return RunRequest(model, oldhtml, proxystr, hostip);
                }
                else
                {
                    response.BodyString = "出错了，原因：输入的脚本字符串不正确，无法序列化成脚本！";
                }
            }
            catch (Exception ex)
            {
                response.BodyString = "出错了，原因：" + ex.Message + "\r\n" + ex.StackTrace;
            }
            return response;
        }


        /// <summary>
        /// </summary>
        /// <param name="proxystr"></param>
        /// <param name="hostip"></param>
        /// <returns></returns>
        public XResponse RunRequests(string proxystr = null, string hostip = null)
        {
            var response = new XResponse();
            for (int index = 0; index < Scripts.Count; index++)
            {
                var t = Scripts[index];
                var regex = new Regex(t.Require);
                if (regex.Match(response.AllContent).Success)
                {
                    var temp = RunRequest(t, response.AllContent, proxystr, hostip);
                    response.AllContent += "第" + index + "次请求结果：\r\n\r\n" + temp.BodyString;
                    response.BodyList.Add(temp.BodyString);
                    response.HeaderList.Add(temp.Header);
                    response.Header = temp.Header;
                    response.BodyData = temp.BodyData;
                    response.BodyString = temp.BodyString;
                    Thread.Sleep(t.Delay);
                }
            }
            return response;
        }

        private XResponse RemoteRun(string ip, string port, ref XCookieManager cookiemanager, XRequest scripts)
        {
            var response = new XResponse();
            try
            {
                var execute = new Http();
                var cmd = new Command
                {
                    Bytes1 = XSerializable.ObjectToBytes(cookiemanager),
                    Bytes2 = XSerializable.ObjectToBytes(scripts),
                    String1 = ip
                };
                var data = XSerializable.ObjectToBytes(cmd);
                var request = new XRequest
                {
                    Method = 3,
                    Url = "http://" + ip + ":" + port + "/",
                    PostString = "{文件数据1-data.rar}"
                };


                execute.ByteData.Data1 = data;

                response = execute.RunRequest(request);
                cmd = XSerializable.BytesToObject<Command>(response.BodyData);

                cookiemanager = XSerializable.BytesToObject<XCookieManager>(cmd.Bytes1);

                return XSerializable.BytesToObject<XResponse>(cmd.Bytes2);
            }
            catch (Exception ex)
            {
                response.BodyString = "出错了，原因：通过侠客代理执行时出错，" + ex.Message;
                return response;
            }

        }

        #region 加载脚本

        /// <summary>
        ///     加载脚本
        /// </summary>
        /// <param name="data">二进制数据</param>
        /// <param name="isLoadCookie">是否读取模块里的cookie覆盖到当前执行器</param>
        /// <returns></returns>
        public bool LoadScript(byte[] data, bool isLoadCookie)
        {
            try
            {
                var s = XSerializable.BytesToObject<XScript>(data);
                Scripts = s.Scripts;
                if (isLoadCookie)
                    CookieManager = s.CookieManager;
                Tag = s.Tag;
                Account = s.Account;
                ByteData = s.ByteData;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     加载脚本
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="isLoadCookie">是否读取模块里的cookie覆盖到当前执行器</param>
        /// <returns></returns>
        public bool LoadScript(string filename, bool isLoadCookie)
        {
            try
            {
                var s = XSerializable.BytesToObject<XScript>(File.ReadAllBytes(FileHelper.GetPath(filename)));
                Scripts = s.Scripts;
                if (isLoadCookie)
                    CookieManager = s.CookieManager;
                Tag = s.Tag;
                Account = s.Account;
                ByteData = s.ByteData;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     加载脚本
        /// </summary>
        /// <param name="xscript"></param>
        /// <param name="isLoadCookie">是否读取模块里的cookie覆盖到当前执行器</param>
        /// <returns></returns>
        public bool LoadScript(XScript xscript, bool isLoadCookie)
        {
            try
            {
                if (xscript != null)
                {
                    Scripts = xscript.Scripts;
                    if (isLoadCookie)
                        CookieManager = xscript.CookieManager;
                    Tag = xscript.Tag;
                    Account = xscript.Account;
                    ByteData = xscript.ByteData;
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #endregion

        #region 标签处理

        /// <summary>
        ///     处理变量标签
        /// </summary>
        /// <param name="str"></param>
        /// <param name="oldhtml"></param>
        /// <returns></returns>
        public string ReplaceTag(string str = "", string oldhtml = "")
        {
            var index = 0;
            restart:
            for (var i = index; i < str.Length; i++)
            {
                if (str[i] == '{')
                {
                    var endindex = str.IndexOf('}', i);
                    if (endindex > -1)
                    {
                        var tagname = str.Substring(i, endindex - i + 1);
                        str = str.Replace(tagname, ReplaceDic(tagname, oldhtml));
                        i++;
                        index = i;
                        goto restart;
                    }
                }
            }

            str = ReplaceRegex(str, oldhtml);
            return str;
        }

        private string ReplaceDic(string tag, string oldhtml = "")
        {
            var kv = tag.Split('=');
            var result = tag;
            switch (kv[0])
            {
                case "{账号}":
                    result = Account.Account;
                    break;
                case "{密码}":
                    result = Account.Password;
                    break;
                case "{附加值1}":
                    result = Account.Other1;
                    break;
                case "{附加值2}":
                    result = Account.Other2;
                    break;
                case "{附加值3}":
                    result = Account.Other3;
                    break;
                case "{附加值4}":
                    result = Account.Other4;
                    break;
                case "{附加值5}":
                    result = Account.Other5;
                    break;

                case "{标签01}":
                    result = Tag.Str01;
                    break;
                case "{标签02}":
                    result = Tag.Str02;
                    break;
                case "{标签03}":
                    result = Tag.Str03;
                    break;
                case "{标签04}":
                    result = Tag.Str04;
                    break;
                case "{标签05}":
                    result = Tag.Str05;
                    break;
                case "{标签06}":
                    result = Tag.Str06;
                    break;
                case "{标签07}":
                    result = Tag.Str07;
                    break;
                case "{标签08}":
                    result = Tag.Str08;
                    break;
                case "{标签09}":
                    result = Tag.Str09;
                    break;
                case "{标签10}":
                    result = Tag.Str10;
                    break;
                case "{标签11}":
                    result = Tag.Str11;
                    break;
                case "{标签12}":
                    result = Tag.Str12;
                    break;
                case "{标签13}":
                    result = Tag.Str13;
                    break;
                case "{标签14}":
                    result = Tag.Str14;
                    break;
                case "{标签15}":
                    result = Tag.Str15;
                    break;
                case "{标签16}":
                    result = Tag.Str16;
                    break;
                case "{标签17}":
                    result = Tag.Str17;
                    break;
                case "{标签18}":
                    result = Tag.Str18;
                    break;
                case "{标签19}":
                    result = Tag.Str19;
                    break;
                case "{标签20}":
                    result = Tag.Str20;
                    break;
                case "{标签21}":
                    result = Tag.Str21;
                    break;
                case "{标签22}":
                    result = Tag.Str22;
                    break;
                case "{标签23}":
                    result = Tag.Str23;
                    break;
                case "{标签24}":
                    result = Tag.Str24;
                    break;
                case "{标签25}":
                    result = Tag.Str25;
                    break;
                case "{标签26}":
                    result = Tag.Str26;
                    break;
                case "{标签27}":
                    result = Tag.Str27;
                    break;
                case "{标签28}":
                    result = Tag.Str28;
                    break;
                case "{标签29}":
                    result = Tag.Str29;
                    break;
                case "{标签30}":
                    result = Tag.Str30;
                    break;


                case "{当前IP}":
                    result = GetMyip();
                    break;

                case "{文件数据":
                    result = tag;
                    break;
            }
            return result;
        }

        private string ReplaceRegex(string sourcecode, string oldcode = "")
        {
            if (sourcecode.Contains("{正则-尾="))
            {
                var r = new Regex("(?<={正则-尾=).*(?=})", RegexOptions.IgnoreCase);
                var ms = r.Matches(sourcecode);
                for (var i = 0; i < ms.Count; i++)
                {
                    var rr = new Regex(ms[i].Value, RegexOptions.IgnoreCase);
                    var m = rr.Matches(oldcode);
                    sourcecode = sourcecode.Replace("{正则-尾=" + ms[i].Value + "}",
                        m.Count > 0 ? m[m.Count - 1].Value : "");
                }
            }
            if (sourcecode.Contains("{正则-头="))
            {
                var r = new Regex("(?<={正则-头=).*(?=})", RegexOptions.IgnoreCase);
                var ms = r.Matches(sourcecode);
                for (var i = 0; i < ms.Count; i++)
                {
                    var rr = new Regex(ms[i].Value, RegexOptions.IgnoreCase);
                    var m = rr.Matches(oldcode);
                    sourcecode = sourcecode.Replace("{正则-头=" + ms[i].Value + "}",
                        m.Count > 0 ? m[0].Value : "");
                }
            }

            if (sourcecode.Contains("{Cookie值"))
            {
                var r = new Regex("(?<={Cookie值=).*?(?=})", RegexOptions.IgnoreCase);
                var ms = r.Matches(sourcecode);
                for (var i = 0; i < ms.Count; i++)
                {
                    sourcecode = sourcecode.Replace("{Cookie值=" + ms[i].Value + "}", CookieGetKey(ms[i].Value));
                }
            }
            if (sourcecode.Contains("{验证码"))
            {
                var r = new Regex("(?<={验证码=).*?(?=})", RegexOptions.IgnoreCase);
                var ms = r.Matches(sourcecode);
                for (var i = 0; i < ms.Count; i++)
                {
                    sourcecode = sourcecode.Replace("{验证码=" + ms[i].Value + "}", GetVerify(ms[i].Value));
                }
            }
            return sourcecode;
        }

        #region 验证码识别

        /// <summary>
        ///     获取并识别验证码
        /// </summary>
        /// <param name="url">验证码地址</param>
        /// <returns>识别结果</returns>
        protected virtual string GetVerify(string url)
        {
            var request = new XRequest
            {
                Url = url
            };
            var response = RunRequest(request, "");
            return GetVerify(response.BodyData, url);
        }

        /// <summary>
        ///     获取并识别验证码
        /// </summary>
        /// <param name="data">图片二进制数据</param>
        /// <param name="url">验证码地址</param>
        /// <returns>识别结果</returns>
        public string GetVerify(byte[] data, string url)
        {
            try
            {
                return GetVerify(data);
            }
            catch
            {
                return "验证码下载失败";
            }
        }


        /// <summary>
        ///     调用自动识别API
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetVerify(byte[] data)
        {
            var cmd = new Command
            {
                Action = "getverify",
                Bytes1 = data
            };
            cmd = HttpHelper.GetHtml(cmd, null, null);
            var result = cmd.IsOk ? cmd.String1 : cmd.Message;
            return result;
        }

        #endregion

        #region 其他变量获取

        private string GetMyip()
        {
            var request = new XRequest
            {
                Url = "http://server.xksoft.com/"
            };
            var str = RunRequest(request).BodyString;
            var r = new Regex("(?<=You IP is \").*?(?=\")");
            var m = r.Match(str);
            return m.Success ? m.Value : "192.168.1.8";
        }

        #endregion

        #endregion
    }
}