using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Zzh.Lib.DB.Repositorys;
using Zzh.Model.DB;

namespace Zzh.Backend.Controllers
{
    public class MenuOperController : BaseController
    {
        Sys_MenuOperRepository menuOperRepo = new Sys_MenuOperRepository();
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> GetListPager(int page, int rows, int menuId)
        {
            var tuple = await menuOperRepo.GetListPager(page, rows, menuId);
            return Json(new { total = tuple.Item1, rows = tuple.Item2 });
        }
        public async Task<ActionResult> EditMeunOper(int menuOperId)
        {
            Sys_MenuOper model = new Sys_MenuOper();
            if (menuOperId >0)
               model = await menuOperRepo.GetModelAsync(menuOperId);
            return View(model);
        }
    }
}