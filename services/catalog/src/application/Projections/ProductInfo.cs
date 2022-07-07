using Microsoft.EntityFrameworkCore.Query.Internal;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate;

namespace TastyBeans.Catalog.Application.Projections;

public class ProductInfo
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string? Taste { get; set; }
    public string[]? FlavorNotes { get; set; }
    public int? RoastLevel { get; set; }
}