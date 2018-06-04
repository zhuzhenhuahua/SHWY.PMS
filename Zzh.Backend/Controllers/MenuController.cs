using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Zzh.Backend.Models;
using Zzh.Lib.DB.Repositorys;
using Zzh.Model.DB;

namespace Zzh.Backend.Controllers
{
    public class MenuController : Controller
    {
        // GET: Menu
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> GetList(int rows, int page, string menuName, int parentId)
        {
            using (Sys_MenuRepository rep = new Sys_MenuRepository())
            {
                var result = await rep.GetListAsync(page, rows, menuName, parentId);
                return Json(new { total = result.Item1, rows = result.Item2 });
            }

        }
        public async Task<JsonResult> GetParentMenuList()
        {
            using (Sys_MenuRepository rep = new Sys_MenuRepository())
            {
                var result = await rep.GetListByParentIdAsync(0);
                result.Insert(0, new Sys_Menu() { MenuId = 0, MenuName = "全部" });
                return Json(result);
            }
        }
        //获取所有二级菜单列表
        public async Task<JsonResult> GetAllChildMeunList()
        {
            using (Sys_MenuRepository rep = new Sys_MenuRepository())
            {
                var result = await rep.GetAllChildMeunList();
                result.Insert(0, new Sys_Menu() { MenuId = 0, MenuName = "全部" });
                return Json(result);
            }
        }
        #region 增删改操作
        public async Task<ActionResult> EditMenu(int mid)
        {
            using (Sys_MenuRepository rep = new Sys_MenuRepository())
            {
                var menu = new Sys_Menu();
                if (mid > 0)
                    menu = await rep.GetMenuAsync(mid);
                var list = await rep.GetListByParentIdAsync(0);
                var parentMenuList = new List<SelectListItem>()
                {
                     new SelectListItem() {   Value="0", Text="无", Selected=true}
                };
                var selectList = new SelectList(list, "MenuId", "MenuName");
                parentMenuList.AddRange(selectList);
                ViewBag.parentMenuList = parentMenuList;
                return View(menu);
            }
        }
        public async Task<JsonResult> SaveMenu(Sys_Menu menu)
        {
            using (Sys_MenuRepository rep = new Sys_MenuRepository())
            {
                var result = await rep.AddOrUpdateAsync(menu);
                return Json(new { isOk = result });
            }
        }
        public async Task<JsonResult> DelMenu(int mid)
        {
            using (Sys_MenuRepository rep = new Sys_MenuRepository())
            {
                var result = await rep.DeleteMenuAsync(mid);
                return Json(new { isOk = result });
            }
        }
        public async Task<JsonResult> GetMenuListTree(int rid)
        {
            using (Sys_MenuRepository rep_Menu = new Sys_MenuRepository())
            {
                using (Sys_RoleMenuRepository rep_RoleMenu = new Sys_RoleMenuRepository())
                {
                    var menuList = await rep_Menu.GetListAsync();//所有菜单
                    var roleMenu = await rep_RoleMenu.GetListAsync(rid);//该角色已经有的菜单ID
                    List<int> menuIds = roleMenu.Select(p => p.MenuId).ToList();
                    var result = ConvertMenuToEasyUiTree(menuList, menuIds);
                    return Json(result);
                }
            }
        }
        #endregion
        /// <summary>
        /// 将menuList转换为EasyUiTreeViewModel
        /// </summary>
        /// <param name="menuList">所有的菜单</param>
        /// <param name="roleMenu">角色所拥有的MenuID的列表</param>
        /// <param name="parentId">菜单父ID</param>
        /// <returns></returns>
        public List<EasyUiTreeViewModel> ConvertMenuToEasyUiTree(List<Sys_Menu> menuList, List<int> roleMenu, int parentId = 0)
        {
            List<EasyUiTreeViewModel> treeModeList = new List<Models.EasyUiTreeViewModel>();
            treeModeList.AddRange(menuList.Where(p => p.ParentId == parentId).OrderBy(p => p.MenuSortID).Select(m => new EasyUiTreeViewModel()
            {
                id = m.MenuId,
                text = m.MenuName,
                @checked = roleMenu.Contains(m.MenuId),
                children = ConvertMenuToEasyUiTree(menuList, roleMenu, m.MenuId)
            }));
            return treeModeList;
        }
    }
}