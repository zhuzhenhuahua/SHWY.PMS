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
    /// PersonTaskDaily表的视图
    /// </summary>
    [Serializable]
    [Table("V_PersonTaskDaily")]
    public class V_PersonTaskDaily
    {
        /// <summary>
        /// PersonTaskDaily的主键
        /// </summary>
        [Key]
        public int Pid { get; set; }
        /// <summary>
        /// 任务ID
        /// </summary>
        public string TaskId { get; set; }
        /// <summary>
        /// 任务日志的时间线
        /// </summary>
        public DateTime DateLine { get; set; }
        public string itemID { get; set; }
        public string ItemName { get; set; }
        public string prodId { get; set; }
        public string ProdName { get; set; }
        public string detail { get; set; }
        public int handlerID { get; set; }
        public string handlerName { get; set; }
        public string taskStatusName { get; set; }
        /// <summary>
        /// 任务过程
        /// </summary>
        [NotMapped]
        public List<PersonTaskProcess> PersonTaskProcess { get; set; }
    }
}
