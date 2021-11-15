using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Events;

public record ProductDetailsUpdated(Guid ProductId, string Name, string Description, IEnumerable<ProductVariant> Variants): Event;
