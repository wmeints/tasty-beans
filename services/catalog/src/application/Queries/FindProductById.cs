using MediatR;

namespace TastyBeans.Catalog.Application.Queries;

public record FindProductById(Guid Id): IRequest<FindProductByIdResult>;