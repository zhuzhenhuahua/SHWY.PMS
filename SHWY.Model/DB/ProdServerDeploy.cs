using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHWY.Model.DB
{
    /// <summary>
    /// 此产品部署到哪个服务器
    /// </summary>
    [Serializable]
    [Table("ProdServerDeploy")]
   public class ProdServerDeploy
    {
        /// <summary>
        /// 自增长主键
        /// </summary>
        [Key]
        public int id { get; set; }
        /// <summary>
        /// 甲方公司ID
        /// </summary>
        public string partyID { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public string itemid { get; set; }
        /// <summary>
        /// 产品ID
        /// </summary>
        public string prodid { get; set; }
        /// <summary>
        /// 部署服务器ID
        /// </summary>
        public int serverid { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        public int port { get; set; }
        /// <summary>
        /// 通信协议类型(见字典表ECodesTypeId.ProtType)
        /// </summary>
        public int porttype { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
    }
}
