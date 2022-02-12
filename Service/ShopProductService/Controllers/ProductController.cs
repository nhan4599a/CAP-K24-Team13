using AspNetCoreSharedComponent.FileValidations;
using DatabaseAccessor;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Exceptions;
using Shared.Models;
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

        private readonly IFileStorable _fileStore;

        private readonly static PaginatedList<ProductDTO> FakeProducts = new List<ProductDTO>
        {
            new ProductDTO
            {
                Id = "MacBook Pro M1",
                ProductName = "Macbook Pro M1",
                CategoryName = "category 1",
                Price = 20000,
                Description = "GOLD",
                IsDisabled = false,
                Quantity = 10,
                Discount = 0,
                Images = new string[]
                {
                    "https://futureworld.com.vn/media/catalog/product/cache/374a8abfba56573d9bc051f80221efb2/m/b/mba_gold_m1_2.jpg"
                }
            },
            new ProductDTO
            {
                Id = "iphone 13",
                ProductName = "Iphone 13",
                CategoryName = "category 2",
                Price = 20000,
                Description = "64gb",
                IsDisabled = false,
                Quantity = 10,
                Discount = 0,
                Images = new string[]
                {
                    "https://mega.com.vn/media/product/20113_iphone_13_256gb_white.jpg"
                }
            },
            new ProductDTO
            {
                Id = "Nike AIR",
                ProductName = "Nike AIR",
                CategoryName = "category 1",
                Price = 20000,
                Description = "White",
                IsDisabled = false,
                Quantity = 10,
                Discount = 0,
                Images =new string[]
                {
                    "https://static.nike.com/a/images/c_limit,w_592,f_auto/t_product_v1/e777c881-5b62-4250-92a6-362967f54cca/air-force-1-07-shoe-NMmm1B.png"
                }
            },
            new ProductDTO
            {
                Id = "Bitis Hunter",
                ProductName = "Bitis Hunter",
                CategoryName = "category 1",
                Price = 20000,
                Description = "Back-Orange",
                IsDisabled = false,
                Quantity = 10,
                Discount = 0,
                Images = new string[]
                {
                    "https://product.hstatic.net/1000230642/product/03400cam__6__5022ef5622dc46b1bd893b238de2200f_1024x1024.jpg"
                }
            },
            new ProductDTO
            {
                Id = "Converse",
                ProductName = "ASM Converse",
                CategoryName = "category 1",
                Price = 20000,
                Description = "WHITE",
                IsDisabled = false,
                Quantity = 10,
                Discount = 0,
                Images = new string[]
                {
                    "https://th.bing.com/th/id/OIP.yRbQi9-1aDN-BXHuyD_vZAHaG5?pid=ImgDet&rs=1"
                }
            }
        }.Paginate(1, 5);

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
            return ApiResult<Guid>.CreateSuccessResult(response.Response);
        }

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
            return ApiResult<ProductDTO>.CreateSuccessResult(response.Response);
        }

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
            return ApiResult<bool>.CreateSuccessResult(response.Response);
        }

        [HttpGet("shop/{shopId}")]
        public async Task<ApiResult> GetProductsOfShop(int shopId)
        {
            if (shopId != 0)
                return ApiResult<PaginatedList<ProductDTO>>.CreateSuccessResult(FakeProducts);
            var response = await _mediator.Send(new FindProductsByShopIdQuery
            {
                ShopId = shopId
            });
            return ApiResult<PaginatedList<ProductDTO>>.CreateSuccessResult(response);
        }

        [HttpGet("search")]
        public async Task<ApiResult> ListProduct([FromQuery] SearchRequestModel requestModel)
        {
            IRequest<PaginatedList<ProductDTO>> request = string.IsNullOrEmpty(requestModel.Keyword)
                ? new FindAllProductQuery { PaginationInfo = requestModel.PaginationInfo, IncludeComments = false }
                : new FindProductsByKeywordQuery
                {
                    Keyword = requestModel.Keyword,
                    PaginationInfo = requestModel.PaginationInfo
                };
            var productList = await _mediator.Send(request);
            return ApiResult<PaginatedList<ProductDTO>>.CreateSuccessResult(productList);
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
            return ApiResult<ProductWithCommentsDTO>.CreateSuccessResult((ProductWithCommentsDTO) product);
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
            return ApiResult<MinimalProductDTO>.CreateSuccessResult(product);
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