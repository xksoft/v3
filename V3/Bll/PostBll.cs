using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System.Collections;
using System.Net;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using V3.Common;
using xEngine.Model.Execute.Http;

namespace V3.Bll
{
    public class PostBll
    {
        public PostBll(Model.GetPostModel inmodel, int pointid, int taskid)
        {
            this.account = new Model.发布相关模型.Account();
            this.model = inmodel;
            this.pointid = pointid;
            this.taskid = taskid;
            if (Model.V3Infos.TaskDb.ContainsKey(taskid))
            {
                Task = Model.V3Infos.TaskDb[taskid];
            }
            else 
            {
                Task = new Model.Task();
            }
            article.DataObject = new Dictionary<int, string>()
            {
            { 0,""},
            { 1,""},
            { 2,""},
            { 3,""},
            { 4,""},
            { 5,""},
            { 6,""},
            { 7,""},
            { 8,""},
            { 9,""},
            { 10,""},
            { 11,""},
            { 12,""},
            { 13,""},
            { 14,""},
            { 15,""},
            { 16,""},
            { 17,""},
            { 18,""},
            { 19,""},
            { 20,""},
            { 21,""},
            { 22,""},
            { 23,""},
            { 24,""},
            { 25,""},
            { 26,""},
            { 27,""},
            { 28,""},
            { 29,""}
            };
        }
       public int pointid = 0;
        int taskid = 0;
        public Model.Task Task = new Model.Task();
        public Model.GetPostModel model = new Model.GetPostModel();
        public Model.Model_Article article = new Model.Model_Article();
        public Model.发布相关模型.Account account = new Model.发布相关模型.Account();
        public string ReplaceTag(string str, bool istest, string oldhtml, bool isfirst)
        {
            Regex myr = new Regex("<.+?>");
            Random r = new Random();

            #region 正则变量
            ArrayList regexlist = Library.RegexHelper.GetArrayList(str, "(?<=〖).*?(?=〗)");
            for (int i = 0; i < regexlist.Count; i++)
            {
                if (regexlist[i].ToString().Contains("验证码KEY"))
                {
                    regexlist[i] = regexlist[i].ToString().Replace("验证码KEY", "");
                    ArrayList temp = Library.RegexHelper.GetArrayList(oldhtml, regexlist[i].ToString());
                    for (int ii = 0; ii < temp.Count; ii++)
                    {
                        str = str.Replace("〖验证码KEY" + regexlist[i].ToString() + "〗", temp[ii].ToString());
                    }
                    str = str.Replace("〖验证码KEY" + regexlist[i].ToString() + "〗", "");

                }
                else if (regexlist[i].ToString().Contains("新浪认证SERVERTIME"))
                {

                    regexlist[i] = regexlist[i].ToString().Replace("新浪认证SERVERTIME", "");
                    ArrayList temp = Library.RegexHelper.GetArrayList(oldhtml, regexlist[i].ToString());
                    for (int ii = 0; ii < temp.Count; ii++)
                    {
                        servertime = temp[ii].ToString();
                        str = str.Replace("〖新浪认证SERVERTIME" + regexlist[i].ToString() + "〗", temp[ii].ToString());
                    }
                    str = str.Replace("〖新浪认证SERVERTIME" + regexlist[i].ToString() + "〗", "");

                }
                else if (regexlist[i].ToString().Contains("新浪认证NONCE"))
                {

                    regexlist[i] = regexlist[i].ToString().Replace("新浪认证NONCE", "");
                    ArrayList temp = Library.RegexHelper.GetArrayList(oldhtml, regexlist[i].ToString());
                    for (int ii = 0; ii < temp.Count; ii++)
                    {
                        nonce = temp[ii].ToString();
                        str = str.Replace("〖新浪认证NONCE" + regexlist[i].ToString() + "〗", temp[ii].ToString());
                    }
                    str = str.Replace("〖新浪认证NONCE" + regexlist[i].ToString() + "〗", "");
                }
                else
                {
                    ArrayList temp = Library.RegexHelper.GetArrayList(oldhtml, regexlist[i].ToString());
                    for (int ii = 0; ii < temp.Count; ii++)
                    {
                        str = str.Replace("〖" + regexlist[i].ToString() + "〗", temp[ii].ToString());
                    }
                    str = str.Replace("〖" + regexlist[i].ToString() + "〗", "");
                }
            }
            #endregion

            #region 特殊辅助变量
            if (str.Contains("【新浪密码】"))
            {
                try
                {
                    str = str.Replace("【新浪密码】", getSinaPassword(account.password));
                }
                catch
                {
                    str = str.Replace("【新浪密码】", "");
                }
            }
            if (str.Contains("【新浪用户名】"))
            {
                try
                {
                    str = str.Replace("【新浪用户名】", GetSinaUserName(account.username));
                }
                catch
                {
                    str = str.Replace("【新浪用户名】", "");
                }
            }
            #endregion

            #region 常规变量
            if (str.Contains("【随机关键词】"))
            {
                try
                {
                    int KeywordDbId = 0;
                    KeywordDbId = Task.KeywordDbId;
                    if (KeywordDbId==0) { KeywordDbId = Model.V3Infos.SendPointDb[pointid].KeywordDbID; }


                        while (str.Contains("【随机关键词】"))
                        {
                            string qian = str.Remove(str.IndexOf("【随机关键词】"));
                            string hou = str.Substring(str.IndexOf("【随机关键词】") + 7);

                            str = qian + Bll.DbBll.GetRandomKeyword(KeywordDbId.ToString()) + hou;


                        }
                       
                    
                }
                catch
                {
                    str = str.Replace("【随机关键词】", "");
                }
            }
            if (str.Contains("【后台地址】"))
            {
                try
                {
                    if (istest || pointid == 0)
                    {
                        if (model.Testadminurl == "")
                            str = str.Replace("【后台地址】", Model.V3Infos.SendPointDb[pointid].AdminUrl);
                        else
                            str = str.Replace("【后台地址】", model.Testadminurl);
                    }
                    else
                    {
                        str = str.Replace("【后台地址】", Model.V3Infos.SendPointDb[pointid].AdminUrl);
                    }
                }
                catch
                {
                    str = str.Replace("【后台地址】", "");
                }
            }

            if (str.Contains("【根域名】"))
            {
                try
                {
                    if (istest || pointid == 0)
                    {
                        str = str.Replace("【根域名】", new Uri(model.Testadminurl).Host);
                    }
                    else
                    {
                        str = str.Replace("【根域名】", new Uri(Model.V3Infos.SendPointDb[pointid].AdminUrl).Host);
                    }

                }
                catch
                {
                    str = str.Replace("【根域名】", "");
                }
            }


            if (str.Contains("【登录账号】"))
            {
                try
                {
                    if (istest || pointid == 0)
                    {
                        str = str.Replace("【登录账号】", model.POST_TestAccount.username);
                    }
                    else
                    {
                        str = str.Replace("【登录账号】", account.username);
                    }
                }
                catch
                {
                    str = str.Replace("【登录账号】", "");
                }
            }

            if (str.Contains("【登录密码】"))
            {
                try
                {
                    if (istest || pointid == 0)
                    {
                        str = str.Replace("【登录密码】", model.POST_TestAccount.password);
                    }
                    else
                    {
                        str = str.Replace("【登录密码】", account.password);
                    }
                }
                catch
                {
                    str = str.Replace("【登录密码】", "");
                }
            }

            if (str.Contains("【登录密码MD5(16)】"))
            {
                try
                {
                    if (istest || pointid == 0)
                    {
                        str = str.Replace("【登录密码MD5(16)】", Library.StrHelper.Md5(model.POST_TestAccount.password, false));
                    }
                    else
                    {
                        str = str.Replace("【登录密码MD5(16)】", Library.StrHelper.Md5(this.account.password, false));
                    }
                }
                catch
                {
                    str = str.Replace("【登录密码MD5(16)】", "");
                }
            }


            if (str.Contains("【登录密码MD5(32)】"))
            {
                try
                {
                    if (istest || pointid == 0)
                    {
                        str = str.Replace("【登录密码MD5(32)】", Library.StrHelper.Md5(model.POST_TestAccount.password, true));
                    }
                    else
                    {
                        str = str.Replace("【登录密码MD5(32)】", Library.StrHelper.Md5(this.account.password, true));
                    }
                }
                catch
                {
                    str = str.Replace("【登录密码MD5(32)】", "");
                }
            }

            if (str.Contains("【登录附加值1】"))
            {
                try
                {
                    if (istest || pointid == 0)
                    {
                        str = str.Replace("【登录附加值1】", model.POST_TestAccount.loginvalue1);
                    }
                    else
                    {
                        str = str.Replace("【登录附加值1】", this.account.loginvalue1);
                    }
                }
                catch
                {
                    str = str.Replace("【登录附加值1】", "");
                }
            }


            if (str.Contains("【登录附加值2】"))
            {
                try
                {
                    if (istest || pointid == 0)
                    {
                        str = str.Replace("【登录附加值2】", model.POST_TestAccount.loginvalue2);
                    }
                    else
                    {
                        str = str.Replace("【登录附加值2】", this.account.loginvalue2);
                    }
                }
                catch
                {
                    str = str.Replace("【登录附加值2】", "");
                }
            }


            if (str.Contains("【来源】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【来源】", "站群软件");
                    }
                    else
                    {
                        str = str.Replace("【来源】", Model.V3Infos.MainDb.Laiyuan.Split('|')[r.Next(0, Model.V3Infos.MainDb.Laiyuan.Split('|').Length)]);
                    }
                }
                catch
                {
                    str = str.Replace("【来源】", "");
                }
            }


