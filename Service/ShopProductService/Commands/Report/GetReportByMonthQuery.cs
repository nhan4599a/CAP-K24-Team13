using MediatR;
using Shared.DTOs;
using System;

namespace ShopProductService.Commands.Report
{
    public class GetReportByMonthQuery : IRequest<SaleReportDTO>
    {
        public DateTime Month { get; set; }

        public int ShopId { get; set; }
    }
}
