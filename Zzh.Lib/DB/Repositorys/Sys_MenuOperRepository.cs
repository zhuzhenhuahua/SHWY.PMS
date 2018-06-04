using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zzh.Model.DB;

namespace Zzh.Lib.DB.Repositorys
{
    public class Sys_MenuOperRepository : BaseRepository
    {
        public async Task<Tuple<int, object>> GetListPager(int page, int rows, int menuId)
        {
            int from = (page - 1) * rows;
            var list = await (from j in context.Sys_MenuOpers
                              join menu in context.Sys_Menus on j.MenuId equals menu.MenuId
                              where menuId > 0 ? j.MenuId == menuId : 1 == 1
                              orderby j.MenuId,j.CreateTime descending
                              select new
                              {
                                  MenuOperId = j.MenuOperId,
                                  MenuId = j.MenuId,
                                  MenuOperBtnID = j.MenuOperBtnID,
                                  MenuOperBtnName = j.MenuOperBtnName,
                                  MenuName = menu.MenuName
                              }
                              ).Skip(from).Take(rows).ToListAsync();
            var total = await (from j in context.Sys_MenuOpers
                               where menuId > 0 ? j.MenuId == menuId : 1 == 1
                               select j).CountAsync();
            return Tuple.Create(total, list as object);

        }
        public async Task<Sys_MenuOper> GetModelAsync(int menuOperId)
        {
            var model = await context.Sys_MenuOpers.Where(p => p.MenuOperId == menuOperId).FirstOrDefaultAsync();
            return model;
        }
    }
}
