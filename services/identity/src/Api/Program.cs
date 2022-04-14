using System.Text.Json;
using System.Text.Json.Serialization;
using Domain.Aggregates.UserAggregate;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecommendCoffee.Identity.Api;
using RecommendCoffee.Identity.Application.Common;
using RecommendCoffee.Identity.Application.EventHandlers;
using RecommendCoffee.Identity.Infrastructure.Bindings;
using RecommendCoffee.Identity.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultDatabase"), 
        sql => sql.EnableRetryOnFailure());
});

builder.Services.AddRazorPages();

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    })
    .AddDapr(daprClientBuilder =>
    {
        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    
        serializerOptions.Converters.Add(new JsonStringEnumConverter());

        daprClientBuilder.UseJsonSerializationOptions(serializerOptions);
    });

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.SignIn.RequireConfirmedEmail = false;
        options.SignIn.RequireConfirmedPhoneNumber = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddTokenProvider("PasswordlessLoginTokenProvider", typeof(PasswordlessLoginTokenProvider));;

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddInMemoryClients(Configuration.GetClients())
    .AddInMemoryApiResources(Configuration.GetApiResources())
    .AddInMemoryIdentityResources(Configuration.GetIdentityResources())
    .AddAspNetIdentity<ApplicationUser>();

builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("DefaultDatabase"))
    .AddDbContextCheck<ApplicationDbContext>();

builder.AddTelemetry();

builder.Services.AddScoped<CustomerRegisteredEventHandler>();
builder.Services.AddTransient<IEmailSender, EmailSender>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await using var scope = app.Services.CreateAsyncScope();
    await using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    await dbContext.Database.MigrateAsync();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseStaticFiles();
app.UseCloudEvents();
app.MapHealthChecks("/healthz", new HealthCheckOptions()
{
    AllowCachingResponses = false
});
app.UseAuthentication();
app.UseAuthorization();
app.UseIdentityServer();
app.MapRazorPages();
app.MapSubscribeHandler();
app.MapControllers();

app.Run();