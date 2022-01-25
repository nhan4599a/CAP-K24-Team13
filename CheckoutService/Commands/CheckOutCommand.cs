using MediatR;
using Shared.DTOs;

namespace CheckoutService.Commands
{
    public record CheckOutCommand(Guid UserId, List<Guid> ProductIds) : IRequest
    {

    }
}
