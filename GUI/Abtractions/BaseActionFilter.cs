using GUI.Areas.User.Controllers;
using GUI.Clients;
using GUI.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace GUI.Abtractions
{
    public class BaseActionFilter : IAsyncActionFilter
    {
        private readonly ICartClient _cartClient;

        public BaseActionFilter(ICartClient cartClient)
        {
            _cartClient = cartClient;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var controller = context.Controller as Controller;
                if (controller is not CartController)
                {
                    var token = await context.HttpContext.GetTokenAsync("access_token");
                    var cartItemsResponse = await _cartClient.GetCartItemsAsync(token, context.HttpContext.User.GetUserId().ToString());
                    if (cartItemsResponse.IsSuccessStatusCode)
                        controller.ViewData["CartItems"] = cartItemsResponse.Content.Data;
                }
            }
            await next();
        }
    }
}
