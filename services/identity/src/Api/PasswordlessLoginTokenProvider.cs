using Domain.Aggregates.UserAggregate;
using Microsoft.AspNetCore.Identity;

namespace RecommendCoffee.Identity.Api;

public class PasswordlessLoginTokenProvider: TotpSecurityStampBasedTokenProvider<ApplicationUser>
{
    public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<ApplicationUser> manager, ApplicationUser user)
    {
        return Task.FromResult(false);
    }

    public override async Task<string> GetUserModifierAsync(string purpose, UserManager<ApplicationUser> manager, ApplicationUser user)
    {
        var emailAddress = await manager.GetEmailAsync(user);
        return $"PasswordlessLogin:{purpose}:{emailAddress}";
    }
}