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
                              orderby j.MenuId, j.CreateTime descending
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
        public async Task<bool> AddOrUpdateAsync(Sys_MenuOper menuOper)
        {
            try
            {
                var sysMenuOper = await GetModelAsync(menuOper.MenuOperId);
                bool isNew = false;

                if (sysMenuOper == null)
                {
                    isNew = true;
                    sysMenuOper = new Sys_MenuOper();
                    sysMenuOper.CreateTime = DateTime.Now;
                }
                foreach (var p in sysMenuOper.GetType().GetProperties())
                {
                    //更新属性
                    var v = menuOper.GetType().GetProperty(p.Name).GetValue(menuOper);
                    if (v != null)
                    {
                        //其他字段更新
                        p.SetValue(sysMenuOper, v);
                    }
                }
                if (isNew)
                    context.Sys_MenuOpers.Add(sysMenuOper);
                return await context.SaveChangesAsync() == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> DelMenuOper(int menuOperId)
        {
            var model = await context.Sys_MenuOpers.Where(p => p.MenuOperId == menuOperId).FirstOrDefaultAsync();
            if (model == null)
                return false;
            context.Sys_MenuOpers.Remove(model);
            return await context.SaveChangesAsync() == 1;
        }
    }
}
