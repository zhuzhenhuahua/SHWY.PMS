using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SHWY.PMS.Models;

namespace SHWY.PMS.Controllers.Demo
{
    public class OperFileController : Controller
    {
        // GET: OperFile
        public ActionResult UpDownload()
        {
            string filePath = Server.MapPath(string.Format("~/{0}", "Files"));

            DirectoryInfo directoryInfo = new DirectoryInfo(filePath);
            if (!directoryInfo.Exists)
            {
                Directory.CreateDirectory(filePath);
            }
            List<JTFileModel> fileModels = new List<JTFileModel>();
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                fileModels.Add(new JTFileModel()
                {
                    Name = file.Name,
                    Extensioin = file.Extension,
                    FullName = file.FullName,
                    Length = file.Length,
                    IsReadOnly = file.IsReadOnly,
                    CreationTime = file.CreationTime
                });
            }
            return View(new FileModesObj() { List = fileModels });
        }
        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase filePara)
        {
            string filePath = Server.MapPath(string.Format("~/{0}", "Files"));
            if (filePara != null && filePara.ContentLength > 0)
            {
                var fileName = filePara.FileName;
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
                filePara.SaveAs(Path.Combine(filePath, fileName));
                ViewBag.msg = "上传成功";
            }

            List<JTFileModel> fileModels = new List<JTFileModel>();
            DirectoryInfo directoryInfo = new DirectoryInfo(filePath);
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                fileModels.Add(new JTFileModel()
                {
                    Name = file.Name,
                    Extensioin = file.Extension,
                    FullName = file.FullName,
                    Length = file.Length,
                    IsReadOnly = file.IsReadOnly,
                    CreationTime = file.CreationTime
                });
            }
            return View("UpDownload", new FileModesObj() { List = fileModels });
        }
        public class FileModesObj
        {
            public List<JTFileModel> List { get; set; }
            public int pageIndex { get; set; }
            public int pageSize { get; set; }
        }
        private List<JTFileModel> GetDirectoryFiles()
        {
            string filePath = Server.MapPath(string.Format("~/{0}", "Files"));
            List<JTFileModel> fileModels = new List<JTFileModel>();
            DirectoryInfo directoryInfo = new DirectoryInfo(filePath);
            foreach (FileInfo file in directoryInfo.GetFiles())
            {

            }
            return null;
        }
    }
}