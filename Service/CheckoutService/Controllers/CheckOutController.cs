using CheckoutService.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Models;
using Shared.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/checkout")]
    public class CheckOutController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CheckOutController(IMediator mediator, ILoggerFactory loggerFactory)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ApiResult> CheckOut([FromForm(Name = "requestModel")] CheckOutRequestModel requestModel)
        {
            Guid userId = Guid.Empty;
            var productIds = new List<Guid>();
            var shippingAddress = requestModel.ShippingAddress;
            System.IO.File.AppendAllText("/home/ec2-user/products.txt", "Number of products: " + requestModel.ProductIds.Count + Environment.NewLine);
            foreach (var productId in requestModel.ProductIds)
            {
                System.IO.File.AppendAllText("/home/ec2-user/products.txt", "Product Id: " + productId + Environment.NewLine);
            }
            try
            {
                userId = Guid.Parse(requestModel.UserId);
                productIds = requestModel.ProductIds.Select(id => Guid.Parse(id)).ToList();
            } catch (Exception e)
            {
                return ApiResult.CreateErrorResult(400, e.Message);
            }
            var result = await _mediator.Send(new CheckOutCommand(userId, productIds, requestModel.ShippingName,
                requestModel.ShippingPhone, shippingAddress, requestModel.OrderNotes));
            if (result.IsSuccess)
                return ApiResult.SucceedResult;
            return ApiResult.CreateErrorResult(404, result.ErrorMessage);
        }
    }
}
