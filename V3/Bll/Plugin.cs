using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.IO;
using V3Plugin;

namespace V3.Bll
{
    class Plugin
    {

      


        /// <summary>
        /// 判断DLL中是否继承了Iplugin接口
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static bool IsValidPlugin(Type t)
        {
            bool ret = false;
            Type[] interfaces = t.GetInterfaces();
            foreach (Type theInterface in interfaces)
            {
                if (theInterface.FullName == "V3Plugin.ProcessPlugin")
                {
                    ret = true;
                    break;
                }
            }
            return ret;
        }



        /// <summary>
        /// 加载插件，这里是在Debug的plugin目录中搜索插件
        /// </summary>
        public static Dictionary<string, ProcessPlugin> LoadAllPlugins()
        {
           Dictionary<string,ProcessPlugin>plugins=new Dictionary<string, ProcessPlugin>();
           string[] dirs = Directory.GetDirectories(Program.ApplicationPath + "\\处理插件\\");
            foreach (var dir in dirs)
            {
                string[] files = Directory.GetFiles(dir);
                int i = 0;
                foreach (string file in files)
                {
                    string ext = file.Substring(file.LastIndexOf("."));
                    if (ext != ".dll") continue;
                    try
                    {
                        Assembly tmp = Assembly.LoadFile(file);
                        Type[] types = tmp.GetTypes();
                        bool ok = false;
                        foreach (Type t in types)
                            if (IsValidPlugin(t))
                            {
                                ProcessPlugin plugin = (ProcessPlugin)tmp.CreateInstance(t.FullName);
                                
                              plugin.MainControl.Dock = DockStyle.Top;
                                plugins.Add(plugin.Id, plugin);
                                ok = true;
                                if (ok) break;
                            }
                    }
                    catch (Exception err)
                    {
                        V3.Common.Log.LogNewline("[c14]加载处理插件时出错：" + err.Message+"[/c]");
                    }
                }
                
            }
            
            return plugins;
        }


    }
}
