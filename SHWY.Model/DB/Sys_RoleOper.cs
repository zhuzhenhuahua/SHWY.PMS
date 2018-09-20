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
    /// 角色对应操作按钮ID
    /// </summary>
    [Serializable]
    [Table("Sys_RoleOper")]
   public class Sys_RoleOper
    {
        [Key, Column(Order = 1)]
        public int RoleId { get; set; }
        [Key, Column(Order = 2)]
        public int MenuId { get; set; }
        [Key, Column(Order = 3)]
        public int MenuOperId { get; set; }
    }
}
