using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Zzh.Backend.Controllers.Filter
{
    public class ActionFilter : ActionFilterAttribute
    {
        /// <summary>
        /// 被拦截Action前执行
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
           // base.OnActionExecuting(filterContext);
            var sessionUser= filterContext.HttpContext.Session["CurrentUser"];
            if (sessionUser == null)
            {
                //filterContext.Result = new RedirectResult("/Account/Login");
                filterContext.Result = new JavaScriptResult() { Script = "alert('dfd')" };
            }
            else
            {

            }
            base.OnActionExecuting(filterContext);
        }
        /// <summary>
        /// 被拦截Action后执行
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }
    }
}