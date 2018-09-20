using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zzh.Model.DB;

namespace Zzh.Backend
{
    public class CurrentUser
    {
        /// <summary>
        /// 当前用户实体
        /// </summary>
        public Sys_User Sys_User { get; set; }
        /// <summary>
        /// 当前用户所属角色下的菜单集合
        /// </summary>
        public List<Sys_RoleMenu> Sys_RoleMenu { get; set; }
        /// <summary>
        /// 当前用户所属角色下的菜单操作按钮集合
        /// </summary>
        public List<Sys_RoleOper> Sys_RoleOper { get; set; }
    }
}