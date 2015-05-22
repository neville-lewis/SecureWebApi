using System;
using System.IdentityModel.Services;
using System.Security;
using System.Security.Permissions;


namespace TheService.APIFramework.Security
{
    /// <summary>
    /// Custom Claim
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]
    public sealed class SetClaimAttribute : CodeAccessSecurityAttribute
    {
        public string Value { get; set; }
        

        public SetClaimAttribute(SecurityAction action = SecurityAction.Demand)
            : base(action)
        {
        }

        public override IPermission CreatePermission()
        {
            return new ClaimsPrincipalPermission(" ", this.Value.ToString());
        }
    }
}