using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using System.Threading.Tasks;

using TheService.Models;

namespace TheService.APIFramework.Security
{
    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {
  

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override Task MatchEndpoint(OAuthMatchEndpointContext context)
        {
            if (context.OwinContext.Request.Method == "OPTIONS" && context.IsTokenEndpoint)
            {
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "POST" });
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "accept", "authorization", "content-type" });
                context.OwinContext.Response.StatusCode = 200;
                context.RequestCompleted();

                return Task.FromResult<object>(null);
            }

            return base.MatchEndpoint(context);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            string allowedOrigin = "*";

            #region This section to be checked against a Data Source and populated with values from there.
            TheUserLoggedIn user = new TheUserLoggedIn();
            user.UserName = context.UserName;
            user.FirstName = "Neville";
            user.LastName = "Lewis";

            #endregion

            #region Check if User object created after verifying credentials against a data source
            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }
            #endregion

            ClaimsIdentity identity = new ClaimsIdentity("JWT-BearerAuth-Test");

            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
        
            foreach (string claim in user.Claims)
            {
                #region USING CUSTOM AUTHORIZE ATTRIBUTE ON API METHODS WITH ROLES THAT CAN MASQUERADE AS CLAIMS
                //identity.AddClaim(new Claim(ClaimTypes.Role, claim, ClaimValueTypes.String));
                #endregion

                #region An example of setting up Claims based authorization using ClaimsPrincipalPermission
                //identity.AddClaim(new Claim("Operation", claim, ClaimValueTypes.String));
            	#endregion

                #region Claims using custom claim attribute called SetClaim.
                identity.AddClaim(new Claim(" ", claim, ClaimValueTypes.String));
                #endregion
            }

            var ticket = new AuthenticationTicket(identity, null);
            context.Validated(ticket);
        }
    }
}