using GUI.Abtractions;
using GUI.Areas.User.Models;
using GUI.Areas.User.ViewModels;
using GUI.Clients;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GUI.Areas.User.Controllers
{
    [Authorize]
    public class CheckoutController : BaseUserController
    {
        private readonly IProductClient _productClient;

        public CheckoutController(IProductClient productClient)
        {
            _productClient = productClient;
        }

        [HttpPost]
        public async Task<IActionResult> Index(CheckOutModel[] models)
        {
            var token = await HttpContext.GetTokenAsync(SystemConstant.Authentication.ACCESS_TOKEN_KEY);
            List<CheckOutViewModel> productList = new();
            var productsError = new List<string>();
            foreach (var model in models)
            {
                var productResponse = await _productClient.GetProductInfoInCheckout(token, model.ProductId);
                if (!productResponse.IsSuccessStatusCode || productResponse.Content.ResponseCode != 200)
                    return StatusCode(StatusCodes.Status500InternalServerError);
                if (model.Quantity > productResponse.Content.Data.Quantity || !productResponse.Content.Data.IsAvailable)
                {
                    productsError.Add(productResponse.Content.Data.ProductName);
                }
                else
                {
                    productList.Add(new CheckOutViewModel
                    {
                        Quantity = model.Quantity,
                        Item = productResponse.Content.Data
                    });
                }
            }
            if (productsError.Count > 0)
            {
                TempData["CheckingOut-Error"] = productsError;
                return Redirect("/cart");
            }
            return View(productList);
        }
    }
}
