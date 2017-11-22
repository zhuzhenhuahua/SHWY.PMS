using System;
using System.Collections.Generic;
using System.Linq;
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

        public JsonResult GetList()
        {
            using (EntAppAsnHeadRepository repo = new EntAppAsnHeadRepository())
            {
                var obj = repo.GetList();
                return Json(obj);
            }
        }
    }
}