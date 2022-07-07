using Microsoft.AspNetCore.Identity;

namespace TastyBeans.Identity.Domain.Aggregates.UserAggregate;

public class ApplicationUser: IdentityUser
{
    public Guid? CustomerId { get; set; }
}