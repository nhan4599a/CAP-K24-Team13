using AspNetCoreSharedComponent.FileValidations;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.DTOs;
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

        [HttpPost]
        public async Task<ApiResult<Guid>> AddProduct([FromForm(Name = "requestModel")] CreateOrEditProductRequestModel requestModel)
        {
            requestModel.ImagePaths = await _fileStore.SaveFilesAsync(Request.Form.Files);
            var response = await _mediator.Send(new CreateProductCommand
            {
                RequestModel = requestModel
            });
            if (!response.IsSuccess)
                return new ApiResult<Guid> { ResponseCode = 500, Data = Guid.Empty, ErrorMessage = response.ErrorMessage };
            return new ApiResult<Guid> { ResponseCode = 200, Data = response.Response };
        }

        [HttpPut("{id}")]
        public async Task<ApiResult<ProductDTO>> EditProduct(string id, [FromForm(Name = "requestModel")] CreateOrEditProductRequestModel requestModel)
        {
            requestModel.ImagePaths = await _fileStore.EditFilesAsync(requestModel.ImagePaths, Request.Form.Files);
            var response = await _mediator.Send(new EditProductCommand
            {
                Id = Guid.Parse(id),
                RequestModel = requestModel
            });
            if (!response.IsSuccess)
                return new ApiResult<ProductDTO> { ResponseCode = 500, Data = null, ErrorMessage = response.ErrorMessage };
            return new ApiResult<ProductDTO> { ResponseCode = 200, Data = response.Response };
        }

        [HttpDelete("{id}")]
        public async Task<ApiResult<bool>> DeleteProduct(string id, [FromQuery] DeleteAction action)
        {
            var response = await _mediator.Send(new ActivateProductCommand
            {
                Id = Guid.Parse(id),
                IsActivateCommand = action == DeleteAction.Activate,
            });
            if (!response.IsSuccess)
                return new ApiResult<bool> { ResponseCode = 500, Data = false, ErrorMessage = response.ErrorMessage };
            return new ApiResult<bool> { ResponseCode = 200, Data = response.Response };
        }

        [HttpGet]
        public async Task<ApiResult<PaginatedList<ProductDTO>>> ListProduct([FromQuery] SearchProductRequestModel requestModel)
        {
            IRequest<PaginatedList<ProductDTO>> request = string.IsNullOrEmpty(requestModel.Keyword)
                ? new FindAllProductQuery { PaginationInfo = requestModel.PaginationInfo }
                : new FindProductsByKeywordQuery
                {
                    Keyword = requestModel.Keyword,
                    PaginationInfo = requestModel.PaginationInfo
                };
            var productList = await _mediator.Send(request);
            return new ApiResult<PaginatedList<ProductDTO>>
            {
                ResponseCode = 200,
                Data = productList
            };
        }

        [HttpGet("{id}")]
        public async Task<ApiResult<ProductDTO>> GetSingleProduct(string id)
        {
            var product = await _mediator.Send(new FindProductByIdQuery
            {
                Id = Guid.Parse(id)
            });
            if (product == null)
                return new ApiResult<ProductDTO> { ResponseCode = 404, ErrorMessage = "Product is not found" };
            return new ApiResult<ProductDTO> { ResponseCode = 200, Data = product };
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