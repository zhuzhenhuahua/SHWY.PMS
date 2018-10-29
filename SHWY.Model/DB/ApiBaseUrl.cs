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
    [Table("ApiBaseUrl")]
   public class ApiBaseUrl
    {
        [Key]
        public int Id { get; set; }
        public string BaseName { get; set; }
        public string BaseUrl { get; set; }
        public string Remark { get; set; }
    }
}
