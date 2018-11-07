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
        public async Task<string> CreateReport(List<int> userIDs)
        {
            DateTime nowTime = DateTime.Now.Date;
            DateTime yesterday = DateTime.Now.AddDays(-1).Date;
            StringBuilder sbDaily = new StringBuilder();
            using (PersonTaskRepository ptaskRepo = new PersonTaskRepository())
            {
                var vTaskList = await ptaskRepo.GetTaskListAsync(yesterday, nowTime.AddDays(1), userIDs);//把昨天和今天的任务一次性查出来
                foreach (var uid in userIDs)
                {
                    if (vTaskList.Where(p => p.handlerID == uid).FirstOrDefault() == null)
                        continue;
                    string uName = vTaskList.Where(p => p.handlerID == uid).FirstOrDefault().handlerName;
                    sbDaily.AppendLine("<br>-------" + uName + nowTime.ToString("yyyy-MM-dd") + "日报----------");
                    sbDaily.AppendLine("<br>昨天：");
                    var y_source = (from j in vTaskList
                                    where j.handlerID==uid&&(
               (j.publishTimeDate >= yesterday && j.publishTimeDate < nowTime)
               || (j.predDeadTimeDate >= yesterday && j.predDeadTimeDate < nowTime))
                                    select j).ToList();
                    sbDaily.AppendLine(await GetContent(y_source, yesterday));
                    sbDaily.AppendLine("<br>今天：");
                    var t_source = (from j in vTaskList
                                    where j.handlerID==uid &&(
               (j.publishTimeDate >= nowTime && j.publishTimeDate < nowTime.AddDays(1))
               || (j.predDeadTimeDate >= nowTime && j.predDeadTimeDate < nowTime.AddDays(1)))
                                    select j).ToList();
                    sbDaily.AppendLine(await GetContent(t_source, nowTime));
                }
            }
            return sbDaily.ToString();
        }
        private async Task<string> GetContent(List<V_PersonTask> taskList,DateTime date)
        {
            using (PersonTaskProcessRepository ptaskProcessRepo = new PersonTaskProcessRepository())
            {
                StringBuilder sb = new StringBuilder();
                int tNum = 1;
                foreach (var task in taskList)
                {
                    sb.AppendLine("<br>"+tNum + "：" + task.detail+"（"+task.taskStatusName+"）");
                    task.PersonTaskProcess = await ptaskProcessRepo.GetListAsync(task.ID,date);
                    if (task.PersonTaskProcess != null && task.PersonTaskProcess.Count > 0)
                    {
                        for (int i = 0; i < task.PersonTaskProcess.Count; i++)
                        {
                            sb.AppendLine(" <br> (" + (i + 1) + "):" + task.PersonTaskProcess[i].Details);
                        }
                    }
                    tNum++;
                }
                return sb.ToString();
            }
        }
    }
}
