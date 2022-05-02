using GUI.Abtractions;
using GUI.Areas.User.Models;
using GUI.Areas.User.ViewModels;
using GUI.Clients;
using GUI.Payments.Factory;
using GUI.Payments.Momo.Models;
using GUI.Payments.Momo.Processor;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GUI.Areas.User.Controllers
{
    [Authorize]
    public class CheckoutController : BaseUserController
    {
        private readonly IProductClient _productClient;

        private readonly IInvoiceClient _invoiceClient;

        private readonly PaymentProcessorFactory _paymentProcessorFactory;

        private readonly ILogger<CheckoutController> _logger;

        public CheckoutController(IProductClient productClient, IInvoiceClient invoiceClient,
            PaymentProcessorFactory paymentProcessorFactory, ILoggerFactory loggerFactory)
        {
            _productClient = productClient;
            _invoiceClient = invoiceClient;
            _paymentProcessorFactory = paymentProcessorFactory;
            _logger = loggerFactory.CreateLogger<CheckoutController>();
        }

        [HttpPost]
        public async Task<IActionResult> Payment([FromQuery] PaymentMethod method, [FromForm] string paymentRefId)
        {
            if (method == PaymentMethod.CoD)
                return StatusCode(StatusCodes.Status404NotFound);
            var accessToken = await HttpContext.GetTokenAsync(SystemConstant.Authentication.ACCESS_TOKEN_KEY);
            var invoice = await _invoiceClient.GetOrderDetailByRefId(accessToken, paymentRefId);
            if (method == PaymentMethod.MoMo)
            {
                var paymentProcessor = _paymentProcessorFactory.Create(method) as MomoWalletProcessor;
                try
                {
                    var momoResponse = (MomoWalletCaptureResponse)await paymentProcessor.ExecuteAsync(new MomoWalletCaptureRequest());
                    if (momoResponse.IsErrorResponse())
                        throw new Exception("Validation failed");
                    return Redirect(momoResponse.PayUrl);
                }
                catch (Exception e)
                {
                    _logger.LogError("Failed to request to momo wallet, error: " + e.Message);
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(CheckOutModel[] models)
        {
            try
            {
                List<CheckOutViewModel> productList = await PrepareProductsInfo(models);
                if (productList.Any(e => !e.IsAvailable))
                {
                    TempData["CheckingOut-Error"] = productList.Where(item => !item.IsAvailable).ToList();
                    return Redirect("/cart");
                }
                return View(productList.Where(item => item.IsAvailable).ToList());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [NonAction]
        private async Task<List<CheckOutViewModel>> PrepareProductsInfo(CheckOutModel[] models)
        {
            var result = new List<CheckOutViewModel>();
            foreach (var model in models)
            {
                var productResponse = await _productClient.GetProductInfoInCheckout(model.ProductId);
                if (!productResponse.IsSuccessStatusCode || productResponse.Content.ResponseCode != 200)
                    throw new Exception("Failed to load product");

                result.Add(new CheckOutViewModel
                {
                    Quantity = model.Quantity,
                    Item = productResponse.Content.Data,
                    IsAvailable = model.Quantity <= productResponse.Content.Data.Quantity && productResponse.Content.Data.IsAvailable
                });
            }
            return result;
        }
    }
}
