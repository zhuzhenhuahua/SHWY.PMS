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
    [Table("ServerIp")]
    public class ServerIp
    {
        [Key, Column(Order = 1)]
        [DisplayName("服务器")]
        public int sid { get; set; }
        [Key, Column(Order = 2)]
        [DisplayName("IP地址")]
        public int ipid { get; set; }
        [DisplayName("甲方公司")]
        public string partyID { get; set; }
    }
}
