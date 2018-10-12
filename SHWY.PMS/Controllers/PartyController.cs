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
    public class PartyController : BaseController
    {
        PartyRepository partyRepo = PartyRepository.CreateInstance();
        // GET: Party
        public ActionResult PartyIndex()
        {
            return View();
        }
        public async Task<JsonResult> GetPartyList(int page, int rows, string name)
        {
            var tuple = await partyRepo.GetPartyListAsync(page, rows, name);
            return Json(new { total = tuple.Item1, rows = tuple.Item2 });
        }
        public async Task<JsonResult> GetAllPartys(int isAddAll)
        {
            var result = await partyRepo.GetPartyListAsync();
            if (isAddAll == 1)
                result.Insert(0, new Party() {  PartyID = "",  name = "全部" });
            return Json(result);
        }
        public async Task<ActionResult> PartyEdit(string partyID)
        {
            var model = await partyRepo.GetPartyAsync(partyID);
            ViewData["isRealOnly"] = (model != null).ToString().ToLower();//不为空时前台控件只读
            return View(model);
        }
        public async Task<JsonResult> IsExistsByPartyID(Party party)
        {
            var i = await partyRepo.GetPartyAsync(party.PartyID);
            return Json(new { isExists = i != null });
        }
        public async Task<JsonResult> SaveParty(Party model)
        {
            var res = await partyRepo.AddOrUpdateAsync(model);
            return Json(new { isOk = res });
        }
        public async Task<JsonResult> DelParty(string partyID)
        {
            var res = await partyRepo.DeleteParty(partyID);
            return Json(new { isOk = res });
        }
    }
}