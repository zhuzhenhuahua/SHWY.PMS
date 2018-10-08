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
        CodeRepository codeRepo = CodeRepository.CreateInstance();
        #region Server查询
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> GetList(int page, int rows, string name,string itemID)
        {
            var tuple = await serverRepo.GetServerListAsync(page, rows, name, itemID);
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
        public async Task<JsonResult> GetServersByItemID(string itemID)
        {
            var result = await serverRepo.GetServerListByItemIDAsync(itemID);
            return Json(result);
        }
        #endregion

        #region IpAddress查询
        public ActionResult IpAddressIndex()
        {
            return View();
        }
        public async Task<JsonResult> GetIpAddressList(int page, int rows, string ipAddress,int belong,string itemID)
        {
            var tuple = await serverRepo.GetIpAddressListAsync(page, rows, ipAddress, belong, itemID);
            return Json(new { total = tuple.Item1, rows = tuple.Item2 });
        }
        public async Task<JsonResult> GetIpAddressListByItemID(int itemID, int belong)
        {
            var list = await serverRepo.GetIpAddressListByItemIDAsync(itemID, belong);
            return Json(list);
        }
        public async Task<JsonResult> GetIpAddressByItemID(int itemID)
        {
            var list = await serverRepo.GetIpAddressListByItemIDAsync(itemID);
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
        public async Task<JsonResult> GetServerIpList(int page, int rows, string serverName,string ItemID)
        {
            var tuple = await serverRepo.GetServerIpListAsync(page, rows, serverName, ItemID);
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
        public async Task<JsonResult> GetDataBaseDeployList(int page, int rows, string name,string itemID)
        {
            var tuple = await serverRepo.GetDatabaseDeployListAsync(page, rows, name, itemID);
            return Json(new { total = tuple.Item1, rows = tuple.Item2 });
        }
        public async Task<JsonResult> GetDatabaseListByItemID(int itemID)
        {
            var list = await serverRepo.GetDatabaseDeployListAsync(itemID);
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

        #region InPortOutPort端口映射
        public ActionResult InPortOutPortIndex()
        {
            return View();
        }
        public async Task<JsonResult> GetInPortOutPortList(int page, int rows, string itemID)
        {
            var tuple = await serverRepo.GetInPortOutPortListAsync(page, rows, itemID);
            return Json(new { total = tuple.Item1, rows = tuple.Item2 });
        }
        public async Task<ActionResult> InPortOutPortEdit(int Id)
        {
            var model = await serverRepo.GetInPortOutPortAsync(Id);
            //项目
            var ItemList = new List<SelectListItem>();
            var items = await itemsRepo.GetListItemsAsync();
            var items2 = new SelectList(items, "ItemID", "NAME");
            ItemList.AddRange(items2);
            ViewBag.ItemList = ItemList;
            //内网IP
            //var iplist = await serverRepo.GetIpAddressListAsync();
            //var InIp4List = new List<SelectListItem>();
            //var iniplist2 = new SelectList(iplist.Where(p=>p.belong==0), "ipid", "ipv4address");
            //InIp4List.AddRange(iniplist2);
            //ViewBag.InIp4List = InIp4List;
            //外网IP
            //var OutIp4List = new List<SelectListItem>();
            //var outiplist2 = new SelectList(iplist.Where(p => p.belong == 1), "ipid", "ipv4address");
            //OutIp4List.AddRange(outiplist2);
            //ViewBag.OutIp4List = OutIp4List;
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

        #region 增删改

        #region IpAddress操作
        public async Task<JsonResult> DelIpAddress(int ipid)
        {
            var result = await serverRepo.DelIpAddressAsync(ipid);
            return Json(new { isOk = result });
        }
        public async Task<ActionResult> IpAddressEdit(int ipid)
        {
            var model = await serverRepo.GetIpAddressAsync(ipid);
            var ItemList = new List<SelectListItem>();
            var items = await itemsRepo.GetListItemsAsync();
            var items2 = new SelectList(items, "ItemID", "NAME");
            ItemList.AddRange(items2);
            ViewBag.ItemList = ItemList;

            var BeLongList = new List<SelectListItem>();
            var belong = await CodeRepository.CreateInstance().GetCodesListAsync(ECodesTypeId.IpAddressBeLong);
            var belong2 = new SelectList(belong, "Code", "Text");
            BeLongList.AddRange(belong2);
            ViewBag.BeLongList = BeLongList;

            ViewBag.isRealOnly = (model != null).ToString().ToLower();
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
            var ItemList = new List<SelectListItem>();
            var items = await itemsRepo.GetListItemsAsync();
            var items2 = new SelectList(items, "ItemID", "NAME");
            ItemList.AddRange(items2);
            ViewBag.ItemList = ItemList;
            ViewBag.isRealOnly = (server != null).ToString().ToLower();
            return View(server);
        }
        public async Task<JsonResult> SaveServer(Servers server)
        {
            var result = await serverRepo.AddOrUpdateAsync(server);
            return Json(new { isOk = result });
        }
        public async Task<JsonResult> DelServer(int sid)
        {
            var result = await serverRepo.DelServerAsync(sid);
            return Json(new { isOk = result });
        }
        #endregion

        #region ServerIP操作
        public async Task<ActionResult> ServerIpEdit(int sid, int ipid)
        {
            var serverIP = await serverRepo.GetServerIpAsync(sid, ipid);
            //项目
            var ItemList = new List<SelectListItem>();
            var items = await itemsRepo.GetListItemsAsync();
            var items2 = new SelectList(items, "ItemID", "NAME");
            ItemList.AddRange(items2);
            ViewBag.ItemList = ItemList;
            //服务器
            //string itemID = serverIP == null ? items[0].ItemID : serverIP.itemid.ToString();
            //var ServerList = new List<SelectListItem>();
            //var servers = await serverRepo.GetServerListByItemIDAsync(itemID);
            //var servers2 = new SelectList(servers, "sid", "name");
            //ServerList.AddRange(servers2);
            //ViewBag.ServerList = ServerList;
            //IP
            //var IpList = new List<SelectListItem>();
            //var iplist = await serverRepo.GetIpAddressListByItemIDAsync(Convert.ToInt32(itemID));
            //var iplist2 = new SelectList(iplist, "ipid", "ipv4address");
            //IpList.AddRange(iplist2);
            //ViewBag.IpList = IpList;

            // ViewBag.isRealOnly = (serverIP != null).ToString().ToLower();
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
            var ItemList = new List<SelectListItem>();
            var items = await itemsRepo.GetListItemsAsync();
            var items2 = new SelectList(items, "ItemID", "NAME");
            ItemList.AddRange(items2);
            ViewBag.ItemList = ItemList;
            //服务器
            //var ServerList = new List<SelectListItem>();
            //var servers = await serverRepo.GetServerListAsync();
            //var servers2 = new SelectList(servers, "sid", "name");
            //ServerList.AddRange(servers2);
            //ViewBag.ServerList = ServerList;
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
            var result = await serverRepo.DelDatabaseDeploy(id);
            return Json(new { isOk = result });
        }
        #endregion

        #endregion
    }
}