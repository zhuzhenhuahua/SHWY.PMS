using System;
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
    [Table("Sys_User")]
   public class Sys_User
    {
        [Key]
        public int Uid { get; set; }
        [DisplayName("姓名")]
        public string Name { get; set; }
        [DisplayName("登录名")]
        public string LoginName { get; set; }
        [DisplayName("密码")]
        public string PassWord { get; set; }
        /// <summary>
        /// 所属角色ID
        /// </summary>
        [DisplayName("所属角色")]
        public int RoleId { get; set; }

        /// <summary>
        /// 所属角色名称
        /// </summary>
        [NotMapped]
        public string RoleName { get; set; }
    }
}
