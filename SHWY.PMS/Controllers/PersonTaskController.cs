using SHWY.Lib.DB.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SHWY.Model.DB;
using SHWY.Utility;
using SHWY.PMS.Controllers.Filter;
using SHWY.Lib.Business.TaskReport;

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
        #region PersonTask任务
        //任务发布管理
        public ActionResult TaskPublishList()
        {
            return View();
        }
        //个人任务
        public ActionResult MyTaskList()
        {
            return View();
        }
        //任务查询
        public async Task<ActionResult> TaskQueryList()
        {
            var sessionUser = Session["CurrentUser"] as CurrentUser;
            TaskQueryViewModel model = new TaskQueryViewModel();
            model.userList = await userRepo.GetUserListAsync();
            model.currentUserID = sessionUser.Sys_User.Uid;
            return View(model);
        }
        public async Task<JsonResult> GetReport(string type, List<int> userIDs, DateTime reportDate)
        {
            if (string.IsNullOrEmpty(type))
                return null;
            string result = string.Empty;
            ITaskReport iReport = null;
            //根据类型不同创建不同的工厂
            if (type == "daily")
                iReport = new daily();
            else if (type == "weekly")
                iReport = new weekly();
            else if (type == "monthly")
                iReport = new monthly();
            result = await iReport.CreateReport(userIDs, reportDate);
            return Json(result);
        }
        public class TaskQueryViewModel
        {
            public List<Sys_User> userList { get; set; }
            public int currentUserID { get; set; }
        }
        //任务发布管理/任务查询列表
        public async Task<JsonResult> GetList(PersonTaskPara para)
        {
            var result = await pTaskRepo.GetListAsync(para.page, para.rows, para.handlerId, para.itemId, para.prodId, para.taskStatus, para.publishForm, para.publishTo);
            return Json(new { total = result.Item1, rows = result.Item2 });
        }
        //个人任务列表
        public async Task<JsonResult> GetMyTaskList(PersonTaskPara para)
        {
            var sessionUser = Session["CurrentUser"] as CurrentUser;
            var result = await pTaskRepo.GetListAsync(para.page, para.rows, sessionUser.Sys_User.Uid, para.itemId, para.prodId, para.taskStatus, para.publishForm, para.publishTo);
            return Json(new { total = result.Item1, rows = result.Item2 });
        }
        public async Task<ActionResult> TaskPublishEdit(string id)
        {
            PersonTask task = await pTaskRepo.GetTaskAsync(id);
            //项目
            var ItemList = new List<SelectListItem>();
            var items = await itemsRepo.GetListItemsAsync();
            var items2 = new SelectList(items, "ItemID", "NAME");
            ItemList.AddRange(items2);
            ViewBag.ItemList = ItemList;
            //产品
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

            //完成程度
            var TaskComplDegree = new List<SelectListItem>();
            var taskDegree = await codeRepo.GetCodesListAsync(ECodesTypeId.taskComplDegree);
            var taskDegree2 = new SelectList(taskDegree, "Code", "Text");
            TaskComplDegree.AddRange(taskDegree2);
            ViewBag.TaskComplDegreeV = TaskComplDegree;
            //服务态度
            var ServiceAttri = new List<SelectListItem>();
            var serAttri = await codeRepo.GetCodesListAsync(ECodesTypeId.serviceAttri);
            var serAttri2 = new SelectList(serAttri, "Code", "Text");
            ServiceAttri.AddRange(serAttri2);
            ViewBag.ServiceAttriV = ServiceAttri;
            //完成速度
            var ComplSpeed = new List<SelectListItem>();
            var speed = await codeRepo.GetCodesListAsync(ECodesTypeId.complSpeed);
            var speed2 = new SelectList(speed, "Code", "Text");
            ComplSpeed.AddRange(speed2);
            ViewBag.ComplSpeedV = ComplSpeed;
            //困难程度
            var TaskDiffLevel = new List<SelectListItem>();
            var diffLevel = await codeRepo.GetCodesListAsync(ECodesTypeId.taskDiffLevel);
            var diffLevel2 = new SelectList(diffLevel, "Code", "Text");
            TaskDiffLevel.AddRange(diffLevel2);
            ViewBag.TaskDiffLevelV = TaskDiffLevel;



            if (id == "0" || string.IsNullOrEmpty(id))
            {
                var session = Session["CurrentUser"] as CurrentUser;
                task.publisherID = task.handlerID = task.followerID = session.Sys_User.Uid;
                task.perdStartTime = DateTime.Now;
                task.predDeadTime = DateTime.Now.AddDays(1);
            }
            else
            {
                if (task.complTime == null || task.complTime.Value.Year <= 1900)
                    task.complTime = null;
            }
            return View(task);
        }
        public async Task<ActionResult> MyTaskPublishEdit(string taskId)
        {
            PersonTask task = await pTaskRepo.GetTaskAsync(taskId);
            if (task == null || string.IsNullOrEmpty(task.ID))
            {
                task.perdStartTime = DateTime.Now;
                task.predDeadTime = DateTime.Now.AddDays(1);
            }
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

            //完成程度
            var TaskComplDegree = new List<SelectListItem>();
            var taskDegree = await codeRepo.GetCodesListAsync(ECodesTypeId.taskComplDegree);
            var taskDegree2 = new SelectList(taskDegree, "Code", "Text");
            TaskComplDegree.AddRange(taskDegree2);
            ViewBag.TaskComplDegreeV = TaskComplDegree;
            //服务态度
            var ServiceAttri = new List<SelectListItem>();
            var serAttri = await codeRepo.GetCodesListAsync(ECodesTypeId.serviceAttri);
            var serAttri2 = new SelectList(serAttri, "Code", "Text");
            ServiceAttri.AddRange(serAttri2);
            ViewBag.ServiceAttriV = ServiceAttri;
            //完成速度
            var ComplSpeed = new List<SelectListItem>();
            var speed = await codeRepo.GetCodesListAsync(ECodesTypeId.complSpeed);
            var speed2 = new SelectList(speed, "Code", "Text");
            ComplSpeed.AddRange(speed2);
            ViewBag.ComplSpeedV = ComplSpeed;
            //困难程度
            var TaskDiffLevel = new List<SelectListItem>();
            var diffLevel = await codeRepo.GetCodesListAsync(ECodesTypeId.taskDiffLevel);
            var diffLevel2 = new SelectList(diffLevel, "Code", "Text");
            TaskDiffLevel.AddRange(diffLevel2);
            ViewBag.TaskDiffLevelV = TaskDiffLevel;
            if (string.IsNullOrEmpty(taskId) || taskId == "0")
            {
                var session = Session["CurrentUser"] as CurrentUser;
                task.publisherID = task.handlerID = task.followerID = session.Sys_User.Uid;
                task.perdStartTime = DateTime.Now;
                task.predDeadTime = DateTime.Now.AddDays(1);
            }
            else
            {
                if (task.complTime == null || task.complTime.Value.Year <= 1900)
                    task.complTime = null;
            }
            return View(task);
        }
        #endregion
        #region TaskStatus
        public async Task<JsonResult> GetAllTaskStatus(int isAddAll)
        {
            var result = await codeRepo.GetTaskStatusListAsync();
            if (isAddAll == 1)
                result.Insert(0, new CodeTaskStatus() { id = 0, name = "全部" });
            return Json(result);
        }
        #endregion
        #region PersonTaskProcess任务过程
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
        #endregion
        #region 任务过程增删改
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
        #endregion
        #region 任务增删改
        public async Task<JsonResult> SaveTask(PersonTask task)
        {
            if (task.ID == "0" || string.IsNullOrEmpty(task.ID))
            {
                task.ID = CommonHelper.GetRandomString("TK");
                task.publishTime = DateTime.Now;
            }
            if (task.complTime == null || task.complTime.Value.Year == 1900)
                task.complTime = Convert.ToDateTime("1900-01-01");
            var result = await pTaskRepo.AddOrUpdateAsync(task);
            return Json(new { isOk = result });
        }
        public async Task<JsonResult> DelTask(string id)
        {
            var result = await pTaskRepo.DeletePersonTask(id);
            return Json(new { isOk = result });
        }
        /// <summary>
        /// 快速标记
        /// </summary>
        /// <param name="taskType"></param>
        /// <returns></returns>
        public async Task<JsonResult> FastSign(string taskID, int taskStatus)
        {
            using (PersonTaskRepository repoTask = new PersonTaskRepository())
            {
                var model = await repoTask.GetTaskAsync(taskID);
                if (model == null || string.IsNullOrEmpty(model.ID))
                    return Json(new { isOk = false, msg = "未查询到该数据" });
                var result = await repoTask.UpdateTaskStatus(taskID, taskStatus);
                return Json(new { isOk = true });
            }
        }
        #endregion
    }
}