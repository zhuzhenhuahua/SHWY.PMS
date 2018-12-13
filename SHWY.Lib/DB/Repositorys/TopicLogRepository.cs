using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHWY.Model.DB;
using System.Data.Entity;

namespace SHWY.Lib.DB.Repositorys
{
    public class TopicLogRepository : BaseRepository
    {
        public async Task<Tuple<List<TopicLog>, int>> GetListAsync(int page, int rows, ETopic eTopic, string topicID)
        {
            int pageIndex = (page - 1) * rows;
            int topic = (int)eTopic;
            int total = await context.TopicLogs.Where(p => p.Topic == topic && p.TopicID == topicID).CountAsync();
            var list = await (from j in context.TopicLogs
                              where j.Topic == topic && j.TopicID == topicID
                              orderby j.addDate descending
                              select j).Skip(pageIndex).Take(rows).ToListAsync();
            return Tuple.Create(list, total);
        }
        public async Task<bool> AddOrUpdateAsync(TopicLog logPara)
        {
            var isAdd = false;
            var newLog = await context.TopicLogs.Where(p => p.Tid == logPara.Tid).FirstOrDefaultAsync();
            if (newLog == null)
            {
                isAdd = true;
                newLog = new TopicLog() { Topic = logPara.Topic, TopicID = logPara.TopicID };
            }
            newLog.addDate = logPara.addDate;
            newLog.ModuleID = logPara.ModuleID;
            newLog.Contents = logPara.Contents;
            if (isAdd)
                context.TopicLogs.Add(newLog);
            return await context.SaveChangesAsync() == 1;
        }
        public async Task<bool> DelAsync(int tid)
        {
            var model = await context.TopicLogs.Where(p => p.Tid == tid).FirstOrDefaultAsync();
            if (model != null)
            {
                context.TopicLogs.Remove(model);
                return await context.SaveChangesAsync() == 1;
            }
            return false;
        }
    }
}
