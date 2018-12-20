using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHWY.Model.DB; 

namespace SHWY.Model.Join
{
    [Serializable]
   public class ServerIPView
    {
        public int sid { get; set; }
        public IpAddress IPAddress { get; set; }
    }
}
