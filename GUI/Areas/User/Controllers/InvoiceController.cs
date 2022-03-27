using GUI.Abtractions;
using GUI.Clients;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GUI.Areas.User.Controllers
{
    public class InvoiceController : BaseUserController
    {
        private readonly IOrderClient _orderClient;

        public InvoiceController(IOrderClient orderClient)
        {
            _orderClient = orderClient;
        }

        [HttpGet("{invoiceCode}")]
        public async Task<IActionResult> Index(string invoiceCode)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var orderDetailResponse = await _orderClient.GetOrderDetail(accessToken, invoiceCode);
            if (!orderDetailResponse.IsSuccessStatusCode)
                return StatusCode(StatusCodes.Status500InternalServerError);
            if (orderDetailResponse.Content.ResponseCode == 403)
                return StatusCode(StatusCodes.Status403Forbidden);
            if (orderDetailResponse.Content.ResponseCode == 404)
                return StatusCode(StatusCodes.Status404NotFound);
            return View(orderDetailResponse.Content.Data);
        }
    }
}
