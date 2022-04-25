using Microsoft.AspNetCore.Identity;

namespace RecommendCoffee.Identity.Domain.Aggregates.UserAggregate;

public class ApplicationUser: IdentityUser
{
    public Guid? CustomerId { get; set; }
}