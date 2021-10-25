using System;
using System.Collections.Generic;
using System.Text;
using V3;
using Model;
using System.Collections;
using System.Reflection;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using V3.Common;
using xEngine.Model.Execute.Http;
using System.IO;
using System.Linq;

namespace V3.Common
{
    public static class ArticleBll
    {
        /// <summary>
        /// 获取文章库所有文章数量
        /// </summary>
        /// <returns></returns>
        public static int GetAllArticleCoun(string dbid)
        {
            if (!System.IO.Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\ArticleDb\\" + dbid + "\\"))
            {
                return 0;

            }
            else
            {
                System.IO.DirectoryInfo Dir =
                    new System.IO.DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\ArticleDb\\" + dbid +
                                                "\\");
                System.IO.FileInfo[] files = Dir.GetFiles();
                return files.Length;
            }
        }

        public static ArrayList GetAllArticle(string dbid)
        {
            ArrayList li = new ArrayList();
            if (!System.IO.Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\ArticleDb\\" + dbid + "\\"))
            { return null; }
            else
            {
                Model.Model_Article datamodel;
               
               string[] files = System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\ArticleDb\\" + dbid + "\\");
             
                for (int i = 0; i < files.Length; i++)
                {
                    string filename = files[i].Remove(files[i].LastIndexOf("."));
                    filename = filename.Substring(filename.LastIndexOf("\\") + 1);
                    object obj =xEngine.Common.XSerializable.BytesToObject<Model.Model_Article>(xFile.ReadFileNoBuff(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\ArticleDb\\" + dbid + "\\", filename));
                    if (obj != null)
                    {


                        datamodel = xEngine.Common.XSerializable.CloneObject<Model.Model_Article>(obj);
                        li.Add(datamodel.DataObject[1].ToString());

                    }

                }
                return li;
            }
        }

        /// <summary>
        /// 增加新文章
        /// </summary>
        /// <param name="dbmodel"></param>
        /// <param name="datamodel"></param>
        /// <returns></returns>
        public static bool addnewdata(Model.ArticleDB dbmodel, Model.Model_Article datamodel)
        {
            lock ("addnewdata" + dbmodel.Id)
            {
                bool result = false;

                try
                {
                    
                    datamodel.Id = Model.V3Infos.ArticleDb[dbmodel.Id].MaxID.ToString();
                    xFile.SaveFileNoBuff(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\ArticleDb\\" + dbmodel.Id + "\\", datamodel.Id, xEngine.Common.XSerializable.ObjectToBytes(datamodel));
                    result = true;
                    Model.V3Infos.ArticleDb[dbmodel.Id].MaxID++;

                }
                catch { }

                if (result)
                    Model.V3Infos.ArticleDb[dbmodel.Id].DataCount++;
                return result;
            }
           
        }
       

        /// <summary>
        /// 获取文章分页信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="dbmodel"></param>
        /// <returns></returns>
        public static ArrayList getpagelist(int page, Model.ArticleDB dbmodel)
        {
           
            if (!System.IO.Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\ArticleDb\\" + dbmodel.Id + "\\"))
                System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\ArticleDb\\" + dbmodel.Id + "\\");
            ArrayList temp = new ArrayList();
            string[] files = System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\ArticleDb\\" + dbmodel.Id + "\\");
            Array.Sort(files, Library.StrHelper.CompareFileName);
            Array.Reverse(files);
            for (int i = (page - 1) * Model.V3Infos.MainDb.PageNumber; i < (page) * Model.V3Infos.MainDb.PageNumber; i++)
            {
                if (i >= files.Length)
                    break;
                try
                {

                    object obj = xEngine.Common.XSerializable.BytesToObject<Model_Article>(System.IO.File.ReadAllBytes(files[i]));
                    if (obj != null)
                    {
                        Model.Model_Article temps = (Model.Model_Article)xEngine.Common.XSerializable.CloneObject<Model_Article>(obj);
                        temp.Add(temps);
                    }

                }
                catch (Exception ex)
                {
                    V3.Common.Log.LogNewline("[c14][系统]加载文章分页出错:" + ex.Message+"[/c]");
                }
            }
            return temp;
        }

        /// <summary>
        /// 清空指定的表
        /// </summary>
        /// <param name="dbmodel"></param>
        public static bool clearDB(Model.ArticleDB dbmodel)
        {
            bool result = false;
            try
            {
                string[] fs = System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\ArticleDb\\" + dbmodel.Id + "\\");
                for (int i = 0; i < fs.Length; i++)
                {
                    try
                    {
                        System.IO.File.Delete(fs[i]);
                    }
                    catch (Exception ee)
                    {
                        result = false;
                        V3.Common.Log.LogNewline("[c14][系统]清空文章库时出错，详细信息:" + ee.Message+"[/c]");
                    }

                }
                result = true;
            }
            catch (Exception ex)
            {
                V3.Common.Log.LogNewline("[c14][系统]清空文章库时出错，详细信息:" + ex.Message+"[/c]");
                result = false;
            }
            finally
            {

            }
            if (result)
                Model.V3Infos.ArticleDb[dbmodel.Id].DataCount = 0;
            Model.V3Infos.ArticleDb[dbmodel.Id].MaxID = 1;
            return result;
        }

        /// <summary>
        /// 删除指定id的文章
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dbmodel"></param>
        /// <returns></returns>
        public static bool dbdelete(string id, Model.ArticleDB dbmodel)
        {
            bool result = false;

            try
            {
                xFile.DelFile(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\ArticleDb\\" + dbmodel.Id + "\\", id);
                result = true;
            }
            catch (Exception ex)
            {
                V3.Common.Log.LogNewline("[c14][系统]删除文章时失败:" + ex.Message+"[/c]");
            }
            finally
            {

            }
            if (result)
                Model.V3Infos.ArticleDb[dbmodel.Id].DataCount--;
            return result;
        }

        /// <summary>
        /// 根据dataid和数据库获取到文章详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dbmodel"></param>
        /// <returns></returns>
        public static bool LoadData(ref Model.Model_Article datamodel, Model.ArticleDB dbmodel)
        {
            bool result = false;
            try
            {
                object obj = xEngine.Common.XSerializable.BytesToObject<Model.Model_Article>(xFile.ReadFileNoBuff(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\ArticleDb\\" + dbmodel.Id + "\\", datamodel.Id));
                if (obj != null)
                {
                    datamodel = xEngine.Common.XSerializable.CloneObject<Model.Model_Article>(obj);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                V3.Common.Log.LogNewline("[c14][系统]文章["+datamodel.Id+"]不存在:" + ex.Message+"[/c]");
            }
            return result;
        }

        /// <summary>
        /// 保存文章的修改
        /// </summary>
        /// <param name="datamodel"></param>
        /// <param name="dbmodel"></param>
        /// <returns></returns>
        public static bool SaveData(Model.Model_Article datamodel, Model.ArticleDB dbmodel)
        {
            bool result = false;
            try
            {
               
                datamodel.Db = dbmodel.Id;
                xFile.SaveFileNoBuff(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\ArticleDb\\" + dbmodel.Id + "\\", datamodel.Id.ToString(), xEngine.Common.XSerializable.ObjectToBytes(datamodel));
                result = true;
            }
            catch (Exception ex)
            {
                V3.Common.Log.LogNewline("[c14][系统]保存文章修改出错:" + ex.Message+"[/c]");
                result = false;
            }
            finally
            {

            }
            if (result)
                Model.V3Infos.ArticleDb[dbmodel.Id].DataCount++;
            return result;
        }


        public static Model.Model_Article GetSendArticle(string articledbid, int model)
        {
            lock ("getarticledb")
            {
                Model.Model_Article result = null;
                var files = System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\ArticleDb\\" + articledbid + "\\");
                if (files.Length == 0)
                    return null;
                List<string> fnames = new List<string>();
                for (int i = 0; i < files.Length; i++)
                {
                    fnames.Add(files[i]);
                }
                while (fnames.Count > 0)
                {
                    try
                    {
                        Random r = new Random();
                        string filename = "";
                        if (model == 3)
                        {
                            filename = fnames[r.Next(0, fnames.Count)];
                            fnames.Remove(filename);
                        }
                        else if (model == 2)
                        {
                            filename = files[files.Length - 1];
                            fnames.Remove(filename);
                        }
                        else
                        {
                            filename = fnames[0];
                            fnames.Remove(filename);
                        }
                        result =xEngine.Common.XSerializable.BytesToObject<Model_Article>(System.IO.File.ReadAllBytes(filename));
                        System.IO.File.Delete(filename);
                    }
                    catch
                    {
                        Log.LogNewline("[c14]加载文章时遇到一个错误，自动忽略读取下一篇！[/c]");
                    }
                    if (result != null&&result.DataObject.Count==30)
                        break;
                }
                V3Infos.ArticleDb[articledbid].DataCount--;
                return result;
            }
        }

        public static Model.Model_Article GetrandomArticle(string articledbid, int model)
        {
            lock ("getarticledb")
            {
                Model.Model_Article result = null;
                var files = System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\ArticleDb\\" + articledbid + "\\");
                if (files.Length == 0)
                    return null;
                Random r = new Random();
                string filename = "";
                if (model == 3)
                    filename = files[r.Next(0, files.Length)];
                else if (model == 2)
                    filename = files[files.Length - 1];
                else
                    filename = files[0];
                result = xEngine.Common.XSerializable.BytesToObject<Model_Article>(System.IO.File.ReadAllBytes(filename));
                return result;
            }
        }
        public static Model.Model_Article GetrandomArticleAndDel(string articledbid,ref string filename)
        {
            lock ("getarticledb")
            {
                Model.Model_Article result = null;
                var files = System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\ArticleDb\\" + articledbid + "\\");
                if (files.Length == 0)
                    return null;
                Random r = new Random();
               filename = files[r.Next(0, files.Length)];
               result = xEngine.Common.XSerializable.BytesToObject<Model_Article>(System.IO.File.ReadAllBytes(filename));
               System.IO.File.Delete(filename);
                return result;
            }
        }
    }
}
