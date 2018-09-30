using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SHWY.PMS.Controllers.Filter;
using SHWY.Lib.DB.Repositorys;
using SHWY.Model.DB;

namespace SHWY.PMS.Controllers
{
    public class UserController : BaseController
    {
        Sys_UserRepository repo = new Sys_UserRepository();
        Sys_RoleRepository roleRepo = new Sys_RoleRepository();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetList(int page, int rows, string userName)
        {
            var result = await repo.GetListAsync(page, rows, userName);
            return Json(new { total = result.Item1, rows = result.Item2 });
        }
        public async Task<JsonResult> GetAllUsers(int isAddAll)
        {
            var result = await repo.GetUserListAsync();
            if (isAddAll == 1)
                result.Insert(0, new Sys_User() { Uid = 0, Name = "全部" });
            return Json(result);
        }
        public async Task<JsonResult> GetListTest()
        {
            var result = await repo.GetListAsync(1, 10, "");
            var json = Json(result.Item2);
            return json;
        }
        #region 增删改
        public async Task<ActionResult> EditUser(int uid)
        {
            Sys_User user = await repo.GetUserAsync(uid);
            ViewBag.RoleList = await roleRepo.GetListAsync();
            return View(user);

        }
        public async Task<JsonResult> SaveUser(Sys_User user)
        {
            var result = await repo.AddOrUpdateAsync(user);
            return Json(new { isOk = result });
        }
        public JsonResult BatchSave(List<Sys_User> userList)
        {
            var result = true;
            return Json(new { isOk = result });
        }
        public async Task<JsonResult> DelUser(int uid)
        {
            var result = await repo.DeleteUser(uid);
            return Json(new { isOk = result });
        }
        #endregion
    }
}