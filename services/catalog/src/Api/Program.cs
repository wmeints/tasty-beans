var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CatalogDatabase"));
});

builder.Services.AddScoped<IEventStore<Product, Guid>, EventStore<Product, Guid>>();
builder.Services.AddScoped<IProductInformationRepository, ProductInformationRepository>();
builder.Services.AddScoped<IIntegrationEventPublisher, IntegrationEventPublisher>();
builder.Services.AddScoped<ProductInformationProjector>();
builder.Services.AddScoped<RegisterProductCommandHandler>();
builder.Services.AddScoped<FindAllProductsQueryHandler>();
builder.Services.AddScoped<FindProductQueryHandler>();

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

app.MapGet("/products", Routes.FindAllProducts);
app.MapGet("/products/{id}", Routes.FindProductById).WithName("GetProductDetails");
app.MapPost("/products", Routes.RegisterProduct);
app.MapPut("/products/{id}", Routes.UpdateProductDetails);

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

await dbContext.Database.MigrateAsync();

app.Run();
