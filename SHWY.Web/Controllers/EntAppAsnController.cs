using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zzh.Lib.DB.Repositorys;

namespace Zzh.Web.Controllers
{
    public class EntAppAsnController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetList()
        {
            using (EntAppAsnHeadRepository resp = new EntAppAsnHeadRepository())
            {
                var obj = resp.GetList();
                return Json(obj);
            }
        }
    }
}