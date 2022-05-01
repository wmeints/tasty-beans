using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TastyBeans.Identity.Domain.Aggregates.UserAggregate;

namespace TastyBeans.Identity.Api.Pages;

public class ConfirmLoginPageModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public ConfirmLoginPageModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<IActionResult> OnGetAsync(string userId, string token, string returnUrl)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user != null)
        {
            var succeeded = await _userManager.VerifyUserTokenAsync(
                user, "PasswordlessLoginTokenProvider", "PasswordlessLogin", token);

            if (succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return LocalRedirect(returnUrl);
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt");
            }
        }
        else
        {
            ModelState.AddModelError("", "Invalid login attempt");
        }

        return Page();
    }
}