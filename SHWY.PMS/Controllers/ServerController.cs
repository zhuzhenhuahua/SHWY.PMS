using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SHWY.Lib.DB.Repositorys;
using SHWY.Model.DB;

namespace SHWY.PMS.Controllers
{
    public class ServerController : BaseController
    {
        ServerRepository serverRepo = ServerRepository.CreateInstance();
         ItemsRepository itemsRepo = ItemsRepository.CreateInstance();
        PartyRepository partyRepo = PartyRepository.CreateInstance();
        CodeRepository codeRepo = CodeRepository.CreateInstance();
        ProductRepository prodRepo = ProductRepository.CreateInstance();
        #region Server查询
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> GetList(int page, int rows, string name, string partyID)
        {
            var tuple = await serverRepo.GetServerListAsync(page, rows, name, partyID);
            return Json(new { total = tuple.Item1, rows = tuple.Item2 });
        }
        public async Task<JsonResult> IsExists(Servers server)
        {
            var model = await serverRepo.GetServerAsync(server.sid);
            return Json(new { IsExists = model != null });
        }
        public async Task<JsonResult> GetAllServers(int isAddAll)
        {
            var result = await serverRepo.GetServerListAsync();
            if (isAddAll == 1)
                result.Insert(0, new Servers() { sid = 0, name = "全部" });
            return Json(result);
        }
        public async Task<JsonResult> GetServersByPartyID(string partyID)
        {
            var result = await serverRepo.GetServerListByPartyIDAsync(partyID);
            return Json(result);
        }
        #endregion

        #region IpAddress查询
        public ActionResult IpAddressIndex()
        {
            return View();
        }
        public async Task<JsonResult> GetIpAddressList(int page, int rows, string ipAddress, int belong, string partyID)
        {
            var tuple = await serverRepo.GetIpAddressListAsync(page, rows, ipAddress, belong, partyID);
            return Json(new { total = tuple.Item1, rows = tuple.Item2 });
        }
        public async Task<JsonResult> GetIpAddressListByPartyID(string partyID, int belong)
        {
            var list = await serverRepo.GetIpAddressListByPartyIDAsync(partyID, belong);
            return Json(list);
        }
        public async Task<JsonResult> GetIpAddressByPartyID(string partyID)
        {
            var list = await serverRepo.GetIpAddressListByPartyIDAsync(partyID);
            return Json(list);
        }
        public async Task<JsonResult> IsExistsIpAddressById(IpAddress ipModel)
        {
            var model = await serverRepo.GetIpAddressAsync(ipModel.ipid);
            return Json(new { IsExists = model != null });
        }
        #endregion

        #region ServerIp查询
        public ActionResult ServerIpIndex()
        {
            return View();
        }
        public async Task<JsonResult> GetServerIpList(int page, int rows, string serverName, string partyID)
        {
            var tuple = await serverRepo.GetServerIpListAsync(page, rows, serverName, partyID);
            return Json(new { total = tuple.Item1, rows = tuple.Item2 });
        }
        public async Task<JsonResult> IsExistsServerIp(ServerIp sip)
        {
            var model = await serverRepo.GetServerIpAsync(sip.sid, sip.ipid);
            return Json(new { IsExists = model != null });
        }
        #endregion

        #region databaseDeploy数据库查询
        public ActionResult DatabaseDeployIndex()
        {
            return View();
        }
        public async Task<JsonResult> GetDataBaseDeployList(int page, int rows, string name, string partyID)
        {
            var tuple = await serverRepo.GetDatabaseDeployListAsync(page, rows, name, partyID);
            return Json(new { total = tuple.Item1, rows = tuple.Item2 });
        }
        public async Task<JsonResult> GetDatabaseListByPartyID(string partyID)
        {
            var list = await serverRepo.GetDatabaseDeployListAsync(partyID);
            return Json(list);
        }
        public async Task<JsonResult> GetAllDatabase(int isAddAll)
        {
            var result = await serverRepo.GetDatabaseDeployListAsync();
            if (isAddAll == 1)
                result.Insert(0, new DatabaseDeploy() { id = 0, name = "全部" });
            return Json(result);
        }
        #endregion

