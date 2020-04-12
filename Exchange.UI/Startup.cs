using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
[assembly: OwinStartup(typeof(Exchange.UI.Startup))]
namespace Exchange.UI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ServicePointManager.Expect100Continue = true;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            app.MapSignalR();
        }
    }
}