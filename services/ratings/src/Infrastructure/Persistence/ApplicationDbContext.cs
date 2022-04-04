using Microsoft.EntityFrameworkCore;

namespace RecommendCoffee.Catalog.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }
}