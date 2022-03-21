using DatabaseAccessor.Models;
using MediatR;
using Shared.Models;

namespace AuthServer.Commands
{
    public class ApproveReportCommand : IRequest<CommandResponse<(User, AccountPunishmentBehavior)>>
    {
        public int ReportId { get; set; }
    }
}
