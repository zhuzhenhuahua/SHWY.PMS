using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Zzh.Backend.Controllers.Demo
{
    public class OperFileController : Controller
    {
        // GET: OperFile
        public ActionResult UpDownload()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            var fileName = file.FileName;
            var filePath = Server.MapPath(string.Format("~/{0}", "Files"));
            file.SaveAs(Path.Combine(filePath, fileName));
            ViewBag.msg = "上传成功";
            return View("UpDownload");
        }
    }
}