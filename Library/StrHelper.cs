using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Library
{
  public  class StrHelper
  {
      public static string Md5(string ConvertString)
      {
          MD5 m = new MD5CryptoServiceProvider();
          byte[] s = m.ComputeHash(UnicodeEncoding.UTF8.GetBytes(ConvertString + "xstore!@#XXXSDFSDF@#$@!@!@#SDFXXCVVVVVVVVVVVV" + "@#$@#$SDFSDF"));
          return BitConverter.ToString(s).Replace("-", "");
      }

      public static string Md5(string str, bool is32)
      {
          if (!is32) //16位MD5加密（取32位加密的9~25字符） 
          {
              return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower().Substring(8, 16);
          }
          else//32位加密 
          {
              return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower();
          }
      }
      public static string GetMd5Str32(string ConvertString)
      {
          MD5 m = new MD5CryptoServiceProvider();
          byte[] s = m.ComputeHash(UnicodeEncoding.UTF8.GetBytes(ConvertString + "xstore!@#XXXSDFSDF@#$@!@!@#SDFXXCVVVVVVVVVVVV" + "@#$@#$SDFSDF"));
          return BitConverter.ToString(s).Replace("-", "");
      }

      public static string SetFileName(string filename)
      {
          string[] bad = new string[25];

          bad[0] = "'";
          bad[1] = "\"";
          bad[2] = ";";
          bad[3] = "-";
          bad[4] = ",";
          bad[5] = "!";
          bad[6] = "~";
          bad[7] = "@";
          bad[8] = "#";
          bad[9] = "$";
          bad[10] = "%";
          bad[11] = "^";
          bad[12] = "&";
          bad[13] = "  ";
          bad[14] = "_";
          bad[15] = "|";
          bad[16] = "\r";
          bad[17] = "?";
          bad[18] = "[";
          bad[19] = "]";
          bad[20] = "<";
          bad[21] = ">";
          bad[22] = ":";
          bad[23] = "\n";
          bad[24] = "/";
          foreach (var s in bad)
          {
              filename = filename.Replace(s, "");
          }
          return filename;
      }
      public static int CompareFileName(string x, string y)
      {
          string xx = x.Substring(x.LastIndexOf("\\") + 1);
          xx = xx.Remove(xx.LastIndexOf("."));
          string yy = y.Substring(y.LastIndexOf("\\") + 1);
          yy = yy.Remove(yy.LastIndexOf("."));
          return int.Parse(xx).CompareTo(int.Parse(yy));
      }

      public static int CompareDirectoryName(string x, string y)
      {
          return int.Parse(x).CompareTo(int.Parse(y));
      }

  }
}
