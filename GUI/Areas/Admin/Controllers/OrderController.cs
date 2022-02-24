using GUI.Clients;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GUI.Areas.Admin.Controllers
{
    [Authorize(Roles = "ShopOwner")]
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderClient _orderClient;

        public OrderController(IOrderClient orderClient)
        {
            _orderClient = orderClient;
        }

        public async Task<IActionResult> Index()
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var ordersResponse = await _orderClient.GetNearByOrders(token, 0);
            if (ordersResponse == null || !ordersResponse.IsSuccessStatusCode)
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            return View(ordersResponse.Content.Data);
        }
    }
}
