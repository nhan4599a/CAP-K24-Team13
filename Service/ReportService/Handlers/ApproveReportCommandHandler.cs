using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using ReportService.Commands;
using Shared.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ReportService.Handlers
{
    public class ApproveReportCommandHandler 
        : IRequestHandler<ApproveReportCommand, CommandResponse<(string, AccountPunishmentBehavior)>>,
        IDisposable
    {
        private readonly IReportRepository _repository;

        public ApproveReportCommandHandler(IReportRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<(string, AccountPunishmentBehavior)>> Handle(ApproveReportCommand request,
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
