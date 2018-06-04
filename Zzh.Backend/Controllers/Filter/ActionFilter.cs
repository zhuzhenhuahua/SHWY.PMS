using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Zzh.Backend.Controllers.Filter
{
    public class ActionFilter : FilterAttribute, IActionFilter
    {
        /// <summary>
        /// 被拦截Action前执行
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
        }
        /// <summary>
        /// 被拦截Action后执行
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }
    }
}