using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SHWY.Lib.DB.Repositorys;
using SHWY.PMS.Controllers.Filter;
using SHWY.Model.DB.Configuration;
using System.Configuration;
using SHWY.Lib.Util;

namespace SHWY.PMS.Controllers
{
    public class DataGridController : BaseController
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
        public ActionResult MergeCells()
        {
            return View();
        }
        //public async Task<JsonResult> GetProductList(int rows, int page)
        //{
        //    using (ProductRepository repo = new ProductRepository())
        //    {
        //        var result = await repo.GetListAsync(rows, page);
        //        var list = result.Item2;
        //        var group =(from j in list
        //                     group j by j.PSize into p
        //                     where p.Count()>1
        //                     select new
        //                     {
        //                         index = list.IndexOf(list.Where(x => x.Pid.Equals(p.Min(a => a.Pid))).FirstOrDefault()),
        //                         rowSpan = p.Count()
        //                     }).ToList();

        //        return Json(new { total = result.Item1, rows = list, mergeGroup = group });
        //    }
        //}
    }
}