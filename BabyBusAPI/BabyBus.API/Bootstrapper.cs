using System.Diagnostics;
using Autofac;
using Autofac.Integration.WebApi;
using System.Reflection;
using BabyBus.Data.Infrastructure;
using BabyBus.Data.Repositories.Login;
using BabyBus.Service.General;
using System.Web.Http;
namespace BabyBus.API
{
    public class Bootstrapper
    {
        public static void Run()
        {
            {
                Stopwatch w = new Stopwatch();
                SetAutofacWebApiServices();
            }
        }
        private static void SetAutofacWebApiServices()
        {
            var configuration = GlobalConfiguration.Configuration;
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(UserRepository).Assembly)
            .Where(t => t.Name.EndsWith("Repository"))
            .AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(UserService).Assembly)
            .Where(t => t.Name.EndsWith("Service"))
            .AsImplementedInterfaces().InstancePerRequest();

            IContainer container = builder.Build();//创建一个容器
            var resolver = new AutofacWebApiDependencyResolver(container);
            configuration.DependencyResolver = resolver;
        }
    }
}