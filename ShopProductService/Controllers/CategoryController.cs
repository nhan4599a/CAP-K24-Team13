using AspNetCoreSharedComponent;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.DTOs;
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
            _fileStore = fileStore;
            _fileStore.SetRelationalPath("categories");
            _rules = FileValidationRuleSet.DefaultValidationRules;
            _rules.Change(FileValidationRuleName.MinFileCount, 1);
            _rules.Change(FileValidationRuleName.MaxFileCount, 1);
        }

        [HttpPost]
        [ActionName("Add")]
        public async Task<ApiResult<bool>> AddCategory([FromForm(Name = "requestModel")] CreateOrEditCategoryRequestModel requestModel)
        {
            requestModel.ImagePath = await _fileStore.SaveFileAsync(Request.Form.Files[0], rules: _rules);
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
        public async Task<ApiResult<bool>> EditCategory(int id, [FromForm(Name = "requestModel")] CreateOrEditCategoryRequestModel requestModel)
        {
            requestModel.ImagePath = await _fileStore.EditFileAsync(requestModel.ImagePath, Request.Form.Files[0], rules: _rules);
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
        public async Task<ApiResult<PaginatedList<CategoryDTO>>> ListCategory([FromQuery] PaginationInfo paginationInfo)
        {
            var categories = await _mediator.Send(new FindAllCategoryQuery
            {
                PaginationInfo = paginationInfo
            });
            return new ApiResult<PaginatedList<CategoryDTO>>
            {
                ResponseCode = 200,
                Data = categories
            };
        }

        [HttpDelete("{id}")]
        [ActionName("Delete")]
        public async Task<ApiResult<bool>> DeleteCategory(int id,
            [FromQuery] DeleteAction action, 
            [FromQuery(Name = "cascade")][ModelBinder(BinderType = typeof(IntToBoolModelBinder))] bool shouldBeCascade)
        {
            var response = await _mediator.Send(new ActivateCategoryCommand
            {
                Id = id,
                IsActivateCommand = action == DeleteAction.Activate,
                ShouldBeCascade = action == DeleteAction.Deactivate || shouldBeCascade
            });
            if (!response.Response)
                return new ApiResult<bool> { ResponseCode = 404, ErrorMessage = response.ErrorMessage, Data = false };
            return new ApiResult<bool> { ResponseCode = 200, Data = true };
        }

        [HttpGet("images/{image}")]
        public IActionResult GetImage(string image)
        {
            var fileResponse = _fileStore.GetFile(image);
            if (!fileResponse.IsExisted)
                return StatusCode(404);
            return PhysicalFile(fileResponse.FullPath, fileResponse.MimeType);
        }
    }
}