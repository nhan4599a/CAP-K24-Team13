using MediatR;
using Shared.Models;
using System;
using System.Collections.Generic;

namespace CheckoutService.Commands
{
    public record CheckOutCommand(Guid UserId, List<Guid> ProductIds, string ShippingName, string ShippingPhone,
        string ShippingAddress, string OrderNotes, PaymentMethod PaymentMethod)
        : IRequest<CommandResponse<string>>
    {

    }
}
