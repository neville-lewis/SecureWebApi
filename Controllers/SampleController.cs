using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Permissions;
using System.Web.Http;
using TheService.APIFramework.Security;

namespace TheService.Controllers
{
    public class SampleController : ApiController
    {
        public IHttpActionResult ReadData(string id)
        {
            
            if (string.IsNullOrEmpty(id))
            {
                return NotFound(); 
            }
            return Ok("ID: " + id);
        }

        #region Secured Methods

        [SetClaim(Value = "Purge")]
        [HttpPost]
        public IHttpActionResult PurgeIt(string id)
        {
            return Ok("Purged id:" + id);
        }


        #region Other methods. Uncomment respective sections in the CustomOAuth class to use these methods.

        [ClaimsPrincipalPermission(SecurityAction.Demand, Operation = "Operation", Resource = "Purge")]
        [HttpPost]
        public IHttpActionResult Purge(string id)
        {

            return Ok("Purged id:" + id);
        }

        [Authorize(Roles = "EditRecord")]
        public IHttpActionResult EditData(string id)
        {
            return Ok("Edited ID:" + id);
        }


        [CustomAuth(MyClaimType = "Action", MyClaimValue = "Delete", Roles = "DeleteRecord")]
        [HttpPost]
        public IHttpActionResult DeleteData(string id)
        {

            return Ok("Deleted record ID:" + id);
        }

        
        #endregion
        
        #endregion

        #region Exception Related


        [SetClaim(Value = "EditRecord")]
        [HttpPost]
        public IHttpActionResult UnhandledError(string id)
        {
            throw new ArgumentException("Some unhandled exception thrown from api method here");
            return Ok("Edited :" + id);
        }



        [SetClaim(Value = "EditRecord")]
        [HttpPost]
        public HttpResponseMessage ReturnException(string id)
        {
            //this will not show up on the error handlers registered
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Something wrong in the input id:" + id);
            return response;
        }



        [SetClaim(Value = "EditRecord")]
        [HttpPost]
        public IHttpActionResult SimulateExceptionInExceptionFilter(string id)
        {
            throw new ArgumentNullException("Simulated an unhandled exception in the exception filter");
            return Ok("Edited id:" + id);
        }


        #endregion

    }
}
