namespace xEngine
{
    /// <summary>
    ///     sdk配置
    /// </summary>
    public static class Config
    {
        internal static int ConnectTimeout = 15000;
        internal static int SendTimeout = 30000;
        internal static int ReceiveTimeout = 30000;

        public static int TimeOut = 30;
        public static bool IsUseHttpClient = false;

        /// <summary>
        ///     Http请求操作超时时间设定(单位：毫秒 默认值，3000 30000 30000）
        /// </summary>
        /// <param name="connectTimeout">连接超时时间，表示如果超过这个时间未链接，则结束连接）</param>
        /// <param name="sendTimeout">发送请求超时时间，如果超过这个时间还未上传完毕，则结束请求</param>
        /// <param name="receiveTimeout">接受请求超时时间，如果超过这个时间还未接收完毕，则结束请求</param>
        public static void SetHttpTimeout(int connectTimeout, int sendTimeout, int receiveTimeout)
        {
            ConnectTimeout = connectTimeout;
            SendTimeout = sendTimeout;
            ReceiveTimeout = receiveTimeout;
        }
    }
}