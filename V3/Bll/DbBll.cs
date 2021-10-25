using System;
using System.Collections.Generic;
using System.Text;
using V3;
using Model;
using System.Collections;
using System.Reflection;
using System.Windows.Forms;
using System.Data;
using System.IO;
using V3.Common;
namespace V3.Bll
{
    class DbBll
    {
        public static Random R = new Random();
        /// <summary>
        /// 新增一个库
        /// </summary>
        /// <param name="dbtype">1文章库 2关键词库 3哈希库 4替换库 5链接库</param>
        /// <param name="dbname">库名</param>
        /// <param name="typeid">组别</param>
        /// <returns></returns>
        public static int AddDb(int dbtype, string dbname, string typeid)
        {
            int result = 0;
            if (dbtype == 1)
            {
                #region 文章库
                ArticleDB tempdb = new ArticleDB();
                tempdb.Name = dbname;
                tempdb.Type = typeid;



                int i = 0;

                foreach (KeyValuePair<string, ArticleDB> s in Model.V3Infos.ArticleDb)
                {


                    if (Convert.ToInt32(s.Key) > i)
                    {
                        i = Convert.ToInt32(s.Key);
                    }


                }
                Model.V3Infos.MainDb.Articledbid = i;
                Model.V3Infos.MainDb.Articledbid++;
                tempdb.Id = Model.V3Infos.MainDb.Articledbid.ToString();
                Model.V3Infos.ArticleDb.Add(tempdb.Id, tempdb);
                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\ArticleDb\\" + tempdb.Id + "\\"))
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\ArticleDb\\" + tempdb.Id + "\\");
                V3Helper.saveArticleDb();
                result = Convert.ToInt32(tempdb.Id);

                #endregion
            }
            else if (dbtype == 2)
            {
                #region 关键词库
                KeywordDB tempdb = new KeywordDB();
                tempdb.Name = dbname;
                tempdb.Type = typeid;


                int i = 0;
                foreach (KeyValuePair<string, KeywordDB> s in Model.V3Infos.KeywordDb)
                {



                    if (Convert.ToInt32(s.Key) > i) { i = Convert.ToInt32(s.Key); }



                }
                Model.V3Infos.MainDb.Keyworddbid = i;
                Model.V3Infos.MainDb.Keyworddbid++;
                tempdb.Id = Model.V3Infos.MainDb.Keyworddbid.ToString();
                Model.V3Infos.KeywordDb.Add(tempdb.Id, tempdb);
                V3Helper.saveKeywordDb();
                result = Convert.ToInt32(tempdb.Id);
                #endregion
            }
            else if (dbtype == 3)
            {
                #region 哈希库
                HashDB tempdb = new HashDB();

                tempdb.Name = dbname;
                tempdb.Type = typeid;



                int i = 0;
                foreach (KeyValuePair<string, HashDB> s in Model.V3Infos.HashDb)
                {



                    if (Convert.ToInt32(s.Key) > i) { i = Convert.ToInt32(s.Key); }



                }
                Model.V3Infos.MainDb.Hashdbid = i;
                V3Infos.MainDb.Hashdbid++;
                tempdb.Id = V3Infos.MainDb.Hashdbid.ToString(); ;
                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\HashDb\\" + tempdb.Id + "_U\\"))
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\HashDb\\" + tempdb.Id + "_U\\");
                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\HashDb\\" + tempdb.Id + "_T\\"))
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\HashDb\\" + tempdb.Id + "_T\\");
                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\HashDb\\" + tempdb.Id + "_C\\"))
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\HashDb\\" + tempdb.Id + "_C\\");
                Model.V3Infos.HashDb.Add(tempdb.Id, tempdb);
                V3Helper.saveHashDb();
                result = Convert.ToInt32(tempdb.Id);
                #endregion
            }
            else if (dbtype == 4)
            {
                #region 替换库
                ReplaceDB tempdb = new ReplaceDB();

                tempdb.Name = dbname;
                tempdb.Type = typeid;
                tempdb.Words.Add("我的电脑→我的计算机");
                tempdb.Words.Add("天气凉了→天气冷了→天气变冷了");
                tempdb.Words.Add("13\\d{9}→13888889999");


                int i = 0;
                foreach (KeyValuePair<string, ReplaceDB> sk in Model.V3Infos.ReplaceDb)
                {


                    if (Convert.ToInt32(sk.Key) > i) { i = Convert.ToInt32(sk.Key); }

                }
                Model.V3Infos.MainDb.Replacedbid = i;
                V3Infos.MainDb.Replacedbid++;
                tempdb.Id = V3Infos.MainDb.Replacedbid.ToString();
                result = Convert.ToInt32(tempdb.Id);
                Model.V3Infos.ReplaceDb.Add(tempdb.Id, tempdb);
                V3Helper.saveReplaceDb();
                #endregion
            }
            else if (dbtype == 5)
            {
                #region 链接库
                LinkDB tempdb = new LinkDB();

                tempdb.Name = dbname;
                tempdb.Type = typeid;

                int i = 0;
                foreach (KeyValuePair<string, LinkDB> sk in Model.V3Infos.LinkDb)
                {
                    if (Convert.ToInt32(sk.Key) > i)
                    {
                        i = Convert.ToInt32(sk.Key);
                    }
                }
                    Model.V3Infos.MainDb.Linkdbid = i;
                    V3Infos.MainDb.Linkdbid++;
                    tempdb.Id = V3Infos.MainDb.Linkdbid.ToString();
                    Model.V3Infos.LinkDb.Add(tempdb.Id, tempdb);
                    result = Convert.ToInt32(tempdb.Id);
                    V3Helper.saveLinkDb();
                #endregion
                

             
            }   return result;
        }

