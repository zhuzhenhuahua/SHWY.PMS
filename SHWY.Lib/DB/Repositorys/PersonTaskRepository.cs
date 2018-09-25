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

        public async Task<Tuple<int,List<V_PersonTask>>> GetListAsync(int pageIndex, int pageSize, int handlerId, int itemId)
        {
            int from = (pageIndex - 1) * pageSize;
            int total = await (from j in context.VPersonTask
                               where (handlerId == 0 ? 1 == 1 : j.handlerID == handlerId)
                             && (itemId == 0 ? 1 == 1 : j.itemID == itemId)
                               select j).CountAsync();
            var list = await (from j in context.VPersonTask
                                  //join item in context.Items on j.itemID equals item.ItemID
                                  //join prod in context.Products on j.prodId equals prod.ProID
                                  //join taskType in context.TaskTypes on j.TaskType equals taskType.ID
                                  //join taskStatus in context.TaskStatus on j.taskStatus equals taskStatus.id
                                  //join pUser in context.Sys_Users on j.publisherID equals pUser.Uid
                                  //join hUser in context.Sys_Users on j.handlerID equals hUser.Uid
                                  //join fUser in context.Sys_Users on j.followerID equals fUser.Uid
                              where (handlerId == 0 ? 1 == 1 : j.handlerID == handlerId)
                              && (itemId == 0 ? 1 == 1 : j.itemID == itemId)
                              orderby j.publishTime descending
                              select j).Skip(from).Take(pageSize).ToListAsync();
            // var list =await context.VPersonTask.SqlQuery("select * from V_PersonTask order by publishTime desc").ToListAsync();
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
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeletePersonTask(string id)
        {
            var task = await context.PersonTasks.Where(p => p.ID == id).FirstOrDefaultAsync();
            if (task != null)
            {
                context.PersonTasks.Remove(task);
                return await context.SaveChangesAsync() == 1;
            }
            return false;
        }
        #endregion
    }
}
