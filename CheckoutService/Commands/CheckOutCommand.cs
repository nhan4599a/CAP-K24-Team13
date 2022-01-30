using MediatR;
using Shared;

namespace CheckoutService.Commands
{
    public record CheckOutCommand(Guid UserId, List<Guid> ProductIds, string ShippingName, string ShippingPhone,
        string ShippingAddress, string OrderNotes)
        : IRequest<CommandResponse<bool>>
    {

    }
}