        /// <summary>
        /// 获取分页显示信息
        /// </summary>
        /// <param name="dbtype">1文章库 2关键词库 3哈希库 4替换库 5链接库</param>
        /// <param name="keyword">查找关键字</param>
        /// <returns></returns>
        public static ArrayList GetDbPageList(int dbtype, string keyword)
        {
            ArrayList temp = new ArrayList();
            if (dbtype == 1)
            {
                #region 文章库
                try
                {
                    string typesearch = "NOGROUP";
                    if (keyword.ToLower().Trim().Contains("group:"))
                    {
                        typesearch = keyword.Split(':')[1];
                    }
                    foreach (KeyValuePair<string, Model.ArticleDB> ss in Model.V3Infos.ArticleDb)
                    {
                        if (typesearch != "NOGROUP")
                        {
                            if (ss.Value.Type == typesearch || typesearch == "AllGroup")
                            {
                                temp.Add(ss.Value);
                            }
                        }
                        else
                        {
                            if (ss.Value.Id.Contains(keyword) || ss.Value.Name.Contains(keyword))
                                temp.Add(ss.Value);
                        }
                    }
                }
                catch (Exception ex)
                {
                    V3.Common.Log.LogNewline("[c14][系统]获取文章库分页信息出错，详细信息:" + ex.Message+"[/c]");
                }
                #endregion
            }
            else if (dbtype == 2)
            {
                #region 关键词库
                try
                {

                    string typesearch = "NOGROUP";
                    if (keyword.ToLower().Trim().Contains("group:"))
                    {
                        typesearch = keyword.Split(':')[1];
                    }
                    foreach (KeyValuePair<string, Model.KeywordDB> ss in Model.V3Infos.KeywordDb)
                    {
                        if (typesearch != "NOGROUP")
                        {
                            if (ss.Value.Type == typesearch || typesearch == "AllGroup")
                            {
                                temp.Add(ss.Value);
                            }
                        }
                        else
                        {
                            if (ss.Value.Id.Contains(keyword) || ss.Value.Name.Contains(keyword))
                                temp.Add(ss.Value);
                        }
                    }
                }
                catch (Exception ex)
                {
                    V3.Common.Log.LogNewline("[c14][系统]获取关键词数据库信息出错，详细信息:" + ex.Message+"[/c]");
                }
                #endregion
            }
            else if (dbtype == 3)
            {
                #region 哈希库
                try
                {
                    string typesearch = "NOGROUP";
                    if (keyword.ToLower().Trim().Contains("group:"))
                    {
                        typesearch = keyword.Split(':')[1];
                    }
                    foreach (KeyValuePair<string, Model.HashDB> ss in Model.V3Infos.HashDb)
                    {
                        if (typesearch != "NOGROUP")
                        {
                            if (ss.Value.Type == typesearch || typesearch == "AllGroup")
                            {
                                temp.Add(ss.Value);
                            }
                        }
                        else
                        {
                            if (ss.Value.Id.Contains(keyword) || ss.Value.Name.Contains(keyword))
                                temp.Add(ss.Value);
                        }
                    }

                }
                catch (Exception ex)
                {
                    V3.Common.Log.LogNewline("[c14][系统]获取哈希数据库信息 出错，详细信息:" + ex.Message+"[/c]");
                }

                #endregion
            }
            else if (dbtype == 4)
            {
                #region 替换库
                try
                {
                    string typesearch = "NOGROUP";
                    if (keyword.ToLower().Trim().Contains("group:"))
                    {
                        typesearch = keyword.Split(':')[1];
                    }
                    foreach (KeyValuePair<string, Model.ReplaceDB> ss in Model.V3Infos.ReplaceDb)
                    {
                        if (typesearch != "NOGROUP")
                        {
                            if (ss.Value.Type == typesearch || typesearch == "AllGroup")
                            {
                                temp.Add(ss.Value);
                            }
                        }
                        else
                        {
                            if (ss.Value.Id.Contains(keyword) || ss.Value.Name.Contains(keyword))
                                temp.Add(ss.Value);
                        }
                    }
                }
                catch (Exception ex)
                {
                    V3.Common.Log.LogNewline("[c14][系统]获取替换库库信息 出错，详细信息:" + ex.Message+"[/c]");
                }
                #endregion
            }
            else if (dbtype == 5)
            {
                #region 链接库
                try
                {

                    string typesearch = "NOGROUP";
                    if (keyword.ToLower().Trim().Contains("group:"))
                    {
                        typesearch = keyword.Split(':')[1];
                    }
                    foreach (KeyValuePair<string, Model.LinkDB> ss in Model.V3Infos.LinkDb)
                    {
                        if (typesearch != "NOGROUP")
                        {
                            if (ss.Value.Type == typesearch || typesearch == "AllGroup")
                            {
                                temp.Add(ss.Value);
                            }
                        }
                        else
                        {
                            if (ss.Value.Id.Contains(keyword) || ss.Value.Name.Contains(keyword))
                                temp.Add(ss.Value);
                        }
                    }
                }
                catch (Exception ex)
                {
                    V3.Common.Log.LogNewline("[c14][系统]获取链接库信息 出错，详细信息:" + ex.Message+"[/c]");
                }
                #endregion
            }
            return temp;
        }

