var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CustomersDatabase"));
});

builder.Services.AddScoped<IEventStore<Customer,Guid>, EventStore<Customer, Guid>>();
builder.Services.AddScoped<IIntegrationEventPublisher, IntegrationEventPublisher>();

builder.Services.Configure<IntegrationEventPublisherOptions>(builder.Configuration.GetSection("Integration"));

builder.Services.AddDaprClient();
builder.Services.AddMvc();

builder.Services.AddProblemDetails(options =>
{
    options.Map<BusinessRulesViolationException>(ProblemDetailsMappings.BusinessRulesViolation);
    options.Map<AggregateNotFoundException>(ProblemDetailsMappings.AggregateNotFound);
});

var app = builder.Build();

app.UseProblemDetails();

app.MapPost("/customers", Routes.RegisterCustomer);

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

await dbContext.Database.MigrateAsync();

app.Run();
