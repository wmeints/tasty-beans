using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using TastyBeans.Identity.Api.Models;

namespace TastyBeans.Identity.Api.Services;

public class ApplicationProfileService: IProfileService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ApplicationProfileService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var user = await _userManager.GetUserAsync(context.Subject);

        var claims = new List<Claim>
        {
            new("CustomerId", user.CustomerId.ToString() ?? Guid.Empty.ToString())
        };
        
        context.IssuedClaims.AddRange(claims);
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var user = await _userManager.GetUserAsync(context.Subject);
        context.IsActive = user != null;
    }
}