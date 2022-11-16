using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Collections;
using System.Net;
using System.IO;
using System.Data;
using System.Windows.Forms;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Search;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Threading.Tasks;
using V3.Bll;
using xEngine.Model.Execute.Http;
using HtmlDocument = xEngine.Plugin.HtmlParsing.HtmlDocument;

namespace V3.Common
{
    public static class TaskRun
    {
        //运行任务
        public static void Run(int taskid)
        {
            try
            {
                Random r = new Random();
                //判断任务是否已经是运行中
                if ((Model.V3Infos.TaskThread.ContainsKey(taskid) && Model.V3Infos.TaskCancelToken.ContainsKey(taskid)) ||
                    Model.V3Infos.TaskThread.ContainsKey(taskid) || Model.V3Infos.TaskWaiting.ContainsKey(taskid))
                {
                    V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：正在运行或者休眠中，请停止后再启动！[/c]");
                    return;
                }

                //如果不是，则创建线程对象
                //if (V3Model.V3Infos.TaskThread.ContainsKey(taskid))
                //{


                //    V3Start.Command.Log.LogNewline("任务【" + taskid + "】：正在重新初始化线程,开始启动任务！", 1);
                //}
                //else
                //{
                CancellationTokenSource cancelToken = new CancellationTokenSource();
                Task t = null;
                if (Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].PlanModel == 3 ||
                    Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].PlanModel == 30)
                {
                    t = new Task(RunThread, taskid, cancelToken.Token, TaskCreationOptions.LongRunning);
                }
                else
                {
                    t = new Task(RunThread, taskid, cancelToken.Token, TaskCreationOptions.LongRunning);
                }
                Model.V3Infos.TaskCancelToken.Add(taskid, cancelToken);
                Model.V3Infos.TaskThread.Add(taskid, t);
                Model.V3Infos.TaskThread[taskid].Start();
                Model.V3Infos.TaskDb[taskid].Bencifabu = 0;
                Model.V3Infos.TaskDb[taskid].Suijirandom = r.Next(Model.V3Infos.TaskDb[taskid].Suijimin,
                    Model.V3Infos.TaskDb[taskid].Suijimax);
                Model.V3Infos.TaskDb[taskid].TaskStatusId = 0;
                Model.V3Infos.TaskDb[taskid].TaskStatusStr = "等待其他任务执行完毕后开始...";
                V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：正在初始化线程，开始启动任务！[/c]");
                //}
            }
            catch (Exception error)
            {
                V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：启动失败，请检查模块以及参数设置，"+error.Message+"[/c]");
                Stop(taskid);
            }
        }
        //停止任务
        public static void Stop(int taskid)
        {

            //判断任务是否已经是运行中
            if (Model.V3Infos.TaskThread.ContainsKey(taskid) && Model.V3Infos.TaskCancelToken.ContainsKey(taskid))
            {
                if (Model.V3Infos.TaskDb[taskid].TaskStatusId==3)
                {
                    Model.V3Infos.TaskCancelToken[taskid].Cancel();
                   // V3Model.V3Infos.TaskCancelToken.Remove(taskid);
                   //V3Model.V3Infos.TaskThread[taskid].Dispose();
                   // V3Model.V3Infos.TaskThread.Remove(taskid);
                   // V3Start.Command.Log.LogNewline("任务【" + taskid + "】：已经成功停止任务！", 1);
                   // V3Model.V3Infos.TaskDb[taskid].TaskStatusId= 4;
                   // V3Model.V3Infos.TaskDb[taskid].TaskStatusStr= "任务于" + DateTime.Now.ToString() + "停止！";
                }
                else
                {
                    Model.V3Infos.TaskCancelToken[taskid].Cancel();

                    //V3Model.V3Infos.TaskThread[taskid].Abort();
                    //V3Model.V3Infos.TaskDb[taskid].TaskStatusId = 4;
                    Model.V3Infos.TaskDb[taskid].TaskStatusStr = "任务正在尝试停止...";
                    //V3Start.Command.TaskBll.SaveTask(taskid);
                    //V3Start.Command.Log.LogNewline("任务【" + taskid + "】：已经成功停止任务！", 1);
                    //V3Model.V3Infos.TaskThread.Remove(taskid);
                }
                
            }else if (Model.V3Infos.TaskWaiting.ContainsKey(taskid))
            {
                Model.V3Infos.TaskDb[taskid].TaskStatusId = 4;
                Model.V3Infos.TaskWaiting.Remove(taskid);
                V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：已经成功停止任务！[/c]");
                Model.V3Infos.TaskDb[taskid].TaskStatusId = 4;
                Model.V3Infos.TaskDb[taskid].TaskStatusStr = "任务于" + DateTime.Now.ToString() + "停止！";
            }
        }
        //任务执行前检查
        static void CheckTask(int taskid)
        {
            // throw new Exception("接收数据出错啦" + serverid);
        }
        //主线程
        static void RunThread(object tid)
        {
            bool waiting = false;
            bool tongbu = false;
            int taskid = Convert.ToInt32(tid);
#if Final
            try
            {
#endif
            while (true)
            {
                if (Model.V3Infos.TaskCancelToken[taskid].IsCancellationRequested)
                {
                    break;
                }

                #region 任务执行前检测
                Thread.Sleep(100);
                V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：任务激活，正在进行任务初始化...[/c]");
                if (Model.V3Infos.TaskDb[taskid].Links == null) { Model.V3Infos.TaskDb[taskid].Links = new List<string>(); }
                Model.V3Infos.TaskDb[taskid].Links.Clear();
                #endregion

                #region 抓取部分
                if (Model.V3Infos.TaskDb[taskid].Isrunget)
                {
                    Thread.Sleep(100);
                    V3.Common.Log.LogNewline("[c12]任务【" + taskid + "】：任务进入抓取过程，使用的抓取模块id是" + Model.V3Infos.TaskDb[taskid].GetModel + "[/c]");
                    Model.V3Infos.TaskDb[taskid].TaskStatusId = 1;
                    Model.V3Infos.TaskDb[taskid].TaskStatusStr = "任务进入抓取过程，使用的抓取模块id是" + Model.V3Infos.TaskDb[taskid].GetModel;
                    if (Model.V3Infos.TaskDb[taskid].IsUseKu)
                    {
                        yuLiaokeywordGet(taskid);
                    }
                    else
                    {
                        if (Model.V3Infos.MainDb.MyModels.ContainsKey(Model.V3Infos.TaskDb[taskid].GetModel))
                        {
                            if (Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].PlanModel == 1 || Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].PlanModel == 10)
                            {
                                KeywordGet(taskid);
                            }
                            else if (Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].PlanModel == 2 || Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].PlanModel == 20)
                            {
                                ZidingyiGet(taskid);
                            }
                            else if (Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].PlanModel == 3 || Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].PlanModel == 30)
                            {
                                SpiderGet(taskid);
                            }
                            else if (Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].PlanModel == 4 ||
                                     Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].PlanModel == 40)
                            {
                                if (Model.V3Infos.TaskDb[taskid].Isrunsend == false)
                                {
                                    V3.Common.Log.LogNewline(
                                        "[c14]任务【" + taskid + "】：请把任务设置为发布模式，否则无法使用同步追踪！[/c]");
                                    Model.V3Infos.TaskCancelToken[taskid].Cancel();
                                   
                                }
                                else
                                {
                                 tongbu = true;
                                 TongbuGet(taskid);
                                }
                                
                                
                                break;
                            }
                            
                        }
                        else
                        {
                            Thread.Sleep(50);
                            throw new Exception("任务" + taskid + "所使用的抓取模块" + Model.V3Infos.TaskDb[taskid].GetModel + "不存在或已删除，请重新设置！");
                        }
                    }
                }
                else
                {
                    V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：本任务设置为不进行抓取过程！[/c]");
                }
                #endregion

                if (!tongbu)
                {
                    #region 发布部分
                    if (Model.V3Infos.TaskDb[taskid].Isrunsend)
                    {
                        Thread.Sleep(100);
                        V3.Common.Log.LogNewline("[c12]任务【" + taskid + "】：任务进入发布过程，使用的发布模块id是" + Model.V3Infos.SendPointDb[Model.V3Infos.TaskDb[taskid].PointId].PostModel + "[/c]");
                        Model.V3Infos.TaskDb[taskid].TaskStatusId = 2;


                        Model.V3Infos.TaskDb[taskid].TaskStatusStr = "任务进入发布过程，使用的发布模块id是" + Model.V3Infos.SendPointDb[Model.V3Infos.TaskDb[taskid].PointId].PostModel;
                        if (Model.V3Infos.MainDb.MyModels.ContainsKey(Model.V3Infos.SendPointDb[Model.V3Infos.TaskDb[taskid].PointId].PostModel))
                        {
                            send(taskid);
                        }
                        else
                        {
                            Thread.Sleep(100);

                            throw new Exception("任务" + taskid + "所使用的发布模块" + Model.V3Infos.SendPointDb[Model.V3Infos.TaskDb[taskid].PointId].PostModel + "不存在或已删除，请重新设置！");

                        }
                    }
                    else
                    {
                        V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：本任务设置为不进行发布过程！[/c]");
                    }
                    #endregion

                    #region 任务末尾处理
                    if (Model.V3Infos.TaskDb[taskid].IsUseLinkDb)
                       V3.Bll.DbBll.SaveDb(5, Model.V3Infos.TaskDb[taskid].LinkDbId.ToString());
                    if (Model.V3Infos.TaskDb[taskid].IsAutoTask && !Model.V3Infos.TaskCancelToken[taskid].IsCancellationRequested)
                    {
                        Random r = new Random();
                        Thread.Sleep(100);
                        int sleeptime = r.Next(Convert.ToInt32(Model.V3Infos.TaskDb[taskid].Jiangetime * 0.7), Convert.ToInt32(Model.V3Infos.TaskDb[taskid].Jiangetime * 1.3));
                        Model.V3Infos.TaskDb[taskid].TaskStatusId = 3;
                        Model.V3Infos.TaskDb[taskid].TaskStatusStr = "任务休眠中，休眠时间" + sleeptime + "分钟,将于" + DateTime.Now.AddMinutes(sleeptime).ToString() + "继续运行！";
                        V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：任务休眠中，休眠时间" + sleeptime + "分钟,将于" + DateTime.Now.AddMinutes(sleeptime).ToString() + "继续运行！[/c]");
                        V3.Bll.TaskBll.SaveTask(taskid);
                        try
                        {
                            Model.V3Infos.TaskWaiting.Add(taskid, DateTime.Now.AddMinutes(sleeptime));
                            waiting = true;
                            break;
                        }
                        catch
                        {

                        }

                        //if (Model.V3Infos.TaskDb[taskid].TaskMoshi == 0)
                        //{

                        //    Random r = new Random();
                        //    Thread.Sleep(100);
                        //    int sleeptime = r.Next(Convert.ToInt32(Model.V3Infos.TaskDb[taskid].Jiangetime * 0.7), Convert.ToInt32(Model.V3Infos.TaskDb[taskid].Jiangetime * 1.3));
                        //    Model.V3Infos.TaskDb[taskid].TaskStatusId = 3;
                        //    Model.V3Infos.TaskDb[taskid].TaskStatusStr = "任务休眠中，休眠时间" + sleeptime + "分钟,将于" + DateTime.Now.AddMinutes(sleeptime).ToString() + "继续运行！";
                        //    V3.Common.Log.LogNewline("任务【" + taskid + "】：任务休眠中，休眠时间" + sleeptime + "分钟,将于color1" + DateTime.Now.AddMinutes(sleeptime).ToString() + "/color1继续运行！", 1);
                        //    V3.Bll.TaskBll.SaveTask(taskid);
                        //    try
                        //    {
                        //        //V3Model.V3Infos.TaskThread[taskid].Wait(sleeptime*60*1000,V3Model.V3Infos.TaskCancelToken[taskid].Token);
                        //        Model.V3Infos.TaskWaiting.Add(taskid, DateTime.Now.AddSeconds(sleeptime));
                        //        waiting = true;
                        //        break;
                        //    }
                        //    catch
                        //    {

                        //    }
                        //}
                        //else if (Model.V3Infos.TaskDb[taskid].TaskMoshi == 1)
                        //{
                        //    Random r = new Random();
                        //    Thread.Sleep(100);
                        //    int sleeptime = Model.V3Infos.TaskDb[taskid].Jiangetime * 60 / Model.V3Infos.TaskDb[taskid].Picinumber;
                        //    Model.V3Infos.TaskDb[taskid].TaskStatusId = 3;
                        //    Model.V3Infos.TaskDb[taskid].TaskStatusStr = "任务休眠中，休眠时间" + sleeptime + "秒,将于" + DateTime.Now.AddSeconds(sleeptime).ToString() + "继续运行！";
                        //    V3.Common.Log.LogNewline("任务【" + taskid + "】：任务休眠中，休眠时间" + sleeptime + "秒,将于color1" + DateTime.Now.AddSeconds(sleeptime).ToString() + "/color1继续运行！", 1);
                        //    V3.Bll.TaskBll.SaveTask(taskid);
                        //    try
                        //    {
                        //        //V3Model.V3Infos.TaskThread[taskid].Wait(sleeptime*1000,V3Model.V3Infos.TaskCancelToken[taskid].Token);
                        //        Model.V3Infos.TaskWaiting.Add(taskid, DateTime.Now.AddSeconds(sleeptime));
                        //        waiting = true;
                        //        break;
                        //    }
                        //    catch
                        //    {

                        //    }
                        //}
                        //else if (Model.V3Infos.TaskDb[taskid].TaskMoshi == 2)
                        //{
                        //    Random r = new Random();
                        //    Thread.Sleep(100);
                        //    int sleeptime = r.Next(Convert.ToInt32(Model.V3Infos.TaskDb[taskid].Jiangetime * 60 / Model.V3Infos.TaskDb[taskid].Picinumber * 0.7), Convert.ToInt32(Model.V3Infos.TaskDb[taskid].Jiangetime * 60 / Model.V3Infos.TaskDb[taskid].Picinumber * 1.3));
                        //    Model.V3Infos.TaskDb[taskid].TaskStatusId = 3;
                        //    Model.V3Infos.TaskDb[taskid].TaskStatusStr = "任务休眠中，休眠时间" + sleeptime + "秒,将于" + DateTime.Now.AddSeconds(sleeptime).ToString() + "继续运行！";
                        //    V3.Common.Log.LogNewline("任务【" + taskid + "】：任务休眠中，休眠时间" + sleeptime + "秒,将于color1" + DateTime.Now.AddSeconds(sleeptime).ToString() + "/color1继续运行！", 1);
                        //    V3.Bll.TaskBll.SaveTask(taskid);
                        //    try
                        //    {
                        //        // V3Model.V3Infos.TaskThread[taskid].Wait(sleeptime*1000,V3Model.V3Infos.TaskCancelToken[taskid].Token);
                        //        Model.V3Infos.TaskWaiting.Add(taskid, DateTime.Now.AddSeconds(sleeptime));
                        //        waiting = true;
                        //        break;
                        //    }
                        //    catch
                        //    {

                        //    }
                        //}
                    }
                    else
                        break;
                    #endregion
                }

               
            }

            if (Model.V3Infos.TaskDb[taskid].ClearHashDb) 
            {
                V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：开始清理哈希库！[/c]");
                try
                {
                    
                    System.IO.Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\HashDb\\" + Model.V3Infos.TaskDb[taskid].HashDbId + "_C\\", true);
                    System.IO.Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\HashDb\\" + Model.V3Infos.TaskDb[taskid].HashDbId + "_T\\", true);
                    System.IO.Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\HashDb\\" + Model.V3Infos.TaskDb[taskid].HashDbId + "_U\\", true);
                }
                catch { }
                V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：哈希库清理完毕！[/c]");
            }

            if (waiting || (tongbu &&!Model.V3Infos.TaskCancelToken[taskid].IsCancellationRequested))
            {
                //加入到休眠列队
                Model.V3Infos.TaskCancelToken.Remove(taskid);
                Model.V3Infos.TaskThread.Remove(taskid);
                Model.V3Infos.TaskDb[taskid].TaskStatusId =3;
                V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：加入休眠列队！[/c]");
            }
            else if (Model.V3Infos.TaskCancelToken[taskid].IsCancellationRequested)
            {
                Model.V3Infos.TaskCancelToken.Remove(taskid);
                Model.V3Infos.TaskThread.Remove(taskid);
                V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：已经成功停止！[/c]");
                Model.V3Infos.TaskDb[taskid].TaskStatusId = 4;
                Model.V3Infos.TaskDb[taskid].TaskStatusStr = "任务于" + DateTime.Now.ToString() + "停止！";
            }
            else
            {
                Model.V3Infos.TaskCancelToken.Remove(taskid);
                Model.V3Infos.TaskThread.Remove(taskid);
                V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：已运行结束！[/c]");
                Model.V3Infos.TaskDb[taskid].TaskStatusId = 4;
                Model.V3Infos.TaskDb[taskid].TaskStatusStr = "任务于" + DateTime.Now.ToString() + "结束！";
            }
