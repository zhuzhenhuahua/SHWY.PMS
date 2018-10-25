using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHWY.Model.DB;
using System.Data.Entity;
using System.Data.SqlClient;

namespace SHWY.Lib.DB.Repositorys
{
    public class PersonTaskRepository : BaseRepository
    {
        //private static PersonTaskRepository _personTaskRepository;
        //private static readonly object locker = new object();
        //private PersonTaskRepository()
        //{

        //}
        //public static PersonTaskRepository CreateInstance()
        //{
        //    if (_personTaskRepository == null)
        //    {
        //        lock (locker)
        //        {
        //            if (_personTaskRepository == null)
        //                _personTaskRepository = new PersonTaskRepository();
        //        }
        //    }
        //    return _personTaskRepository;
        //}

        public async Task<Tuple<int, List<V_PersonTask>>> GetListAsync(int pageIndex, int pageSize,
            int handlerId, string itemId, string prodId, int taskStatus, DateTime? publishForm, DateTime? publishTo)
        {
            DateTime tempTo = new DateTime();
            if (publishTo != null)
            {
                tempTo = publishTo.Value.AddDays(1);
            }

            int from = (pageIndex - 1) * pageSize;
            int total = await (from j in context.VPersonTask
                               where (handlerId == 0 ? 1 == 1 : j.handlerID == handlerId)
                             && (string.IsNullOrEmpty(itemId) ? 1 == 1 : j.itemID == itemId)
                              && (string.IsNullOrEmpty(prodId) ? 1 == 1 : j.prodId == prodId)
                             && (taskStatus == 0 ? 1 == 1 : j.taskStatus == taskStatus)
                             && (publishForm == null ? 1 == 1 : j.publishTimeDate >= publishForm.Value)
                             && (publishTo == null ? 1 == 1 : j.publishTimeDate < tempTo)
                               select j).CountAsync();
            var list = await (from j in context.VPersonTask
                              where (handlerId == 0 ? 1 == 1 : j.handlerID == handlerId)
                            && (string.IsNullOrEmpty(itemId) ? 1 == 1 : j.itemID == itemId)
                            && (string.IsNullOrEmpty(prodId) ? 1 == 1 : j.prodId == prodId)
                            && (taskStatus == 0 ? 1 == 1 : j.taskStatus == taskStatus)
                               && (publishForm == null ? 1 == 1 : j.publishTimeDate >= publishForm.Value)
                             && (publishTo == null ? 1 == 1 : j.publishTimeDate < tempTo)
                              orderby j.publishTime descending
                              select j).Skip(from).Take(pageSize).ToListAsync();
            return Tuple.Create<int, List<V_PersonTask>>(total, list);
        }
        public async Task<PersonTask> GetTaskAsync(string id)
        {
            try
            {
                var task = await (from j in context.PersonTasks
                                  where j.ID == id
                                  select j).FirstOrDefaultAsync();
                if (task == null)
                    return new PersonTask();
                return task;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<V_PersonTask>> GetTaskListAsync(int handlerId)
        {
            try
            {
                var taskList = await (from j in context.VPersonTask
                                      where j.handlerID == handlerId && j.taskStatus != (int)ETaskStatus.已完成
                                      select j).ToListAsync();
                return taskList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region 增删改
        public async Task<bool> AddOrUpdateAsync(PersonTask ptask)
        {
            try
            {
                var taskNew = await GetTaskAsync(ptask.ID);
                bool isNew = false;

                if (taskNew == null || string.IsNullOrEmpty(taskNew.ID))
                {
                    isNew = true;
                }
                foreach (var p in taskNew.GetType().GetProperties())
                {
                    //更新属性
                    var v = ptask.GetType().GetProperty(p.Name).GetValue(ptask);
                    if (v != null)
                    {
                        //其他字段更新
                        p.SetValue(taskNew, v);
                    }
                }
                if (isNew)
                    context.PersonTasks.Add(ptask);
                return await context.SaveChangesAsync() == 1;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeletePersonTask(string id)
        {
            var task = await context.PersonTasks.Where(p => p.ID == id).FirstOrDefaultAsync();
            if (task != null)
            {
                //删除任务前必须先删除任务过程记录
                var processList = await context.PersonTaskProcess.Where(p => p.TaskId == task.ID).ToListAsync();
                if (processList != null && processList.Count > 0)
                {
                    context.PersonTaskProcess.RemoveRange(processList);
                }
                context.PersonTasks.Remove(task);
                return await context.SaveChangesAsync() > 0;
            }
            return false;
        }
        #endregion
    }
}
