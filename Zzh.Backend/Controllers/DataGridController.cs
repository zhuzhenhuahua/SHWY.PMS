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
        public ActionResult Editor()
        {
            return View();
        }
    }
}