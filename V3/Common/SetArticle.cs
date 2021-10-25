using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Threading;
using System.Collections;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Security.Cryptography;
using System.Windows.Forms;
using V3.Common;
using V3Plugin;

namespace V3.Common
{
    class SetArticle
    {
      
        public   Model.Model_Article art;
        public Model.Task Task = null;
        public int gonggongkuid = 0;
        Random ran = new Random();
        public string FormateHtmlCode(string Str) 
        {
            Dictionary<string, string> Dic = new Dictionary<string, string>() {
            {"&ensp;"," "},
            {"&emsp;"," "},
            {"&nbsp;"," "},
            {"&lt;","<"},
            {"&gt;",">"},
            {"&amp;","&"},
            {"&quot;","\""},
            {"&copy;","©"},
            {"&reg;","®"},
            };
            foreach (KeyValuePair<string, string> k in Dic)
            {
                Str = Str.Replace(k.Key, k.Value);
            }
            return Str;
        
        }
        public void chuli()
        {
            lock ("ProcessPlugin")
            {
                //调用处理插件
                foreach (var p in Task.Plugins)
                {
                    if (Program.ProcessPluginList.ContainsKey(p))
                    {

                        ProcessPlugin plugin = (ProcessPlugin)Program.ProcessPluginList[p].Clone();
                        if (Task.Plugins_before.Contains(plugin.Id))
                        {
                            Log.LogNewline("[c11]【" + Task.TaskName + "】：在所有处理功能之前调用文章处理插件[" + plugin.ProcessName + "]...[/c]");
                            try
                            {
                                plugin.KeyWords = Model.V3Infos.KeywordDb[Task.KeywordDbId.ToString()].Keywords;
                                plugin.Parameters = Task.PluginParameters[p];
                                art.DataObject = plugin.ArticleProcess(art.DataObject);
                                Task.PluginParameters[p] = plugin.Parameters;
                            }
                            catch (Exception error)
                            {
                                Log.LogNewline("[c14]【" + Task.TaskName + "】：插件[" + plugin.ProcessName + "]运行时出现重大错误：" + error.Message + ",请联系作者：【" + plugin.Author + "】解决！");
                            }
                        }

                    }
                }
            }
            htmlguolv();
            replace();
            createProcess();

            if (art.DataObject[0].Length > 0 && Task.IstitleFanYi == true)
            {
                art.DataObject[0] = fanyi(art.DataObject[0]);
               

            }

            for (int i = 0; i < 30; i++)
            {
                if (art.DataObject[i] == null) { return; }
                if(Task.FormateHtmlCode)
                {
                    art.DataObject[i] = FormateHtmlCode(art.DataObject[i]);
                }
                if (i < 25 && art.DataObject[i].ToString().Trim().Length > 0)
                {

                    if (i > 0 && i < 25 && art.DataObject[i].ToString().Trim().Length > 0)
                    {
                        art.DataObject[i] = fanyi(art.DataObject[i]);
                    }
                }

                if (art.DataObject[i] != null && i == 1)
                {

                    string title = art.DataObject[0].ToString();
                    string content = art.DataObject[1].ToString();
                    processConfusion(ref title, ref content);
                    art.DataObject[0] = title;
                    art.DataObject[i] = content;
                }
                if (i == 1)
                {   
                    art.DataObject[1]= settagsfromkeyword(art.DataObject[1]);
                    if (Task.IsMaoTiHuan == true)
                    {
                        if (Task.MaouseDefaultlinkDb)
                        {
                            art.DataObject[1] = maohunxiao(art.DataObject[1], Task.LinkDbId);
                        }
                        else
                        {
                            art.DataObject[1] = maohunxiao(art.DataObject[1], Task.MaolinkDbId);
                        }

                    }
                 
                    art.DataObject[1] = lianlun(art.DataObject[1].ToString());
                    art.DataObject[1] = zidingyilink(art.DataObject[1].ToString());
                    if (Task.Isjianfan)
                    {
                        art.DataObject[0] = jianfan(art.DataObject[0], Task.Jianfanfangshi);
                        art.DataObject[1] = jianfan(art.DataObject[1], Task.Jianfanfangshi);
                    }
                    charudaima();
                    settages(ref art);
                    setart(ref art);
                }
               
               
            }


            lock ("ProcessPlugin")
            {
                //调用处理插件
                foreach (var p in Task.Plugins)
                {
                    if (Program.ProcessPluginList.ContainsKey(p))
                    {

                        ProcessPlugin plugin = (ProcessPlugin)Program.ProcessPluginList[p].Clone();
                        if (!Task.Plugins_before.Contains(plugin.Id))
                        {
                            Log.LogNewline("[c11]【" + Task.TaskName + "】：在所有处理功能之后调用文章处理插件[" + plugin.ProcessName + "]...[/c]");
                            try
                            {
                                plugin.KeyWords = Model.V3Infos.KeywordDb[Task.KeywordDbId.ToString()].Keywords;
                                plugin.Parameters = Task.PluginParameters[p];
                                art.DataObject = plugin.ArticleProcess(art.DataObject);
                                Task.PluginParameters[p] = plugin.Parameters;
                            }
                            catch (Exception error)
                            {
                                Log.LogNewline("[c14]【" + Task.TaskName + "】：插件[" + plugin.ProcessName + "]运行时出现重大错误：" + error.Message + ",请联系作者：【" + plugin.Author + "】解决！[/c]");
                            }
                        }

                    }
                }
            }
        }

        #region  总的原创化处理
       
