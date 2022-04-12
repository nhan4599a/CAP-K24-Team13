﻿using GUI.Abtractions;
using GUI.Areas.User.ViewModels;
using GUI.Clients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GUI.Areas.User.Controllers
{
    public class ShopController : BaseUserController
    {
        private readonly IProductClient _productClient;
        private readonly IShopClient _shopClient;
        private readonly ICategoryClient _categoryClient;

        public ShopController(IProductClient productClient, IShopClient shopClient,
            ICategoryClient categoryClient)
        {
            _productClient = productClient;
            _shopClient = shopClient;
            _categoryClient = categoryClient;
        }

        public async Task<IActionResult> Index(int id)
        {
            var bestSellerProductsResponse = await _productClient.GetBestSellerProducts(id);
            var shopCategoriesResponse = await _categoryClient.GetCategoriesOfShop(id, 4);
            var shopResponse = await _shopClient.GetShop(id);
            if (!bestSellerProductsResponse.IsSuccessStatusCode || !shopCategoriesResponse.IsSuccessStatusCode
                    || !shopResponse.IsSuccessStatusCode)
                return StatusCode(StatusCodes.Status500InternalServerError);
            var productsOfCategory = new Dictionary<int, List<ProductDTO>>();
            foreach (var categoryId in shopCategoriesResponse.Content.Data.Select(e => e.Id))
            {
                var productsResponse = await _productClient.GetProductsOfCategory(new int[] { categoryId }, 1, 5);
                if (!productsResponse.IsSuccessStatusCode)
                    return StatusCode(StatusCodes.Status500InternalServerError);
                productsOfCategory.Add(categoryId, productsResponse.Content.Data.ToList());
            }
            return View(new ShopDetailViewModel
            {
                Categories = shopCategoriesResponse.Content.Data.ToList(),
                Shop = shopResponse.Content.ResultObj,
                BestSeller = bestSellerProductsResponse.Content.Data,
                Products = productsOfCategory
            });
        }

        public async Task<IActionResult> Categories(int id, [FromQuery(Name = "cat")] List<int> categoryId, int pageNumber = 1)
        {
            var shopCategoriesResponse = await _categoryClient.GetCategoriesOfShop(id, 0);
            var shopResponse = await _shopClient.GetShop(id);
            var productsResponse = await _productClient.GetProductsOfCategory(categoryId.ToArray(), pageNumber, 20);
            if (!shopResponse.IsSuccessStatusCode || !shopCategoriesResponse.IsSuccessStatusCode
                    || !productsResponse.IsSuccessStatusCode)
                return StatusCode(StatusCodes.Status500InternalServerError);
            return View(new ShopCategoryViewModel
            {
                Categories = shopCategoriesResponse.Content.Data.ToList(),
                Shop = shopResponse.Content.ResultObj,
                Products = productsResponse.Content.Data
            });
        }
    }
}
