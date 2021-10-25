using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace V3.Common
{
   public class xFile
    {
        public static bool SaveFile(string path, string filename, byte[] content)
        {
            lock ("xFiledata")
            {
                bool result = false;
                try
                {
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    File.WriteAllBytes(path + filename + ".membuff", content);


                    File.WriteAllBytes(path + filename + ".mem", content);
                    result = true;
                }
                catch (Exception ex)
                {
                    V3.Common.Log.LogNewline("[c14]文件保存失败:"+ex.Message+" [/c]");
             
                   
                }
                return result;
            }
        }

       public static byte[] ReadFile(string path, string filename)
       {  
           byte[] result = null;
           if (File.Exists(path + filename + ".mem") || File.Exists(path + filename + ".membuff"))
           {
               lock ("xFiledata")
               {
                 
                   try
                   {
                       result = File.ReadAllBytes(path + filename + ".mem");
                       if (result.Length == 0)
                           result = File.ReadAllBytes(path + filename + ".membuff");

                   }
                   catch (Exception ex)
                   {



                       V3.Common.Log.LogNewline("[c14]文件读取失败:" + ex.Message + " ！[/c]");
                   }
               }

           }
           return result;
       }

       public static bool SaveFileNoBuff(string path, string filename, byte[] content)
        {
           bool result = false;
           lock ("xFiledata")
           {

               try
               {
                   if (!Directory.Exists(path))
                       Directory.CreateDirectory(path);
                   File.WriteAllBytes(path + filename + ".mem", content);
                   result = true;
               }
               catch (Exception ex)
               {
                   V3.Common.Log.LogNewline("[c14]文件保存失败:" + ex.Message + "[/c]");
               }

           }

           return result;
        }
        public static byte[] ReadFileNoBuff(string path, string filename)
       {
           byte[] result = null;
           if (File.Exists(path + filename + ".mem"))
            {
                lock ("xFiledata")
                {

                    try
                    {
                        result = File.ReadAllBytes(path + filename + ".mem");

                    }
                    catch (Exception ex)
                    {
                        V3.Common.Log.LogNewline("[c14]文件读取失败:" + ex.Message + "[/c]");
                    }

                }
            }
            return result;
        }

       public static bool DelFile(string path, string filename)
       {
           bool result = false;
           
               lock ("xFiledata")
               {
                  
                   try
                   {
                       File.Delete(path + filename + ".mem");
                       result = true;
                   }
                   catch (Exception ex)
                   {
                       V3.Common.Log.LogNewline("[c14]文件删除失败:" + ex.Message + "[/c]");
                   }
                   
               }
           
           return result;
       }
    }
}
