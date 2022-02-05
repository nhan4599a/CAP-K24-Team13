using GUI.Clients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GUI.Areas.User.Controllers
{
    public class UserAccountController : Controller
    {
        private readonly IOrderHistoryClient _orderHistoryClient;

        public UserAccountController(IOrderHistoryClient orderHistoryClient)
        {
            _orderHistoryClient = orderHistoryClient;
        }

        public async Task<IActionResult> Profile()
        {
            var orderHistoryRespone = await _orderHistoryClient.GetOrderUserHistory("db5ccda5-45b8-416f-6458-08d9e89ecc9b");
            if (!orderHistoryRespone.IsSuccessStatusCode)
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            return View(orderHistoryRespone.Content.Data);
        }
       
    }
}
