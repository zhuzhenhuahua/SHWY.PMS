using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SHWY.PMS.Controllers.Filter
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
            var sessionUser = filterContext.HttpContext.Session["CurrentUser"];
            if (sessionUser == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Account",
                    action = "Login"
                }));
                //filterContext.Result = new ContentResult() { Content = "登录超时，请重新登录" };
                //filterContext.Result = new JavaScriptResult() { Script = "alert('dfd')" };
                //var result= new JavaScriptResult() { Script = "window.top.location.href = '/Account/Login'" };
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