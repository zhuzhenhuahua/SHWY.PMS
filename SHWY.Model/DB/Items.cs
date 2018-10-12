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
    /// 甲方项目名称
    /// </summary>
    [Serializable]
    [Table("Item")]
    public class Items
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [Key]
        [DisplayName("项目ID")]
        public string ItemID { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        [DisplayName("项目名称")]
        public string NAME { get; set; }
        /// <summary>
        /// 项目别名
        /// </summary>
        [DisplayName("项目别名")]
        public string ALIAS { get; set; }

        /// <summary>
        /// 所属甲方公司
        /// </summary>
        [DisplayName("所属甲方公司")]
        public string partyID { get; set; }

        [NotMapped]
        public List<Items> children { get; set; }
    }
}
