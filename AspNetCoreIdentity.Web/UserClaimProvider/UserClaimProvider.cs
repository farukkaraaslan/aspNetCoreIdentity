using AspNetCoreIdentity.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AspNetCoreIdentity.Web.UserClaimProvider;

public class UserClaimProvider : IClaimsTransformation
{
    private readonly UserManager<AppUser> userManager;

    public UserClaimProvider(UserManager<AppUser> userManager)
    {
        this.userManager = userManager;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var identityUser= principal.Identity as ClaimsIdentity;
        var currentUser = await userManager.FindByNameAsync(identityUser.Name);

        if (currentUser == null)
        {
            return principal;
        }

        if (currentUser.City != null)
        {
            if (principal.HasClaim(x=>x.Type != "city"))
            {
                Claim cityClaim = new Claim("city", currentUser.City);
                identityUser.AddClaim(cityClaim);
            }
        }
        return principal;
    }
}
