using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Zzh.Model.XML
{
    /// <summary>
    /// 客户推送出库单至仓库返回结果
    /// </summary>
    [XmlRoot("Response")]
    public class soToWmsResponse
    {
        [XmlElement("status")]
        public string status { get; set; }

        [XmlArray("failureItems"),XmlArrayItem("item")]
        public List<item> failureItems { get; set; }
    }
    public class item
    {
        [XmlElement("systemId")]
        public string systemId { get; set; }
        [XmlElement("errmsg")]
        public string errmsg { get; set; }
    }
}
