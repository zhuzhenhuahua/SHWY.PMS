using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHWY.Model.DB;
using System.Data.Entity;
using SHWY.Model.Join;

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
        public async Task<Tuple<int, object>> GetServerListAsync(int pageIndex, int pageSize, string name, string partyID)
        {
            int form = (pageIndex - 1) * pageSize;
            int total = await (from j in context.Servers
                               where (string.IsNullOrEmpty(name) ? 1 == 1 : j.name.Contains(name))
                               && (string.IsNullOrEmpty(partyID) ? 1 == 1 : j.partyId == partyID)
                               select j
                             ).CountAsync();
            var list = await (from j in context.Servers
                              join pary in context.Partys on j.partyId equals pary.PartyID into tempParty
                              from party in tempParty.DefaultIfEmpty()
                              where (string.IsNullOrEmpty(name) ? 1 == 1 : j.name.Contains(name))
                                 && (string.IsNullOrEmpty(partyID) ? 1 == 1 : j.partyId == partyID)
                              orderby j.sid descending
                              select new
                              {
                                  partyName = party == null ? "" : party.name,
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
        public async Task<List<Servers>> GetServerListByPartyIDAsync(string partyID)
        {
            var list = await context.Servers.Where(p => p.partyId == partyID).OrderByDescending(p => p.sid).ToListAsync();
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
                serverNew = new Servers();
            }
            serverNew.name = serverPara.name;
            serverNew.partyId = serverPara.partyId;
            serverNew.remark = serverPara.remark;
            serverNew.loginName = serverPara.loginName;
            serverNew.password = serverPara.password;
            if (isAdd)
                context.Servers.Add(serverNew);
            return await context.SaveChangesAsync() == 1;
        }
        #endregion

        #region IpAddress
        public async Task<Tuple<int, object>> GetIpAddressListAsync(int pageIndex, int pageSize, string ipAddress, int belong, string partyID)
        {
            int form = (pageIndex - 1) * pageSize;
            var total = await (from j in context.IpAddress
                               where (j.ipv4address.Contains(ipAddress))
                               && (belong < 0 ? 1 == 1 : j.belong == belong)
                               && (string.IsNullOrEmpty(partyID) ? 1 == 1 : j.partyID == partyID)
                               select j).CountAsync();
            var list = await (from j in context.IpAddress
                              join pary in context.Partys on j.partyID equals pary.PartyID into tempParty
                              from party in tempParty.DefaultIfEmpty()
                              where (j.ipv4address.Contains(ipAddress))
                             && (belong < 0 ? 1 == 1 : j.belong == belong)
                             && (string.IsNullOrEmpty(partyID) ? 1 == 1 : j.partyID == partyID)
                              orderby j.ipid descending
                              select new
                              {
                                  j.ipid,
                                  j.ipv4address,
                                  partyName = party == null ? "" : party.name,
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
        public async Task<List<IpAddress>> GetIpAddressListByPartyIDAsync(string partyID)
        {
            var list = await context.IpAddress.Where(p => p.partyID == partyID).OrderByDescending(p => p.ipid).ToListAsync();
            return list;
        }
        public async Task<List<IpAddress>> GetIpAddressListByPartyIDAsync(string partyID, int belong)
        {
            var list = await context.IpAddress.Where(p => p.partyID == partyID && p.belong == belong).OrderByDescending(p => p.ipid).ToListAsync();
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
                ipNew = new IpAddress();
            }
            ipNew.ipv4address = ipPara.ipv4address;
            ipNew.partyID = ipPara.partyID;
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
        public async Task<Tuple<int, object>> GetServerIpListAsync(int pageIndex, int pageSize, string serverName, string partyID)
        {
            int form = (pageIndex - 1) * pageSize;
            var total = await (from j in context.ServerIps
                               join server in context.Servers on j.sid equals server.sid
                               where (server.name.Contains(serverName))
                               && (string.IsNullOrEmpty(partyID) ? 1 == 1 : j.partyID == partyID)
                               select j).CountAsync();
            var list = await (from j in context.ServerIps
                              join paty in context.Partys on j.partyID equals paty.PartyID into tempParty
                              from party in tempParty.DefaultIfEmpty()
                              join server in context.Servers on j.sid equals server.sid
                              join ipAddress in context.IpAddress on j.ipid equals ipAddress.ipid
                              where (server.name.Contains(serverName))
                             && (string.IsNullOrEmpty(partyID) ? 1 == 1 : j.partyID == partyID)
                              orderby j.ipid descending
                              select new
                              {
                                  j.ipid,
                                  ipv4address = ipAddress.ipv4address,
                                  ipv6address = ipAddress.ipv6address,
                                  belong = ipAddress.belong,
                                  partyName = party == null ? "" : party.name,
                                  sid = server.sid,
                                  sName = server.name,
                                  server.loginName,
                                  server.password,
                                  server.remark
                              }).Skip(form).Take(pageSize).ToListAsync();
            return Tuple.Create<int, object>(total, list);
        }
        public async Task<List<ServerIPView>> GetServerIPListBySidAsync(List<int> sids)
        {
            var list = await (from j in context.ServerIps
                              join ip in context.IpAddress on j.ipid equals ip.ipid into tempIP
                              from ips in tempIP.DefaultIfEmpty()
                              where sids.Contains(j.sid)
                              select new ServerIPView
                              {
                                  sid = j.sid,
                                  IPAddress = ips
                              }).ToListAsync();
            return list;
        }
        public async Task<ServerIp> GetServerIpAsync(int sid, int ipid)
        {
            var model = await context.ServerIps.Where(p => p.sid == sid && p.ipid == ipid).FirstOrDefaultAsync();
            return model;
        }
        public async Task<int> GetServerIpCountBySidAsync(int sid)
        {
            var total = await context.ServerIps.Where(p => p.sid == sid).CountAsync();
            return total;
        }
        public async Task<int> GetServerIpCountByIpidAsync(int ipid)
        {
            var total = await context.ServerIps.Where(p => p.ipid == ipid).CountAsync();
            return total;
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
            model.partyID = serverIPPara.partyID;
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

        #region InPortOutPort
        public async Task<Tuple<int, object>> GetInPortOutPortListAsync(int pageIndex, int pageSize, string partyID)
        {
            int form = (pageIndex - 1) * pageSize;
            int total = await (from j in context.InPortOutPorts
                               where string.IsNullOrEmpty(partyID) ? 1 == 1 : j.partyID == partyID
                               select j).CountAsync();
            var obj = await (from j in context.InPortOutPorts
                             join paty in context.Partys on j.partyID equals paty.PartyID into tempParty
                             from party in tempParty.DefaultIfEmpty()
                             join InIp in context.IpAddress on j.inIpId equals InIp.ipid
                             join OutIp in context.IpAddress on j.outIpId equals OutIp.ipid
                             join portType in context.Codes.Where(p => p.TypeId == (int)ECodesTypeId.ProtType) on j.porttype.ToString() equals portType.Code
                             where string.IsNullOrEmpty(partyID) ? 1 == 1 : j.partyID == partyID
                             orderby j.ID descending
                             select new
                             {
                                 j.ID,
                                 partyName = party.name,
                                 inIpAddress = InIp.ipv4address,
                                 j.inPort,
                                 outIpAddress = OutIp.ipv4address,
                                 j.outPort,
                                 porttypeText = portType.Text
                             }).Skip(form).Take(pageSize).ToListAsync();
            return new Tuple<int, object>(total, obj);
        }
        public async Task<InPortOutPort> GetInPortOutPortAsync(int id)
        {
            var model = await context.InPortOutPorts.Where(p => p.ID == id).FirstOrDefaultAsync();
            return model;
        }
        public async Task<int> GetInPortOutPortCountByIpIDAsync(int ipid)
        {
            var total = await context.InPortOutPorts.Where(p => p.inIpId == ipid || p.outIpId == ipid).CountAsync();
            return total;
        }
        public async Task<bool> AddOrUpdateInPortOutPortAsync(InPortOutPort paraModel)
        {
            var isAdd = false;
            var model = await context.InPortOutPorts.Where(p => p.ID == paraModel.ID).FirstOrDefaultAsync();
            if (model == null)
            {
                isAdd = true;
                model = new InPortOutPort();
            }
            model.partyID = paraModel.partyID;
            model.inIpId = paraModel.inIpId;
            model.inPort = paraModel.inPort;
            model.outIpId = paraModel.outIpId;
            model.outPort = paraModel.outPort;
            model.porttype = paraModel.porttype;
            if (isAdd)
                context.InPortOutPorts.Add(model);
            return await context.SaveChangesAsync() == 1;
        }
        public async Task<bool> DelInPortOutPort(int id)
        {
            var model = await context.InPortOutPorts.Where(p => p.ID == id).FirstOrDefaultAsync();
            if (model != null)
            {
                context.InPortOutPorts.Remove(model);
                return await context.SaveChangesAsync() == 1;
            }
            return false;
        }
        #endregion

        #region DatabaseDepoly
        public async Task<Tuple<int, object>> GetDatabaseDeployListAsync(int pageIndex, int pageSize, string name, string partyID)
        {
            int form = (pageIndex - 1) * pageSize;
            int total = await (from j in context.DatabaseDeploys
                               where (j.name.Contains(name))
                               && (string.IsNullOrEmpty(partyID) ? 1 == 1 : j.partyID == partyID)
                               select j).CountAsync();
            var obj = await (from j in context.DatabaseDeploys
                             join pat in context.Partys on j.partyID equals pat.PartyID into tempParty
                             from party in tempParty.DefaultIfEmpty()
                             join ser in context.Servers on j.serverid equals ser.sid into tempServer
                             from server in tempServer.DefaultIfEmpty()
                             join schema in context.Codes.Where(p => p.TypeId == (int)ECodesTypeId.databaseSchema) on j.schemaid.ToString() equals schema.Code
                             join dbType in context.Codes.Where(p => p.TypeId == (int)ECodesTypeId.databaseType) on j.type.ToString() equals dbType.Code
                             where (j.name.Contains(name))
                               && (string.IsNullOrEmpty(partyID) ? 1 == 1 : j.partyID == partyID)
                             orderby j.id descending
                             select new
                             {
                                 j.id,
                                 j.name,
                                 PartyName = party == null ? "" : party.name,
                                 ServerName = server == null ? "" : server.name,
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
        public async Task<List<DatabaseDeploy>> GetDatabaseDeployListAsync(string partyID)
        {
            var list = await context.DatabaseDeploys.Where(p => p.partyID == partyID).ToListAsync();
            return list;
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
            model.partyID = dbDeploy.partyID;
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
