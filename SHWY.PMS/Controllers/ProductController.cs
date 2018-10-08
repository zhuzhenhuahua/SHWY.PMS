﻿using System;
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
        ItemsRepository itemsRepo = ItemsRepository.CreateInstance();
        ServerRepository serverRepo = ServerRepository.CreateInstance();
        CodeRepository codeRepo = CodeRepository.CreateInstance();

        #region 产品部署数据库
        public ActionResult ProdDBDeployIndex()
        {
            return View();
        }
        public async Task<JsonResult> GetProdDBDeployList(int page, int rows, string itemid, string prodid, int dbid)
        {
            var tuple = await prodRepo.GetProdDBListAsync(page, rows, itemid, prodid, dbid);
            return Json(new { total = tuple.Item1, rows = tuple.Item2 });
        }
        public async Task<ActionResult> ProdDBEdit(int id)
        {
            var prodDBDeply = await prodRepo.GetProdDBDeployAsync(id);

            //项目
            var ItemList = new List<SelectListItem>();
            var items = await itemsRepo.GetListItemsAsync();
            var items2 = new SelectList(items, "ItemID", "NAME");
            ItemList.AddRange(items2);
            ViewBag.ItemList = ItemList;
            //产品
            var ProdList = new List<SelectListItem>();
            var prods = await prodRepo.GetListAsync();
            var prods2 = new SelectList(prods, "ProID", "NAME");
            ProdList.AddRange(prods2);
            ViewBag.ProdList = ProdList;
            //数据库
            //var DBList = new List<SelectListItem>();
            //var dblist = await serverRepo.GetDatabaseDeployListAsync();
            //var dblist2 = new SelectList(dblist, "id", "name");
            //DBList.AddRange(dblist2);
            //ViewBag.DBList = DBList;
            if (prodDBDeply == null)
                prodDBDeply = new ProdDBDeploy();
            return View(prodDBDeply);
        }

        public async Task<JsonResult> SaveProdDBDeploy(ProdDBDeploy model)
        {
            var result = await prodRepo.AddOrUpdateProdDBDeployAsync(model);
            return Json(new { isOk = result });
        }
        public async Task<JsonResult> DelProdDBDdeploy(int id)
        {
            var result = await prodRepo.DelProdDBDeployAsync(id);
            return Json(new { isOk = result });
        }
        #endregion

        #region ProdServerDeploy产品部署服务器
        public ActionResult ProdServerIndex()
        {
            return View();
        }
        public async Task<JsonResult> GetProdServerList(int page, int rows, string prodID, int serverID, string itemID)
        {
            var tuple = await prodRepo.GetProdServerDeployListAsync(page, rows, prodID, serverID, itemID);
            return Json(new { total = tuple.Item1, rows = tuple.Item2 });
        }
        public async Task<ActionResult> ProdServerEdit(int id)
        {
            var prodDeply = await prodRepo.GetProdServerDeployAsync(id);

            //项目
            var ItemList = new List<SelectListItem>();
            var items = await itemsRepo.GetListItemsAsync();
            var items2 = new SelectList(items, "ItemID", "NAME");
            ItemList.AddRange(items2);
            ViewBag.ItemList = ItemList;
            //产品
            var ProdList = new List<SelectListItem>();
            var prods = await prodRepo.GetListAsync();
            var prods2 = new SelectList(prods, "ProID", "NAME");
            ProdList.AddRange(prods2);
            ViewBag.ProdList = ProdList;
            //服务器
            //var ServerList = new List<SelectListItem>();
            //var serlist = await serverRepo.GetServerListAsync();
            //var serlist2 = new SelectList(serlist, "sid", "name");
            //ServerList.AddRange(serlist2);
            //ViewBag.ServerList = ServerList;
            //通讯协议
            var ProtTypeList = new List<SelectListItem>();
            var prots = await codeRepo.GetCodesListAsync(ECodesTypeId.ProtType);
            var prots2 = new SelectList(prots, "Code", "Text");
            ProtTypeList.AddRange(prots2);
            ViewBag.ProtTypeList = ProtTypeList;

            if (prodDeply == null)
                prodDeply = new ProdServerDeploy();
            return View(prodDeply);
        }
        public async Task<JsonResult> SaveProdDeploy(ProdServerDeploy model)
        {
            var result = await prodRepo.AddOrUpdateProdServerDeployAsync(model);
            return Json(new { isOk = result });
        }
        public async Task<JsonResult> DelProdDdeploy(int id)
        {
            var result = await prodRepo.DelProdServerDeployAsync(id);
            return Json(new { isOk = result });
        }
        #endregion

        #region Product
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> GetList(int page, int rows, string prodName)
        {
            var result = await prodRepo.GetListAsync(page, rows, prodName);
            return Json(new { total = result.Item1, rows = result.Item2 });
        }
        public async Task<JsonResult> GetAllProds(int isAddAll)
        {
            var result = await prodRepo.GetListAsync();
            if (isAddAll == 1)
                result.Insert(0, new Product() { ProID = "", NAME = "全部" });
            return Json(result);
        }
        public async Task<JsonResult> IsExistsByProId(Product prod)
        {
            Product p = await prodRepo.GetProductAsync(prod.ProID);
            return Json(new { isExists = !string.IsNullOrEmpty(p.ProID) });
        }
        #endregion
        #region Product增删改
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
        #endregion
    }
}