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
    [Table("PlanPoint")]
   public class PlanPoint
    {
        [Key,Column(Order =1)]
        public string PlanID { get; set; }
        [Key, Column(Order = 2)]
        public int num { get; set; }
        public string ItemID { get; set; }
        public string ProID { get; set; }
        public int ModuleID { get; set; }
        public int TaskType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Text { get; set; }
        public string NeedID { get; set; }
        public int handlerID { get; set; }
        public int PublisherId { get; set; }
        public DateTime addTime { get; set; }
        public DateTime UpdTime { get; set; }
    }
}
