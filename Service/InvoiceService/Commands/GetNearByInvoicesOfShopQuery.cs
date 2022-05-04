using MediatR;
using Shared.DTOs;
using System.Collections.Generic;

namespace InvoiceService.Commands
{
    public class GetNearByInvoicesOfShopQuery : IRequest<List<InvoiceDTO>>
    {
        public int ShopId { get; set; }
    }
}
