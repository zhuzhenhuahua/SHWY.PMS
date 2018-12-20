using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHWY.Model.DB; 

namespace SHWY.Model.Join
{
   public class ItemProdDB
    {
        public string prodID { get; set; }
        public DatabaseDeploy database { get; set; }
    }
}
