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
        public async Task<ActionResult> Index()
        {
            MyPersonTaskList list = new MyPersonTaskList();
            list.personTaskList = await personTaskRepo.GetTaskListAsync(CurrentUser.Sys_User.Uid);
            return View(list);
        }
    }
    public class MyPersonTaskList
    {
        public List<V_PersonTask> personTaskList { get; set; }
    }
}