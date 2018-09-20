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
    public class MenuOperController : BaseController
    {
        Sys_MenuOperRepository menuOperRepo = new Sys_MenuOperRepository();
        Sys_MenuRepository menuRepo = new Sys_MenuRepository();
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> GetListPager(int page, int rows, int menuId)
        {
            var tuple = await menuOperRepo.GetListPager(page, rows, menuId);
            return Json(new { total = tuple.Item1, rows = tuple.Item2 });
        }
        #region 增删改
        public async Task<ActionResult> EditMeunOper(int menuOperId)
        {
            Sys_MenuOper model = new Sys_MenuOper();
            if (menuOperId > 0)
            {
                model = await menuOperRepo.GetModelAsync(menuOperId);
                var menu = await menuRepo.GetMenuAsync(model.MenuId);//根据menuID找到对应的菜单
                model.MenuParentID = menu.ParentId;
            }


            return View(model);
        }
        public async Task<JsonResult> Save(Sys_MenuOper model)
        {
            model.CreateTime = DateTime.Now;
            var result = await menuOperRepo.AddOrUpdateAsync(model);
            return Json(new { isOk = result });
        }
        public async Task<JsonResult> DelMenuOper(int menuOperID) {
            var result = await menuOperRepo.DelMenuOper(menuOperID);
            return Json(new { isOk = result });
        }
        #endregion
    }
}