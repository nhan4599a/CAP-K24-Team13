using DatabaseAccessor.Contexts;
using DatabaseAccessor.Mapping;
using DatabaseAccessor.Models;
using DatabaseAccessor.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.DTOs;
using Shared.RequestModels;
using System;
using System.Collections.Generic;
using System.IO;
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
            var cartItem =
                cart?.Details?.FirstOrDefault(c => c.ProductId.ToString() == requestModel.ProductId);
            if (cartItem != null)
            {
                return CommandResponse<bool>.Error("Product is already existed cart", null);
            }
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = Guid.Parse(requestModel.UserId),
                    Details = new List<CartDetail>()
                };
                _dbContext.Carts.Add(cart);
            }
            cart.Details.Add(new CartDetail
            {
                ProductId = Guid.Parse(requestModel.ProductId),
                Quantity = 1
            });
            await _dbContext.SaveChangesAsync();
            return CommandResponse<bool>.Success(true);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<CommandResponse<bool>> EditQuantityAsync(
            AddOrEditQuantityCartItemRequestModel requestModel)
        {
            var cartItem = await _dbContext.CartDetails
                .FirstOrDefaultAsync(cartItem => cartItem.Cart.UserId.ToString() == requestModel.UserId
                    && cartItem.ProductId.ToString() == requestModel.ProductId);
            if (cartItem == null)
                return CommandResponse<bool>.Error("Destination cart item does not exsisted", null);
            cartItem.Quantity = requestModel.Quantity;
            await _dbContext.SaveChangesAsync();
            return CommandResponse<bool>.Success(true);
        }

        public async Task<CartDTO> GetCartAsync(string userId = "69")
        {
            var cart = await _dbContext.Carts.AsNoTracking()
                .FirstOrDefaultAsync(cartItem => cartItem.UserId.ToString() == userId);
            return null;
        }

        public async Task<CommandResponse<bool>> RemoveCartItemAsync(RemoveCartItemRequestModel requestModel)
        {
            var cart = 
                await _dbContext.Carts.FirstOrDefaultAsync(cart => cart.UserId.ToString() == requestModel.UserId);
            if (cart == null)
                return CommandResponse<bool>.Error("User does not have cart", null);
            var cartItem = cart.Details.FirstOrDefault(item => item.ProductId.ToString() == requestModel.ProductId);
            if (cartItem == null) return CommandResponse<bool>.Error("Can't find cart item", null);
            cart.Details.Remove(cartItem);
            await _dbContext.SaveChangesAsync();
            return CommandResponse<bool>.Success(true);
        }
    }
}
