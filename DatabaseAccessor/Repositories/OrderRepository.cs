﻿using DatabaseAccessor.Contexts;
using DatabaseAccessor.Models;
using DatabaseAccessor.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<CommandResponse<bool>> AddOrderAsync(Guid userId, List<Guid> productIds, string shippingName,
            string shippingPhone, string shippingAddress, string orderNotes)
        {
            var cart = await _dbContext.Carts.FirstOrDefaultAsync(cart => cart.UserId == userId);
            if (cart == null)
                return CommandResponse<bool>.Error("Cart not found!", null);
            Dictionary<int, Invoice> invoices = new();
            var products = cart.Details.Where(item => productIds.Contains(item.ProductId)).ToList();
            foreach (var cartItem in products)
            {
                var actualProduct = await _dbContext.ShopProducts.FindAsync(cartItem.ProductId);
                if (!invoices.ContainsKey(cartItem.ShopId))
                {
                    invoices.Add(cartItem.ShopId, new Invoice
                    {
                        UserId = userId,
                        Phone = shippingPhone,
                        FullName = shippingName,
                        ShippingAddress = shippingAddress,
                        Note = orderNotes
                    });
                }
                invoices[cartItem.ShopId].Details.Add(new InvoiceDetail
                {
                    ProductId = cartItem.ProductId,
                    Price = actualProduct.Price,
                    Quantity = cartItem.Quantity
                });
                cart.Details.Remove(cartItem);
            }
            _dbContext.Invoices.AddRange(invoices.Values);
            await _dbContext.SaveChangesAsync();
            return CommandResponse<bool>.Success(true);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}