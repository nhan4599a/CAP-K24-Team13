using GUI.Attributes;
using GUI.Clients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GUI.Areas.User.Controllers
{
    [VirtualArea("User")]
    public class ProductController : Controller
    {
        private readonly IProductClient _productClient;
        private readonly ICartClient _cartClient;

        public ProductController(IProductClient productClient, ICartClient cartClient)
        {
            _productClient = productClient;
            _cartClient = cartClient;
        }

        public async Task<IActionResult> Detail(string id)
        {
        	var productResponse = await _productClient.GetProductAsync(id);
            var cartItemCountResponse = await _cartClient.GetCartItemCountAsync("F081C3C0-3314-44D8-1055-08D9DA433EEF");
        	if (!productResponse.IsSuccessStatusCode || productResponse.Content.ResponseCode != StatusCodes.Status200OK)
			    return new StatusCodeResult(StatusCodes.Status404NotFound);
            ViewBag.CartItemCount = cartItemCountResponse.IsSuccessStatusCode ? cartItemCountResponse.Content : 0;
		    return View(productResponse.Content.Data);
	    }
    }
}
