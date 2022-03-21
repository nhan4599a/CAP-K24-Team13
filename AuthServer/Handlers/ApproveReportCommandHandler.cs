using AuthServer.Commands;
using DatabaseAccessor.Models;
using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using Shared.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AuthServer.Handlers
{
    public class ApproveReportCommandHandler : IRequestHandler<ApproveReportCommand, CommandResponse<(User, AccountPunishmentBehavior)>>,
        IDisposable
    {
        private readonly IReportRepository _repository;

        public ApproveReportCommandHandler(IReportRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<(User, AccountPunishmentBehavior)>> Handle(ApproveReportCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.ApproveReportAsync(request.ReportId);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
