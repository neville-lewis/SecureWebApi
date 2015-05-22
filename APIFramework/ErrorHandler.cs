using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;
using System.Web.Script.Serialization;
using TheService.Models;

namespace TheService.APIFramework
{

    
    public class ErrorHandler : DelegatingHandler
    {
        /// <summary>
        /// Do something at every request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {

            HttpResponseMessage resp = await base.SendAsync(request, cancellationToken);


            return resp;
        }

    }

 
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            JavaScriptSerializer jz = new JavaScriptSerializer();
            ExceptionDetails ed = new ExceptionDetails();

            if (context.Exception is System.Security.SecurityException)
            {

                ed.err_code = "Some custom error code here";
                ed.message = "not enough privileges.";
                ed.err_type = "Authorization Failed";

                string errMsg = jz.Serialize(ed);

                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent(errMsg),
                    ReasonPhrase = "Security Exception Handled"
                });
            }
            else if (context.Exception is System.ArgumentNullException)
            {
                //throwing a system.exception, as any exception of base type can happen here in the 
                //[CustomExceptionFilterAttribute] as a result of some unhandled exception thrown in the controllers 
                //that got us here.
                throw new Exception("Rethrown from CustomExceptionFilterAttribute: " + context.Exception.Message);
            }
            else if (context.Exception is System.Exception)
            {
                ed.err_code = "Some custom error code here";
                ed.message = context.Exception.Message;
                ed.err_type = "Application";

                string errMsg = jz.Serialize(ed);

                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(errMsg),
                    ReasonPhrase = "Application Exception Handled"
                });

            }
        }

    }


  
    public class CustomExceptionHandler : ExceptionHandler
    {



        public override void Handle(ExceptionHandlerContext context)
        {
            if (context.Exception is System.Exception)
            {
                JavaScriptSerializer jz = new JavaScriptSerializer();
                ExceptionDetails ed = new ExceptionDetails();

                ed.err_code = "";
                ed.message = "Error caught in CustomExceptionHandler: " + context.Exception.Message;
                ed.err_type = "CustomExceptionHandler";

                string errMsg = jz.Serialize(ed);

                HttpResponseMessage response = context.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, errMsg);


                context.Result = new JsonErrorResult
                {
                    Request = context.ExceptionContext.Request,
                    Content = errMsg
                };

            }
            base.Handle(context);

        }


    
        private class JsonErrorResult : IHttpActionResult
        {
            public HttpRequestMessage Request { get; set; }

            public string Content { get; set; }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                HttpResponseMessage response =
                                 new HttpResponseMessage(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(Content);
                response.RequestMessage = Request;
                return Task.FromResult(response);
            }
        }
    }



}