        public void  createProcess()
        {
            if (Task.WeiyuanchuangXiang[0])//标题原创化
            {

                if (Task.YuanChuangJianGe > 0 && Task.YuanChuangJianGe <= 5)
                {
                    if (Task.YuanChuangJianGe_Count == Task.YuanChuangJianGe)
                    {
                        Task.YuanChuangJianGe_Count = 0;
                    }
                    else
                    {
                        Task.YuanChuangJianGe_Count++;
                     
                        return;
                    }
                }
                else if (Task.YuanChuangJianGe > 5)
                {
                    if (Task.YuanChuangJianGe_Count == Task.YuanChuangJianGe)
                    {
                        if (ran.Next(0, 2) == 1)
                        {

                            Task.YuanChuangJianGe_Count = Convert.ToInt32(Task.YuanChuangJianGe * (ran.Next(10, 31) / 100));
                        }
                        else
                        {
                            Task.YuanChuangJianGe_Count = -Convert.ToInt32(Task.YuanChuangJianGe * (ran.Next(10, 31) / 100));

                        }

                    }
                    else
                    {
                        Task.YuanChuangJianGe_Count++;
                      
                        return;
                    }
                }
                art.DataObject[0] = titleyuanchuang(art.DataObject[0], Task.WeiyuanchuangTitle);
            }


            if (Task.WeiyuanchuangDu != "不进行原创处理")
            {
                for (int objc = 1; objc < 30; objc++)
                {
                    if (art.DataObject[objc] == null || art.DataObject[objc].Trim().Length == 0 || objc == 30) { return; }
                    string input = art.DataObject[objc].ToString();

                    //对内容的处理

                    if (Task.WeiyuanchuangXiang[1] == true)
                        art.DataObject[objc] = input = contentCreateProcess(input);
                    if (Task.WeiyuanchuangXiang[2] == true)
                        art.DataObject[objc] = input = paragraphCreateProcess(input);
                    if (Task.WeiyuanchuangXiang[3] == true)
                        art.DataObject[objc] = input = sentenceCreateProcess(input);
                    if (Task.WeiyuanchuangXiang[4] == true)
                        art.DataObject[objc] = input = summaryCreateProcess(input);


                }

            }
            else
            {
                return;
            }
            
        }
        //获取强度值
        private bool getIntension()
        {
            int result = 0;
            switch (Task.WeiyuanchuangDu)
            {
                    
                case "最轻度": //20%的概率
                    result = ran.Next(1, 6);
                    break;
                case "轻度": //33%的概率
                    result = ran.Next(1, 4);
                    break;
                case "中度"://50%的概率
                    result = ran.Next(1, 3);
                    break;
                case "较强度"://75%的概率
                    result = ran.Next(1, 5);
                    if ((5 - result) != 1)
                        result = 1;
                    break;
                case "最强度"://%100的概率
                    result = 1;
                    break;
            }
            if (result == 1)
                return true;
            else
                return false;
        }
        public  string GetTitleFromArticle(bool isRandom, string ArticleContent)
        {

           
            ArrayList titlelist = new ArrayList();
            string title = "无标题";
            string article = ArticleContent;


            article = article.Replace("\r", "|");
            article = article.Replace("\n", "|");
            article = article.Replace(".", "|");
            article = article.Replace("。", "|");
            string[] titles = article.Split('|');
            for (int i = 0; i < titles.Length; i++)
            {

                if (titles[i].Contains("，")) { titles[i] = titles[i].Remove(titles[i].IndexOf("，")); }
                if (titles[i].Contains(",")) { titles[i] = titles[i].Remove(titles[i].IndexOf(",")); }
                if (titles[i].Length < 25 && titles[i].Length > 5 && !IsNumberString(titles[i]) && !titles[i].Contains("\""))
                {
                    titlelist.Add(titles[i]);
                }



            }
            if (titlelist.Count <= 0) { return titles[0]; }
            if (isRandom == true)
            {
                Random r = new Random();
                title = titlelist[r.Next(0, titlelist.Count)].ToString();
            }
            else { title = titlelist[0].ToString(); }
            titlelist.Clear();
            Regex re = new Regex("<.+?>");
            title = re.Replace(title, "");
            Log.LogNewlineNosave("[c12]【" + Task.TaskName + "】：从文章中取出一个标题【" + title + "】[/c]");
            return title.Trim().Replace(";", "").Replace("\'", "").Replace("；", "").Replace("、", "").Replace("”", "").Replace("“", "");
        }
        public static bool IsNumberString(string str)
        {
            Regex r = new Regex(@"\d");
            MatchCollection co = r.Matches(str);

            if (co.Count > 0)
            {
                return true;
            }
            else { return false; }

        }
        ////标题原创
        private string titleyuanchuang(string yuanTitle,string formate)
        { 
            Regex re= new Regex("<.+?>");
            if (yuanTitle.Trim().Length == 0) {  return yuanTitle; }
            try
            {
               
                yuanTitle = re.Replace(yuanTitle, "");
            }
            catch { }


                   string resutlTitle = "";
                   if (formate.Contains("原标题"))
                   {
                       formate = formate.Replace("[原标题]", yuanTitle);
                    
                
                   }
                   if (formate.Contains("随机关键字"))
                   {
                       while (formate.Contains("随机关键字"))
                       {
                       string qian= formate.Remove(formate.IndexOf("[随机关键字]"));
                       string hou = formate.Substring(formate.IndexOf("[随机关键字]") + 7);

                       formate = qian + getRandomStringFromMaterial() + hou;
                      

                       }
                     
                     
                    
                   }
                   if (formate.Contains("主关键字"))
                   {
                       if (art.DataObject[29]==null||art.DataObject[29].ToString().Length==0)
                       {
                           Log.LogNewline("[c11]【" + Task.TaskName + "】：标题原创时遇到问题，设置了标题插入主关键字但是由于抓取方式原因文章中无主关键字，跳过该处理！[/c]");

                       formate = formate.Replace("[主关键字]", "");
                       }
                       else
                       {
                           formate = formate.Replace("[主关键字]", re.Replace(art.DataObject[29], ""));
                       }
                     
                     
                   }
                   if (formate.Contains("文章中的一句话"))
                   {
                       
                       while (formate.Contains("文章中的一句话"))
                       {

                           string qian = formate.Remove(formate.IndexOf("[文章中的一句话]"));
                           string hou = formate.Substring(formate.IndexOf("[文章中的一句话]") + 9);
                           if (art.DataObject[1].ToString().Trim().Length != 0)
                           {
                               formate = qian + GetTitleFromArticle(true, re.Replace(art.DataObject[1], "")) + hou;
                           }
                           else { formate = qian + hou; }


                       }
                       
                    
                   }
                   if (formate.Contains("随机连接符"))
                   {

                       while (formate.Contains("随机连接符"))
                       {

                           string qian = formate.Remove(formate.IndexOf("[随机连接符]"));
                           string hou = formate.Substring(formate.IndexOf("[随机连接符]") + 7);

                           if (art.DataObject[1].ToString().Trim().Length != 0)
                           {
                               formate = qian +getRandomlianjiefu()+ hou;
                           }
                           else { formate = qian + hou; }


                       }


                   }
            if (formate.Contains("随机位置插入关键字"))
            {
                int c = new Regex("\\[随机位置插入关键字\\]").Matches(formate).Count;
                formate = formate.Replace("[随机位置插入关键字]", "");
                for (int i = 0; i < c; i++)
                {
                    int index = ran.Next(1,formate.Length);
                    if (index>0) {
                        string qian = formate.Remove(index);
                        string hou = formate.Substring(index);
                        formate = qian + getRandomStringFromMaterial() + hou;
                    }
                }
            }
            
           resutlTitle = formate;
           Log.LogNewlineNosave("[c12]【" + Task.TaskName + "】：标题原创结果【" + resutlTitle + "】[/c]");

         return resutlTitle;
        }
        ////内容原创
        public string contentCreateProcess(string wenzhang)
        {
            int wc = 0;

            Random r = new Random();
            ArrayList ar = new ArrayList();
            
           

            string str = wenzhang;

            for (int i1 = 0; i1 < Model.Model_WeiYuanChuang.words.Count; i1++)
                {
                    string[] ss = Model.Model_WeiYuanChuang.words[i1].ToString().Split('=');
                    if (ss.Length < 2 || ss[0].Length == 0 || ss[1].Length == 0) { continue; }

                    Dictionary<string, string> keydic = new Dictionary<string, string>();

                    for (int i = 0; i < 2; i++)
                    {
                        ar.Add(ss[i]);
                        if (str.Contains(ss[i]))
                        {
                           
                                string key = "##" + r.Next(10000, 99999);
                                str = str.Replace(ss[i], key);
                                keydic.Add(key, ss[i]);
                                wc++;
                            
                        }

                    }


                    if (keydic.Count > 0 && ar.Count > 0)
                    {
                        foreach (KeyValuePair<string, string> s in keydic)
                        {
                           
                                ar.Remove(s.Value);
                                string g = ar[r.Next(0, ar.Count)].ToString();
                                str = str.Replace(s.Key, g);
                                ar.Add(s.Value);
                           


                        }
                    }

                    ar.Clear();

                }
            Log.LogNewline("[c12]【" + Task.TaskName + "】：伪原创" + wc + "处！[/c]");
             return  str;

            
        }
        private string summaryCreateProcess(string input)
        {
            string output = "";
            if (getIntension())
            {
                string[] result = Regex.Split(input, "。");
                int i = ran.Next(0, result.Length);
                output = result[i];
                result[i] = "";
                foreach (string s in result)
                {
                    if (!s.Equals(""))
                        output += s;
                }
                return output;
            }
            return input;
        }
        private string sentenceCreateProcess(string input)
        {
            if (input == null)
                input = "";
            input = input.ToLower().Replace("，", "。<BR>&nbsp; &nbsp; &nbsp; &nbsp;");
            return input;
        }
        //段落原创处理
        private string paragraphCreateProcess(string input)
        {
            if (getIntension())//几率决定
            {
                StringBuilder buffer = new StringBuilder();

                ArrayList oldList = splitData(input);
                ArrayList newList = RandomSort(oldList);

                for (int i = 0; i < newList.Count; i++)
                {
                    buffer.Append(Convert.ToString(newList[i]));
                }
                String outmsg = buffer.ToString().Replace("%$%$%$1", "<BR>").Replace("%$%$%$3", "<P");
                return outmsg;
            }
            else
                return input;
        }
        //打乱数组随机发布
        private ArrayList RandomSort(ArrayList array)
        {
            int len = array.Count;
            System.Collections.Generic.List<int> tempList = new System.Collections.Generic.List<int>();
            ArrayList newlist = new ArrayList();
            int i = 0;
            while (tempList.Count < len)
            {
                int iter = ran.Next(0, len);
                if (!tempList.Contains(iter))
                {
                    tempList.Add(iter);
                    newlist.Add(array[iter]);
                    i++;
                }
            }
            return newlist;
        }
        ////根据<br>和<p>来分隔字符串，并且填充到List
        private ArrayList splitData(string data)
        {
            if (data == null)
                data = "";
            data = data.ToLower().Replace("<br />", "<BR>%$%$%$1");
            data = data.ToLower().Replace("<br>", "<BR>%$%$%$1");
            data = data.ToLower().Replace("<p", "<p%$%$%$3");

            ArrayList returnList = new ArrayList();
            string[] temp = Regex.Split(data, "<BR>");
            foreach (string strInTemp in temp)
            {
                string[] s = Regex.Split(strInTemp, "<p");

                foreach (string sInStr in s)
                {
                    returnList.Add(sInStr);
                }
            }
            return returnList;
        }
        #endregion

