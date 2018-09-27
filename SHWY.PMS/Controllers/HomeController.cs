using SHWY.Lib.DB.Repositorys;
using SHWY.Model.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SHWY.PMS.Controllers
{
    public class HomeController : BaseController
    {
        PersonTaskRepository personTaskRepo = new PersonTaskRepository();
        public ActionResult Index()
        {
            return View();
        }

        public async Task<PartialViewResult> myTaskType_Partial()
        {
            var list = await personTaskRepo.GetTaskListAsync(CurrentUser.Sys_User.Uid);
            return PartialView( new MyPersonTaskList() { personTaskList = list });
        }
    }
    public class MyPersonTaskList
    {
        public List<V_PersonTask> personTaskList { get; set; }
    }
}