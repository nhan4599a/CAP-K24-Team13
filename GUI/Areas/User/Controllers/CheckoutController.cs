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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared;
using Shared.Extensions;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

        private readonly IConfiguration _configuration;

        public CheckoutController(IProductClient productClient, IInvoiceClient invoiceClient,
            PaymentProcessorFactory paymentProcessorFactory, ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _productClient = productClient;
            _invoiceClient = invoiceClient;
            _paymentProcessorFactory = paymentProcessorFactory;
            _logger = loggerFactory.CreateLogger<CheckoutController>();
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Payment([FromQuery] PaymentMethod method, [FromForm] string paymentRefId)
        {
            if (method == PaymentMethod.CoD)
                return StatusCode(StatusCodes.Status404NotFound);
            var accessToken = await HttpContext.GetTokenAsync(SystemConstant.Authentication.ACCESS_TOKEN_KEY);
            var invoice = await _invoiceClient.GetOrderDetailByRefId(accessToken, paymentRefId);
            if (!invoice.IsSuccessStatusCode || invoice.Content.ResponseCode != 200)
                return StatusCode(StatusCodes.Status500InternalServerError);
            if (method == PaymentMethod.MoMo)
            {
                var paymentProcessor = _paymentProcessorFactory.Create(method);
                var extraData = $"user={accessToken}";
                var momoRequest = new MomoWalletCaptureRequest
                {
                    AccessKey = _configuration["MOMO_ACCESS_KEY"],
                    PartnerCode = _configuration["MOMO_PARTNER_CODE"],
                    RequestId = paymentRefId,
                    OrderId = paymentRefId,
                    OrderInfo = $"Order no {paymentRefId}",
                    ResponseLanguage = "en",
                    RedirectUrl = "https://cap-k24-team13.herokuapp.com/order-history",
                    IpnUrl = "https://cap-k24-team13.herokuapp.com/checkout/momo-payment-postback",
                    Amount = (int)Math.Ceiling(invoice.Content.Data.Sum(e => e.TotalPrice)),
                    ExtraData = extraData
                };
                try
                {
                    var momoResponse = (MomoWalletCaptureResponse)await paymentProcessor.ExecuteAsync(momoRequest);
                    _logger.LogInformation("Request signature: " + momoRequest.Signature);
                    if (momoResponse.IsErrorResponse())
                        throw new Exception("Validation failed: " 
                            + string.Join(",", momoResponse.SubErrors.Select(e => e.Field)));
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

        [Route("/checkout/momo-payment-postback")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> MomoPaymentPostback()
        {
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            var rawMessage = await reader.ReadToEndAsync();
            var momoIpnRequest = JsonConvert.DeserializeObject<MomoWalletIpnRequest>(rawMessage);
            momoIpnRequest.AccessKey = _configuration["MOMO_ACCESS_KEY"];
            var momoProcessor = (MomoWalletProcessor)_paymentProcessorFactory.Create(PaymentMethod.MoMo);
            if (momoProcessor.Security.ValidateIpnRequest(momoIpnRequest))
            {
                var accessToken = momoIpnRequest.ExtraData.Split("=")[1];
                var data = await _invoiceClient.MakeAsPaid(accessToken, momoIpnRequest.OrderId, new MakeAsPaidRequestModel
                {
                    AccessToken = accessToken,
                    WalletIpnRequest = rawMessage
                });
                if (!data.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Request failed, status code: " + (int)data.StatusCode);
                }
                if (data.Content != null)
                {
                    if (data.Content.ResponseCode != 200)
                    {
                        _logger.LogInformation("Request failed: " + data.Content.ErrorMessage);
                    }
                    else
                    {
                        _logger.LogInformation("Request status: " + data.Content.ResponseCode);
                    }
                }
                if (data.Content == null)
                {
                    _logger.LogError("Content is null");
                }
            }
            _logger.LogError("Validate momo signature failed");
            return StatusCode(StatusCodes.Status204NoContent);
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
