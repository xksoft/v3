using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace xEngine.Common
{
    public static class IP
    {
        private static QqWryLocator _qqWry;

        public static string GetIpInfo(string ip)
        {
            var regex =
                new Regex(@"(((\d{1,2})|(1\d{2})|(2[0-4]\d)|(25[0-5]))\.){3}((\d{1,2})|(1\d{2})|(2[0-4]\d)|(25[0-5]))");
            if (!regex.Match(ip).Success)
            {
                return "来自火星的IP";
            }
            if (ip == null)
                return "未知IP";
            if (!ip.Contains("."))
                return "未知IP";
            if (_qqWry == null)
            {
                try
                {
                    _qqWry = new QqWryLocator("QQWry.Dat");
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
   
            //初始化数据库文件，并获得IP记录数，通过Count可以获得
            var ipinfo = _qqWry.Query(ip); //查询一个IP地址
            return ipinfo.Country + ipinfo.Local;
        }
    }

    public class IpLocation
    {
        public string Ip { get; set; }
        public string Country { get; set; }
        public string Local { get; set; }
    }

    public class QqWryLocator
    {
        private readonly byte[] _data;
        private readonly long _firstStartIpOffset;

        private readonly Regex _regex =
            new Regex(@"(((\d{1,2})|(1\d{2})|(2[0-4]\d)|(25[0-5]))\.){3}((\d{1,2})|(1\d{2})|(2[0-4]\d)|(25[0-5]))");

        public readonly long Count;

        public QqWryLocator(string dataPath)
        {
            using (var fs = new FileStream(dataPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                _data = new byte[fs.Length];
                fs.Read(_data, 0, _data.Length);
            }
            var buffer = new byte[8];
            Array.Copy(_data, 0, buffer, 0, 8);
            _firstStartIpOffset = ((buffer[0] + (buffer[1]*0x100)) + ((buffer[2]*0x100)*0x100)) +
                                  (((buffer[3]*0x100)*0x100)*0x100);
            long lastStartIpOffset = ((buffer[4] + (buffer[5]*0x100)) + ((buffer[6]*0x100)*0x100)) +
                                     (((buffer[7]*0x100)*0x100)*0x100);
            Count = Convert.ToInt64((lastStartIpOffset - _firstStartIpOffset)/7.0);

            if (Count <= 1L)
            {
                throw new ArgumentException("ip FileDataError");
            }
        }

        private static long IpToInt(string ip)
        {
            var separator = new[] {'.'};
            if (ip.Split(separator).Length == 3)
            {
                ip = ip + ".0";
            }
            var strArray = ip.Split(separator);
            var num2 = ((long.Parse(strArray[0])*0x100L)*0x100L)*0x100L;
            var num3 = (long.Parse(strArray[1])*0x100L)*0x100L;
            var num4 = long.Parse(strArray[2])*0x100L;
            var num5 = long.Parse(strArray[3]);
            return (((num2 + num3) + num4) + num5);
        }

        private static string IntToIp(long ipInt)
        {
            var num = (ipInt & 0xff000000L) >> 0x18;
            if (num < 0L)
            {
                num += 0x100L;
            }
            var num2 = (ipInt & 0xff0000L) >> 0x10;
            if (num2 < 0L)
            {
                num2 += 0x100L;
            }
            var num3 = (ipInt & 0xff00L) >> 8;
            if (num3 < 0L)
            {
                num3 += 0x100L;
            }
            var num4 = ipInt & 0xffL;
            if (num4 < 0L)
            {
                num4 += 0x100L;
            }
            return (num + "." + num2 + "." + num3 + "." + num4);
        }

        public IpLocation Query(string ip)
        {
            if (!_regex.Match(ip).Success)
            {
                throw new ArgumentException("IP格式错误");
            }
            var ipLocation = new IpLocation
            {
                Ip = ip
            };
            var intIp = IpToInt(ip);
            if ((intIp >= IpToInt("127.0.0.1") && (intIp <= IpToInt("127.255.255.255"))))
            {
                ipLocation.Country = "本机内部环回地址";
                ipLocation.Local = "";
            }
            else
            {
                if ((((intIp >= IpToInt("0.0.0.0")) && (intIp <= IpToInt("2.255.255.255"))) ||
                     ((intIp >= IpToInt("64.0.0.0")) && (intIp <= IpToInt("126.255.255.255")))) ||
                    ((intIp >= IpToInt("58.0.0.0")) && (intIp <= IpToInt("60.255.255.255"))))
                {
                    ipLocation.Country = "网络保留地址";
                    ipLocation.Local = "";
                }
            }
            var right = Count;
            var left = 0L;
            var middle = 0L;
            var startIp = 0L;
            var endIpOff = 0L;
            var endIp = 0L;
            var countryFlag = 0;
            while (left < (right - 1L))
            {
                middle = (right + left)/2L;
                startIp = GetStartIp(middle, out endIpOff);
                if (intIp == startIp)
                {
                    left = middle;
                    break;
                }
                if (intIp > startIp)
                {
                    left = middle;
                }
                else
                {
                    right = middle;
                }
            }
            startIp = GetStartIp(left, out endIpOff);
            endIp = GetEndIp(endIpOff, out countryFlag);
            if ((startIp <= intIp) && (endIp >= intIp))
            {
                string local;
                ipLocation.Country = GetCountry(endIpOff, countryFlag, out local);
                ipLocation.Local = local;
            }
            else
            {
                ipLocation.Country = "未知";
                ipLocation.Local = "";
            }
            return ipLocation;
        }

        private long GetStartIp(long left, out long endIpOff)
        {
            var leftOffset = _firstStartIpOffset + (left*7L);
            var buffer = new byte[7];
            Array.Copy(_data, leftOffset, buffer, 0, 7);
            endIpOff = (Convert.ToInt64(buffer[4].ToString()) + (Convert.ToInt64(buffer[5].ToString())*0x100L)) +
                       ((Convert.ToInt64(buffer[6].ToString())*0x100L)*0x100L);
            return ((Convert.ToInt64(buffer[0].ToString()) + (Convert.ToInt64(buffer[1].ToString())*0x100L)) +
                    ((Convert.ToInt64(buffer[2].ToString())*0x100L)*0x100L)) +
                   (((Convert.ToInt64(buffer[3].ToString())*0x100L)*0x100L)*0x100L);
        }

        private long GetEndIp(long endIpOff, out int countryFlag)
        {
            var buffer = new byte[5];
            Array.Copy(_data, endIpOff, buffer, 0, 5);
            countryFlag = buffer[4];
            return ((Convert.ToInt64(buffer[0].ToString()) + (Convert.ToInt64(buffer[1].ToString())*0x100L)) +
                    ((Convert.ToInt64(buffer[2].ToString())*0x100L)*0x100L)) +
                   (((Convert.ToInt64(buffer[3].ToString())*0x100L)*0x100L)*0x100L);
        }

        /// <summary>
        ///     Gets the country.
        /// </summary>
        /// <param name="endIpOff">The end ip off.</param>
        /// <param name="countryFlag">The country flag.</param>
        /// <param name="local">The local.</param>
        /// <returns>country</returns>
        private string GetCountry(long endIpOff, int countryFlag, out string local)
        {
            var country = "";
            var offset = endIpOff + 4L;
            switch (countryFlag)
            {
                case 1:
                case 2:
                    country = GetFlagStr(ref offset, ref countryFlag, ref endIpOff);
                    offset = endIpOff + 8L;
                    local = (1 == countryFlag) ? "" : GetFlagStr(ref offset, ref countryFlag, ref endIpOff);
                    break;
                default:
                    country = GetFlagStr(ref offset, ref countryFlag, ref endIpOff);
                    local = GetFlagStr(ref offset, ref countryFlag, ref endIpOff);
                    break;
            }
            return country;
        }

        private string GetFlagStr(ref long offset, ref int countryFlag, ref long endIpOff)
        {
            var flag = 0;
            var buffer = new byte[3];

            while (true)
            {
                //用于向前累加偏移量
                var forwardOffset = offset;
                flag = _data[forwardOffset++];
                //没有重定向
                if (flag != 1 && flag != 2)
                {
                    break;
                }
                Array.Copy(_data, forwardOffset, buffer, 0, 3);
                forwardOffset += 3;
                if (flag == 2)
                {
                    countryFlag = 2;
                    endIpOff = offset - 4L;
                }
                offset = (Convert.ToInt64(buffer[0].ToString()) + (Convert.ToInt64(buffer[1].ToString())*0x100L)) +
                         ((Convert.ToInt64(buffer[2].ToString())*0x100L)*0x100L);
            }
            if (offset < 12L)
            {
                return "";
            }
            return GetStr(ref offset);
        }

        private string GetStr(ref long offset)
        {
            byte lowByte = 0;
            byte highByte = 0;
            var stringBuilder = new StringBuilder();
            var bytes = new byte[2];
            var encoding = Encoding.GetEncoding("GB2312");
            while (true)
            {
                lowByte = _data[offset++];
                if (lowByte == 0)
                {
                    return stringBuilder.ToString();
                }
                if (lowByte > 0x7f)
                {
                    highByte = _data[offset++];
                    bytes[0] = lowByte;
                    bytes[1] = highByte;
                    if (highByte == 0)
                    {
                        return stringBuilder.ToString();
                    }
                    stringBuilder.Append(encoding.GetString(bytes));
                }
                else
                {
                    stringBuilder.Append((char) lowByte);
                }
            }
        }
    }
}