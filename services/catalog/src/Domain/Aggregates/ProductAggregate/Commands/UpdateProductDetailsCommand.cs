using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Commands
{
    public record UpdateProductDetailsCommand(Guid Id, string Name, string Description,  IEnumerable<ProductVariant> Variants);
}
