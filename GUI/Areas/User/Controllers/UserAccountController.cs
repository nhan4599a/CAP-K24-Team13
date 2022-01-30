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
            var orderHistoryRespone = await _orderHistoryClient.GetOrderUserHistory("B8A936EB-3904-4DBE-D29F-08D9E0150BF3");
            if (!orderHistoryRespone.IsSuccessStatusCode)
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            return View(orderHistoryRespone.Content.Data);
        }
       
    }
}
