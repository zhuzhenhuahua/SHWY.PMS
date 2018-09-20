using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zzh.Model.DB
{
    [Serializable]
    [Table("Sys_Configuration")]
    public class Sys_Configuration
    {
        [Key]
        public int Id { get; set; }
        public string Domain { get; set; }

        public string Module { get; set; }
        public string Functions { get; set; }
        public string Keys { get; set; }
        public string Value { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}
