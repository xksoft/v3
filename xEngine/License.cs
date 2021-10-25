#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using xEngine.Common;
using xEngine.Model;
using xEngine.Model.License;

#endregion

namespace xEngine
{
    /// <summary>
    ///     授权相关类
    /// </summary>
    public class License
    {
        public static string build = "20161106";

        #region 代理相关

        /// <summary>
        ///     从侠客平台上获取代理服务器信息（公有1000条）
        /// </summary>
        /// <returns></returns>
        public static List<string> GetProxy(bool basicproxy = true)
        {
            var cmd = new Command
            {
                Action = "getproxy",
                Bool1 = basicproxy
            };
            cmd = HttpHelper.GetHtml(cmd, null, null);
            if (!cmd.IsOk)
                return new List<string>();
            var result = new List<string>();
            if (cmd.Dic1 == null)
                return result;
            result.AddRange(cmd.Dic1.Select(s => s.Key));
            return result;
        }

        #endregion

        #region 属性

        internal static string Runhash;

        /// <summary>
        ///     应用标识
        /// </summary>
        public static string AppKey
        {
            get;
            set;
        }

        /// <summary>
        ///     Sessionid
        /// </summary>
        public static string Sessionid
        {
            get;
            internal set;
        }


        /// <summary>
        ///     用户信息
        /// </summary>
        public static MyInfo MyInfo
        {
            get;
            internal set;
        }

        /// <summary>
        ///     软件信息
        /// </summary>
        public static MySoft MySoft
        {
            get;
            internal set;
        }

        /// <summary>
        ///     授权信息
        /// </summary>
        public static MyLicense MyLicense
        {
            get;
            internal set;
        }

        #endregion

        #region 数据存储方法

        /// <summary>
        ///     将二进制数据保存到服务器上
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool SaveData(byte[] data)
        {
            if (MyLicense == null)
                return false;
            var cmd = new Command
            {
                Action = "savedata",
                Number1 = MyLicense.Id,
                Bytes1 = data
            };
            cmd = HttpHelper.GetHtml(cmd, null, null);
            return cmd.IsOk;
        }

        /// <summary>
        ///     从服务器上加载二进制数据
        /// </summary>
        /// <returns></returns>
        public static byte[] LoadData()
        {
            if (MyLicense == null)
                return null;
            var cmd = new Command
            {
                Action = "loaddata",
                Number1 = MyLicense.Id
            };
            cmd = HttpHelper.GetHtml(cmd, null, null);
            return cmd.IsOk ? cmd.Bytes1 : null;
        }

        #endregion

        #region 登录相关

        public static bool IsSuperxUser()
        {
            var cmd = new Command
            {
                Action = "checksuperx",
            };

            cmd = HttpHelper.GetHtml(cmd, null, null);
            return cmd.IsOk;
        }

        /// <summary>
        ///     授权登录
        /// </summary>
        /// <param name="licensekey"></param>
        /// <returns></returns>
        public static string LicenseInit(string licensekey)
        {
            var cmd = new Command
            {
                Action = "licensekeyinit",
                String1 = licensekey,
                String2 = AppKey
            };

            cmd = HttpHelper.GetHtml(cmd, null, null);
            if (cmd.IsOk)
            {
                MyInfo = GetMyInfo(cmd.Dic1);
                MyLicense = GetMyLicense(cmd.Dic2);
                MySoft = GetMySoft(cmd.Dic3);
                Sessionid = cmd.Sessionid;
                return "true";
            }

            return cmd.Message;
        }

        /// <summary>
        ///     注销登录
        /// </summary>
        public static void Logout()
        {
            Sessionid = "";
            MyInfo = null;
            MySoft = null;
            MyLicense = null;
        }

        /// <summary>
        ///     登录方法
        /// </summary>
        /// <param name="email">账号</param>
        /// <param name="password">密码</param>
        /// <returns>返回"true"表示登录成功，其他则为失败信息</returns>
        public static string Login(string email, string password)
        {
            if (MyInfo != null)
                return "license logged";
            if (email != null && password != null && email != "demo")
            {
                var cmd = Logincmd(email, password);
                return cmd.IsOk ? "true" : cmd.Message;
            }
            return "email or password is null";
        }

