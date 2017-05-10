using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;
using AdministratorManagement.Core.DataAccess;
using AdministratorManagement.Core.Handlers;
using CacheCow.Server;

namespace AdministratorManagement
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public override void Init()
        {
            //webapi should open session here
            this.PostAuthenticateRequest += (sender, e) => HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            base.Init();
        }
        protected void Application_Start()
        {
            //AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);

            //AuthConfig.RegisterOpenAuth();
            GlobalConfiguration.Configure(WebApiConfig.Register);
          

            //Database.SetInitializer(new CreateDatabaseIfNotExists<BabyBusDataContext>());
            log4net.Config.XmlConfigurator.Configure();
            Bootstrapper.Run();

            //表示不使用 Content-Type 标头的在 RFC 2616 中定义的媒体类型。
            //http://www.asp.net/web-api/overview/formats-and-model-binding/media-formatters
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            //etag
            GlobalConfiguration.Configuration.MessageHandlers.Add(new CachingHandler(GlobalConfiguration.Configuration));

            GlobalConfiguration.Configuration.MessageHandlers.Add(
                new AuthenticationHandler());
            GlobalConfiguration.Configuration.MessageHandlers.Add(new ResponseHeaderHandler());
        }
    }
}
