﻿using DatabaseAccessor;
using DatabaseAccessor.Model;
using Microsoft.AspNetCore.Mvc;
using Shared;
using System.Threading.Tasks;
using Shared.DTOs;
using System.Linq;
using System.Collections.Generic;
using ShopProductService.RequestModel;

namespace ShopProductService.Controllers
{
    [ApiController]
    [Route("/api/products")]
    public class ProductController : Controller
    {
        private ApplicationDbContext _dbContext;

        public ProductController(ApplicationDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        [HttpPost]
        [ActionName("Add")]
        public ApiResult<bool> AddProduct(AddProductRequestModel requestModel)
        {
            _dbContext.ShopProducts.Add(new ShopProduct
            {
                ProductName = requestModel.ProductName,
                CategoryId = requestModel.CategoryId,
                Description = requestModel.Description,
                Quantity = requestModel.Quantity,
                Price = requestModel.Price,
                Discount = requestModel.Discount
            });

            _dbContext.SaveChangesAsync();

            return new ApiResult<bool> { ResponseCode = 200, Data = true };
        }

        [HttpGet]
        public ApiResult<List<ProductDTO>> ListProduct([FromQuery] int pageNumber, int pageSize = 5)
        {
            var products = _dbContext.ShopProducts.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).Select(product => ProductDTO.FromSource(product)).Cast<ProductDTO>().ToList();
            return new ApiResult<List<ProductDTO>> { ResponseCode = 200, Data = products };
        }
    }
}