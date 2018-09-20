using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zzh.Model.DB
{
    /// <summary>
    /// 角色对应菜单
    /// </summary>
    [Serializable]
    [Table("Sys_RoleMenu")]
    public class Sys_RoleMenu
    {
        [Key, Column(Order = 1)]
        public int RoleId { get; set; }
        [Key, Column(Order = 2)]
        public int MenuId { get; set; }
    }
}