        #region InPortOutPort端口映射查询
        public ActionResult InPortOutPortIndex()
        {
            return View();
        }
        public async Task<JsonResult> GetInPortOutPortList(int page, int rows, string partyID)
        {
            var tuple = await serverRepo.GetInPortOutPortListAsync(page, rows, partyID);
            return Json(new { total = tuple.Item1, rows = tuple.Item2 });
        }
        #endregion

        #region 增删改

        #region IpAddress操作
        public async Task<JsonResult> DelIpAddress(int ipid)
        {
            //删除时先判断服务器IP对应（ServerIp），端口映射（InPortOutPort）中是否有引用数据
            int total = await serverRepo.GetServerIpCountByIpidAsync(ipid);
            if (total > 0)
                return Json(new { isOk = false, msg = "先删除服务器IP对应数据" });

            var count = await serverRepo.GetInPortOutPortCountByIpIDAsync(ipid);
            if (count > 0)
                return Json(new { isOk = false, msg = "先删除端口映射对应数据" });

            var result = await serverRepo.DelIpAddressAsync(ipid);
            return Json(new { isOk = result, msg = "" });
        }
        public async Task<ActionResult> IpAddressEdit(int ipid)
        {
            var model = await serverRepo.GetIpAddressAsync(ipid);
            var PartyList = new List<SelectListItem>();
            var party = await partyRepo.GetPartyListAsync();
            var party2 = new SelectList(party, "PartyID", "name");
            PartyList.AddRange(party2);
            ViewBag.PartyList = PartyList;

            var BeLongList = new List<SelectListItem>();
            var belong = await CodeRepository.CreateInstance().GetCodesListAsync(ECodesTypeId.IpAddressBeLong);
            var belong2 = new SelectList(belong, "Code", "Text");
            BeLongList.AddRange(belong2);
            ViewBag.BeLongList = BeLongList;

            return View(model);
        }
        public async Task<JsonResult> SaveIpAddress(IpAddress model)
        {
            var result = await serverRepo.AddOrUpdateIpAddress(model);
            return Json(new { isOk = result });
        }
        #endregion

        #region Server操作
        public async Task<ActionResult> ServerEdit(int sid)
        {
            var server = await serverRepo.GetServerAsync(sid);
            var PartyList = new List<SelectListItem>();
            var party = await partyRepo.GetPartyListAsync();
            var party2 = new SelectList(party, "PartyID", "name");
            PartyList.AddRange(party2);
            ViewBag.PartyList = PartyList;
            return View(server);
        }
        public async Task<JsonResult> SaveServer(Servers server)
        {
            var result = await serverRepo.AddOrUpdateAsync(server);
            return Json(new { isOk = result });
        }
        public async Task<JsonResult> DelServer(int sid)
        {
            //删除服务器时先判断服务器ip（ServerIp），产品服务器（ProdServerDeploy）中是否有数据
            var total = await serverRepo.GetServerIpCountBySidAsync(sid);
            if (total > 0)
                return Json(new { isOk = false, msg = "先删除服务器IP对应数据" });
            var count = await prodRepo.GetProdServerCountByServerIDAsync(sid);
            if (count > 0)
                return Json(new { isOk = false, msg = "先删除产品服务器对应数据" });
            var result = await serverRepo.DelServerAsync(sid);
            return Json(new { isOk = result, msg = "" });
        }
        #endregion

