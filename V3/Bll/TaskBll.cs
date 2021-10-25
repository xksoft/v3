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
using System.Threading.Tasks;
using Task = Model.Task;
using V3.Common;namespace V3.Bll
{
    public static class TaskBll
    {

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public static int Add(Task model)
        {



            int i = 0;
            foreach (KeyValuePair<int, Task> sk in Model.V3Infos.TaskDb)
            {
                

                    if (Convert.ToInt32(sk.Key) > i) { i = Convert.ToInt32(sk.Key); }

            }
            Model.V3Infos.MainDb.Taskid = i;
            V3Infos.MainDb.Taskid++;
            model.id = V3Infos.MainDb.Taskid;
            Model.V3Infos.TaskDb.Add(model.id, model);
            V3Helper.saveTaskDb();
            return model.id;
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public static bool Update(Model.Task model)
        {
            try
            {
                V3Infos.TaskDb[model.id] = model;
                V3Helper.saveTaskDb();
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public static bool Delete(int id)
        {
            try
            {
                Model.V3Infos.TaskDb.Remove(id);
                V3Helper.saveTaskDb();
                return true;
            }
            catch { return false; }
        }


        /// <summary>
        /// 保存task
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool SaveTask(int id)
        {
            if (Model.V3Infos.TaskDb.ContainsKey(id))
            {
                return Update(Model.V3Infos.TaskDb[id]);
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 获取任务状态
        /// </summary>
        /// <param name="id">任务id</param>
        /// <returns>1运行中 2停止中</returns>
        public static int getTaskStatus(int id)
        {
            if (Model.V3Infos.TaskDb.ContainsKey(id))
            {
                if(Model.V3Infos.TaskThread.ContainsKey(id))
                {
                    if (Model.V3Infos.TaskThread[id].Status == TaskStatus.Running || Model.V3Infos.TaskThread[id].Status == TaskStatus.WaitingToRun)
                        return 1;
                    else
                    {
                        //V3Model.V3Infos.TaskThread.Remove(id);
                        return 2;
                    }
                }
                else if (Model.V3Infos.TaskWaiting.ContainsKey(id))
                {
                    return 1;
                }
                else
                {
                    return 2;
                }

            }
            else
            {
                return 2;
            }
        }
    }
}
