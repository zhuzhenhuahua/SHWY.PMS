using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHWY.Model.DB
{
    [Serializable]
    [Table("Party")]
   public class Party
    {
        public string PartyID { get; set; }
        public string name { get; set; }
        public string text { get; set; }
    }
}