            if (str.Contains("【作者】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【作者】", "xiaoxia");
                    }
                    else
                    {
                        str = str.Replace("【作者】", Model.V3Infos.MainDb.Zuozhe.Split('|')[r.Next(0, Model.V3Infos.MainDb.Zuozhe.Split('|').Length)]);
                    }
                }
                catch
                {
                    str = str.Replace("【作者】", "");
                }
            }


            if (str.Contains("【发布分类】"))
            {
                try
                {
                    if (AddClassName != "")
                    {
                        str = str.Replace("【发布分类】", AddClassName);
                        AddClassName = "";
                    }
                    else if (fenleistr != "")
                    {
                        string fenleiid = fenleistr.Split('|')[r.Next(0, fenleistr.Split('|').Length)];
                        str = str.Replace("【发布分类】", fenleiid);
                    }
                    else
                        str = str.Replace("【发布分类】", "");

                }
                catch
                {
                    str = str.Replace("【发布分类】", "");
                }
            }

            if (str.Contains("【Unix时间戳】"))
            {
                str = str.Replace("【Unix时间戳】", ((DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1))).TotalSeconds).ToString());
            }

            if (str.Contains("【时间:年】"))
            {

                str = str.Replace("【时间:年】", DateTime.Now.ToString("yyyy"));

            }


            if (str.Contains("【时间:月】"))
            {

                str = str.Replace("【时间:月】", DateTime.Now.ToString("MM"));

            }


            if (str.Contains("【时间:日】"))
            {

                str = str.Replace("【时间:日】", DateTime.Now.ToString("dd"));

            }


            if (str.Contains("【时间:时】"))
            {

                str = str.Replace("【时间:时】", DateTime.Now.ToString("HH"));

            }


            if (str.Contains("【时间:分】"))
            {

                str = str.Replace("【时间:分】", DateTime.Now.ToString("mm"));

            }


            if (str.Contains("【时间:秒】"))
            {

                str = str.Replace("【时间:秒】", DateTime.Now.ToString("ss"));

            }


            if (str.Contains("【时间:毫秒】"))
            {

                str = str.Replace("【时间:毫秒】", DateTime.Now.ToString("ff"));

            }


            if (str.Contains("【随机数字1位】"))
            {

                str = str.Replace("【随机数字1位】", r.Next(0, 9).ToString());

            }

            if (str.Contains("【随机数字2位】"))
            {
                str = str.Replace("【随机数字2位】", r.Next(10, 99).ToString());
            }

            if (str.Contains("【随机数字3位】"))
            {
                str = str.Replace("【随机数字3位】", r.Next(100, 999).ToString());
            }

            if (str.Contains("【随机数字4位】"))
            {
                str = str.Replace("【随机数字4位】", r.Next(1000, 9999).ToString());
            }

            if (str.Contains("【随机数字5位】"))
            {
                str = str.Replace("【随机数字5位】", r.Next(10000, 99999).ToString());
            }

            if (str.Contains("【随机数字6位】"))
            {
                str = str.Replace("【随机数字6位】", r.Next(100000, 999999).ToString());
            }

            if (str.Contains("【随机数字7位】"))
            {
                str = str.Replace("【随机数字7位】", r.Next(1000000, 9999999).ToString());
            }

            if (str.Contains("【随机数字8位】"))
            {
                str = str.Replace("【随机数字8位】", r.Next(10000000, 99999999).ToString());
            }

            if (str.Contains("【随机数字9位】"))
            {
                str = str.Replace("【随机数字9位】", r.Next(100000000, 999999999).ToString());
            }
            if (str.Contains("【模型值1】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值1】", "站群引擎测试发布文章");
                    }
                    else
                    {
                        str = str.Replace("【模型值1】", article.DataObject[0]);
                    }
                }
                catch
                {
                    str = str.Replace("【模型值1】", "");
                }
            }

            if (str.Contains("【模型值2】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值2】", "<P>您好，您现在看到的这篇文章是站群引擎V3版本的测试文章！</P><P>看到这篇文章意味着您的发布模块配置已经完成，您可以正常使用V3了！</P><P>by xiaoxia </P><P>(这是模型2)</P><P>如有不明之处可联系客服</P><P>客服电话：0771-2758795</P><P>企业QQ:800033536   </P><P>官方网站：www.xiake.org</P>");
                    }
                    else
                    {
                        str = str.Replace("【模型值2】", article.DataObject[1]);
                    }
                }
                catch
                {
                    str = str.Replace("【模型值2】", "");
                }
            }

            if (str.Contains("【模型值3】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值3】", "<P>本文是站群引擎V3版本的测试文章,模型值3</P>");
                    }
                    else
                    {
                        str = str.Replace("【模型值3】", article.DataObject[2]);
                    }
                }
                catch
                {
                    str = str.Replace("【模型值3】", "");
                }
            }

            if (str.Contains("【模型值4】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值4】", "<P>本文是站群引擎V3版本的测试文章,模型值4</P>");
                    }
                    else
                    {
                        str = str.Replace("【模型值4】", article.DataObject[3]);
                    }
                }
                catch
                {
                    str = str.Replace("【模型值4】", "");
                }
            }

            if (str.Contains("【模型值5】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值5】", "<P>本文是站群引擎V3版本的测试文章,模型值5</P>");
                    }
                    else
                    {
                        str = str.Replace("【模型值5】", article.DataObject[4]);
                    }
                }
                catch
                {
                    str = str.Replace("【模型值5】", "");
                }
            }

            if (str.Contains("【模型值6】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值6】", "<P>本文是站群引擎V3版本的测试文章,模型值6</P>");
                    }
                    else
                    {
                        str = str.Replace("【模型值6】", article.DataObject[5]);
                    }
                }
                catch
                {
                    str = str.Replace("【模型值6】", "");
                }
            }

            if (str.Contains("【模型值7】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值7】", "<P>本文是站群引擎V3版本的测试文章,模型值7</P>");
                    }
                    else
                    {
                        str = str.Replace("【模型值7】", article.DataObject[6]);
                    }
                }
                catch
                {
                    str = str.Replace("【模型值7】", "");
                }
            }

            if (str.Contains("【模型值8】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值8】", "<P>本文是站群引擎V3版本的测试文章,模型值8</P>");
                    }
                    else
                    {
                        str = str.Replace("【模型值8】", article.DataObject[7]);
                    }
                }
                catch
                {
                    str = str.Replace("【模型值8】", "");
                }
            }

            if (str.Contains("【模型值9】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值9】", "<P>本文是站群引擎V3版本的测试文章,模型值9</P>");
                    }
                    else
                    {
                        str = str.Replace("【模型值9】", article.DataObject[8]);
                    }
                }
                catch
                {
                    str = str.Replace("【模型值9】", "");
                }
            }

            if (str.Contains("【模型值10】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值10】", "<P>本文是站群引擎V3版本的测试文章,模型值10</P>");
                    }
                    else
                    {
                        str = str.Replace("【模型值10】", article.DataObject[9]);
                    }
                }
                catch
                {
                    str = str.Replace("【模型值10】", "");
                }
            }

            if (str.Contains("【模型值11】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值11】", "<P>本文是站群引擎V3版本的测试文章,模型值11</P>");
                    }
                    else
                    {
                        str = str.Replace("【模型值11】", article.DataObject[10]);
                    }
                }
                catch
                {
                    str = str.Replace("【模型值11】", "");
                }
            }

            if (str.Contains("【模型值12】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值12】", "<P>本文是站群引擎V3版本的测试文章,模型值12</P>");
                    }
                    else
                    {
                        str = str.Replace("【模型值12】", article.DataObject[11]);
                    }
                }
                catch
                {
                    str = str.Replace("【模型值12】", "");
                }
            }

            if (str.Contains("【模型值13】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值13】", "<P>本文是站群引擎V3版本的测试文章,模型值13</P>");
                    }
                    else
                    {
                        str = str.Replace("【模型值13】", article.DataObject[12]);
                    }
                }
                catch
                {
                    str = str.Replace("【模型值13】", "");
                }
            }

            if (str.Contains("【模型值14】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值14】", "<P>本文是站群引擎V3版本的测试文章,模型值14</P>");
                    }
                    else
                    {
                        str = str.Replace("【模型值14】", article.DataObject[13]);
                    }
                }
                catch
                {
                    str = str.Replace("【模型值14】", "");
                }
            }

            if (str.Contains("【模型值15】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值15】", "<P>本文是站群引擎V3版本的测试文章,模型值15</P>");
                    }
                    else
                    {
                        str = str.Replace("【模型值15】", article.DataObject[14]);
                    }
                }
                catch
                {
                    str = str.Replace("【模型值15】", "");
                }
            }

            if (str.Contains("【模型值16】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值16】", "<P>本文是站群引擎V3版本的测试文章,模型值16</P>");
                    }
                    else
                    {
                        str = str.Replace("【模型值16】", article.DataObject[15]);
                    }
                }
                catch
                {
                    str = str.Replace("【模型值16】", "");
                }
            }

            if (str.Contains("【模型值17】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值17】", "<P>本文是站群引擎V3版本的测试文章,模型值17</P>");
                    }
                    else
                    {
                        str = str.Replace("【模型值17】", article.DataObject[16]);
                    }
                }
                catch
                {
                    str = str.Replace("【模型值17】", "");
                }
            }

            if (str.Contains("【模型值18】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值18】", "<P>本文是站群引擎V3版本的测试文章,模型值18</P>");
                    }
                    else
                    {
                        str = str.Replace("【模型值18】", article.DataObject[17]);
                    }
                }
                catch
                {
                    str = str.Replace("【模型值18】", "");
                }
            }

            if (str.Contains("【模型值19】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值19】", "<P>本文是站群引擎V3版本的测试文章,模型值19</P>");
                    }
                    else
                    {
                        str = str.Replace("【模型值19】", article.DataObject[18]);
                    }
                }
                catch
                {
                    str = str.Replace("【模型值19】", "");
                }
            }

            if (str.Contains("【模型值20】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值20】", "<P>本文是站群引擎V3版本的测试文章,模型值20</P>");
                    }
                    else
                    {
                        str = str.Replace("【模型值20】", article.DataObject[19]);
                    }
                }
                catch
                {
                    str = str.Replace("【模型值20】", "");
                }
            }

            if (str.Contains("【模型值21】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值21】", "<P>本文是站群引擎V3版本的测试文章,模型值21</P>");
                    }
                    else
                    {
                        str = str.Replace("【模型值21】", article.DataObject[20]);
                    }
                }
                catch
                {
                    str = str.Replace("【模型值21】", "");
                }
            }

            if (str.Contains("【模型值22】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值22】", "<P>本文是站群引擎V3版本的测试文章,模型值22</P>");
                    }
                    else
                    {
                        str = str.Replace("【模型值22】", article.DataObject[21]);
                    }
                }
                catch
                {
                    str = str.Replace("【模型值22】", "");
                }
            }

            if (str.Contains("【模型值23】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值23】", "<P>本文是站群引擎V3版本的测试文章,模型值23</P>");
                    }
                    else
                    {
                        str = str.Replace("【模型值23】", article.DataObject[22]);
                    }
                }
                catch
                {
                    str = str.Replace("【模型值23】", "");
                }
            }

            if (str.Contains("【模型值24】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值24】", "<P>本文是站群引擎V3版本的测试文章,模型值24</P>");
                    }
                    else
                    {
                        str = str.Replace("【模型值24】", article.DataObject[23]);
                    }
                }
                catch
                {
                    str = str.Replace("【模型值24】", "");
                }
            }

            if (str.Contains("【模型值25】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值25】", "<P>本文是站群引擎V3版本的测试文章,模型值25</P>");
                    }
                    else
                    {
                        str = str.Replace("【模型值25】", article.DataObject[24]);
                    }
                }
                catch
                {
                    str = str.Replace("【模型值25】", "");
                }
            }

            if (str.Contains("【模型值26】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值26】", "<P>本文是站群引擎V3版本的测试文章,模型值26</P>");
                    }
                    else
                    {
                        str = str.Replace("【模型值26】", article.DataObject[25]);
                    }
                }
                catch
                {
                    str = str.Replace("【模型值26】", "");
                }
            }

            if (str.Contains("【模型值27】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值27】", "<P>本文是站群引擎V3版本的测试文章,模型值27</P>");
                    }
                    else
                    {
                        str = str.Replace("【模型值27】", article.DataObject[26]);
                    }
                }
                catch
                {
                    str = str.Replace("【模型值27】", "");
                }
            }

            if (str.Contains("【模型值28】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值28】", "<P>本文是站群引擎V3版本的测试文章,模型值28</P>");
                    }
                    else
                    {
                        str = str.Replace("【模型值28】", article.DataObject[27]);
                    }
                }
                catch
                {
                    str = str.Replace("【模型值28】", "");
                }
            }

            if (str.Contains("【模型值29】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值29】", "<P>本文是站群引擎V3版本的测试文章,模型值29</P>");
                    }
                    else
                    {
                        str = str.Replace("【模型值29】", article.DataObject[28]);
                    }
                }
                catch
                {
                    str = str.Replace("【模型值29】", "");
                }
            }

            if (str.Contains("【模型值30】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值30】", "<P>本文是站群引擎V3版本的测试文章,模型值30</P>");
                    }
                    else
                    {
                        str = str.Replace("【模型值30】", article.DataObject[29]);
                    }
                }
                catch
                {
                    str = str.Replace("【模型值30】", "");
                }
            }


            if (str.Contains("【模型值1(UBB模式)】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值1(UBB模式)】", "站群引擎V3版测试发布文章");
                    }
                    else
                    {
                        str = str.Replace("【模型值1(UBB模式)】", DoHtmlToUBB(article.DataObject[0]));
                    }
                }
                catch
                {
                    str = str.Replace("【模型值1(UBB模式)】", "");
                }
            }

            if (str.Contains("【模型值2(UBB模式)】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值2(UBB模式)】", DoHtmlToUBB("<P>您好，您现在看到的这篇文章是站群引擎V3版本的测试文章！</P><P>看到这篇文章意味着您的发布模块配置已经完成，您可以正常使用V3了！</P><P>by xiaoxia </P><P>(这是模型2)</P><P>如有不明之处可联系客服</P><P>客服电话：0772-8251627&nbsp;&nbsp;0772-8251193</P><P>企业QQ:4006068809   </P><P>官方网站：www.xiake.org</P>"));
                    }
                    else
                    {
                        str = str.Replace("【模型值2(UBB模式)】", DoHtmlToUBB(article.DataObject[1]));
                    }
                }
                catch
                {
                    str = str.Replace("【模型值2(UBB模式)】", "");
                }
            }

            if (str.Contains("【模型值3(UBB模式)】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值3(UBB模式)】", DoHtmlToUBB("<P>本文是站群引擎V3版本的测试文章,模型值3</P>"));
                    }
                    else
                    {
                        str = str.Replace("【模型值3(UBB模式)】", DoHtmlToUBB(article.DataObject[2]));
                    }
                }
                catch
                {
                    str = str.Replace("【模型值3(UBB模式)】", "");
                }
            }

            if (str.Contains("【模型值4(UBB模式)】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值4(UBB模式)】", DoHtmlToUBB("<P>本文是站群引擎V3版本的测试文章,模型值4</P>"));
                    }
                    else
                    {
                        str = str.Replace("【模型值4(UBB模式)】", DoHtmlToUBB(article.DataObject[3]));
                    }
                }
                catch
                {
                    str = str.Replace("【模型值4(UBB模式)】", "");
                }
            }

            if (str.Contains("【模型值5(UBB模式)】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值5(UBB模式)】", DoHtmlToUBB("<P>本文是站群引擎V3版本的测试文章,模型值5</P>"));
                    }
                    else
                    {
                        str = str.Replace("【模型值5(UBB模式)】", DoHtmlToUBB(article.DataObject[4]));
                    }
                }
                catch
                {
                    str = str.Replace("【模型值5(UBB模式)】", "");
                }
            }


            if (str.Contains("【模型值1(截短模式)】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值1(截短模式)】", "站群引擎V3版测试发布文章");
                    }
                    else
                    {
                        str = str.Replace("【模型值1(截短模式)】", Substring(V3.Common.Format.ToTxt(article.DataObject[0]), Model.V3Infos.MainDb.Jieduanbiaoti));
                    }
                }
                catch
                {
                    str = str.Replace("【模型值1(截短模式)】", "");
                }
            }

            if (str.Contains("【模型值2(截短模式)】"))
            {

                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值2(截短模式)】", V3.Common.Format.ToTxt("<P>您好，您现在看到的这篇文章是站群引擎V3版本的测试文章！</P><P>看到这篇文章意味着您的发布模块配置已经完成，您可以正常使用V3了！</P><P>by xiaoxia </P><P>(这是模型2)</P><P>如有不明之处可联系客服</P><P>客服电话：0772-8251627&nbsp;&nbsp;0772-8251193</P><P>企业QQ:4006068809   </P><P>官方网站：www.xiake.org</P>"));
                    }
                    else
                    {
                        str = str.Replace("【模型值2(截短模式)】", Substring(V3.Common.Format.ToTxt(myr.Replace(article.DataObject[1], "")), Model.V3Infos.MainDb.Jieduanzhengwen));
                    }
                }
                catch
                {
                    str = str.Replace("【模型值2(截短模式)】", "");
                }
            }


            if (str.Contains("【模型值1(截短UBB模式)】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值1(截短UBB模式)】", "站群引擎V3版测试发布文章");
                    }
                    else
                    {
                        str = str.Replace("【模型值1(截短UBB模式)】", DoHtmlToUBB(Substring(V3.Common.Format.ToTxt(article.DataObject[0]), Model.V3Infos.MainDb.Jieduanbiaoti)));
                    }
                }
                catch
                {
                    str = str.Replace("【模型值1(截短UBB模式)】", "");
                }
            }

            if (str.Contains("【模型值2(截短UBB模式)】"))
            {
                try
                {
                    if (istest || pointid == 0 || pointid == 999999)
                    {
                        str = str.Replace("【模型值2(截短UBB模式)】", DoHtmlToUBB(V3.Common.Format.ToTxt("<P>您好，您现在看到的这篇文章是站群引擎V3版本的测试文章！</P><P>看到这篇文章意味着您的发布模块配置已经完成，您可以正常使用V3了！</P><P>by xiaoxia </P><P>(这是模型2)</P><P>如有不明之处可联系客服</P><P>客服电话：0772-8251627&nbsp;&nbsp;0772-8251193</P><P>企业QQ:4006068809   </P><P>官方网站：www.xiake.org</P>")));
                    }
                    else
                    {
                        str = str.Replace("【模型值2(截短UBB模式)】", DoHtmlToUBB(Substring(V3.Common.Format.ToTxt(myr.Replace(article.DataObject[1], "")), Model.V3Infos.MainDb.Jieduanzhengwen)));
                    }
                }
                catch
                {
                    str = str.Replace("【模型值2(截短UBB模式)】", "");
                }
            }
            #endregion
            if (isfirst)
                return ReplaceTag(str, istest, oldhtml, false);
            else
                return str;
        }
        #region 临时变量
        public bool LoginOk = false;
        public string sendvcode = "";
        public string loginvcode = "";
        #endregion

        #region 主流程
        public string RunAction(Model.发布相关模型.GetPostAction action, bool istest, string oldhtml)
        {
            Model.发布相关模型.GetPostAction newAction = xEngine.Common.XSerializable.CloneObject<Model.发布相关模型.GetPostAction>(action);
           
            bool isutf8 = false;
            if (pointid == 0 || Model.V3Infos.SendPointDb[pointid].IsUseModelBianma || istest)
                isutf8 = action.IsUtf8;
            else
            {
                isutf8 = Model.V3Infos.SendPointDb[pointid].IsPostUtf8;
            }

            string result = string.Empty;
            string vcode = "";
            if (newAction.IsPost)
            {
                string postdata = string.Empty;
                Dictionary<string, string> newPostData = new Dictionary<string, string>();
                GetVcode(ref newAction, istest, oldhtml, ref vcode);
                string PostString = "";
                foreach (KeyValuePair<string, string> k in newAction.PostData)
                {
                    string pk=ReplaceTag(k.Key, istest, oldhtml, true);
                    string pv=ReplaceTag(k.Value, istest, oldhtml, true);
                    newPostData.Add(pk,pv);
                    PostString += pk + "=" + pv+"&";
                }
                //新发布接口//////////////////////////

                XRequest request=new XRequest();
                request.Method = newAction.PostModel ? 2 : 1;
                request.PostData.AddRange(newPostData);

                xEngine.Execute.Http execute = new xEngine.Execute.Http();
               
                execute.CookieManager = account.MyCookie;
                execute.IsRedirect = action.IsGetRedirect;
                request.Url = ReplaceTag(newAction.ActionUrl, istest, oldhtml, true);
                request.Referer = ReplaceTag(newAction.RefrereUrl, istest, oldhtml, true);
                request.Encoding = isutf8 ? Encoding.UTF8.WebName : Encoding.Default.WebName;
                request.UserAgent = action.UserAgent;
                xEngine.Model.Execute.Http.XResponse response = execute.RunRequest(request);
                Program.NetWorkUplaod += Encoding.UTF8.GetBytes(PostString).Length;
                if (response.BodyData != null)
                {
                    Program.NetWorkDownload += (response.BodyData.Length);
                }
               // account.MyCookie = execute.CookieManager;
               result=response.Header+ response.BodyString;

                 }
            else
            {
                //新接口///
                xEngine.Execute.Http execute = new xEngine.Execute.Http();
                execute.LoadScript(Properties.Resources.get,false);
                execute.CookieManager = account.MyCookie;
                execute.IsRedirect = action.IsGetRedirect;
                execute.Scripts[0].Url = ReplaceTag(newAction.ActionUrl, istest, oldhtml, true);
                execute.Scripts[0].Referer = ReplaceTag(newAction.RefrereUrl, istest, oldhtml, true);
                execute.Scripts[0].Encoding = isutf8 ? Encoding.UTF8.WebName : Encoding.Default.WebName;
                execute.Scripts[0].UserAgent = newAction.UserAgent;
              //  execute.IsAutoEncoding = true;
                xEngine.Model.Execute.Http.XResponse response = execute.RunRequest(execute.Scripts[0]);
                account.MyCookie = execute.CookieManager;
                result = response.Header + response.BodyString;
                if (response.BodyData != null)
                {
                    Program.NetWorkDownload += (response.BodyData.Length);
                }
                ///////////////////
                //result = XiakeApp.Class.httpHelper.HttpGET(ReplaceTag(action.ActionUrl, istest, oldhtml, true), ReplaceTag(action.RefrereUrl, istest, "", true), isutf8 ? 2 : 1, ref account.cookie, V3Model.V3Infos.dbsV3.GetTimeOut, action.UserAgent, action.IsGetRedirect);
            }
            loginvcode = "";
            sendvcode = "";
            return result;
        }
        /// <summary>
        /// 上次发布状态
        /// </summary>
        string laststatus = "";
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="action"></param>
        /// <param name="istest"></param>
        /// <param name="oldhtml"></param>
        /// <param name="vcode"></param>
        /// <param name="postdata"></param>
        private void GetVcode(ref Model.发布相关模型.GetPostAction action, bool istest, string oldhtml, ref string vcode)
        {
            Dictionary<string,string>newPostData=new Dictionary<string, string>();
            foreach (var k in action.PostData)
            {
                newPostData.Add(k.Key,k.Value);
                if (k.Value.Contains("【登录验证码】"))
                {
                    if (model.Stp1_POST_VcodeModel)
                    {

                        V3Form.发布模块.VcodeInput frm = new V3Form.发布模块.VcodeInput();

                        //新方法获取验证码
                        xEngine.Execute.Http execute = new xEngine.Execute.Http();
                        execute.CookieManager = account.MyCookie;
                        xEngine.Model.Execute.Http.XRequest request = new xEngine.Model.Execute.Http.XRequest();
                        request.Url = ReplaceTag(model.Stp1_POST_VcheckcodeUrl, istest, oldhtml, true);
                        request.Referer = ReplaceTag(action.RefrereUrl, istest, oldhtml, true);
                        xEngine.Model.Execute.Http.XResponse response = execute.RunRequest(request);
                        if (response.BodyData != null)
                        {
                            Program.NetWorkDownload += (response.BodyData.Length);
                        }
                        System.Drawing.Image img = null;
                        try
                        {
                            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(response.BodyData))
                            {
                                img = System.Drawing.Image.FromStream(ms);
                                ms.Flush();
                            }
                        }
                        catch { }
                        //////////////


                        if (img != null)
                        {
                            frm.vcodepic.Image = img;
                        }
                        frmMain.MyFrmMain.Invoke((EventHandler)(delegate
                        {
                            frm.ShowDialog();
                        }));
                        if (frm.issave)
                        {
                            vcode = frm.vcodetxt.Text;
                            newPostData[k.Key]=newPostData[k.Key].Replace("【登录验证码】", vcode);
                            loginvcode = vcode;
                        }

                    }
                    else if (account.Status.Contains("验证码"))
                    {

                        V3Form.发布模块.VcodeInput frm = new V3Form.发布模块.VcodeInput();
                        ///新方法获取验证码
                        xEngine.Execute.Http execute = new xEngine.Execute.Http();
                        execute.CookieManager = account.MyCookie;
                        xEngine.Model.Execute.Http.XRequest request = new xEngine.Model.Execute.Http.XRequest();
                        request.Url = ReplaceTag(model.Stp1_POST_VcheckcodeUrl, istest, oldhtml, true);
                        request.Referer = ReplaceTag(model.Stp1_POST_VcheckcodeUrl, istest, oldhtml, true);
                        xEngine.Model.Execute.Http.XResponse response = execute.RunRequest(request);
                        if (response.BodyData != null)
                        {
                            Program.NetWorkDownload += (response.BodyData.Length);
                        }
                        System.Drawing.Image img = null;
                        try
                        {
                            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(response.BodyData))
                            {
                                img = System.Drawing.Image.FromStream(ms);
                                ms.Flush();
                            }
                        }
                        catch { }
                        //////////////
                        if (img != null)
                        {
                            frm.vcodepic.Image = img;
                        }
                        frm.ShowDialog();
                        if (frm.issave)
                        {
                            vcode = frm.vcodetxt.Text;
                            loginvcode = vcode;
                            newPostData[k.Key] = newPostData[k.Key].Replace("【登录验证码】", vcode);
                        }

                    }
                    newPostData[k.Key] = newPostData[k.Key].Replace("【登录验证码】", "");
                }
                if (k.Value.Contains("【发布验证码】"))
                {
                    if (model.Stp3_POST_VcodeModel)
                    {

                        V3Form.发布模块.VcodeInput frm = new V3Form.发布模块.VcodeInput();
                        ///新方法获取验证码
                        xEngine.Execute.Http execute = new xEngine.Execute.Http();
                        execute.CookieManager = account.MyCookie;
                        xEngine.Model.Execute.Http.XRequest request = new xEngine.Model.Execute.Http.XRequest();
                        request.Url = ReplaceTag(model.Stp3_POST_VcodeUrl, istest, oldhtml, true);
                        request.Referer = ReplaceTag(action.RefrereUrl, istest, oldhtml, true);
                        xEngine.Model.Execute.Http.XResponse response = execute.RunRequest(request);
                        if (response.BodyData != null)
                        {
                            Program.NetWorkDownload += (response.BodyData.Length);
                        }
                        System.Drawing.Image img = null;
                        try
                        {
                            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(response.BodyData))
                            {
                                img = System.Drawing.Image.FromStream(ms);
                                ms.Flush();
                            }
                        }
                        catch { }
                        //////////////
                        if (img != null)
                        {
                            frm.vcodepic.Image = img;
                        }
                        frmMain.MyFrmMain.Invoke((EventHandler) (delegate
                        {
                            frm.ShowDialog();
                        }));

                        if (frm.issave)
                        {
                            vcode = frm.vcodetxt.Text;
                            sendvcode = vcode;
                            newPostData[k.Key] = newPostData[k.Key].Replace("【发布验证码】", vcode);
                        }

                    }
                    else if (laststatus.Contains("验证码"))
                    {

                        V3Form.发布模块.VcodeInput frm = new V3Form.发布模块.VcodeInput();
                        ///新方法获取验证码
                        xEngine.Execute.Http execute = new xEngine.Execute.Http();
                        execute.CookieManager = account.MyCookie;
                        xEngine.Model.Execute.Http.XRequest request = new xEngine.Model.Execute.Http.XRequest();
                        request.Url = ReplaceTag(model.Stp3_POST_VcodeUrl, istest, oldhtml, true);
                        request.Referer = ReplaceTag(action.RefrereUrl, istest, oldhtml, true);
                        xEngine.Model.Execute.Http.XResponse response = execute.RunRequest(request);
                        if (response.BodyData != null)
                        {
                            Program.NetWorkDownload += (response.BodyData.Length);
                        }
                        System.Drawing.Image img = null;
                        try
                        {
                            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(response.BodyData))
                            {
                                img = System.Drawing.Image.FromStream(ms);
                                ms.Flush();
                            }
                        }
                        catch { }
                        //////////////
                        if (img != null)
                        {
                            frm.vcodepic.Image = img;
                        }
                        frmMain.MyFrmMain.Invoke((EventHandler) (delegate
                        {
                            frm.ShowDialog();
                        }));
                        if (frm.issave)
                        {
                            vcode = frm.vcodetxt.Text;
                            sendvcode = vcode;
                            newPostData[k.Key] = newPostData[k.Key].Replace("【发布验证码】", vcode);
                        }

                    }
                    newPostData[k.Key] = newPostData[k.Key].Replace("【发布验证码】", "");
                }
            }
            action.PostData =xEngine.Common.XSerializable.CloneObject<Dictionary<string,string>>(newPostData) ;

        }

        #region 登录
        /// <summary>
        /// 执行登录动作
        /// </summary>
        /// <param name="stp">-1为全局登录，其他则到具体步骤</param>
        /// <param name="istest">是否是测试登录</param>
        /// <returns></returns>
        public string LoginStart(bool istest)
        {
            string oldhtml = "";
            if (istest || pointid == 0)
            {
                if (!model.Ismajiamodel)
                {
                    V3Form.发布模块.UserInput frm = new V3Form.发布模块.UserInput();
                    model.POST_TestAccount.MyCookie = new xEngine.Model.Execute.Http.XCookieManager();
                    frm.username.Text = model.POST_TestAccount.username;
                    frm.password.Text = model.POST_TestAccount.password;
                    frm.value1.Text = model.POST_TestAccount.loginvalue1;
                    frm.value2.Text = model.POST_TestAccount.loginvalue2;
                  frmMain.MyFrmMain.Invoke((EventHandler) (delegate
                    {
                        frm.ShowDialog();
                    }));
                    if (frm.issave)
                    {
                        model.POST_TestAccount.username = frm.username.Text;
                        model.POST_TestAccount.password = frm.password.Text;
                        model.POST_TestAccount.loginvalue1 = frm.value1.Text;
                        model.POST_TestAccount.loginvalue2 = frm.value2.Text;
                        account = model.POST_TestAccount;
                    }
                }
                else
                {
                   
                   account.MyCookie = new xEngine.Model.Execute.Http.XCookieManager();
                    xEngine.Execute.Http execute = new xEngine.Execute.Http();
                    execute.CookieManager = model.POST_TestAccount.MyCookie;
                    execute.CookieAddStr(istest?model.Majiastr:account.Majiastr);
                }
            }
            if (model.Ismajiamodel)
                V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：正在使用账号【马甲模式】“" + account.username + "”进行登录...[/c]");
            else
                V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：正在使用账号【脚本模式】“" + account.username + "”进行登录...[/c]");
            if (model.Stp1_POST_LoginAction!=null)
            {
                for (int i = 0; i < model.Stp1_POST_LoginAction.Length; i++)
                {
                    string result = RunAction(model.Stp1_POST_LoginAction[i], istest, oldhtml);
                    oldhtml += result;
                }
            }
            
            return oldhtml.Replace("\0","");
        }
        /// <summary>
        /// 登录动作
        /// </summary>
        /// <param name="istest">是否是测试登录</param>
        /// <param name="tempstr">返回结果</param>
        /// <returns></returns>
        public string Login(bool istest, ref string tempstr)
        {
            string returnstr = "";
            if (istest || pointid == 0)
            {
                this.account = model.POST_TestAccount;
                account.Status = "未知";
                returnstr = LoginRun1(istest, ref tempstr);
            }
            else
            {
                if (!Model.V3Infos.SendPointDb.ContainsKey(pointid)) { return "无法再发布点中找到当前站点！"; }
                if (Model.V3Infos.SendPointDb[pointid].AccountModel)
                {
                    for (int ii = 0; ii < Model.V3Infos.SendPointDb[pointid].AccountData.AccountTrue.Count; ii++)
                    {
                        if (Model.V3Infos.SendPointDb[pointid].AccountData.AccountTrue.Count == 0)
                        {
                            LoginOk = false;
                            V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：登录失败，该发布点所有的可用账号均已失效，请检查发布点账号设置！[/c]");
                            return "抱歉，该发布点所有的可用账号均已失效，请检查发布点账号设置！";
                        }
                        else
                        {
                            account = Model.V3Infos.SendPointDb[pointid].AccountData.GetNewAccount();
                            if (account != null)
                            {
                                returnstr = LoginRun1(istest, ref tempstr);
                            }
                            else {

                                return "抱歉，该发布点所有的可用账号均已失效，请检查发布点账号设置！";
                             
                            }
                           
                        }
                        if (LoginOk)
                            break;
                    }
                }
                else
                {
                    account = Model.V3Infos.SendPointDb[pointid].Oneaccount;
                    returnstr = LoginRun1(istest, ref tempstr);
                }
                if (Model.V3Infos.TaskDb.ContainsKey(taskid))
                {
                    V3.Bll.PointBll.Update(Model.V3Infos.SendPointDb[Model.V3Infos.TaskDb[taskid].PointId]);
                    V3.Bll.TaskBll.SaveTask(taskid);
                }
            }
            return returnstr;
        }
        private string LoginRun1(bool istest, ref string tempstr)
        {
           
            string result = "";
           
            if (!model.Stp1_POST_NeedLogin)
            {
                V3.Common.Log.LogNewline("[c12]任务【" + taskid + "】：恭喜！本发布模块设置为不需要登录模式，直接忽略登录过程！[/c]");
                LoginOk = true;
                account.CountFalse = 0;
                return "LoginOK";
            }
            else if (!model.Stp1_POST_NeedLoginMore && account.Status == "成功登录")
            {
                V3.Common.Log.LogNewline(
                    "[c12]任务【" + taskid + "】：恭喜！，账号“" + account.username +
                    "”已经处于成功登录状态，直接忽略登录过程！[/c]");
                LoginOk = true;
                account.CountFalse = 0;
                return "LoginOK";
            }
            else
            {
                if (model.Ismajiamodel)
                {
                    account.MyCookie = new xEngine.Model.Execute.Http.XCookieManager();
                    xEngine.Execute.Http execute = new xEngine.Execute.Http();
                    if (model.Majiastr.Length > 0)
                    {
                        V3.Common.Log.LogNewline( "[c11]任务【" + taskid + "】：使用模块自带马甲登录...[/c]");
                        execute.CookieManager =account.MyCookie;
                        execute.CookieAddStr(model.Majiastr);
                    }
                    else {
                        V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：模块马甲无数据，使用站点马甲登录...[/c]");
                        execute.CookieManager = account.MyCookie;
                        execute.CookieAddStr(account.Majiastr);
                    }
 
                }
                else
                {
                    account.MyCookie = new xEngine.Model.Execute.Http.XCookieManager();
                }
                

                int trynumber = (Model.V3Infos.SendPointDb.ContainsKey(pointid)
                    ? Model.V3Infos.SendPointDb[pointid].AccountData.tryLoginFalse
                    : 7);



                for (int i = 0; i < trynumber; i++)
                {
                    result = LoginRun2(istest, ref tempstr);
                   
                    if (result == "LoginOK")
                    {
                        V3.Common.Log.LogNewline(
                            "[c12]任务【" + taskid + "】：恭喜！，账号“" + account.username +
                            "”已经成功登录啦！[/c]");
                        account.Status = "成功登录";
                        account.CountFalse = 0;
                        LoginOk = true;
                        break;
                    }
                    else if (result.Contains("超时") || result.Contains("验证码"))
                    {
                        if (result.Contains("超时"))
                        {
                            V3.Common.Log.LogNewline(
                                "[c14]任务【" + taskid + "】：登录失败，原因“登录超时/”，系统将会自动重试登录" +
                                (trynumber - i) + "次！[/c]");
                            account.Status = "登录超时";
                            LoginOk = false;
                            account.CountFalse++;
                        }
                        else
                        {
                            V3.Common.Log.LogNewline(
                                "[c14]任务【" + taskid + "】：登录失败，原因“验证码输入错误或需要验证码/”，系统将会自动重试登录" +
                                (trynumber - i) + "次！[/c]");
                            account.Status = "验证码问题";
                            LoginOk = false;
                            account.CountFalse++;
                        }
                    }
                    else if (result.Contains("无法解析此远程名称"))
                    {
                        V3.Common.Log.LogNewline(
                            "[c14]任务【" + taskid + "】：登录失败，原因“参数填写的地址不正确，无法解析这个域名/”，系统将会自动重试登录" +
                            (trynumber - i) + "次！[/c]");
                        account.Status = "填写的地址不正确，无法解析这个域名";
                        LoginOk = false;
                        account.CountFalse++;
                    }
                    else if (result.Contains("(403)"))
                    {
                        V3.Common.Log.LogNewline(
                            "[c14]任务【" + taskid +
                            "】：登录失败，原因“参数填写的地址不存在或不可访问，返回403了！/”，系统将会自动重试登录" + (trynumber - i) +
                            "次！[/c]");
                        account.Status = "参数填写的地址不存在，返回404了！";
                        LoginOk = false;
                        account.CountFalse++;
                    }
                    else if (result.Contains("(404)"))
                    {
                        V3.Common.Log.LogNewline(
                            "[c14]任务【" + taskid + "】：登录失败，原因“参数填写的地址不存在，返回404了！/”，系统将会自动重试登录" +
                            (trynumber - i) + "次！[/c]");
                        account.Status = "参数填写的地址不存在，返回404了！";
                        LoginOk = false;
                        account.CountFalse++;
                    }
                    else if (result.Contains("(500)"))
                    {
                        V3.Common.Log.LogNewline(
                            "[c14]任务【" + taskid +
                            "】：登录失败，原因“您的网站发生内部错误，请先检查你的网站，代码（500）/”，系统将会自动重试登录" +
                            (trynumber - i) + "次！[/c]");
                        account.Status = "您的网站发生内部错误，请先检查你的网站，代码（500）";
                        LoginOk = false;
                        account.CountFalse++;
                    }
                    else if (result.Contains("无效的 URI"))
                    {
                        V3.Common.Log.LogNewline(
                            "[c14]任务【" + taskid + "】：登录失败，原因“填写的地址不正确，是无效的URL/”，系统将会自动重试登录" +
                            (trynumber - i) + "次！[/c]");
                        account.Status = "填写的地址不正确，是无效的UR";
                        LoginOk = false;
                        account.CountFalse++;
                    }
                    else
                    {
                        V3.Common.Log.LogNewline(
                            "[c14]任务【" + taskid + "】：登录失败，原因“" + result + "/”，停止登录过程！[/c]");
                        account.Status = result;
                        LoginOk = false;
                        account.CountFalse++;
                        return result;
                    }
                }
                if (!LoginOk)
                {
                    if (taskid == 0)
                        V3.Common.Log.LogNewline(
                            "[c14]任务【" + taskid + "】：由于账号“" + account.username +
                            "”连续7次登录失败，停止登录过程！[/c]");
                    else
                        V3.Common.Log.LogNewline(
                            "[c14]任务【" + taskid + "】：由于账号“" + account.username + "”连续" +
                            Model.V3Infos.SendPointDb[pointid].AccountData.tryLoginFalse + "次登录失败，停止登录过程！[/c]");
                }
                account.LastActiveTime = DateTime.Now;
                return result;
            }
        }
        private string LoginRun2(bool istest, ref string tempstr)
        {
            tempstr = LoginStart(istest);
            if (model.Stp1_POST_Truetag!=null)
            {
                for (int i = 0; i < model.Stp1_POST_Truetag.Length; i++)
                {
                    if (tempstr.Contains(model.Stp1_POST_Truetag[i]))
                    {
                        return "LoginOK";
                    }
                }
            }
            if (model.Stp1_POST_Falsetag!=null)
            {
                for (int i = 0; i < model.Stp1_POST_Falsetag.Length; i++)
                {
                    string[] temp = model.Stp1_POST_Falsetag[i].Split('=');
                    if (temp.Length == 2)
                    {
                        if (tempstr.Contains(temp[0]))
                        {
                            return temp[1];
                        }

                    }
                    else
                    {
                        if (tempstr.Contains(model.Stp1_POST_Falsetag[i]))
                        {
                            return model.Stp1_POST_Falsetag[i];
                        }
                    }
                }
            }
           
            return "没有找到登录标记，无法判断是否成功登录！请重新修改登录参数";
        }
        #endregion

        #region 分类
        public string[] ClassGet(string htmlstr)
        {
            return getFinalResult(htmlstr, model.Stp2_POST_GetClassRules);
        }
        string[] getFinalResult(string html, string regex)
        {
            try
            {
                MatchCollection mccc = Regex.Matches(html, regex, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                string[] result = new string[mccc.Count];
                for (int i = 0; i < mccc.Count; i++)
                {
                    result[i] = mccc[i].Groups["typeid"].Value + "`" + mccc[i].Groups["typename"].Value;
                }
                V3.Common.Log.LogNewline("[c12]任务【" + taskid + "】：获取到了" + mccc.Count + "个分类！[/c]");
                return result;
            }
            catch (Exception ex)
            {
                frmMain.MyFrmMain.Invoke((EventHandler) (delegate
                {
                    XtraMessageBox.Show("正则表达式：" + regex + "出错，错误原因：\r\n\r\n" + ex.Message, "出错啦",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                   
                })); 
                return new string[0];
            }
        }
        string AddClassName = "";
        public bool ClassAdd(string classname, bool istest)
        {
            if (!model.Stp2_POST_UsedAddClass){
                V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：该模块没有启用增加分类功能/，停止过程！[/c]");
                return false;
            }
            AddClassName = classname;
            if (!LoginOk)
            {
                string temp = "";
                Login(false, ref temp);
                if (!LoginOk)
                {
                    V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：啊哦，由于没有登录成功，所以不能添加分类/，停止过程！[/c]");
                }
            }
            string str = RunAction(model.Stp2_POST_Post, istest, "");
            if (str.Contains(model.Stp2_POST_GetAddOktag))
            {
                V3.Common.Log.LogNewline("[c12]任务【" + taskid + "】：恭喜，成功添加了分类“" + classname + "”！[/c]");
                return true;
            }
            else
            {
                V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：添加分类“" + classname + "/”失败了！[/c]");
                return false;
            }
        }
        #endregion

        #region 发布
        static TimeSpan OldTime = new TimeSpan(DateTime.Now.Ticks);
        public static void isRunThread()
        {
            if (Model.V3Infos.MainDb.MaxSendThread == 0)
                return;
            else
            {
                double sleeptime = 10000 / Model.V3Infos.MainDb.MaxSendThread;
                while (true)
                {
                    lock ("times")
                    {
                        if ((new TimeSpan(DateTime.Now.Ticks).Subtract(OldTime).TotalMilliseconds < sleeptime))
                        {
                            System.Threading.Thread.Sleep(10);
                        }
                        else
                        {
                            OldTime = new TimeSpan(DateTime.Now.Ticks);
                            break;
                        }
                    }
                }

            }
        }

        public string fenleistr = "";
        public string SendStart(bool istest)
        {
            string oldhtml = "";
            if (model.Stp3_POST_SendAction!=null)
            {
                for (int i = 0; i < model.Stp3_POST_SendAction.Length; i++)
                {
                    oldhtml += RunAction(model.Stp3_POST_SendAction[i], istest, oldhtml);
                }
            }
           
            return oldhtml;
        }
        public string Send(bool istest, ref string tempstr, ref string thisLink)
        {
            for (int i=0;i<30;i++)
            {
                if (article.DataObject.ContainsKey(i))
                {
                    if (article.DataObject[i].Length>0)
                    {
                        article.DataObject[i] = article.DataObject[i].Replace("\r\n", "");
                    }
                }
            }
            isRunThread();
            if (Model.V3Infos.SendPointDb.ContainsKey(pointid))
            {
                if (account.CountFalse >= Model.V3Infos.SendPointDb[pointid].AccountData.tryLoginCount)
                {
                    account.Status = "连续" + account.CountFalse + "次发布失败！";
                    account.CountFalse = 0;
                    LoginOk = false;
                }
            }
            if (!LoginOk && model.Stp1_POST_NeedLogin)
            {
                string temp = "";
                Login(false, ref temp);
                if (!LoginOk)
                {
                    V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：由于没有登录成功，所以不能发布，停止过程！[/c]");
                    return "没有登录成功，无法发布";
                }
            }
            else
            {
                if (istest || pointid == 0)
                {
                    this.account = model.POST_TestAccount;
                    account.Status = "未知";

                }
                else
                {
                    if (!Model.V3Infos.SendPointDb.ContainsKey(pointid)) {  }
                    if (Model.V3Infos.SendPointDb[pointid].AccountModel)
                    {
                        for (int ii = 0; ii < Model.V3Infos.SendPointDb[pointid].AccountData.AccountTrue.Count; ii++)
                        {
                            if (Model.V3Infos.SendPointDb[pointid].AccountData.AccountTrue.Count == 0)
                            {
                                LoginOk = false;
                                V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：无法取到账号，该发布点所有的可用账号均已失效，请检查发布点账号设置！[/c]");
                               
                            }
                            else
                            {
                                account = Model.V3Infos.SendPointDb[pointid].AccountData.GetNewAccount();


                            }

                        }
                    }
                    else
                    {
                        account = Model.V3Infos.SendPointDb[pointid].Oneaccount;

                    }
                    LoginOk = true;
                }
            }

            if (Model.V3Infos.TaskDb.ContainsKey(taskid))
                fenleistr = Model.V3Infos.TaskDb[taskid].Sendclass;
            if (model.Stp3_POST_SendAction==null||model.Stp3_POST_SendAction.Length == 0)
                return "没有任何发布动作，请设置发布参数！";
            if (istest || pointid == 0)
            {
                V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：正在提交数据，请稍候...[/c]");
            }
            tempstr = SendStart(istest);
            string oldhtml = "";
            if (model.Stp3_POST_Truetag!=null)
            {
                for (int i = 0; i < model.Stp3_POST_Truetag.Length; i++)
                {

                    if (tempstr.Contains(model.Stp3_POST_Truetag[i]))
                    {
                        if (istest || pointid == 0)
                        {
                            V3.Common.Log.LogNewline("[c12]任务【" + taskid + "】：发布成功，正在处理“生成html”“链轮”[/c]");
                        }
                        if (Model.V3Infos.TaskDb.ContainsKey(taskid) && Model.V3Infos.TaskDb[taskid].IsUseMakeHtml)
                            SendMakeHtml(istest, ref tempstr, ref oldhtml);
                        else if (Model.V3Infos.TaskDb.ContainsKey(taskid) && !Model.V3Infos.TaskDb[taskid].IsUseMakeHtml) { }
                        else
                            SendMakeHtml(istest, ref tempstr, ref oldhtml);
                        if (Model.V3Infos.TaskDb.ContainsKey(taskid) && Model.V3Infos.TaskDb[taskid].IsUseLinkDb)
                        {
                            SendGetLink(istest, tempstr, ref thisLink, ref oldhtml);
                            Model.Link links = new Model.Link();
                            links.Title = article.DataObject[0];
                            links.Keyword = article.DataObject[29];
                            links.Url = thisLink;
                            Model.V3Infos.LinkDb[Model.V3Infos.TaskDb[taskid].LinkDbId.ToString()].Links.Add(links);
                            Model.V3Infos.LinkDb[Model.V3Infos.TaskDb[taskid].LinkDbId.ToString()].DataCount++;
                        }
                        else if (Model.V3Infos.TaskDb.ContainsKey(taskid) && !Model.V3Infos.TaskDb[taskid].IsUseLinkDb) { }
                        else
                            SendGetLink(istest, tempstr, ref thisLink, ref oldhtml);
                        account.Status = "成功登录";
                        account.LastActiveTime = DateTime.Now;
                        laststatus = "SendOK";
                        return "SendOK";
                    }
                }
            }
           
            string resultstr = "未找到返回标记，无法识别是否发布成功，请修改发布模块的返回标记！";
            if (model.Stp3_POST_Falsetag!=null)
            {
                for (int i = 0; i < model.Stp3_POST_Falsetag.Length; i++)
                {
                    string[] temp = model.Stp3_POST_Falsetag[i].Split('=');
                    if (temp.Length == 2)
                    {
                        if (tempstr.Contains(temp[0]))
                            resultstr = temp[1];
                    }
                    else
                    {
                        if (tempstr.Contains(model.Stp3_POST_Falsetag[i]))
                            resultstr = model.Stp3_POST_Falsetag[i];
                    }
                }
               
            }
           
            laststatus = resultstr;
            if (resultstr == "未找到返回标记，无法识别是否发布成功，请修改发布模块的返回标记！")
                Log.LogSendError(tempstr);
            if (resultstr.Contains("超时"))
            {
                resultstr = "连接目标网站超时！";
            }
            else if (tempstr.Contains("无法解析此远程名称"))
            {
                resultstr = "参数填写的地址不正确，无法解析这个域名";
            }
            else if (tempstr.Contains("(403)"))
            {
                resultstr = "参数填写的地址不存在或不可访问，返回403了！";
            }
            else if (tempstr.Contains("(404)"))
            {
                resultstr = "参数填写的地址不存在，返回404了！";
            }
            else if (tempstr.Contains("(500)"))
            {
                resultstr = "您的网站发生内部错误，请先检查你的网站，代码（500）";
            }
            else if (tempstr.Contains("无效的 URI"))
            {
                resultstr = "填写的地址不正确，是无效的URL";
            }
            account.CountFalse++;
            return resultstr;
        }
        /// <summary>
        /// 提取链轮连接
        /// </summary>
        /// <param name="istest"></param>
        /// <param name="tempstr"></param>
        /// <param name="thisLink"></param>
        /// <param name="oldhtml"></param>
        private void SendGetLink(bool istest, string tempstr, ref string thisLink, ref string oldhtml)
        {
            if (model.Stp3_POST_SupportLinkDb)
            {
                bool isutf8 = false;
                if (pointid == 0 || !Model.V3Infos.SendPointDb[pointid].IsUseModelBianma || istest)
                    isutf8 = model.Stp3_POST_SendAction[0].IsUtf8;
                else
                {
                    isutf8 = Model.V3Infos.SendPointDb[pointid].IsPostUtf8;
                }
                if (model.Stp3_POST_LinkGetModel)
                {
                    string thisurl = ReplaceTag(model.Stp3_POST_LinkGetUrl, istest, tempstr, true);
                    if (istest || pointid == 0)
                    {
                        V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：正在提取链轮地址，链轮提取页面" + thisurl+"[/c]");
                    }

                    xEngine.Execute.Http execute = new xEngine.Execute.Http();
                    execute.CookieManager = account.MyCookie;
                    xEngine.Model.Execute.Http.XRequest request = new xEngine.Model.Execute.Http.XRequest();
                    request.Url =thisurl;
                    request.Referer = thisurl;
                    xEngine.Model.Execute.Http.XResponse response = execute.RunRequest(request);
                    oldhtml = response.BodyString;
                        
                        
                       
                    GetBll bll = new GetBll(oldhtml, model.Stp3_POST_GetLinkrules);
                    ArrayList ary = bll.getAllRules(taskid);
                    if (ary.Count == 0)
                        thisLink = "";
                    else
                        thisLink = ReplaceTag(ary[0].ToString(), istest, oldhtml, true);
                }
                else
                {
                    if (istest || pointid == 0)
                    {
                        V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：正在提取链轮地址...[/c]");
                    }
                    GetBll bll = new GetBll(tempstr, model.Stp3_POST_GetLinkrules);
                    ArrayList ary = bll.getAllRules(taskid);
                    if (ary.Count == 0)
                        thisLink = "";
                    else
                        thisLink = ReplaceTag(ary[0].ToString(), istest, oldhtml, true);
                }
                if (istest || pointid == 0)
                {
                    string retrunmsg = "本发布规则包含提取链轮属性，本次发布提取到的链轮结果为：\r\n\r\n" + thisLink;
                    Program.f_frmMain.Invoke((EventHandler)(delegate
                    {
                        XtraMessageBox.Show( retrunmsg, "提取链轮结果（本窗口仅测试时弹出)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }));
                }
            }
        }
        /// <summary>
        /// 生成html
        /// </summary>
        /// <param name="istest"></param>
        /// <param name="tempstr"></param>
        /// <param name="oldhtml"></param>
        private void SendMakeHtml(bool istest, ref string tempstr, ref string oldhtml)
        {
            account.CountTrue++;
            if (model.Stp3_POST_NeedMakeHtml && (istest || account.CountTrue >= Model.V3Infos.MainDb.MyModels[Model.V3Infos.SendPointDb[pointid].PostModel].Stp3_POST_makeHtmlCount))
            {
                bool isutf8 = false;
                if (pointid == 0 || !Model.V3Infos.SendPointDb[pointid].IsUseModelBianma || istest)
                    isutf8 = model.Stp3_POST_SendAction[0].IsUtf8;
                else
                {
                    isutf8 = Model.V3Infos.SendPointDb[pointid].IsPostUtf8;
                }

                oldhtml = tempstr;
                System.Diagnostics.Stopwatch sws = new System.Diagnostics.Stopwatch();
                sws.Start();
                if (model.Stp3_POST_MakeHtmlUrls != null)
                {
                    for (int ii = 0; ii < model.Stp3_POST_MakeHtmlUrls.Length; ii++)
                    {
                        string thisurl = ReplaceTag(model.Stp3_POST_MakeHtmlUrls[ii], istest, oldhtml, true);
                        Log.LogNewline("[c12]连续" + account.CountTrue + "次发布成功，正在执行生成Html脚本，当前访问：" + Substring(thisurl, 20) + "...[/c]");
                        if (istest || pointid == 0)
                        {
                            V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：正在访问第" + (ii) + "个生成html页面" + thisurl+"[/c]");
                        }

                        xEngine.Execute.Http execute = new xEngine.Execute.Http();
                        execute.CookieManager = account.MyCookie;
                        xEngine.Model.Execute.Http.XRequest request = new xEngine.Model.Execute.Http.XRequest();
                        request.Url = thisurl;
                        request.Referer = thisurl;
                        xEngine.Model.Execute.Http.XResponse response = execute.RunRequest(request);
                        if (response.BodyData != null)
                        {
                            Program.NetWorkDownload += (response.BodyData.Length);
                        }
                        oldhtml = response.BodyString;

                        tempstr += oldhtml;
                        if (istest || pointid == 0)
                        {
                            string str = oldhtml;
                            Program.f_frmMain.Invoke((EventHandler)(delegate
                            {
                                XtraMessageBox.Show("访问第" + (ii) + "个生成页面" + thisurl + "返回的结果如下：\r\n\r\n" + str, "生成HTML结果（本窗口仅测试时弹出)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }));
                        }
                    } 
                }
              
                sws.Stop();
                Log.LogNewline("[c12]生成html脚本执行完毕，累计耗时：" + sws.ElapsedMilliseconds + "[/c]");
                account.CountTrue = 0;
            }
        }
        #endregion

        #endregion

        #region 辅助算法
        // SHA1加密
        public string GetSHA1(string str)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "SHA1").ToLower();
        }
        public string servertime = "";
        public string nonce = "";
        //获取新浪的及密码
        public string getSinaPassword(string youpassword)
        {
            return GetSHA1(GetSHA1(GetSHA1(youpassword)) + servertime + nonce);
        }
        // 把HTML内容转为UBB代码
        private string DoHtmlToUBB(string _Html)
        {

            _Html = Regex.Replace(_Html, "<br>|<br />", "\r\n", RegexOptions.IgnoreCase);
            _Html = Regex.Replace(_Html, "<p>|<p.+?>", "\r\n", RegexOptions.IgnoreCase);
            _Html = Regex.Replace(_Html, "\\son[\\w]{3,16}\\s?=\\s*([\'\"]).+?\\1", "", RegexOptions.IgnoreCase);
            _Html = Regex.Replace(_Html, "<hr[^>]*>", "[hr]", RegexOptions.IgnoreCase);

            _Html = Regex.Replace(_Html, "<(\\/)?blockquote([^>]*)>", "[$1blockquote]", RegexOptions.IgnoreCase);
            _Html = Regex.Replace(_Html, "<img[^>]*smile=\"(\\d+)\"[^>]*>", "'[s:$1]", RegexOptions.IgnoreCase);
            _Html = Regex.Replace(_Html, "<img[^>]*src=[\'\"\\s]*([^\\s\'\"]+)[^>]*>", "", RegexOptions.IgnoreCase);
            _Html = Regex.Replace(_Html, "<a[^>]*href=[\'\"\\s]*([^\\s\'\"]*)[^>]*>(.+?)<\\/a>", "[url=$1]$2[/url]", RegexOptions.IgnoreCase);

            _Html = Regex.Replace(_Html, "<[^>]*?>", "", RegexOptions.IgnoreCase);
            _Html = Regex.Replace(_Html, "&amp;", "&", RegexOptions.IgnoreCase);
            _Html = Regex.Replace(_Html, "&nbsp;", " ", RegexOptions.IgnoreCase);
            _Html = Regex.Replace(_Html, "&lt;", "<", RegexOptions.IgnoreCase);
            _Html = Regex.Replace(_Html, "&gt;", ">", RegexOptions.IgnoreCase);

            return _Html;
        }
        //那字符截短
        public string Substring(string html, int leght)
        {
            if (html.Length >= leght)
                return html.Substring(0, leght);
            else
                return html;
        }

        private string GetSinaUserName(string youname)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(youname);
            string str = Convert.ToBase64String(bytes);
            return str;
        }
        #endregion
    }
}
