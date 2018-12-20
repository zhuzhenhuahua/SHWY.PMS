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
    [Table("Server")]
   public class Servers
    {
        /// <summary>
        /// 服务器主键ID（自增长）
        /// </summary>
        [Key]
        [DisplayName("服务器ID")]
        public int sid { get; set; }
        [DisplayName("服务器名称")]
        public string name { get; set; }
        [DisplayName("甲方公司")]
        public string partyId { get; set; }
        [DisplayName("备注")]
        public string remark { get; set; }
        [DisplayName("登录名")]
        public string loginName { get; set; }
        [DisplayName("密码")]
        public string password { get; set; }

        [NotMapped]
        public string itemName { get; set; }
        [NotMapped]
        public List<ServerIp> ServerIP { get; set; }
    }
}
