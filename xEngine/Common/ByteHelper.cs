#region

using System;

#endregion

namespace xEngine.Common
{
    /// <summary>
    /// </summary>
    public class ByteHelper
    {
        /// <summary>
        ///     在数据包头部加上它的长度
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] MakeLength(byte[] data)
        {
            var result = new byte[data.Length + 4];
            Array.Copy(BitConverter.GetBytes(data.Length), 0, result, 0, 4);
            Array.Copy(data, 0, result, 4, data.Length);
            return result;
        }
    }
}