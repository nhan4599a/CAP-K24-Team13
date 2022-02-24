﻿using GUI.Clients;
using GUI.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GUI.Areas.User.Controllers
{
    [Authorize]
    public class UserAccountController : Controller
    {
        private readonly IOrderClient _orderHistoryClient;

        public UserAccountController(IOrderClient orderHistoryClient)
        {
            _orderHistoryClient = orderHistoryClient;
        }

        public async Task<IActionResult> Profile()
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var orderHistoryRespone = await _orderHistoryClient.GetOrderUserHistory(token, User.GetUserId().ToString() );
            if (!orderHistoryRespone.IsSuccessStatusCode)
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            return View(orderHistoryRespone.Content.Data);
        }
       
    }
}
