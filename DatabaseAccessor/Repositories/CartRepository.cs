using DatabaseAccessor.Mapping;
using DatabaseAccessor.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.DTOs;
using Shared.Models;
using Shared.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var cartItem = await _dbContext.CartItems.FirstOrDefaultAsync(c => c.ProductId == requestModel.ProductId && requestModel.UserId == c.UserId);
            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    UserId = requestModel.UserId,
                    ProductId = requestModel.ProductId,
                    Quantity = requestModel.Quantity,
                    Price = requestModel.Price,
                    ShopName = requestModel.ShopName,
                };
                await _dbContext.CartItems.AddAsync(cartItem);
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

        public async Task<CommandResponse<bool>> EditQuantity(AddOrEditQuantityCartItemRequestModel requestModel)
        {
            var cartItem = await _dbContext.CartItems.FirstOrDefaultAsync(c => c.ProductId == requestModel.ProductId && requestModel.UserId == c.UserId);
            if (cartItem == null) return CommandResponse<bool>.Success(false);
            cartItem.Quantity = requestModel.Quantity;
            var result = await _dbContext.SaveChangesAsync() > 0;
            return CommandResponse<bool>.Success(result);
        }

        public List<CartItemDto> GetCartAsync(string userId = "69")
        {
            var cartItems = _dbContext.CartItems.AsNoTracking()
                .Where(cartItem => cartItem.UserId == userId);
            var result = new List<CartItemDto>();
            foreach (var cartItem in cartItems)
            {
                result.Add(_mapper.MapToCartItemDto(cartItem));
            }
            return result;
        }

        public async Task<CommandResponse<bool>> RemoveCartItem(RemoveCartItemRequestModel requestModel)
        {
            var cartItem = await _dbContext.CartItems.FirstOrDefaultAsync(x => x.ProductId == requestModel.ProductId && x.UserId == requestModel.UserId);
            if (cartItem == null) return CommandResponse<bool>.Success(false);
            _dbContext.CartItems.Remove(cartItem);
            var result = await _dbContext.SaveChangesAsync() > 0;
            return CommandResponse<bool>.Success(result);
        }
    }
}