        public static void UpdateInfo()
        {
            var cmd = new Command
            {
                Action = "updateinfo"
            };
            cmd = HttpHelper.GetHtml(cmd, null, null);
            Sessionid = cmd.Sessionid;
            MyInfo = GetMyInfo(cmd.Dic1);
        }

        private static Command Logincmd(string email, string password)
        {
            var cmd = new Command
            {
                Action = "login",
                String1 = email,
                String2 = password
            };
            cmd = HttpHelper.GetHtml(cmd, null, null);
            if (!cmd.IsOk)
                return cmd;
            //取出授权信息
            Sessionid = cmd.Sessionid;
            MyInfo = GetMyInfo(cmd.Dic1);
            return cmd;
        }

        internal static MyInfo GetMyInfo(Dictionary<string, string> hss)
        {
            var info = new MyInfo
            {
                Id = Convert.ToInt32(GetValue(hss, "id")),
                Email = GetValue(hss, "email"),
                Money = Convert.ToDouble(GetValue(hss, "money")),
                Name = GetValue(hss, "name"),
                Phone = GetValue(hss, "phone"),
                Photo = GetValue(hss, "photo"),
                Power = Convert.ToInt32(GetValue(hss, "power")),
                Qq = GetValue(hss, "qq"),
                WeixinOpenid = GetValue(hss, "user_weixinOpenid"),
                QqOpenid = GetValue(hss, "user_qqOpenid"),
                Newpm = Convert.ToInt32(GetValue(hss, "user_newpm")),
                MyKey = GetValue(hss, "user_key")
            };

            if (MyInfo == null)
            {
                info.LoginIp = GetValue(hss, "loginip");
                info.LoginTime = DateTime.Parse(GetValue(hss, "logintime"));
                info.Face = Base32.FromBase32String(GetValue(hss, "face"));
            }
            else
            {
                info.LoginIp = MyInfo.LoginIp;
                info.LoginTime = MyInfo.LoginTime;
                info.Face = MyInfo.Face;
            }
            return info;
        }

        internal static MySoft GetMySoft(Dictionary<string, string> hss)
        {
            var mysoft = new MySoft
            {
                Id = Convert.ToInt32(GetValue(hss, "soft_id")),
                Key = GetValue(hss, "soft_key"),
                Name = GetValue(hss, "soft_name"),
                Version = GetValue(hss, "soft_ver")
            };


            return mysoft;
        }

        internal static MyLicense GetMyLicense(object hs)
        {
            var hss = hs as Dictionary<string, string>;
            var mylicense = new MyLicense
            {
                Key = GetValue(hss, "license_codeKey"),
                Level = Convert.ToInt32(GetValue(hss, "license_level")),
                Expiration = DateTime.Parse(GetValue(hss, "license_expiration")),
                Name = GetValue(hss, "license_name"),
                Id = Convert.ToInt32(GetValue(hss, "license_id")),
                Custom = GetValue(hss, "license_custom"),
                Description = GetValue(hss, "license_description")
            };
            Runhash = GetValue(hss, "hash");
            LicenseAlive(mylicense.Id);
            return mylicense;
        }

        private static void LicenseAlive(int licenseid)
        {
            var thread = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(60 * 1000);
                    if (MyLicense != null)
                    {
                        var cmd = new Command
                        {
                            Action = "licensealive",
                            String1 = MySoft.Id.ToString(),
                            String2 = licenseid.ToString(),
                            String3 = Runhash
                        };
                        cmd = HttpHelper.GetHtml(cmd, null, null);
                        if (!cmd.IsOk && cmd.String1 == "killme")
                        {
                            File.WriteAllText("error.txt", DateTime.Now + ":\r\n" + cmd.Message, Encoding.UTF8);
                            Environment.Exit(999);
                        }
                    }
                }
            })
            {
                IsBackground = true
            };
            thread.Start();
        }

        /// <summary>
        /// </summary>
        /// <param name="hs"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetValue(Dictionary<string, string> hs, string name)
        {
            if (hs != null)
                return hs.ContainsKey(name) ? hs[name] : "";
            return "";
        }

        #endregion
    }
}