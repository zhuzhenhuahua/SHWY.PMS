using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SHWY.Lib.DB.Repositorys;
using SHWY.Lib.Util;
using SHWY.Model.DB;
using System.Text;

namespace SHWY.PMS.Controllers
{
    public class ItemsController : BaseController
    {
        ItemsRepository repoItems = ItemsRepository.CreateInstance();
        ProductRepository repoprod = ProductRepository.CreateInstance();
        PartyRepository partyRepo = PartyRepository.CreateInstance();
        ServerRepository repoServer = ServerRepository.CreateInstance();
        CodeRepository repoCode = CodeRepository.CreateInstance();
        // GET: Items
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> GetList(int page, int rows, string itemName, string partyID)
        {
            var result = await repoItems.GetListAsync(page, rows, itemName, partyID);
            return Json(new { total = result.Item1, rows = result.Item2 });
        }
        public async Task<JsonResult> GetTreeList(string itemName, string partyID)
        {
            var result = await repoItems.GetItemTreeList(itemName, partyID);
            return Json(result);
        }
        public async Task<JsonResult> GetItemListByPartyID(string partyID)
        {
            var list = await repoItems.GetItemListByPartyIDAsync(partyID);
            return Json(list);
        }
        public async Task<JsonResult> GetAllItems(int isAddAll)
        {
            var result = await repoItems.GetListItemsAsync();
            if (isAddAll == 1)
                result.Insert(0, new Items() { ItemID = "", NAME = "全部" });
            return Json(result);
        }
        public async Task<JsonResult> IsExistsByItemId(Items item)
        {
            Items i = await repoItems.GetItemAsync(item.ItemID);
            return Json(new { isExists = i != null });
        }
        public async Task<string> GetTDeployInfo(string itemID)
        {
            Items i = await repoItems.GetItemAsync(itemID);
            var listProd = await repoprod.GetProdServerDeployListByItemIDAsync(itemID);//产品-服务器对应
            var listDB = await repoprod.GetProdDBListAsync(itemID);//产品-数据库对应
            List<int> serverIDs = listProd.Select(p => p.server.sid).ToList();//所有服务器ID的集合，用于批量取IP
            var listIP = await repoServer.GetServerIPListBySidAsync(serverIDs);//IP

            StringBuilder sbText = new StringBuilder();
            sbText.AppendLine("【项目】：" + i.NAME + "<br/><br/>");
            int proNum = 1;
            string nbsp3 = StringJoinHelper.GetNBSPLen(3);
            string nbsp4 = StringJoinHelper.GetNBSPLen(4);
            string nbsp8 = StringJoinHelper.GetNBSPLen(8);
            string nbsp12 = StringJoinHelper.GetNBSPLen(12);
            foreach (var item in listProd)
            {
                //产品说明
                sbText.AppendLine(nbsp4 + "【产品" + proNum + "】：" + item.prod?.NAME + nbsp3 + "【别名】：" + item.prod?.ALIAS + "<br/>");
                //服务器说明
                var ips = listIP.Where(p => p.sid == item.server.sid);
                string ipStr = string.Empty;
                foreach (var ip in ips)
                {
                    if (ip.IPAddress.belong == 0)//内网
                        ipStr += $"【内网IP】:{ip.IPAddress?.ipv4address}";
                   else if (ip.IPAddress.belong == 1)//外网
                        ipStr += $"【外网IP】:{ip.IPAddress?.ipv4address}";
                }
                sbText.AppendLine($"{nbsp8}【部署服务器】：{item.server?.name}，【登录名】：{item.server?.loginName}，【密码】：{item.server?.password}，{ipStr}<br/>");

                var prodDBList = listDB.Where(p => p.prodID == item.prod?.ProID).ToList();
                foreach (var db in prodDBList)
                {
                    if (db.database != null)
                    {
                        string dbSchemaid = await repoCode.GetCodesTextAsync(ECodesTypeId.databaseSchema, db.database.schemaid.ToString());
                        string dbTypeName = await repoCode.GetCodesTextAsync(ECodesTypeId.databaseType, db.database.type.ToString());
                        if (db.database.type == 1)//sqlserver
                            sbText.AppendLine($"{nbsp8}【部署数据库名】：{db.database.name}，【数据库架构】：{dbSchemaid}，【数据库类型】：{dbTypeName}，【sqlServerCatlog】：{db.database.sqlServerCatlog}，【username】：{db.database.username}，【password】{db.database.password}<br/><br/>");
                        else if (db.database.type == 2)//oracle
                            sbText.AppendLine($"{nbsp8}【部署数据库名】：{db.database.name}，【数据库架构】：{dbSchemaid}，【数据库类型】：{dbTypeName}，【orclServiceName】：{db.database.orclServiceName}，【username】：{db.database.username}，【password】{db.database.password}<br/><br/>");
                        else if (db.database.type == 3)//mongoDB
                            sbText.AppendLine($"{nbsp8}【部署数据库名】：{db.database.name}，【数据库架构】：{dbSchemaid}，【数据库类型】：{dbTypeName}，【mongoAdminDBName】：{db.database.mongoAdminDBName}，【mongoDBName】：{db.database.mongoDBName}，【password】{db.database.password}<br/><br/>");
                    }
                }

                proNum++;
            }

            return sbText.ToString();
        }
        #region 增删改
        public async Task<ActionResult> EditItem(string strItemId)
        {
            Items item = await repoItems.GetItemAsync(strItemId);
            //甲方公司
            var PartyList = new List<SelectListItem>();
            var party = await partyRepo.GetPartyListAsync();
            var party2 = new SelectList(party, "PartyID", "name");
            PartyList.AddRange(party2);
            ViewBag.PartyList = PartyList;

            ViewData["isRealOnly"] = (item != null).ToString().ToLower();//ItemID不为空时前台控件只读
            if (item == null)
                item = new Items();
            return View(item);
        }
        public async Task<JsonResult> SaveItem(Items item)
        {
            var result = await repoItems.AddOrUpdateAsync(item);
            return Json(new { isOk = result });
        }
        public async Task<JsonResult> DelItem(string itemId)
        {
            var result = await repoItems.DeleteItem(itemId);
            return Json(new { isOk = result });
        }
        #endregion
    }
}