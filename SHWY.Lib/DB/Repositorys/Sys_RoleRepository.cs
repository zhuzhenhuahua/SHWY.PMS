using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHWY.Model.DB;

namespace SHWY.Lib.DB.Repositorys
{
    public class Sys_RoleRepository : BaseRepository
    {
        public async Task<List<Sys_Role>> GetListAsync()
        {
            var list = await (from j in context.Sys_Roles
                              orderby j.Rid descending
                              select j).ToListAsync();
            return list;
        }
        public async Task<Tuple<int, List<Sys_Role>>> GetListAsync(int page, int rows, string roleName)
        {
            int from = (page - 1) * rows;
            var total = await (from j in context.Sys_Roles
                               where j.RName.Contains(roleName)
                               select j).CountAsync();
            var list = await (from j in context.Sys_Roles
                              where j.RName.Contains(roleName)
                              orderby j.Rid descending
                              select j).Skip(from).Take(rows).ToListAsync();
            return Tuple.Create(total, list);
        }
        public async Task<Sys_Role> GetRoleAsync(int rid)
        {
            return await context.Sys_Roles.Where(p => p.Rid == rid).FirstOrDefaultAsync();
        }
        public async Task<bool> AddOrUpdateAsync(Sys_Role role)
        {
            try
            {
                var sysRole = await GetRoleAsync(role.Rid);
                bool isNew = false;

                if (sysRole == null)
                {
                    isNew = true;
                    sysRole = new Sys_Role();
                }
                foreach (var p in sysRole.GetType().GetProperties())
                {
                    //更新属性
                    var v = role.GetType().GetProperty(p.Name).GetValue(role);
                    if (v != null)
                    {
                        //其他字段更新
                        p.SetValue(sysRole, v);
                    }
                }
                if (isNew)
                    context.Sys_Roles.Add(sysRole);
                return await context.SaveChangesAsync() == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> DeleteRoleAsync(int rid)
        {
            var role = await context.Sys_Roles.Where(p => p.Rid == rid).FirstOrDefaultAsync();
            if (role != null)
            {
                var roleMenus = await context.Sys_RoleMenus.Where(p => p.RoleId == rid).ToListAsync();
                if(roleMenus.Count>0)
                {
                    context.Sys_RoleMenus.RemoveRange(roleMenus);//删除角色时，先删除角色下所有的菜单
                }
                context.Sys_Roles.Remove(role);
                return await context.SaveChangesAsync() > 0;
            }
            return false;
        }
    }
}
