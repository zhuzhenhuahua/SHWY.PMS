using SHWY.Lib.DB.Repositorys;
using SHWY.Model.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SHWY.PMS.Controllers.Filter;
using SHWY.Utility;

namespace SHWY.PMS.Controllers
{
    public class HomeController : BaseController
    {
        PersonTaskRepository personTaskRepo = new PersonTaskRepository();
        CodeRepository codeRepo = CodeRepository.CreateInstance();
        public ActionResult Index()
        {
            return View();
        }

        public async Task<PartialViewResult> myTaskType_Partial()
        {
            var session = Session["CurrentUser"] as CurrentUser;
            var list = await personTaskRepo.GetTaskListAsync(session.Sys_User.Uid);
            return PartialView(new MyPersonTaskList() { personTaskList = list });
        }
        public JsonResult SetUserThemes(string themesName)
        {
            var session = Session["CurrentUser"] as CurrentUser;
            EasyuiThemesHelper.SetValue(session.Sys_User.Uid, themesName);
            return Json(new { isOK = true });
        }
    }
    public class MyPersonTaskList
    {
        public List<V_PersonTask> personTaskList { get; set; }
    }
}