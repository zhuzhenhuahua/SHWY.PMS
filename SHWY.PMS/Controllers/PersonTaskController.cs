using SHWY.Lib.DB.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SHWY.Model.DB;

namespace SHWY.PMS.Controllers
{
    public class PersonTaskController : BaseController
    {
        PersonTaskRepository pTaskRepo = new PersonTaskRepository();
        // GET: PersonTask
        public ActionResult TaskPublishList()
        {
            return View();
        }
        public async Task<JsonResult> GetList(int page, int rows, int handlerId,int itemId)
        {
            var result = await pTaskRepo.GetListAsync(page, rows, handlerId,itemId);
            return Json(new { total = result.Item1, rows = result.Item2 });
        }
        public async Task<ActionResult> TaskPublishEdit(string id)
        {
            PersonTask task = await pTaskRepo.GetTaskAsync(id);
            return View(task);
        }
        public async Task<JsonResult> SaveTask(PersonTask task)
        {
            var result = await pTaskRepo.AddOrUpdateAsync(task);
            return Json(new { isOk = result });
        }
        public async Task<JsonResult> DelTask(string id)
        {
            var result = await pTaskRepo.DeletePersonTask(id);
            return Json(new { isOk = result });
        }
    }
}