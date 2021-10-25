#region

using System;
using System.Linq;
using xEngine.Model.Common;

#endregion

namespace xEngine.Common
{
    public static class KeywordDbHelper
    {
        private static readonly Random Random = new Random();

        public static KeywordDb LoadKeywordDb(string file)
        {
            var db = FileHelper.LoadModel<KeywordDb>(file);
            for (var i = 0; i < db.AllKeyword.Length; i++)
            {
                db.IndexDic.Add(db.AllKeyword[i], i);
            }
            return db;
        }

        public static string GetKeyword(KeywordDb db, string mainkeyword = null)
        {
            if (mainkeyword == null)
            {
                return db.AllKeyword[Random.Next(0, db.AllKeyword.Length)];
            }
            else
            {
                if (db.HotKeywordCache.ContainsKey(mainkeyword))
                {
                    var value = db.HotKeywordCache.Get(mainkeyword);

                    return
                        db.AllKeyword[
                            value.Length > 0
                                ? value[Random.Next(0, value.Length)]
                                : Random.Next(0, db.AllKeyword.Length)];
                }
                else
                {
                    var result = (from d in db.AllDictionary
                        where (
                            d.Key.Contains(mainkeyword) //主词包含
                            || d.Value.Contains(mainkeyword) //父词包含
                            ||
                            (db.AllDictionary.ContainsKey(mainkeyword) &&
                             (d.Value.Contains(db.AllDictionary[mainkeyword]) ||
                              d.Key.Contains(db.AllDictionary[mainkeyword]))))
                            //父词的父词包含
                              && d.Key != mainkeyword
                        //不要父词
                        select d).ToArray();
                    var strs = new int[result.Length];
                    for (var i = 0; i < result.Length; i++)
                    {
                        strs[i] = db.IndexDic[result[i].Key];
                    }

                    db.HotKeywordCache.Add(mainkeyword, strs);

                    var value = db.HotKeywordCache.Get(mainkeyword);

                    return
                        db.AllKeyword[
                            value.Length > 0
                                ? value[Random.Next(0, value.Length)]
                                : Random.Next(0, db.AllKeyword.Length)];
                }
            }
        }
    }
}