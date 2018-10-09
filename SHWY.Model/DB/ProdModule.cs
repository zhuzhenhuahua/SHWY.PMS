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
    /// 产品模块
    /// </summary>
    [Serializable]
    [Table("ProdModule")]
   public class ProdModule
    {
        /// <summary>
        /// 模块ID（产品ID+001）
        /// </summary>
        [Key]
        public int ModuleID { get; set; }
        /// <summary>
        /// 产品ID
        /// </summary>
        public string ProID { get; set; }
        /// <summary>
        /// 模块名称
        /// </summary>
        public string NAME { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}
