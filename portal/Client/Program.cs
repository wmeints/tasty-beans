using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using RecommendCoffee.Portal.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
    .AddHttpClient("RecommendCoffee.Portal.Api", client =>
    {
        client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
    })
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

builder.Services.AddScoped(sp => sp
    .GetRequiredService<IHttpClientFactory>()
    .CreateClient("RecommendCoffee.Portal.Api")
);

builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Authentication", options.ProviderOptions);
    
    options.ProviderOptions.DefaultScopes.Add("catalog");
    options.ProviderOptions.DefaultScopes.Add("registration");
    options.ProviderOptions.DefaultScopes.Add("subscriptions");
    options.ProviderOptions.DefaultScopes.Add("ratings");
});

await builder.Build().RunAsync();
