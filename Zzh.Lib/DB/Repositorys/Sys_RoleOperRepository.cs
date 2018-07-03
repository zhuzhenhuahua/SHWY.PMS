using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zzh.Model.DB;

namespace Zzh.Lib.DB.Repositorys
{
    public class Sys_RoleOperRepository : BaseRepository
    {
        public async Task<List<Sys_RoleOper>> GetListAsync(int roleId)
        {
            return await context.Sys_RoleOpers.Where(p => p.RoleId == roleId).ToListAsync();
        }
    }
}
