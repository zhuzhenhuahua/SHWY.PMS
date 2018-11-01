using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SHWY.PMS.Controllers.Demo
{
    [RoutePrefix("api/signalR")]
    public class SignalRController : ApiController
    {
        [HttpGet]
        [Route("testapi")]
        public string testapi(string code)
        {
            return code;
        }
    }
}
