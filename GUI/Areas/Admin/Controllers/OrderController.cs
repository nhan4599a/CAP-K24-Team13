using GUI.Clients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GUI.Areas.Admin.Controllers
{
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
            var ordersResponse = await _orderClient.GetNearByOrders(0);
            if (ordersResponse == null || !ordersResponse.IsSuccessStatusCode)
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            return View(ordersResponse.Content.Data);
        }
    }
}
