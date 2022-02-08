using GUI.Abtractions;
using GUI.Areas.User.ViewModels;
using GUI.Clients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GUI.Areas.User.Controllers
{
    [Authorize]
    public class CartController : BaseUserController
    {
        private readonly ICartClient _cartClient;

        public CartController(ICartClient cartClient)
        {
            _cartClient = cartClient;
        }

        public async Task<IActionResult> Index()
        {
            var cartItemsResponse = await _cartClient.GetCartItemsAsync("0B008236-860D-44BB-3328-08D9E8AFEA5C");
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
