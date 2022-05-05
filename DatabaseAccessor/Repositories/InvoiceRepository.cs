using DatabaseAccessor.Contexts;
using DatabaseAccessor.Mapping;
using DatabaseAccessor.Models;
using DatabaseAccessor.Repositories.Abstraction;
using EFCore.BulkExtensions;
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

        public async Task<Dictionary<string, InvoiceWithItemDTO[]>> GetOrderHistoryAsync(string userId)
        {
            var parsedUserId = Guid.Parse(userId);
            var queryResult = await _dbContext.Invoices
                .AsNoTracking()
                .Include(e => e.Details)
                .ThenInclude(e => e.Product)
                .AsSplitQuery()
                .Where(item => item.UserId == parsedUserId)
                .OrderByDescending(e => e.CreatedAt)
                .Select(item => _mapper.MapToInvoiceWithItemDTO(item))
                .ToListAsync();

            return queryResult.GroupBy(invoice => invoice.RefId)
                .ToDictionary(group => group.Key, group => group.ToArray());
        }

        public async Task<List<InvoiceDTO>> GetOrdersOfShopWithInTimeAsync(int shopId, DateOnly startDate, DateOnly endDate)
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
                .Select(invoice => _mapper.MapToInvoiceDTO(invoice))
                .ToListAsync();
        }

        public async Task<CommandResponse<string>> AddOrderAsync(Guid userId, List<Guid> productIds, string shippingName,
            string shippingPhone, string shippingAddress, string orderNotes, PaymentMethod paymentMethod)
        {
            var cart = await _dbContext.Carts.FirstOrDefaultAsync(cart => cart.UserId == userId);
            if (cart == null)
                return CommandResponse<string>.Error("Cart not found!", null);
            Dictionary<int, Invoice> invoices = new();
            var products = cart.Details.Where(item => productIds.Contains(item.ProductId)).ToList();
            var refId = Guid.NewGuid().ToString();
            foreach (var cartItem in products)
            {
                var actualProduct = await _dbContext.ShopProducts.FindAsync(cartItem.ProductId);
                if (actualProduct.IsDisabled || !actualProduct.IsVisible)
                    return CommandResponse<string>.Error($"The product {actualProduct.ProductName} is not available now", null);
                if (!invoices.ContainsKey(cartItem.ShopId))
                {
                    invoices.Add(cartItem.ShopId, new Invoice
                    {
                        UserId = userId,
                        Phone = shippingPhone,
                        FullName = shippingName,
                        ShippingAddress = shippingAddress,
                        Note = orderNotes,
                        ShopId = cartItem.ShopId,
                        PaymentMethod = paymentMethod,
                        RefId = refId,
                        IsPaid = paymentMethod == PaymentMethod.CoD
                    });
                }
                invoices[cartItem.ShopId].Details.Add(new InvoiceDetail
                {
                    ProductId = cartItem.ProductId,
                    Price = actualProduct.Price * (1 - (actualProduct.Discount / 100.0)),
                    Quantity = cartItem.Quantity
                });
                if (actualProduct.Quantity < cartItem.Quantity)
                    return CommandResponse<string>.Error($"Sorry, the product {actualProduct.ProductName} is not in sufficient quantity", null);
                cart.Details.Remove(cartItem);
            }
            _dbContext.Invoices.AddRange(invoices.Values);
            await _dbContext.SaveChangesAsync();
            return CommandResponse<string>.Success(refId);
        }

        public async Task<CommandResponse<bool>> ChangeOrderStatusAsync(int invoiceId, InvoiceStatus newStatus)
        {
            var invoice = await _dbContext.Invoices.FindAsync(invoiceId);
            if (invoice == null)
                return CommandResponse<bool>.Error("Order not found", null);

            if (await _dbContext.ShopStatus.AnyAsync(shop => shop.ShopId == invoice.ShopId && shop.IsDisabled))
                return CommandResponse<bool>.Error("Shop is already disabled", null);

            // checking new status is valid
            if (newStatus != InvoiceStatus.Canceled)
            {
                if (invoice.Status >= newStatus)
                    return CommandResponse<bool>.Error($"Cannot change status from {invoice.Status} to {newStatus}", null);

                if (newStatus - invoice.Status > 1)
                    return CommandResponse<bool>.Error($"Cannot change status from {invoice.Status} to {newStatus}", null);

                if (!invoice.IsPaid)
                    return CommandResponse<bool>.Error("Invoice is paid by user", null);
            }
            else
            {
                if (invoice.Status == InvoiceStatus.Succeed)
                    return CommandResponse<bool>.Error($"Cannot change status from {invoice.Status} to {newStatus}", null);
            }

            if (newStatus == InvoiceStatus.Confirmed)
            {
                foreach (var detail in invoice.Details)
                {
                    if (detail.Product.IsDisabled || !detail.Product.IsVisible)
                        return CommandResponse<bool>.Error($"The product {detail.Product.ProductName} is not available now", null);
                    if (detail.Product.Quantity < detail.Quantity)
                        return CommandResponse<bool>.Error($"The product {detail.Product.ProductName} is not in sufficient quantity", null);
                }
            }

            invoice.Status = newStatus;
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
                                        .SelectMany(e => e.Invoice.Details)
                                        .Sum(detail => detail.Price * detail.Quantity),
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
                                        .SelectMany(e => e.Invoice.Details)
                                        .Sum(detail => detail.Price * detail.Quantity),
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
                                        .SelectMany(e => e.Invoice.Details)
                                        .Sum(detail => detail.Price * detail.Quantity),
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
                                                        .SelectMany(e => e.Invoice.Details)
                                                        .Sum(detail => detail.Price * detail.Quantity),
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

        public async Task<CommandResponse<PaginatedList<InvoiceWithReportDTO>>> FindInvoicesAsync(int shopId, string key,
            string value, PaginationInfo paginationInfo)
        {
            var result = _dbContext.Invoices.AsNoTracking().Include(e => e.Report)
                .Where(invoice => invoice.ShopId == shopId);
            if (!string.IsNullOrWhiteSpace(key))
            {
                var field = typeof(Invoice).GetProperty(key);
                if (field == null)
                    return CommandResponse<PaginatedList<InvoiceWithReportDTO>>.Error("Key not existed. Is it a typo?", null);
                if (field.PropertyType == typeof(string)
                        || field.PropertyType.IsEnum
                        || field.PropertyType.IsNumberType()
                        || field.PropertyType.FullName == typeof(DateTime).FullName)
                {
                    try
                    {
                        if (field.PropertyType.IsNumberType())
                        {
                            object numberValue = field.PropertyType
                                .GetMethod("Parse", new[] { typeof(string) }).Invoke(null, new[] { value });
                            result = result.Where(key, Operator.Equal, numberValue, field.PropertyType);
                        }
                        else if (field.PropertyType.FullName == typeof(DateTime).FullName)
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
                                return CommandResponse<PaginatedList<InvoiceWithReportDTO>>
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
                        return CommandResponse<PaginatedList<InvoiceWithReportDTO>>.Error(e.Message, e);
                    }
                }
                else
                {
                    return CommandResponse<PaginatedList<InvoiceWithReportDTO>>.Error(
                        $"Provided type {field.PropertyType.FullName} is not supported", null);
                }
            }
            var returnResult = await result
                .OrderByDescending(invoice => invoice.CreatedAt)
                .Select(invoice => _mapper.MapToInvoiceWithReportDTO(invoice))
                .PaginateAsync(paginationInfo.PageNumber, paginationInfo.PageSize);
            return CommandResponse<PaginatedList<InvoiceWithReportDTO>>.Success(returnResult);
        }

        private IQueryable<InvoiceStatusChangedHistory> GetInvoicesInTime(int shopId, DateTime startDate, DateTime endDate)
        {
            return _dbContext.InvoiceStatusChangedHistories
                    .AsNoTracking()
                    .Where(history => history.Invoice.ShopId == shopId)
                    .Where(history => history.ChangedDate.Date >= startDate && history.ChangedDate.Date <= endDate);
        }

        public async Task<InvoiceWithItemDTO> GetInvoiceDetailAsync(string invoiceCode)
        {
            var result = await _dbContext.Invoices
                .AsNoTracking()
                .Include(e => e.Details)
                .ThenInclude(e => e.Product)
                .AsSplitQuery()
                .FirstOrDefaultAsync(invoice => invoice.InvoiceCode == invoiceCode);

            return _mapper.MapToInvoiceWithItemDTO(result);
        }

        public async Task<InvoiceWithItemDTO[]> GetInvoiceDetailByRefIdAsync(string refId)
        {
            return await _dbContext.Invoices
                .AsNoTracking()
                .Include(e => e.Details)
                .ThenInclude(e => e.Product)
                .AsSplitQuery()
                .Where(invoice => invoice.RefId == refId)
                .Select(invoice => _mapper.MapToInvoiceWithItemDTO(invoice))
                .ToArrayAsync();
        }

        public async Task MakeAsPaidAsync(string refId)
        {
            await _dbContext.Invoices.Where(item => item.RefId == refId)
                .BatchUpdateAsync(new Invoice
                {
                    IsPaid = true
                }, new List<string> { "IsPaid" });
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}