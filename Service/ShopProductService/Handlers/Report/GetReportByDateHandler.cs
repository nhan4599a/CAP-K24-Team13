using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using Shared.DTOs;
using ShopProductService.Commands.Report;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Report
{
    public class GetReportByDateHandler : IRequestHandler<GetReportByDateQuery, SaleReportDTO>
    {
        private readonly IReportRepository _reportRepository;

        public GetReportByDateHandler(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<SaleReportDTO> Handle(GetReportByDateQuery request, CancellationToken cancellationToken)
        {
            var result = await _reportRepository.GetSaleReportByDate(date: request.Date, shopId: request.ShopId);

            return result;
        }
    }
}
