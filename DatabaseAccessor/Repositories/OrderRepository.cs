using DatabaseAccessor.Contexts;
using DatabaseAccessor.Models;
using DatabaseAccessor.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.DTOs;
using Shared.Models;
using Shared.RequestModels;
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


        public async Task<CommandResponse<List<InvoiceDTO>>> GetOrderListAsync(GetInvoiceListRequestModel requestModel)
        {
            var invoices = new List<Invoice>();
            if (requestModel != null || requestModel.InvoiceStatus != null)
            {
                invoices = await _dbContext.Invoices.Where(x => x.Status == requestModel.InvoiceStatus).ToListAsync();
            }
            else
            {
                invoices = await _dbContext.Invoices.ToListAsync();

            }

            if (invoices.Count == 0)
            {
                return null;
            }

            var result = new List<InvoiceDTO>();

            foreach (var invoice in invoices)
            {
                var invoiceDto = new InvoiceDTO
                {
                    Status = invoice.Status,
                    Created = invoice.Created,
                    FullName = invoice.FullName,
                    Id = invoice.Id,
                    InvoiceCode = invoice.InvoiceCode,
                    Note = invoice.Note,
                    Phone = invoice.Phone,
                    ShippingAddress = invoice.ShippingAddress,
                    ShopId = invoice.ShopId,
                    UserId = invoice.UserId
                };
                result.Add(invoiceDto);
            }
            return CommandResponse<List<InvoiceDTO>>.Success(result);
        }

        public async Task<CommandResponse<InvoiceDTO>> GetOrderDetailAsync(int invoiceId)
        {
            var invoice = await _dbContext.Invoices.FirstOrDefaultAsync(x => x.Id == invoiceId);
            if (invoice == null) return null;

            var invoiceDto = new InvoiceDTO
            {
                Status = invoice.Status,
                Created = invoice.Created,
                FullName = invoice.FullName,
                Id = invoice.Id,
                InvoiceCode = invoice.InvoiceCode,
                Note = invoice.Note,
                Phone = invoice.Phone,
                ShippingAddress = invoice.ShippingAddress,
                ShopId = invoice.ShopId,
                UserId = invoice.UserId
            };

            var invoiceDetails = await _dbContext.InvoiceDetails.Where(x => x.InvoiceId == invoiceId).ToListAsync();
            if (invoiceDetails.Count > 0)
            {
                foreach (var invoiceDetail in invoiceDetails)
                {
                    var invoiceDetailDto = new InvoiceDetailDTO
                    {
                        InvoiceId = invoiceId,
                        Id = invoiceDetail.Id,
                        Price = invoiceDetail.Price,
                        ProductId = invoiceDetail.ProductId,
                        Quantity = invoiceDetail.Quantity
                    };
                    invoiceDto.Details.Add(invoiceDetailDto);
                }
            }
            return CommandResponse<InvoiceDTO>.Success(invoiceDto);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
