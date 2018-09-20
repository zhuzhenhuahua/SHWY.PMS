using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHWY.Model.DB;

namespace SHWY.Lib.DB.Repositorys
{
    public class Sys_RoleMenuRepository : BaseRepository
    {
        Sys_RoleOperRepository repo_RoleOper = new Sys_RoleOperRepository();
        Sys_MenuOperRepository repo_menuOper = new Sys_MenuOperRepository();
        public async Task<bool> SaveRoleMenuAsync(int rid, List<int> menuListId)
        {
            try
            {
                var roleMenus = await context.Sys_RoleMenus.Where(p => p.RoleId == rid).ToListAsync();
                context.Sys_RoleMenus.RemoveRange(roleMenus);//先全部删除该角色下所有的菜单权限
                var roleMenuOper = await context.Sys_RoleOpers.Where(p => p.RoleId == rid).ToListAsync();
                context.Sys_RoleOpers.RemoveRange(roleMenuOper);//先全部删除该角色下所有的操作权限
                var menuOperList = await repo_menuOper.GetListAsync();//所有的实体，用于获取menuID
                //await context.SaveChangesAsync();
                foreach (int menuid in menuListId)
                {
                    if (menuid.ToString().Length == 7)
                    {
                        //插入Sys_RoleOper表
                        var newRoleOper = new Sys_RoleOper();
                        newRoleOper.RoleId = rid;
                        newRoleOper.MenuId = menuOperList.Where(p => p.MenuOperId == menuid).FirstOrDefault().MenuId;
                        newRoleOper.MenuOperId = menuid;
                        context.Sys_RoleOpers.Add(newRoleOper);
                    }
                    else
                    {
                        //插入Sys_RoleMenu表
                        var newRoleMenu = new Sys_RoleMenu();
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
