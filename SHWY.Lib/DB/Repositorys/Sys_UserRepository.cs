using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHWY.Model.DB;

namespace SHWY.Lib.DB.Repositorys
{
    public class Sys_UserRepository : BaseRepository
    {
        public async Task<Tuple<int, List<Sys_User>>> GetListAsync(int pageIndex, int pageSize, string userName)
        {
            int from = (pageIndex - 1) * pageSize;
            int total = await (from j in context.Sys_Users
                               where userName == "" ? 1 == 1 : j.Name.Contains(userName)
                               select j).CountAsync();
            var list = await (from j in context.Sys_Users
                              where userName == "" ? 1 == 1 : j.Name.Contains(userName)
                              orderby j.Uid descending
                              select j).Skip(from).Take(pageSize).ToListAsync();
            using (Sys_RoleRepository repoRole = new Sys_RoleRepository())
            {
                //这里后期需要优化
                foreach (var item in list)
                {
                    var role = await repoRole.GetRoleAsync(item.RoleId);
                    if (role != null)
                        item.RoleName = role.RName;
                }
            }
            return Tuple.Create(total, list);
        }

        public async Task<Sys_User> GetUserAsync(int uid)
        {
            try
            {
                var user = await (from j in context.Sys_Users
                                  where j.Uid == uid
                                  select j).FirstOrDefaultAsync();
                if (user == null)
                    return new Sys_User();
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Sys_User>> GetUserListAsync()
        {
            var list = await (from j in context.Sys_Users
                              orderby j.Name
                              select j).ToListAsync();
            return list;
        }
        public async Task<int> GetUserListCountByRoleID(int roleID)
        {
            var total = await context.Sys_Users.Where(p => p.RoleId == roleID).CountAsync();
            return total;
        }
        public async Task<Sys_User> GetUserAsync(string loginName, string pwd)
        {
            var user = await (from j in context.Sys_Users
                              where j.LoginName == loginName && j.PassWord == pwd
                              select j).FirstOrDefaultAsync();
            return user;
        }
        #region 增删改
        public async Task<bool> AddOrUpdateAsync(Sys_User user)
        {
            try
            {
                var sysUser = await GetUserAsync(user.Uid);
                bool isNew = false;

                if (sysUser.Uid.Equals(0))
                {
                    isNew = true;
                }
                foreach (var p in sysUser.GetType().GetProperties())
                {
                    //更新属性
                    var v = user.GetType().GetProperty(p.Name).GetValue(user);
                    if (v != null)
                    {
                        //其他字段更新
                        p.SetValue(sysUser, v);
                    }
                }
                if (isNew)
                    context.Sys_Users.Add(sysUser);
                return await context.SaveChangesAsync() == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteUser(int uid)
        {
            var user = await context.Sys_Users.Where(p => p.Uid == uid).FirstOrDefaultAsync();
            if (user != null)
            {
                context.Sys_Users.Remove(user);
                return await context.SaveChangesAsync() == 1;
            }
            return false;
        }
        #endregion
    }
}
