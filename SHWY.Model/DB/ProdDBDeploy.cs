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
    [Table("ProdDBDeploy")]
  public class ProdDBDeploy
    {
        [Key]
        public int id { get; set; }
        public int itemId { get; set; }
        public string prodId { get; set; }
        public int dbId { get; set; }
    }
}
