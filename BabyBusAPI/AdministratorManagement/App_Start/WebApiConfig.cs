using System.Web.Http;
using System.Web.OData.Extensions;

namespace AdministratorManagement
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );

            //config.Routes.MapHttpRoute(
            // name: "BabybusAdminApi",
            // routeTemplate: "melon-flat-ui/Template/api/{controller}/{id}",
            // defaults: new { id = RouteParameter.Optional }
            //);

            config.AddODataQueryFilter();
        }
    }
}
