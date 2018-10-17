using SHWY.Lib.DB.Repositorys;
using SHWY.Model.DB;
using SHWY.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SHWY.PMS.Controllers
{
    public class ApiManaController : BaseController
    {
        ProductRepository prodRepo = ProductRepository.CreateInstance();
        CodeRepository codeRepo = CodeRepository.CreateInstance();
        #region ApiUrl
        public ActionResult ApiUrlIndex()
        {
            return View();
        }
        public async Task<JsonResult> GetParentApiUrlList(int isAddAll)
        {
            using (ApiManaRepository rep = new ApiManaRepository())
            {
                var result = await rep.GetApiUrlListByParentID(0);
                if (isAddAll == 1)
                    result.Insert(0, new ApiUrl() { UrlID = 0, Name = "全部" });
                return Json(result);
            }
        }
        public async Task<JsonResult> GetApiUrlTreeList(int parentId)
        {
            using (ApiManaRepository rep = new ApiManaRepository())
            {
                var treelist = await rep.GetApiUrlTreeList(parentId);
                return Json(treelist);
            }
        }
        public async Task<ActionResult> EditApiUrl(int urlID)
        {
            using (ApiManaRepository rep = new ApiManaRepository())
            {
                var model = await rep.GetApiUrlAsync(urlID);
                if (model == null)
                    model = new ApiUrl() { ProdID = "0007" };
                //产品
                var ProdList = new List<SelectListItem>();
                var prods = await prodRepo.GetListAsync();
                var prods2 = new SelectList(prods, "ProID", "NAME");
                ProdList.AddRange(prods2);
                ViewBag.ProdList = ProdList;
                //上级接口名称ParentUrlList
                var ParentUrlList = new List<SelectListItem>() { new SelectListItem() { Value = "0", Text = "无", Selected = true } };
                var urllist = await rep.GetApiUrlListByParentID(0);
                var urllist2 = new SelectList(urllist, "UrlID", "Name");
                ParentUrlList.AddRange(urllist2);
                ViewBag.ParentUrlList = ParentUrlList;
                //请求类型MethodList
                var MethodList = new List<SelectListItem>();
                var metlist = await codeRepo.GetCodesListAsync(ECodesTypeId.MethodType);
                var metlist2 = new SelectList(metlist, "Code", "Text");
                MethodList.AddRange(metlist2);
                ViewBag.MethodList = MethodList;
                return View(model);

            }
        }
        public async Task<JsonResult> SaveApiUrl(ApiUrl model)
        {
            using (ApiManaRepository rep = new ApiManaRepository())
            {
                var res = await rep.AddOrUpdateApiUrlAsync(model);
                return Json(new { isOk = res });
            }
        }
        public async Task<JsonResult> DelApiUrl(int urlID)
        {
            using (ApiManaRepository rep = new ApiManaRepository())
            {
                var res = await rep.DelApiUrlAsync(urlID);
                return Json(new { isOk = res });
            }
        }
        public async Task<ActionResult> APISendTest(int urlID)
        {
            using (ApiManaRepository rep = new ApiManaRepository())
            {
                var model = await rep.GetApiUrlAsync(urlID);
                model.apiParas = await rep.GetApiParaListAsync(urlID);
                return View(model);
            }
        }
        public async Task<JsonResult> SendAPi(int urlID, string url, string urlPara)
        {
            using (ApiManaRepository rep = new ApiManaRepository())
            {
                var apiUrl = await rep.GetApiUrlAsync(urlID);
                Dictionary<string, string> dicPara = new Dictionary<string, string>();
                string[] paraArray = urlPara.Split('&');
                foreach (string str in paraArray)
                {
                    string[] p = str.Split('=');
                    dicPara.Add(p[0], p[1]);
                }
                string result = string.Empty;
                if (apiUrl.method == 1)
                    result = RequestUtility.HttpGet(url + apiUrl.Url, dicPara);
                else
                    result = RequestUtility.HttpPost(url + apiUrl.Url, dicPara);
                return Json(result);
            }
        }
        #endregion

        #region ApiParameter
        public ActionResult ApiParameterIndex()
        {
            return View();
        }
        #endregion
    }
}