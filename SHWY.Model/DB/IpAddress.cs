using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHWY.Model.DB
{
    [Serializable]
    [Table("IpAddress")]
    public class IpAddress
    {
        [Key]
        [DisplayName("IpID")]
        public int ipid { get; set; }
        [DisplayName("IP4地址")]
        public string ipv4address { get; set; }
        [DisplayName("甲方公司")]
        public string partyID { get; set; }
        [DisplayName("IP6地址")]
        public string ipv6address { get; set; }
        /// <summary>
        /// 内外网标识(0:内网1:外网)
        /// </summary>
        [DisplayName("内外网")]
        public int belong { get; set; }
    }
}
