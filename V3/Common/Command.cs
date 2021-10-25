using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xEngine.Common;
using xEngine.Execute;
using xEngine.Model.Execute.Http;
namespace V3.Common
{
  public  class Command
    {
      public static xEngine.Model.Command RunCommand(xEngine.Model.Command command, string url)
      {
          command.Ticks = DateTime.Now.Ticks;
          var result = new xEngine.Model.Command();

          var data = XSerializable.ObjectToBytes(command);
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

              var response = execute.RunRequest(request, "");


              result = XSerializable.BytesToObject<xEngine.Model.Command>(response.BodyData);
          }
          catch (Exception ex)
          {
              result.IsOk = false;
              result.Action = "0";
              if (ex.Message.Contains("无法从传输连接中读取数据"))
              {
                  result.Message = "通讯失败：从服务器接收数据超时，请检查您的网络是否正常！";
              }
              else if (ex.Message.Contains("值不能为空"))
              {
                  result.Message = "通讯失败，可能没部署好服务端";
              }
              else
              {
                  result.Message = "通讯失败：" + ex.Message;
              }
          }
          return result ?? (new xEngine.Model.Command
          {
              IsOk = false,
              Action = "0",
              Message = "通讯失败：返回的数据包无法还原成模型！"
          });
      }
    }
}
