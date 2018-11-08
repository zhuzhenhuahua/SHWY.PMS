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
    /// 预期完成时间(实际完成时间)-发布时间，期间每天都应该有一条记录
    /// </summary>
    [Serializable]
    [Table("PersonTaskDaily")]
   public class PersonTaskDaily
    {
        /// <summary>
        /// 主键自增长ID
        /// </summary>
        [Key]
        public int Pid { get; set; }
        /// <summary>
        /// 关联任务ID
        /// </summary>
        public string TaskId { get; set; }
        /// <summary>
        /// 时间线（预期完成时间-发布时间，期间每天都应该有一条记录）
        /// </summary>
        public DateTime DateLine { get; set; }
    }
}
