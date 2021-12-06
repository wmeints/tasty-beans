using RecommendCoffee.Customers.Application.CommandHandlers;
using RecommendCoffee.Customers.Application.QueryHandlers;
using RecommendCoffee.Customers.Domain.Projections.CustomerInfoProjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CustomersDatabase"));
});

builder.Services.AddScoped<IEventStore<Customer,Guid>, EventStore<Customer, Guid>>();
builder.Services.AddScoped<IIntegrationEventPublisher, IntegrationEventPublisher>();
builder.Services.AddScoped<ICustomerInformationRepository, CustomerInformationRepository>();
builder.Services.AddScoped<CustomerInformationProjector>();
builder.Services.AddScoped<RegisterCustomerCommandHandler>();
builder.Services.AddScoped<FindCustomerQueryHandler>();
builder.Services.AddScoped<FindAllCustomersQueryHandler>();

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

app.MapGet("/customers", Routes.FindAllCustomers);
app.MapGet("/customers/{id}", Routes.FindCustomerById);
app.MapPost("/customers", Routes.RegisterCustomer);

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

await dbContext.Database.MigrateAsync();

app.Run();
