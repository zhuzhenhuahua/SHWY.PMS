using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SHWY.Lib.DB.Repositorys;
using SHWY.Model.DB;
using SHWY.Utility;

namespace SHWY.PMS.Controllers
{
    public class PlanController : BaseController
    {
        ItemsRepository itemsRepo = ItemsRepository.CreateInstance();
        ProductRepository prodRepo = ProductRepository.CreateInstance();
        CodeRepository codeRepo = CodeRepository.CreateInstance();
        Sys_UserRepository userRepo = new Sys_UserRepository();
        #region Plan
        // GET: Plan
        public ActionResult PlanIndex()
        {
            return View();
        }
        public async Task<JsonResult> GetPlanListAsync(int page, int rows, string title)
        {
            using (PlanRepository planRepo = new PlanRepository())
            {
                var tuple = await planRepo.GetPlanListAsync(page, rows, title);
                return Json(new { total = tuple.Item1, rows = tuple.Item2 });
            }

        }
        public async Task<ActionResult> PlanEdit(string planID)
        {
            using (PlanRepository planRepo = new PlanRepository())
            {
                Plan model = await planRepo.GetPlanAsync(planID);
                if (model == null)
                {
                    model = new Plan() { startTime = DateTime.Now, endTime = DateTime.Now.AddDays(3) };
                }
                //计划类型
                var PlanTypeList = new List<SelectListItem>();
                var plantype = await planRepo.GetPlanTypeListAsync();
                var plantype2 = new SelectList(plantype, "ID", "NAME");
                PlanTypeList.AddRange(plantype2);
                ViewBag.PlanTypeList = PlanTypeList;
                return View(model);
            }

        }
        public async Task<JsonResult> SavePlan(Plan model)
        {
            using (PlanRepository planRepo = new PlanRepository())
            {
                if (string.IsNullOrEmpty(model.PlanID))
                {
                    model.PlanID = CommonHelper.GetRandomString("PL");
                }
                var res = await planRepo.AddOrUpdatePlanAsync(model);
                return Json(new { isOk = res });
            }
        }
        public async Task<JsonResult> DelPlan(string planID)
        {
            using (PlanRepository planRepo = new PlanRepository())
            {
                var res = await planRepo.DelPlanAsync(planID);
                return Json(new { isOk = res });
            }
        }
        #endregion

        #region PlanPoint
        public ActionResult PlanPointIndex()
        {
            return View();
        }
        public async Task<JsonResult> GetPlanPointListAsync(int page, int rows, string planID, string itemID)
        {
            if (string.IsNullOrEmpty(planID))
            {
                return null;
            }
            using (PlanRepository planRepo = new PlanRepository())
            {
                var tuple = await planRepo.GetPlanPointListAsync(page, rows, planID, itemID);
                return Json(new { total = tuple.Item1, rows = tuple.Item2 });
            }

        }

        public async Task<ActionResult> PlanPointEdit(string planID, int num)
        {
            using (PlanRepository planRepo = new PlanRepository())
            {
                PlanPoint model = await planRepo.GetPlanPointAsync(planID, num);
                if (model == null)
                {
                    model = new PlanPoint() { StartTime = DateTime.Now, EndTime = DateTime.Now.AddDays(3), NeedID = "0" };
                }
                //项目
                var ItemList = new List<SelectListItem>();
                var items = await itemsRepo.GetListItemsAsync();
                var items2 = new SelectList(items, "ItemID", "NAME");
                ItemList.AddRange(items2);
                ViewBag.ItemList = ItemList;
                //产品
                var ProdList = new List<SelectListItem>();
                var prods = await prodRepo.GetListAsync();
                var prods2 = new SelectList(prods, "ProID", "NAME");
                ProdList.AddRange(prods2);
                ViewBag.ProdList = ProdList;
                //任务类型
                var TaskTypeList = new List<SelectListItem>();
                var tasktypes = await codeRepo.GetTaskTypeListAsync();
                var tasktypes2 = new SelectList(tasktypes, "ID", "NAME");
                TaskTypeList.AddRange(tasktypes2);
                ViewBag.TaskTypeList = TaskTypeList;
                //处理人
                var UserList = new List<SelectListItem>();
                var userlist = await userRepo.GetUserListAsync();
                var userlist2 = new SelectList(userlist, "Uid", "Name");
                UserList.AddRange(userlist2);
                ViewBag.UserList = UserList;
                return View(model);
            }

        }

        public async Task<JsonResult> SavePlanPoint(PlanPoint model)
        {
            using (PlanRepository planRepo = new PlanRepository())
            {
                if (model.num == 0)
                {
                    var session = Session["CurrentUser"] as CurrentUser;
                    model.PublisherId = session.Sys_User.Uid;
                }
                var res = await planRepo.AddOrUpdatePlanPointAsync(model);
                return Json(new { isOk = res });
            }
        }
        public async Task<JsonResult> DelPlanPoint(string planID, int num)
        {
            using (PlanRepository planRepo = new PlanRepository())
            {
                var res = await planRepo.DelPlanPointAsync(planID, num);
                return Json(new { isOk = res });
            }
        }
        #endregion
    }
}