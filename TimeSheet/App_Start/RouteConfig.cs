using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TimeSheetSession
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                 name: "Default",
                 url: "{controller}/{action}/{id}",
                 defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
             );

            routes.MapRoute(
         name: "ValidateLoginUser",
         url: "{controller}/{action}",
         defaults: new { controller = "Login", action = "ValidateUser" }
         );

            routes.MapRoute(
               name: "SaveTimesheetEntry",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Timesheet", action = "SaveTimesheetEntry", id = UrlParameter.Optional }
           );

            routes.MapRoute(
              name: "GetEmployeeList",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Dashboard", action = "GetEmployeeList", id = UrlParameter.Optional }
          );

            routes.MapRoute(
              name: "SaveEmployeeDetail",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Dashboard", action = "SaveEmployeeDetail", id = UrlParameter.Optional }
          );

            routes.MapRoute(
             name: "GetFilterWorkList",
             url: "{controller}/{action}/{id}",
             defaults: new { controller = "Dashboard", action = "GetFilterWorkList", id = UrlParameter.Optional }
         );
            routes.MapRoute(
            name: "GetFilterProjectList",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Dashboard", action = "GetFilterProjectList", id = UrlParameter.Optional }
        );


            routes.MapRoute(
              name: "SaveProjectDetail",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Dashboard", action = "SaveProjectDetail", id = UrlParameter.Optional }
          );

            routes.MapRoute(
              name: "GetDashboardMenu",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Dashboard", action = "GetDashboardMenu", id = UrlParameter.Optional }
          );

            routes.MapRoute(
              name: "GetWorkingHourList",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Dashboard", action = "GetWorkingHourList", id = UrlParameter.Optional }
          );

            routes.MapRoute(
             name: "LogOut",
             url: "{controller}/{action}/{id}",
             defaults: new { controller = "Dashboard", action = "LogOut", id = UrlParameter.Optional }
         );

            routes.MapRoute(
               name: "SaveChatForum",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "ChatForum", action = "SaveChatForum", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "GetChatForumDetailList",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "ChatForum", action = "GetChatForumDetailList", id = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "DeleteProject",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Dashboard", action = "DeleteProject", id = UrlParameter.Optional }
            ); 
        }
    }
}
