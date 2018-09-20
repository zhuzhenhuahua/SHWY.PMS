using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHWY.Model.DB;
using System.Data.Entity;

namespace SHWY.Lib.DB.Repositorys
{
    public class PersonTaskRepository : BaseRepository
    {
        public async Task<Tuple<int, object>> GetListAsync(int pageIndex, int pageSize, int handlerId)
        {
            int from = (pageIndex - 1) * pageSize;
            int total = await (from j in context.PersonTasks
                               where handlerId == 0 ? 1 == 1 : j.handlerID == handlerId
                               select j).CountAsync();
            var list = await (from j in context.PersonTasks
                              join items in context.Items on j.itemID equals items.ItemID into tempItems
                              from ITEMS in tempItems.DefaultIfEmpty()//关联项目表
                              join prod in context.Products on j.prodId equals prod.ProID into tempProd
                              from PROD in tempProd.DefaultIfEmpty()//关联产品表
                              where handlerId == 0 ? 1 == 1 : j.handlerID == handlerId
                              orderby j.publishTime descending
                              select new
                              {
                                  ID = j.ID,
                                  j.JIRID,
                                  j.itemID,
                                  j.prodId,
                                  j.TaskType,
                                  j.detail,
                                  j.publishTime,
                                  j.predDeadTime,
                                  j.taskStatus,
                                  j.remark,
                                  j.publisherID,
                                  j.handlerID,
                                  j.followerID,
                                  j.complTime,
                                  j.taskComplDegree,
                                  j.serviceAttri,
                                  j.complSpeed,
                                  j.taskDiffLevel,
                                  j.evaDesc,
                                  ItemName=ITEMS==null?"":ITEMS.NAME,
                                  ProdName= PROD==null?"":PROD.NAME
                              }).Skip(from).Take(pageSize).ToListAsync();
            return new Tuple<int, object>(total, list);
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
            catch (Exception)
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
