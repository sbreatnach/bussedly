using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Cors;

namespace bussedly
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // allow CORS requests for all services on this server
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            config.Services.Add(typeof(IExceptionLogger),
                                new CommonExceptionLogger());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "v1/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
