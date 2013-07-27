using System;
using System.Web.Routing;
using Microsoft.AspNet.SignalR;

namespace GameOfLife.SignalRApplication
{
    public class Global : System.Web.HttpApplication
    {
        public void Application_Start(Object sender, EventArgs e)
        {
            RouteTable.Routes.MapHubs();
        }
    }
}