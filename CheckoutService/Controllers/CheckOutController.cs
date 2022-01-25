using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace CheckoutService.Controllers
{
    public class CheckOutController : Controller
    {
        private readonly IMediator _mediator;

        public CheckOutController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public ApiResult CheckOut(string userId, string[] productIds)
        {

        }
    }
}
