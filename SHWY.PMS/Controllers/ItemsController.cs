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
    public class ItemsController : BaseController
    {
        ItemsRepository repoItems = ItemsRepository.CreateInstance();
        PartyRepository partyRepo = PartyRepository.CreateInstance();
        // GET: Items
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> GetList(int page, int rows, string itemName,string partyID)
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