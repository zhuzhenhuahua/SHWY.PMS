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
            var sessionUser= filterContext.HttpContext.Session["CurrentUser"];
            if ( sessionUser == null)
            {
                //filterContext.Controller = new AccountController();
                //string scheme = filterContext.HttpContext.Request.Url.Scheme;//http
                //string authority = filterContext.HttpContext.Request.Url.Authority;//localhost:28158
                //string path = scheme + "://" + authority + "/Account/Login";
                //filterContext.Result = new RedirectResult(path);
                filterContext.Result = new ContentResult() { Content = "登录超时，请重新登录" };
                //filterContext.Result = new JavaScriptResult() { Script = "alert('dfd')" };
                //var result= new JavaScriptResult() { Script = "window.top.location.href = '/Account/Login'" };
            }
            //else
            //{

            //}
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