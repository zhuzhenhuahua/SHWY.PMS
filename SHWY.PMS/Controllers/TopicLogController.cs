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
    public class TopicLogController : BaseController
    {
        // GET: TopicLog
        public ActionResult TopicIndex(string topicID,int topic = 0)
        {
            TopicParaModel para = new TopicParaModel() { Topic = topic, nowTime=DateTime.Now.ToString("yyyy-MM-dd"), TopicID= topicID };
            return View(para);
        }
        public ActionResult ProTopicIndex(string topicID, int topic = 0)
        {
            TopicParaModel para = new TopicParaModel() { Topic = topic, nowTime = DateTime.Now.ToString("yyyy-MM-dd"), TopicID = topicID };
            return View(para);
        }
        public async Task<JsonResult> GetTopicLogListAsync(int page, int rows, int eTopic, string topicID)
        {
            using (TopicLogRepository repo = new TopicLogRepository())
            {
                ETopic topic = (ETopic)eTopic;
                var tuple = await repo.GetListAsync(page, rows, topic, topicID);
                List<object> objList = new List<object>();
                foreach (var log in tuple.Item1)
                {
                    objList.Add(new
                    {
                        Tid = log.Tid,
                        log.Topic,
                        log.TopicID,
                        ModuleID=log.ModuleID,
                        addDate = log.addDate.ToString("yyyy-MM-dd"),
                        log.Contents
                    });
                }
                return Json(new { total = tuple.Item2, rows = objList });
            }
        }
        public async Task<JsonResult> SaveTopicLog(TopicLog log)
        {
            using (TopicLogRepository repo = new TopicLogRepository())
            {
                var res = await repo.AddOrUpdateAsync(log);
                return Json(new { isOk = res });
            }
        }
        public async Task<JsonResult> DelTopicLog(int tid)
        {
            using (TopicLogRepository repo = new TopicLogRepository())
            {
                var res = await repo.DelAsync(tid);
                return Json(new { isOk = res });
            }
        }
        public class TopicParaModel
        {
            public string nowTime { get; set; }
            public int Topic { get; set; }
            public string TopicID { get; set; }
        }
    }
}