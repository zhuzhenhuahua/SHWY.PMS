using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SHWY.Lib.DB.Repositorys;
using SHWY.Model.DB;
using SHWY.PMS.Controllers;

namespace SHWY.PMS.Controllers
{
    public class ProductController : BaseController
    {
        ProductRepository prodRepo = ProductRepository.CreateInstance();
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> GetList(int page, int rows, string prodName)
        {
            var result = await prodRepo.GetListAsync(page, rows, prodName);
            return Json(new { total = result.Item1, rows = result.Item2 });
        }
        public async Task<JsonResult> IsExistsByProId(Product prod)
        {
            Product p = await prodRepo.GetProductAsync(prod.ProID);
            return Json(new { isExists = !string.IsNullOrEmpty(p.ProID) });
        }
        public async Task<ActionResult> EditProd(string prodId)
        {
            Product prod = await prodRepo.GetProductAsync(prodId);
            ViewData["isRealOnly"] = (!string.IsNullOrEmpty(prod.ProID)).ToString().ToLower();//ProdId不为空时前台控件只读
            return View(prod);
        }
        public async Task<JsonResult> SaveProd(Product prod)
        {
            var result = await prodRepo.AddOrUpdateAsync(prod);
            return Json(new { isOk = result });
        }
        public async Task<JsonResult> DelProd(string prodId)
        {
            var result = await prodRepo.DeleteProd(prodId);
            return Json(new { isOk = result });
        }
    }
}