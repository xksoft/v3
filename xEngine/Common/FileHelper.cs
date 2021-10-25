#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using xEngine.Model.Execute.Http;
using xEngine.Model.Execute.Rules;

#endregion

#pragma warning disable 693

namespace xEngine.Common
{
    /// <summary>
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        ///     获取以本程序根目录为准的相对路径的绝对路径
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetPath(string str)
        {
            var codebase = Assembly.GetExecutingAssembly().CodeBase;
            codebase = codebase.Substring(8, codebase.Length - 8);
            codebase = codebase.Substring(0, codebase.LastIndexOf("/", StringComparison.Ordinal));
            return Path.GetFullPath(codebase + "\\" + str);
        }

        #region 泛型操作

        /// <summary>
        ///     保存模型
        /// </summary>
        /// <param name="filename">文件路径</param>
        /// <param name="model">模型</param>
        /// <returns>是否保存成功</returns>
        public static bool SaveModel(string filename, object model)
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(Path.GetFullPath(filename))))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(filename)));
                }
                File.WriteAllBytes(Path.GetFullPath(filename), XSerializable.ObjectToBytes(model));
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     加载模型
        /// </summary>
        /// <typeparam name="TYpe">类型</typeparam>
        /// <param name="filename">文件名</param>
        /// <returns>返回模型，null为加载失败</returns>
        public static TYpe LoadModel<TYpe>(string filename)
        {
            try
            {
                var s = XSerializable.BytesToObject<TYpe>(File.ReadAllBytes(Path.GetFullPath(filename)));
                if (s != null)
                {
                    return s;
                }
                return default(TYpe);
            }
            catch
            {
                return default(TYpe);
            }
        }

        /// <summary>
        ///     保存模型为二进制
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns>二进制</returns>
        public static byte[] SaveModel(object model)
        {
            try
            {
                return XSerializable.ObjectToBytes(model);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///     从二进制加载模型
        /// </summary>
        /// <typeparam name="TYpe">类型</typeparam>
        /// <param name="data">二进制数据</param>
        /// <returns>返回模型，null为加载失败</returns>
        public static TYpe LoadModel<TYpe>(byte[] data)
        {
            try
            {
                var s = XSerializable.BytesToObject<TYpe>(data);
                if (s != null)
                {
                    return s;
                }
                return default(TYpe);
            }
            catch
            {
                return default(TYpe);
            }
        }

        #endregion

        #region 加载文件

        /// <summary>
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static RulesMain LoadRules(string fileName)
        {
            try
            {
                var s = XSerializable.BytesToObject<RulesMain>(File.ReadAllBytes(Path.GetFullPath(fileName)));
                return s;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="bytedata"></param>
        /// <returns></returns>
        public static RulesMain LoadRules(byte[] bytedata)
        {
            try
            {
                var s = XSerializable.BytesToObject<RulesMain>(bytedata);
                return s;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static XAccount LoadAccount(string fileName)
        {
            try
            {
                var s = XSerializable.BytesToObject<XAccount>(File.ReadAllBytes(Path.GetFullPath(fileName)));
                return s;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="bytedata"></param>
        /// <returns></returns>
        public static XAccount LoadAccount(byte[] bytedata)
        {
            try
            {
                var s = XSerializable.BytesToObject<XAccount>(bytedata);
                return s;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static XTag LoadTag(string fileName)
        {
            try
            {
                var s = XSerializable.BytesToObject<XTag>(File.ReadAllBytes(Path.GetFullPath(fileName)));
                return s;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="bytedata"></param>
        /// <returns></returns>
        public static XTag LoadTag(byte[] bytedata)
        {
            try
            {
                var s = XSerializable.BytesToObject<XTag>(bytedata);
                return s;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static XCookieManager LoadCookie(string fileName)
        {
            try
            {
                var s = XSerializable.BytesToObject<XCookieManager>(File.ReadAllBytes(Path.GetFullPath(fileName)));
                return s;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="bytedata"></param>
        /// <returns></returns>
        public static XCookieManager LoadCookie(byte[] bytedata)
        {
            try
            {
                var s = XSerializable.BytesToObject<XCookieManager>(bytedata);
                return s;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static XScript LoadScript(string fileName)
        {
            try
            {
                var s = XSerializable.BytesToObject<XScript>(File.ReadAllBytes(Path.GetFullPath(fileName)));
                return s;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="bytedata"></param>
        /// <returns></returns>
        public static XScript LoadScript(byte[] bytedata)
        {
            try
            {
                var s = XSerializable.BytesToObject<XScript>(bytedata);
                return s;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region 文件操作帮助

        /// <summary>
        ///     获取文件列表
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pattren"></param>
        /// <returns></returns>
        public static List<string> GetFileList(string path, string pattren)
        {
            return GetFileList(path, pattren, false);
        }

        /// <summary>
        ///     获取文件列表
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pattren"></param>
        /// <param name="isHaveSubdir"></param>
        /// <returns></returns>
        public static List<string> GetFileList(string path, string pattren, bool isHaveSubdir)
        {
            var list = new List<string>();
            try
            {
                var s = Directory.GetFiles(Path.GetFullPath(path), pattren);
                list.AddRange(s);
                if (isHaveSubdir)
                {
                    var dirs = Directory.GetDirectories(path);
                    foreach (var t in dirs)
                        list.AddRange(GetFileList(t, pattren, true));
                }
            }
            catch
            {
            }
            return list;
        }

        /// <summary>
        ///     获取文件哈希值
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static string GetFileHash(string filepath)
        {
            string hashcode;
            try
            {
                var fs = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var md5 = new MD5CryptoServiceProvider();
                var byt = md5.ComputeHash(fs);
                hashcode = BitConverter.ToString(byt);
                hashcode = hashcode.Replace("-", "");
                fs.Close();
                fs.Dispose();
            }
            catch
            {
                return "";
            }
            return hashcode;
        }

        #endregion
    }
}