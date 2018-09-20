using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zzh.Model.DB;

namespace Zzh.Lib.DB.Repositorys
{
    public class ProductRepository : BaseRepository
    {
        public async Task<Tuple<int, List<Product>>> GetListAsync(int rows, int page)
        {
            int form = (page - 1) * rows;
            var total = await (from j in context.Products
                               select j).CountAsync();
            var list = await (from j in context.Products
                              orderby j.Pid
                              select j).Skip(form).Take(rows).ToListAsync();
            return Tuple.Create(total, list);
        }
    }
}
