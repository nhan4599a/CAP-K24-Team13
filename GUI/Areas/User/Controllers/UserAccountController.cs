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
            var orderHistoryRespone = await _orderHistoryClient.GetOrderUserHistory("C61AF282-818D-43CA-6DA9-08D9E08DBE4D");
            if (!orderHistoryRespone.IsSuccessStatusCode)
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            return View(orderHistoryRespone.Content.Data);
        }
       
    }
}
