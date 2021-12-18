using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.DTOs;
using Shared.RequestModels;
using ShopProductService.Commands.Product;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopProductService.Controllers
{
    [ApiController]
    [Route("/api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ProductImageManager _imageManager;

        public ProductController(IMediator mediator, ProductImageManager imageManager)
        {
            _mediator = mediator;
            _imageManager = imageManager;
        }

        [HttpPost]
        public async Task<ApiResult<Guid>> AddProduct([FromForm(Name = "requestModel")] CreateOrEditProductRequestModel requestModel)
        {
            requestModel.ImagePaths = await _imageManager.SaveFilesAsync(Request.Form.Files);
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
            var oldFilesName = requestModel.ImagePaths;
            requestModel.ImagePaths = await _imageManager.EditFilesAsync(oldFilesName, Request.Form.Files);
            var response = await _mediator.Send(new EditProductCommand
            {
                Id = new Guid(id),
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
                Id = new Guid(id),
                IsActivateCommand = action == DeleteAction.Activate,
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
                return new ApiResult<ProductDTO> { ResponseCode = 404, ErrorMessage = "Product is not found" };
            return new ApiResult<ProductDTO> { ResponseCode = 200, Data = product };
        }

        [HttpGet("images/{imageId}")]
        public PhysicalFileResult Index(string imageId)
        {
            var fileResponse = _imageManager.GetImage(imageId);
            if (!fileResponse.IsExisted)
                return null;
            return PhysicalFile(fileResponse.FullPath, fileResponse.MimeType);
        }
    }
}