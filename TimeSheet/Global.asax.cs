using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using TimeSheet.Controllers;

namespace TimeSheetSession
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);//WEB API 1st
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            var msg = new MessageController();
            msg.SetUpTimer();
        }
    }
}
