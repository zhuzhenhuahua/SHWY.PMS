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
    /// 主题日志
    /// </summary>
    [Serializable]
    [Table("TopicLog")]
    public class TopicLog
    {
        /// <summary>
        /// 主键自增长ID
        /// </summary>
        [Key]
        public int Tid { get; set; }
        /// <summary>
        /// 记录日志的主题，1:项目日志，2:产品日志,对应枚举ETopicCode
        /// </summary>
        public int Topic { get; set; }
        /// <summary>
        /// 与主题关联的表主键
        /// </summary>
        public string TopicID { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime addDate { get; set; }
        /// <summary>
        /// 模块ID
        /// </summary>
        public int ModuleID { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Contents { get; set; }
    }
    public enum ETopic
    {
        项目日志=1,
        产品日志=2
    }
}
