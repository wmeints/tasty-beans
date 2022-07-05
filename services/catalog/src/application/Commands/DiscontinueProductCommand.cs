using MediatR;

namespace TastyBeans.Catalog.Application.Commands;

public record DiscontinueProductCommand(Guid ProductId): IRequest<DiscontinueProductCommandResponse>;