        /// <summary>
        /// 删除数据库
        /// </summary>
        /// <param name="dbtype">1文章库 2关键词库 3哈希库 4替换库 5链接库</param>
        /// <param name="id">数据库id</param>
        /// <returns></returns>
        public static bool DbDelete(int dbtype, string id)
        {
            bool result = false;
            if (!CheckDbId(dbtype, id))
                return false;
            if (dbtype == 1)
            {
                #region 文章库
                try
                {
                    System.IO.Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\ArticleDb\\" + id + "\\", true);
                }
                catch { }
                result = true;
                if (result)
                    Model.V3Infos.ArticleDb.Remove(id);
                V3Helper.saveArticleDb();
                #endregion
            }
            else if (dbtype == 2)
            {
                #region 关键词库
                Model.V3Infos.KeywordDb.Remove(id);
                result = true;
                V3Helper.saveKeywordDb();
                #endregion
            }
            else if (dbtype == 3)
            {
                #region 哈希库
                try
                {
                    System.IO.Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\HashDb\\" + id + "_C\\",true);
                    System.IO.Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\HashDb\\" + id + "_T\\", true);
                    System.IO.Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\HashDb\\" + id + "_U\\", true);
                }
                catch { }
                Model.V3Infos.HashDb.Remove(id);
                V3Helper.saveHashDb();
                result = true;
                #endregion
            }
            else if (dbtype == 4)
            {
                #region 替换库

                Model.V3Infos.ReplaceDb.Remove(id);
                V3Helper.saveReplaceDb();
                result = true;
                #endregion
            }
            else if (dbtype == 5)
            {
                #region 链接库
                Model.V3Infos.LinkDb.Remove(id);
                V3Helper.saveLinkDb();
                result = true;
                #endregion
            }
            return result;
        }

