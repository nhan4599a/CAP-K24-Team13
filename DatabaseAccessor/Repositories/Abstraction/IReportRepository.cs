using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories.Abstraction
{
    public interface IReportRepository
    {
        Task<SaleReportDTO> GetSaleReportByDate(DateTime date, int shopId);

        Task<SaleReportDTO> GetSaleReportByMonth(DateTime date, int shopId);
    }
}
