using DatabaseAccessor.Contexts;
using DatabaseAccessor.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ReportRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SaleReportDTO> GetSaleReportByDate(DateTime date, int shopId)
        {
            var invoices = await _dbContext.Invoices.Where(x => x.CreatedAt.Date == date.Date && x.ShopId == shopId).ToListAsync();

            var result = new SaleReportDTO();

            foreach (var invoice in invoices)
            {
                var productInvoice = await _dbContext.InvoiceDetails.Where(x => x.InvoiceId == invoice.Id).ToListAsync();

                foreach (var item in productInvoice)
                {
                    var productReport = new ProductReportDTO()
                    {

                        Date = date.Date,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        ProductId = item.ProductId,
                        ProductName = (await _dbContext.ShopProducts.FirstOrDefaultAsync(x => x.Id == item.ProductId)).ProductName,
                        Subtotal = item.Price * item.Quantity
                    };
                    result.ProductsList.Add(productReport);
                    result.Total += productReport.Subtotal;
                }
            }
            return result;
        }

        public async Task<SaleReportDTO> GetSaleReportByMonth(DateTime date, int shopId)
        {
            var invoices = await _dbContext.Invoices.Where(x =>
                                                              x.CreatedAt.Date.Year == date.Date.Year
                                                            && x.CreatedAt.Month == date.Month
                                                            && x.ShopId == shopId).ToListAsync();

            var result = new SaleReportDTO();

            foreach (var invoice in invoices)
            {
                var productInvoice = await _dbContext.InvoiceDetails.Where(x => x.InvoiceId == invoice.Id).ToListAsync();

                foreach (var item in productInvoice)
                {
                    var productReport = new ProductReportDTO()
                    {

                        Date = date.Date,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        ProductId = item.ProductId,
                        ProductName = (await _dbContext.ShopProducts.FirstOrDefaultAsync(x => x.Id == item.ProductId)).ProductName,
                        Subtotal = item.Price * item.Quantity
                    };
                    result.ProductsList.Add(productReport);
                    result.Total += productReport.Subtotal;
                }
            }
            return result;
        }
    }
}