        static bool CheckDbId(int dbtype, string id)
        {
            foreach (KeyValuePair<int, Model.SendPoint> s in Model.V3Infos.SendPointDb)
            {
                if (dbtype == 1)
                {
                    if (s.Value.ArticleDbID == Convert.ToInt32(id))
                    {
                        Log.LogNewline("[c14]删除文章库ID" + id + "失败了，因为发布点" + s.Value.name + "[" + s.Value.id + "]还在使用它！[/c]");
                        return false;
                    }
                }
                else if (dbtype == 2)
                {
                    if (s.Value.KeywordDbID == Convert.ToInt32(id))
                    {
                        Log.LogNewline("[c14]删除关键词库ID" + id + "失败了，因为发布点" + s.Value.name + "[" + s.Value.id + "]还在使用它！[/c]");
                        return false;
                    }
                }
                else if (dbtype == 3)
                {
                    if (s.Value.HashDbID == Convert.ToInt32(id))
                    {
                        Log.LogNewline("[c14]删除哈希库ID" + id + "失败了，因为发布点" + s.Value.name + "[" + s.Value.id + "]还在使用它！[/c]");
                        return false;
                    }
                }
                else if (dbtype == 4)
                {
                    if (s.Value.ReplaceDbid == Convert.ToInt32(id))
                    {
                        Log.LogNewline("[c14]删除替换库ID" + id + "失败了，因为发布点" + s.Value.name + "[" + s.Value.id + "]还在使用它！[/c]");
                        return false;
                    }
                }
                else if (dbtype == 5)
                {
                    if (s.Value.LinkDbID == Convert.ToInt32(id))
                    {
                        Log.LogNewline("[c14]删除链接库ID" + id + "失败了，因为发布点" + s.Value.name + "[" + s.Value.id + "]还在使用它！[/c]");
                        return false;
                    }
                }
            }

            foreach (KeyValuePair<int, Model.Task> s in Model.V3Infos.TaskDb)
            {
                if (dbtype == 1)
                {
                    if (s.Value.ArticleDbId == Convert.ToInt32(id))
                    {
                        Log.LogNewline("[c14]删除文章库ID" + id + "失败了，因为任务" + s.Value.TaskName + "[" + s.Value.id + "]还在使用它！[/c]");
                        return false;
                    }
                }
                else if (dbtype == 2)
                {
                    if (s.Value.KeywordDbId == Convert.ToInt32(id))
                    {
                        Log.LogNewline("[c14]删除关键词库ID" + id + "失败了，因为任务" + s.Value.TaskName + "[" + s.Value.id + "]还在使用它！[/c]");
                        return false;
                    }
                }
                else if (dbtype == 3)
                {
                    if (s.Value.HashDbId == Convert.ToInt32(id))
                    {
                        Log.LogNewline("[c14]删除哈希库ID" + id + "失败了，因为任务" + s.Value.TaskName + "[" + s.Value.id + "]还在使用它！[/c]");
                        return false;
                    }
                }
                else if (dbtype == 4)
                {
                    if (s.Value.ReplaceDbId == Convert.ToInt32(id))
                    {
                        Log.LogNewline("[c14]删除替换库ID" + id + "失败了，因为任务" + s.Value.TaskName + "[" + s.Value.id + "]还在使用它！[/c]");
                        return false;
                    }
                }
                else if (dbtype == 5)
                {
                    if (s.Value.LinkDbId == Convert.ToInt32(id))
                    {
                        Log.LogNewline("[c14]删除链接库ID" + id + "失败了，因为任务" + s.Value.TaskName + "[" + s.Value.id + "]还在使用它！[/c]");
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 重命名数据库
        /// </summary>
        /// <param name="dbtype">1文章库 2关键词库 3哈希库 4替换库 5链接库</param>
        /// <param name="id"></param>
        /// <param name="newname"></param>
        /// <param name="typeid"></param>
        /// <returns></returns>
        public static bool DbRename(int dbtype, string id, string newname, string typeid)
        {
            bool result = false;
            if (dbtype == 1)
            {
                #region 文章库

                Model.V3Infos.ArticleDb[id].Name = newname;
                Model.V3Infos.ArticleDb[id].Type = typeid;
                V3Helper.saveArticleDb();
                result = true;
                #endregion
            }
            else if (dbtype == 2)
            {
                #region 关键词库

                Model.V3Infos.KeywordDb[id].Name = newname;
                Model.V3Infos.KeywordDb[id].Type = typeid;
                V3Helper.saveKeywordDb();
                result = true;

                #endregion
            }
            else if (dbtype == 3)
            {
                #region 哈希库

                Model.V3Infos.HashDb[id].Name = newname;
                Model.V3Infos.HashDb[id].Type = typeid;
                V3Helper.saveHashDb();
                result = true;

                #endregion
            }
            else if (dbtype == 4)
            {
                #region 替换库
                Model.V3Infos.ReplaceDb[id].Name = newname;
                Model.V3Infos.ReplaceDb[id].Type = typeid;
                V3Helper.saveReplaceDb();
                result = true;
                #endregion
            }
            else if (dbtype == 5)
            {
                #region 链接库

                Model.V3Infos.LinkDb[id].Name = newname;
                Model.V3Infos.LinkDb[id].Type = typeid;
                V3Helper.saveLinkDb();
                result = true;
                #endregion
            }

            return result;
        }



        /// <summary>
        /// 保存数据库
        /// </summary>
        /// <param name="dbtype">1文章库 2关键词库 3哈希库 4替换库 5链接库</param>
        /// <param name="dbid"></param>
        /// <returns></returns>
        public static bool SaveDb(int dbtype, string dbid)
        {
            bool result = false;
            if (dbtype == 1)
            {
                #region 文章库
                V3Helper.saveArticleDb();
                #endregion
            }
            else if (dbtype == 2)
            {
                #region 关键词库
                V3Helper.saveKeywordDb();
                result = true;
                #endregion
            }
            else if (dbtype == 3)
            {
                #region 哈希库
                V3Helper.saveHashDb();
                result = true;
                #endregion
            }
            else if (dbtype == 4)
            {
                #region 替换库
                V3Helper.saveReplaceDb();
                result = true;
                #endregion
            }
            else if (dbtype == 5)
            {
                #region 链接库
                V3Helper.saveLinkDb();
                result = true;
                #endregion
            }
            return result;
        }

        public static int GetArticleOkCount(string articledbid, string hashdbid)
        {
            int result = 0;
            try
            {
                result = System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\ArticleDb\\" + articledbid + "\\").Length;
            }
            catch { }
            return result;
        }

        public static string GetRandomKeyword(string keyworddb_id) 
        {
            if (V3Infos.KeywordDb.ContainsKey(keyworddb_id))
            {
                int count = V3Infos.KeywordDb[keyworddb_id].Keywords.Count;
                if (count > 0)
                {
                    int index = R.Next(0, count);
                    return V3Infos.KeywordDb[keyworddb_id].Keywords[index];
                }
                else { return ""; }
            }
            else { return ""; }
        }
    }
}
