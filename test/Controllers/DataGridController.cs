using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Zzh.Lib.DB.Repositorys;

namespace test.Controllers
{
    public class DataGridController : Controller
    {
        // GET: DataGrid
        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetList(int page, int rows)
        {
            using (EntAppAsnHeadRepository repo = new EntAppAsnHeadRepository())
            {
                var tuple = await repo.GetList(page, rows,"");
                return Json(new { total = tuple.Item1, rows = tuple.Item2 });
            }
        }
    }
}