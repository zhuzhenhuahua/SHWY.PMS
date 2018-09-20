using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using SHWY.Model.XML;
using System.IO;
using System.Xml;
using SHWY.Lib.Util;

namespace SHWY.PMS.Controllers
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






            string s = HttpUtility.HtmlEncode(xml2);
            string s2 = HttpUtility.HtmlDecode(s);


            byte[] bytes = Convert.FromBase64String("1BB91B374273E331D7885E9BDC757D65A630520CFB55067E2BE1E415F0662B63711BFB0F8CCFF5D82490934FEC2258B1A1AC7A2EDC7C2D948164937BB9A496FB2660F3B354437B740D03D71213615ABF371AD071C16EFD009F819E6E1869E5D32F4E5A6513FD6AE2444FB519824BFA24773FF29B826798140A4F597BDBA9D56D6A0533937A5D694631776DEDAC78F49EED68004A1FA1B8197111A374A07E90609660101EAFD189408245F9F1C099759EC7BD4D381839A2C60D2F933EE84EB133C761AB0B249AF5E2DB3266859ADEF5EE");
            string str = Encoding.Unicode.GetString(bytes);


            return View();
        }
    }
}