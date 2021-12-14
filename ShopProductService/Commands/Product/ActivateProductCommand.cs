using MediatR;
using Shared;
using System;

namespace ShopProductService.Commands.Product
{
    public class ActivateProductCommand : IRequest<CommandResponse<bool>>
    {
        public Guid Id { get; set; }

        public bool IsActivateCommand { get; set; }
    }
}
