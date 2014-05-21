using System.Net.Http.Headers;
using System.Web.Http;
using AutoMapper;
using VNApi2.BLL;

namespace VNApi2
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //for JSON:
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize;
            config.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;

            config.Routes.MapHttpRoute(
                name: "LangCat",
                routeTemplate: "api/{controller}/{language}/{category}",
                defaults: new { language = "no", category = "Overnatting"}
            );

            config.Routes.MapHttpRoute(
                name: "Lang",
                routeTemplate: "api/{controller}/{language}"
            );

            config.Routes.MapHttpRoute(
                name: "LangId",
                routeTemplate: "api/{controller}/{language}/{id}",
                defaults: new { language = "no"/*, id = RouteParameter.Optional */}
            );

             config.Routes.MapHttpRoute(
                name: "PostalArea",
                routeTemplate: "api/ProductByPost/{area}",
                defaults: new { area = "OSLO"}
            );

            config.Routes.MapHttpRoute(
                name: "Position",
                routeTemplate: "api/{controller}/{language}/{latitude}/{longitude}",
                defaults: new { latitude = RouteParameter.Optional, longitude = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/"
            );


            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            config.EnableQuerySupport();

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            config.EnableSystemDiagnosticsTracing();

            Mapper.AddProfile<DomainToApiModel>();
        }
    }
}
