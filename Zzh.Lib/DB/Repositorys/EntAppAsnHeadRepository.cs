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

        public EntAppAsnHeadRepository() : base("name=LocalConnection")
        {

        }
        public async Task<Tuple<int, List<EntAppAsnHead>>> GetList(int pageIndex, int pageSize, string asn_no)
        {
            try
            {
                int total = await context.EntAppAsnHeads.Where(p => p.Asn_No.Contains(asn_no)).CountAsync();
                int from = (pageIndex - 1) * pageSize;
                var obj = await (from j in context.EntAppAsnHeads
                                 where j.Asn_No.Contains(asn_no)
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
