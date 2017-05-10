using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData.Extensions;

namespace BabyBus.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
               name: "BabybusAdminApi",
               routeTemplate: "BabybusAdminFront/api/{controller}/{id}",
               defaults: new { id = RouteParameter.Optional }
           );

           // config.Routes.MapHttpRoute(
           //    name: "BabybusAdminHtml",
           //    routeTemplate: "BabybusAdminFront/*.html",
           //    defaults: new { id = RouteParameter.Optional }
           //);
            config.AddODataQueryFilter();
        }
    }
}
