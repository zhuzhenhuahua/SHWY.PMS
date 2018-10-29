using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SHWY.Lib.DB.Repositorys;
using SHWY.PMS.Controllers.Filter;

namespace SHWY.PMS.Controllers
{
    //该类作为登录验证，暂不继承BaseController，如果后期需要继承，则在filter中判断
    public class AccountController : Controller
    {
        Sys_UserRepository userRepo = new Sys_UserRepository();
        Sys_RoleMenuRepository repo_RoleMeun = new Sys_RoleMenuRepository();
        Sys_RoleOperRepository repo_RoleOper = new Sys_RoleOperRepository();
        // GET: Account
        public ActionResult Login()
        {
            LoginViewModel model = new LoginViewModel();
            Session.Clear();
            return View(model);
        }
        [LogRecord("系统登录")]
        public async Task<ActionResult> LoginSubmit(LoginViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.LoginName))
            {
                ViewData["errorMsg"] = "用户名不能为空";
                return View("Login", viewModel);
            }
            if (string.IsNullOrEmpty(viewModel.Password))
            {
                ViewData["errorMsg"] = "密码不能为空";
                return View("Login", viewModel);
            }
            var userModel = await userRepo.GetUserAsync(viewModel.LoginName, viewModel.Password);
            if (userModel == null)
            {
                ViewData["errorMsg"] = "账号或密码输入错误";
                return View("Login", viewModel);
            }
            CurrentUser currentUser = new CurrentUser();
            currentUser.Sys_User = userModel;
            //currentUser.Sys_RoleMenu = await repo_RoleMeun.GetListAsync(userModel.RoleId);
            //currentUser.Sys_RoleOper = await repo_RoleOper.GetListAsync(userModel.RoleId);
            Session["CurrentUser"] = currentUser;
            Session.Timeout = 180;//登录过期时间（分钟）
            return Redirect("/Home/Index"); ; 

        }
        public JsonResult GetSessionUser()
        {
            var user = Session["CurrentUser"] as CurrentUser;
            return Json(user.Sys_User);
        }
        public JsonResult GetSessionUserExists()
        {
            var json = Json(new { isExists= Session["CurrentUser"] != null });
            return json;
        }
        public class LoginViewModel
        {
            public string LoginName { get; set; }
            public string Password { get; set; }
        }
    }
}