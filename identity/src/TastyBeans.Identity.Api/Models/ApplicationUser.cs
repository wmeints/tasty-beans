using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

namespace TastyBeans.Identity.Api.Models;

public class ApplicationUser: IdentityUser
{
    [NotNull]
    public Guid? CustomerId { get; set; }
}