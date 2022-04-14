using Microsoft.AspNetCore.Identity;

namespace Domain.Aggregates.UserAggregate;

public class ApplicationUser: IdentityUser
{
    public Guid? CustomerId { get; set; }
}