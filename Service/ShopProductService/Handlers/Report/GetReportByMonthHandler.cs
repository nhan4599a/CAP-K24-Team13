using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using Shared.DTOs;
using ShopProductService.Commands.Report;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Report
{
    public class GetReportByMonthHandler : IRequestHandler<GetReportByMonthQuery, SaleReportDTO>
    {
        private readonly IReportRepository _reportRepository;

        public GetReportByMonthHandler(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<SaleReportDTO> Handle(GetReportByMonthQuery request, CancellationToken cancellationToken)
        {
            var result = await _reportRepository.GetSaleReportByMonth(date: request.Month, shopId: request.ShopId);

            return result;
        }
    }
}
