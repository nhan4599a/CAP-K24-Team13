using GUI.Abtractions;
using GUI.Areas.User.ViewModels;
using GUI.Clients;
using GUI.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared;
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
            var token = await HttpContext.GetTokenAsync(SystemConstant.Authentication.ACCESS_TOKEN_KEY);
            var cartItemsResponse = await _cartClient.GetCartItemsAsync(token, User.GetUserId().ToString());
            if (!cartItemsResponse.IsSuccessStatusCode)
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            ViewData["CartItems"] = cartItemsResponse.Content.Data;
            return View(new CartViewModel
            {
                Email = User.GetUsername(),
                Size = cartItemsResponse.Content.Data.Count,
                Items = cartItemsResponse.Content.Data
            });
        }
    }
}
