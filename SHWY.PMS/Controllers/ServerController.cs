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
    public class ServerController : BaseController
    {
        ServerRepository serverRepo = ServerRepository.CreateInstance();
        ItemsRepository itemsRepo = ItemsRepository.CreateInstance();
        // GET: Server
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> GetList(int page, int rows, string name)
        {
            var tuple = await serverRepo.GetServerListAsync(page, rows, name);
            return Json(new { total = tuple.Item1, rows = tuple.Item2 });
        }
        public async Task<JsonResult> IsExists(int sid)
        {
            var model = await serverRepo.GetServerAsync(sid);
            return Json(new { IsExists = model != null });
        }
        #region 增删改
        public async Task<ActionResult> ServerEdit(int sid)
        {
            var server = await serverRepo.GetServerAsync(sid);
            var ItemList = new List<SelectListItem>();
            var items = await itemsRepo.GetListItemsAsync();
            var items2 = new SelectList(items, "ItemID", "NAME");
            ItemList.AddRange(items2);
            ViewBag.ItemList = ItemList;
            ViewBag.isRealOnly = (server != null).ToString().ToLower();
            return View(server);
        }
        public async Task<JsonResult> SaveServer(Servers server)
        {
            var result = await serverRepo.AddOrUpdateAsync(server);
            return Json(new { isOk = result });
        }
        public async Task<JsonResult> DelServer(int sid)
        {
            var result = await serverRepo.DelServerAsync(sid);
            return Json(new { isOk = result });
        }
        #endregion
    }
}