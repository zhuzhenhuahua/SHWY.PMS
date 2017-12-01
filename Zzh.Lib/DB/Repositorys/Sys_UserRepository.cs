using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zzh.Model.DB;

namespace Zzh.Lib.DB.Repositorys
{
    public class Sys_UserRepository : BaseRepository
    {
        public async Task<Tuple<int, List<Sys_User>>> GetList(int pageIndex, int pageSize, string userName)
        {
            int from = (pageIndex - 1) * pageSize;
            int total = await context.Sys_Users.Where(p => p.Name.Contains(userName)).CountAsync();
            var list = await (from j in context.Sys_Users
                              where j.Name.Contains(userName)
                              orderby j.Uid descending
                              select j).Skip(from).Take(pageSize).ToListAsync();
            return Tuple.Create(total, list);
        }
    }
}
