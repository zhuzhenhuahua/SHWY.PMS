using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHWY.Model.DB
{
    [Serializable]
    [Table("Plan")]
   public class Plan
    {
        /// <summary>
        /// 主键（PL+年月日时分秒+4位随机数）
        /// </summary>
        [Key]
        public string PlanID { get; set; }
        public string title { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public string text { get; set; }
        public int planType { get; set; }
        public DateTime addTime { get; set; }
    }
}
