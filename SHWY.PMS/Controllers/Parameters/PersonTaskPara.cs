using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SHWY.PMS.Controllers
{
    public class PersonTaskPara
    {
       public int page { get; set; }
       public int rows { get; set; }
       public int handlerId { get; set; }
       public string itemId { get; set; }
        public string prodId { get; set; }
       public int taskStatus { get; set; }
        public DateTime? publishForm { get; set; }
        public DateTime? publishTo { get; set; }
    }
}