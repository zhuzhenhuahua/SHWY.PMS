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
    /// 端口映射
    /// </summary>
    [Serializable]
    [Table("InPortOutPort")]
   public class InPortOutPort
    {
        /// <summary>
        /// 自增长主键ID
        /// </summary>
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// 甲方公司ID
        /// </summary>
        public string partyID { get; set; }
        /// <summary>
        /// 内网IP(ID)
        /// </summary>
        public int inIpId { get; set; }
        /// <summary>
        /// 内网端口
        /// </summary>
        public int inPort { get; set; }
        /// <summary>
        /// 外网IP(ID)
        /// </summary>
        public int outIpId { get; set; }
        /// <summary>
        /// 外网端口
        /// </summary>
        public int outPort { get; set; }
        /// <summary>
        /// 端口类型(对应ECodesTypeId.ProtType)
        /// </summary>
        public int porttype { get; set; }
    }
}
