using SHWY.Lib.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SHWY.PMS.Controllers.Filter
{
    /// <summary>
    /// 标识调用Action后是否写入日志
    /// </summary>
    public class LogRecordAttribute : ActionFilterAttribute
    {
        private string logInfo { get; set; }
        public LogRecordAttribute(string _logInfo)
        {
            logInfo = _logInfo;
        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
           var currentUser= filterContext.HttpContext.Session["CurrentUser"] as CurrentUser;
            SHLog4net.LogInfo(currentUser?.Sys_User.LoginName + logInfo);
            base.OnActionExecuted(filterContext);

        }
    }
}