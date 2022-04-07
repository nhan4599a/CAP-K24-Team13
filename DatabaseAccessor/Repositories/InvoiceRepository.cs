using DatabaseAccessor.Contexts;
using DatabaseAccessor.Mapping;
using DatabaseAccessor.Models;
using DatabaseAccessor.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using Shared.Extensions;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            var invoices = await _dbContext.InvoiceDetails
                .AsNoTracking()
                .Include(e => e.Invoice)
                .Include(e => e.Product)
                .Where(item => item.Invoice.UserId.ToString() == userId).ToListAsync();
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
                if (actualProduct.IsDisabled)
                    return CommandResponse<bool>.Error($"The product {actualProduct.ProductName} is not available now", null);
                if (!invoices.ContainsKey(cartItem.ShopId))
                {
                    invoices.Add(cartItem.ShopId, new Invoice
                    {
                        UserId = userId,
                        Phone = shippingPhone,
                        FullName = shippingName,
                        ShippingAddress = shippingAddress,
                        Note = orderNotes,
                        ShopId = cartItem.ShopId
                    });
                }
                invoices[cartItem.ShopId].Details.Add(new InvoiceDetail
                {
                    ProductId = cartItem.ProductId,
                    Price = actualProduct.Price * (1 - (actualProduct.Discount / 100.0)),
                    Quantity = cartItem.Quantity
                });
                if (actualProduct.Quantity < cartItem.Quantity)
                    return CommandResponse<bool>.Error($"Sorry, the product {actualProduct.ProductName} is not in sufficient quantity", null);
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
            if (newStatus == InvoiceStatus.Confirmed)
            {
                foreach (var detail in invoice.Details)
                {
                    if (detail.Product.IsDisabled)
                        return CommandResponse<bool>.Error($"The product {detail.Product.ProductName} is not available now", null);
                    if (detail.Product.Quantity < detail.Quantity)
                        return CommandResponse<bool>.Error($"The product {detail.Product.ProductName} is not in sufficient quantity", null);
                }
            }
            await _dbContext.SaveChangesAsync();
            return CommandResponse<bool>.Success(true);
        }

        public async Task<StatisticResult> StatisticAsync(int shopId, StatisticStrategy strategy, 
            StatisticDateRange range)
        {
            var builder = new StatisticResult.Builder(strategy);
            var invoices = GetInvoicesInTime(shopId, range.Range.Start, range.Range.End);
            if (strategy == StatisticStrategy.ByDay)
            {
                var dbStatisticResult = await invoices
                    .GroupBy(invoice => invoice.ChangedDate.Date)
                    .Select(group => new
                    {
                        group.Key,
                        Value = new StatisticResultItem
                        {
                            Income = group.Where(e => e.NewStatus == InvoiceStatus.Succeed)
                                        .SelectMany(e => e.Invoice.Details).Sum(detail => detail.Price * detail.Quantity),
                            Data = new StatisticResultItemData
                            {
                                NewInvoiceCount = group.Count(e => e.NewStatus == InvoiceStatus.New),
                                SucceedInvoiceCount = group.Count(e => e.NewStatus == InvoiceStatus.Succeed),
                                CanceledInvoiceCount = group.Count(e => e.NewStatus == InvoiceStatus.Canceled)
                            }
                        }
                    }).ToListAsync();
                dbStatisticResult.ForEach(group =>
                {
                    builder.AddItem(group.Key, group.Value);
                });
            }
            else if (strategy == StatisticStrategy.ByMonth)
            {
                var dbStatisticResult = await invoices
                    .GroupBy(invoice => new { invoice.ChangedDate.Month, invoice.ChangedDate.Year })
                    .Select(group => new
                    {
                        group.Key,
                        Value = new StatisticResultItem
                        {
                            Income = group.Where(e => e.NewStatus == InvoiceStatus.Succeed)
                                        .SelectMany(e => e.Invoice.Details).Sum(detail => detail.Price * detail.Quantity),
                            Data = new StatisticResultItemData
                            {
                                NewInvoiceCount = group.Count(e => e.NewStatus == InvoiceStatus.New),
                                SucceedInvoiceCount = group.Count(e => e.NewStatus == InvoiceStatus.Succeed),
                                CanceledInvoiceCount = group.Count(e => e.NewStatus == InvoiceStatus.Canceled)
                            }
                        }
                    }).ToListAsync();
                dbStatisticResult.ForEach(group =>
                {
                    builder.AddItem(group.Key.Month, group.Key.Year, group.Value);
                });
            }
            else if (strategy == StatisticStrategy.ByQuarter)
            {
                var dbStatisticResult = await invoices
                    .GroupBy(invoice => new { Quarter = Math.Ceiling(invoice.ChangedDate.Month / 3d), invoice.ChangedDate.Year })
                    .Select(group => new
                    {
                        group.Key,
                        Value = new StatisticResultItem
                        {
                            Income = group.Where(e => e.NewStatus == InvoiceStatus.Succeed)
                                        .SelectMany(e => e.Invoice.Details).Sum(detail => detail.Price * detail.Quantity),
                            Data = new StatisticResultItemData
                            {
                                NewInvoiceCount = group.Count(e => e.NewStatus == InvoiceStatus.New),
                                SucceedInvoiceCount = group.Count(e => e.NewStatus == InvoiceStatus.Succeed),
                                CanceledInvoiceCount = group.Count(e => e.NewStatus == InvoiceStatus.Canceled)
                            }
                        }
                    }).ToListAsync();
                dbStatisticResult.ForEach(group =>
                {
                    builder.AddItem((int)group.Key.Quarter, group.Key.Year, group.Value);
                });
            }
            else
            {
                var dbStatisticResult = await invoices
                                    .GroupBy(invoice => invoice.ChangedDate.Year)
                                    .Select(group => new
                                    {
                                        group.Key,
                                        Value = new StatisticResultItem
                                        {
                                            Income = group.Where(e => e.NewStatus == InvoiceStatus.Succeed)
                                                        .SelectMany(e => e.Invoice.Details).Sum(detail => detail.Price * detail.Quantity),
                                            Data = new StatisticResultItemData
                                            {
                                                NewInvoiceCount = group.Count(e => e.NewStatus == InvoiceStatus.New),
                                                SucceedInvoiceCount = group.Count(e => e.NewStatus == InvoiceStatus.Succeed),
                                                CanceledInvoiceCount = group.Count(e => e.NewStatus == InvoiceStatus.Canceled)
                                            }
                                        }
                                    }).ToListAsync();
                dbStatisticResult.ForEach(group =>
                {
                    builder.AddItem(group.Key, group.Value);
                });
            }
            return builder.Build(range);
        }
        
        public async Task<CommandResponse<PaginatedList<InvoiceDTO>>> FindInvoicesAsync(int shopId, string key,
            string value, PaginationInfo paginationInfo)
        {
            var result = _dbContext.Invoices.AsNoTracking().Include(e => e.Details).Include(e => e.Report)
                .AsSplitQuery()
                .Where(invoice => invoice.ShopId == shopId);
            if (!string.IsNullOrWhiteSpace(key))
            {
                var field = typeof(Invoice).GetProperty(key);
                if (field == null)
                    return CommandResponse<PaginatedList<InvoiceDTO>>.Error("Key not existed. Is it a typo?", null);
                if (field.PropertyType == typeof(string)
                        || field.PropertyType.IsEnum
                        || field.PropertyType.IsNumberType()
                        || field.PropertyType.FullName == "System.DateTime")
                {
                    try
                    {
                        if (field.PropertyType.IsNumberType())
                        {
                            object numberValue = field.PropertyType
                                .GetMethod("Parse", new[] { typeof(string) }).Invoke(null, new[] { value });
                            result = result.Where(key, Operator.Equal, numberValue, field.PropertyType);
                        }
                        else if (field.PropertyType.FullName == "System.DateTime")
                        {
                            if (DateTimeExtension.TryParseExact(value, "dd/MM/yyyy", out DateTime dateTime))
                            {
                                result = result.Where($"{key}.Date", Operator.Equal, dateTime.Date, field.PropertyType);
                            }
                            else if (DateTimeExtension.TryParseExact(value, "M/yyyy", out dateTime))
                            {
                                result = result.Where($"{key}.Month", Operator.Equal, dateTime.Month, typeof(int))
                                    .Where($"{key}.Year", Operator.Equal, dateTime.Year, typeof(int));
                            }
                            else if (DateTimeExtension.TryParseExact(value, "yyyy", out dateTime))
                            {
                                result = result.Where($"{key}.Year", Operator.Equal, dateTime.Year, typeof(int));
                            }
                            else
                            {
                                return CommandResponse<PaginatedList<InvoiceDTO>>
                                    .Error("Provided datetime value is not supported", null);
                            }
                        }
                        else if (field.PropertyType.IsEnum)
                        {
                            var enumObj = Convert.ChangeType(Regex.IsMatch(value, @"^\d+$")
                                ? Enum.ToObject(field.PropertyType, byte.Parse(value))
                                : Enum.Parse(field.PropertyType, value, true), field.PropertyType);
                            result = result.Where(key, Operator.Equal, enumObj, field.PropertyType);
                        }
                        else
                            result = result.Where<Invoice, string>(key, "Contains", value);
                    }
                    catch (Exception e)
                    {
                        return CommandResponse<PaginatedList<InvoiceDTO>>.Error(e.Message, e);
                    }
                }
                else
                {
                    return CommandResponse<PaginatedList<InvoiceDTO>>.Error(
                        $"Provided type {field.PropertyType.FullName} is not supported", null);
                }
            }
            var returnResult = await result
                .Select(invoice => _mapper.MapToInvoiceDTO(invoice))
                .PaginateAsync(paginationInfo.PageNumber, paginationInfo.PageSize);
            return CommandResponse<PaginatedList<InvoiceDTO>>.Success(returnResult);
        }

        private IQueryable<InvoiceStatusChangedHistory> GetInvoicesInTime(int shopId, DateTime startDate, DateTime endDate)
        {
            return _dbContext.InvoiceStatusChangedHistories
                    .AsNoTracking()
                    .Where(history => history.Invoice.ShopId == shopId)
                    .Where(history => history.ChangedDate.Date >= startDate && history.ChangedDate.Date <= endDate);
        }

        public async Task<InvoiceDetailDTO> GetInvoiceDetailAsync(string invoiceCode)
        {
            var result = await _dbContext.Invoices
                .AsNoTracking()
                .Include(e => e.Report)
                .Include(e => e.Details)
                .ThenInclude(e => e.Product)
                .AsSplitQuery()
                .FirstOrDefaultAsync(invoice => invoice.InvoiceCode == invoiceCode);

            return _mapper.MapToInvoiceDetailDTO(result);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}