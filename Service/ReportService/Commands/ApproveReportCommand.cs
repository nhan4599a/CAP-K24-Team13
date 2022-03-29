using MediatR;
using Shared.Models;

namespace ReportService.Commands
{
    public class ApproveReportCommand : IRequest<CommandResponse<(string, AccountPunishmentBehavior)>>
    {
        public int ReportId { get; set; }
    }
}
