using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zzh.Model.DB;

namespace Zzh.Lib.DB.Repositorys
{
    public class Sys_RoleMenuRepository : BaseRepository
    {
        public async Task<bool> SaveRoleMenuAsync(int rid, List<int> menuListId)
        {
            try
            {
                var roleMenus = await context.Sys_RoleMenus.Where(p => p.RoleId == rid).ToListAsync();
                foreach (var item in roleMenus)
                {
                    context.Sys_RoleMenus.Remove(item);
                }
                await context.SaveChangesAsync();
                foreach (int menuid in menuListId)
                {
                    var newRoleMenu = await context.Sys_RoleMenus.Where(p => p.RoleId == rid && p.MenuId == menuid).FirstOrDefaultAsync();
                    if (newRoleMenu == null)
                    {
                        newRoleMenu = new Sys_RoleMenu();
                        newRoleMenu.RoleId = rid;
                        newRoleMenu.MenuId = menuid;
                        context.Sys_RoleMenus.Add(newRoleMenu);
                    }
                }
                int s = await context.SaveChangesAsync();
                return s > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Sys_RoleMenu>> GetListAsync(int rid)
        {
            return await context.Sys_RoleMenus.Where(p => p.RoleId == rid).ToListAsync();
        }
    }
}
