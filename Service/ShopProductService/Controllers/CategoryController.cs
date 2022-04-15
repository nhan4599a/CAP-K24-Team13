using AspNetCoreSharedComponent.FileValidations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Models;
using ShopProductService.Commands.Category;
using System.Threading.Tasks;

namespace ShopProductService.Controllers
{
    [ApiController]
    [Route("/api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public CategoryController(IMediator mediator, IFileStorable fileStore)
        {
            _mediator = mediator;
        }
        
        [HttpGet("shop/{shopId}")]
        public async Task<ApiResult> GetCategoriesOfShop(int shopId, [FromQuery] PaginationInfo paginationInfo)
        {
            var categories = await _mediator.Send(new FindCategoriesByShopIdQuery
            {
                ShopId = shopId,
                PaginationInfo = paginationInfo
            });
            return ApiResult<PaginatedList<CategoryDTO>>.CreateSucceedResult(categories);
        }
    }
}