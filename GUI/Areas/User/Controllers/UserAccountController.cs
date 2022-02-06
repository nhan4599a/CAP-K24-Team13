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
            var orderHistoryRespone = await _orderHistoryClient.GetOrderUserHistory("0B008236-860D-44BB-3328-08D9E8AFEA5C");
            if (!orderHistoryRespone.IsSuccessStatusCode)
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            return View(orderHistoryRespone.Content.Data);
        }
       
    }
}
