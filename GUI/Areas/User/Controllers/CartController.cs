using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared;
using System;
using System.Collections.Generic;

namespace GUI.Areas.User.Controllers
{
    public class CartController : Controller
    {
        private const string CART_SESSION_KEY = "Cart";

        private readonly ISession _session;
        private readonly Dictionary<Guid, CartItem> _cartItems;

        public CartController(IHttpContextAccessor contextAccessor)
        {
            _session = contextAccessor.HttpContext.Session;
            _cartItems = JsonConvert.DeserializeObject<Dictionary<Guid, CartItem>>(_session.GetString(CART_SESSION_KEY) ?? "{}");
        }

        public IReadOnlyCollection<CartItem> Index() => _cartItems.Values;

        private void UpdateCartAsync()
        {
            _session.SetString(CART_SESSION_KEY, JsonConvert.SerializeObject(_cartItems));
        }
    }
}
