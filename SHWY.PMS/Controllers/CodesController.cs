using SHWY.Lib.DB.Repositorys;
using SHWY.Model.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SHWY.PMS.Controllers
{
    public class CodesController : BaseController
    {
        CodeRepository codeRepo = CodeRepository.CreateInstance();
        // GET: Codes
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> GetCodeListByTypeID(int typeId)
        {
            var list = await codeRepo.GetCodesListAsync(typeId);
            return Json(new { rows = list });
        }
        public async Task<ActionResult> CodeEdit(int codeID,int typeID)
        {
            var model = await codeRepo.GetCodeAsync(codeID);
            if (model == null)
            {
                model = new Codes() { Status = 1, TypeId=typeID };
            }
            var StatusList = new List<SelectListItem>();
            StatusList.Add(new SelectListItem() { Value = "1", Text = "正常显示", Selected = true });
            StatusList.Add(new SelectListItem() { Value = "0", Text = "隐藏" });
            ViewBag.StatusList = StatusList;
            return View(model);
        }
        public async Task<JsonResult> SaveCode(Codes codePara)
        {
            var result =await codeRepo.AddOrUpdateCode(codePara);
            return Json(new { isOk = result });
        }
        #region CodeType
        public async Task<JsonResult> GetCodeTypeList(int page, int rows, string typeName)
        {
            using (CodeTypeRepository repoCodeType = new CodeTypeRepository())
            {
                var tuple = await repoCodeType.GetCodeTypeListAsync(page, rows, typeName);
                return Json(new { total = tuple.Item1, rows = tuple.Item2 });
            }
        }
        public async Task<ActionResult> CodeTypeEdit(int typeId)
        {
            using (CodeTypeRepository repoCodeType = new CodeTypeRepository())
            {
                var model = await repoCodeType.GetCodeTypeAsync(typeId);
                if (model == null)
                    model = new CodeType();
                return View(model);
            }
        }
        public async Task<JsonResult> SaveCodeType(CodeType modelPara)
        {
            using (CodeTypeRepository repoCodeType = new CodeTypeRepository())
            {
                var result = await repoCodeType.AddOrUpdateAsync(modelPara);
                return Json(new { isOk = result });
            }
        }
        public async Task<JsonResult> DelCodeType(int typeID)
        {
            using (CodeTypeRepository repoCodeType = new CodeTypeRepository())
            {
                var result = await repoCodeType.DelCodeTypeAsync(typeID);
                return Json(new { isOk = result });
            }
        }
        #endregion
    }
}