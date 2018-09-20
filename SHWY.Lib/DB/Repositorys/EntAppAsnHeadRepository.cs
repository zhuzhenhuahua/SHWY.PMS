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

        public EntAppAsnHeadRepository() : base("name=092Connection")
        {

        }
        public async Task<Tuple<int, List<EntAppAsnHead>>> GetListAsync(int pageIndex, int pageSize, string asn_no, string bill_No_Split)
        {
            try
            {
                int total = await context.EntAppAsnHeads.Where(p => p.Asn_No.Contains(asn_no)).CountAsync();
                int from = (pageIndex - 1) * pageSize;
                var obj = await (from j in context.EntAppAsnHeads
                                 where j.Asn_No.Contains(asn_no) && j.Bill_No_Split.Contains(bill_No_Split)
                                 orderby j.id
                                 select j).Skip(from).Take(pageSize).ToListAsync();
                return Tuple.Create(total, obj);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<EntAppAsnHead> GetEntAppAsnHeadAsync(int id)
        {
            var obj = await (from j in context.EntAppAsnHeads
                             where j.id == id
                             select j).FirstOrDefaultAsync();
            return obj;
        }
        public async Task<bool> UpdateAsync(EntAppAsnHead head)
        {
            var asnObj =await context.EntAppAsnHeads.Where(p => p.id == head.id).FirstOrDefaultAsync();
            if (asnObj == null)
                return false;
            var sjObj = await context.EndAppSjHeads.Where(p => p.Bill_No_Split == asnObj.Bill_No_Split).FirstOrDefaultAsync();
            if (sjObj == null)
                return false;
            asnObj.Bill_No_Split = head.Bill_No_Split;
            sjObj.Bill_No_Split = head.Bill_No_Split;
            return await context.SaveChangesAsync() >0;
        }
    }
}
