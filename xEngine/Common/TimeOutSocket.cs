#region

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

#endregion

namespace xEngine.Common
{
    internal class TimeOutSocket
    {
        private readonly ManualResetEvent _timeoutObject = new ManualResetEvent(false);

        internal TcpClient Connect(string host, int port, int connecttimeout, int sendtimeout, int rectivetimeout,
            IPEndPoint lep)
        {
            _timeoutObject.Reset();
            var state = (lep != null) ? new TcpClient(lep) : new TcpClient();
            state.BeginConnect(GetIp(host), port, CallBackMethod, state);
            if (!_timeoutObject.WaitOne(connecttimeout, false))
            {
                throw new TimeoutException("连接" + host + "超时！");
            }
            state.ReceiveTimeout = rectivetimeout;
            state.SendTimeout = sendtimeout;
            return state;
        }

        private void CallBackMethod(IAsyncResult asyncresult)
        {
            _timeoutObject.Set();
        }

        private string GetIp(string host)
        {
            try
            {
                IPAddress[] IPs = Dns.GetHostAddresses(host);
                if (IPs.Length > 0)
                {
                    return IPs[0].ToString();
                }
                else
                {
                    throw new TimeoutException("解析" + host + "的真实IP时超时！");
                }
            }
            catch (Exception ex)
            {

                throw new TimeoutException("解析" + host + "的真实IP时超时！ " + ex.Message);
            }

        }
    }
}