#if Final
            }
            catch (Exception ex)
            {
                if (Model.V3Infos.TaskDb[taskid].IsUseLinkDb)
                    DbBll.SaveDb(5, Model.V3Infos.TaskDb[taskid].LinkDbId.ToString());
                if (!ex.Message.Contains("线程") &&!ex.Message.Contains("Thread"))
                {
                    Model.V3Infos.TaskDb[taskid].TaskStatusId = 4;
                    Model.V3Infos.TaskDb[taskid].TaskStatusStr = "任务异常停止，原因：" + ex.Message + " 代码行:+" + ex.ToString();
                    Common.Log.LogNewline("[c14]任务【" + taskid + "】：任务异常停止，原因：" + ex.Message + " 代码行:+" + ex.ToString() + "[/c]");
                    Stop(taskid);
                }
                if (Model.V3Infos.TaskDb.ContainsKey(taskid))
                {
                    Bll.PointBll.Update(Model.V3Infos.SendPointDb[Model.V3Infos.TaskDb[taskid].PointId]);
                    Bll.TaskBll.SaveTask(taskid);
                }
            }
#endif
        }

        #region 抓取过程方法

        #region 将文章保存到文章库,判断精度
        static bool AddArtsToDbs(int taskid, Model.Model_Article art)
        {

            Regex r = new Regex("<.*?>");
                if (art == null)
                {
                    V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：跳过一条数据，因为该文章对象为空！" + Model.V3Infos.TaskDb[taskid].MinTitlestr + "！");
                    return false;
                }
                if (Model.V3Infos.TaskDb[taskid].Qianchuorhouchu == true)
                {
                    if (MarkNewArticle(ref art, taskid).Contains("含有敏感字符")) 
                    {
                        V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：跳过一条数据，因为包含敏感字符，当前设置为跳过！" + Model.V3Infos.TaskDb[taskid].MinTitlestr + "！[/c]");
                        return false;
                    }
                }

                bool result = false;
                if (r.Replace(art.DataObject[0],"").Length < Model.V3Infos.TaskDb[taskid].MinTitlestr)
                {
                    V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：跳过一条数据，因为该数据的标题小于任务要求的长度" + Model.V3Infos.TaskDb[taskid].MinTitlestr + "！[/c]");
                    return false;
                }
                if (r.Replace(art.DataObject[1],"").Length < Model.V3Infos.TaskDb[taskid].MinContentstr)
                {
                    V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：跳过一条数据，因为该数据的内容小于任务要求的长度" + Model.V3Infos.TaskDb[taskid].MinContentstr + "！[/c]");
                    return false;
                }
                for (int i = 0; i < 30; i++)
                {
                    if (art.DataObject[i] == null)
                        art.DataObject[i] = "";
                }
                if (Model.V3Infos.TaskDb[taskid].HashModel == 1)
                {
                    art.Date = DateTime.Now.ToString();
                    string hash = Library.StrHelper.Md5(art.DataObject[0]);
                    if (!CheckTitleHash(taskid, hash))
                    {
                        AddTitleHash(taskid, hash);
                        if (ArticleBll.addnewdata(Model.V3Infos.ArticleDb[Model.V3Infos.TaskDb[taskid].ArticleDbId.ToString()], art))
                        {
                            result = true;
                            Model.V3Infos.TaskDb[taskid].CountAllGet++;
                            Model.V3Infos.TaskDb[taskid].CountThisGet++;
                            
                        }
                    }
                    else
                    {
                        V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：跳过一条数据，因为文章库已经包含了名为“" + Substring(art.DataObject[0], 15) + "”的数据！[/c]");
                    }
                }
                else if (Model.V3Infos.TaskDb[taskid].HashModel == 2)
                {
                    art.Date = DateTime.Now.ToString();
                    string hash = Library.StrHelper.Md5(art.DataObject[1]);
                    if (!CheckContentHash(taskid, hash))
                    {
                        AddContentHash(taskid, hash);
                        if (ArticleBll.addnewdata(Model.V3Infos.ArticleDb[Model.V3Infos.TaskDb[taskid].ArticleDbId.ToString()], art))
                        {
                            result = true;
                            Model.V3Infos.TaskDb[taskid].CountAllGet++;
                            Model.V3Infos.TaskDb[taskid].CountThisGet++;
                          
                        }
                    }
                    else
                    {
                        V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：跳过一条数据，因为文章库已经包含了名为“" + Substring(art.DataObject[0], 15) + "”的数据！[/c]");
                    }
                }
                else
                {
                    art.Date = DateTime.Now.ToString();
                    string hash = Library.StrHelper.Md5(art.DataObject[28]);
                    if (!CheckUrlHash(taskid, hash))
                    {
                        if (ArticleBll.addnewdata(Model.V3Infos.ArticleDb[Model.V3Infos.TaskDb[taskid].ArticleDbId.ToString()], art))
                        {
                            result = true;
                            Model.V3Infos.TaskDb[taskid].CountAllGet++;
                            Model.V3Infos.TaskDb[taskid].CountThisGet++;
                            
                         
                        }
                    }
                    else
                    {
                        V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：跳过一条数据，因为文章库已经包含了名为“" + Substring(art.DataObject[0], 15) + "”的数据！[/c]");
                    }
                }
                return result;

        }
        //提取到文章
        static Model.Model_Article GetArtModel(int taskid, ArrayList FinalLinks, int ii, Model.Model_Article art, string FinalHtml)
        {
            if (Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Stp3_GET_GetModel == 1)
            {
                for (int i = 0; i < 30; i++)
                {
                   
                    if (Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Stp3_GET_Rules[i] != null)
                    {
                        V3.Bll.GetBll get = new Bll.GetBll(FinalHtml, Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Stp3_GET_Rules[i].isGetPublicRules ? Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Stp3_GET_PublicRules : Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Stp3_GET_Rules[i].Rules);
                        if (FinalLinks[ii].ToString().StartsWith("http"))
                            get.oldurl = FinalLinks[ii].ToString();
                        System.Collections.ArrayList list = new System.Collections.ArrayList();
                     
                        list = get.getAllRules(taskid);
                      
                        if (list.Count > 0)
                        {
                            try
                            {
                                if (
                                    list[Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Stp3_GET_Rules[i].selectedValue] == null)
                                    art.DataObject[i] = "";
                                else
                                    art.DataObject[i] =list[ Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Stp3_GET_Rules[i].selectedValue].ToString().Trim();
                            }
                            catch
                            {
                                art.DataObject[i] = "";
                            }
                        }
                        else
                        {
                            art.DataObject[i] = "";
                        }
                        
                    }
                    else
                    {
                        art.DataObject[i] = "";
                    }
                    if (i == 1 && Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Stp3_neemorepage)
                    {
                        Queue que = new Queue();
                        Dictionary<string, int> chk = new Dictionary<string, int>();
                        chk.Add(FinalLinks[ii].ToString(), 0);
                        string url = "";
                        string html = "";
                        V3.Bll.GetBll get = new GetBll(FinalHtml, Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Stp3_pagerule);
                        if (FinalLinks[ii].ToString().StartsWith("http"))
                            get.oldurl = FinalLinks[ii].ToString();
                        System.Collections.ArrayList list = new System.Collections.ArrayList();
                        list = get.getAllRules(taskid);
                        if (list.Count > 0)
                        {
                            for (int iii = 0; iii < list.Count; iii++)
                            {
                                if (!chk.ContainsKey(list[iii].ToString()))
                                {
                                    chk.Add(list[iii].ToString(), 0);
                                    que.Enqueue(list[iii].ToString());
                                }
                            }
                        }
                        Log.LogNewline("[c12]获取到"+que.Count+"个内容分页链接[/c]");
                        while (que.Count > 0)
                        {
                            Log.LogNewline("[c12]剩余"+que.Count+"个内容分页链接未抓取[/c]");
                            url = que.Dequeue().ToString();


                            xEngine.Execute.Http execute = new xEngine.Execute.Http();
                            execute.LoadScript(Properties.Resources.get,false);
                            execute.CookieManager = GetCookie(taskid);
                            execute.Scripts[0].Url = url;
                            execute.Scripts[0].Referer = "";
                            execute.Scripts[0].Encoding = Encoding.Default.WebName;
                            execute.IsAutoEncoding = true;
                            execute.Scripts[0].UserAgent=  Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].UserAgent == "" ? "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; InfoPath.2)" : Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].UserAgent;
                            XResponse response = execute.RunRequest(execute.Scripts[0]);
                            html = "源地址：" + execute.Scripts[0].Url + "\r\n\r\n" + Library.HtmlHelper.GetHtmlFromByte(response.BodyData, response.ContentType);
                             get = new GetBll(html, Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Stp3_pagerule);
                            if (FinalLinks[ii].ToString().StartsWith("http"))
                                get.oldurl = FinalLinks[ii].ToString();
                            list = new System.Collections.ArrayList();
                            list = get.getAllRules(taskid);
                            if (list.Count > 0)
                            {
                                for (int iii = 0; iii < list.Count; iii++)
                                {
                                    if (!chk.ContainsKey(list[iii].ToString()))
                                    {
                                        chk.Add(list[iii].ToString(), 0);
                                        que.Enqueue(list[iii].ToString());
                                    }
                                }
                            }
                            try
                            {
                                get = new Bll.GetBll(html, Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Stp3_GET_Rules[i].isGetPublicRules ? Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Stp3_GET_PublicRules : Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Stp3_GET_Rules[i].Rules);
                            }
                            catch { Log.LogNewline("[c14]本模块内容提取规则有误！[/c]"); break; }
                            if (FinalLinks[ii].ToString().StartsWith("http"))
                                get.oldurl = FinalLinks[ii].ToString();
                            list = new System.Collections.ArrayList();
                        
                            list = get.getAllRules(taskid);
                            if (list.Count > 0 && list[Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Stp3_GET_Rules[i].selectedValue] != null)
                            {
                                art.DataObject[i] += list[Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Stp3_GET_Rules[i].selectedValue].ToString().Trim();
                            }

                        }
                    }
                }
            }
            else
            {
                Library.HtmlHelper.Article article = Library.HtmlHelper.HtmlToArticle.GetArticle(FinalHtml);
                if (article != null)
                {
                    art.DataObject[0] = article.Title.Trim();
                    art.DataObject[1] = article.ContentWithTags.Trim();
                }
                else
                {
                    HtmlDocument htmldoc = new HtmlDocument();
                    htmldoc.LoadHtml(FinalHtml);
                    string title = V3.Common.AiGet.getTitle(htmldoc.DocumentNode);
                    string content = V3.Common.AiGet.GetMainContent(htmldoc.DocumentNode, taskid);
                    art.DataObject[0] = title.Trim();
                    art.DataObject[1] = content.Trim();
                }
            }


            return art;


        }
        //精度判断
        /// <summary>
        /// 是否满足精度
        /// </summary>
        /// <param name="taskid">任务id</param>
        /// <param name="art">文章模型</param>
        /// <returns></returns>
        static bool jingdu(int taskid, Model.Model_Article art)
        {
            if (art == null) { Log.LogNewline("[c14]文章为空，无法确定精度！[/c]"); return false; }
            if (Model.V3Infos.TaskDb[taskid].Get1_jingduModel == 0)//不启用
            {
                return true;
            }
            else if (Model.V3Infos.TaskDb[taskid].Get1_jingduModel == 1)//模式2：主词必须出现在标题中
            {
                if (art.DataObject[29] != null)
                {
                    if (art.DataObject[0].Contains(art.DataObject[29])) { return true; }
                    else { return false; }
                }
                else //没有主词
                {
                    Log.LogNewline("[c11]当前抓取模式下没有主关键词，无法确定精度，自动保存数据！[/c]");
                    return true;
                }

            }
            else if (Model.V3Infos.TaskDb[taskid].Get1_jingduModel == 2)//模式3：主词必须出现在内容中
            {

                if (art.DataObject[29] != null)
                {
                    if (art.DataObject[1].Contains(art.DataObject[29])) { return true; }
                    else { return false; }
                }
                else //没有主词
                {
                    Log.LogNewline("[c11]当前抓取模式下没有主关键词，无法确定精度，自动保存数据！[/c]");
                    return true;
                }
            }
            else if (Model.V3Infos.TaskDb[taskid].Get1_jingduModel == 3)//模式4：主词必须出现在标题或内容中
            {
                if (art.DataObject[29] != null)
                {
                    if (art.DataObject[1].Contains(art.DataObject[29]) || art.DataObject[0].Contains(art.DataObject[29])) { return true; }
                    else { return false; }
                }
                else //没有主词
                {
                    Log.LogNewline("[c11]当前抓取模式下没有主关键词，无法确定精度，自动保存数据！[/c]");
                    return true;
                }


            }
            else if (Model.V3Infos.TaskDb[taskid].Get1_jingduModel == 4)//模式5：主词必须出现在标题和内容中
            {
                if (art.DataObject[29] != null)
                {
                    if (art.DataObject[1].Contains(art.DataObject[29]) && art.DataObject[0].Contains(art.DataObject[29])) { return true; }
                    else { return false; }
                }
                else //没有主词
                {
                    Log.LogNewline("[c11]当前抓取模式下没有主关键词，无法确定精度，自动保存数据！[/c]");
                    return true;
                }

            }
            else if (Model.V3Infos.TaskDb[taskid].Get1_jingduModel == 5)//模式6：种子词必须出现在标题中
            {
                if (Model.V3Infos.TaskDb[taskid].Get1_jingdukeyword.Length == 0)
                {
                    Log.LogNewline("[c14]当前任务没有种子词，无法确定精度，不保存数据！[/c]");
                    return false;
                }
                else
                {

                    for (int i = 0; i < Model.V3Infos.TaskDb[taskid].Get1_jingdukeyword.Length; i++)
                    {
                        if (art.DataObject[0].Contains(Model.V3Infos.TaskDb[taskid].Get1_jingdukeyword[i]))
                        {
                            return true;
                        }
                    }

                    return false;
                }


            }
            else if (Model.V3Infos.TaskDb[taskid].Get1_jingduModel == 6)//模式7：种子词必须出现在内容中
            {
                if (Model.V3Infos.TaskDb[taskid].Get1_jingdukeyword.Length == 0)
                {
                    Log.LogNewline("[c14]当前任务没有种子词，无法确定精度，不保存数据！[/c]");
                    return false;
                }
                else
                {

                    for (int i = 0; i < Model.V3Infos.TaskDb[taskid].Get1_jingdukeyword.Length; i++)
                    {
                        if (art.DataObject[1].Contains(Model.V3Infos.TaskDb[taskid].Get1_jingdukeyword[i]))
                        {
                            return true;
                        }
                    }

                    return false;
                }
            }
            else if (Model.V3Infos.TaskDb[taskid].Get1_jingduModel == 7)//模式8：种子词必须出现在标题或内容中
            {

                if (Model.V3Infos.TaskDb[taskid].Get1_jingdukeyword.Length == 0)
                {
                    Log.LogNewline("[c14]当前任务没有种子词，无法确定精度，不保存数据！[/c]");
                    return false;
                }
                else
                {

                    for (int i = 0; i < Model.V3Infos.TaskDb[taskid].Get1_jingdukeyword.Length; i++)
                    {
                        if (art.DataObject[0].Contains(Model.V3Infos.TaskDb[taskid].Get1_jingdukeyword[i]) || art.DataObject[1].Contains(Model.V3Infos.TaskDb[taskid].Get1_jingdukeyword[i]))
                        {
                            return true;
                        }
                    }

                    return false;
                }
            }
            else if (Model.V3Infos.TaskDb[taskid].Get1_jingduModel == 8)//模式9：种子词必须出现在标题和内容中
            {


                if (Model.V3Infos.TaskDb[taskid].Get1_jingdukeyword.Length == 0)
                {
                    Log.LogNewline("[c11]当前任务没有种子词，无法确定精度，不保存数据！[/c]");
                    return false;
                }
                else
                {

                    for (int i = 0; i < Model.V3Infos.TaskDb[taskid].Get1_jingdukeyword.Length; i++)
                    {
                        if (art.DataObject[0].Contains(Model.V3Infos.TaskDb[taskid].Get1_jingdukeyword[i]) && art.DataObject[1].Contains(Model.V3Infos.TaskDb[taskid].Get1_jingdukeyword[i]))
                        {
                            return true;
                        }
                    }

                    return false;
                }
            }
            return false;

        }
        #endregion

        #region 关键字智能抓取
        /// <summary>
        /// 主过程
        /// </summary>
        /// <param name="taskid"></param>
        static void KeywordGet(int taskid)
        {
            List<string> keywords =xEngine.Common.XSerializable.CloneObject<List<string>>(Model.V3Infos.KeywordDb[Model.V3Infos.TaskDb[taskid].KeywordDbId.ToString()].Keywords) ;
            if (Model.V3Infos.TaskDb[taskid].GetRunModel)
            {
                Thread.Sleep(100);
                Model.V3Infos.TaskDb[taskid].TaskStatusStr = "抓取模式为“关键字智能抓取”,正在准备抓取...";
                V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：抓取模式为“关键字智能抓取”无条件抓取模式，本次需要抓取的关键字总数为" + keywords.Count + "个[/c]");
                Random r = new Random();
                int counts = 0;
                while (keywords.Count > 0)
                {
                    if (Model.V3Infos.TaskCancelToken[taskid].IsCancellationRequested)
                    {
                        break;
                    }
                    string thiskeyword = keywords[0].ToString();
                    Model.V3Infos.TaskDb[taskid].TaskStatusStr = "开始进行关键字“" + thiskeyword + "”的抓取！";
                    V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：开始进行关键字“" + thiskeyword + "”的抓取！待抓关键字：" + keywords.Count + "[/c]");
                    int count = KeywordGetGo(taskid, thiskeyword);
                    counts = counts + count;
                    Model.V3Infos.TaskDb[taskid].TaskStatusStr = "关键字" + thiskeyword + "的抓取完毕";
                    V3.Common.Log.LogNewline("[c12]任务【" + taskid + "】：关键字“" + thiskeyword + "”的抓取完毕，获取到" + count + "条数据！待抓关键字：" + keywords.Count + "[/c]");
                    keywords.Remove(keywords[0]);
                }
                Model.V3Infos.TaskDb[taskid].TaskStatusStr = "已经全部抓取完所有关键字";
                V3.Common.Log.LogNewline("[c12]任务【" + taskid + "】：已经全部抓取完所有关键字，本地抓取动作共抓取到" + counts + "条数据[/c]");
            }
            else
            {
                int dbdatacount = V3.Bll.DbBll.GetArticleOkCount(Model.V3Infos.TaskDb[taskid].ArticleDbId.ToString(), Model.V3Infos.TaskDb[taskid].HashDbId.ToString());
                Random r = new Random();
                int mubiaoNumber = Model.V3Infos.TaskDb[taskid].GetPiciNumber * Model.V3Infos.TaskDb[taskid].Picinumber;
                if (dbdatacount < mubiaoNumber)
                {
                    Thread.Sleep(100);
                    V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：剩余数据为" + dbdatacount + "条，不达标，触发抓取过程，本次需要抓取的数据总数为" + mubiaoNumber + "条，当前关键字总数：" + keywords.Count + "个！[/c]");
                }
                else
                {
                    Thread.Sleep(100);
                    Model.V3Infos.TaskDb[taskid].TaskStatusStr = "剩余数据为" + dbdatacount + "条，达到发布需求，跳过抓取过程！";
                    V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：剩余数据为" + dbdatacount + "条，达到发布需求，跳过抓取过程！[/c]");
                    return;
                }
                Thread.Sleep(100);
                V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：抓取模式为按需抓取模式，本次需要抓取的数据总数为" + Model.V3Infos.TaskDb[taskid].GetPiciNumber + "*" + Model.V3Infos.TaskDb[taskid].Picinumber + "=" + (Model.V3Infos.TaskDb[taskid].GetPiciNumber * Model.V3Infos.TaskDb[taskid].Picinumber) + "条，当前关键字总数：" + keywords.Count + "个！[/c]");
                int counts = 0;
                int keycount = keywords.Count;
                for (int i = 0; i < keycount; i++)
                {
                    if (Model.V3Infos.TaskCancelToken[taskid].IsCancellationRequested)
                    {
                        break;
                    }
                    string thiskeyword = keywords[r.Next(0, keywords.Count)].ToString();
                    Model.V3Infos.TaskDb[taskid].TaskStatusStr = "开始进行关键字“" + thiskeyword + "”的抓取！";
                    V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：开始进行关键字“" + thiskeyword + "”的抓取！待抓关键字：" + keywords.Count + ",待抓数据" + (mubiaoNumber - counts) + "条！[/c]");
                    int count = KeywordGetGo(taskid, thiskeyword);
                    counts = counts + count;
                    V3.Common.Log.LogNewline("[c12任务【" + taskid + "】：关键字“" + thiskeyword + "”的抓取完毕，获取到" + count + "条数据！待抓关键字：" + keywords.Count + ",待抓数据" + (mubiaoNumber - counts) + "条！[/c]");
                    keywords.Remove(thiskeyword);
                    if ((mubiaoNumber - counts) <= 0)
                        break;
                }
                if (Model.V3Infos.TaskCancelToken[taskid].IsCancellationRequested)
                {
                    Model.V3Infos.TaskDb[taskid].TaskStatusStr = "手动停止，本地抓取动作共抓取到" + counts + "条数据";
                    V3.Common.Log.LogNewline("[c12]任务【" + taskid + "】：手动停止，本地抓取动作共抓取到" + counts + "条数据[/c]");
                }
                else
                {
                    Model.V3Infos.TaskDb[taskid].TaskStatusStr = "已经完成抓取任务，本地抓取动作共抓取到" + counts + "条数据";
                    V3.Common.Log.LogNewline("[c12]任务【" + taskid + "】：已经完成抓取任务，本地抓取动作共抓取到" + counts + "条数据[/c]");
                }
           
            }
        }
        // 根据关键字和任务id执行分步骤
        static int KeywordGetGo(int taskid, string keyword)
        {

            int count = 0;
            Queue que = new Queue();
            //获取根地址
            string RootURl = Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Stp1_GET_Keyword_TestUrl.Replace("[关键词]", keyword).Replace("[关键词UTF8编码]", System.Web.HttpUtility.UrlEncode(keyword, Encoding.UTF8)).Replace("[关键词GBK编码]", System.Web.HttpUtility.UrlEncode(keyword, Encoding.Default));
            que.Enqueue(RootURl);
            xEngine.Model.Execute.Http.XCookieManager cookie = new xEngine.Model.Execute.Http.XCookieManager();

            if (Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Stp1_GET_refrereurl != "")
            {
                cookie = GetCookie(taskid);
                xEngine.Execute.Http execute = new xEngine.Execute.Http();
                execute.LoadScript(Properties.Resources.get,false);
                execute.CookieManager = cookie;
                execute.Scripts[0].Url = Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Stp1_GET_refrereurl;
                execute.Scripts[0].Referer = "";
                execute.Scripts[0].Encoding = Encoding.Default.WebName;
                execute.IsAutoEncoding = true;
                execute.Scripts[0].UserAgent = Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].UserAgent == "" ? "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; InfoPath.2)" : Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].UserAgent;
               execute.RunRequest(execute.Scripts[0]);

            }
            else if(RootURl.Length>0){
                cookie = GetCookie(taskid);
            }
           
            //用来判断是否重复地址
            Dictionary<string, int> chk = new Dictionary<string, int>();
            int pagenumber = 0;
            ArrayList FinalLinks = new ArrayList();
            string RootHtml = "";
            while (que.Count > 0)
            {
                if (Model.V3Infos.TaskCancelToken[taskid].IsCancellationRequested)
                {
                    break;
                }
                if (Task.Factory.CancellationToken.IsCancellationRequested)
                {
                    break;
                }
                //第一步
                pagenumber++;
                RootURl = que.Dequeue().ToString();
                xEngine.Execute.Http execute = new xEngine.Execute.Http();
                execute.LoadScript(Properties.Resources.get,false);
                execute.CookieManager = cookie;
                execute.Scripts[0].Url = RootURl;
                execute.Scripts[0].Referer = Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Stp1_GET_refrereurl;
                execute.Scripts[0].Encoding = Encoding.Default.WebName;
                execute.IsAutoEncoding = true;
                execute.Scripts[0].UserAgent = Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].UserAgent == "" ? "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; InfoPath.2)" : Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].UserAgent;
                XResponse response= execute.RunRequest(execute.Scripts[0]);
                if (response.BodyData != null)
                {
                    Program.NetWorkDownload += (response.BodyData.Length);
                }
                RootHtml = "源地址：" + execute.Scripts[0].Url + "\r\n\r\n" +  Library.HtmlHelper.GetHtmlFromByte(response.BodyData, response.ContentType);
                if (RootHtml.Contains("操作超时"))
                {
                    Model.V3Infos.TaskDb[taskid].TaskStatusStr = "任务在抓取地址" + RootURl + "时网络超时";
                    V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：任务在抓取地址" + RootURl + "时网络超时，请检查您的网络连通性或增大抓取间隔，避免被屏蔽！[/c]");
                }
                Thread.Sleep(Model.V3Infos.TaskDb[taskid].Get1_jiangetime);
                if (Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Stp1_NeedGetPage)
                {
                    if (pagenumber >= Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Stp1_PageNumber)
                    {
                        Model.V3Infos.TaskDb[taskid].TaskStatusStr = "关键字“" + keyword + "”的提取分页数，已达上限";
                        V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：关键字“" + keyword + "”的提取分页数，已达上限！[/c]");
                        break;
                    }
                    V3.Bll.GetBll get1 = new Bll.GetBll(RootHtml, Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Stp1_PageRules);
                    if (RootURl.StartsWith("http"))
                        get1.oldurl = RootURl;
                    System.Collections.ArrayList list = get1.getAllRules(taskid);
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i] = list[i].ToString().Replace("[关键词]", keyword);
                        list[i] = list[i].ToString().Replace("[关键词UTF8编码]", System.Web.HttpUtility.UrlEncode(keyword, Encoding.UTF8));
                        list[i] = list[i].ToString().Replace("[关键词GBK编码]", System.Web.HttpUtility.UrlEncode(keyword, Encoding.Default));
                    }
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (!chk.ContainsKey(list[i].ToString()))
                        {
                            chk.Add(list[i].ToString(), 0);
                            que.Enqueue(list[i].ToString());
                        }
                    }
                }
                //第二步
                if (!Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Stp2_GET_NoNeedStp2)
                {
                    V3.Bll.GetBll get1 = new Bll.GetBll(RootHtml, Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Stp2_GET_Keyword_Rules.Rules);
                    if (RootURl.StartsWith("http"))
                        get1.oldurl = RootURl;
                    System.Collections.ArrayList list = new System.Collections.ArrayList();
                    list = get1.getAllRules(taskid);
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (Model.V3Infos.TaskCancelToken[taskid].IsCancellationRequested)
                        {
                            break;
                        }
                        if (!chk.ContainsKey(list[i].ToString()))//检查如果存在改链接，则继续获取，避免用户设置了引申数量导致抓不到数据
                        {
                            chk.Add(list[i].ToString(), 0);
                            if (!CheckUrlHash(taskid, Library.StrHelper.Md5(list[i].ToString())))
                            {
                                FinalLinks.Add(list[i].ToString());
                                Model.V3Infos.TaskDb[taskid].TaskStatusStr = "得到链接：" + list[i].ToString();
                                if (FinalLinks.Count >= Model.V3Infos.TaskDb[taskid].Get1_getnumber)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    if (FinalLinks.Count >= Model.V3Infos.TaskDb[taskid].Get1_getnumber)
                    {
                        Model.V3Infos.TaskDb[taskid].TaskStatusStr = "提取到的链接数以达到引申数量上限";
                        V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：关键字“" + keyword + "”提取到的链接数以达到引申数量上限“" + Model.V3Infos.TaskDb[taskid].Get1_getnumber + "”！[/c]");
                        break;
                    }
                }
                else
                {
                    FinalLinks.Add(RootURl);
                    break;
                }
                Thread.Sleep(10);
                V3.Common.Log.LogNewline("[c12]任务【" + taskid + "】：关键字“" + keyword + "”已取到目标结果链接" + FinalLinks.Count + "个，当前取结果第" + pagenumber + "页！[/c]");
            }
            //第三步
            if (Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Stp2_GET_NoNeedStp2)
            {
                Model.Model_Article art = new Model.Model_Article();
                art.DataObject = new Dictionary<int, string>(){
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
                art = GetArtModel(taskid, FinalLinks, 0, art, RootHtml);

                if (Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].PlanModel == 1 || Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].PlanModel == 10)
                {
                    art.DataObject[29] = keyword;
                    art.DataObject[28] = RootURl;
                }
                if (jingdu(taskid, art))
                {
                    if (AddArtsToDbs(taskid, art))
                    {
                        try
                        {
                            AddUrlHash(taskid, Library.StrHelper.Md5(art.DataObject[28].ToString()));//存入链接
                        }
                        catch { }
                        Model.V3Infos.TaskDb[taskid].TaskStatusStr = "提抓到数据" + Substring(art.DataObject[0], 8);
                        V3.Common.Log.LogNewline("[c12]任务【" + taskid + "】：成功采集到“" + Substring(art.DataObject[0], 15) + "”的数据累计:" + count + "，池剩余：" + (FinalLinks.Count) + "！[/c]");
                        count++;
                    }
                }
                else
                {
                    V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：数据不符合精度要求，跳过！[/c]");
                }

            }
            else
            {
                for (int ii = 0; ii < FinalLinks.Count; ii++)
                {
                    if (Model.V3Infos.TaskCancelToken[taskid].IsCancellationRequested)
                    {
                        break;
                    }
                    Model.Model_Article art = new Model.Model_Article();
                    art.DataObject = new Dictionary<int, string>()
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
                    if (Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Stp3_RefrereURL != "")
                    {
                        cookie = GetCookie(taskid);
                        xEngine.Execute.Http execute = new xEngine.Execute.Http();
                        execute.LoadScript(Properties.Resources.get,false);
                        execute.CookieManager = cookie;
                        execute.Scripts[0].Url = Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Stp3_RefrereURL;
                        execute.Scripts[0].Referer = Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Stp1_GET_refrereurl;
                        execute.Scripts[0].Encoding = Encoding.Default.WebName;
                        execute.IsAutoEncoding = true;
                        execute.Scripts[0].UserAgent = Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].UserAgent == "" ? "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; InfoPath.2)" : Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].UserAgent;
                        execute.RunRequest(execute.Scripts[0]);
                    }
                    
                  

                    xEngine.Execute.Http executes = new xEngine.Execute.Http();
                    executes.LoadScript(Properties.Resources.get,false);
                    executes.CookieManager = cookie;
                    executes.Scripts[0].Url = FinalLinks[ii].ToString();
                    executes.Scripts[0].Referer = Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Stp3_RefrereURL;
                    executes.Scripts[0].Encoding = Encoding.Default.WebName;
                    executes.IsAutoEncoding = true;
                    executes.Scripts[0].UserAgent = Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].UserAgent == "" ? "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; InfoPath.2)" : Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].UserAgent;
                    XResponse response = executes.RunRequest(executes.Scripts[0]);
                    if (response.BodyData != null)
                    {
                        Program.NetWorkDownload += (response.BodyData.Length);
                    }
                    string FinalHtml = Library.HtmlHelper.GetHtmlFromByte(response.BodyData, response.ContentType);
                    if (FinalHtml.Contains("操作超时"))
                    {
                        Model.V3Infos.TaskDb[taskid].TaskStatusStr = "抓取超时";
                        V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：任务在抓取地址" + FinalLinks[ii].ToString() + "时网络超时，请检查您的网络连通性或增大抓取间隔，避免被屏蔽！[/c]");
                    }
                    Thread.Sleep(Model.V3Infos.TaskDb[taskid].Get1_jiangetime);
                    art = GetArtModel(taskid, FinalLinks, 0, art, FinalHtml);
                    art.DataObject[29] = keyword;
                    art.DataObject[28] = FinalLinks[ii].ToString();
                    if (jingdu(taskid, art))
                    {
                        if (AddArtsToDbs(taskid, art))
                        {
                            AddUrlHash(taskid, Library.StrHelper.Md5(art.DataObject[28].ToString()));//存入链接
                            Model.V3Infos.TaskDb[taskid].TaskStatusStr = "抓取到一条数据" + Substring(art.DataObject[0], 8);
                            V3.Common.Log.LogNewline("[c12]任务【" + taskid + "】：成功采集到“" + Substring(art.DataObject[0], 15) + "”的数据，累计:" + count + "，池剩余：" + (FinalLinks.Count - ii) + "！[/c]");
                            count++;
                        }
                    }
                    else { V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：数据不符合精度要求，跳过！[/c]"); }
                }
            }
            return count;
        }
        #endregion

        #region 自定义抓取

        static void ZidingyiGet(int taskid)//自定义抓取
        {
            if (!Model.V3Infos.TaskDb[taskid].GetRunModel && (V3.Bll.DbBll.GetArticleOkCount(Model.V3Infos.TaskDb[taskid].ArticleDbId.ToString(), Model.V3Infos.TaskDb[taskid].HashDbId.ToString())) > (Model.V3Infos.TaskDb[taskid].GetPiciNumber * Model.V3Infos.TaskDb[taskid].Picinumber))
            {
                V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：本任务的剩余数据量已达指定批次数，所以本次跳过抓取过程直接进入发布过程！[/c]");
                Model.V3Infos.TaskDb[taskid].TaskStatusStr = "任务的剩余数据量已达指定批次数，所以本次跳过抓取过程直接进入发布过程";
                return;
            }
            int zidingyi_count = 0;
            Queue zidingyi_que = new Queue();
            Dictionary<string, int> zidingyi_chk = new Dictionary<string, int>();
            System.Collections.ArrayList zidingyi_alllist = new ArrayList();
            System.Collections.ArrayList zidingyi_FinalLinks = new ArrayList();
            xEngine.Model.Execute.Http.XCookieManager zidingyi_cookie = new xEngine.Model.Execute.Http.XCookieManager();
            //用来判断是否重复地址
            int zidingyi_pagenumber = 1;
            string RootURl ="";
            for (int rooturlcount = 0; rooturlcount < (Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].IsuseTaskRukou ? Model.V3Infos.TaskDb[taskid].RukouUrl : Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Zidingyi_stp1_RuKouUrls).Count; rooturlcount++)//循环入口链接列表
            {
                RootURl= (Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].IsuseTaskRukou ? Model.V3Infos.TaskDb[taskid].RukouUrl : Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Zidingyi_stp1_RuKouUrls)[rooturlcount].ToString();
                if (RootURl.Trim().Length == 0) { break; }
                zidingyi_que.Enqueue(RootURl);
            }
            if (Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Stp1_GET_refrereurl != "")
            {
                zidingyi_cookie = GetCookie(taskid);

                xEngine.Execute.Http executes = new xEngine.Execute.Http();
                executes.LoadScript(Properties.Resources.get, false);
                executes.CookieManager = zidingyi_cookie;
                executes.Scripts[0].Url = Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Stp1_GET_refrereurl;
                executes.Scripts[0].Referer = "";
                executes.Scripts[0].Encoding = Encoding.Default.WebName;
                executes.IsAutoEncoding = true;
                executes.Scripts[0].UserAgent = Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].UserAgent == "" ? "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; InfoPath.2)" : Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].UserAgent;
                XResponse response = executes.RunRequest(executes.Scripts[0]);
                if (response.BodyData != null)
                {
                    Program.NetWorkDownload += (response.BodyData.Length);
                }
                string FinalHtml = Library.HtmlHelper.GetHtmlFromByte(response.BodyData, response.ContentType);
            }
            else if(RootURl.Length>0)
            {
                zidingyi_cookie = GetCookie(taskid );
            }

           

            while (zidingyi_que.Count > 0)
            {
                if (Model.V3Infos.TaskCancelToken[taskid].IsCancellationRequested)
                {
                    break;
                }
                //第一步
                string zidingyi_RootHtml = "";
                RootURl = "";
                RootURl = zidingyi_que.Dequeue().ToString();
               
                xEngine.Execute.Http executes = new xEngine.Execute.Http();
                executes.LoadScript(Properties.Resources.get,false);
                executes.CookieManager = zidingyi_cookie;
                executes.Scripts[0].Url = RootURl;
                executes.Scripts[0].Referer = Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Zidingyi_stp1_refrereurl;
                executes.Scripts[0].Encoding = Encoding.Default.WebName;
                executes.IsAutoEncoding = true;
                executes.Scripts[0].UserAgent = Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].UserAgent == "" ? "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; InfoPath.2)" : Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].UserAgent;
                XResponse response = executes.RunRequest(executes.Scripts[0]);
                if (response.BodyData != null)
                {
                    Program.NetWorkDownload += (response.BodyData.Length);
                }
                zidingyi_RootHtml = Library.HtmlHelper.GetHtmlFromByte(response.BodyData, response.ContentType);

                if (zidingyi_RootHtml.Contains("操作超时"))
                {
                    Model.V3Infos.TaskDb[taskid].TaskStatusStr = "操作超时";
                    V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：任务在抓取地址" + RootURl + "时网络超时，请检查您的网络连通性或增大抓取间隔，避免被屏蔽！[/c]");
                }
                Thread.Sleep(Model.V3Infos.TaskDb[taskid].zidingyi_Get1_jiangetime);

                if (zidingyi_pagenumber >= Model.V3Infos.TaskDb[taskid].Zidingyi_Totalpage)
                {
                    Model.V3Infos.TaskDb[taskid].TaskStatusStr = "获取的页数已达到指定的页数";
                    V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：获取的页数已达到指定的页数“" + Model.V3Infos.TaskDb[taskid].Zidingyi_Totalpage + "”[/c]");
                    break;
                }
                V3.Bll.GetBll get1 = new Bll.GetBll(zidingyi_RootHtml, Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Zidingyi_Stp1_PageRules);
                if (RootURl.StartsWith("http"))
                    get1.oldurl = RootURl;
                System.Collections.ArrayList list = get1.getAllRules(taskid);
                if (!zidingyi_alllist.Contains(RootURl))
                {
                    zidingyi_alllist.Add(RootURl);
                }
                for (int i = 0; i < list.Count; i++)
                {

                    if (!zidingyi_chk.ContainsKey(list[i].ToString()))
                    {
                        zidingyi_pagenumber++;
                        Model.V3Infos.TaskDb[taskid].TaskStatusStr = "获取到一个新列表页" + list[i].ToString();
                        V3.Common.Log.LogNewline("[c12]任务【" + taskid + "】：成功获取到一个新列表页“" + Substring2(list[i].ToString(), 10) + "”，累计获取数:" + zidingyi_pagenumber + ",上限：" + Model.V3Infos.TaskDb[taskid].Zidingyi_Totalpage + "，蜘蛛池剩余：" + zidingyi_que.Count + "[/c]");
                        zidingyi_chk.Add(list[i].ToString(), 0);
                        if (Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Zidingyi_Stp1_PageRules.GetModel != 4)
                        {
                            zidingyi_que.Enqueue(list[i].ToString());
                        }
                        zidingyi_alllist.Add(list[i]);
                        if (zidingyi_pagenumber >= Model.V3Infos.TaskDb[taskid].Zidingyi_Totalpage)
                        {
                          break;
                        }
                    }
                    else 
                    {
                       //列表页已经存在
                    }
                }
                Thread.Sleep(10);
            }
            // 第二步
            for (int listcount = 0; listcount < zidingyi_alllist.Count; listcount++)
            {
                if (Model.V3Infos.TaskCancelToken[taskid].IsCancellationRequested)
                {
                    break;
                }
                //zidingyi_cookie = GetCookie(taskid, zidingyi_alllist[listcount].ToString());

                xEngine.Execute.Http executes = new xEngine.Execute.Http();
                executes.LoadScript(Properties.Resources.get,false);
                executes.CookieManager = zidingyi_cookie;
                executes.Scripts[0].Url = zidingyi_alllist[listcount].ToString();
                executes.IsRedirect = true;
                executes.Scripts[0].Referer = Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Zidingyi_stp1_refrereurl;
                executes.Scripts[0].Encoding = Encoding.Default.WebName;
                executes.IsAutoEncoding = true;
                executes.Scripts[0].UserAgent = Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].UserAgent == "" ? "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; InfoPath.2)" : Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].UserAgent;
                XResponse response= executes.RunRequest(executes.Scripts[0]);
                if (response.BodyData != null)
                {
                    Program.NetWorkDownload += (response.BodyData.Length);
                }
                string rhtml = Library.HtmlHelper.GetHtmlFromByte(response.BodyData, response.ContentType);

                V3.Bll.GetBll get1 = new Bll.GetBll(rhtml, Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Zidingyi_Stp2_GET_Rules.Rules);
                if (zidingyi_alllist[listcount].ToString().StartsWith("http"))
                    get1.oldurl = zidingyi_alllist[listcount].ToString();
                System.Collections.ArrayList list = new System.Collections.ArrayList();
                list = get1.getAllRules(taskid);
                for (int i = 0; i < list.Count; i++)
                {
                    if (!zidingyi_chk.ContainsKey(list[i].ToString()))
                    {
                        zidingyi_chk.Add(list[i].ToString(), 0);
                        string hash = Library.StrHelper.Md5(list[i].ToString());
                        if (CheckUrlHash(taskid, hash))
                        {
                            continue;
                        }
                        zidingyi_FinalLinks.Add(list[i].ToString());
                        if (zidingyi_FinalLinks.Count >= Model.V3Infos.TaskDb[taskid].Zidingyi_total_getnumber)
                        {
                            break;
                        }
                    }
                }
                if (zidingyi_FinalLinks.Count >= Model.V3Infos.TaskDb[taskid].Zidingyi_total_getnumber)
                {
                    Model.V3Infos.TaskDb[taskid].TaskStatusStr = "提取到的链接数以达到任务设置最大链接数量上限";
                    V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】”：提取到的链接数以达到任务设置最大链接数量上限“" + Model.V3Infos.TaskDb[taskid].Zidingyi_total_getnumber + "”！[/c]");
                    break;
                }
                V3.Common.Log.LogNewline("[c12]任务【" + taskid + "】：已取到目标结果链接" + zidingyi_FinalLinks.Count + "个，当前获取第" + listcount + "个列表页,剩余：" + (zidingyi_alllist.Count - listcount) + "页！[/c]");
                Thread.Sleep(10);
            }


            V3.Common.Log.LogNewline("[c12]任务【" + taskid + "】：列表抓取完毕，开始抓取正文，池剩余：" + (zidingyi_FinalLinks.Count) + "！[/c]");
            for (int ii = 0; ii < zidingyi_FinalLinks.Count; ii++)//获取最后内容
            {
                if (Model.V3Infos.TaskCancelToken[taskid].IsCancellationRequested)
                {
                    break;
                }
                Model.Model_Article art = new Model.Model_Article();
                art.DataObject = new Dictionary<int, string>()
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
                if (Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Zidingyi_Stp3_RefrereURL != "")
                {
                    zidingyi_cookie = GetCookie(taskid);

                    xEngine.Execute.Http executes = new xEngine.Execute.Http();
                    executes.LoadScript(Properties.Resources.get,false);
                    executes.CookieManager = zidingyi_cookie;
                    executes.Scripts[0].Url = Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Zidingyi_Stp3_RefrereURL;
                    executes.Scripts[0].Referer = Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Zidingyi_stp1_refrereurl;
                    executes.Scripts[0].Encoding = Encoding.Default.WebName;
                    executes.IsAutoEncoding = true;
                    executes.Scripts[0].UserAgent = Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].UserAgent == "" ? "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; InfoPath.2)" : Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].UserAgent;
                    executes.RunRequest(executes.Scripts[0]);
                    
}
               // zidingyi_cookie = GetCookie(taskid, zidingyi_FinalLinks[ii].ToString());

                xEngine.Execute.Http executes1 = new xEngine.Execute.Http();
                executes1.LoadScript(Properties.Resources.get,false);
                executes1.CookieManager = zidingyi_cookie;
                executes1.Scripts[0].Url = zidingyi_FinalLinks[ii].ToString();
                executes1.Scripts[0].Referer = Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Zidingyi_Stp3_RefrereURL;
                executes1.Scripts[0].Encoding = Encoding.Default.WebName;
                executes1.IsAutoEncoding = true;
                executes1.Scripts[0].UserAgent = Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].UserAgent == "" ? "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; InfoPath.2)" : Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].UserAgent;
                XResponse response = executes1.RunRequest(executes1.Scripts[0]);
                if (response.BodyData != null)
                {
                    Program.NetWorkDownload += (response.BodyData.Length);
                }
                string FinalHtml = "源地址：" + executes1.Scripts[0].Url + "\r\n\r\n" + Library.HtmlHelper.GetHtmlFromByte(response.BodyData, response.ContentType);
                
                if (FinalHtml.Contains("操作超时"))
                {
                    Model.V3Infos.TaskDb[taskid].TaskStatusStr = "操作超时";
                    V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：任务在抓取地址" + zidingyi_FinalLinks[ii].ToString() + "时网络超时，请检查您的网络连通性或增大抓取间隔，避免被屏蔽！[/c]");
                }
                Thread.Sleep(Model.V3Infos.TaskDb[taskid].zidingyi_Get1_jiangetime);
                art = GetArtModel(taskid, zidingyi_FinalLinks, ii, art, FinalHtml);
                //if (Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].PlanModel == 2 || Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].PlanModel == 20)
                //{
                    art.DataObject[28] = zidingyi_FinalLinks[ii].ToString();
                    if (art.DataObject[29].ToString() == "")
                    {
                        art.DataObject[29] = Bll.DbBll.GetRandomKeyword(Model.V3Infos.TaskDb[taskid].KeywordDbId.ToString());
                    }
                //}

                if (jingdu(taskid, art))
                {


                    if (AddArtsToDbs(taskid, art))
                    {
                        string hash = Library.StrHelper.Md5(zidingyi_FinalLinks[ii].ToString());
                        AddUrlHash(taskid, hash);
                        zidingyi_count++;
                        Model.V3Infos.TaskDb[taskid].TaskStatusStr = "抓取到一条数据“" + Substring(art.DataObject[0], 8) + "”的数据";
                        V3.Common.Log.LogNewline("[c12]任务【" + taskid + "】：成功采集到“" + Substring(art.DataObject[0], 15) + "”的数据，累计:" + zidingyi_count + "，池剩余：" + (zidingyi_FinalLinks.Count - ii) + "！[/c]");
                    }
                }
                else
                {
                    V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：数据不符合精度要求，跳过！[/c]");
                }
                Thread.Sleep(10);
            }
        }

        #endregion

        #region 蜘蛛爬行抓取


        static void SpiderGet(int taskid)
        {
            bool IsFirstRun = false;
            if (!Model.V3Infos.TaskDb[taskid].GetRunModel && (V3.Bll.DbBll.GetArticleOkCount(Model.V3Infos.TaskDb[taskid].ArticleDbId.ToString(), Model.V3Infos.TaskDb[taskid].HashDbId.ToString())) > (Model.V3Infos.TaskDb[taskid].GetPiciNumber * Model.V3Infos.TaskDb[taskid].Picinumber))
            {
                Model.V3Infos.TaskDb[taskid].TaskStatusStr = "任务的剩余数据量已达指定批次数，所以本次跳过抓取过程直接进入发布过程";
                V3.Common.Log.LogNewline("[c11]【" + taskid + "】：本任务的剩余数据量已达指定批次数，所以本次跳过抓取过程直接进入发布过程！[/c]");
                return;
            }
            int spider_count = 0;
            Dictionary<string, int> spider_chk = new Dictionary<string, int>();
            System.Collections.ArrayList spider_FinalLinks = new ArrayList();
            xEngine.Model.Execute.Http.XCookieManager spider_cookie = new xEngine.Model.Execute.Http.XCookieManager();
            Model.GetPostModel gg = Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel];
            string queid = Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].mids + "-" + Model.V3Infos.TaskDb[taskid].HashDbId;
            if (!Model.V3Infos.MainDb.Spiderque.ContainsKey(queid))
            {
                Model.V3Infos.MainDb.Spiderque.Add(queid, new Queue());
                IsFirstRun = true;
            } string RootURl = "";
             for (int i = 0; i < (Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].IsuseTaskRukou ? Model.V3Infos.TaskDb[taskid].RukouUrl : Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Stp1_spider_mainurl).Count; i++)
            {
                RootURl=(Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].IsuseTaskRukou ? Model.V3Infos.TaskDb[taskid].RukouUrl : Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Stp1_spider_mainurl)[i].ToString();
                if (RootURl.Trim().Length == 0) { break; }
                if (RootURl == null) { break; }
                try
                {
                    Model.V3Infos.MainDb.Spiderque[queid].Enqueue(RootURl);
                }
                catch
                {
                    Model.V3Infos.MainDb.Spiderque[queid].Dequeue();

                    Model.V3Infos.MainDb.Spiderque[queid].Enqueue(RootURl);
                }
            }
          
            spider_chk = new Dictionary<string, int>();
            spider_FinalLinks = new ArrayList();
            spider_cookie = new xEngine.Model.Execute.Http.XCookieManager();
            if (Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Stp1_GET_refrereurl != "")
            {
                spider_cookie = GetCookie(taskid);

                xEngine.Execute.Http executes1 = new xEngine.Execute.Http();
                executes1.LoadScript(Properties.Resources.get, false);
                executes1.CookieManager = spider_cookie;
              
                executes1.Scripts[0].Url = Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Stp1_GET_refrereurl;
                executes1.Scripts[0].Referer = "";
                executes1.Scripts[0].Encoding = Encoding.Default.WebName;
                executes1.IsAutoEncoding = true;
                executes1.Scripts[0].UserAgent = Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].UserAgent == "" ? "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; InfoPath.2)" : Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].UserAgent;
                executes1.RunRequest(executes1.Scripts[0]);
            }
            else if(RootURl.Length>0)
            {
                spider_cookie = GetCookie(taskid);
            }

            if (Model.V3Infos.MainDb.Spiderque[queid].Count==0)
            {
                IsFirstRun = true;
            }
           
            V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：蜘蛛缓存队列剩余" + Model.V3Infos.MainDb.Spiderque[queid].Count + "条！[/c]");
            int hucount = 0;
            bool first = true;
            while (Model.V3Infos.MainDb.Spiderque[queid].Count > 0)
            {
                if (Model.V3Infos.TaskCancelToken[taskid].IsCancellationRequested)
                {
                    break;
                }

                if (spider_count >= Model.V3Infos.TaskDb[taskid].Spider_maxget)
                {
                    Model.V3Infos.TaskDb[taskid].TaskStatusStr = "蜘蛛爬行已抓取到指定数量";
                    V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：蜘蛛爬行已抓取到指定数量" + Model.V3Infos.TaskDb[taskid].Spider_maxget + "，结束爬行过程！[/c]");
                    break;
                }
                string RootHtml = "";
                RootURl = "";
               object oo = Model.V3Infos.MainDb.Spiderque[queid].Dequeue();
                while (oo == null) 
                { 
                    oo = Model.V3Infos.MainDb.Spiderque[queid].Dequeue(); 
                }
                RootURl = oo.ToString();

                Model.V3Infos.TaskDb[taskid].TaskStatusStr = "正在抓取" + RootURl;
                string hash = Library.StrHelper.Md5(RootURl);
                if (IsFirstRun)
                {
                    if (CheckUrlHash(taskid, hash))
                    {
                      
                    }
                    else
                    {
                        AddUrlHash(taskid, hash);
                    }
                    IsFirstRun = false;
                }
                else
                {

                    if (CheckUrlHash(taskid, hash))
                    {
                        hucount++;
                        continue;
                    }
                    else
                    {
                        AddUrlHash(taskid, hash);
                    }
                }
                //spider_cookie = GetCookie(taskid, RootURl);

                xEngine.Execute.Http executes1 = new xEngine.Execute.Http();
                executes1.LoadScript(Properties.Resources.get,false);
                executes1.CookieManager = spider_cookie;
                executes1.Scripts[0].Url = RootURl;
                executes1.Scripts[0].Referer = Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Stp1_spider_refererurl;
                executes1.Scripts[0].Encoding = Encoding.Default.WebName;
                executes1.IsAutoEncoding = true;
                executes1.Scripts[0].UserAgent = Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].UserAgent == "" ? "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; InfoPath.2)" : Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].UserAgent;
                XResponse response= executes1.RunRequest(executes1.Scripts[0]);
                if (response.BodyData != null)
                {
                    Program.NetWorkDownload += (response.BodyData.Length);
                }
                RootHtml = "源地址：" + executes1.Scripts[0].Url + "\r\n\r\n" + Library.HtmlHelper.GetHtmlFromByte(response.BodyData, response.ContentType);

               Thread.Sleep(Model.V3Infos.TaskDb[taskid].Spider_jiange);
               V3.Bll.GetBll get1 = new Bll.GetBll(RootHtml, Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Stp1_spider_rules);
                if (RootURl.StartsWith("http"))
                    get1.oldurl = RootURl;
                System.Collections.ArrayList list = new System.Collections.ArrayList();
                list = get1.getAllRules(taskid);
                for (int i = 0; i < list.Count; i++)
                {
                    if (Model.V3Infos.TaskCancelToken[taskid].IsCancellationRequested)
                    {
                        break;
                    }
                    if (!spider_chk.ContainsKey(list[i].ToString()))
                    {
                        spider_chk.Add(list[i].ToString(), 0);
                        hash = Library.StrHelper.Md5(list[i].ToString());
                        if (CheckUrlHash(taskid, hash))
                        {
                            continue;
                        }
                        else
                        {
                            try
                            {
                                Model.V3Infos.MainDb.Spiderque[queid].Enqueue(list[i].ToString());
                            }
                            catch { Model.V3Infos.MainDb.Spiderque[queid].Dequeue(); Model.V3Infos.MainDb.Spiderque[queid].Enqueue(list[i].ToString()); }
                        }
                    }
                }
                spider_FinalLinks.Clear();
                spider_FinalLinks.Add(RootURl);
                Model.Model_Article art = new Model.Model_Article();
                art.DataObject = new Dictionary<int, string>()
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
                GetArtModel(taskid, spider_FinalLinks, 0, art, RootHtml);
                art.DataObject[28] = RootURl;
                if (art.DataObject[29].ToString() == "")
                {
                    art.DataObject[29] = Bll.DbBll.GetRandomKeyword(Model.V3Infos.TaskDb[taskid].KeywordDbId.ToString());
                }
                if (jingdu(taskid, art))
                {

                    if (AddArtsToDbs(taskid, art))
                    {
                        spider_count++;
                        Model.V3Infos.TaskDb[taskid].TaskStatusStr = "抓取到一条“" + Substring(art.DataObject[0], 8) + "”的数据";
                        V3.Common.Log.LogNewline("[c12]任务【" + taskid + "】：成功采集到“" + Substring(art.DataObject[0],15) + "”的数据，累计:" + spider_count + "，上限：" + Model.V3Infos.TaskDb[taskid].Spider_maxget + "，池剩余：" + (Model.V3Infos.MainDb.Spiderque[queid].Count) + "！[/c]");
                    }
                }
                else
                {
                    V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：数据不符合精度要求，跳过！[/c]");
                }
                Thread.Sleep(10);
            }

           
            if (Model.V3Infos.TaskCancelToken[taskid].IsCancellationRequested)
            {
                V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：任务被终止！[/c]");
            }
            else
            {
                V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：忽略" + hucount + "个重复链接！[/c]");
                V3.Common.Log.LogNewline("[c12]任务【" + taskid + "】：蜘蛛缓存队列已经无数据！[/c]");
                Model.V3Infos.TaskDb[taskid].TaskStatusStr = "蜘蛛缓存队列已经无数据";
            }
        }
        #endregion

        #region 追踪模式
        //用来判断是否重复地址
        private static void TongbuGet(int taskid)
        {
            Queue tongbu_que = new Queue();
            Dictionary<string, int> tongbu_chk = new Dictionary<string, int>();
            List<string> tongbu_alllist = new List<string>();
            System.Collections.ArrayList tongbu_FinalLinks = new ArrayList();
            xEngine.Model.Execute.Http.XCookieManager tongbu_cookie = new xEngine.Model.Execute.Http.XCookieManager();

            ArrayList allart = new ArrayList();
            int tongbu_count = 0;


            if (Model.V3Infos.TaskCancelToken[taskid].IsCancellationRequested)
            {
                return;
            }
            //获取所有用户输入的列表页链接
            tongbu_alllist = (Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].IsuseTaskRukou
                ? Model.V3Infos.TaskDb[taskid].RukouUrl
                : Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Tongbu_stp1_RuKouUrls);
            // 第二步
            for (int listcount = 0; listcount < tongbu_alllist.Count; listcount++)
            {
                if (Model.V3Infos.TaskCancelToken[taskid].IsCancellationRequested)
                {
                    break;
                }
                tongbu_cookie = GetCookie(taskid);

                xEngine.Execute.Http executes1 = new xEngine.Execute.Http();
                executes1.LoadScript(Properties.Resources.get, false);
                executes1.CookieManager = tongbu_cookie;
                executes1.Scripts[0].Url = tongbu_alllist[listcount].ToString();
                executes1.Scripts[0].Referer =
                    Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Tongbu_stp1_refrereurl;
                executes1.Scripts[0].Encoding = Encoding.Default.WebName;
                executes1.IsAutoEncoding = true;
                executes1.Scripts[0].UserAgent =
                    Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].UserAgent == ""
                        ? "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; InfoPath.2)"
                        : Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].UserAgent;
                XResponse response = executes1.RunRequest(executes1.Scripts[0]);
                if (response.BodyData != null)
                {
                    Program.NetWorkDownload += (response.BodyData.Length);
                }
                string rhtml = Library.HtmlHelper.GetHtmlFromByte(response.BodyData, response.ContentType);
                V3.Bll.GetBll get1 = new Bll.GetBll(rhtml,
                    Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Tongbu_Stp2_Rules.Rules);
                if (tongbu_alllist[listcount].ToString().StartsWith("http"))
                    get1.oldurl = tongbu_alllist[listcount].ToString();
                System.Collections.ArrayList list = new System.Collections.ArrayList();
                list = get1.getAllRules(taskid);
                for (int i = 0; i < list.Count; i++)
                {
                    if (Model.V3Infos.TaskCancelToken[taskid].IsCancellationRequested)
                    {
                        break;
                    }
                    if (!tongbu_chk.ContainsKey(list[i].ToString()))
                    {
                        tongbu_chk.Add(list[i].ToString(), 0);
                        string hash = Library.StrHelper.Md5(list[i].ToString());
                        if (CheckUrlHash(taskid, hash))
                        {
                            continue;
                        }
                        tongbu_FinalLinks.Add(list[i].ToString());

                    }
                }
                Model.V3Infos.TaskDb[taskid].TaskStatusStr = "已取到目标结果链接" + tongbu_FinalLinks.Count + "个";
                V3.Common.Log.LogNewline(
                    "[c12]任务【" + taskid + "】：已取到目标结果链接" + tongbu_FinalLinks.Count + "个，当前获取第" +
                    listcount + "个列表页,剩余：" + (tongbu_alllist.Count - listcount) + "页！[/c]");
                Thread.Sleep(10);
            }
            Model.V3Infos.TaskDb[taskid].TaskStatusStr = "列表抓取完毕，开始抓取正文";
            V3.Common.Log.LogNewline(
                "[c11]任务【" + taskid + "】：列表抓取完毕，开始抓取正文，池剩余：" + (tongbu_FinalLinks.Count) + "...[/c]");
            for (int ii = 0; ii < tongbu_FinalLinks.Count; ii++) //获取最后内容
            {
                if (Model.V3Infos.TaskCancelToken[taskid].IsCancellationRequested)
                {
                    break;
                }

                Model.Model_Article art = new Model.Model_Article();
                art.DataObject = new Dictionary<int, string>()
                {
                    {0, ""},
                    {1, ""},
                    {2, ""},
                    {3, ""},
                    {4, ""},
                    {5, ""},
                    {6, ""},
                    {7, ""},
                    {8, ""},
                    {9, ""},
                    {10, ""},
                    {11, ""},
                    {12, ""},
                    {13, ""},
                    {14, ""},
                    {15, ""},
                    {16, ""},
                    {17, ""},
                    {18, ""},
                    {19, ""},
                    {20, ""},
                    {21, ""},
                    {22, ""},
                    {23, ""},
                    {24, ""},
                    {25, ""},
                    {26, ""},
                    {27, ""},
                    {28, ""},
                    {29, ""}
                };
                if (Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Tongbu_stp1_refrereurl != "")
                {
                    tongbu_cookie = GetCookie(taskid);

                    xEngine.Execute.Http executes1 = new xEngine.Execute.Http();
                    executes1.LoadScript(Properties.Resources.get, false);
                    executes1.CookieManager = tongbu_cookie;
                    executes1.Scripts[0].Url =
                        Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Tongbu_stp1_refrereurl;
                    executes1.Scripts[0].Referer = "";
                    executes1.Scripts[0].Encoding = Encoding.Default.WebName;
                    executes1.IsAutoEncoding = true;
                    executes1.Scripts[0].UserAgent =
                        Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].UserAgent == ""
                            ? "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; InfoPath.2)"
                            : Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].UserAgent;
                    executes1.RunRequest(executes1.Scripts[0]);

                }
                
                //tongbu_cookie = GetCookie(taskid, tongbu_FinalLinks[ii].ToString());

                xEngine.Execute.Http executes2 = new xEngine.Execute.Http();
                executes2.LoadScript(Properties.Resources.get, false);
                executes2.CookieManager = tongbu_cookie;
                executes2.Scripts[0].Url = tongbu_FinalLinks[ii].ToString();
                executes2.Scripts[0].Referer =
                    Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].Tongbu_Stp3_RefrereURL;
                executes2.Scripts[0].Encoding = Encoding.Default.WebName;
                executes2.IsAutoEncoding = true;
                executes2.Scripts[0].UserAgent =
                    Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].UserAgent == ""
                        ? "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; InfoPath.2)"
                        : Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].UserAgent;
                XResponse response = executes2.RunRequest(executes2.Scripts[0]);
                if (response.BodyData != null)
                {
                    Program.NetWorkDownload += (response.BodyData.Length);
                }
                string FinalHtml = "源地址：" + executes2.Scripts[0].Url + "\r\n\r\n" + Library.HtmlHelper.GetHtmlFromByte(response.BodyData, response.ContentType);
                if (FinalHtml.Contains("操作超时"))
                {
                    Model.V3Infos.TaskDb[taskid].TaskStatusStr = "操作超时";
                    V3.Common.Log.LogNewline(
                        "[c14]任务【" + taskid + "】：任务在抓取地址" + tongbu_FinalLinks[ii].ToString() +
                        "时网络超时，请检查您的网络连通性或增大抓取间隔，避免被屏蔽！[/c]");
                }
                art = GetArtModel(taskid, tongbu_FinalLinks, ii, art, FinalHtml);
                art.DataObject[28] = tongbu_FinalLinks[ii].ToString();
                if (art.DataObject[29].ToString() == "")
                {
                    art.DataObject[29] = Bll.DbBll.GetRandomKeyword(Model.V3Infos.TaskDb[taskid].KeywordDbId.ToString());
                }
                if (jingdu(taskid, art))
                {
                    Regex r = new Regex("<.*?>");
                    if (art == null)
                    {
                        V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：跳过一条数据，因为该文章对象为空！[/c]");
                        continue; ;
                    }
                    if (Model.V3Infos.TaskDb[taskid].Qianchuorhouchu == true)
                    {
                        if (MarkNewArticle(ref art, taskid).Contains("含有敏感字符"))
                        {
                            V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：跳过一条数据，因为包含敏感字符，当前设置为跳过！[/c]");
                            continue; ;
                        }
                    }

                  
                    if (r.Replace(art.DataObject[0], "").Length < Model.V3Infos.TaskDb[taskid].MinTitlestr)
                    {
                        V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：跳过一条数据，因为该数据的标题小于任务要求的长度" + Model.V3Infos.TaskDb[taskid].MinTitlestr + "！[/c]");
                        continue;
                    }
                    if (r.Replace(art.DataObject[1], "").Length < Model.V3Infos.TaskDb[taskid].MinContentstr)
                    {
                        V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：跳过一条数据，因为该数据的内容小于任务要求的长度" + Model.V3Infos.TaskDb[taskid].MinContentstr + "！[/c]");
                        continue;
                    }


                    //文章模型存入arraylist
                    if (Model.V3Infos.TaskDb[taskid].HashModel == 1) //标题判断
                    {

                        string hash = Library.StrHelper.Md5(art.DataObject[0].ToString());
                        if (CheckTitleHash(taskid, hash))
                        {
                            V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：抓取到重复标题“" + Substring(art.DataObject[0].ToString(), 8) + "”当前设置为根据标题判断，自动跳过！[/c]");
                            continue;
                        }
                        else
                        {
                            AddTitleHash(taskid, hash);
                        }

                    }
                    else if (Model.V3Infos.TaskDb[taskid].HashModel == 2)
                    {
                        string hash = Library.StrHelper.Md5(art.DataObject[1].ToString());
                        if (CheckContentHash(taskid, hash))
                        {
                            Model.V3Infos.TaskDb[taskid].TaskStatusStr = "抓取到重复内容";
                            V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：抓取到重复正文！当前设置为根据内容判断，自动跳过！[/c]");
                            continue;
                        }
                        else
                        {
                            AddContentHash(taskid, hash);
                        }


                    }
                    allart.Add(art);
                    tongbu_count++;
                    
                    Model.V3Infos.TaskDb[taskid].TaskStatusStr = "抓取到一条数据“" + Substring(art.DataObject[0], 8);
                   
                    V3.Common.Log.LogNewline(
                        "[c12]任务【" + taskid + "】：成功采集到“" + Substring(art.DataObject[0], 15) +
                        "”的数据，累计:" + tongbu_count + "，池剩余：" + (tongbu_FinalLinks.Count - ii) +
                        "！[/c]");
                    if (tongbu_count == Model.V3Infos.TaskDb[taskid].Picinumber)
                    {
                        V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：同步追踪已抓取到本次指定发布数量" + Model.V3Infos.TaskDb[taskid].Picinumber + "，结束抓取过程！[/c]");
                        break;
                    }
                   
                        Thread.Sleep(Model.V3Infos.TaskDb[taskid].Tongbu_get1_jiangetime);
                    
                }
                else
                {
                    V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：数据不符合精度要求，跳过！[/c]");
                }
                Thread.Sleep(10);
            }
            int sendok = 0;
            if (allart.Count > 0)
            {
                if (Model.V3Infos.TaskCancelToken[taskid].IsCancellationRequested)
                {
                    return;
                }
                V3.Bll.PostBll bll =
                    new PostBll(
                        Model.V3Infos.MainDb.MyModels[
                            Model.V3Infos.SendPointDb[Model.V3Infos.TaskDb[taskid].PointId].PostModel],
                        Model.V3Infos.TaskDb[taskid].PointId, taskid);
                for (int i = 0; i < allart.Count; i++)
                {
                    if (Model.V3Infos.TaskCancelToken[taskid].IsCancellationRequested)
                    {
                        return;
                    }
                    Model.Model_Article art = (Model.Model_Article) allart[i];
                    if (MarkNewArticle(ref art, taskid).Contains("含有敏感字符"))
                    {

                        V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：啊哦，跳过一条数据，因为包含敏感字符，当前设置为跳过！[/c]" );
                    }
                    else
                    {
                        string tempstr = "";
                        if (Model.V3Infos.TaskDb[taskid].MoreAccountUseModel)
                        {
                            bll.Login(false, ref tempstr);
                        }
                        else 
                        {

                            
                        }
                        if (sendStart(taskid, art, bll, true))
                        {
                            sendok++;
                        }
                        if (
                            !CheckUrlHash(taskid,
                                Library.StrHelper.Md5(((Model.Model_Article) allart[i]).DataObject[28])))
                        {
                            AddUrlHash(taskid, Library.StrHelper.Md5(((Model.Model_Article) allart[i]).DataObject[28]));
                        }



                        if (i != Model.V3Infos.TaskDb[taskid].Picinumber - 1)
                        {
                            if (Model.V3Infos.TaskDb[taskid].TaskMoshi == 0 && Model.V3Infos.TaskDb[taskid].Fabujiange > 0)
                            {
                                int sleeptime = Model.V3Infos.TaskDb[taskid].Fabujiange;
                                V3.Common.Log.LogNewline(
                                    "[c11]任务【" + taskid + "/】：暂停" + sleeptime + "秒，将于" +
                                    DateTime.Now.AddSeconds(sleeptime) + "发布下一篇文章！[/c]");

                                Model.V3Infos.TaskDb[taskid].TaskStatusStr = "暂停" + sleeptime + "秒，将于" +
                                                                             DateTime.Now.AddSeconds(sleeptime) + "发布下一篇文章！";

                                int sleep = 0;
                                while (true)
                                {
                                    sleep++;
                                    if (sleep >= sleeptime || Model.V3Infos.TaskCancelToken[taskid].IsCancellationRequested)
                                    {
                                        break;
                                    }
                                    Thread.Sleep(1000);
                                }

                            }
                            if (Model.V3Infos.TaskDb[taskid].TaskMoshi == 1)
                            {
                                int sleeptime = 0;
                                sleeptime = Model.V3Infos.TaskDb[taskid].Fabujiange / Model.V3Infos.TaskDb[taskid].Picinumber;
                                V3.Common.Log.LogNewline(
                                    "[c11]任务【" + taskid + "/】：暂停" + sleeptime + "秒，将于" +
                                    DateTime.Now.AddSeconds(sleeptime) + "发布下一篇文章！[/c]");
                                Model.V3Infos.TaskDb[taskid].TaskStatusStr = "暂停" + sleeptime + "秒，将于" +
                                                                             DateTime.Now.AddSeconds(sleeptime) + "发布下一篇文章！";
                                int sleep = 0;
                                while (true)
                                {
                                    sleep++;
                                    if (sleep == sleeptime || Model.V3Infos.TaskCancelToken[taskid].IsCancellationRequested)
                                    {
                                        break;
                                    }
                                    Thread.Sleep(1000);
                                }


                            }
                            else if (Model.V3Infos.TaskDb[taskid].TaskMoshi == 2)
                            {
                                Random r = new Random();
                                int ir = r.Next(0, 30);
                                int sleeptime = 0;

                                if (ir % 2 == 0)
                                {
                                    sleeptime = Model.V3Infos.TaskDb[taskid].Fabujiange +
                                                (int)(Model.V3Infos.TaskDb[taskid].Fabujiange * (double)ir / 100);
                                }
                                else
                                {
                                    sleeptime = Model.V3Infos.TaskDb[taskid].Fabujiange -
                                                (int)(Model.V3Infos.TaskDb[taskid].Fabujiange * (double)ir / 100);
                                    if (sleeptime < 0)
                                    {
                                        sleeptime = 1;
                                    }
                                }
                                V3.Common.Log.LogNewline(
                                    "[c11]任务【" + taskid + "】：暂停" + sleeptime + "秒，将于" +
                                    DateTime.Now.AddSeconds(sleeptime) + "发布下一篇文章！[/c]");
                                Model.V3Infos.TaskDb[taskid].TaskStatusStr = "暂停" + sleeptime + "秒，将于" +
                                                                             DateTime.Now.AddSeconds(sleeptime) + "发布下一篇文章！";
                                int sleep = 0;
                                while (true)
                                {
                                    sleep++;
                                    if (sleep == sleeptime || Model.V3Infos.TaskCancelToken[taskid].IsCancellationRequested)
                                    {
                                        break;
                                    }
                                    Thread.Sleep(1000);
                                }
                            }
                        }
                        else 
                        {
                            V3.Common.Log.LogNewline("[c12]任务【" + taskid + "】：本批次发布过程结束，成功发布了" + sendok + "条数据！[/c]");
                            break;
                        
                        }

                    }
                    Thread.Sleep(10);
                }
                Model.V3Infos.TaskDb[taskid].TaskStatusStr = "追踪完毕,下次追踪：" +
                                                             DateTime.Now.AddSeconds(
                                                                 Model.V3Infos.TaskDb[taskid].Tongbu_ZhiXingJianGe);
                V3.Common.Log.LogNewline("[c12]任务【" + taskid + "】：本次追踪完毕,共获取到：" + tongbu_count + "条数据，成功发布：" +sendok + "条数据！系统将会在“" + Model.V3Infos.TaskDb[taskid].Tongbu_ZhiXingJianGe + "”秒后再次追踪！[/c]");
            }
            else
            {
                Model.V3Infos.TaskDb[taskid].TaskStatusStr = "追踪完毕,没有发现目标更新内容,下次追踪：" +DateTime.Now.AddSeconds( Model.V3Infos.TaskDb[taskid].Tongbu_ZhiXingJianGe);
                V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：本次追踪完毕,没有发现目标更新内容！系统将会在“" + Model.V3Infos.TaskDb[taskid].Tongbu_ZhiXingJianGe + "”秒钟后再次追踪！[/c]");
            }
            sendok = 0;
            tongbu_FinalLinks.Clear();
            allart.Clear(); //清空文章模型列表
            if (!Model.V3Infos.TaskCancelToken[taskid].IsCancellationRequested)
            {
                Model.V3Infos.TaskWaiting.Add(taskid,DateTime.Now.AddSeconds(Model.V3Infos.TaskDb[taskid].Tongbu_ZhiXingJianGe));

            }

        }

        #endregion

        #region 根据语料库生成文章
        /// <summary>
        /// 主过程
        /// </summary>
        /// <param name="taskid"></param>
        static void yuLiaokeywordGet(int taskid)
        {
            int success = 0;
            List<string> keywords =xEngine.Common.XSerializable.CloneObject<List<string>>(Model.V3Infos.KeywordDb[Model.V3Infos.TaskDb[taskid].KeywordDbId.ToString()].Keywords);
            if (Model.V3Infos.TaskDb[taskid].GetRunModel)
            {
                Thread.Sleep(100);
                Model.V3Infos.TaskDb[taskid].TaskStatusStr = "抓取模式为“语料库生成文章”,正在准备生成...";
                V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：抓取模式为“语料库生成文章”无条件生成模式，本次需要抓取的关键字总数为" + keywords.Count + "个[/c]");
                Random r = new Random();
                if (Model.V3Infos.TaskDb[taskid].YulLiaoMoShi)//随机获取
                {
                    V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：正在初始化语料库处理引擎，这个过程视语料库大小时间不等，请稍候....[/c]");
                    string path = Model.V3Infos.TaskDb[taskid].YuLiaoKuPath;
                    IndexSearcher search = new IndexSearcher(path);
                    Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_CURRENT);
                    Lucene.Net.QueryParsers.QueryParser qp = new Lucene.Net.QueryParsers.QueryParser("content", analyzer);
                    Query query = qp.Parse("的");
                    IndexReader reader = IndexReader.Open(path, true);//单个索引库
                    Searcher searcher = new IndexSearcher(reader);
                    TopScoreDocCollector collector = TopScoreDocCollector.create(Model.V3Infos.TaskDb[taskid].YuJuShuLiang, false);
                    searcher.Search(query, collector);
                    ScoreDoc[] hit = collector.TopDocs().scoreDocs;

                    for (int i = 0; i < Model.V3Infos.TaskDb[taskid].YuLiaoMax1; i++)
                    {
                        if (Model.V3Infos.TaskCancelToken[taskid].IsCancellationRequested)
                        {
                            break;
                        }
                        Model.Model_Article art = yuLiaoGetSuiJi(taskid, searcher, hit);
                     
                        if (AddArtsToDbs(taskid, art) == true)
                        {
                            success++;
                            //显示产生的 标题 ，和统计
                            V3.Common.Log.LogNewline("[c12]任务【" + taskid + "】：产生一篇文章，标题为" + Substring(art.DataObject[0].ToString(), 8) + ",共抓取到" + success + "篇文章！[/c]");

                        }

                    }
                    reader.Close();
                    search.Close();
                    GC.Collect();
                }
                else
                {
                    for (int i = 0; i < keywords.Count; i++)
                    {
                        if (Model.V3Infos.TaskCancelToken[taskid].IsCancellationRequested)
                        {
                            break;
                        }
                        for (int ii = 0; ii < Model.V3Infos.TaskDb[taskid].YuLiaoYingShen; ii++)
                        {
                            if (Model.V3Infos.TaskCancelToken[taskid].IsCancellationRequested)
                            {
                                break;
                            }
                            Model.Model_Article art = yuLiaoGetkeyword(taskid, keywords[i].ToString());
                       
                            if (AddArtsToDbs(taskid, art) == true)
                            {
                                //显示产生的 标题 ，
                                success++;
                                V3.Common.Log.LogNewline("[c12]任务【" + taskid + "】：产生一篇文章，标题为" + Substring(art.DataObject[0].ToString(), 8) + ",共抓取到" + success + "篇文章！[/c]");
                            }
                        }
                        //和统计
                    }
                    //和统计
                }
            }
            else
            {
                int dbdatacount = V3.Bll.DbBll.GetArticleOkCount(Model.V3Infos.TaskDb[taskid].ArticleDbId.ToString(), Model.V3Infos.TaskDb[taskid].HashDbId.ToString());
                Random r = new Random();
                int mubiaoNumber = Model.V3Infos.TaskDb[taskid].GetPiciNumber * Model.V3Infos.TaskDb[taskid].Picinumber;
                int getcount = 0;
                if (dbdatacount < mubiaoNumber)
                {
                    if (Model.V3Infos.TaskDb[taskid].YulLiaoMoShi)
                    {
                        V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：正在初始化语料库处理引擎，这个过程视语料库大小时间不等，请稍候....[/c]");
                        string path = Model.V3Infos.TaskDb[taskid].YuLiaoKuPath;
                        IndexSearcher search = new IndexSearcher(path);
                        Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_CURRENT);
                        Lucene.Net.QueryParsers.QueryParser qp = new Lucene.Net.QueryParsers.QueryParser("content", analyzer);
                        Query query = qp.Parse("的");
                        IndexReader reader = IndexReader.Open(path, true);//单个索引库
                        Searcher searcher = new IndexSearcher(reader);
                        TopScoreDocCollector collector = TopScoreDocCollector.create(Model.V3Infos.TaskDb[taskid].YuJuShuLiang, false);
                        searcher.Search(query, collector);
                        ScoreDoc[] hit = collector.TopDocs().scoreDocs;
                        for (int i = 0; i < Model.V3Infos.TaskDb[taskid].YuLiaoMax1; i++)
                        {
                            if (Model.V3Infos.TaskCancelToken[taskid].IsCancellationRequested)
                            {
                                break;
                            }
                            Model.Model_Article art = yuLiaoGetSuiJi(taskid, searcher, hit);
                            //文章模型存入arraylist
                            if (Model.V3Infos.TaskDb[taskid].HashModel == 1)//标题判断
                            {

                                string hash = Library.StrHelper.Md5(art.DataObject[0].ToString());
                                if (CheckTitleHash(taskid, hash))
                                {
                                    V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：抓取到重复标题“" + Substring(art.DataObject[0].ToString(), 8) + "”！当前设置为根据标题判断，自动跳过！[/c]");
                                    continue;
                                }
                                else
                                {
                                  
                                    if (AddArtsToDbs(taskid, art) == true)
                                    {
                                        V3.Common.Log.LogNewline("[c12]任务【" + taskid + "】：产生一篇文章，标题为" + Substring(art.DataObject[0].ToString(), 8) + ",共抓取到" + getcount + "篇文章！[/c]");
                                        getcount++;
                                    }
                                    AddTitleHash(taskid, hash);
                                    if (getcount >= mubiaoNumber)
                                        return;
                                }

                            }
                            else if (Model.V3Infos.TaskDb[taskid].HashModel == 2)
                            {
                                string hash = Library.StrHelper.Md5(art.DataObject[1].ToString());
                                if (CheckContentHash(taskid, hash))
                                {
                                    Model.V3Infos.TaskDb[taskid].TaskStatusStr = "抓取到重复内容";
                                    V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：抓取到重复内容！当前设置为根据内容判断，自动跳过！[/c]");
                                    continue;
                                }
                                else
                                {
                                    
                                    if (AddArtsToDbs(taskid, art) == true)
                                    {
                                        V3.Common.Log.LogNewline("[c12]任务【" + taskid + "】：产生一篇文章，标题为" + Substring(art.DataObject[0].ToString(), 8) + ",共抓取到" + getcount + "篇文章！[/c]");
                                        getcount++;
                                    }
                                    AddContentHash(taskid, hash);
                                    if (getcount >= mubiaoNumber)
                                        return;
                                }


                            }
                            else
                            {
                             
                                if (AddArtsToDbs(taskid, art) == true)
                                {
                                    V3.Common.Log.LogNewline("[c12]任务【" + taskid + "】：产生一篇文章，标题为" + Substring(art.DataObject[0].ToString(), 8) + ",共抓取到" + getcount + "篇文章！[/c]");
                                    getcount++;
                                    if (getcount >= mubiaoNumber)
                                        return;
                                }

                            }
                           
                            
                          
                        }

                        reader.Close();
                        search.Close();
                        GC.Collect();
                    }
                    else
                    {
                        for (int i = 0; i < keywords.Count; i++)
                        {
                            if (Model.V3Infos.TaskCancelToken[taskid].IsCancellationRequested)
                            {
                                break;
                            }
                            for (int ii = 0; ii < Model.V3Infos.TaskDb[taskid].YuLiaoYingShen; ii++)
                            {
                                if (Model.V3Infos.TaskCancelToken[taskid].IsCancellationRequested)
                                {
                                    break;
                                }
                                Model.Model_Article art = yuLiaoGetkeyword(taskid, keywords[i].ToString());
                               
                                   
                                    //文章模型存入arraylist
                                    if (Model.V3Infos.TaskDb[taskid].HashModel == 1)//标题判断
                                    {

                                        string hash = Library.StrHelper.Md5(art.DataObject[0].ToString());
                                        if (CheckTitleHash(taskid, hash))
                                        {
                                            V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：抓取到重复标题“" + Substring(art.DataObject[0].ToString(), 8) + "”！当前设置为根据标题判断，自动跳过！[/c]");
                                            continue;
                                        }
                                        else
                                        {
                                          
                                            if (AddArtsToDbs(taskid, art) == true)
                                            {
                                                V3.Common.Log.LogNewline("[c12]任务【" + taskid + "】：产生一篇文章，标题为" + Substring(art.DataObject[0].ToString(), 8) + ",共抓取到" + getcount + "篇文章！[/c]");
                                                getcount++;
                                            }
                                            AddTitleHash(taskid, hash);
                                        }

                                    }
                                    else if (Model.V3Infos.TaskDb[taskid].HashModel == 2)
                                    {
                                        if (art== null) { return; }
                                        string hash = Library.StrHelper.Md5(art.DataObject[1].ToString());
                                        if (CheckContentHash(taskid, hash))
                                        {
                                            Model.V3Infos.TaskDb[taskid].TaskStatusStr = "抓取到重复内容";
                                            V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：抓取到重复内容！当前设置为根据内容判断，自动跳过！[/c]");
                                            continue;
                                        }
                                        else
                                        {
                                           
                                            if (AddArtsToDbs(taskid, art) == true)
                                            {
                                                V3.Common.Log.LogNewline("[c12]任务【" + taskid + "】：产生一篇文章，标题为" + Substring(art.DataObject[0].ToString(), 8) + ",共抓取到" + getcount + "篇文章！[/c]");
                                                getcount++;
                                            }
                                            AddContentHash(taskid, hash);
                                        }


                                    }
                                    else
                                    {
                                     
                                    if (AddArtsToDbs(taskid, art) == true)
                                    {
                                        V3.Common.Log.LogNewline("[c12]任务【" + taskid + "】：产生一篇文章，标题为" + Substring(art.DataObject[0].ToString(), 8) + ",共抓取到" + getcount + "篇文章！[/c]");
                                        getcount++;
                                    }
                                
                                }

                                //显示产生的 标题 ，
                            }
                            //和统计
                        }
                        //和统计
                    }
                }
                else
                {
                    Thread.Sleep(100);
                    Model.V3Infos.TaskDb[taskid].TaskStatusStr = "剩余数据为" + dbdatacount + "条，达到发布需求，跳过抓取过程!";
                    V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：剩余数据为" + dbdatacount + "条，达到发布需求，跳过抓取过程！[/c]");
                    return;
                }

            }
        }
        // 根据关键字和任务id执行分步骤

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
        public static string GetTitleFromArticle(bool isRandom, string ArticleContent)
        {
            ArrayList titlelist = new ArrayList();
            string title = "无标题";
            string article = ArticleContent;

            article = article.Replace("\r", "|");
            article = article.Replace("\n", "|");
            article = article.Replace(".", "|");
            article = article.Replace("。", "|");
            article = article.Replace("<p>", "|");
            article = article.Replace("</p>", "|");
            string[] titles = article.Split('|');
            for (int i = 0; i < titles.Length; i++)
            {

                if (titles[i].Contains("，")) { titles[i] = titles[i].Remove(titles[i].IndexOf("，")); }
                if (titles[i].Contains(",")) { titles[i] = titles[i].Remove(titles[i].IndexOf(",")); }
                if (titles[i].Length < 45 && titles[i].Length > 8)
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
            return title.Trim().Replace(";", "").Replace("\'", "").Replace("；", "").Replace("、", "").Replace("”", "").Replace("“", "");
        }
        public static string getRandomStringFromMaterial(int taskid)//获取一个随机关键字
        {
            Random ran = new Random();
            if (Model.V3Infos.KeywordDb[Model.V3Infos.TaskDb[taskid].KeywordDbId.ToString()].Keywords.Count == 0)//如果没有关键字
            {
                return "";
            }
            int ii = ran.Next(0, Model.V3Infos.KeywordDb[Model.V3Infos.TaskDb[taskid].KeywordDbId.ToString()].Keywords.Count);
            return Model.V3Infos.KeywordDb[Model.V3Infos.TaskDb[taskid].KeywordDbId.ToString()]._Keywords[ii].ToString();

        }
        static Model.Model_Article yuLiaoGetSuiJi(int taskid, Searcher searcher, ScoreDoc[] hit)
        {
            try
            {
                string s = "";
                Model.Model_Article art = new Model.Model_Article();
                art.DataObject = new Dictionary<int, string>()
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
                Random r = new Random();
                int duanlushu = r.Next(5, 10);
                ArrayList chk = new ArrayList();
                for (int i = 0; i < duanlushu; i++)
                {
                    int hitid = r.Next(0, hit.Length);
                    if (!chk.Contains(hitid))
                    {
                        chk.Add(hitid);
                    }
                    else
                    {
                        continue;
                    }
                    Document doc = searcher.Doc(hit[hitid].doc);
                    s += doc.Get("content").ToString();
                }
                art.DataObject[0] = GetTitleFromArticle(true, s);
                art.DataObject[1] = s;
                art.DataObject[29] = getRandomStringFromMaterial(taskid);
                return art;
            }
            catch { return null; }

        }
        static Model.Model_Article yuLiaoGetkeyword(int taskid, string keyword)
        {

            try
            {
                string s = "";
                Model.Model_Article art = new Model.Model_Article();
                art.DataObject = new Dictionary<int, string>()
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
                string path = Model.V3Infos.TaskDb[taskid].YuLiaoKuPath;
                ArrayList houxuantitle = new ArrayList(); ;
                int duan = 0;
                IndexSearcher search = new IndexSearcher(path);
                Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_CURRENT);

                Lucene.Net.QueryParsers.QueryParser qp = new Lucene.Net.QueryParsers.QueryParser("content", analyzer);
                Query query = qp.Parse(keyword);

                //Query query = queryParser.Parse(keywords);


                TimeSpan tsStart = new TimeSpan(DateTime.Now.Ticks);

                IndexReader reader = IndexReader.Open(path, true);//单个索引库

                Searcher searcher = new IndexSearcher(reader);
                TopScoreDocCollector collector = TopScoreDocCollector.create(Model.V3Infos.TaskDb[taskid].YuJuShuLiang, false);
                searcher.Search(query, collector);
                ScoreDoc[] hit = collector.TopDocs().scoreDocs;
                if (hit.Length == 0) { V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：关键词'"+keyword+"'本次没有获取到数据！[/c]"); return null; }
                Random r = new Random();
                for (int i = 0; i < hit.Length; i++)
                {
                    if (duan > 5) { break; }
                    if (r.Next(0, 100) % 20 == 0)
                    {
                        Document doc = searcher.Doc(hit[i].doc);
                        s += doc.Get("content").ToString();
                        duan++;
                    }
                }
                TimeSpan tsEnd = new TimeSpan(DateTime.Now.Ticks);
                TimeSpan ts = tsEnd.Subtract(tsStart).Duration();
                art.DataObject[0] = GetTitleFromArticle(true, s);

                art.DataObject[1] = s;
                art.DataObject[29] = keyword;


                reader.Close(); search.Close(); GC.Collect();

                return art;
            }
            catch
            {
                return null;
            
            }

        }
        #endregion
        #endregion

        #region  发布过程方法
        static void send(int taskid)
        {
            // Queue que = new Queue();
            Thread.Sleep(10);
            // V3.Command.Log.LogNewline("任务【" + taskid + "】：正在加载本批次所需发布的数据信息...", 1);
            int datacount = V3.Bll.DbBll.GetArticleOkCount(Model.V3Infos.TaskDb[taskid].ArticleDbId.ToString(), Model.V3Infos.TaskDb[taskid].HashDbId.ToString());
            //  que = V3.Command.DbBll.GetArticleIds(V3Model.V3Infos.TaskDb[taskid].ArticleDbId.ToString(), V3Model.V3Infos.TaskDb[taskid].HashDbId.ToString(), V3Model.V3Infos.TaskDb[taskid].Picinumber, V3Model.V3Infos.TaskDb[taskid].DataGetmodel, ref datacount);
            V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：当前采集库剩余数据为“" + datacount + "，本次需要发布" + Model.V3Infos.TaskDb[taskid].Picinumber + "条,开始发布过程！[/c]");
            V3.Bll.PostBll bll = new PostBll(Model.V3Infos.MainDb.MyModels[Model.V3Infos.SendPointDb[Model.V3Infos.TaskDb[taskid].PointId].PostModel], Model.V3Infos.TaskDb[taskid].PointId, taskid);
            string tempstr = "";
            bll.Login(false, ref tempstr);
            int countSendok = 0;
            if (bll.LoginOk)
            {
                for (int i = 0; i < Model.V3Infos.TaskDb[taskid].Picinumber; i++)
                {
                   
                    

                    if (Model.V3Infos.TaskCancelToken[taskid].IsCancellationRequested)
                    {
                        break;
                    }
                    Model.Model_Article article = V3.Common.ArticleBll.GetSendArticle(Model.V3Infos.TaskDb[taskid].ArticleDbId.ToString(), Model.V3Infos.TaskDb[taskid].DataGetmodel);
                  
                    if (article == null)
                    {
                        V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：当前文章库已没有任何可发数据，任务结束发布过程！[/c]");
                        break;
                    }
                    else
                    {
                       
                        if (Model.V3Infos.TaskDb[taskid].MoreAccountUseModel)
                            bll.Login(false, ref tempstr);
                        //对文章进行处理
                        string r = MarkNewArticle(ref article, taskid);


                        //对免费版加入广告代码
                        string [] ads=new string[6];
                        ads[0] = "<p><b>本文章由站群引擎V3免费版发布。站群，管理网站如此简单!</b></p>";
                        ads[1] = "<p><b>本文章由站群引擎V3免费版发布。站群，站群管理更加专业!</b></p>";
                        ads[2] = "<p><b>本文章由站群引擎V3免费版发布。站群，最强大的站群管理系统!</b></p>";
                        ads[3] = "<p><b>本文章由站群引擎V3免费版发布。站群，轻松维护网站，获取流量更加轻松!</b></p>";
                        ads[4] = "<p><b>本文章由站群引擎V3免费版发布。站群，轻松采集，灵活处理，完美发布!</b></p>";
                        ads[5] = "<p><b>本文章由站群引擎V3免费版发布。站群，让您不愁没有流量!</b></p>";
                        

                        if (Program.Level<3)
                        {

                            Random rad = new Random();
                            Random rads=new Random();
                            if(rad.Next(0,3)==1)
                            {

                                article.DataObject[1] = article.DataObject[1] + ads[rads.Next(0,6)]+"【www.xiake.org】";
                              
                            }
                        }

                    
                        if (r.Contains("含有敏感字符"))
                        {
                            V3.Common.Log.LogNewline("[c11]任务【" + taskid + "/】：" + r + "”,任务队列中尚剩余“" + (Model.V3Infos.TaskDb[taskid].Picinumber - i - 1) + "”条待发数据！[/c]");
                        }
                        else
                        {
                            Model.V3Infos.TaskDb[taskid].TaskStatusStr = "正在发布数据 " + Substring(article.DataObject[0], 15);
                            V3.Common.Log.LogNewline("[c11]任务【" + taskid + "】：正在发布数据“" + Substring(article.DataObject[0], 15) + "”,任务队列中尚剩余“" + (Model.V3Infos.TaskDb[taskid].Picinumber - i - 1) + "”条待发数据！[/c]");
                            if (sendStart(taskid, article, bll, false))
                            {
                                countSendok++;
                                ArticleBll.dbdelete(article.Id, Model.V3Infos.ArticleDb[Model.V3Infos.TaskDb[taskid].ArticleDbId.ToString()]);
                            }
                        }
                    }
                    if (i != Model.V3Infos.TaskDb[taskid].Picinumber - 1)
                    {
                        if (Model.V3Infos.TaskDb[taskid].TaskMoshi == 0 && Model.V3Infos.TaskDb[taskid].Fabujiange>0)
                        {
                            int sleeptime = Model.V3Infos.TaskDb[taskid].Fabujiange;
                            V3.Common.Log.LogNewline(
                                "[c11]任务【" + taskid + "/】：暂停" + sleeptime + "秒，将于" +
                                DateTime.Now.AddSeconds(sleeptime) + "发布下一篇文章！[/c]");

                            Model.V3Infos.TaskDb[taskid].TaskStatusStr = "暂停" + sleeptime + "秒，将于" +
                                                                         DateTime.Now.AddSeconds(sleeptime) + "发布下一篇文章！";

                            int sleep = 0;
                            while (true)
                            {
                                sleep++;
                                if (sleep >= sleeptime || Model.V3Infos.TaskCancelToken[taskid].IsCancellationRequested)
                                {
                                    break;
                                }
                                Thread.Sleep(1000);
                            }

                        }
                        if (Model.V3Infos.TaskDb[taskid].TaskMoshi == 1)
                        {
                            int sleeptime = 0;
                            sleeptime = Model.V3Infos.TaskDb[taskid].Fabujiange/Model.V3Infos.TaskDb[taskid].Picinumber;
                            V3.Common.Log.LogNewline(
                                "[c11]任务【" + taskid + "/】：暂停" + sleeptime + "秒，将于" +
                                DateTime.Now.AddSeconds(sleeptime) + "发布下一篇文章！[/c]");
                            Model.V3Infos.TaskDb[taskid].TaskStatusStr = "暂停" + sleeptime + "秒，将于" +
                                                                         DateTime.Now.AddSeconds(sleeptime) + "发布下一篇文章！";
                            int sleep = 0;
                            while (true)
                            {
                                sleep++;
                                if (sleep >= sleeptime || Model.V3Infos.TaskCancelToken[taskid].IsCancellationRequested)
                                {
                                    break;
                                }
                                Thread.Sleep(1000);
                            }


                        }
                        else if (Model.V3Infos.TaskDb[taskid].TaskMoshi == 2)
                        {
                            Random r = new Random();
                            int ir = r.Next(0, 30);
                            int sleeptime = 0;

                            if (ir%2 == 0)
                            {
                                sleeptime = Model.V3Infos.TaskDb[taskid].Fabujiange +
                                            (int) (Model.V3Infos.TaskDb[taskid].Fabujiange*(double) ir/100);
                            }
                            else
                            {
                                sleeptime = Model.V3Infos.TaskDb[taskid].Fabujiange -
                                            (int) (Model.V3Infos.TaskDb[taskid].Fabujiange*(double) ir/100);
                                if (sleeptime < 0)
                                {
                                    sleeptime = 1;
                                }
                            }
                            V3.Common.Log.LogNewline(
                                "[c11]任务【" + taskid + "/】：暂停" + sleeptime + "秒，将于" +
                                DateTime.Now.AddSeconds(sleeptime) + "发布下一篇文章！[/c]");
                            Model.V3Infos.TaskDb[taskid].TaskStatusStr = "暂停" + sleeptime + "秒，将于" +
                                                                         DateTime.Now.AddSeconds(sleeptime) + "发布下一篇文章！";
                            int sleep = 0;
                            while (true)
                            {
                                sleep++;
                                if (sleep >= sleeptime || Model.V3Infos.TaskCancelToken[taskid].IsCancellationRequested)
                                {
                                    break;
                                }
                                Thread.Sleep(1000);
                            }
                        }
                    }
                }
            }
            else
            {
                V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：任务没有登录成功，跳过发布过程！[/c]");
                Model.V3Infos.TaskDb[taskid].TaskStatusStr = "任务没有登录成功，跳过发布过程";
            }
            V3.Bll.TaskBll.SaveTask(taskid);
            if (Model.V3Infos.TaskCancelToken[taskid].IsCancellationRequested)
            {
                V3.Common.Log.LogNewline("[c12]任务【" + taskid + "】：发布过程被停止，成功发布了" + countSendok + "条数据！[/c]");
                Model.V3Infos.TaskDb[taskid].TaskStatusStr = "发布过程被停止，成功发布了" + countSendok + "条数据";
            }
            else
            {
                V3.Common.Log.LogNewline("[c12]任务【" + taskid + "】：本批次发布过程结束，成功发布了" + countSendok + "条数据！[/c]");
                Model.V3Infos.TaskDb[taskid].TaskStatusStr = "本批次发布过程结束，成功发布了" + countSendok + "条数据";

            }
           
        }
        static bool sendStart(int taskid, Model.Model_Article article, V3.Bll.PostBll bll, bool isNosave)
        {
            Model.V3Infos.TaskDb[taskid].TaskStatusStr = "正在发布：" + article.DataObject[0].ToString();
            bll.article = article;
            string tempstr = "";
            string linkstr = "";

          

            string jieguo = bll.Send(false, ref tempstr, ref linkstr);
            if (jieguo == "SendOK")
            {
                Model.V3Infos.TaskDb[taskid].CountthisPost++;
                Model.V3Infos.TaskDb[taskid].CountAllPost++;
                Model.V3Infos.TaskDb[taskid].TaskStatusStr = "已经成功发布名为“" + Substring(article.DataObject[0], 15) + "”的数据";
                V3.Common.Log.LogNewline("[c12]任务【" + taskid + "】：成功发布文章“" + Substring(article.DataObject[0], 15) + "”[/c]");
              
                return true;
            }
            else
            {
                Model.V3Infos.TaskDb[taskid].TaskStatusStr = "发布名为“" + Substring(article.DataObject[0], 15) + "”的数据时，失败了";
                V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：“" + Substring(article.DataObject[0], 15) + "”发布失败，原因：" + jieguo + "[/c]");
             
                return false;
            }
        }
        #endregion

        #region 替换，过滤，混淆，伪原创 ,用户定制处理方法
       public static  string geregex(string html, string regex, int num, bool msg)
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

       public static   string weibusuijicharu(int taskid)
        {


            string result = null;
            try
            {
                if (xEngine.License.MyLicense.Custom.Contains("wenzhangmoweicharu"))
                {
                    string gonggongkustr = geregex(Model.V3Infos.TaskDb[taskid].Canshu, "(?<=<wenzhangmoweicharu>).*?(?=</wenzhangmoweicharu>)", 1, false);
                    if (gonggongkustr.Split('|').Length >= 2)
                    {
                        result = gonggongkustr;

                    }
                }
            }
            catch { }
            return result;

        }

       public static string getRandomStringFromarticle(string  DBarticle_id)//获取一个随机文章内容
        {
            Random ran=new Random();
            try
            {
                if (Model.V3Infos.ArticleDb[DBarticle_id].DataCount == 0)//如果没有文章
                {
                    Log.LogNewlineNosave("内容处理：无法随机获取一篇文章！因为文章库里无内容！");
                    return "";
                }
              


                return V3.Common.ArticleBll.GetrandomArticle(DBarticle_id,3).DataObject[1].ToString();
            }
            catch
            {
                Log.LogNewlineNosave("内容处理：随机获取一篇文章时出错。");
                return "";
            }

        }

       static string MarkNewArticle(ref Model.Model_Article art, int taskid)
        {
            try
            {
                Model.V3Infos.TaskDb[taskid].TaskStatusStr = "正在处理" + art.DataObject[0].ToString();
                Model.V3Infos.TaskDb[taskid].Links.Clear();
                Common.SetArticle s = new SetArticle();
                s.Task = Model.V3Infos.TaskDb[taskid];
                s.art = art;
                s.chuli();
                return s.minganguolv();
            }
            catch (Exception error)
            {
                V3.Common.Log.LogNewline("[c14]任务【" + taskid + "】：文章处理时出错，将跳过处理，原因：" + error + "[/c]");

                return "";
            }
        }
       #endregion

        #region 辅助方法
        /// <summary>
        /// 增加标题哈希值
        /// </summary>
        /// <param name="taskid"></param>
        /// <param name="txt"></param>
        static void AddTitleHash(int taskid, string txt)
        {
            lock ("addtitle")
            {
                try
                {
                    if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\HashDb\\" + Model.V3Infos.TaskDb[taskid].HashDbId + "_T\\"))
                        Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\HashDb\\" + Model.V3Infos.TaskDb[taskid].HashDbId + "_T\\");
                    System.IO.File.Create(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\HashDb\\" + Model.V3Infos.TaskDb[taskid].HashDbId + "_T\\" + txt);
                    Model.V3Infos.HashDb[Model.V3Infos.TaskDb[taskid].HashDbId.ToString()].DataCount++;
                }
                catch
                { }
            }
        }
        /// <summary>
        /// 增加内容哈希值
        /// </summary>
        /// <param name="taskid"></param>
        /// <param name="txt"></param>
        static void AddContentHash(int taskid, string txt)
        {
            lock ("addcontent")
            {
                try
                {
                    if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\HashDb\\" + Model.V3Infos.TaskDb[taskid].HashDbId + "_C\\"))
                        Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\HashDb\\" + Model.V3Infos.TaskDb[taskid].HashDbId + "_C\\");
                    System.IO.File.Create(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\HashDb\\" + Model.V3Infos.TaskDb[taskid].HashDbId + "_C\\" + txt);
                    Model.V3Infos.HashDb[Model.V3Infos.TaskDb[taskid].HashDbId.ToString()].DataCount++;
                }
                catch
                { }
            }
        }
        /// <summary>
        /// 增加地址哈希值
        /// </summary>
        /// <param name="taskid"></param>
        /// <param name="txt"></param>
        static void AddUrlHash(int taskid, string txt)
        {
            lock ("addurl")
            {
                try
                {
                    if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\HashDb\\" + Model.V3Infos.TaskDb[taskid].HashDbId + "_U\\"))
                        Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\HashDb\\" + Model.V3Infos.TaskDb[taskid].HashDbId + "_U\\");
                    System.IO.File.Create(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\HashDb\\" + Model.V3Infos.TaskDb[taskid].HashDbId + "_U\\" + txt);
                    Model.V3Infos.HashDb[Model.V3Infos.TaskDb[taskid].HashDbId.ToString()].DataCount++;
                    
                }
                catch
                { }
            }
        }
        /// <summary>
        /// 判断标题哈希值
        /// </summary>
        /// <param name="taskid"></param>
        /// <param name="txt"></param>
        /// <returns></returns>
        static bool CheckTitleHash(int taskid, string txt)
        {
            bool result = false;
            try
            {
                result = System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\HashDb\\" + Model.V3Infos.TaskDb[taskid].HashDbId + "_T\\" + txt);
            }
            catch { return false; }
            return result;
        }
        /// <summary>
        /// 判断内容哈希值
        /// </summary>
        /// <param name="taskid"></param>
        /// <param name="txt"></param>
        /// <returns></returns>
        static bool CheckContentHash(int taskid, string txt)
        {
            bool result = false;
            try
            {
                result = System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\HashDb\\" + Model.V3Infos.TaskDb[taskid].HashDbId + "_C\\" + txt);
            }
            catch { return false; }
            return result;
        }
        /// <summary>
        /// 判断地址哈希值
        /// </summary>
        /// <param name="taskid"></param>
        /// <param name="txt"></param>
        /// <returns></returns>
        static bool CheckUrlHash(int taskid, string txt)
        {
            bool result = false;
            try
            {
                result = System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\HashDb\\" + Model.V3Infos.TaskDb[taskid].HashDbId + "_U\\" + txt);
            }
            catch { return false; }
            return result;
        }

        /// <summary>
        /// 截断长字符
        /// </summary>
        /// <param name="str">输入字符</param>
        /// <param name="lenght">截断长度</param>
        /// <returns></returns>
        static string Substring(string str, int lenght)
        {
            return (str.Trim().Length > lenght) ? str.Trim().Substring(0, lenght) + "..." : str.Trim();
        }
        /// <summary>
        /// 截断长字符(从后往前）
        /// </summary>
        /// <param name="str">输入字符</param>
        /// <param name="lenght">截断长度</param>
        /// <returns></returns>
        static string Substring2(string str, int lenght)
        {
            try
            {
                return (str.Trim().Length > lenght) ? "..." + str.Trim().Substring(str.Length - lenght, lenght - 1) : str.Trim();
            }
            catch { return str; }
        }

        static xEngine.Model.Execute.Http.XCookieManager GetCookie(int taskid)
        {
            xEngine.Model.Execute.Http.XCookieManager cookie = new xEngine.Model.Execute.Http.XCookieManager();
            if (Model.V3Infos.TaskDb[taskid].Getmajiamodel)
            {
                xEngine.Execute.Http execute = new xEngine.Execute.Http();
                execute.CookieManager = cookie;
                execute.CookieAddStr(Model.V3Infos.TaskDb[taskid].Getmajia);
            }
            else
            {
                xEngine.Execute.Http execute = new xEngine.Execute.Http();
                execute.CookieManager = cookie;
                execute.CookieAddStr(Model.V3Infos.MainDb.MyModels[Model.V3Infos.TaskDb[taskid].GetModel].GetMajia);
            }
            return cookie;
        }
        #endregion

      
    }
}
