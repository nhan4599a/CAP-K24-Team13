using InvoiceService.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Commands;
using Shared.DTOs;
using Shared.Models;
using Shared.RequestModels;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/order")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ApiResult<InvoiceDTO>> GetOrderDetail(int id)
        {
            var result = await _mediator.Send(new GetOrderDetailQuery { OrderId = id });
            if (result == null)
                return new ApiResult<InvoiceDTO> { ResponseCode = 200 };
            return new ApiResult<InvoiceDTO> { Data = result.Response, ResponseCode = 200 };
        }

        [Route("GetListOrder")]
        [HttpPost]
        public async Task<ApiResult<List<InvoiceDTO>>> GetListOrder(GetInvoiceListRequestModel requestModel)
        {
            var result = await _mediator.Send(new GetOrderListQuery { RequestModel = requestModel });
            if (result == null) return new ApiResult<List<InvoiceDTO>>()
            {
                ResponseCode = 200,
            };

            return new ApiResult<List<InvoiceDTO>>
            {
                Data = result.Response,
                ResponseCode = 200
            };
        }
    }
}
