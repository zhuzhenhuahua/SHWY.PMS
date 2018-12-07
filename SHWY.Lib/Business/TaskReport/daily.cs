using SHWY.Lib.DB.Repositorys;
using SHWY.Model.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHWY.Lib.Business.TaskReport
{
    public class daily : ITaskReport
    {
        public async Task<string> CreateReport(List<int> userIDs, DateTime reportDate)
        {
            DateTime nowTime = reportDate;
            DateTime yesterDay = nowTime.AddDays(-1);
            StringBuilder sbDaily = new StringBuilder();
            using (PersonTaskRepository ptaskRepo = new PersonTaskRepository())
            {
                using (Sys_UserRepository userRepo = new Sys_UserRepository())
                {
                    var vTaskList = await ptaskRepo.GetPersonTaskDailyListAsync(userIDs, yesterDay, nowTime);//把昨天和今天的任务一次性查出来
                    foreach (var uid in userIDs)
                    {
                        var user = await userRepo.GetUserDicAsync(uid);
                        if (vTaskList.Where(p => p.handlerID == uid).FirstOrDefault() == null)
                        {
                            sbDaily.AppendLine("<br>-------" + user.Name + nowTime.ToString("yyyy-MM-dd") + "日报----------");
                            sbDaily.AppendLine("<br>昨天：");
                            sbDaily.AppendLine("<br>今天：");
                            continue;
                        }
                        sbDaily.AppendLine("<br>-------" + user.Name + nowTime.ToString("yyyy-MM-dd") + "日报----------");
                        sbDaily.AppendLine("<br>昨天：");
                        var y_source = (from j in vTaskList
                                        where j.handlerID == uid && j.DateLine == yesterDay
                                        select j).ToList();
                        sbDaily.AppendLine(GetContent(y_source, yesterDay));
                        sbDaily.AppendLine("<br>今天：");
                        var t_source = (from j in vTaskList
                                        where j.handlerID == uid && j.DateLine == nowTime
                                        select j).ToList();
                        sbDaily.AppendLine(GetContent(t_source, nowTime));
                    }
                }
            }
            return sbDaily.ToString();
        }
        private string GetContent(List<V_PersonTaskDaily> taskList, DateTime date)
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
                        var addDate = date.AddDays(1);
                        var daliyProcess = task.PersonTaskProcess.Where(j => j.WorkStartTime >= date && j.WorkEndTime < addDate).ToList();//日报只取当天的任务过程
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
    }
}
