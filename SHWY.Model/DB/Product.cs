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
    /// <summary>
    /// 产品实体类
    /// </summary>
    [Serializable]
    [Table("Prod")]
   public class Product
    {
        /// <summary>
        /// 产品ID主键（自增长）
        /// </summary>
        [Key]
        [DisplayName("产品编号")]
        public string ProID { get; set; }
        [DisplayName("产品名称")]
        public string NAME { get; set; }
        [DisplayName("产品别名")]
        public string ALIAS { get; set; }
    }
}
