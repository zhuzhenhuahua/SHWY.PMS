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
    [Table("App_SJ_Head")]
    public class EntApp_SJ_Head
    {
        [Key]
        public int Id { get; set; }
        public string Bill_No_Split { get; set; }
    }
}
