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
    /// 产品数据库部署
    /// </summary>
    [Serializable]
    [Table("ProdDBDeploy")]
  public class ProdDBDeploy
    {
        [Key]
        public int id { get; set; }
        public string partyID { get; set; }
        public string itemId { get; set; }
        public string prodId { get; set; }
        public int dbId { get; set; }
    }
}
