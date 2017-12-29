using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Zzh.Lib.DB.Repositorys;
using Zzh.Model.DB;

namespace Zzh.Backend.Controllers
{
    public class RoleController : Controller
    {
        // GET: Role
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> GetList(int rows, int page, string roleName)
        {
            using (Sys_RoleRepository rep = new Sys_RoleRepository())
            {
                var result = await rep.GetListAsync(page, rows, roleName);
                return Json(new { total = result.Item1, rows = result.Item2 });
            }

        }
        public async Task<ActionResult> EditRole(int rid)
        {
            using (Sys_RoleRepository rep = new Sys_RoleRepository())
            {
                var role = new Sys_Role();
                if (rid > 0)
                    role = await rep.GetRoleAsync(rid);
                return View(role);
            }
        }
        public async Task<JsonResult> SaveRole(Sys_Role role)
        {
            using (Sys_RoleRepository rep = new Sys_RoleRepository())
            {
                var result = await rep.AddOrUpdateAsync(role);
                return Json(new { isOk = result });
            }
        }
        public async Task<JsonResult> DelRole(int rid)
        {
            using (Sys_RoleRepository rep = new Sys_RoleRepository())
            {
                var result = await rep.DeleteRoleAsync(rid);
                return Json(new { isOk = result });
            }
        }

        public ActionResult RoleMenuIndex(int rid)
        {
            ViewBag.rid = rid;
            return View();
        }

        public async Task<JsonResult> SaveRoleMenu(int roleId, List<int> checkedIdList)
        {
            using (Sys_RoleMenuRepository repo = new Sys_RoleMenuRepository())
            {
                bool result = false;
                if (roleId > 0 && checkedIdList.Count > 0)
                {
                    result = await repo.SaveRoleMenuAsync(roleId, checkedIdList);
                }
                return Json(new { isOK = result });
            }
        }
    }
}