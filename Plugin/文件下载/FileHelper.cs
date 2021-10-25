using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 文件下载
{
   public class FileHelper
    {
        public enum FileExtension
        {
            JPG = 255216,
            GIF = 7173,
            BMP = 6677,
            PNG = 13780,
            COM = 7790,
            EXE = 7790,
            DLL = 7790,
            RAR = 8297,
            ZIP = 8075,
            XML = 6063,
            HTML = 6033,
            ASPX = 239187,
            CS = 117115,
            JS = 119105,
            TXT = 210187,
            SQL = 255254,
            BAT = 64101,
            torrent = 10056,
            RDP = 255254,
            PSD = 5666,
            PDF = 3780,
            CHM = 7384,
            LOG = 70105,
            REG = 8269,
            HLP = 6395,
            DOC = 208207,
            XLS = 208207,
            DOCX = 208207,
            XLSX = 208207,
        }

        public static string GetExtName(byte[] r)
        {


            string fileclass = "";
            byte buffer;
            try
            {
                buffer = r[0];
                fileclass = buffer.ToString();
                buffer = r[1];
                fileclass += buffer.ToString();
            }
            catch
            {
                return "unknow";
            }
            foreach (FileExtension item in FileExtension.GetValues(typeof(FileExtension)))
            {

                if (Convert.ToInt32(item).ToString() == fileclass)
                {
                    return item.ToString().ToLower();
                }

            }
            return "unknow";

        }
    }
}
