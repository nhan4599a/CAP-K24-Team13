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
                var cartItemsResponse = await _cartClient.GetCartItemsAsync("3ea07403-8d98-4acd-c46f-08d9e300baaf");
                if (cartItemsResponse.IsSuccessStatusCode)
                    controller.ViewData["CartItems"] = cartItemsResponse.Content;
            }
            await next();
        }
    }
}
