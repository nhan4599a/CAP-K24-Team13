﻿using DatabaseAccessor.Repositories.Interfaces;
using MediatR;
using Shared;
using ShopProductService.Commands.Cart;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Cart
{
    public class EditQuantityCartItemHandler : IRequestHandler<EditQuantityCartItemCommand, CommandResponse<bool>>
    {
        private readonly ICartRepository _cartRepository;

        public EditQuantityCartItemHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        public async Task<CommandResponse<bool>> Handle(EditQuantityCartItemCommand request, CancellationToken cancellationToken)
        {
            return await _cartRepository.EditQuantity(request.requestModel);
        }
    }
}
