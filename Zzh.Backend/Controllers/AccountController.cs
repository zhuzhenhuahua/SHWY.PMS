using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Zzh.Backend.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            LoginViewModel model = new LoginViewModel();
            return View(model);
        }
        public ActionResult LoginSubmit(LoginViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.LoginName))
            {
                ViewData["errorMsg"] = "用户名不能为空";
                return View("Login", viewModel);
            }
            if (string.IsNullOrEmpty(viewModel.Password))
            {
                ViewData["errorMsg"] = "密码不能为空";
                return View("Login",viewModel);
            }
            return View("Login",viewModel);

        }
        public class LoginViewModel
        {
            public string LoginName { get; set; }
            public string Password { get; set; }
        }
    }
}