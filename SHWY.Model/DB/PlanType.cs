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
    [Table("PlanType")]
    public class PlanType
    {
        [Key]
        public int ID { get; set; }
        public string NAME { get; set; }
    }
}
