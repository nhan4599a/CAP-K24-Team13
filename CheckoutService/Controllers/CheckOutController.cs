using CheckoutService.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.RequestModels;

namespace CheckoutService.Controllers
{
    [ApiController]
    [Route("/api/checkout")]
    public class CheckOutController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CheckOutController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ApiResult> CheckOut([FromForm(Name = "requestModel")] CheckOutRequestModel requestModel)
        {
            var userId = Guid.Parse(requestModel.UserId);
            var productIds = requestModel.ProductIds.Select(id => Guid.Parse(id)).ToList();
            var shippingAddress = requestModel.ShippingAddress;
            var result = await _mediator.Send(new CheckOutCommand(userId, productIds, requestModel.ShippingName,
                requestModel.ShippingPhone, shippingAddress, requestModel.OrderNotes));
            if (result.IsSuccess)
                return ApiResult.SucceedResult;
            return ApiResult.CreateErrorResult(404, result.ErrorMessage);
        }
    }
}
