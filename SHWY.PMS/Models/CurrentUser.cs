using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SHWY.Model.DB;

namespace SHWY.PMS
{
    public static class CurrentUser
    {
        /// <summary>
        /// 当前用户实体
        /// </summary>
        public static Sys_User Sys_User { get; set; }
        /// <summary>
        /// 当前用户所属角色下的菜单集合
        /// </summary>
        public static List<Sys_RoleMenu> Sys_RoleMenu { get; set; }
        /// <summary>
        /// 当前用户所属角色下的菜单操作按钮集合
        /// </summary>
        public static List<Sys_RoleOper> Sys_RoleOper { get; set; }
    }
}