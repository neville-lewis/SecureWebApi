using System.Linq;
using System.Security.Claims;


namespace TheService.APIFramework.Security
{
    public class ClaimCheck : ClaimsAuthorizationManager
    {

        public override bool CheckAccess(AuthorizationContext context)
        {
            var action = context.Action.First().Value;

            #region Uncomment the lines below when using ClaimsPrincipalPermission; Not required when using custom claims [SetClaim].
            //var resource = context.Resource.First().Value;
            //this will pass only when calling a method decorated with [ClaimsPrincipalPermission] with proper (Operation) and (Resource) set
            //bool retValForClaimsPrincipalPermission = context.Principal.HasClaim(action, resource);
            #endregion     

            //this will return true only when calling a method decorated with  a custom ClaimsPrincipalPermission, [SetClaim] in this project.
            bool retVal = context.Principal.HasClaim(" ", action);
            return retVal;
        }
    }
}
