using AuthServer.Commands;
using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using Shared.DTOs;
using Shared.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AuthServer.Handlers
{
    public class GetAllReportsQueryHandler : IRequestHandler<GetAllReportsQuery, PaginatedList<ReportDTO>>, IDisposable
    {
        private readonly IReportRepository _repository;

        public GetAllReportsQueryHandler(IReportRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedList<ReportDTO>> Handle(GetAllReportsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllReportsAsync(request.PaginationInfo);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
