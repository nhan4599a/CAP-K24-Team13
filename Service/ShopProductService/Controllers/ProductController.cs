using AspNetCoreSharedComponent.FileValidations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Exceptions;
using Shared.Models;
using Shared.RequestModels;
using ShopProductService.Commands.Product;
using System;
using System.Threading.Tasks;

namespace ShopProductService.Controllers
{
    [ApiController]
    [Route("/api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly IFileStorable _fileStore;

        public ProductController(IMediator mediator, IFileStorable fileStore)
        {
            _mediator = mediator;
            if (fileStore != null)
            {
                _fileStore = fileStore;
                _fileStore.SetRelationalPath("products");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ApiResult> AddProduct([FromForm(Name = "requestModel")] CreateOrEditProductRequestModel requestModel)
        {
            try
            {
                requestModel.ImagePaths = await _fileStore.SaveFilesAsync(Request.Form.Files);
            }
            catch (ImageValidationException ex)
            {
                return ApiResult.CreateErrorResult(400, ex.Message);
            }
            var response = await _mediator.Send(new CreateProductCommand
            {
                RequestModel = requestModel
            });
            if (!response.IsSuccess)
                return ApiResult.CreateErrorResult(500, response.ErrorMessage);
            return ApiResult<Guid>.CreateSucceedResult(response.Response);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ApiResult> EditProduct(string id, [FromForm(Name = "requestModel")] CreateOrEditProductRequestModel requestModel)
        {
            try
            {
                requestModel.ImagePaths = await _fileStore.EditFilesAsync(requestModel.ImagePaths, Request.Form.Files);
            }
            catch (ImageValidationException ex)
            {
                return ApiResult.CreateErrorResult(400, ex.Message);
            }
            var response = await _mediator.Send(new EditProductCommand
            {
                Id = Guid.Parse(id),
                RequestModel = requestModel
            });
            if (!response.IsSuccess)
                return ApiResult.CreateErrorResult(500, response.ErrorMessage);
            return ApiResult<ProductDTO>.CreateSucceedResult(response.Response);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ApiResult> DeleteProduct(string id, [FromQuery] DeleteAction action)
        {
            var response = await _mediator.Send(new ActivateProductCommand
            {
                Id = Guid.Parse(id),
                IsActivateCommand = action == DeleteAction.Activate,
            });
            if (!response.IsSuccess)
                return ApiResult.CreateErrorResult(500, response.ErrorMessage);
            return ApiResult<bool>.CreateSucceedResult(response.Response);
        }

        [HttpGet("shop/{shopId}")]
        public async Task<ApiResult> GetProductsOfShop(int shopId)
        {
            var response = await _mediator.Send(new FindProductsByShopIdQuery
            {
                ShopId = shopId
            });
            return ApiResult<PaginatedList<ProductDTO>>.CreateSucceedResult(response);
        }

        [HttpGet("search")]
        public async Task<ApiResult> ListProduct([FromQuery] SearchRequestModel requestModel)
        {
            IRequest<PaginatedList<ProductDTO>> request = string.IsNullOrEmpty(requestModel.Keyword)
                ? new FindAllProductQuery { PaginationInfo = requestModel.PaginationInfo }
                : new FindProductsByKeywordQuery
                {
                    Keyword = requestModel.Keyword,
                    PaginationInfo = requestModel.PaginationInfo
                };
            var productList = await _mediator.Send(request);
            return ApiResult<PaginatedList<ProductDTO>>.CreateSucceedResult(productList);
        }

        [HttpGet("{id}")]
        public async Task<ApiResult> GetSingleProduct(string id)
        {
            var product = await _mediator.Send(new FindProductByIdQuery
            {
                Id = Guid.Parse(id)
            });
            if (product == null)
                return ApiResult.CreateErrorResult(404, "Product is not found");
            return ApiResult<ProductWithCommentsDTO>.CreateSucceedResult((ProductWithCommentsDTO) product);
        }

        [HttpGet("less/{id}")]
        public async Task<ApiResult> GetMinimalSingleProduct(string id)
        {
            var product = await _mediator.Send(new FindProductByIdQuery
            {
                Id = Guid.Parse(id),
                IsMinimal = true
            });
            if (product == null)
                return ApiResult.CreateErrorResult(404, "Product is not found");
            return ApiResult<MinimalProductDTO>.CreateSucceedResult(product);
        }

        [HttpPost("{productId}/import")]
        public async Task<ApiResult> ImportProduct(string productId, [FromBody] int importedQuantity)
        {
            var newQuantityResponse = await _mediator.Send(new ImportProductCommand
            {
                ProductId = Guid.Parse(productId),
                Quantity = importedQuantity
            });
            if (!newQuantityResponse.IsSuccess)
                return ApiResult.CreateErrorResult(500, newQuantityResponse.ErrorMessage);
            return ApiResult<int>.CreateSucceedResult(newQuantityResponse.Response);
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("images/{image}")]
        public IActionResult GetImage(string image)
        {
            var fileResponse = _fileStore.GetFile(image);
            if (!fileResponse.IsExisted)
                return StatusCode(StatusCodes.Status404NotFound);
            return PhysicalFile(fileResponse.FullPath, fileResponse.MimeType);
        }
    }
}