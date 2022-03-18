using MediatR;
using Shared.DTOs;
using System;

namespace ShopProductService.Commands.Report
{
    public class GetReportByDateQuery : IRequest<SaleReportDTO>
    {
        public DateTime Date { get; set; }
        public int ShopId { get; set; }
    }
}
