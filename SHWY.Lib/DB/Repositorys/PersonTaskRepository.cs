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
        public async Task<List<V_PersonTask>> GetTaskListAsync(DateTime fromDate, DateTime toDate, List<int> userIDs)
        {
            var taskList = await (from j in context.VPersonTask
                                  where userIDs.Contains(j.handlerID) && (
                                  (j.publishTimeDate >= fromDate && j.publishTimeDate < toDate) || (j.predDeadTimeDate >= fromDate && j.predDeadTimeDate < toDate)
                                  )
                                  orderby j.prodId
                                  select j).ToListAsync();
            foreach (var item in taskList)
            {
                item.PersonTaskProcess = await context.PersonTaskProcess.Where(p => p.TaskId == item.ID).ToListAsync();
            }
            return taskList;
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
                    context.PersonTasks.Add(taskNew);
                await AddTaskDailyAsync(taskNew);
                int res = await context.SaveChangesAsync();
                return res > 0;
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

        public async Task<bool> UpdateTaskStatus(string id, int taskStatus)
        {
            var model = await context.PersonTasks.Where(p => p.ID == id).FirstOrDefaultAsync();
            if (model != null)
            {
                model.taskStatus = taskStatus;
                if (taskStatus == 3 || taskStatus == 4)
                {
                    model.complTime = model.predDeadTime;
                }
                return await context.SaveChangesAsync() > 0;
            }
            return false;
        }
        #endregion

        #region PersonTaskDaily任务日志
        public async Task<List<V_PersonTaskDaily>> GetPersonTaskDailyListAsync(List<int> userIDs, DateTime fromDate, DateTime toDate, bool isGetProcess = true)
        {
            var list = await (from j in context.V_PersonTaskDailys
                              where userIDs.Contains(j.handlerID) &&
                              j.DateLine >= fromDate && j.DateLine <= toDate
                              select j
                           ).ToListAsync();
            if (isGetProcess)
            {
                foreach (var item in list)
                {
                    item.PersonTaskProcess = await context.PersonTaskProcess.Where(p => p.TaskId == item.TaskId).ToListAsync();
                }
            }
            return list;
        }
        private async Task<bool> AddTaskDailyAsync(PersonTask newTask)
        {
            var list = await context.PersonTaskDailys.Where(p => p.TaskId == newTask.ID).ToListAsync();
            if (list.Count > 0)
                context.PersonTaskDailys.RemoveRange(list);//增加前先全部删除
            //增加到任务日志。从开始时间到完成时间（预期完成时间），每一天都会生成一条记录，增加前先根据任务ID删除
            DateTime endTime = newTask.predDeadTime.Date;
            if (newTask.complTime != null && newTask.complTime.Value.Year != 1900)
                endTime = newTask.complTime.Value.Date;
            TimeSpan timeSpan = endTime.Subtract(newTask.perdStartTime.Date);
            int totalDay = timeSpan.Days + 1;
            for (int i = 0; i < totalDay; i++)
            {
                PersonTaskDaily daily = new PersonTaskDaily() { TaskId = newTask.ID, DateLine = newTask.perdStartTime.Date.AddDays(i) };
                context.PersonTaskDailys.Add(daily);
            }
            return true;//下面方法一起提交，这里就不需要提交了,所以此方法用了private
        }
        #endregion
    }
}
