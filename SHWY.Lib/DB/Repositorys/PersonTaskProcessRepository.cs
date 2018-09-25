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
        public async Task<List<PersonTaskProcess>> GetListAsync(string taskId)
        {
            var list = await (from j in context.PersonTaskProcess
                              where j.TaskId == taskId
                              select j).ToListAsync();
            return list;
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
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
