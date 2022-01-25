using MediatR;
using Shared;
using Shared.DTOs;

namespace CheckoutService.Commands
{
    public record CheckOutCommand(Guid UserId, List<Guid> ProductIds, string ShippingAddress) : IRequest<CommandResponse<bool>>
    {

    }
}
