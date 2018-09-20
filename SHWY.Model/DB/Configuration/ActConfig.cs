using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHWY.Model.DB.Configuration.Att;

namespace SHWY.Model.DB.Configuration
{
    [MDConfig("ElasticSearch", "Act_usertreasure")]
  public  class ActConfig
    {
        [MDKey("IndexName")]
        public string IndexName { get; set; }
        [MDKey("SharId")]
        public string SharId { get; set; }
        [MDKey("UserId")]
        public string UserId { get; set; }
        [MDKey("PassWord")]
        public string PassWord { get; set; }
        [MDKey("Ports")]
        public string Ports { get; set; }
    }
}
