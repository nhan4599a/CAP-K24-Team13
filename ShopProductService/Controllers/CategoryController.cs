using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.DTOs;
using Shared.RequestModels;
using ShopProductService.Commands.Category;
using System.Threading.Tasks;

namespace ShopProductService.Controllers
{
    [ApiController]
    [Route("/api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ActionName("Add")]
        public async Task<ApiResult<bool>> AddCategory(AddOrEditCategoryRequestModel requestModel)
        {
            var response = await _mediator.Send(new AddCategoryCommand
            {
                RequestModel = requestModel
            });
            if (!response.Response)
                return new ApiResult<bool> { ResponseCode = 500, ErrorMessage = response.ErrorMessage, Data = false };
            return new ApiResult<bool> { ResponseCode = 200, Data = true };
        }

        [HttpGet("{id}")]
        public async Task<ApiResult<CategoryDTO>> GetCategory(int id)
        {
            var category = await _mediator.Send(new FindCategoryByIdQuery
            {
                Id = id
            });
            if (category == null)
                return new ApiResult<CategoryDTO> { ResponseCode = 404, ErrorMessage = "Category is not found" };
            return new ApiResult<CategoryDTO> { ResponseCode = 200, Data = category };
        }

        [HttpPut("{id}")]
        public async Task<ApiResult<bool>> EditCategory(int id, AddOrEditCategoryRequestModel requestModel)
        {
            var response = await _mediator.Send(new EditCategoryCommand
            {
                Id = id,
                RequestModel = requestModel
            });
            if (!response.Response)
                return new ApiResult<bool> { ResponseCode = 500, ErrorMessage = response.ErrorMessage, Data = false };
            return new ApiResult<bool> { ResponseCode = 200, Data = true };
        }

        [HttpGet]
        [ActionName("Index")]
        public async Task<ApiResult<PaginatedDataList<CategoryDTO>>> ListCategory([FromQuery] PaginationInfo paginationInfo)
        {
            var categories = await _mediator.Send(new FindAllCategoryQuery());
            return new ApiResult<PaginatedDataList<CategoryDTO>>
            {
                ResponseCode = 200,
                Data = categories.Paginate(paginationInfo.PageNumber, paginationInfo.PageSize)
            };
        }

        [HttpDelete("{id}")]
        [ActionName("Delete")]
        public async Task<ApiResult<bool>> DeleteCategory(int id)
        {
            var response = await _mediator.Send(new DeleteCategoryCommand
            {
                Id = id
            });
            if (!response.Response)
                return new ApiResult<bool> { ResponseCode = 404, ErrorMessage = response.ErrorMessage, Data = false };
            return new ApiResult<bool> { ResponseCode = 200, Data = true };
        }
    }
}
