using Microsoft.EntityFrameworkCore;
using RecommendCoffee.Identity.Api;
using RecommendCoffee.Identity.Api.Data;
using RecommendCoffee.Identity.Api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityDatabase"));
});

builder.Services
    .AddDefaultIdentity<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedEmail = false;
        options.SignIn.RequireConfirmedPhoneNumber = false;
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddAspNetIdentity<ApplicationUser>()
    .AddInMemoryIdentityResources(IdentityConfig.IdentityResources)
    .AddInMemoryClients(IdentityConfig.Clients)
    .AddInMemoryApiScopes(IdentityConfig.ApiScopes);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    await dbContext.Database.EnsureCreatedAsync();
    await dbContext.Database.MigrateAsync();
}

app.UseStaticFiles();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapDefaultControllerRoute();

app.Run();