        #region 内容混淆
        private string getRandomStringFromMaterial()//获取一个随机关键字
        {
            try
            {
                if (Model.V3Infos.KeywordDb[Task.KeywordDbId.ToString()].Keywords.Count <= 0)//如果没有关键字
                {
                    return "";
                }
                int ii = ran.Next(0, Model.V3Infos.KeywordDb[Task.KeywordDbId.ToString()].Keywords.Count);
                Log.LogNewlineNosave("[c12]【" + Task.TaskName + "】:获取到一个关键字【" + Model.V3Infos.KeywordDb[Task.KeywordDbId.ToString()]._Keywords[ii].ToString() + "】[/c]");
                return Model.V3Infos.KeywordDb[Task.KeywordDbId.ToString()]._Keywords[ii].ToString();
            }
            catch (Exception error)
            {
                Log.LogNewlineNosave("[c14]【" + Task.TaskName + "】：随机获取关键词时出错，" + error.Message+"[/c]");
                return "";
            }
         
        }
        private string getRandomStringFromMaterial(string  keywordid)//获取一个随机关键字
        {
            try
            {
                if (Model.V3Infos.KeywordDb[keywordid].Keywords.Count <= 0)//如果没有关键字
                {
                    return "";
                }
                int ii = ran.Next(0, Model.V3Infos.KeywordDb[keywordid].Keywords.Count);
                Log.LogNewlineNosave("[c12]【" + Task.TaskName + "】：获取到一个关键字【" + Model.V3Infos.KeywordDb[keywordid]._Keywords[ii].ToString() + "】[/c]");
                return Model.V3Infos.KeywordDb[keywordid]._Keywords[ii].ToString();
            }
            catch(Exception error)
            {
                Log.LogNewlineNosave("[c14]【" + Task.TaskName + "】：随机获取关键词时出错，" + error.Message+"[/c]");
                return "";
            }

        }
        private string getRandomlianjiefu()//获取一个标题连接符
        {
            if (Task.Lianjiefus.Count <= 0)//如果没有关键字
            {
                return "";
            }
            int ii = ran.Next(0, Task.Lianjiefus.Count);
            return Task.Lianjiefus[ii].ToString();

        }
        private Model.Link getRandomLinks(int linkdbId)//获取链接库里面的一个链接信息
        {
            if (Model.V3Infos.LinkDb[linkdbId.ToString()].Links.Count == 0)//如果没有关键字
            {
                return  null;
            }
            int ii = ran.Next(0, Model.V3Infos.LinkDb[linkdbId.ToString()].Links.Count);
         
            return (Model.Link)Model.V3Infos.LinkDb[linkdbId.ToString()].Links[ii];

        }
        private void processConfusion(ref string title, ref string content)  //处理内容混淆
        {

            if (Task.Ishunxiao == true)//混淆
            {
                if (content.Length == 0) { return; }

                else if (Task.Hunxiaofangshi == "内容混淆关键字模式")
                {
                    content = insertConfusionString(content, "content", false);
                }

                else if (Task.Hunxiaofangshi == "内容混淆关键字模式(含段落)")
                {
                    content = insertConfusionString(content, "content", true);
                }
             

            }
        }
        /// <summary>
        /// 链轮
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string lianlun(string str)
        {
            Regex reg = new Regex("<.+?>");
            if (Task.IsLianLun == false) { return str; }
            try
            {
                string result = "";
                int linkdbid = 0;
                if (!Task.UserDefaultLinkDB)//使用默认链接库
                {
                    linkdbid = Task.LinkDbId;
                }
                else
                {
                    linkdbid = Task.OtherLinkDB;
                }

                if (Task.Islianzhong)//中插入连接
                {
                    try
                    {
                        if (!Task.Lunjiangemoshi == true)//间隔模式
                        {
                            interval = Task.Zhongjiange;
                           
                        }
                        else
                        {
                            if (Task.Lunzongshu == 0) { Log.LogNewline("[c11]【" + Task.TaskName + "】:链接总数设置为0，不进行链轮！[/c]"); return str; }
                            interval = reg.Replace(str, "").Length / Task.Lunzongshu;

                            if (interval < 30) { Log.LogNewline("[c11]【" + Task.TaskName + "】：文章内容过短，但是插入数量设置太高，自动设置链接插入间隔为30！[/c]"); interval = 30; }
                        }




                        result = lian(interval, linkdbid, str, Task.Zhongstring, "lianlun");

                    }
                    catch (Exception error)
                    {

                        Log.LogNewline("[c14]【" + Task.TaskName + "】：链轮时出错：" + error.Message + "[/c]");
                        return result;
                    }
                }
                else { result = str; }
                if (Task.Isliantou)//头部插入连接
                {
                    for (int i = 0; i < Task.Toushuliang; i++)
                    {
                        Model.Link l = getRandomLinks(linkdbid);
                        if (l == null) { return str; }
                        result = Task.Toustring.Replace("[链接地址]", l.Url).Replace("[关键词]", l.Keyword).Replace("[标题]", l.Title) + result;
                    }

                }
                if (Task.Islianwei)//尾部插入连接
                {
                    for (int i = 0; i < Task.Weishuliang; i++)
                    {
                        Model.Link l = getRandomLinks(linkdbid);
                        if (l == null) { return str; }
                        result = result + Task.Weistring.Replace("[链接地址]", l.Url).Replace("[关键词]", l.Keyword).Replace("[标题]", l.Title);
                    }
                }
                // Log.logyichangstr("任务『" + frmMain.DBS.task[TaskId].TaskName + "』：混淆格式设置有误！", "任务『" + frmMain.DBS.task[TaskId].TaskName + "』：混淆格式设置有误！");
                return result;
            }
            catch(Exception error)
            {
                Log.LogNewline("[c14]【" + Task.TaskName + "】：链轮时出错：" + error.Message + "[/c]");
                return str;
            }
        }
        public ArrayList tagword = new ArrayList();
        private string zidingyilink(string str)
        {
            tagword.Clear();
            Regex reg = new Regex("<.+?>");

            if (Task.IszidingLink == false) { return str; }
            Task.Bencifabu++; 
            try
            {

                //随机链接模式
                if (Task.Issuijilian == true)
                {
                    if (Task.Bencifabu >= Task.Suijirandom && Task.Bencifabu % Task.Suijirandom == 0)
                    {


                        //-------------------------------------------------------------
                        string result = "";
                        int linkdbid = 0;

                        linkdbid = Task.ZidingLinkDb;


                        if (Task.Zidingzhong)//中插入连接
                        {
                            try
                            {
                                if (!Task.Zidingjiangemoshi)//true,总数模式
                                {
                                    interval = reg.Replace(str, "").Length / (Task.Zidingzhongshuliang);
                                    if (interval < 30) { Log.LogNewline("[c11]【" + Task.TaskName + "】：文章内容过短，但是插入数量设置太高，自动设置链接插入间隔为30！[/c]"); interval = 30; }
                                }
                                else
                                {
                                    interval = Task.Zidingzhongjiange;
                                }


                                result = lian(interval, linkdbid, str, Task.Zidingzhongstring, "zidingyi");
                            }
                            catch (Exception error)
                            {

                                Log.LogNewline("[c14]【" + Task.TaskName + "】：自定义链接时出错：" + error.Message + "[/c]");
                                return result;
                            }
                        }
                        else { result = str; }
                        if (Task.Zidingtou)//头部插入连接
                        {
                            for (int i = 0; i < Task.Zidingyitoushulian; i++)
                            {

                                Model.Link l = getRandomLinks(linkdbid);
                                if (l == null) { return str; }
                                result = Task.Zidingtoustring.Replace("[链接地址]", l.Url).Replace("[关键词]", l.Keyword).Replace("[标题]", l.Title) + result;
                            }

                        }
                        if (Task.Zidingwei)//尾部插入连接
                        {
                            for (int i = 0; i < Task.Zidingweishuliang; i++)
                            {
                                Model.Link l = getRandomLinks(linkdbid);
                                if (l == null) { return str; }

                                result = result + Task.Zidingweistring.Replace("[链接地址]", l.Url).Replace("[关键词]", l.Keyword).Replace("[标题]", l.Title);
                            }
                        }
                        // Log.logyichangstr("任务『" + frmMain.DBS.task[TaskId].TaskName + "』：混淆格式设置有误！", "任务『" + frmMain.DBS.task[TaskId].TaskName + "』：混淆格式设置有误！");
                        return result;
                    }
                    else { return str; }
                    /////---------------------------------------------------------------------------
                
                
                }
                else
                {
                    ///-------------------------------------------------------------
                    string result = "";
                    int linkdbid = 0;

                    linkdbid = Task.ZidingLinkDb;


                    if (Task.Zidingzhong)//中插入连接
                    {
                        try
                        {
                            if (!Task.Zidingjiangemoshi)//true,总数模式
                            {
                                interval =reg.Replace(str, "").Length / Task.Zidingzhongshuliang;
                                if (interval < 30) { Log.LogNewline("[c11]【" + Task.TaskName + "】：文章内容过短或插入数量设置太高，自动设置链接插入间隔为30！[/c]"); interval = 30; }
                            }
                            else
                            {
                                interval = Task.Zidingzhongjiange;
                            }


                            result = lian(interval, linkdbid, str, Task.Zidingzhongstring,"lianlun");
                        }
                        catch (Exception error)
                        {

                            Log.LogNewline("[c14]【" + Task.TaskName + "】：自定义链接时出错：" + error.Message + "[/c]");
                            return result;
                        }
                    }
                    else { result = str; }
                    if (Task.Zidingtou)//头部插入连接
                    {
                        for (int i = 0; i < Task.Zidingyitoushulian; i++)
                        {

                            Model.Link l = getRandomLinks(linkdbid);
                            if (l == null) { return str; }
                            result = Task.Zidingtoustring.Replace("[链接地址]", l.Url).Replace("[关键词]", l.Keyword).Replace("[标题]", l.Title) + result;
                        }

                    }
                    if (Task.Zidingwei)//尾部插入连接
                    {
                        for (int i = 0; i < Task.Zidingweishuliang; i++)
                        {
                            Model.Link l = getRandomLinks(linkdbid);
                            if (l == null) { return str; }

                            result = result + Task.Zidingweistring.Replace("[链接地址]", l.Url).Replace("[关键词]", l.Keyword).Replace("[标题]", l.Title);
                        }
                    }
                    // Log.logyichangstr("任务『" + frmMain.DBS.task[TaskId].TaskName + "』：混淆格式设置有误！", "任务『" + frmMain.DBS.task[TaskId].TaskName + "』：混淆格式设置有误！");
                    return result;
                    /////---------------------------------------------------------------------------
                }
            }
            catch (Exception error)
            {
                Log.LogNewline("[c14]【" + Task.TaskName + "】：链轮时出错：" + error.Message + "[/c]");
                return str;
            }
        }
        
