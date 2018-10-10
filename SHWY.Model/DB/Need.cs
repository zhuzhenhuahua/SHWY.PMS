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
    /// 需求
    /// </summary>
    [Serializable]
    [Table("Need")]
    public class Need
    {
        /// <summary>
        /// 主键（ND+YYYYMMDDHHMMSS+四位随机数）
        /// </summary>
        [Key]
        public string NeedID { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public string ItemID { get; set; }
        /// <summary>
        /// 产品ID
        /// </summary>
        public string ProID { get; set; }
        /// <summary>
        /// 模块ID
        /// </summary>
        public int ModuleID { get; set; }
        /// <summary>
        /// 任务类型
        /// </summary>
        public int TaskType { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime proposeTime { get; set; }
        /// <summary>
        /// deadTime
        /// </summary>
        public DateTime deadTime { get; set; }
        /// <summary>
        /// 需求内容
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 发布人ID
        /// </summary>
        public int PublisherID { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime addTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime updTime { get; set; }
    }
}
