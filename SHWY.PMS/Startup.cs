using Microsoft.Owin;
using Owin;
[assembly: OwinStartup(typeof(SHWY.PMS.Startup))]

namespace SHWY.PMS
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();//做映射
        }
    }
}