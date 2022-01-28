using GUI.Abtractions;
using GUI.Areas.User.Models;
using GUI.Areas.User.ViewModels;
using GUI.Clients;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GUI.Areas.User.Controllers
{
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
            List<CheckOutViewModel> productList = new();
            foreach (var model in models)
            {
                var productResponse = await _productClient.GetProductInfoInCheckout(model.ProductId);
                if (!productResponse.IsSuccessStatusCode || productResponse.Content.ResponseCode == 404)
                {
                    ViewBag.HasError = true;
                }
                productList.Add(new CheckOutViewModel
                {
                    Quantity = model.Quantity,
                    Item = productResponse.Content.Data
                });
            }
            return View(productList);
        }
    }
}
