using System;
using System.Diagnostics;
using System.Reflection;
using System.Security;
using System.Web.Http;
using AdministratorManagement.Core.DataAccess;
using AdministratorManagement.Core.Services;
using Autofac;
using Autofac.Integration.WebApi;

namespace AdministratorManagement
{
    public class Bootstrapper
    {
        public static void Run()
        {
            {
                SetAutofacWebApiServices();
            }
        }
        private static void SetAutofacWebApiServices()
        {
            var configuration = GlobalConfiguration.Configuration;
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            //builder.RegisterType<AdministratorManagement.Core.DataAccess.UserRepository>().As<AdministratorManagement.Core.DataAccess.IUserRepository>().SingleInstance();
            //builder.RegisterType<AdministratorManagement.Core.DataAccess.AccessTokenRepository>().As<AdministratorManagement.Core.DataAccess.IAccessTokenRepository>().SingleInstance();
            //builder.RegisterType<AuthenticationService>().As<IAuthenticationService>().SingleInstance();

            builder.RegisterAssemblyTypes(typeof(UserRepository).Assembly)
            .Where(t => t.Name.EndsWith("Repository"))
            .AsImplementedInterfaces().SingleInstance();

            builder.RegisterAssemblyTypes(typeof(AuthenticationService).Assembly)
            .Where(t => t.Name.EndsWith("Service"))
            .AsImplementedInterfaces().SingleInstance();

            IContainer container = builder.Build();//创建一个容器
             _resolver = new AutofacWebApiDependencyResolver(container);
            configuration.DependencyResolver = _resolver;
        }

        private static AutofacWebApiDependencyResolver _resolver;
        [SecurityCritical]
        public static T GetService<T>() where T : class
        {
            if (_resolver != null) 
            {
                return _resolver.GetService(typeof(T)) as T;
            }
            else
            {
                return null;
            }
        }

    }
}