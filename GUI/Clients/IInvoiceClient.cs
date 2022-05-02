using Refit;
using Shared.DTOs;
using Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GUI.Clients
{
    public interface IInvoiceClient
    {
        [Get("/orders/user/{userId}")]
        Task<ApiResponse<ApiResult<List<OrderItemDTO>>>> GetOrderUserHistory([Authorize("Bearer")] string token, string userId);

        [Get("/orders/{invoiceCode}")]
        Task<ApiResponse<ApiResult<InvoiceDetailDTO>>> GetOrderDetail([Authorize("Bearer")] string token, string invoiceCode);

        [Get("/orders/ref/{refId}")]
        Task<ApiResponse<ApiResult<InvoiceDetailDTO[]>>> GetOrderDetailByRefId([Authorize("Bearer")] string token, string refId);

        [Post("/orders/paid/{refId}")]
        Task<ApiResponse<ApiResult<bool>>> MakeAsPaid(string refId, [Body] MakeAsPaidRequestModel requestModel);
    }
}
