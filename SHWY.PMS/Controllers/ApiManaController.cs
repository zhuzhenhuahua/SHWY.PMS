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
        #region ApiBaseUrl
        public ActionResult BaseUrlIndex()
        {
            return View();
        }
        public async Task<JsonResult> GetBaseUrlList(int page, int rows)
        {
            using (ApiManaRepository rep = new ApiManaRepository())
            {
                var tuple = await rep.GetApiBaseUrlListAsync(page, rows);
                return Json(new { total = tuple.Item1, rows = tuple.Item2 });
            }
        }
        public async Task<JsonResult> GetAllBaseUrlList()
        {
            using (ApiManaRepository rep = new ApiManaRepository())
            {
                var list = await rep.GetApiBaseUrlListAsync();
                return Json(list);
            }

        }
        public async Task<ActionResult> EditBaseUrl(int Id)
        {
            using (ApiManaRepository rep = new ApiManaRepository())
            {
                var model = await rep.GetApiBaseUrlAsync(Id);
                return View(model);
            }
        }
        public async Task<JsonResult> SaveApiBaseUrl(ApiBaseUrl model)
        {
            using (ApiManaRepository rep = new ApiManaRepository())
            {
                var res = await rep.AddOrUpdateApiBaseUrlAsync(model);
                return Json(new { isOk = res });
            }
        }
        public async Task<JsonResult> DelBaseUrl(int BaseUrlId)
        {
            using (ApiManaRepository rep = new ApiManaRepository())
            {
                var res = await rep.DelApiBaseUrlAsync(BaseUrlId);
                return Json(new { isOk = res });
            }
        }
        #endregion

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
                model.apiParas = await rep.GetApiParaListAsync(urlID, 1);//这里只查输入参数
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
                    if (!string.IsNullOrEmpty(str))
                    {
                        string[] p = str.Split('=');
                        dicPara.Add(p[0], p[1]);
                    }
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
        public async Task<JsonResult> GetApiParaListAsync(int urlID, int inOrOutPut, int page, int rows)
        {
            if (urlID == 0 && inOrOutPut == 0)
                return null;
            using (ApiManaRepository rep = new ApiManaRepository())
            {
                var tuple = await rep.GetApiParaListAsync(page, rows, urlID, inOrOutPut);
                return Json(new { total = tuple.Item1, rows = tuple.Item2 });
            }
        }
        public async Task<ActionResult> ApiParaEdit(int paraID, int urlID)
        {
            using (ApiManaRepository rep = new ApiManaRepository())
            {
                var model = await rep.GetApiParaByParaIDAsync(urlID, paraID);
                if (model == null)
                    model = new ApiParameter() { ApiUrlID = urlID, DataType = 1, IsNull = true, InOROutPut = 1 };
                //数据类型
                var DataTypeList = new List<SelectListItem>();
                var datatype = await codeRepo.GetCodesListAsync(ECodesTypeId.DataTypeByApiPara);
                var datatype2 = new SelectList(datatype, "Code", "Text");
                DataTypeList.AddRange(datatype2);
                ViewBag.DataTypeList = DataTypeList;
                //输入输出类型
                var InOrOutPutList = new List<SelectListItem>();
                InOrOutPutList.Add(new SelectListItem() { Value = "1", Text = "输入参数", Selected = true });
                InOrOutPutList.Add(new SelectListItem() { Value = "2", Text = "输出参数" });
                ViewBag.InOrOutPutList = InOrOutPutList;
                //允许NULL值
                var ISNULLList = new List<SelectListItem>();
                ISNULLList.Add(new SelectListItem() { Value = "True", Text = "可空", Selected = true });
                ISNULLList.Add(new SelectListItem() { Value = "False", Text = "不允许为空"});
                ViewBag.ISNULLList = ISNULLList;
                return View(model);
            }
        }
        public async Task<JsonResult> SaveUrlPara(ApiParameter para)
        {
            using (ApiManaRepository rep = new ApiManaRepository())
            {
                var res = await rep.AddOrUpdateApiParaAsync(para);
                return Json(new { isOk = res });
            }
        }
        public async Task<JsonResult> DelUrlPara(int paraID, int urlID)
        {
            using (ApiManaRepository rep = new ApiManaRepository())
            {
                var res = await rep.DelApiParaAsync(urlID, paraID);
                return Json(new { isOk = res });
            }
        }
        #endregion
    }
}