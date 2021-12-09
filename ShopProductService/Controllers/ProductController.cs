using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.DTOs;
using Shared.RequestModels;
using ShopProductService.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopProductService.Controllers
{
    [ApiController]
    [Route("/api/products")]
    public class ProductController : Controller
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ApiResult<bool>> AddProduct(AddOrEditProductRequestModel requestModel)
        {
            var response = await _mediator.Send(new AddProductCommand
            {
                RequestModel = requestModel
            });
            if (!response.Response)
                return new ApiResult<bool> { ResponseCode = 500, Data = false, ErrorMessage = response.ErrorMessage };
            return new ApiResult<bool> { ResponseCode = 200, Data = true };
        }

        [HttpPut("{id}")]
        public async Task<ApiResult<bool>> EditProduct(string id, AddOrEditProductRequestModel requestModel)
        {
            var response = await _mediator.Send(new EditProductCommand
            {
                Id = new Guid(id),
                RequestModel = requestModel
            });
            if (!response.Response)
                return new ApiResult<bool> { ResponseCode = 500, Data = false, ErrorMessage = response.ErrorMessage };
            return new ApiResult<bool> { ResponseCode = 200, Data = true };
        }

        [HttpDelete("{id}")]
        public async Task<ApiResult<bool>> DeleteProduct(string id)
        {
            var response = await _mediator.Send(new DeleteProductCommand
            {
                Id = new Guid(id)
            });
            if (!response.Response)
                return new ApiResult<bool> { ResponseCode = 500, Data = false, ErrorMessage = response.ErrorMessage };
            return new ApiResult<bool> { ResponseCode = 200, Data = true };
        }

        [HttpGet]
        public async Task<ApiResult<PaginatedDataList<ProductDTO>>> ListProduct([FromQuery] SearchProductRequestModel requestModel)
        {
            IRequest<List<ProductDTO>> request = string.IsNullOrEmpty(requestModel.Keyword) ? new FindAllProductQuery() :
                new FindProductsByKeywordQuery
                {
                    Keyword = requestModel.Keyword
                };
            var productList = await _mediator.Send(request);
            return new ApiResult<PaginatedDataList<ProductDTO>>
            {
                ResponseCode = 200,
                Data = productList.Paginate(requestModel.PaginationInfo.PageNumber, requestModel.PaginationInfo.PageSize)
            };
        }

        [HttpGet("{id}")]
        public async Task<ApiResult<ProductDTO>> GetSingleProduct(string id)
        {
            var product = await _mediator.Send(new FindProductByIdQuery
            {
                Id = new Guid(id)
            });
            if (product == null)
                return new ApiResult<ProductDTO> { ResponseCode = 404 };
            return new ApiResult<ProductDTO> { ResponseCode = 200, Data = product };
        }
    }
}