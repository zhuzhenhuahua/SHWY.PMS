﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHWY.Model.DB
{
    [Serializable]
    [Table("IpAddress")]
   public class IpAddress
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DisplayName("IpID")]
        public int ipid { get; set; }
        [DisplayName("IP4地址")]
        public string ipv4address { get; set; }
        [DisplayName("项目")]
        public int itemid { get; set; }
        [DisplayName("IP6地址")]
        public string ipv6address { get; set; }
        [DisplayName("内外网")]
        public int belong { get; set; }

        [NotMapped]
        public string itemName { get; set; }
    }
}
