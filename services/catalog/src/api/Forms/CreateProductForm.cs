using System.ComponentModel.DataAnnotations;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate;

namespace TastyBeans.Catalog.Api.Forms;

public class CreateProductForm
{
    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; } = "";
    
    [Required(AllowEmptyStrings = false)]
    public string Description { get; set; } = "";
    public List<ProductVariant> Variants { get; set; } = new();
}