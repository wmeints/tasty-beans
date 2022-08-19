using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TastyBeans.Identity.Api.Configuration;
using TastyBeans.Identity.Api.Data;
using TastyBeans.Identity.Api.Models;
using TastyBeans.Identity.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultDatabase"));
});

builder.Services.AddControllers();
builder.Services.AddRazorPages();

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.SignIn.RequireConfirmedEmail = false;
        options.SignIn.RequireConfirmedPhoneNumber = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddTokenProvider("PasswordlessLoginTokenProvider", typeof(PasswordlessLoginTokenProvider));

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddInMemoryClients(IdentityConfig.GetClients())
    .AddInMemoryApiScopes(IdentityConfig.GetApiScopes())
    .AddInMemoryApiResources(IdentityConfig.GetApiResources())
    .AddInMemoryIdentityResources(IdentityConfig.GetIdentityResources())
    .AddProfileService<ApplicationProfileService>()
    .AddAspNetIdentity<ApplicationUser>();

builder.Services.AddTransient(sp => new EmailSender(
    builder.Configuration["Email:HostName"],
    Int32.Parse(builder.Configuration["Email:Port"])));

var app = builder.Build();

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseIdentityServer();
app.MapRazorPages();

app.Run();