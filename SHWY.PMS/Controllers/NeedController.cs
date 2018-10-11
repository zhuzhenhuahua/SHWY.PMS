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
    public class NeedController : BaseController
    {
        ItemsRepository itemsRepo = ItemsRepository.CreateInstance();
        ProductRepository prodRepo = ProductRepository.CreateInstance();
        CodeRepository codeRepo = CodeRepository.CreateInstance();
        // GET: Need
        public ActionResult NeedIndex()
        {
            return View();
        }

        public async Task<JsonResult> GetNeedListAsync(int page, int rows, string itemID)
        {
            using (NeedRepository needRepo = new NeedRepository())
            {
                var tuple = await needRepo.GetNeedListAsync(page, rows, itemID);
                return Json(new { total = tuple.Item1, rows = tuple.Item2 });
            }

        }
        public async Task<JsonResult> GetNeedByItemID(string itemID)
        {
            using (NeedRepository needRepo = new NeedRepository())
            {
                var list = await needRepo.GetNeedListByItemIDAsync(itemID);
                return Json(list);
            }
        }
        public async Task<ActionResult> NeedEdit(string needID)
        {
            using (NeedRepository needRepo = new NeedRepository())
            {
                Need model = await needRepo.GetNeedAsync(needID);
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
                if (model == null)
                {
                    model = new Need() {  proposeTime=DateTime.Now, deadTime = DateTime.Now.AddDays(3) };
                }
                return View(model);
            }

        }
        public async Task<JsonResult> SaveNeed(Need model)
        {
            using (NeedRepository needRepo = new NeedRepository())
            {
                if (string.IsNullOrEmpty(model.NeedID))
                {
                    var session = Session["CurrentUser"] as CurrentUser;
                    model.NeedID = CommonHelper.GetRandomString("ND");
                    model.PublisherID = session.Sys_User.Uid;
                }
                var res = await needRepo.AddOrUpdateNeedAsync(model);
                return Json(new { isOk = res });
            }
        }
        public async Task<JsonResult> DelNeed(string needID)
        {
            using (NeedRepository needRepo = new NeedRepository())
            {
                var res = await needRepo.DelNeedAsync(needID);
                return Json(new { isOk = res });
            }
        }
    }
}