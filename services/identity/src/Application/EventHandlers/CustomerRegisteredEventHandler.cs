using Domain.Aggregates.UserAggregate;
using Microsoft.AspNetCore.Identity;
using RecommendCoffee.Identity.Application.IntegrationEvents;

namespace RecommendCoffee.Identity.Application.EventHandlers;

public class CustomerRegisteredEventHandler
{
    private readonly UserManager<ApplicationUser> _userManager;

    public CustomerRegisteredEventHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task HandleAsync(CustomerRegisteredEvent evt)
    {
        var user = new ApplicationUser
        {
            CustomerId = evt.CustomerId,
            Email = evt.EmailAddress,
            UserName = evt.EmailAddress,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };

        await _userManager.CreateAsync(user);
    }
}