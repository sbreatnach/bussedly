using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;
using bussedly.Models;

namespace bussedly
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            
            // register all your components with the container here
            container.RegisterType<IStopRepository, BusEireannRepository>(
                new ContainerControlledLifetimeManager());
            container.RegisterType<IBusRepository, BusEireannRepository>(
                new ContainerControlledLifetimeManager());
            
            GlobalConfiguration.Configuration.DependencyResolver =
                new UnityDependencyResolver(container);
        }
    }
}