        int interval = 0;
        private string insertConfusionString(string myr, string type, bool paragraph)  //插入混淆字符,如果标题混淆，混淆间隔就取3-10之间的随机数
        {
            Regex reg = new Regex("<.+?>");
            string output = "";
            
            if (type.Equals("title"))
                interval = ran.Next(5, 15);
            else
            {
                if (Task.Guanjiancimoshi == false)//总数限制模式
                {

                    interval = Task.Keywordjiange;
                }
                if (Task.Guanjiancimoshi == true)//总数限制模式
                {

                    interval = reg.Replace(myr, "").Length / Task.Keywordtotal;
                    if (interval < 30) { Log.LogNewline("[c11]【" + Task.TaskName + "】：文章内容过短，但是插入数量设置太高，自动设置链接插入间隔为30！[/c]"); interval = 30; }
                }
                if (paragraph)//判断是不是含段落          
                    myr = confuseParagraph(myr);
            }
           
                
                if (type.Equals("title"))
                {
                    string sString = subString(myr, interval);
                    output += sString + processConfusionFormattitle();//混淆标题
                    myr = myr.Substring(sString.Length);
                }
                else
                {
                    output = hun(interval, myr);
                }
            
          
            return output;
        }
        private string confuseParagraph(string data) //段落混淆
        {
            int duanhuncount = 0;
            StringBuilder buffer = new StringBuilder();

            ArrayList oldList = splitData(data);
            ArrayList newList = RandomSort(oldList);

            for (int i = 0; i < newList.Count; i++)
            {
                duanhuncount++;
                buffer.Append(Convert.ToString(newList[i]));
            }
            String outmsg = buffer.ToString().Replace("%$%$%$1", "<BR>").Replace("%$%$%$3", "<P");
            Log.LogNewlineNosave("[c12]【" + Task.TaskName + "】：段落混淆" + duanhuncount + "处！[/c]");
            return outmsg;
        }
        private string subString(string str, int len)//字符截取,中文一个字符两个字节处理
        {
           Regex regex = new Regex("^[\u4e00-\u9fa5]$");
           
            StringBuilder sb = new StringBuilder();
            int nLength = 0;
            int count = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (regex.IsMatch((str[i]).ToString()))
                {
                    sb.Append(str[i]);
                    nLength += 2;
                    ++count;
                    if (count >= len)
                        break;
                }
                else
                {
                    sb.Append(str[i]);
                    nLength = nLength + 1;
                }
            }
            return sb.ToString();
        }
        private string processConfusionFormat()//混淆内容
        {
            string result = "";
            string 随机素材 = getRandomStringFromMaterial();
            if (Task.Hunxiaogeshi.Contains("|") && Task.Hunxiaogeshi.Contains("-"))
            {
                try
                {
                    string[] 替换格式 = Task.Hunxiaogeshi.Split(new char[] { '|' });
                    string[] 替换参数 = 替换格式[0].Split(new char[] { '-' });
                    int 全局替换次数 = Convert.ToInt32(替换参数[0]);
                    int 单次替换次数 = Convert.ToInt32(替换参数[1]);
                    if (替换格式.Length > 2)
                    {
                        int i8 = ran.Next(1, 替换格式.Length);
                        替换格式[1] = Convert.ToString(替换格式[i8]);
                    }
                    string 替换的内容 = 替换格式[1].Replace("[$素材$(1)]", 随机素材);
                    替换的内容 = 替换的内容.Replace("[$素材$(2)]", HttpUtility.UrlEncode(随机素材, Encoding.Default));
                    替换的内容 = 替换的内容.Replace("[$素材$(3)]", HttpUtility.UrlEncode(随机素材, Encoding.UTF8));
                    if (替换格式[1].Contains("CODE"))
                        替换的内容 = 替换的内容.Replace("%", "_").Replace("CODE", "");
                    else
                    {
                        //if (!frmMain.DBS.website[frmMain.DBS.task[TaskId].TaskStieId].Mode1_APIurl.Contains(".php"))
                        //    替换的内容 = 替换的内容.Replace("%", "%25"); ;
                    }
                    result = 替换的内容;
                }
                catch
                {
                   // Log.logyichangstr("任务『" + frmMain.DBS.task[TaskId].TaskName + "』：混淆格式设置有误！", "任务『" + frmMain.DBS.task[TaskId].TaskName + "』：混淆格式设置有误！");
                    return result;
                }
            }
            else
            {
               // Log.logyichangstr("任务『" + frmMain.DBS.task[TaskId].TaskName + "』：混淆格式设置有误！", "任务『" + frmMain.DBS.task[TaskId].TaskName + "』：混淆格式设置有误！");
                return result;
            }
            return result;
        }
        private string processConfusionFormattitle()//混淆标题
        {
            string result = "";
            string[] splitString = "$素材$".Split('$');
            result = splitString[0];
            result += getRandomStringFromMaterial();
            result += splitString[splitString.Length - 1];
            return result;
        }
        private string maohunxiao(string content,int linkdbid) 
        {
            try
            {
                if (content == null) { return content; }
               
                Regex r = new Regex("aa");
                int count = 0;
                if (Model.V3Infos.LinkDb[linkdbid.ToString()].Links.Count == 0)//如果没有链接
                {
                    Log.LogNewline("[c11]【" + Task.TaskName + "】：锚文本混淆时链接库无链接，不进行混淆！[/c]");
                    return content;
                }
                for (int i = 0; i < Model.V3Infos.LinkDb[linkdbid.ToString()].Links.Count; i++)
                {

                    
                    Model.Link link = (Model.Link)Model.V3Infos.LinkDb[linkdbid.ToString()].Links[i];
                    if (link.Keyword.Trim().Length > 0)
                    {
                        r = new Regex(link.Keyword.Trim());



                     int zong=r.Matches(content).Count;
                     content = r.Replace(content, Task.Maostring.Replace("[关键词]", link.Keyword).Replace("[链接地址]", link.Url).Replace("[标题]", link.Title), Task.Maoxiangtong);
                     if (zong > Task.Maoxiangtong)
                     {
                         count = count + Task.Maoxiangtong;
                     }
                        else
                     {
                         count = count + zong;
                     }
                     if (count > Task.Maoshulian)
                            {
                                Log.LogNewline("[c11]【" + Task.TaskName + "】：锚文本数量达到最大值，不再在该文章插入！[/c]");
                                Log.LogNewlineNosave("[c12]【" + Task.TaskName + "】：锚混淆" + count + "处！[/c]");
                                return content;
                            }
                        

                    }
                    if (count > Task.Maoshulian)
                    {
                        Log.LogNewline("[c11]【" + Task.TaskName + "】：锚文本数量达到最大值，不再在该文章插入！[/c]");
                        Log.LogNewlineNosave("[c12【" + Task.TaskName + "】：锚混淆" + count + "处！[/c]");
                        return content;
                    }

                }
                Log.LogNewlineNosave("[c12]【" + Task.TaskName + "】：锚混淆" + count + "处！[/c]");
                return content;
            }
            catch (Exception err) { Log.LogNewlineNosave("[c14]【" + Task.TaskName + "】：锚混淆出现错误，" + err.Message + "！[/c]"); return content; }
        }
        #endregion

        #region 内容过滤


