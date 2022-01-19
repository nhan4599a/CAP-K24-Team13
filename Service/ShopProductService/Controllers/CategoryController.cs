using AspNetCoreSharedComponent.FileValidations;
using AspNetCoreSharedComponent.ModelBinders;
using DatabaseAccessor;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Models;
using Shared.RequestModels;
using Shared.Validations;
using ShopProductService.Commands.Category;
using System.Collections.Generic;
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

        private PaginatedList<CategoryDTO> FakeCategories = new List<CategoryDTO>
        {
            new CategoryDTO
            {
                Id = -5,
                CategoryName = "Fake category 1",
                IsDisabled = false,
                Image = ""
            },
            new CategoryDTO
            {
                Id = -4,
                CategoryName = "Fake category 2",
                IsDisabled = false,
                Image = ""
            },
            new CategoryDTO
            {
                Id = -3,
                CategoryName = "Fake category 3",
                IsDisabled = false,
                Image = ""
            },
            new CategoryDTO
            {
                Id = -2,
                CategoryName = "Fake category 4",
                IsDisabled = false,
                Image = ""
            },
            new CategoryDTO
            {
                Id = -1,
                CategoryName = "Fake category 5",
                IsDisabled = false,
                Image = ""
            }
        }.Paginate(1, 5);
        
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
        public async Task<ApiResult<bool>> EditCategory(int id,
            [FromForm(Name = "requestModel")] CreateOrEditCategoryRequestModel requestModel)
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

        public async Task<ApiResult<PaginatedList<CategoryDTO>>> GetCategoriesOfShop(int id)
        {
            if (id != 0)
                return new ApiResult<PaginatedList<CategoryDTO>>
                {
                    ResponseCode = 200,
                    Data = FakeCategories
                };
            
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
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