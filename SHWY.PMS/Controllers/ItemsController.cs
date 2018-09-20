using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SHWY.Lib.DB.Repositorys;
using SHWY.Model.DB;

namespace SHWY.PMS.Controllers
{
    public class ItemsController : BaseController
    {
        ItemsRepository repoItems = new ItemsRepository();
        // GET: Items
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> GetList(int page, int rows, string itemName)
        {
            var result = await repoItems.GetListAsync(page, rows, itemName);
            return Json(new { total = result.Item1, rows = result.Item2 });
        }
        public async Task<ActionResult> EditItem(int itemId)
        {
            Items item = await repoItems.GetItemAsync(itemId);
            return View(item);
        }
        public async Task<JsonResult> SaveItem(Items item)
        {
            var result = await repoItems.AddOrUpdateAsync(item);
            return Json(new { isOk = result });
        }
        public async Task<JsonResult> DelItem(int itemId)
        {
            var result = await repoItems.DeleteItem(itemId);
            return Json(new { isOk = result });
        }
    }
}