using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace TheService.APIFramework.Security
{

    /// <summary>
    /// When using a Custom Authorization attribute
    /// </summary>
    public class CustomAuth : AuthorizeAttribute
    {
        public string MyClaimType { get; set; }
        public string MyClaimValue { get; set; }

        public override Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            #region Two ways to get all user related claims/Roles
            var identity = actionContext.RequestContext.Principal.Identity as ClaimsIdentity;
            var principal = actionContext.RequestContext.Principal as ClaimsPrincipal;
            #endregion

            #region Do Something with Headers
            //Get bearer token from headers 
            IEnumerable<string> headerValues = actionContext.Request.Headers.GetValues("Authorization");
            string strToken = headerValues.FirstOrDefault();

            //Remove 'Bearer' from string and convert string to a security token
            JwtSecurityTokenHandler jwt = new JwtSecurityTokenHandler();
            string[] splitAuthHeader = strToken.Split(' ');
            SecurityToken st = jwt.ReadToken(splitAuthHeader[1].Trim());

            //These two values are of no consequence but are here just to show how you can send in values using custom attributes
            //These can then be checked against the roles or claims in the {SecurityToken st} variable above.
            string claimType = MyClaimType;
            string claimValue = MyClaimValue;

            #endregion

            return base.OnAuthorizationAsync(actionContext, cancellationToken);
        }
    }
}