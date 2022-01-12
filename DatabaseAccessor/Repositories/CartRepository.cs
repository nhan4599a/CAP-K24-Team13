using DatabaseAccessor.Contexts;
using DatabaseAccessor.Mapping;
using DatabaseAccessor.Models;
using DatabaseAccessor.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.DTOs;
using Shared.RequestModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly Mapper _mapper;

        public CartRepository(ApplicationDbContext dbContext, Mapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<CommandResponse<bool>> AddProductToCart(AddOrEditQuantityCartItemRequestModel requestModel)
        {
            var cart = await _dbContext.Carts.
                FirstOrDefaultAsync(c => requestModel.UserId == c.UserId.ToString());
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = Guid.Parse(requestModel.UserId),
                };
            }
            var cartItem = cart.Details.FirstOrDefault(item => item.ProductId.ToString() == requestModel.ProductId);
            if (cartItem == null)
            {
                cart.Details.Add(new CartDetail
                {
                    ProductId = Guid.Parse(requestModel.ProductId),
                    Quantity = 1
                });
            }
            else
            {
                cartItem.Quantity = requestModel.Quantity;
            }
            var result = await _dbContext.SaveChangesAsync() > 0;
            return CommandResponse<bool>.Success(result);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<CommandResponse<bool>> EditQuantity(
            AddOrEditQuantityCartItemRequestModel requestModel)
        {
            var cartItem = await _dbContext.CartDetails
                .FirstOrDefaultAsync(cartItem => cartItem.Cart.UserId.ToString() == requestModel.UserId
                    && cartItem.ProductId.ToString() == requestModel.ProductId);
            if (cartItem == null) return CommandResponse<bool>.Success(false);
            cartItem.Quantity = requestModel.Quantity;
            var result = await _dbContext.SaveChangesAsync() > 0;
            return CommandResponse<bool>.Success(result);
        }

        public async Task<CartDTO> GetCartAsync(string userId = "69")
        {
            var cart = await _dbContext.Carts.AsNoTracking()
                .FirstOrDefaultAsync(cartItem => cartItem.UserId.ToString() == userId);
            return _mapper.MapToCartItemDto(cart);
        }

        public async Task<CommandResponse<bool>> RemoveCartItem(RemoveCartItemRequestModel requestModel)
        {
            var cart = await _dbContext.Carts.FirstOrDefaultAsync(cart => cart.UserId.ToString() == requestModel.UserId);
            if (cart == null)
                return CommandResponse<bool>.Error("User does not have cart", null);
            var cartItem = cart.Details.FirstOrDefault(item => item.ProductId.ToString() == requestModel.ProductId);
            if (cartItem == null) return CommandResponse<bool>.Error("Can't find cart item", null);
            cart.Details.Remove(cartItem);
            var result = await _dbContext.SaveChangesAsync() > 0;
            return CommandResponse<bool>.Success(result);
        }
    }
}
