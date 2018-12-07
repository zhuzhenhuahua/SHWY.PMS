using SHWY.Lib.DB.Repositorys;
using SHWY.Model.DB;
using SHWY.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHWY.Lib.Business.TaskReport
{
    public class weekly : ITaskReport
    {
        public async Task<string> CreateReport(List<int> userIDs, DateTime reportDate)
        {
            DateTime nowTime = reportDate;
            var weekStart = CommonHelper.GetTimeStartByType(ETimeType.Week, nowTime).Value;
            var weekEnd = CommonHelper.GetTimeEndByType(ETimeType.Week, nowTime).Value;
            int weekOfYear = CommonHelper.GetWeekOfYear(weekStart);//当前第几周
            StringBuilder sbWeekly = new StringBuilder();
            using (PersonTaskRepository ptaskRepo = new PersonTaskRepository())
            {
                using (Sys_UserRepository userRepo = new Sys_UserRepository())
                {
                    var vTaskList = await ptaskRepo.GetPersonTaskDailyListAsync(userIDs, weekStart, weekEnd);//把本周的任务一次性查出来
                    var vTaskListDistinct = GetDistinct(vTaskList);//去重复
                    foreach (var uid in userIDs)
                    {
                        var user = await userRepo.GetUserDicAsync(uid);
                        var myTaskDaily = vTaskListDistinct.Where(p => p.handlerID == uid).ToList();//个人本周任务列表
                        if (myTaskDaily.Count == 0)
                        {
                            sbWeekly.AppendLine("<br>-------" + user.Name + "第" + weekOfYear + "周周报----------");
                            continue;
                        }
                        var myTaskGroupByProd = myTaskDaily.GroupBy(p => p.ProdName).Select(p => new { p.Key }).ToList();//按产品分组

                        sbWeekly.AppendLine("<br>-------" + user.Name + "第" + weekOfYear + "周周报----------");
                        foreach (var prod in myTaskGroupByProd)
                        {
                            sbWeekly.AppendLine("<br>产品：" + prod.Key);
                            var myProdTaskList = myTaskDaily.Where(p => p.ProdName == prod.Key).ToList();
                            sbWeekly.AppendLine(GetContent(myProdTaskList, weekStart));
                        }
                    }
                }
            }
            return sbWeekly.ToString();
        }
        private string GetContent(List<V_PersonTaskDaily> taskList, DateTime weekStart)
        {
            using (PersonTaskProcessRepository ptaskProcessRepo = new PersonTaskProcessRepository())
            {
                StringBuilder sb = new StringBuilder();
                int tNum = 1;
                foreach (var task in taskList)
                {
                    sb.AppendLine("<br>" + tNum + "：" + task.detail + "（" + task.taskStatusName + "）");//拼接任务描述
                    if (task.PersonTaskProcess != null && task.PersonTaskProcess.Count > 0)
                    {
                        var addDate = weekStart.AddDays(7);
                        var daliyProcess = task.PersonTaskProcess.Where(j => j.WorkStartTime >= weekStart && j.WorkEndTime < addDate).ToList();//日报只取当天的任务过程
                        for (int i = 0; i < daliyProcess.Count; i++)
                        {
                            sb.AppendLine(" <br> (" + (i + 1) + "):" + daliyProcess[i].Details);//拼接任务过程
                        }
                    }
                    tNum++;
                }
                return sb.ToString();
            }
        }
        /// <summary>
        /// 周报、月报统计时需要去掉重复的任务
        /// </summary>
        /// <returns></returns>
        private List<V_PersonTaskDaily> GetDistinct(List<V_PersonTaskDaily> dailyList)
        {
            List<V_PersonTaskDaily> distinct = new List<V_PersonTaskDaily>();
            foreach (var item in dailyList)
            {
                if (distinct.Where(p => p.TaskId == item.TaskId).Count() == 0)
                {
                    distinct.Add(item);
                }
            }
            return distinct;
        }
    }
}
