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
        #region Server查询
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> GetList(int page, int rows, string name)
        {
            var tuple = await serverRepo.GetServerListAsync(page, rows, name);
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
        #endregion

        #region IpAddress查询
        public ActionResult IpAddressIndex()
        {
            return View();
        }
        public async Task<JsonResult> GetIpAddressList(int page, int rows, string ipAddress)
        {
            var tuple = await serverRepo.GetIpAddressListAsync(page, rows, ipAddress);
            return Json(new { total = tuple.Item1, rows = tuple.Item2 });
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
        public async Task<JsonResult> GetServerIpList(int page, int rows, string serverName)
        {
            var tuple = await serverRepo.GetServerIpListAsync(page, rows, serverName);
            return Json(new { total = tuple.Item1, rows = tuple.Item2 });
        }
        public async Task<JsonResult> IsExistsServerIp(ServerIp sip)
        {
            var model = await serverRepo.GetServerIpAsync(sip.sid, sip.ipid);
            return Json(new { IsExists = model != null });
        }
        #endregion

        #region databaseDeploy数据库管理
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

            var ServerList = new List<SelectListItem>();
            var servers = await serverRepo.GetServerListAsync();
            var servers2 = new SelectList(servers, "sid", "name");
            ServerList.AddRange(servers2);
            ViewBag.ServerList = ServerList;

            var IpList = new List<SelectListItem>();
            var iplist = await serverRepo.GetIpAddressListAsync();
            var iplist2 = new SelectList(iplist, "ipid", "ipv4address");
            IpList.AddRange(iplist2);
            ViewBag.IpList = IpList;

            var ItemList = new List<SelectListItem>();
            var items = await itemsRepo.GetListItemsAsync();
            var items2 = new SelectList(items, "ItemID", "NAME");
            ItemList.AddRange(items2);
            ViewBag.ItemList = ItemList;

            ViewBag.isRealOnly = (serverIP != null).ToString().ToLower();
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
        #endregion

        #endregion
    }
}