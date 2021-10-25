#region

using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using MyProto;

#endregion

#pragma warning disable 693

namespace xEngine.Common
{
    /// <summary>
    /// </summary>
    public class XSerializable
    {
        /// <summary>
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Zip(byte[] buffer)
        {
            var ms = new MemoryStream();
            var zip = new GZipStream(ms, CompressionMode.Compress, true);
            zip.Write(buffer, 0, buffer.Length);
            zip.Close();
            ms.Position = 0;
            var zipBuffer = new byte[ms.Length];
            ms.Read(zipBuffer, 0, zipBuffer.Length);
            ms.Close();
            return zipBuffer;
        }

        /// <summary>
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Unzip(byte[] buffer)
        {
            try
            {
                var ms = new MemoryStream();
                ms.Write(buffer, 0, buffer.Length);
                ms.Position = 0;
                var zip = new GZipStream(ms, CompressionMode.Decompress, true);
                var zipBuffer = new byte[1024];
                var ms2 = new MemoryStream();
                while (true)
                {
                    var bytesRead = zip.Read(zipBuffer, 0, zipBuffer.Length);
                    if (bytesRead == 0)
                    {
                        break;
                    }
                    ms2.Write(zipBuffer, 0, bytesRead);
                }
                zip.Close();
                return ms2.ToArray();
            }
            catch
            {
                return new byte[0];
            }
        }

        /// <summary>
        ///     克隆一个对象
        /// </summary>
        /// <typeparam name="TYpe"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TYpe CloneObject<TYpe>(object obj)
        {
            return BytesToObject<TYpe>(ObjectToBytes(obj));
        }

        /// <summary>
        ///     克隆一个对象
        /// </summary>
        /// <typeparam name="TYpe"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TYpe CloneObjectByMs<TYpe>(object obj)
        {
            return (TYpe) BytesToObjectByMs(ObjectToBytesByMs(obj));
        }

        #region 新序列化方法

        /// <summary>
        ///     二进制数据转换成对象
        /// </summary>
        /// <typeparam name="TYpe">对象类型</typeparam>
        /// <param name="bytes">二进制数据</param>
        /// <returns>返回的对象</returns>
        public static TYpe BytesToObject<TYpe>(byte[] bytes)
        {
            try
            {
                using (var memStream = new MemoryStream(StrHelper.DESDecrypt(bytes)))
                {
                    return StreamToObject<TYpe>(memStream);
                }
            }
            catch (Exception ex)
            {
                if (bytes == null)
                    throw new ArgumentNullException("反序列化失败, EX,data=null:" + ex.Message);
                // File.WriteAllText(FileHelper.GetPath("Serializable.txt"), ex.Message + "\r\n" + Encoding.UTF8.GetString(Bytes));
                throw new ArgumentNullException("反序列化失败, EX,data=" + bytes.Length + ":" + ex.Message);
            }
        }

        private static TYpe StreamToObject<TYpe>(Stream ms)
        {
            return Serializer.Deserialize<TYpe>(ms);
        }

        /// <summary>
        ///     将一个对象保存成二进制数据
        /// </summary>
        /// <param name="obj">要保存的对象（需要符合契约）</param>
        /// <returns>返回的二进制</returns>
        public static byte[] ObjectToBytes(object obj)
        {
            try
            {
                var ms = new MemoryStream();
                Serializer.Serialize(ms, obj);
                ms.Position = 0;
                var bytes = new byte[ms.Length];
                ms.Read(bytes, 0, bytes.Length);
                ms.Close();
                return StrHelper.DESEncrypt(bytes);
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("序列化失败，EX:" + ex.Message);
            }
        }

        #endregion

        #region 老序列化方法

        /// <summary>
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static object BytesToObjectByMs(byte[] bytes)
        {
            using (var ms = new MemoryStream(Unzip(bytes)))
            {
                IFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(ms);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] ObjectToBytesByMs(object obj)
        {
            if (obj == null)
                return new byte[0];
            using (var ms = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                var bytes = ms.GetBuffer();
                return Zip(bytes);
            }
        }

        #endregion
    }
}