using Shared.DTOs;
using Shared.Models;
using System;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories.Abstraction
{
    public interface IReportRepository : IDisposable
    {
        public Task<PaginatedList<ReportDTO>> GetAllReportsAsync(PaginationInfo paginationInfo);

        public Task<CommandResponse<int>> CreateReportAsync(int invoiceId, Guid reporter);

        public Task<PaginatedList<ReportDTO>> GetReports(PaginationInfo paginationInfo);
    }
}
