using MediatR;
using Shared;
using Shared.DTOs;
using Shared.RequestModels;

namespace InvoiceService.Commands
{
    public class GetOrderListQuery : IRequest<CommandResponse<List<InvoiceDTO>>>
    {
        public GetInvoiceListRequestModel? RequestModel { get; set; }
    }
}
