using GUI.Clients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GUI.Areas.User.Controllers
{
    public class UserAccountController : Controller
    {
        private readonly IOrderClient _orderHistoryClient;

        public UserAccountController(IOrderClient orderHistoryClient)
        {
            _orderHistoryClient = orderHistoryClient;
        }

        public async Task<IActionResult> Profile()
        {
            var orderHistoryRespone = await _orderHistoryClient.GetOrderUserHistory("324DFA41-D0E8-46CD-1975-08D9EB65B707");
            if (!orderHistoryRespone.IsSuccessStatusCode)
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            return View(orderHistoryRespone.Content.Data);
        }
       
    }
}
