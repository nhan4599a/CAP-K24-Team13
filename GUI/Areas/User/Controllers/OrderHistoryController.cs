using GUI.Clients;
using GUI.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GUI.Areas.User.Controllers
{
    [Authorize]
    [Route("/order-history")]
    public class OrderHistoryController : Controller
    {
        private readonly IOrderClient _orderHistoryClient;

        private readonly ILogger<OrderHistoryController> _logger;

        public OrderHistoryController(IOrderClient orderHistoryClient, ILoggerFactory loggerFactory)
        {
            _orderHistoryClient = orderHistoryClient;
            _logger = loggerFactory.CreateLogger<OrderHistoryController>();
        }

        public async Task<IActionResult> Index()
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            _logger.LogInformation("Token: " + token);
            var orderHistoryRespone = await _orderHistoryClient.GetOrderUserHistory(token, User.GetUserId().ToString());
            if (!orderHistoryRespone.IsSuccessStatusCode)
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            return View(orderHistoryRespone.Content.Data);
        }
       
    }
}
