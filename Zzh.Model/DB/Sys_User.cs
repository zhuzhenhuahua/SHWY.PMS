using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zzh.Model.DB
{
    [Serializable]
    [Table("Sys_User")]
   public class Sys_User
    {
        [Key]
        public int Uid { get; set; }
        public string Name { get; set; }
        public string LoginName { get; set; }
        public string PassWord { get; set; }
        /// <summary>
        /// 所属角色ID
        /// </summary>
        public int RoleId { get; set; }
    }
}
