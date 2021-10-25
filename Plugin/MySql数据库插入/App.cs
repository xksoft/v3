using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using V3Plugin;

namespace MySql数据库插入
{
    public class App : ProcessPlugin
    {
        public static frmMain frm = new frmMain();
        public UserControl MainControl
        {
            get
            {

                return frm;
            }
        }
        //任务关键词库关键词列表
        public List<string> KeyWords
        {
            get;
            set;
        }
        //插件唯一编号，供V3识别，不允许重复，在http://www.xiake.org/guid.html页面获取，一旦设置请不要修改
        public string Id
        {

            get { return "FDFB24D8-863E-F10E-90CF-BEC207FF18B7"; }
        }

        //个人信息以及联系方式，插件出现错误即会显示该信息以便用户联系反馈
        public string Author
        {
            get { return "小易 QQ：24271786"; }
        }

        //插件进程名称，供V3内部代码调用显示
        public string ProcessName
        {
            get { return "MySql数据库插入"; }
        }
        //由V3传入的30个文章模型值，从0-29
        public Dictionary<int, string> ArticleProcess(Dictionary<int, string> objects)
        {
            string sql = Parameters[0];
            for (int i = 0; i < 29; i++)
            {

                sql = sql.Replace("[模型值" + (i + 1) + "]", objects[i]);

            }
            MySqlHelper.ConnectionStringManager = Parameters[1];
            try
            {
                if (MySqlHelper.ExecuteNonQuery(sql) > 0)
                {
                    Console.WriteLine("[MySql数据插入]:成功插入数据到远程数据库中！");
                }
                else
                {
                    Console.WriteLine("[MySql数据插入]:无法插入数据到指定的远程数据库！");
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("[MySql数据插入]执行错误：" + error.Message);
            }
            return objects;
        }

        public object Clone()
        {
            return new App();
        }



        //自定义配置参数，由V3调用时会触发set，在set里面需对参数进行判断。保存V3任务时会触发get，注意对参数初始化，插件二次升级时请做好默认参数判断。
        public string[] Parameters
        {
            get
            {
                string[] s = new string[2];
               
                s[0] = frm.textBox_Sql.Text;
                s[1] = frm.textBox_ConnStr.Text;
                return s;
            }
            set
            {
                if (value == null) { value = new string[2]; }
                if (value.Length == 2)
                {
                    frm.textBox_Sql.Text = value[0];
                    frm.textBox_ConnStr.Text = value[1];
                }

            }
        }
    }
}
