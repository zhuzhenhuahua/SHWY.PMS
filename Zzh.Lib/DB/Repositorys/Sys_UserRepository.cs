using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zzh.Model.DB;

namespace Zzh.Lib.DB.Repositorys
{
    public class Sys_UserRepository : BaseRepository
    {
        public async Task<Tuple<int, List<Sys_User>>> GetList(int pageIndex, int pageSize, string userName)
        {
            int from = (pageIndex - 1) * pageSize;
            int total = await (from j in context.Sys_Users
                               where userName == "" ? 1 == 1 : j.Name.Contains(userName)
                               select j).CountAsync();
            var list = await (from j in context.Sys_Users
                              where userName == "" ? 1 == 1 : j.Name.Contains(userName)
                              orderby j.Uid descending
                              select j).Skip(from).Take(pageSize).ToListAsync();
            return Tuple.Create(total, list);
        }

        public async Task<Sys_User> GetUser(int uid)
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
        public async Task<bool> AddOrUpdate(Sys_User user)
        {
            try
            {
                var sysUser = await GetUser(user.Uid);
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
    }
}
