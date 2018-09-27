using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHWY.Model.DB;
using System.Data.Entity;

namespace SHWY.Lib.DB.Repositorys
{
    public class ServerRepository : BaseRepository
    {
        private static ServerRepository serverRepository;
        private static readonly object locker = new object();
        private ServerRepository()
        {

        }
        public static ServerRepository CreateInstance()
        {
            if (serverRepository == null)
            {
                lock (locker)
                {
                    if (serverRepository == null)
                        serverRepository = new ServerRepository();
                }
            }
            return serverRepository;
        }
        public async Task<Tuple<int, object>> GetServerListAsync(int pageIndex, int pageSize, string name)
        {
            int form = (pageIndex - 1) * pageSize;
            int total = await (from j in context.Servers
                               where (string.IsNullOrEmpty(name) ? 1 == 1 : j.name.Contains(name))
                               select j
                             ).CountAsync();
            var list = await (from j in context.Servers
                              join item in context.Items on j.itemid equals item.ItemID
                              where (string.IsNullOrEmpty(name) ? 1 == 1 : j.name.Contains(name))
                              orderby j.sid descending
                              select new
                              {
                                  j.itemid,
                                  itemName = item.NAME,
                                  j.loginName,
                                  j.name,
                                  j.password,
                                  j.remark,
                                  j.sid
                              }
                            ).Skip(form).Take(pageSize).ToListAsync();
            return Tuple.Create<int, object>(total, list);
        }
        public async Task<Servers> GetServerAsync(int sid)
        {
            var model = await context.Servers.Where(p => p.sid == sid).FirstOrDefaultAsync();
            return model;
        }
        public async Task<bool> DelServerAsync(int sid)
        {
            var model = await context.Servers.Where(p => p.sid == sid).FirstOrDefaultAsync();
            if (model != null)
            {
                context.Servers.Remove(model);
                return await context.SaveChangesAsync() == 1;
            }
            return false;

        }
        public async Task<bool> AddOrUpdateAsync(Servers serverPara)
        {
            var isAdd = false;
            var serverNew = await context.Servers.Where(p => p.sid == serverPara.sid).FirstOrDefaultAsync();
            if (serverNew == null)
            {
                isAdd = true;
                serverNew = new Servers() { sid = serverPara.sid };
            }
            serverNew.name = serverPara.name;
            serverNew.itemid = serverPara.itemid;
            serverNew.remark = serverPara.remark;
            serverNew.loginName = serverPara.loginName;
            serverNew.password = serverPara.password;
            if (isAdd)
                context.Servers.Add(serverNew);
            return await context.SaveChangesAsync() == 1;
        }
    }
}
