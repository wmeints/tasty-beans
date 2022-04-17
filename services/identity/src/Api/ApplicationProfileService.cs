using System.Security.Claims;
using Domain.Aggregates.UserAggregate;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;

namespace RecommendCoffee.Identity.Api;

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
            new Claim("CustomerId", user.CustomerId?.ToString() ?? Guid.Empty.ToString())
        };
        
        context.IssuedClaims.AddRange(claims);
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var user = await _userManager.GetUserAsync(context.Subject);
        context.IsActive = user != null;
    }
}