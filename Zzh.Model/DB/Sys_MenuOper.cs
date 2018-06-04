﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zzh.Model.DB
{
    [Serializable]
    [Table("Sys_MenuOper")]
   public class Sys_MenuOper
    {
        [Key]
        public int MenuOperId { get; set; }
        public int MenuId { get; set; }
        public string MenuOperBtnID { get; set; }
        public string MenuOperBtnName { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
