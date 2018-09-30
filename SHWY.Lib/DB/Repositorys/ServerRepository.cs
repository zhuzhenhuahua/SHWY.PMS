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

        #region Server
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
        public async Task<List<Servers>> GetServerListAsync()
        {
            var list = await context.Servers.OrderByDescending(p => p.sid).ToListAsync();
            return list;
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
        #endregion

        #region IpAddress
        public async Task<Tuple<int, object>> GetIpAddressListAsync(int pageIndex, int pageSize, string ipAddress)
        {
            int form = (pageIndex - 1) * pageSize;
            var total = await (from j in context.IpAddress
                               where j.ipv4address.Contains(ipAddress)
                               select j).CountAsync();
            var list = await (from j in context.IpAddress
                              join item in context.Items on j.itemid.ToString() equals item.ItemID
                              where j.ipv4address.Contains(ipAddress)
                              orderby j.ipid descending
                              select new
                              {
                                  j.ipid,
                                  j.ipv4address,
                                  j.itemid,
                                  itemName = item.NAME,
                                  j.ipv6address,
                                  j.belong
                              }).Skip(form).Take(pageSize).ToListAsync();
            return Tuple.Create<int, object>(total, list);
        }
        public async Task<List<IpAddress>> GetIpAddressListAsync()
        {
            var list = await context.IpAddress.OrderByDescending(p => p.ipid).ToListAsync();
            return list;
        }
        public async Task<IpAddress> GetIpAddressAsync(int ipid)
        {
            var model = await context.IpAddress.Where(p => p.ipid == ipid).FirstOrDefaultAsync();
            return model;
        }

        public async Task<bool> AddOrUpdateIpAddress(IpAddress ipPara)
        {
            var isAdd = false;
            var ipNew = await context.IpAddress.Where(p => p.ipid == ipPara.ipid).FirstOrDefaultAsync();
            if (ipNew == null)
            {
                isAdd = true;
                ipNew = new IpAddress() { ipid = ipPara.ipid };
            }
            ipNew.ipv4address = ipPara.ipv4address;
            ipNew.itemid = ipPara.itemid;
            ipNew.ipv6address = ipPara.ipv6address;
            ipNew.belong = ipPara.belong;
            if (isAdd)
                context.IpAddress.Add(ipNew);
            return await context.SaveChangesAsync() == 1;
        }
        public async Task<bool> DelIpAddressAsync(int ipid)
        {
            var model = await context.IpAddress.Where(p => p.ipid == ipid).FirstOrDefaultAsync();
            if (model != null)
            {
                context.IpAddress.Remove(model);
                return await context.SaveChangesAsync() == 1;
            }
            return false;
        }
        #endregion

        #region ServerIp
        public async Task<Tuple<int, object>> GetServerIpListAsync(int pageIndex, int pageSize, string serverName)
        {
            int form = (pageIndex - 1) * pageSize;
            var total = await (from j in context.ServerIps
                               join server in context.Servers on j.sid equals server.sid
                               where server.name.Contains(serverName)
                               select j).CountAsync();
            var list = await (from j in context.ServerIps
                              join item in context.Items on j.itemid.ToString() equals item.ItemID
                              join server in context.Servers on j.sid equals server.sid
                              join ipAddress in context.IpAddress on j.ipid equals ipAddress.ipid
                              where server.name.Contains(serverName)
                              orderby j.ipid descending
                              select new
                              {
                                  j.ipid,
                                  ipv4address = ipAddress.ipv4address,
                                  ipv6address = ipAddress.ipv6address,
                                  belong = ipAddress.belong,
                                  j.itemid,
                                  itemName = item.NAME,
                                  sid = server.sid,
                                  sName = server.name,
                                  server.loginName,
                                  server.password,
                                  server.remark
                              }).Skip(form).Take(pageSize).ToListAsync();
            return Tuple.Create<int, object>(total, list);
        }
        public async Task<ServerIp> GetServerIpAsync(int sid, int ipid)
        {
            var model = await context.ServerIps.Where(p => p.sid == sid && p.ipid == ipid).FirstOrDefaultAsync();
            return model;
        }
        public async Task<bool> AddOrUpdateServerIpAsync(ServerIp serverIPPara)
        {
            var isAdd = false;
            var model = await context.ServerIps.Where(p => p.sid == serverIPPara.sid && p.ipid == serverIPPara.ipid).FirstOrDefaultAsync();
            if (model == null)
            {
                isAdd = true;
                model = new ServerIp() { ipid = serverIPPara.ipid, sid = serverIPPara.sid };
            }
            model.itemid = serverIPPara.itemid;
            if (isAdd)
                context.ServerIps.Add(model);
            return await context.SaveChangesAsync() == 1;
        }
        public async Task<bool> DelServerIpsAsync(int sid, int ipid)
        {
            var model = await context.ServerIps.Where(p => p.sid == sid && p.ipid == ipid).FirstOrDefaultAsync();
            if (model != null)
            {
                context.ServerIps.Remove(model);
                return await context.SaveChangesAsync() == 1;
            }
            return false;
        }
        #endregion

        #region DatabaseDepoly
        public async Task<Tuple<int, object>> GetDatabaseDeployListAsync(int pageIndex, int pageSize, string name)
        {
            int form = (pageIndex - 1) * pageSize;
            int total = await (from j in context.DatabaseDeploys
                               where j.name.Contains(name)
                               select j).CountAsync();
            var obj = await (from j in context.DatabaseDeploys
                             join item in context.Items on j.itemid.ToString() equals item.ItemID
                             join server in context.Servers on j.serverid equals server.sid
                             join schema in context.Codes.Where(p => p.TypeId == (int)ECodesTypeId.databaseSchema) on j.schemaid.ToString() equals schema.Code
                             join dbType in context.Codes.Where(p => p.TypeId == (int)ECodesTypeId.databaseType) on j.type.ToString() equals dbType.Code
                             where j.name.Contains(name)
                             orderby j.id descending
                             select new
                             {
                                 j.id,
                                 j.name,
                                 ItemName = item.NAME,
                                 ServerName = server.name,
                                 SchemaName = schema.Text,
                                 dbType = dbType.Text,
                                 j.remark,
                                 j.sqlServerCatlog,
                                 j.mongoAdminDBName,
                                 j.mongoDBName,
                                 j.orclServiceName,
                                 j.username,
                                 j.password
                             }).Skip(form).Take(pageSize).ToListAsync();
            return Tuple.Create<int, object>(total, obj);
        }
        public async Task<List<DatabaseDeploy>> GetDatabaseDeployListAsync()
        {
            var list = await context.DatabaseDeploys.ToListAsync();
            return list;
        }
        public async Task<DatabaseDeploy> GetDatabaseDeployAsync(int id)
        {
            var model = await context.DatabaseDeploys.Where(p => p.id == id).FirstOrDefaultAsync();
            return model;
        }
        public async Task<bool> AddOrUpdateDatabaseDeploy(DatabaseDeploy dbDeploy)
        {
            var isAdd = false;
            var model = await context.DatabaseDeploys.Where(p => p.id == dbDeploy.id).FirstOrDefaultAsync();
            if (model == null)
            {
                isAdd = true;
                model = new DatabaseDeploy();
            }
            model.name = dbDeploy.name;
            model.itemid = dbDeploy.itemid;
            model.serverid = dbDeploy.serverid;
            model.schemaid = dbDeploy.schemaid;
            model.type = dbDeploy.type;
            model.remark = dbDeploy.remark;
            model.sqlServerCatlog = dbDeploy.sqlServerCatlog;
            model.mongoAdminDBName = dbDeploy.mongoAdminDBName;
            model.mongoDBName = dbDeploy.mongoDBName;
            model.orclServiceName = dbDeploy.orclServiceName;
            model.username = dbDeploy.username;
            model.password = dbDeploy.password;
            if (isAdd)
                context.DatabaseDeploys.Add(model);
            return await context.SaveChangesAsync() == 1;
        }
        public async Task<bool> DelDatabaseDeploy(int id)
        {
            var model = await context.DatabaseDeploys.Where(p => p.id == id).FirstOrDefaultAsync();
            if (model != null)
            {
                context.DatabaseDeploys.Remove(model);
                return await context.SaveChangesAsync() == 1;
            }
            return false;
        }
        #endregion
    }
}
