using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RecommendCoffee.Identity.Api.Pages;

public class LoginPageModel: PageModel
{
    private readonly IEmailSender _emailSender;
    private readonly UserManager<ApplicationUser> _userManager;

    public LoginPageModel(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
    {
        _userManager = userManager;
        _emailSender = emailSender;
    }

    [BindProperty]
    public LoginPageModelInput Input { get; set; } = new LoginPageModelInput();

    public class LoginPageModelInput
    {
        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = "";
    }

    
    
    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(Input.Email);

            if (user != null)
            {
                var token = await _userManager.GenerateUserTokenAsync(
                    user, "PasswordlessLoginTokenProvider", "PasswordlessLogin");

                var pageUrl = Url.Page("/ConfirmLogin", new { token, userId = user.Id, returnUrl });
                var loginUrl = $"{Request.Scheme}://{Request.Host}{pageUrl}";

                await _emailSender.SendEmailAsync(Input.Email, "Complete your login",
                    $"Please login using this <a href='{HtmlEncoder.Default.Encode(loginUrl)}'>magic link</a>.");

                return RedirectToPage("/LoginStarted");
            }
        }
        else
        {
            ModelState.AddModelError("", "Login attempt failed.");
        }

        return Page();
    }
}