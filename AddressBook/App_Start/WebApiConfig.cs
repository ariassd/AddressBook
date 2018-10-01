using System.Web.Http;

namespace AddressBook
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

            config.Routes.MapHttpRoute(
                name: "Api name",
                routeTemplate: "api/{controller}/{field}/{text}",
                defaults: new { field = RouteParameter.Optional, text = RouteParameter.Optional }
            );
        }
    }
}
