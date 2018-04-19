using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Zzh.Lib.DB.Repositorys;
using Zzh.Backend.Controllers.Filter;
using Zzh.Model.DB.Configuration;
using System.Configuration;
using Zzh.Lib.Util;

namespace Zzh.Backend.Controllers
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
        public async Task<JsonResult> GetProductList(int rows, int page)
        {
            ActConfig actConfi = MDConfigurationManager.GetConfig<ActConfig>();
           var sdf=await QQEmailHelper.SendMailAsync("标题标题标题标题标题标题标题标题","865729986@qq.com", "内容内容内容内容内容内容内容内容内容内容内容内容内容内容内容");

            using (ProductRepository repo = new ProductRepository())
            {
                var result = await repo.GetListAsync(rows, page);
                var list = result.Item2;
                var group =(from j in list
                             group j by j.PSize into p
                             where p.Count()>1
                             select new
                             {
                                 index = list.IndexOf(list.Where(x => x.Pid.Equals(p.Min(a => a.Pid))).FirstOrDefault()),
                                 rowSpan = p.Count()
                             }).ToList();

                return Json(new { total = result.Item1, rows = list, mergeGroup = group });
            }
        }
    }
}