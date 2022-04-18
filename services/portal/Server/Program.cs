var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.MapHealthChecks("/healthz");
app.MapFallbackToFile("index.html");

app.Run();
