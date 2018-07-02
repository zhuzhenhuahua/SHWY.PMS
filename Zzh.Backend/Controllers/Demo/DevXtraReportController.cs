using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Zzh.Lib.DB.Repositorys;

namespace Zzh.Backend.Controllers.Demo
{
    public class DevXtraReportController : Controller
    {
        Sys_UserRepository repo = new Sys_UserRepository();
        // GET: DevXtraReport
        public async Task<ActionResult> Index()
        {
            string filePath = Server.MapPath(string.Format("~/{0}", "Files"));
            var result = await repo.GetListAsync(1, 10, "");
            return View();
        }
    }
}