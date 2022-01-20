using GUI.Areas.User.ViewModels;
using GUI.Attributes;
using GUI.Clients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GUI.Areas.User.Controllers
{
    [VirtualArea("User")]
    public class CartController : Controller
    {
        private readonly ICartClient _cartClient;

        public CartController(ICartClient cartClient)
        {
            _cartClient = cartClient;
        }

        public async Task<IActionResult> Index()
        {
            var cartItemsResponse = await _cartClient.GetCartItemsAsync("90CCBB88-C919-4CA7-FC67-08D9DBBCCCFB");
            if (!cartItemsResponse.IsSuccessStatusCode)
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            return View(new CartViewModel
            {
                Email = "customer@test.com",
                Size = cartItemsResponse.Content.Count,
                Items = cartItemsResponse.Content
            });
        }
    }
}
