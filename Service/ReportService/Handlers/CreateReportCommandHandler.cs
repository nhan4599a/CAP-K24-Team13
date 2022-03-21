using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using ReportService.Commands;
using Shared.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ReportService.Handlers
{
    public class CreateReportCommandHandler : IRequestHandler<CreateReportCommand, CommandResponse<int>>, IDisposable
    {
        private readonly IReportRepository _repository;

        public CreateReportCommandHandler(IReportRepository repository)
        {
            _repository = repository;
        }

        public Task<CommandResponse<int>> Handle(CreateReportCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
