using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TastyBeans.Identity.Api;
using TastyBeans.Identity.Application.EventHandlers;
using TastyBeans.Identity.Domain.Aggregates.UserAggregate;
using TastyBeans.Identity.Infrastructure.Persistence;
using TastyBeans.Shared.Diagnostics;
using TastyBeans.Shared.Infrastructure.Bindings;

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
    .AddInMemoryApiScopes(Configuration.GetApiScopes())
    .AddInMemoryIdentityResources(Configuration.GetIdentityResources())
    .AddProfileService<ApplicationProfileService>()
    .AddAspNetIdentity<ApplicationUser>();

builder.Services.AddHeaderPropagation();
builder.Services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();

var telemetryOptions = builder.Configuration.GetSection("Telemetry").Get<TelemetryOptions>();

builder.Services.AddTracing(telemetryOptions,
    "TastyBeans.Identity.Api",
    "TastyBeans.Identity.Application",
    "TastyBeans.Identity.Domain",
    "TastyBeans.Identity.Infrastructure");

builder.Services.AddMetrics(telemetryOptions,
    "TastyBeans.Identity.Api",
    "TastyBeans.Identity.Application",
    "TastyBeans.Identity.Domain",
    "TastyBeans.Identity.Infrastructure");

builder.Services.AddLogging(telemetryOptions);

builder.Services.AddScoped<CustomerRegisteredEventHandler>();
builder.Services.AddEmailSender();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await using var scope = app.Services.CreateAsyncScope();
    await using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    await dbContext.Database.MigrateAsync();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | 
                       ForwardedHeaders.XForwardedProto | 
                       ForwardedHeaders.XForwardedHost
});

app.UseStaticFiles();
app.UseHeaderPropagation();
app.UseCloudEvents();
app.MapHealthChecks("/healthz", new HealthCheckOptions()
{
    AllowCachingResponses = false
});

// This is bad for your health, but we need this to make the demo work properly.
// Might change this later to the correct host headers, etc.
app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.UseAuthentication();
app.UseAuthorization();
app.UseIdentityServer();
app.MapRazorPages();
app.MapSubscribeHandler();
app.MapControllers();

app.Run();