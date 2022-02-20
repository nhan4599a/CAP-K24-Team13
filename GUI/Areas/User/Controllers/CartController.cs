using GUI.Abtractions;
using GUI.Areas.User.ViewModels;
using GUI.Clients;
using Microsoft.AspNetCore.Authorization;
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
            var cartItemsResponse = await _cartClient.GetCartItemsAsync("C61AF282-818D-43CA-6DA9-08D9E08DBE4D");
            if (!cartItemsResponse.IsSuccessStatusCode)
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            ViewData["CartItems"] = cartItemsResponse.Content.Data;
            return View(new CartViewModel
            {
                Email = "customer@test.com",
                Size = cartItemsResponse.Content.Data.Count,
                Items = cartItemsResponse.Content.Data
            });
        }
    }
}
