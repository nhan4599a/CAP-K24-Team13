using DatabaseAccessor.Contexts;
using DatabaseAccessor.Mapping;
using DatabaseAccessor.Models;
using DatabaseAccessor.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using Shared.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly Mapper _mapper;

        public ReportRepository(ApplicationDbContext dbContext, Mapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<PaginatedList<ReportDTO>> GetAllReportsAsync(PaginationInfo paginationInfo)
        {
            return await _dbContext.Reports
                .AsNoTracking()
                .Include(e => e.Reporter)
                .Include(e => e.AffectedUser)
                .AsSplitQuery()
                .Select(report => _mapper.MapToReportDTO(report))
                .PaginateAsync(paginationInfo.PageNumber, paginationInfo.PageSize);
        }

        public async Task<CommandResponse<int>> CreateReportAsync(int invoiceId, Guid reporter)
        {
            var report = await _dbContext.Reports.FirstOrDefaultAsync(e => e.AffectedInvoiceId == invoiceId);
            if (report != null)
            {
                return CommandResponse<int>.Error("Report is already created", null);
            }
            var affectedInvoice = await _dbContext.Invoices.FindAsync(invoiceId);
            if (affectedInvoice == null)
            {
                return CommandResponse<int>.Error("Invoice does not existed", null);
            }
            report = new Report
            {
                AffectedInvoiceId = invoiceId,
                ReporterId = reporter,
                AffectedUserId = affectedInvoice.UserId
            };
            _dbContext.Reports.Add(report);
            await _dbContext.SaveChangesAsync();
            return CommandResponse<int>.Success(report.Id);
        }
        public Task<PaginatedList<ReportDTO>> GetReports(PaginationInfo paginationInfo)
        {
            return _dbContext.Reports
                .AsNoTracking()
                .Include(e => e.Reporter)
                .Include(e => e.AffectedUser)
                .AsSplitQuery()
                .OrderBy(report => report.CreatedAt)
                .Select(report => _mapper.MapToReportDTO(report))
                .PaginateAsync(paginationInfo.PageNumber, paginationInfo.PageSize);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
