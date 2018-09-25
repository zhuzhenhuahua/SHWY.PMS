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
    [Table("PersonTaskProcess")]
   public class PersonTaskProcess
    {
        [Key]
        public int Id { get; set; }
        public string TaskId { get; set; }
        public DateTime WorkStartTime { get; set; }
        public DateTime WorkEndTime { get; set; }
        public string Details { get; set; }
        public DateTime addTime { get; set; }

    }
}
