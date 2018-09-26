using SHWY.Lib.DB.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SHWY.Model.DB;
using SHWY.Utility;

namespace SHWY.PMS.Controllers
{
    public class PersonTaskController : BaseController
    {
        PersonTaskRepository pTaskRepo = new PersonTaskRepository(); //ersonTaskRepository.CreateInstance();
        PersonTaskProcessRepository pTaskProcessRepo = new PersonTaskProcessRepository();
        ItemsRepository itemsRepo = ItemsRepository.CreateInstance();
        ProductRepository prodRepo = ProductRepository.CreateInstance();
        CodeRepository codeRepo = CodeRepository.CreateInstance();
        Sys_UserRepository userRepo = new Sys_UserRepository();
        // GET: PersonTask
        public ActionResult TaskPublishList()
        {
            return View();
        }
        public async Task<JsonResult> GetList(int page, int rows, int handlerId, string itemId)
        {

            var result = await pTaskRepo.GetListAsync(page, rows, handlerId, itemId);
            return Json(new { total = result.Item1, rows = result.Item2 });
        }
        public async Task<ActionResult> TaskPublishEdit(string id)
        {
            PersonTask task = await pTaskRepo.GetTaskAsync(id);
            //产品
            var ItemList = new List<SelectListItem>();
            var items = await itemsRepo.GetListItemsAsync();
            var items2 = new SelectList(items, "ItemID", "NAME");
            ItemList.AddRange(items2);
            ViewBag.ItemList = ItemList;
            //项目
            var ProdList = new List<SelectListItem>();
            var prods = await prodRepo.GetListAsync();
            var prods2 = new SelectList(prods, "ProID", "NAME");
            ProdList.AddRange(prods2);
            ViewBag.ProdList = ProdList;
            //任务类型
            var TaskTypeList = new List<SelectListItem>();
            var tasktypes = await codeRepo.GetTaskTypeListAsync();
            var tasktypes2 = new SelectList(tasktypes, "ID", "NAME");
            TaskTypeList.AddRange(tasktypes2);
            ViewBag.TaskTypeList = TaskTypeList;
            //任务状态
            var TaskStatusList = new List<SelectListItem>();
            var taskstatus = await codeRepo.GetTaskStatusListAsync();
            var taskstatus2 = new SelectList(taskstatus, "id", "name");
            TaskStatusList.AddRange(taskstatus2);
            ViewBag.TaskStatusList = TaskStatusList;
            //人员
            var UserList = new List<SelectListItem>();
            var userlist = await userRepo.GetUserListAsync();
            var userlist2 = new SelectList(userlist, "Uid", "Name");
            UserList.AddRange(userlist2);
            ViewBag.UserList = UserList;

            var codes = await codeRepo.GetCodesListAsync();
            //完成程度
            var TaskComplDegree = new List<SelectListItem>();
            var taskDegree = codes.Where(p => p.TypeId == (int)ECodesTypeId.taskComplDegree).ToList();
            var taskDegree2 = new SelectList(taskDegree, "Code", "Text");
            TaskComplDegree.AddRange(taskDegree2);
            ViewBag.TaskComplDegreeV = TaskComplDegree;
            //服务态度
            var ServiceAttri = new List<SelectListItem>();
            var serAttri = codes.Where(p => p.TypeId == (int)ECodesTypeId.serviceAttri).ToList();
            var serAttri2 = new SelectList(serAttri, "Code", "Text");
            ServiceAttri.AddRange(serAttri2);
            ViewBag.ServiceAttriV = ServiceAttri;
            //完成速度
            var ComplSpeed = new List<SelectListItem>();
            var speed = codes.Where(p => p.TypeId == (int)ECodesTypeId.complSpeed).ToList();
            var speed2 = new SelectList(speed, "Code", "Text");
            ComplSpeed.AddRange(speed2);
            ViewBag.ComplSpeedV = ComplSpeed;
            //困难程度
            var TaskDiffLevel = new List<SelectListItem>();
            var diffLevel = codes.Where(p => p.TypeId == (int)ECodesTypeId.taskDiffLevel).ToList();
            var diffLevel2 = new SelectList(diffLevel, "Code", "Text");
            TaskDiffLevel.AddRange(diffLevel2);
            ViewBag.TaskDiffLevelV = TaskDiffLevel;



            if (id == "0" || string.IsNullOrEmpty(id))
            {
                task.publisherID = task.handlerID = task.followerID = CurrentUser.Sys_User.Uid;
            }
            else
            {
                if (task.complTime == null || task.complTime.Value.Year <= 1900)
                    task.complTime = null;
            }
            return View(task);
        }
        public ActionResult TaskProcessEdit()
        {
            return View();
        }
        public async Task<JsonResult> GetProcessListAsync(int page, int rows, string taskId)
        {
            var tuple = await pTaskProcessRepo.GetListAsync(page, rows, taskId);
            List<object> objList = new List<object>();
            foreach (var p in tuple.Item2)
            {
                objList.Add(new
                {
                    Id = p.Id,
                    WorkStartTime = p.WorkStartTime.ToString("yyyy-MM-dd HH:mm"),
                    WorkEndTime = p.WorkEndTime.ToString("yyyy-MM-dd HH:mm"),
                    Details = p.Details
                });
            }
            return Json(new { total = tuple.Item1, rows = objList });
        }
        public async Task<JsonResult> SavePersonProcess(PersonTaskProcess process)
        {
            if (process.Id == 0)
                process.addTime = DateTime.Now;
            var result = await pTaskProcessRepo.AddOrUpdateAsync(process);
            return Json(new { isOk = result });
        }
        public async Task<JsonResult> DelTaskProcess(int id)
        {
            var result = await pTaskProcessRepo.DelPersonProcessAsync(id);
            return Json(new { isOk = result });
        }
        public async Task<JsonResult> SaveTask(PersonTask task)
        {
            if (task.ID == "0" || string.IsNullOrEmpty(task.ID))
            {
                task.ID = CommonHelper.GetRandomString("TK");
                task.publishTime = DateTime.Now;
            }
            if (task.complTime == null || task.complTime.Value.Year == 1)
                task.complTime = Convert.ToDateTime("1900-01-01");
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