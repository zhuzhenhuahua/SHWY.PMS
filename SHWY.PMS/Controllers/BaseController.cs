using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SHWY.PMS.Controllers.Filter;

namespace SHWY.PMS.Controllers
{
    [ActionFilter]
    [ExceptionFilter]
    public class BaseController : Controller
    {
    }
}