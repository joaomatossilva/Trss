using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Trss.Identity
{
    public static class PrincipalExtensions
    {
        public static string GetUserId(this IPrincipal principal)
        {
            return GetClaimValue(principal, TrrsClaimTypes.UserId);
        }

        private static string GetClaimValue(IPrincipal principal, string claimType)
        {
            var claimsPrincipal = principal as ClaimsPrincipal;
            if (claimsPrincipal == null)
            {
                return String.Empty;
            }
            var claim = claimsPrincipal.FindFirst(claimType);
            if (claim == null)
            {
                return String.Empty;
            }

            return claim.Value;
        }
    }
}
