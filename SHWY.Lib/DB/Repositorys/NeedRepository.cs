using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHWY.Model.DB;
using SHWY.Utility;

namespace SHWY.Lib.DB.Repositorys
{
    public class NeedRepository : BaseRepository
    {
        public async Task<Tuple<int, object>> GetNeedListAsync(int pageIndex, int pageSize, string itemID)
        {
            int form = (pageIndex - 1) * pageSize;
            var total = await (from j in context.Needs
                               where string.IsNullOrEmpty(itemID) ? 1 == 1 : j.ItemID == itemID
                               select j).CountAsync();
            var obj = await (from j in context.Needs
                             join ite in context.Items on j.ItemID equals ite.ItemID into tempItem
                             from item in tempItem.DefaultIfEmpty()
                             join pro in context.Products on j.ProID equals pro.ProID into tempProd
                             from prod in tempProd.DefaultIfEmpty()
                             join modu in context.ProdModules on j.ModuleID equals modu.ModuleID into tempModu
                             from module in tempModu.DefaultIfEmpty()
                             join tk in context.TaskTypes on j.TaskType equals tk.ID into tempTk
                             from taskType in tempTk.DefaultIfEmpty()
                             join puber in context.Sys_Users on j.PublisherID equals puber.Uid into tempUser
                             from user in tempUser.DefaultIfEmpty()
                             where string.IsNullOrEmpty(itemID) ? 1 == 1 : j.ItemID == itemID
                             orderby j.addTime descending
                             select new
                             {
                                 j.NeedID,
                                 itemName = item == null ? "" : item.NAME,
                                 prodName = prod == null ? "" : prod.NAME,
                                 moduleName = module == null ? "" : module.NAME,
                                 taskTypeName = taskType == null ? "" : taskType.NAME,
                                 proposeTime = j.proposeTime,
                                 deadTime = j.deadTime,
                                 j.Text,
                                 publisher = user.Name,
                             }).Skip(form).Take(pageSize).ToListAsync();
            return Tuple.Create<int, object>(total, obj);
        }
        public async Task<List<Need>> GetNeedListByItemIDAsync(string itemID)
        {
            var nTime = DateTime.Now.AddMonths(-1);
            var list = await context.Needs.Where(p => p.ItemID == itemID && p.addTime > nTime).ToListAsync();
            return list;
        }
        public async Task<Need> GetNeedAsync(string needID)
        {
            var model = await context.Needs.Where(p => p.NeedID == needID).FirstOrDefaultAsync();
            return model;
        }
        public async Task<bool> AddOrUpdateNeedAsync(Need needPara)
        {
            var isAdd = false;
            var model = await context.Needs.Where(p => p.NeedID == needPara.NeedID).FirstOrDefaultAsync();
            if (model == null)
            {
                model = new Need()
                {
                    NeedID = needPara.NeedID,
                    addTime = DateTime.Now,
                    PublisherID = needPara.PublisherID,
                };
                isAdd = true;
            }
            model.ItemID = needPara.ItemID;
            model.ProID = needPara.ProID;
            model.ModuleID = needPara.ModuleID;
            model.TaskType = needPara.TaskType;
            model.proposeTime = needPara.proposeTime;
            model.deadTime = needPara.deadTime;
            model.updTime = DateTime.Now;
            model.Text = needPara.Text;
            if (isAdd)
                context.Needs.Add(model);
            return await context.SaveChangesAsync() == 1;
        }
        public async Task<bool> DelNeedAsync(string needID)
        {
            var model = await context.Needs.Where(p => p.NeedID == needID).FirstOrDefaultAsync();
            if (model != null)
            {
                context.Needs.Remove(model);
                return await context.SaveChangesAsync() == 1;
            }
            return false;
        }
    }
}
