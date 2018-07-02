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
    /// 角色对应操作按钮ID
    /// </summary>
    [Serializable]
    [Table("Sys_RoleOper")]
   public class Sys_RoleOper
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public int ROId { get; set; }
        public int MenuId { get; set; }
        public int MenuOperId { get; set; }
    }
}
