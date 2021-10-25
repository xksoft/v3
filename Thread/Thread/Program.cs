using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace ThreadPool
{
    class Program
    {
        private static readonly object Lockobj = new object();

        static void Main(string[] args)
        {
            //线程数量
            const int max = 50;
            for (var i = 0; i < max; i++)
            {
                var thread = new Thread(Work)
                {
                    IsBackground = true,
                    Name = i.ToString()
                };
                thread.Start();
            }

            Console.WriteLine("按一次回车，添加一条任务！");
            while (true)
            {
                Console.ReadKey();
                var work = new WorkModel
                {
                    WorkType = 1,
                    Url = "http://www.baidu.com"
                };
                WorkPool.Add(work);
            }



        }

        private static void Work()
        {
            while (true)
            {
                var work = GetWorkModel();
                if (work != null)
                {
                    switch (work.WorkType)
                    {
                        case 1:
                            {
                                DoWork1(work.Url);
                                break;
                            }
                    }
                }
                Thread.Sleep(1);
            }
        }

        static readonly List<WorkModel> WorkPool = new List<WorkModel>();

        private static void DoWork1(string url)
        {
            using (var client = new WebClient())
            {
                var str = client.DownloadString(url);
                Console.WriteLine("线程：" + Thread.CurrentThread.Name + " 任务完成：" + str.Length);
            }
        }

        private static WorkModel GetWorkModel()
        {
            lock (Lockobj)
            {
                WorkModel work = null;
                if (WorkPool.Count > 0)
                {
                    work = WorkPool[0];
                    WorkPool.RemoveAt(0);
                }
                return work;
            }
        }
    }

    public class WorkModel
    {
        public int WorkType;
        public string Url;
    }
}
