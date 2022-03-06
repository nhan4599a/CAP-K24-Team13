using AspNetCoreSharedComponent.FileValidations;
using AspNetCoreSharedComponent.ModelBinders;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Exceptions;
using Shared.Models;
using Shared.RequestModels;
using Shared.Validations;
using ShopProductService.Commands.Category;
using System.Threading.Tasks;

namespace ShopProductService.Controllers
{
    [ApiController]
    [Route("/api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly IFileStorable _fileStore;

        private readonly FileValidationRuleSet _rules;
        
        public CategoryController(IMediator mediator, IFileStorable fileStore)
        {
            _mediator = mediator;
            if (fileStore != null)
            {
                _fileStore = fileStore;
                _fileStore.SetRelationalPath("categories");
            }
            _rules = FileValidationRuleSet.DefaultSingleValidationRules;
            _rules.Change(FileValidationRuleName.SingleMaxFileSize, (long)(0.3 * 1024 * 1024));
        }

        [Authorize]
        [HttpPost("shop/{shopId}")]
        public async Task<ApiResult> AddCategory(int shopId, 
            [FromForm(Name = "requestModel")] CreateOrEditCategoryRequestModel requestModel)
        {
            try
            {
                requestModel.ImagePath = await _fileStore.SaveFileAsync(Request.Form.Files[0], rules: _rules);
            }
            catch (ImageValidationException ex)
            {
                return ApiResult.CreateErrorResult(400, ex.Message);
            }
            var response = await _mediator.Send(new AddCategoryCommand
            {
                ShopId = shopId,
                RequestModel = requestModel
            });
            if (!response.IsSuccess)
                return ApiResult.CreateErrorResult(500, response.ErrorMessage);
            return ApiResult<bool>.CreateSucceedResult(true);
        }

        [HttpGet("{categoryId}")]
        public async Task<ApiResult> GetCategory(int categoryId)
        {
            var category = await _mediator.Send(new FindCategoryByIdQuery
            {
                Id = categoryId
            });
            if (category == null)
                return ApiResult.CreateErrorResult(404, "Category is not found");
            return ApiResult<CategoryDTO>.CreateSucceedResult(category);
        }

        [Authorize]
        [HttpPut("{categoryId}")]
        public async Task<ApiResult> EditCategory(int categoryId,
            [FromForm(Name = "requestModel")] CreateOrEditCategoryRequestModel requestModel)
        {
            try
            {
                requestModel.ImagePath = await _fileStore.EditFileAsync(requestModel.ImagePath,
                    Request.Form.Files[0], rules: _rules);
            }
            catch (ImageValidationException ex)
            {
                return ApiResult.CreateErrorResult(400, ex.Message);
            }
            var response = await _mediator.Send(new EditCategoryCommand
            {
                Id = categoryId,
                RequestModel = requestModel
            });
            if (!response.IsSuccess)
                return ApiResult.CreateErrorResult(500, response.ErrorMessage);
            return ApiResult<bool>.CreateSucceedResult(true);
        }

        [HttpGet]
        public async Task<ApiResult> GetAllCategories([FromQuery] PaginationInfo paginationInfo)
        {
            var categories = await _mediator.Send(new FindAllCategoriesQuery
            {
                PaginationInfo = paginationInfo
            });
            return ApiResult<PaginatedList<CategoryDTO>>.CreateSucceedResult(categories);
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

        [Authorize]
        [HttpDelete("{categoryId}")]
        public async Task<ApiResult> DeleteCategory(int categoryId,
            [FromQuery] DeleteAction action,
            [FromQuery(Name = "cascade")][ModelBinder(BinderType = typeof(IntToBoolModelBinder))] bool shouldBeCascade)
        {
            var response = await _mediator.Send(new ActivateCategoryCommand
            {
                Id = categoryId,
                IsActivateCommand = action == DeleteAction.Activate,
                ShouldBeCascade = action == DeleteAction.Deactivate || shouldBeCascade
            });
            if (!response.IsSuccess)
                return ApiResult.CreateErrorResult(500, response.ErrorMessage);
            return ApiResult<bool>.CreateSucceedResult(true);
        }

        [HttpGet("shop/{shopId}")]
        public async Task<ApiResult> GetCategoriesOfShop(int shopId)
        {
            var result = await _mediator.Send(new FindCategoriesByShopIdQuery
            {
                ShopId = shopId
            });
            return ApiResult<PaginatedList<CategoryDTO>>.CreateSucceedResult(result);
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("images/{image}")]
        public IActionResult GetImage(string image)
        {
            var fileResponse = _fileStore.GetFile(image);
            if (!fileResponse.IsExisted)
                return NotFound();
            return PhysicalFile(fileResponse.FullPath, fileResponse.MimeType);
        }
    }
}