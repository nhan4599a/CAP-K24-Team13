using GUI.Areas.User.Controllers;
using GUI.Clients;
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
            var controller = context.Controller as Controller;
            if (controller is not CartController)
            {
                var cartItemsResponse = await _cartClient.GetCartItemsAsync("324DFA41-D0E8-46CD-1975-08D9EB65B707");
                if (cartItemsResponse.IsSuccessStatusCode)
                    controller.ViewData["CartItems"] = cartItemsResponse.Content;
            }
            await next();
        }
    }
}
