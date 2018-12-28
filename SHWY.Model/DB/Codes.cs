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
    /// 统一字典表
    /// </summary>
    [Serializable]
    [Table("Codes")]
    public class Codes
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Text { get; set; }
        public int Status { get; set; }
        /// <summary>
        /// 字典类型
        /// </summary>
        public int TypeId { get; set; }

    }
}
