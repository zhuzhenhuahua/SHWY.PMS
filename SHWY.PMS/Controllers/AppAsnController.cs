using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Zzh.Lib.DB.Repositorys;
using Zzh.Model.DB;

namespace Zzh.Backend.Controllers
{
    public class AppAsnController : Controller
    {
        // GET: AppAsn
        public ActionResult IndexSplitNo()
        {
            return View();
        }
        public async Task<JsonResult> GetList(int page, int rows, string asnNo = "", string bill_No_Split = "")
        {
            using (EntAppAsnHeadRepository repo = new EntAppAsnHeadRepository())
            {
                var tuple = await repo.GetListAsync(page, rows, asnNo, bill_No_Split);
                return Json(new { total = tuple.Item1, rows = tuple.Item2 });
            }
        }
        public async Task<ActionResult> EditAsn(int id)
        {
            using (EntAppAsnHeadRepository repo = new EntAppAsnHeadRepository())
            {
                EntAppAsnHead head = await repo.GetEntAppAsnHeadAsync(id);
                if (head == null)
                    head = new EntAppAsnHead();
                return View(head);
            }
        }
        public async Task<JsonResult> Edit(EntAppAsnHead head)
        {
            using (EntAppAsnHeadRepository repo = new EntAppAsnHeadRepository())
            {
                var result = await repo.UpdateAsync(head);
                return Json(new { isOk = result });
            }
        }
    }
}