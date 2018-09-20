using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SHWY.PMS.Controllers.Filter
{
    public class ExceptionFilterAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            Exception ex = filterContext.Exception;
            filterContext.HttpContext.Response.Write(ex.Message);
        }
    }
}