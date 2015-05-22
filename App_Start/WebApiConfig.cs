using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using TheService.APIFramework;

namespace TheService
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
                 routeTemplate: "api/{controller}/{action}/{id}",
                 defaults: new { id = RouteParameter.Optional }
             );

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

  
            config.Filters.Add(new CustomExceptionFilterAttribute());
            
            //this wont Work if the CustomExceptionFilterAttribute is used. unless the exception gets thrown in the CustomExceptionFilterAttribute itself.
            config.Services.Replace(typeof(IExceptionHandler), new CustomExceptionHandler()); 

            config.MessageHandlers.Add(new ErrorHandler());

        }
    }
}
