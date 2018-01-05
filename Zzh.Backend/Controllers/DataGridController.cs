using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Zzh.Lib.DB.Repositorys;

namespace Zzh.Backend.Controllers
{
    public class DataGridController : Controller
    {
        // GET: DataGrid
        public ActionResult BasicGrid()
        {
            return View();
        }

        public async Task<JsonResult> GetList(int page, int rows,string asn_no)
        {
            using (EntAppAsnHeadRepository repo = new EntAppAsnHeadRepository())
            {
                var tuple = await repo.GetListAsync(page, rows,asn_no,"");
                return Json(new { total = tuple.Item1, rows = tuple.Item2 });
            }
        }
    }
}