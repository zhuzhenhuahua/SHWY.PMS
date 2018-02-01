using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using Zzh.Model.XML;
using System.IO;
using System.Xml;
using Zzh.Lib.Util;

namespace Zzh.Backend.Controllers
{
    public class XMLSerializaController : Controller
    {
        // GET: XMLSerializa
        public ActionResult Index()
        {
            string xml = @"
                        <Response>
	                        <status>All-ok</status>
	                        <failureItems>
		                        <item>
			                        <systemId>管理号1</systemId>
			                        <errmsg>错误原因1</errmsg>
		                        </item>
		                        <item>
			                        <systemId>管理号2</systemId>
			                        <errmsg>错误原因2</errmsg>
		                        </item>
	                        </failureItems>
                         </Response>";
            //反序列化
            soToWmsResponse model = (soToWmsResponse)XMLHelper.Deserialize<soToWmsResponse>(xml);
            //序列化
            string xml2 = XMLHelper.Serializer<soToWmsResponse>(model);
            string xml3 = XMLHelper.XmlSerializer<soToWmsResponse>(model);
            return View();
        }
    }
}