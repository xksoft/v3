using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xEngine.Common
{
   public  class SocketHelper
    {
       public static byte[] MakeByte(byte[] data, int id)
        {
            var megabyte = new byte[data.Length + 5];
            megabyte[data.Length] = 127;
            megabyte[data.Length + 1] = 1;
            megabyte[data.Length + 2] = 125;
            megabyte[data.Length + 3] = 5;
            megabyte[data.Length + 4] = (byte)id;
            Array.Copy(data, 0, megabyte, 0, data.Length);
            return megabyte;
        }
        public static int FindTag(byte[] data, int startindex, int receiveLength, out int id)
        {
            unsafe
            {
                fixed (byte* d = data)
                {
                    for (var i = startindex; i < receiveLength; i++)
                    {
                        if (receiveLength - i >= 5)
                        {
                            if (d[i] == 127 && d[i + 1] == 1 && d[i + 2] == 125 && d[i + 3] == 5)
                            {
                                id = d[i + 4];
                                return i;
                            }
                        }
                    }
                }
            }
            id = 0;
            return -1;
        }
    }
}
