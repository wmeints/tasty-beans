using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TastyBeans.Identity.Api.Models;
using TastyBeans.Identity.Api.Services;

namespace TastyBeans.Identity.Api.Pages;

public class LoginPageModel : PageModel
{
    private readonly EmailSender _emailSender;
    private readonly UserManager<ApplicationUser> _userManager;

    public LoginPageModel(UserManager<ApplicationUser> userManager, EmailSender emailSender)
    {
        _userManager = userManager;
        _emailSender = emailSender;
    }

    [BindProperty]
    public LoginPageModelInput Input { get; set; } = new ();

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await _userManager.FindByEmailAsync(Input.EmailAddress);

        if (user != null)
        {
            var token = await _userManager.GenerateUserTokenAsync(user, 
                "PasswordlessLoginTokenProvider",
                "PasswordlessLogin");

            var pageUrl = Url.Page("/CompleteLogin", new { token = token, userId = user.Id, returnUrl });
            var loginUrl = $"{Request.Scheme}://{Request.Host}{pageUrl}";

            await _emailSender.SendEmailAsync(user.Email, "Complete your login",
                $"Please login using this <a href='{HtmlEncoder.Default.Encode(loginUrl)}'>magic link</a>.");

            return RedirectToPage("/LoginPending");
        }
        else
        {
            ModelState.AddModelError("", "Invalid login attempt.");
        }

        return Page();
    }
    
    public class LoginPageModelInput
    {
        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
    }
}