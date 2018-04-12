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
    [Table("Product")]
   public class Product
    {
        [Key]
        public string Pid { get; set; }
        public string PName { get; set; }
        public string PSize { get; set; }
        public decimal CostPrice { get; set; }
        public decimal MarketPrice { get; set; }
    }
}
