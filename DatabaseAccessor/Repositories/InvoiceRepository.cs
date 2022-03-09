using DatabaseAccessor.Contexts;
using DatabaseAccessor.Converters;
using DatabaseAccessor.Mapping;
using DatabaseAccessor.Models;
using DatabaseAccessor.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.DTOs;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly Mapper _mapper;

        public ApplicationDbContext DbContext => _dbContext;

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

        public Task<StatisticResult<Invoice>> StatisticAsync(StatisticStrategy strategy)
        {

            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;
            var invoiceGroups = _dbContext.Invoices
                .AsNoTracking()
                .Include(e => e.Details)
                .Where(invoice => invoice.CreatedAt.Year == currentYear && (
                    strategy != StatisticStrategy.ByDay || invoice.CreatedAt.Month == currentMonth
                ))
                .GroupBy(invoice => new { invoice.Status, invoice.CreatedAt });
            var highestIncome = 0d;
            var lowestIncome = double.MaxValue;
            DateTime? highestDate = null;
            DateTime? lowestDate = null;
            var statisticResultItems =
                new SortedDictionary<StatisticDateResult, StatisticResultItem>(StatisticDateResult.DefaultComparer);
            foreach (var group in invoiceGroups)
            {
                var invoiceList = group.ToList();
                var newInvoiceList = group.Where(invoice => invoice.Status == InvoiceStatus.New).ToList();
                var confirmedInvoiceList = group.Where(invoice => invoice.Status == InvoiceStatus.Succeed).ToList();
                var estimatedIncome = group.Sum(invoice => invoice.Details.Sum(detail => detail.Price));
                var actualIncome = group.Where(invoice => invoice.Status == InvoiceStatus.Succeed)
                        .Sum(invoice => invoice.Details.Sum(detail => detail.Price));
                if (actualIncome > highestIncome)
                {
                    highestIncome = actualIncome;
                    highestDate = group.Key.CreatedAt;
                }
                if (actualIncome < lowestIncome)
                {
                    lowestIncome = actualIncome;
                    lowestDate = group.Key.CreatedAt;
                }
                var statisticDateResult = new StatisticDateResult(strategy, group.Key.CreatedAt.ToDateOnly());
                statisticResultItems.Add(statisticDateResult, new StatisticResultItem
                {
                    Data = new StatisticResultItemData
                    {
                        Total = invoiceList.Count,
                        NewInvoiceCount = newInvoiceList.Count,
                        SucceedInvoiceCount = confirmedInvoiceList.Count,
                        CanceledInvoiceCount = invoiceList.Count(invoice => invoice.Status == InvoiceStatus.Canceled)
                    },
                    EstimatedIncome = estimatedIncome,
                    ActualIncome = actualIncome
                });
            }
            if (strategy == StatisticStrategy.ByDay)
            {
                for (int i = 1; i <= DateTime.Now.Day; i++)
                {
                    var dateOnlyObj = DateOnly.FromDateTime(new DateTime(currentYear, currentMonth, i));
                    var statisticDateResult = new StatisticDateResult(strategy, dateOnlyObj);
                    statisticResultItems.TryAdd(statisticDateResult, new StatisticResultItem());
                }
            }
            else
            {
                for (int i = 1; i <= DateTime.Now.Month; i++)
                {
                    var dateOnlyObj = DateOnly.FromDateTime(new DateTime(currentYear, i, 1));
                    var statisticDateResult = new StatisticDateResult(strategy, dateOnlyObj);
                    statisticResultItems.TryAdd(statisticDateResult, new StatisticResultItem());
                }
            }
            var statisticResult = new StatisticResult<Invoice>(strategy)
            {
                Details = statisticResultItems.ToDictionary(e => e.Key.ToString(), e => e.Value),
                HighestIncome = highestIncome,
                LowestIncome = lowestIncome
            };
            if (highestDate.HasValue)
                statisticResult.HighestDate = new StatisticDateResult(strategy, highestDate.Value.ToDateOnly());
            if (lowestDate.HasValue)
                statisticResult.LowestDate = new StatisticDateResult(strategy, lowestDate.Value.ToDateOnly());
            return Task.FromResult(statisticResult);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

