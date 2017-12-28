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
    public class UserController : Controller
    {
        Sys_UserRepository repo = new Sys_UserRepository();
        // GET: User
        public async Task<ActionResult> Index()
        {
            return View();
        }

        public async Task<JsonResult> GetList(int page, int rows, string username)
        {
            var result = await repo.GetListAsync(page, rows, username);
            return Json(new { total = result.Item1, rows = result.Item2 });
        }
        public async Task<ActionResult> EditUser(int uid)
        {
            Sys_User user = await repo.GetUserAsync(uid);
            return View(user);

        }
        public async Task<JsonResult> SaveUser(Sys_User user)
        {
            var result = await repo.AddOrUpdateAsync(user);
            return Json(new { isOk = result});
        }
        public async Task<JsonResult> DelUser(int uid)
        {
            var result = await repo.DeleteUser(uid);
            return Json(new { isOk = result });
        }
    }
}