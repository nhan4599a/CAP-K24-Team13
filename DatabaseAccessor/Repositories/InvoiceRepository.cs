﻿using DatabaseAccessor.Contexts;
using DatabaseAccessor.Mapping;
using DatabaseAccessor.Models;
using DatabaseAccessor.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.DTOs;
using Shared.Linq;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly Mapper _mapper;

        public InvoiceRepository(ApplicationDbContext context, Mapper mapper)
        {
            _dbContext = context;
            _mapper = mapper ?? Mapper.GetInstance();
        }

        public async Task<List<OrderItemDTO>> GetOrderHistoryAsync(string userId)
        {
            var invoices = await _dbContext.InvoiceDetails.Where(item => item.Invoice.UserId.ToString() == userId).ToListAsync();
            return invoices.Select(item => _mapper.MapToOrderItemDTO(item)).ToList();
        }

        public async Task<List<OrderDTO>> GetOrdersOfShopAsync(int shopId)
        {
            return await _dbContext.Invoices
                .AsNoTracking()
                .Where(item => item.ShopId == shopId).Select(invoice => _mapper.MapToOrderDTO(invoice)).ToListAsync();
        }

        public async Task<List<OrderDTO>> GetOrdersOfShopWithInTimeAsync(int shopId, DateOnly startDate, DateOnly endDate)
        {
            var currentDate = DateOnly.FromDateTime(DateTime.Now);
            if (startDate > endDate)
                throw new InvalidOperationException($"{nameof(startDate)} must be less than or equal to {nameof(endDate)}");
            if (endDate > currentDate)
                throw new InvalidOperationException($"{nameof(endDate)} must be less than {currentDate:dd/MM/yyyy}");
            return await _dbContext.Invoices
                .AsNoTracking()
                .Where(item => item.ShopId == shopId && item.CreatedAt >= startDate.ToDateTime(TimeOnly.MinValue)
                    && item.CreatedAt <= endDate.ToDateTime(TimeOnly.MaxValue))
                .Select(invoice => _mapper.MapToOrderDTO(invoice))
                .ToListAsync();
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

        public async Task<CommandResponse<bool>> ChangeOrderStatusAsync(int invoiceId, InvoiceStatus newStatus)
        {
            var invoice = await _dbContext.Invoices.FindAsync(invoiceId);
            if (invoice == null)
                return CommandResponse<bool>.Error("Order not found", null);

            // checking new status is valid
            if (newStatus != InvoiceStatus.Canceled)
            {
                if (invoice.Status >= newStatus)
                    return CommandResponse<bool>.Error($"Cannot change status from {invoice.Status} to {newStatus}", null);

                if (newStatus - invoice.Status > 1)
                    return CommandResponse<bool>.Error($"Cannot change status from {invoice.Status} to {newStatus}", null);
            }
            else
            {
                if (invoice.Status == InvoiceStatus.Succeed)
                    return CommandResponse<bool>.Error($"Cannot change status from {invoice.Status} to {newStatus}", null);
            }

            invoice.Status = newStatus;
            await _dbContext.SaveChangesAsync();
            return CommandResponse<bool>.Success(true);
        }

        public async Task<StatisticResult> StatisticAsync(StatisticStrategy strategy)
        {
            var builder = new StatisticResult.Builder(strategy);
            if (strategy == StatisticStrategy.ByDay)
            {
                var dbStatisticResult = await _dbContext.Invoices
                    .AsNoTracking()
                    .GroupBy(invoice => invoice.CreatedAt.Date)
                    .Where(group => group.Key.Year == DateTime.Now.Year && group.Key.Month == DateTime.Now.Month)
                    .Select(group => new
                    {
                        group.Key,
                        Value = new StatisticResultItem
                        {
                            EstimatedIncome = group.SelectMany(e => e.Details).Sum(detail => detail.Price * detail.Quantity),
                            ActualIncome = group.Where(e => e.Status == InvoiceStatus.Succeed)
                                    .SelectMany(e => e.Details).Sum(detail => detail.Price * detail.Quantity),
                            Data = new StatisticResultItemData
                            {
                                Total = group.Count(),
                                NewInvoiceCount = group.Count(invoice => invoice.Status == InvoiceStatus.New),
                                SucceedInvoiceCount = group.Count(invoice => invoice.Status == InvoiceStatus.Succeed),
                                CanceledInvoiceCount = group.Count(invoice => invoice.Status == InvoiceStatus.Canceled)
                            }
                        }
                    }).ToListAsync();
                dbStatisticResult.ForEach(group =>
                {
                    builder.AddItem(group.Key, group.Value);
                });
            }
            else
            {
                var dbStatisticResult = await _dbContext.Invoices
                    .AsNoTracking()
                    .GroupBy(invoice => new { invoice.CreatedAt.Month, invoice.CreatedAt.Year })
                    .Where(group => group.Key.Year == DateTime.Now.Year)
                    .Select(group => new
                    {
                        group.Key,
                        Value = new StatisticResultItem
                        {
                            EstimatedIncome = group.SelectMany(e => e.Details).Sum(detail => detail.Price * detail.Quantity),
                            ActualIncome = group.Where(e => e.Status == InvoiceStatus.Succeed)
                                    .SelectMany(e => e.Details).Sum(detail => detail.Price * detail.Quantity),
                            Data = new StatisticResultItemData
                            {
                                Total = group.Count(),
                                NewInvoiceCount = group.Count(invoice => invoice.Status == InvoiceStatus.New),
                                SucceedInvoiceCount = group.Count(invoice => invoice.Status == InvoiceStatus.Succeed),
                                CanceledInvoiceCount = group.Count(invoice => invoice.Status == InvoiceStatus.Canceled)
                            }
                        }
                    }).ToListAsync();
                dbStatisticResult.ForEach(group =>
                {
                    builder.AddItem(group.Key.Month, group.Key.Year, group.Value);
                });
            }
            return builder.Result;
        }

        public async Task<PaginatedList<InvoiceDTO>> FindInvoicesAsync(string key, object value,
            PaginationInfo paginationInfo)
        {
            var members = typeof(Invoice).GetMember(key);
            if (members.Length == 0)
                throw new ArgumentException("Key not existed. Is it a typo ?");
            if (members[0].GetType() != typeof(string))
                throw new ArgumentException("Currently, only string type is supported");
            return await _dbContext.Invoices
                .Where<Invoice, string>(key, "Contains", value)
                .Select(invoice => _mapper.MapToInvoiceDTO(invoice))
                .PaginateAsync(paginationInfo.PageNumber, paginationInfo.PageSize);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}