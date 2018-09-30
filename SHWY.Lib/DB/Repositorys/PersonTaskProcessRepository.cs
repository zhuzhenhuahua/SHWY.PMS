using SHWY.Model.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHWY.Lib.DB.Repositorys
{
    public class PersonTaskProcessRepository : BaseRepository
    {
        public async Task<Tuple<int, List<PersonTaskProcess>>> GetListAsync(int pageIndex, int pageSize, string taskId)
        {
            int form = (pageIndex - 1) * pageSize;
            int total = await (from j in context.PersonTaskProcess
                               where j.TaskId == taskId
                               select j).CountAsync();
            var list = await (from j in context.PersonTaskProcess
                              where j.TaskId == taskId
                              orderby j.addTime descending
                              select j).Skip(form).Take(pageSize).ToListAsync();
            return Tuple.Create<int, List<PersonTaskProcess>>(total, list);
        }
        public async Task<bool> AddOrUpdateAsync(PersonTaskProcess process)
        {
            try
            {
                var processNew = await context.PersonTaskProcess.Where(p => p.Id == process.Id).FirstOrDefaultAsync();
                bool isNew = false;

                if (processNew == null || processNew.Id == 0)
                {
                    isNew = true;
                    processNew = new PersonTaskProcess();
                    processNew.addTime = DateTime.Now;
                }
                if (string.IsNullOrEmpty(processNew.TaskId))
                    processNew.TaskId = process.TaskId;
                processNew.WorkStartTime = process.WorkStartTime;
                processNew.WorkEndTime = process.WorkEndTime;
                processNew.Details = process.Details;
                if (isNew)
                    context.PersonTaskProcess.Add(processNew);
                return await context.SaveChangesAsync() == 1;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> DelPersonProcessAsync(int id)
        {
            var process = await context.PersonTaskProcess.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (process != null)
            {
                context.PersonTaskProcess.Remove(process);
                return await context.SaveChangesAsync() == 1;
            }
            return false;
        }
    }
}
