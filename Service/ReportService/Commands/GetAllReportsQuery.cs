using MediatR;
using Shared.DTOs;
using Shared.Models;

namespace ReportService.Commands
{
    public class GetAllReportsQuery : IRequest<PaginatedList<ReportDTO>>
    {
        public PaginationInfo PaginationInfo { get; set; } = PaginationInfo.Default;
    }
}
