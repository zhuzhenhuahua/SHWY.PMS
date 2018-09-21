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
        PersonTaskRepository pTaskRepo = PersonTaskRepository.CreateInstance();
        ItemsRepository itemsRepo = ItemsRepository.CreateInstance();
        ProductRepository prodRepo = ProductRepository.CreateInstance();
        CodeRepository codeRepo = CodeRepository.CreateInstance();
        // GET: PersonTask
        public ActionResult TaskPublishList()
        {
            return View();
        }
        public async Task<JsonResult> GetList(int page, int rows, int handlerId, int itemId)
        {
            var result = await pTaskRepo.GetListAsync(page, rows, handlerId, itemId);
            return Json(new { total = result.Item1, rows = result.Item2 });
        }
        public async Task<ActionResult> TaskPublishEdit(string id)
        {
            PersonTask task = await pTaskRepo.GetTaskAsync(id);

            var ItemList = new List<SelectListItem>();
            var items= await itemsRepo.GetListItemsAsync();
            var items2 = new SelectList(items, "ItemID", "NAME");
            ItemList.AddRange(items2);
            ViewBag.ItemList = ItemList;

            var ProdList = new List<SelectListItem>();
            var prods= await prodRepo.GetListAsync();
            var prods2 = new SelectList(prods, "ProID", "NAME");
            ProdList.AddRange(prods2);
            ViewBag.ProdList = ProdList;

            var TaskTypeList = new List<SelectListItem>();
            var tasktypes = await codeRepo.GetTaskTypeListAsync();
            var tasktypes2 = new SelectList(tasktypes,"ID","NAME");
            TaskTypeList.AddRange(tasktypes2);
            ViewBag.TaskTypeList = TaskTypeList;

            var TaskStatusList = new List<SelectListItem>();
            var taskstatus = await codeRepo.GetTaskStatusListAsync();
            var taskstatus2 = new SelectList(taskstatus, "id", "name");
            TaskStatusList.AddRange(taskstatus2);
            ViewBag.TaskStatusList = TaskStatusList;
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