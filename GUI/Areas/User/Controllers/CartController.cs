using GUI.Abtractions;
using GUI.Areas.User.ViewModels;
using GUI.Clients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GUI.Areas.User.Controllers
{
    public class CartController : BaseUserController
    {
        private readonly ICartClient _cartClient;

        public CartController(ICartClient cartClient)
        {
            _cartClient = cartClient;
        }

        public async Task<IActionResult> Index()
        {
            var cartItemsResponse = await _cartClient.GetCartItemsAsync("3ea07403-8d98-4acd-c46f-08d9e300baaf");
            if (!cartItemsResponse.IsSuccessStatusCode)
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            ViewData["CartItems"] = cartItemsResponse.Content;
            return View(new CartViewModel
            {
                Email = "customer@test.com",
                Size = cartItemsResponse.Content.Count,
                Items = cartItemsResponse.Content
            });
        }
    }
}
