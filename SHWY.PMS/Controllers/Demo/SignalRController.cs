using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SHWY.PMS.Controllers.Demo
{
    [RoutePrefix("api/signalr")]
    public class SignalRController : ApiController
    {
        [HttpGet]
        [Route("testapi")]
        public string testapi(string msg)
        {
            SignalRMessage message = new PMS.SignalRMessage("All","testapi", msg);
            ServerHub hub = new ServerHub();
            hub.Send(message);
            return "sucess";
        }
    }
}
