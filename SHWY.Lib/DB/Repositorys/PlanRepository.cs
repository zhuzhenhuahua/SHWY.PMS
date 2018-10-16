using SHWY.Model.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHWY.Lib.DB.Repositorys
{
    public class PlanRepository : BaseRepository
    {
        #region Plan
        public async Task<Tuple<int, object>> GetPlanListAsync(int pageIndex, int pageSize, string title)
        {
            int form = (pageIndex - 1) * pageSize;
            var total = await (from j in context.Plans
                               where j.title.Contains(title)
                               select j).CountAsync();
            var obj = await (from j in context.Plans
                             join ptype in context.PlanTypes on j.planType equals ptype.ID into tempPType
                             from planType in tempPType.DefaultIfEmpty()
                             where j.title.Contains(title)
                             orderby j.addTime descending
                             select new
                             {
                                 j.PlanID,
                                 j.title,
                                 j.startTime,
                                 j.endTime,
                                 j.text,
                                 planTypeName = planType == null ? "" : planType.NAME
                             }).Skip(form).Take(pageSize).ToListAsync();
            return Tuple.Create<int, object>(total, obj);
        }
        public async Task<Plan> GetPlanAsync(string planID)
        {
            var model = await context.Plans.Where(p => p.PlanID == planID).FirstOrDefaultAsync();
            return model;
        }
        public async Task<bool> AddOrUpdatePlanAsync(Plan planPara)
        {
            var isAdd = false;
            var model = await context.Plans.Where(p => p.PlanID == planPara.PlanID).FirstOrDefaultAsync();
            if (model == null)
            {
                model = new Plan()
                {
                    PlanID = planPara.PlanID,
                    addTime = DateTime.Now,
                };
                isAdd = true;
            }
            model.title = planPara.title;
            model.startTime = planPara.startTime;
            model.endTime = planPara.endTime;
            model.text = planPara.text;
            model.planType = planPara.planType;
            if (isAdd)
                context.Plans.Add(model);
            return await context.SaveChangesAsync() == 1;
        }
        public async Task<bool> DelPlanAsync(string planID)
        {
            var model = await context.Plans.Where(p => p.PlanID == planID).FirstOrDefaultAsync();
            if (model != null)
            {
                context.Plans.Remove(model);
                return await context.SaveChangesAsync() == 1;
            }
            return false;
        }
        #endregion

        #region PlanPoint
        public async Task<Tuple<int, object>> GetPlanPointListAsync(int pageIndex, int pageSize, string planID, string itemID)
        {
            int form = (pageIndex - 1) * pageSize;
            var total = await (from j in context.PlanPoints
                               where j.PlanID == planID && (string.IsNullOrEmpty(itemID) ? 1 == 1 : j.ItemID == itemID)
                               select j).CountAsync();
            var obj = await (from j in context.PlanPoints
                             join ite in context.Items on j.ItemID equals ite.ItemID into tempItem
                             from item in tempItem.DefaultIfEmpty()
                             join pro in context.Products on j.ProID equals pro.ProID into tempProd
                             from prod in tempProd.DefaultIfEmpty()
                             join modu in context.ProdModules on j.ModuleID equals modu.ModuleID into tempModu
                             from module in tempModu.DefaultIfEmpty()
                             join tk in context.TaskTypes on j.TaskType equals tk.ID into tempTk
                             from taskType in tempTk.DefaultIfEmpty()
                             join puber in context.Sys_Users on j.PublisherId equals puber.Uid into tempUser
                             from userPuber in tempUser.DefaultIfEmpty()
                             join hander in context.Sys_Users on j.handlerID equals hander.Uid into tempHander
                             from handler in tempHander.DefaultIfEmpty()
                             join ned in context.Needs on j.NeedID equals ned.NeedID into tempNed
                             from need in tempNed.DefaultIfEmpty()
                             where j.PlanID == planID && (string.IsNullOrEmpty(itemID) ? 1 == 1 : j.ItemID == itemID)
                             orderby j.addTime descending
                             select new
                             {
                                 j.PlanID,
                                 j.num,
                                 itemName = item == null ? "" : item.NAME,
                                 prodName = prod == null ? "" : prod.NAME,
                                 moduleName = module == null ? "" : module.NAME,
                                 taskTypeName = taskType == null ? "" : taskType.NAME,
                                 j.StartTime,
                                 j.EndTime,
                                 j.Text,
                                 needName = need == null ? "" : need.Text,
                                 Publisher = userPuber == null ? "" : userPuber.Name,
                                 Handler = handler == null ? "" : handler.Name,
                             }).Skip(form).Take(pageSize).ToListAsync();
            return Tuple.Create<int, object>(total, obj);
        }

        public async Task<PlanPoint> GetPlanPointAsync(string planID, int num)
        {
            var model = await context.PlanPoints.Where(p => p.PlanID == planID && p.num == num).FirstOrDefaultAsync();
            return model;
        }

        public async Task<int> GetPlanPointMaxNumByPlanIDAsync(string planID)
        {
            var list = await context.PlanPoints.Where(p => p.PlanID == planID).ToListAsync();
            if (list.Count == 0)
                return 0;
            else
            {
                var maxNum = list.Max(p => p.num);
                return maxNum;
            }
        }

        public async Task<bool> AddOrUpdatePlanPointAsync(PlanPoint planPointPara)
        {
            var isAdd = false;
            var model = await context.PlanPoints.Where(p => p.PlanID == planPointPara.PlanID && p.num == planPointPara.num).FirstOrDefaultAsync();
            if (model == null)
            {
                int planTolal = await GetPlanPointMaxNumByPlanIDAsync(planPointPara.PlanID);
                model = new PlanPoint()
                {
                    PlanID = planPointPara.PlanID,
                    num = planTolal + 1,
                    addTime = DateTime.Now,
                    PublisherId = planPointPara.PublisherId
                };
                isAdd = true;
            }
            model.ItemID = planPointPara.ItemID;
            model.ProID = planPointPara.ProID;
            model.ModuleID = planPointPara.ModuleID;
            model.TaskType = planPointPara.TaskType;
            model.StartTime = planPointPara.StartTime;
            model.EndTime = planPointPara.EndTime;
            model.Text = planPointPara.Text;
            model.NeedID = planPointPara.NeedID;
            model.handlerID = planPointPara.handlerID;
            model.UpdTime = DateTime.Now;
            if (isAdd)
                context.PlanPoints.Add(model);
            return await context.SaveChangesAsync() == 1;
        }
        public async Task<bool> DelPlanPointAsync(string planID, int num)
        {
            var model = await context.PlanPoints.Where(p => p.PlanID == planID && p.num == num).FirstOrDefaultAsync();
            if (model != null)
            {
                context.PlanPoints.Remove(model);
                return await context.SaveChangesAsync() == 1;
            }
            return false;
        }
        #endregion

        #region PlanType
        public async Task<List<PlanType>> GetPlanTypeListAsync()
        {
            var list = await context.PlanTypes.ToListAsync();
            return list;
        }
        #endregion
    }
}
