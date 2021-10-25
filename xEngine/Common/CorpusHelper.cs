#region

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using xEngine.Model.Common;

#endregion

namespace xEngine.Common
{
    public class CorpusHelper
    {
        private static readonly Random random = new Random();
        private static readonly Regex juhao = new Regex("{句号语料}");
        private static readonly Regex gantanhao = new Regex("{感叹号语料}");
        private static readonly Regex wenhao = new Regex("{问号语料}");
        private static readonly Regex douhao = new Regex("{逗号语料}");
        private static readonly Regex qita = new Regex("{其他语料}");

        public static CorpusDb LoadCorpusDb(string filename)
        {
            var db = new CorpusDb();
            var lines = File.ReadAllLines(filename, Encoding.UTF8);
            foreach (var t in lines)
            {
                var last = t.Substring(t.Length - 1, 1).Trim();
                var neirong = t.Substring(0, t.Length - 1).Trim();

                var jz = new CorpusElement();
                switch (last)
                {
                    case "，":
                        jz.type = CEType.douhao;
                        jz.content = neirong;
                        jz.len = neirong.Length;
                        db.DouhaoList.Add(jz);
                        break;
                    case "。":
                        jz.type = CEType.juhao;
                        jz.content = neirong;
                        jz.len = neirong.Length;
                        db.JuhaoList.Add(jz);
                        break;
                    case "？":

                        jz.type = CEType.wenhao;
                        jz.content = neirong;
                        jz.len = neirong.Length;
                        db.WenhaoList.Add(jz);
                        break;
                    case "！":
                        jz.type = CEType.gantanhao;
                        jz.content = neirong;
                        jz.len = neirong.Length;
                        db.GantanhaoList.Add(jz);
                        break;
                    default:
                        jz.type = CEType.qita;
                        jz.content = neirong;
                        jz.len = neirong.Length;
                        db.QitaList.Add(jz);
                        break;
                }
            }
            return db;
        }

        public static string Parse(string str, CorpusDb db)
        {
            var juhaomc = juhao.Matches(str);
            var gantanhaomc = gantanhao.Matches(str);
            var wenhaomc = wenhao.Matches(str);
            var douhaomc = douhao.Matches(str);
            var qitamc = qita.Matches(str);


            if (juhaomc.Count > 0)
            {
                str = (from Match match in juhaomc
                    select
                        db.JuhaoList.Count > 0
                            ? db.JuhaoList[random.Next(0, db.JuhaoList.Count)].content
                            : db.QitaList[random.Next(0, db.QitaList.Count)].content).Aggregate(str,
                                (current, jz) => juhao.Replace(current, jz, 1));
            }

            if (gantanhaomc.Count > 0)
            {
                str = (from Match match in gantanhaomc
                    select
                        db.GantanhaoList.Count > 0
                            ? db.GantanhaoList[random.Next(0, db.GantanhaoList.Count)].content
                            : db.QitaList[random.Next(0, db.QitaList.Count)].content).Aggregate(str,
                                (current, jz) => gantanhao.Replace(current, jz, 1));
            }

            if (wenhaomc.Count > 0)
            {
                str = (from Match match in wenhaomc
                    select
                        db.WenhaoList.Count > 0
                            ? db.WenhaoList[random.Next(0, db.WenhaoList.Count)].content
                            : db.QitaList[random.Next(0, db.QitaList.Count)].content).Aggregate(str,
                                (current, jz) => wenhao.Replace(current, jz, 1));
            }

            if (douhaomc.Count > 0)
            {
                str = (from Match match in douhaomc
                    select
                        db.DouhaoList.Count > 0
                            ? db.DouhaoList[random.Next(0, db.DouhaoList.Count)].content
                            : db.QitaList[random.Next(0, db.QitaList.Count)].content).Aggregate(str,
                                (current, jz) => douhao.Replace(current, jz, 1));
            }

            if (qitamc.Count > 0)
            {
                str = (from Match match in qitamc
                    select
                        db.QitaList.Count > 0
                            ? db.QitaList[random.Next(0, db.QitaList.Count)].content
                            : db.DouhaoList[random.Next(0, db.DouhaoList.Count)].content).Aggregate(str,
                                (current, jz) => qita.Replace(current, jz, 1));
            }

            return str;
        }
    }
}