        //正规化处理，处理qq号，手机号，邮箱，图片，超链接
        private void htmlguolv()
        {


            for (int objc = 0; objc < 2; objc++)
            {
                if (Task.Iszhenggui == true) //文章正规化处理
                {
                    V3.Common.Format f = new Format();
                    if (objc != 0)
                    {
                        art.DataObject[objc] = f.formatText(art.DataObject[objc].ToString(), Task.id);
                    }
                }
                if (Task.NoQQ == true) //过滤QQ
                {
                    Regex r = new Regex(@"\d{6,10}");
                    art.DataObject[objc] = r.Replace(art.DataObject[objc].ToString(), Task.MyQQ);
                }
                if (Task.NoEmail == true) //过滤邮箱
                {
                    Regex r = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                    art.DataObject[objc] = r.Replace(art.DataObject[objc].ToString(),
                        Task.MyEmail);
                }
                if (Task.NoA == true) //过滤a标签
                {
                    Regex regex2 = new Regex(@"<a\s+href=(?<url>.+?)>(?<content>.+?)</a>",
                        RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    MatchCollection mc2 = regex2.Matches(art.DataObject[objc].ToString());
                    foreach (Match m2 in mc2)
                    {
                        if (m2.Success)
                        {
                            string url = m2.Groups["url"].Value;
                            string content = m2.Groups["content"].Value;
                            if (Task.MyA.Length > 0) //替换链接
                            {
                                art.DataObject[objc] = art.DataObject[objc].Replace(url,
                                    "\"" + Task.MyA + "\"");
                            }
                            else //移除a标签
                            {
                                art.DataObject[objc] = art.DataObject[objc].Replace(m2.Value, content);
                            }
                        }
                    }

                }
                if (Task.NoPhone == true) //过滤手机
                {
                    Regex r = new Regex(@"(13\d{9}|15\d{9}|18\d{9})");

                    art.DataObject[objc] = r.Replace(art.DataObject[objc].ToString(),
                        Task.MyPhone);

                    r = new Regex(@"\d{3,4}-\d{7,8}");
                    art.DataObject[objc] = r.Replace(art.DataObject[objc].ToString(),
                        Task.MyPhone);



                }
                if (Task.NoPic == true) //过滤图片
                {
                    Regex r = new Regex("<img.*?>");
                    art.DataObject[objc] = r.Replace(art.DataObject[objc].ToString(), Task.MyPic);
                }

                if (Task.NoUrl == true) //过滤网址
                {
                    Regex r = new Regex("href=([\"\"'])?(?<href>[^'\"\"]+)\\1[^>]");
                    art.DataObject[objc] = r.Replace(art.DataObject[objc].ToString(), Task.MyUrl.Trim().Length > 0 ? " href=\"" + Task.MyUrl + "\" " : " href=\"#\" ");
                }
            } 
        }
        #endregion

        #region 敏感字符过滤
        public   string  minganguolv()
        {
            string s = "";
            for(int objc=0; objc< 30;objc++)
            {

                if (art.DataObject[objc] == null) { return ""; }
                string result = art.DataObject[objc].ToString();
     
            try
            {
                if (Task.Mingan == "不过滤") { }
                else if (Task.Mingan == "过滤为*号")
                {
                    foreach (string i in Model.V3Infos.MainDb.Minganwords)
                        {
                            Regex regex1 = new Regex( i, RegexOptions.IgnoreCase);
                            result = regex1.Replace(result, "*河蟹*");
                        }
                }
                else if (Task.Mingan == "过滤为空") 
                {
                    foreach (string i in Model.V3Infos.MainDb.Minganwords)
                        {
                            Regex regex1 = new Regex( i, RegexOptions.IgnoreCase);
                            result = regex1.Replace(result, "");
                        }
                   
                }
                else//直接跳过不发布
                {
                    ArrayList m = new ArrayList();
                    string r = "";
                    foreach (string i in Model.V3Infos.MainDb.Minganwords)
                        {
                        Regex regex1 = new Regex(i, RegexOptions.IgnoreCase);
                        MatchCollection mc=regex1.Matches(result);
                        if (mc.Count > 0)//如果
                        {
                            foreach (Match h in mc)
                            {
                                if (!m.Contains(h.Groups[0].Value)) { m.Add(h.Groups[0].Value); }
                            }
                            foreach (string g in m)
                            {
                                r = r + "  " + g + "/  ";
                            }

                            result = "含有敏感字符:" + r + "，当前设置为跳过，不发布！";
                            s = "含有敏感字符:" + r + "，当前设置为跳过，不发布！";
                            return s;
                           
                        }
                        
                    }
                
                }


                art.DataObject[objc] = result;
               

            }
            catch 
            {
                // Log.logyichangstr("任务『" + frmMain.DBS.task[TaskId].TaskName + "』：敏感字过滤设置有误！MSG:" + ex.Message, "任务『" + frmMain.DBS.task[TaskId].TaskName + "』：敏感字过滤设置有误！MSG:" + ex.Message);
            }
              
            }
                 return s;
        }
        #endregion

        #region 内容替换
        public void replace()
        {
            int ticount = 0;
           
            if (Task.Istihuan == false) { return; }


                Random r = new Random();
               
                for (int objc = 0; objc <25; objc++)
                {
                    if (art.DataObject[objc]==null||art.DataObject[objc].ToString().Length==0) { continue; }
                     string[][] allValue = new string[Model.V3Infos.ReplaceDb[Task.ReplaceDbId.ToString()].Words.Count][];
                    for (int i=0;i<allValue.Length;i++)
                    {
                        allValue[i] = Model.V3Infos.ReplaceDb[Task.ReplaceDbId.ToString()].Words[i].Split('→');
                    }
                   
                    string str =art.DataObject[objc].ToString();
                    if (Task.Isshuangxiang)
                    {
                        Dictionary<string, string> keydic = new Dictionary<string, string>();

                        for (int i1 = 0; i1 < allValue.Length; i1++)
                        {
                            string[] ss = new string[allValue[i1].Length];
                            ArrayList ar = new ArrayList();

                            for (int ii = 0; ii < allValue[i1].Length; ii++)
                            {
                                ss[ii] = allValue[i1][ii];
                                ar.Add(ss[ii]);
                            }



                            for (int i = 0; i < ss.Length; i++)
                            {

                                try
                                {

                                    if (ss[i].Length == 0) { continue; }
                                    if (Regex.IsMatch(str, ss[i]))
                                    {
                                        string rr = ss[i];
                                        ar.Remove(rr);
                                        if (ar.Count >= 1)
                                        {
                                            string key = Guid.NewGuid().ToString().Replace("-", "");
                                            str = Regex.Replace(str, rr, key);
                                            keydic.Add(key, ar[r.Next(0, ar.Count)].ToString());

                                        }
                                        ar.Add(rr);
                                    }
                                }
                                catch
                                {

                                    V3.Common.Log.LogNewline("[c14]【" + Task.TaskName + "】：替换库正则【" + ss[i] + "】有误，无法继续替换，请修改规则！[/c]");
                                    return;
                                }
                            }
                        }
                        foreach (KeyValuePair<string, string> s in keydic)
                        {

                            str = str.Replace(s.Key, s.Value);
                            ticount++;
                        }

                    }

                    else //单向替换
                    {
                        Dictionary<string, string> keydic = new Dictionary<string, string>();
                        for (int i1 = 0; i1 < allValue.Length; i1++)
                        {
                            string[] ss = new string[allValue[i1].Length];
                            ArrayList ar = new ArrayList();

                            for (int ii = 0; ii < allValue[i1].Length; ii++)
                            {
                                ss[ii] = allValue[i1][ii];
                                ar.Add(ss[ii]);
                            }



                            for (int i = 0; i < ss.Length; i++)
                            {

                                try
                                {

                                    if (ss[0].Length == 0) { continue; }
                                    if (Regex.IsMatch(str, ss[0]))
                                    {
                                        ar.Remove(ss[0]);
                                        if (ar.Count >= 1)
                                        {
                                            string key = Guid.NewGuid().ToString().Replace("-", "");
                                            str = Regex.Replace(str, ss[0], key);
                                            keydic.Add(key, ar[r.Next(0, ar.Count)].ToString());
                                            break;
                                        }
                                        else { break; }
                                    }
                                }
                                catch
                                {

                                    V3.Common.Log.LogNewline("[c14]【" + Task.TaskName + "】：替换库正则【" + ss[i] + "】有误，无法继续替换，请修改规则！[/c]");
                                    return;
                                }
                            }
                        }
                        foreach (KeyValuePair<string, string> s in keydic)
                        {

                            str = str.Replace(s.Key, s.Value);
                            ticount++;
                        }
                    }

                  
                    art.DataObject[objc] = str;
               
                }
                Log.LogNewlineNosave("[c12]【" + Task.TaskName + "】：根据替换库替换掉" + ticount + "处！[/c]");
           
        }
     
        // <summary>
        // 根据值头部和尾部，取所有匹配的值用空格分开
        // </summary>
        //private string getRegexValue(string regexString, string input, string replaceValue)
        //{
        //    string output = "";
        //    Regex regex1 = new Regex(regexString, RegexOptions.IgnoreCase);
        //    foreach (Match m in regex1.Matches(input))
        //    {
        //        if (m.Success)
        //        {
        //            input = input.Replace(m.Value, replaceValue);
        //        }
        //    }
        //    output = input;
        //    return output;
        //}
        #endregion

        #region 翻译
        string fanyi(string sss)
        {

            if (Task.Fanyiformate.Length == 3) 
            {
              
                return sss;
            }
            Dictionary<string, string> keydic = new Dictionary<string, string>();
            Random r = new Random();
            Regex reg = new Regex("<.+?>");
            MatchCollection conn = reg.Matches(sss);
            for (int i = 0; i < conn.Count; i++)
            {

                string key = "aaa" + r.Next(10, 9999) + "aaa".ToString();
                sss = Regex.Replace(sss, conn[i].Value, key);
                if (!keydic.ContainsKey(key))
                    keydic.Add(key, conn[i].Value);

            }
            string result=sss;
            string shang = "";
           fanyi f = new Common.fanyi();
           string formate = Task.Fanyiformate;
           string[] fs = formate.Split('→');
           for (int i = 0; i < fs.Length; i++)
           {
               if (shang.Length == 0)//第一次
               {
                   shang = "中文";

               }
               else
               {
                   if (shang == "中文" && fs[i] == "英文")
                   {
                       result = f.zhongtoying(result);
                       Regex rnochinese = new Regex("[\u4e00-\u9fa5]");
                       result = rnochinese.Replace(result, "");
                       shang = "英文";
                   }
                   else if (shang == "英文" && fs[i] == "中文")
                   {
                       result = f.yingtozhong(result);
                       shang = "中文";
                   }
                 
                  


                 
               }
           }


           foreach (KeyValuePair<string, string> s in keydic)
           {
               result = result.Replace(s.Key, s.Value);
           }
            return result;
        }
        #endregion
      
        // 返回汉字串的简体
        public string WordTraditionalToSimple(string Str)
        {
            string S = Microsoft.VisualBasic.Strings.StrConv(Str, Microsoft.VisualBasic.VbStrConv.SimplifiedChinese, 0);
            return S;
        }
        //返回汉字串的繁体
        public string WordSimpleToTraditional(string Str)
        {
            string S = Microsoft.VisualBasic.Strings.StrConv(Str, Microsoft.VisualBasic.VbStrConv.TraditionalChinese, System.Globalization.CultureInfo.CurrentCulture.LCID);
            return S;
        }
        string jianfan(string content,bool jian_fan)
    {
        if (jian_fan == true) 
        {
          return WordTraditionalToSimple(content);
        }
        else
        {
            return   WordSimpleToTraditional(content);
           
        }
    }
        public static string Replace(string src, string pattern, string replacement, int count)
        {
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

            return regex.Replace(src, replacement, count);

        }
        public void charudaima() 
        {

            if( art.DataObject[1] !=null){

                art.DataObject[1] = Task.ArticleTou + art.DataObject[1] + Task.ArticleWei;
            }
           
        }
        public string lian(int jiange,int linkdb, string str,string formate,string what)//链接库插入通用规则 
        {
            tagword.Clear();
            int liancount = 0;
            StringBuilder sb = new StringBuilder();
            Dictionary<string, string> k = new Dictionary<string, string>();

            ArrayList ta = Library.RegexHelper.GetArrayList(str, "<a.*?</a>");
           
                for (int i = 0; i < ta.Count; i++)
                {
                    if (!k.ContainsKey(ta[i].ToString()))
                    {
                        k.Add("⊙" + (i+1) + "ん", ta[i].ToString());
                        str = str.Replace(ta[i].ToString(), "⊙" + (i+1) + "ん");
                    }
                }

            //指定不在里面插入的标签b标签
                 ta = Library.RegexHelper.GetArrayList(str, "<b.*?</b>");

                for (int i = 0; i < ta.Count; i++)
                {
                    if (!k.ContainsKey(ta[i].ToString()))
                    {
                        k.Add("⊙" + (i+1)*100 + "ん", ta[i].ToString());
                        str = str.Replace(ta[i].ToString(), "⊙" + (i+1) *100+ "ん");
                    }
                }

                //指定不在里面插入的标签i标签
                ta = Library.RegexHelper.GetArrayList(str, "<i.*?</i>");

                for (int i = 0; i < ta.Count; i++)
                {
                    if (!k.ContainsKey(ta[i].ToString()))
                    {
                        k.Add("⊙" + (i+1)*1000 + "ん", ta[i].ToString());
                        str = str.Replace(ta[i].ToString(), "⊙" + (i+1)*1000 + "ん");
                    }
                }


                //指定不在里面插入的标签u标签
                ta = Library.RegexHelper.GetArrayList(str, "<u.*?</u>");

                for (int i = 0; i < ta.Count; i++)
                {
                    if (!k.ContainsKey(ta[i].ToString()))
                    {
                        k.Add("⊙" + (i+1)*10000 + "ん", ta[i].ToString());
                        str = str.Replace(ta[i].ToString(), "⊙" + (i+1)*10000 + "ん");
                    }
                }




            ArrayList t = Library.RegexHelper.GetArrayList(str, "<.+?>");
            int scount = k.Count;
           
                for (int i = scount; i < t.Count + scount; i++)
                {
                    if (!k.ContainsKey(t[i - scount].ToString()))
                    {
                        k.Add("⊙" + (i+1)*100000 + "ん", t[i-scount].ToString());
                        str = str.Replace(t[i - scount].ToString(), "⊙" + (i+1) * 100000 + "ん");
                    }
                }
           




            //Regex r = new Regex(@"[^!@#$%&*()<>`1234567890]");
            bool ok = true;
            int count = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '⊙')
                    ok = false;
                else if (str[i] == 'ん')
                    ok = true;
                sb.Append(str[i]);
               // bool aa=r.IsMatch((str[i]).ToString());
                if (ok && str[i] != 'ん')
                {
                   count++;
                    if (count >= jiange)
                    {
                        count = 0;
                        Model.Link l;
                        if (Convert.ToInt32(99) == 11861)
                        {
                            Log.LogNewline("[c11]【" + Task.TaskName + "】：无重复获取链接中...[/c]");
                            l = getLink(linkdb);
                        }
                        else
                        {
                            l = getRandomLinks(linkdb);
                            if (l == null)
                            {
                                Log.LogNewline("[c14]【" + Task.TaskName + "】：无法从链接库里面取到链接！[/c]");
                                break;
                            }

                        }
                        if (l == null)
                        {
                            Log.LogNewline("[14]【" + Task.TaskName + "】：无法从链接库里面取到链接！[/c]");
                            break;
                        }

                        sb.Append(formate.Replace("[链接地址]", l.Url).Replace("[关键词]", l.Keyword).Replace("[标题]", l.Title)); 
                        liancount++;
                    }
                }
                else if (str[i] != 'ん' && str[i] != '⊙')
                {
                    count++;
                }
            }
            string result = sb.ToString();
            foreach (KeyValuePair<string, string> kv in k)
            {
              
               result=result.Replace(kv.Key, kv.Value);
            }
            Log.LogNewlineNosave("[c12]【" + Task.TaskName + "】：插入链接" + liancount + "处！[/c]");
         
          
            return result;
        }
        public string hun(int jiange, string str)//通用混淆规则
        {
           
            StringBuilder sb = new StringBuilder();
            Dictionary<string, string> k = new Dictionary<string, string>();


            ArrayList ta = Library.RegexHelper.GetArrayList(str, "<a.*?</a>");
            for (int i = 0; i < ta.Count; i++)
            {
                if (!k.ContainsKey(ta[i].ToString()))
                {
                    k.Add("⊙" + (i+1) + "ん", ta[i].ToString());
                    str = str.Replace(ta[i].ToString(), "⊙" + (i+1) + "ん");
                }
            }


            ArrayList t = Library.RegexHelper.GetArrayList(str, "<.+?>");
            int scount = k.Count;

            for (int i = scount; i < t.Count + scount; i++)
            {
                if (!k.ContainsKey(t[i - scount].ToString()))
                {
                    k.Add("⊙" + (i+1)*1000 + "ん", t[i - scount].ToString());
                    str = str.Replace(t[i - scount].ToString(), "⊙" + (i+1)*1000 + "ん");
                }
            }
            Regex r = new Regex(@"[^!@#$%&*()<>`1234567890]");
            bool ok = true;
            int count = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '⊙')
                    ok = false;
                else if (str[i] == 'ん')
                    ok = true;
                sb.Append(str[i]);
                bool aa = r.IsMatch((str[i]).ToString());
                if (ok && str[i] != 'ん' && aa)
                {
                    count++;
                    if (count >= jiange)
                    {
                        count = 0;
                        sb.Append(processConfusionFormat());
                    }
                }
            }
            string result = sb.ToString();
            foreach (KeyValuePair<string, string> kv in k)
            {
                result = result.Replace(kv.Key, kv.Value);
               
            }
           
            return result;
        }
        public static string geregex(string html, string regex, int num, bool msg)
        {
            string result = "";
            try
            {

                MatchCollection mccc = Regex.Matches(html, regex, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                for (int i = 0; i < mccc.Count; i++)
                {
                    result = mccc[i].Groups[0].Value;
                    if (i == num)
                        break;
                }
                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
        }
        public string settagsfromkeyword(string s) 
        {
            
         string str = s;
         string[] sc = geregex(Task.Canshu, "(?<=<settags>).*?(?=</settags>)", 1, false).Split('|');
            if(sc.Length<3)
            {
                return s;
                Log.LogNewline("[c14]tags参数设置错误！[/c]");
            }
        int jiange = s.Length / Convert.ToInt32(sc[2]);
        if (sc[0] == "True" && sc[1] == "False")
        {
            art.DataObject[29] = "";
         
            List<string> mykeywords = new List<string>();
            if (Convert.ToInt32(sc[2]) >= Model.V3Infos.KeywordDb[Task.KeywordDbId.ToString()]._Keywords.Count)
            {
                mykeywords = xEngine.Common.XSerializable.CloneObject<List<string>>(Model.V3Infos.KeywordDb[Task.KeywordDbId.ToString()]._Keywords);
            }
            else//随机取出指定数量的关键词 
            {

                List<string> arr = xEngine.Common.XSerializable.CloneObject<List<string>>(Model.V3Infos.KeywordDb[Task.KeywordDbId.ToString()]._Keywords);
                int len = Model.V3Infos.KeywordDb[Task.KeywordDbId.ToString()]._Keywords.Count;
                    Random rand = new Random();
                for (int i = 0; i < Convert.ToInt32(sc[2]); i++)
                {
                   
                    int r = rand.Next(0,arr.Count);
                    mykeywords.Add(arr[r].ToString());
                    arr.Remove(arr[r]);
                }
            }

           





            ArrayList zuiwords = new ArrayList();
            int liancount = 0;
            StringBuilder sb = new StringBuilder();
            Dictionary<string, string> k = new Dictionary<string, string>();

            ArrayList ta = Library.RegexHelper.GetArrayList(s, "<a.*?</a>");

            for (int i = 0; i < ta.Count; i++)
            {
                if (!k.ContainsKey(ta[i].ToString()))
                {
                    k.Add("⊙" + i + "ん", ta[i].ToString());
                    str = str.Replace(ta[i].ToString(), "⊙" + i + "ん");
                }
            }
            ArrayList t = Library.RegexHelper.GetArrayList(str, "<.+?>");
            int scount = k.Count;

            for (int i = scount; i < t.Count + scount; i++)
            {
                if (!k.ContainsKey(t[i - scount].ToString()))
                {
                    k.Add("⊙" + i + "ん", t[i - scount].ToString());
                    str = str.Replace(t[i - scount].ToString(), "⊙" + i + "ん");
                }
            }

            Regex re = new Regex(@"[^!@#$%&*()<>`1234567890]");
            bool ok = true;
            int count = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '⊙')
                    ok = false;
                else if (str[i] == 'ん')
                    ok = true;
                sb.Append(str[i]);
                bool aa = re.IsMatch((str[i]).ToString());
                if (ok && str[i] != 'ん' && aa)
                {
                    count++;
                    if (count > jiange)
                    {
                        count = 0;
                        if (mykeywords.Count > 0)
                        {
                            liancount++;
                            sb.Append(mykeywords[mykeywords.Count - 1]);
                            if (!zuiwords.Contains(mykeywords[mykeywords.Count - 1]))
                            {
                                zuiwords.Add(mykeywords[mykeywords.Count - 1]);
                            }
                            mykeywords.RemoveAt(mykeywords.Count - 1);
                        }

                    }
                }
            }
            Log.LogNewline("[c12]最终得到取出" + zuiwords.Count + "个关键词作为tags...[/c]");
            for (int i = 0; i < zuiwords.Count; i++)
            {
                if (i == 0)
                { art.DataObject[29] = zuiwords[i].ToString(); }
                else
                {

                    art.DataObject[29] += "," + zuiwords[i].ToString();

                }
            }
            string result = sb.ToString();
            foreach (KeyValuePair<string, string> kv in k)
            {

                result = result.Replace(kv.Key, kv.Value);
            }
            Log.LogNewlineNosave("内容处理：插入关键字" + liancount + "个！");

            return result;
        }
        else
        { return str; }

        }

        #region  文章处理方法
        //随机调用链接库中链接的时候，同样是随机调用，但是链接地址不能重复。在链轮 自定义链轮 锚文本都进行判断
        private Model.Link getLink(int linkdbId)//不重复，获取链接库里面的一个链接信息
        {
            //Model.Model_Link l=null;
            //int trycount=0;
            //if (Model.V3Infos.LinkDb[linkdbId.ToString()].Links.Count == 0)//如果没有链接
            //{
            //    return null;
            //}
            //while(l==null&&trycount<10)
            //{
            //    l = getRandomLinks(linkdbId);
            //    if (Task.Links.Contains(l.Url))
            //    {
            //        l = null;
            //        trycount++;
            //    }
            //    else
            //    {
            //        Task.Links.Add(l.Url);
            //        return l;
            //    }
            
            //}
            //return l;

            return null;


        }
        #endregion

        #region 修改tags为一个随机关键词
        private   void settages(ref Model.Model_Article art)
       {
           //if (xEngine.License.MyLicense.Custom.Contains("kan"))
           //{

           //    string gonggongkustr = geregex(Task.Canshu, "(?<=<modifytags>).*?(?=</modifytags>)", 1, false);
           //    if (gonggongkustr.Split('|').Length >= 2)
           //    {
           //        string[] modifytags = gonggongkustr.Split('|');
           //        if(modifytags[0].ToString()=="True")
           //        {
           //        art.DataObject[29]=getRandomStringFromMaterial(modifytags[1].ToString());
           //        }


           //    }



           //    string suijilink = geregex(Task.Canshu, "(?<=<suijilink>).*?(?=</suijilink>)", 1, false);
           //    if (suijilink.Split('|').Length >= 5)
           //    {
           //        string[] suijilinkcanshu = suijilink.Split('|');
           //        //随机插入链接
           //        if (suijilinkcanshu[0].ToString() == "True")
           //        {
           //            string linkdbid = suijilinkcanshu[1];
           //            string suijifrom = suijilinkcanshu[2];
           //            string suijito = suijilinkcanshu[3];
           //            string suijigeshi = suijilinkcanshu[4];
           //            Random r = new Random();
           //           int count= r.Next(Convert.ToInt32(suijifrom), Convert.ToInt32(suijito));
           //           Regex reg = new Regex("<.+?>");
           //           if (count == 0) { return; }
           //               art.DataObject[1] = lian((reg.Replace(art.DataObject[1],"").Length / count),Convert.ToInt32( linkdbid), art.DataObject[1].ToString(), suijigeshi, "");
                      
                      
           //        }
           //    }



           //    string zidinglink = geregex(Task.Canshu, "(?<=<zidinglink>).*?(?=</zidinglink>)", 1, false);
           //    if (zidinglink.Split('|').Length >= 5)
           //    {
           //        string[] zidinglinkcanshu = zidinglink.Split('|');
             
           //        //随机插入链接
           //        if (zidinglinkcanshu[0].ToString() == "True")
           //        {
           //            string linkdbid = zidinglinkcanshu[1];
           //            string di = zidinglinkcanshu[2];
           //            string p = zidinglinkcanshu[3];
           //            string zidinggeshi = zidinglinkcanshu[4];
           //            Random r = new Random();
           //            int count = Convert.ToInt32(di);
           //            int diji = 0;
           //            int start=0;
           //            string content = art.DataObject[1].ToString();
           //            while(content.Contains(p))
           //            {
           //                Log.LogNewline("找到符号：【"+p+"】！", 1);
           //                int  a = content.Length;
           //                content = "ん" + content.Substring(content.IndexOf(p) + 1);
           //                int b=content.Length;
           //                start += a - b;
                           
                       
           //                diji++;
                          
           //                if(diji==count)//第几个
           //                {

           //                    string hou = art.DataObject[1].ToString().Substring(start);
           //                    string qian = art.DataObject[1].ToString().Remove(start); 
           //                 Model.Model_Link inlink=getLink(Convert.ToInt32(linkdbid));
           //                 if (inlink == null)
           //                 {
           //                     Log.LogNewline("无法从链接库里面取到链接！", 4);
           //                     break;
           //                 }
           //                 else
           //                 {
           //                     hou = hou.Replace("ん",p);
           //                     qian = qian.Replace("ん",p);
           //                     art.DataObject[1] = qian + (zidinggeshi.Replace("[链接地址]", inlink.Url).Replace("[关键词]", inlink.Keyword).Replace("[标题]", inlink.Title)) + hou;
           //                     break;

           //                 }

           //                }
                       
           //            }
           //        }
           //    }



           //}
           
       }
        #endregion

        #region  文章处理功能
        private void setart(ref Model.Model_Article art)
        {

          
        
        
        }
        //把一段文字随机插入到一篇文章内容里面
        private string suijiinsert(string content, string insertstr)
        {
            Random r=new Random();
            content=content.ToLower().Replace("</p>","⊙");

            string result = "";
            string[] s = content.Split('⊙');
            if (s.Length >4) 
            {
                int rr=r.Next(0,s.Length);
                for (int i = 0; i < s.Length;i++ ) 
                {
                if(i==rr)
                {
                    result+= insertstr + s[i] + "⊙";
                }
                else    
                {
                    result += s[i] + "⊙";
                }

                }
            }
            else
            {
                s = content.Split('。');
                if (s.Length > 4) 
                {
                    int rr = r.Next(0, s.Length);
                    for (int i = 0; i < s.Length; i++)
                    {
                        if (i == rr)
                        {
                            result = insertstr + s[i] + "。";
                        }
                        else
                        {
                            result += s[i] + "。";
                        }

                    }
                }
                else
                {
                    s = content.Split(',');
                    if(s.Length>4)
                    {
                        int rr = r.Next(0, s.Length);
                        for (int i = 0; i < s.Length; i++)
                        {
                            if (i == rr)
                            {
                                result = insertstr + s[i] + "，";
                            }
                            else
                            {
                                result += s[i] + "，";
                            }

                        }
                    }
                    else
                    {
                    result=content;
                    }
                }

                }
            return result.Replace("⊙", "</p>");
        
        }
        //随机获取文章库里面一篇文章中的一段
        private string  getduanfromku() 
        {
            string filename = "";
            Model.Model_Article art = ArticleBll.GetrandomArticleAndDel(Task.ArticleDbId.ToString(), ref  filename);
            try
            {
                string duan= GetduanFromArticle(true,ref art);
                string sss = filename.Substring(filename.LastIndexOf('\\') + 1);
                try
                {
                    xFile.SaveFileNoBuff(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\ArticleDb\\" + Task.ArticleDbId.ToString() + "\\", sss.Remove(sss.LastIndexOf('.')), xEngine.Common.XSerializable.ObjectToBytes(art));
                }
                catch (Exception err) 
                {
                    Log.LogNewline("[c14]文章重写失败！原因："+err.Message+"[/c]"); 
                }
                return duan;
               
            }
            catch { Log.LogNewline("[c14]无法从文章库里面随机取出一段，因为文章库无数据！[/c]"); return ""; }
            
        }
       
        public static string GetduanFromArticle(bool isRandom,ref  Model.Model_Article art)
        {

            Regex re = new Regex("<p[\\s\\S]*?</p>");
            ArrayList titlelist = new ArrayList();
            string title = "无标题";
            string article =art.DataObject[1].ToString();

           MatchCollection conn= re.Matches(article);
           if (conn.Count>0)
            {
               
                for (int i = 0; i < conn.Count; i++)
                {
                    titlelist.Add(conn[i].Value);
                }
            
            }
            else if (article.Contains("</br>"))
            {
                article = article.Replace("</br>", "⊙");

                string[] al = article.Split('⊙');
                for (int i = 0; i < al.Length;i++ ) 
                {
                    titlelist.Add(al[i].ToString().Replace("⊙","</br>"));
                
                }


            
            
            }

           
         
          
            if (titlelist.Count <= 0) { return ""; }
            if (isRandom == true)
            {
                Random r = new Random();
                title = titlelist[r.Next(0, titlelist.Count)].ToString();
            }
            else { title = titlelist[0].ToString(); }
            titlelist.Clear();
 art.DataObject[1]=art.DataObject[1].ToString().Replace(title,"");
            return title.Trim();
           
        }
        //去掉第一段
        private string overwrite(string content) 
        {

            if (content.Contains("</p>")) { content = content.Substring(content.IndexOf("</p>") + 4); }
            else if (content.Contains("。"))
            {
                content = content.Substring(content.IndexOf("。") + 1);
            }
            else if (content.Contains("!"))
            {
                content = content.Substring(content.IndexOf("!") + 1);
            }
            else if (content.Contains("！"))
            {
                content = content.Substring(content.IndexOf("！") + 1);
            }
            return content;

        
        }
        //去掉最后一段
        private string overwritelast(string content)
        {

            if (content.Contains("<p")) { content = content.Remove(content.LastIndexOf("<p")); }
            else if (content.Contains("。"))
            {
                content = content.Remove(content.LastIndexOf("。"));
            }
            else if (content.Contains("!"))
            {
                content = content.Remove(content.LastIndexOf("!"));
            }
            else if (content.Contains("！"))
            {
                content = content.Remove(content.LastIndexOf("！"));
            }
            return content;


        }
        //自由组合
        private string chonggou(string content)
        {

            Random rdm = new Random();
           
            content = content.ToLower();
            Regex re = new Regex("<p[\\s\\S]*?</p>");
            MatchCollection conn = re.Matches(content);
            string [] ps=new string[conn.Count];
            
            for (int i = 0; i < conn.Count;i++ )
            {
                ps[i] = conn[i].Value;
            }
            //有p标签
            string result="";
            if (ps.Length > 0)
            {
                string[] sp = RandomSort(ps);
                for (int p = 0; p < sp.Length; p++)
                {
                    int rdouorju = rdm.Next(0, 2);
                    string r = "";
                    if (rdouorju == 0)
                    {


                        #region  以逗号重组
                        if (sp[p].Contains("，"))
                        {

                            string tou = sp[p].Remove(sp[p].IndexOf(">") + 1);
                            string cont = sp[p].Replace(tou, "").Replace("</p>", "");
                            string[] ido = RandomSort(cont.Split('，'));
                            for (int i = 0; i < ido.Length; i++)
                            {
                                string asaa = ido[i].Substring(ido[i].Length - 1);
                                if (i == ido.Length - 1)
                                {
                                    if (ido[i].Substring(ido[i].Length - 1) == "。" || ido[i].Substring(ido[i].Length - 1) == "！" || ido[i].Substring(ido[i].Length - 1) == "!" || ido[i].Substring(ido[i].Length - 1) == "?" || ido[i].Substring(ido[i].Length - 1) == "？" || ido[i].Substring(ido[i].Length - 1) == ".")
                                    {
                                        r += ido[i];
                                    }
                                    else { r += ido[i] + "。"; }
                                }
                                else
                                {
                                    if (ido[i].Substring(ido[i].Length - 1) == "。" || ido[i].Substring(ido[i].Length - 1) == "！" || ido[i].Substring(ido[i].Length - 1) == "!" || ido[i].Substring(ido[i].Length - 1) == "?" || ido[i].Substring(ido[i].Length - 1) == "？" || ido[i].Substring(ido[i].Length - 1) == ".")
                                    {
                                        r += ido[i];
                                    }
                                    else
                                    {
                                        r += ido[i] + "，";
                                    }
                                }
                            }
                            result += tou + r + "</p>";

                        }
                        else if (sp[p].Contains(","))
                        {
                            string tou = sp[p].Remove(sp[p].IndexOf(">") + 1);
                            string cont = sp[p].Replace(tou, "").Replace("</p>", "");
                            string[] ido = RandomSort(cont.Split(','));
                            for (int i = 0; i < ido.Length; i++)
                            {
                                if (i == ido.Length - 1)
                                {
                                    if (ido[i].Substring(ido[i].Length - 1) == "。" || ido[i].Substring(ido[i].Length - 1) == "！" || ido[i].Substring(ido[i].Length - 1) == "!" || ido[i].Substring(ido[i].Length - 1) == "?" || ido[i].Substring(ido[i].Length - 1) == "？" || ido[i].Substring(ido[i].Length - 1) == ".")
                                    {
                                        r += ido[i];
                                    }
                                    else { r += ido[i] + "。"; }
                                }
                                else
                                {
                                    if (ido[i].Substring(ido[i].Length - 1) == "。" || ido[i].Substring(ido[i].Length - 1) == "！" || ido[i].Substring(ido[i].Length - 1) == "!" || ido[i].Substring(ido[i].Length - 1) == "?" || ido[i].Substring(ido[i].Length - 1) == "？" || ido[i].Substring(ido[i].Length - 1) == ".")
                                    {
                                        r += ido[i];
                                    }
                                    else
                                    {
                                        r += ido[i] + ",";
                                    }
                                }
                            }
                            result += tou + r + "</p>";

                        }
                        else
                        {

                            result += sp[p];

                        }
                        #endregion
                    }
                    else
                    {
                       
                        #region 以句号重组
                            if (sp[p].Contains("。"))
                            {

                                string tou = sp[p].Remove(sp[p].IndexOf(">") + 1);
                                string cont = sp[p].Replace(tou, "").Replace("</p>", "");
                                string[] ido = RandomSort(cont.Split('。'));

                                for (int i = 0; i < ido.Length; i++)
                                {
                                   if(ido[i].Trim().Length>0){
                                        string asaa = ido[i].Substring(ido[i].Length - 1);
                                        if (i == ido.Length - 1)
                                        {
                                            if (ido[i].Substring(ido[i].Length - 1) == "。" || ido[i].Substring(ido[i].Length - 1) == "！" || ido[i].Substring(ido[i].Length - 1) == "!" || ido[i].Substring(ido[i].Length - 1) == "?" || ido[i].Substring(ido[i].Length - 1) == "？" || ido[i].Substring(ido[i].Length - 1) == ".")
                                            {
                                                r += ido[i];
                                            }
                                            else { r += ido[i] + "。"; }
                                        }
                                        else
                                        {
                                            if (ido[i].Substring(ido[i].Length - 1) == "。" || ido[i].Substring(ido[i].Length - 1) == "！" || ido[i].Substring(ido[i].Length - 1) == "!" || ido[i].Substring(ido[i].Length - 1) == "?" || ido[i].Substring(ido[i].Length - 1) == "？" || ido[i].Substring(ido[i].Length - 1) == ".")
                                            {
                                                r += ido[i];
                                            }
                                            else
                                            {
                                                r += ido[i] + "。";
                                            }
                                        }
                                    }
                                  
                                }
                                result += tou + r + "</p>";
                               
                              

                            }

                            else
                            {

                                result += sp[p];

                            }
                       
                        #endregion

                    }


                }
            }
            else //以逗号重组
            {


                string r = "";
                if (content.Contains("，"))
                {


                    string[] ido = RandomSort(content.Split('，'));
                    for (int i = 0; i < ido.Length; i++)
                    {
                        string asaa = ido[i].Substring(ido[i].Length - 1);
                        if (i == ido.Length - 1)
                        {
                            if (ido[i].Substring(ido[i].Length - 1) == "。" || ido[i].Substring(ido[i].Length - 1) == "！" || ido[i].Substring(ido[i].Length - 1) == "!")
                            {
                                r += ido[i];
                            }
                            else { r += ido[i] + "。"; }
                        }
                        else
                        {
                            if (ido[i].Substring(ido[i].Length - 1) == "。" || ido[i].Substring(ido[i].Length - 1) == "！" || ido[i].Substring(ido[i].Length - 1) == "!")
                            {
                                r += ido[i];
                            }
                            else
                            {
                                r += ido[i] + "，";
                            }
                        }
                    }
                    result += r;

                }
                else if (content.Contains(","))
                {

                    string[] ido = RandomSort(content.Split(','));
                    for (int i = 0; i < ido.Length; i++)
                    {
                        if (i == ido.Length - 1)
                        {
                            if (ido[i].Substring(ido[i].Length - 1) == "。" || ido[i].Substring(ido[i].Length - 1) == "！" || ido[i].Substring(ido[i].Length - 1) == "!")
                            {
                                r += ido[i];
                            }
                            else { r += ido[i] + "。"; }
                        }
                        else
                        {
                            if (ido[i].Substring(ido[i].Length - 1) == "。" || ido[i].Substring(ido[i].Length - 1) == "！" || ido[i].Substring(ido[i].Length - 1) == "!")
                            {
                                r += ido[i];
                            }
                            else
                            {
                                r += ido[i] + ",";
                            }
                        }
                    }
                    result += r;

                }
                else
                { result = content; }
            }
                   
               
                    

            
            return result;
       
        
        }
        public bool IsChina(string CString) { bool BoolValue = false; for (int i = 0; i < CString.Length; i++) { if (Convert.ToInt32(Convert.ToChar(CString.Substring(i, 1))) > Convert.ToInt32(Convert.ToChar(128))) { BoolValue = true; } } return BoolValue; }
        //自由组合并且随机插入一段
        private string chonggou(string content,string insertstr)
        {
        
            Random rdm = new Random();

            content = content.ToLower();
            Regex re = new Regex("<p[\\s\\S]*?</p>");
            MatchCollection conn = re.Matches(content);
            string[] ps = new string[conn.Count];

            for (int i = 0; i < conn.Count; i++)
            {
                ps[i] = conn[i].Value;
            }
            //有p标签
            string result = "";
            if (ps.Length > 0)
            {
                string[] sp = RandomSort(ps); 
                int insertindex = rdm.Next(0, sp.Length);
                for (int p = 0; p < sp.Length; p++)
                {
                    int rdouorju = rdm.Next(0, 2);
                    string r = "";
                    if (rdouorju == 0)
                    {


                        #region  以逗号重组
                        if (sp[p].Contains("，"))
                        {

                            string tou = sp[p].Remove(sp[p].IndexOf(">") + 1);
                            string cont = sp[p].Replace(tou, "").Replace("</p>", "");
                            string[] ido = RandomSort(cont.Split('，'));
                           
                            for (int i = 0; i < ido.Length; i++)
                            {
                                string asaa = ido[i].Substring(ido[i].Length - 1);
                                if (i == ido.Length - 1)
                                {
                                    if (ido[i].Substring(ido[i].Length - 1) == "。" || ido[i].Substring(ido[i].Length - 1) == "！" || ido[i].Substring(ido[i].Length - 1) == "!" || ido[i].Substring(ido[i].Length - 1) == "?" || ido[i].Substring(ido[i].Length - 1) == "？" || ido[i].Substring(ido[i].Length - 1) == ".")
                                    {
                                        r += ido[i];
                                    }
                                    else { r += ido[i] + "。"; }
                                }
                                else
                                {
                                    if (ido[i].Substring(ido[i].Length - 1) == "。" || ido[i].Substring(ido[i].Length - 1) == "！" || ido[i].Substring(ido[i].Length - 1) == "!" || ido[i].Substring(ido[i].Length - 1) == "?" || ido[i].Substring(ido[i].Length - 1) == "？" || ido[i].Substring(ido[i].Length - 1) == ".")
                                    {
                                        r += ido[i];
                                    }
                                    else
                                    {
                                        r += ido[i] + "，";
                                    }
                                }
                               
                            }
                            result += tou + r + "</p>";

                        }
                        else if (sp[p].Contains(","))
                        {
                            string tou = sp[p].Remove(sp[p].IndexOf(">") + 1);
                            string cont = sp[p].Replace(tou, "").Replace("</p>", "");
                            string[] ido = RandomSort(cont.Split(','));
                           
                            for (int i = 0; i < ido.Length; i++)
                            {
                                if (i == ido.Length - 1)
                                {
                                    if (ido[i].Substring(ido[i].Length - 1) == "。" || ido[i].Substring(ido[i].Length - 1) == "！" || ido[i].Substring(ido[i].Length - 1) == "!" || ido[i].Substring(ido[i].Length - 1) == "?" || ido[i].Substring(ido[i].Length - 1) == "？" || ido[i].Substring(ido[i].Length - 1) == ".")
                                    {
                                        r += ido[i];
                                    }
                                    else { r += ido[i] + "。"; }
                                }
                                else
                                {
                                    if (ido[i].Substring(ido[i].Length - 1) == "。" || ido[i].Substring(ido[i].Length - 1) == "！" || ido[i].Substring(ido[i].Length - 1) == "!" || ido[i].Substring(ido[i].Length - 1) == "?" || ido[i].Substring(ido[i].Length - 1) == "？" || ido[i].Substring(ido[i].Length - 1) == ".")
                                    {
                                        r += ido[i];
                                    }
                                    else
                                    {
                                        r += ido[i] + ",";
                                    }
                                }
                               
                            }
                            result += tou + r + "</p>";

                        }
                        else
                        {

                            result += sp[p];

                        }
                        #endregion
                    }
                    else
                    {

                        #region 以句号重组
                        if (sp[p].Contains("。"))
                        {

                            string tou = sp[p].Remove(sp[p].IndexOf(">") + 1);
                            string cont = sp[p].Replace(tou, "").Replace("</p>", "");
                            string[] ido = RandomSort(cont.Split('。'));
                           
                            for (int i = 0; i < ido.Length; i++)
                            {
                                if (ido[i].Trim().Length > 0)
                                {
                                    string asaa = ido[i].Substring(ido[i].Length - 1);
                                    if (i == ido.Length - 1)
                                    {
                                        if (ido[i].Substring(ido[i].Length - 1) == "。" || ido[i].Substring(ido[i].Length - 1) == "！" || ido[i].Substring(ido[i].Length - 1) == "!" || ido[i].Substring(ido[i].Length - 1) == "?" || ido[i].Substring(ido[i].Length - 1) == "？" || ido[i].Substring(ido[i].Length - 1) == ".")
                                        {
                                            r += ido[i];
                                        }
                                        else { r += ido[i] + "。"; }
                                    }
                                    else
                                    {
                                        if (ido[i].Substring(ido[i].Length - 1) == "。" || ido[i].Substring(ido[i].Length - 1) == "！" || ido[i].Substring(ido[i].Length - 1) == "!" || ido[i].Substring(ido[i].Length - 1) == "?" || ido[i].Substring(ido[i].Length - 1) == "？" || ido[i].Substring(ido[i].Length - 1) == ".")
                                        {
                                            r += ido[i];
                                        }
                                        else
                                        {
                                            r += ido[i] + "。";
                                        }
                                    }
                                    
                                }

                            }
                            result += tou + r + "</p>";



                        }

                        else
                        {

                            result += sp[p];

                        }

                        #endregion

                    }
                    if(p==insertindex)
                    {
                        result += insertstr;
                    }

                }
            }
            else //以逗号重组
            {


                string r = "";
                if (content.Contains("，"))
                {


                    string[] ido = RandomSort(content.Split('，'));
                    for (int i = 0; i < ido.Length; i++)
                    {
                        string asaa = ido[i].Substring(ido[i].Length - 1);
                        if (i == ido.Length - 1)
                        {
                            if (ido[i].Substring(ido[i].Length - 1) == "。" || ido[i].Substring(ido[i].Length - 1) == "！" || ido[i].Substring(ido[i].Length - 1) == "!")
                            {
                                r += ido[i];
                            }
                            else { r += ido[i] + "。"; }
                        }
                        else
                        {
                            if (ido[i].Substring(ido[i].Length - 1) == "。" || ido[i].Substring(ido[i].Length - 1) == "！" || ido[i].Substring(ido[i].Length - 1) == "!")
                            {
                                r += ido[i];
                            }
                            else
                            {
                                r += ido[i] + "，";
                            }
                        }
                    }
                    result += r;

                }
                else if (content.Contains(","))
                {

                    string[] ido = RandomSort(content.Split(','));
                    for (int i = 0; i < ido.Length; i++)
                    {
                        if (i == ido.Length - 1)
                        {
                            if (ido[i].Substring(ido[i].Length - 1) == "。" || ido[i].Substring(ido[i].Length - 1) == "！" || ido[i].Substring(ido[i].Length - 1) == "!")
                            {
                                r += ido[i];
                            }
                            else { r += ido[i] + "。"; }
                        }
                        else
                        {
                            if (ido[i].Substring(ido[i].Length - 1) == "。" || ido[i].Substring(ido[i].Length - 1) == "！" || ido[i].Substring(ido[i].Length - 1) == "!")
                            {
                                r += ido[i];
                            }
                            else
                            {
                                r += ido[i] + ",";
                            }
                        }
                    }
                    result += r;

                }
                else
                { result = content; }
            }





            return result;


        }
        public static T[] RandomSort<T>(T[] array)
        {
            int len = array.Length;
            System.Collections.Generic.List<int> list = new System.Collections.Generic.List<int>();
            T[] ret = new T[len];
            Random rand = new Random();
            int i = 0;
            while (list.Count < len)
            {
                int iter = rand.Next(0, len);
                if (!list.Contains(iter))
                {
                    list.Add(iter);
                    ret[i] = array[iter];
                    i++;
                }

            }
            return ret;
        }
        #endregion


     
    }
}
