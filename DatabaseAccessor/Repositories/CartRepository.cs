using DatabaseAccessor.Contexts;
using DatabaseAccessor.Mapping;
using DatabaseAccessor.Models;
using DatabaseAccessor.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.DTOs;
using Shared.RequestModels;
using System;
using System.Collections.Generic;
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

        public async Task<CommandResponse<bool>> AddProductToCartAsync(AddOrEditQuantityCartItemRequestModel requestModel)
        {
            var user = await _dbContext.Users.FindAsync(Guid.Parse(requestModel.UserId));
            var product = await _dbContext.ShopProducts.FindAsync(Guid.Parse(requestModel.ProductId));
            if (user == null)
            {
                return CommandResponse<bool>.Error("UserId is incorrect", null);
            }
            if (product == null)
            {
                return CommandResponse<bool>.Error("ProductId is incorrect", null);
            }
            var cart = await _dbContext.Carts.
                FirstOrDefaultAsync(c => c.UserId.ToString() == requestModel.UserId);
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = Guid.Parse(requestModel.UserId),
                };
                _dbContext.Carts.Add(cart);
            }
            var cartDetail = cart.Details
                .FirstOrDefault(item => item.ProductId == Guid.Parse(requestModel.ProductId));
            if (cartDetail == null)
                cart.Details.Add(new CartDetail
                {
                    ProductId = Guid.Parse(requestModel.ProductId),
                    Quantity = requestModel.Quantity,
                });
            else
                cartDetail.Quantity += requestModel.Quantity;
            await _dbContext.SaveChangesAsync();
            return CommandResponse<bool>.Success(true);
        }

        public async Task<CommandResponse<bool>> EditQuantityAsync(
            AddOrEditQuantityCartItemRequestModel requestModel)
        {
            var cart = await _dbContext.Carts
                .FirstOrDefaultAsync(cart => cart.UserId == Guid.Parse(requestModel.UserId));
            if (cart == null)
                return CommandResponse<bool>.Error("User does not have cart", null);
            var cartItem = cart.Details
                .FirstOrDefault(item => item.ProductId == Guid.Parse(requestModel.ProductId));
            if (cartItem == null)
                return CommandResponse<bool>.Error("Destination cart item does not exsisted", null);
            cartItem.Quantity = requestModel.Quantity;
            await _dbContext.SaveChangesAsync();
            return CommandResponse<bool>.Success(true);
        }

        public async Task<CommandResponse<bool>> RemoveCartItemAsync(RemoveCartItemRequestModel requestModel)
        {
            var cart = await _dbContext.Carts
                .FirstOrDefaultAsync(cart => cart.UserId.ToString() == requestModel.UserId);
            if (cart == null)
                return CommandResponse<bool>.Error("User does not have cart", null);
            var cartItem = cart.Details
                .FirstOrDefault(item => item.ProductId.ToString() == requestModel.ProductId);
            if (cartItem == null) return CommandResponse<bool>.Error("Can't find cart item", null);
            cart.Details.Remove(cartItem);
            await _dbContext.SaveChangesAsync();
            return CommandResponse<bool>.Success(true);
        }

        public async Task<List<CartItemDTO>> GetCartAsync(string userId)
        {
            var cart = await _dbContext.Carts.FirstOrDefaultAsync(cart => cart.UserId.ToString() == userId);
            if (cart == null)
                return new List<CartItemDTO>();
            return cart.Details.Select(item => _mapper.MapToCartItemDTO(item)).ToList();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