        #region ServerIP操作
        public async Task<ActionResult> ServerIpEdit(int sid, int ipid)
        {
            var serverIP = await serverRepo.GetServerIpAsync(sid, ipid);
            if (serverIP == null)
                serverIP = new ServerIp();
            //项目
            var PartyList = new List<SelectListItem>();
            var party = await partyRepo.GetPartyListAsync();
            var party2 = new SelectList(party, "PartyID", "name");
            PartyList.AddRange(party2);
            ViewBag.PartyList = PartyList;
            return View(serverIP);
        }
        public async Task<JsonResult> DelServerIp(int sid, int ipid)
        {
            var result = await serverRepo.DelServerIpsAsync(sid, ipid);
            return Json(new { isOk = result });
        }
        public async Task<JsonResult> SaveServerIp(ServerIp serIp)
        {
            var result = await serverRepo.AddOrUpdateServerIpAsync(serIp);
            return Json(new { isOk = result });
        }
        #endregion

        #region databaseDeploy数据库管理操作
        public async Task<ActionResult> DatabaseDeployEdit(int id)
        {
            var model = await serverRepo.GetDatabaseDeployAsync(id);
            //项目
            var PartyList = new List<SelectListItem>();
            var party = await partyRepo.GetPartyListAsync();
            var party2 = new SelectList(party, "PartyID", "name");
            PartyList.AddRange(party2);
            ViewBag.PartyList = PartyList;
            //数据库架构
            var SchemaidList = new List<SelectListItem>();
            var sehemalist = await codeRepo.GetCodesListAsync(ECodesTypeId.databaseSchema);
            var sehemalist2 = new SelectList(sehemalist, "Code", "Text");
            SchemaidList.AddRange(sehemalist2);
            ViewBag.SchemaidList = SchemaidList;
            //数据库类型
            var DBTypeList = new List<SelectListItem>();
            var typelist = await codeRepo.GetCodesListAsync(ECodesTypeId.databaseType);
            var typelist2 = new SelectList(typelist, "Code", "Text");
            DBTypeList.AddRange(typelist2);
            ViewBag.DBTypeList = DBTypeList;
            if (model == null)
                model = new DatabaseDeploy();

            return View(model);
        }
        public async Task<JsonResult> SaveDatabaseDeploy(DatabaseDeploy dbDeploy)
        {
            var result = await serverRepo.AddOrUpdateDatabaseDeploy(dbDeploy);
            return Json(new { isOk = result });
        }
        public async Task<JsonResult> DelDatabaseDeploy(int id)
        {
            var t = typeof(ECodesTypeId);

            //删除时先判断产品数据库对应（ProdDBDeploy）中是否有对应数据
            var total = await prodRepo.GetProdDBDeployCountByDBIDAsync(id);
            if (total > 0)
                return Json(new { isOk = false, msg = "先删除产品数据库对应数据" });

            var result = await serverRepo.DelDatabaseDeploy(id);
            return Json(new { isOk = result });
        }
        #endregion

        #region InPortOutPort端口映射增删改
        public async Task<ActionResult> InPortOutPortEdit(int Id)
        {
            var model = await serverRepo.GetInPortOutPortAsync(Id);
            //项目
            var PartyList = new List<SelectListItem>();
            var party = await partyRepo.GetPartyListAsync();
            var party2 = new SelectList(party, "PartyID", "name");
            PartyList.AddRange(party2);
            ViewBag.PartyList = PartyList;
            //端口类型
            var PortTypeList = new List<SelectListItem>();
            var typelist = await codeRepo.GetCodesListAsync(ECodesTypeId.ProtType);
            var typelist2 = new SelectList(typelist, "Code", "Text");
            PortTypeList.AddRange(typelist2);
            ViewBag.PortTypeList = PortTypeList;
            if (model == null)
                model = new InPortOutPort();
            return View(model);
        }
        public async Task<JsonResult> SaveInPortOutPort(InPortOutPort port)
        {
            var result = await serverRepo.AddOrUpdateInPortOutPortAsync(port);
            return Json(new { isOk = result });
        }
        public async Task<JsonResult> DelInPortOutPort(int Id)
        {
            var result = await serverRepo.DelInPortOutPort(Id);
            return Json(new { isOk = result });
        }
        #endregion

        #endregion
    }
}