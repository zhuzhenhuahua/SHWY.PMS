using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHWY.Model.DB
{
    [Serializable]
    [Table("Sys_Role")]
   public class Sys_Role
    {
        /// <summary>
        /// 主键角色ID
        /// </summary>
        [Key]
        public int Rid { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RName { get; set; }
    }
}
