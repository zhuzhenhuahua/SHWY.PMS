using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zzh.Model.DB;

namespace Zzh.Lib.DB.Repositorys
{
    public class EntAppAsnHeadRepository : BaseRepository
    {

        public EntAppAsnHeadRepository() : base("name=Default2Connection")
        {

        }
        public async Task<Tuple<int, List<EntAppAsnHead>>> GetList(int pageIndex, int pageSize)
        {
            try
            {
                int total = await context.EntAppAsnHeads.CountAsync();
                int from = (pageIndex - 1) * pageSize;
                var obj = await (from j in context.EntAppAsnHeads
                                 orderby j.id
                                 select j).Skip(from).Take(pageSize).ToListAsync();
                return Tuple.Create(total, obj);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
