using System.Web.Http;
using System.Web.Http.Cors;

namespace WebComment.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // enable cross site request
            //config.EnableCors();

            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            // Web API configuration and services
            //config.Filters.Add(new PermissionActionFilter());
            // Use camel case for JSON data.
            // config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            //config.Routes.MapHttpRoute(
            //    name: "Comment",
            //    routeTemplate: "comment/{action}/{id}",
            //    defaults: new { controller = "Comment", action = "Get", id = RouteParameter.Optional }
            //);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
