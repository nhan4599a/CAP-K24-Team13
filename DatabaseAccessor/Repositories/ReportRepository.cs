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
                .OrderBy(report => report.Status)
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
            return CommandResponse<int>.Success(report.Id);
        }

        public async Task<CommandResponse<(User, AccountPunishmentBehavior)>> ApproveReportAsync(int reportId)
        {
            var report = await _dbContext.Reports.FindAsync(reportId);
            if (report == null)
            {
                return CommandResponse<(User, AccountPunishmentBehavior)>.Error("Report doesn't existed", null);
            }
            if (report.Status == ReportStatus.Approved)
            {
                return CommandResponse<(User, AccountPunishmentBehavior)>.Error("Report is already approved", null);
            }
            int reportCount = await _dbContext.Reports.Where(e => e.AffectedUserId == report.AffectedUserId).CountAsync();
            var punishment = reportCount switch
            {
                1 => AccountPunishmentBehavior.SendAlertEmail,
                2 => AccountPunishmentBehavior.LockedOut,
                _ => AccountPunishmentBehavior.LockedOutPermanently
            };
            report.Punishment = punishment;
            report.Status = ReportStatus.Approved;
            await _dbContext.SaveChangesAsync();
            return CommandResponse<(User, AccountPunishmentBehavior)>.Success(new (report.AffectedUser, punishment));
        }

        public Task<PaginatedList<ReportDTO>> GetReports(PaginationInfo paginationInfo)
        {
            return _dbContext.Reports
                .AsNoTracking()
                .Include(e => e.Reporter)
                .Include(e => e.AffectedUser)
                .AsSplitQuery()
                .OrderBy(report => report.CreatedAt)
                .ThenBy(report => report.Status)
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
