using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TastyBeans.Identity.Api.Models;

namespace TastyBeans.Identity.Api.Pages;

public class LogoutPageModel : PageModel
{
    private readonly IIdentityServerInteractionService _interactionService;
    private readonly SignInManager<ApplicationUser> _userManager;

    public LogoutPageModel(IIdentityServerInteractionService interactionService, SignInManager<ApplicationUser> userManager)
    {
        _interactionService = interactionService;
        _userManager = userManager;
    }

    public async Task<IActionResult> OnGetAsync(string logoutId)
    {
        var context = await _interactionService.GetLogoutContextAsync(logoutId);
        await _userManager.SignOutAsync();

        return Redirect(context.PostLogoutRedirectUri);
    }
}