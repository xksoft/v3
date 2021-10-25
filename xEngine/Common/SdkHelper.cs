#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using xEngine.Execute;
using xEngine.Model;
using xEngine.Model.Execute.Http;

#endregion

namespace xEngine.Common
{
    internal static class SdkHelper
    {
        #region 接口通讯

        internal static Command RunCommand(Command command, string url = "http://server.xksoft.com/api")
        {
            command.Ticks = DateTime.Now.Ticks;
            var result = new Command();
            command.String8 = StrHelper.GetMd5(Guid.NewGuid().ToString(), false, true);

            var data = XSerializable.ObjectToBytes(command);

            var isRestart = false;
            byte[] bytedata = new byte[0];
        restart:
            try
            {
                var execute = new Http();
                var request = new XRequest
                {
                    Method = 3,
                    Url = url
                };
                request.Header.Add(new KeyValuePair<string, string>("Xapi", "xiake"));
                request.Header.Add(new KeyValuePair<string, string>("id", command.Action));
                request.PostString = "{文件数据1-data.rar}";

                execute.ByteData.Data1 = data;

                var response = isRestart && url == "http://server.xksoft.com/api"
                    ? execute.RunRequest(request, null, null, "121.40.172.22")
                    : execute.RunRequest(request);
                bytedata = response.BodyData;
                result = XSerializable.BytesToObject<Command>(bytedata);
                if (result.String8 != command.String8 && url == "http://server.xksoft.com/api")
                {
                    throw new Exception("HASH ERROR");
                }
            }
            catch (Exception ex)
            {
                if (!isRestart)
                {
                    isRestart = true;
                    goto restart;
                }

                result.IsOk = false;
                result.Action = "Network Error";
                if (ex.Message.Contains("无法从传输连接中读取数据"))
                {
                    result.Message = "通讯失败：从服务器接收数据超时，请检查您的网络是否正常!";
                }
                else if (ex.Message.Contains("值不能为空"))
                {
                    result.Message = "通讯失败，可能没部署好服务端";
                }
                else
                {
                    result.Message = "通讯失败：" + ex.Message + (bytedata != null ? Encoding.UTF8.GetString(bytedata) : "");
                }
            }
            return result ?? (new Command
            {
                IsOk = false,
                Action = "0",
                Message = "通讯失败：返回的数据包无法还原成模型！"
            });
        }

        #endregion
    }
}