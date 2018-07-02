using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Zzh.Lib.DB.Repositorys;

namespace Zzh.Backend.Controllers
{
    public class AccountController : Controller
    {
        Sys_UserRepository userRepo = new Sys_UserRepository();
        // GET: Account
        public ActionResult Login()
        {
            LoginViewModel model = new LoginViewModel();
            return View(model);
        }
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

            return Redirect("/Home/Index"); ; 

        }
        public class LoginViewModel
        {
            public string LoginName { get; set; }
            public string Password { get; set; }
        }
    }
}