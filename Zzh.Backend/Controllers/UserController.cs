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
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetList(int page, int rows, string username)
        {
            using (Sys_UserRepository repo = new Sys_UserRepository())
            {
                var result = await repo.GetList(page, rows, username);
                return Json(new { total = result.Item1, rows = result.Item2 });
            }
        }
        public ActionResult EditUser()
        {
            Sys_User user = new Sys_User();
            return View(user);
        }
    }
}