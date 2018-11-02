using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SHWY.PMS
{
    public class ServerHub : Hub
    {
        public void Send(SignalRMessage message)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<ServerHub>();
            context.Clients.All.send(message);
        }
    }
    /// <summary>
    /// 返回的数据类型
    /// </summary>
    public class SignalRMessage
    {
        public string clientIp { get; set; }
        public string head { get; set; }
        public string body { get; set; }
        public SignalRMessage(string clientIp, string _head, string _body)
        {
            this.clientIp = clientIp;
            this.head = _head;
            this.body = _body;
        }
    }
}