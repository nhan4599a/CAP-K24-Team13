using AspNetCoreSharedComponent.FileValidations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.DTOs;
using Shared.Models;
using Shared.RequestModels;
using Shared.Validations;
using ShopInterfaceService.Commands;
using ShopInterfaceService.Mediator;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ShopInterfaceService.Controllers
{
    [Route("api/interfaces")]
    [ApiController]
    public class ShopInterfaceController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IFileStorable _fileStore;
        private readonly FileValidationRuleSet rules;

        public ShopInterfaceController(IMediator mediator, IFileStorable imageManager)
        {
            _mediator = mediator;
            _fileStore = imageManager;
            rules = FileValidationRuleSet.DefaultValidationRules;
            rules.Change(FileValidationRuleName.MinFileCount, 1);
        }

        [HttpPost("{shopId}")]
        public async Task<ApiResult<bool>> CreateShopInterface(int shopId, 
            [FromForm(Name = "requestModel")] CreateOrEditInterfaceRequestModel requestModel)
        {
            requestModel.ShopImages = await _fileStore.SaveFilesAsync(Request.Form.Files, rules: rules);
            var result = await _mediator.Send(new CreateOrEditShopInterfaceCommand
            {
                ShopId = shopId,
                RequestModel = requestModel
            });
            if (!result.IsSuccess)
                return new ApiResult<bool> { ResponseCode = 500, Data = false, ErrorMessage = result.ErrorMessage };
            return new ApiResult<bool> { ResponseCode = 200, Data = true };
        }

        [HttpPut("{shopId}")]
        public async Task<ApiResult<bool>> EditShopInterface(int shopId,
            [FromForm(Name = "requestModel")] CreateOrEditInterfaceRequestModel requestModel)
        {
            requestModel.ShopImages = await _fileStore.EditFilesAsync(requestModel.ShopImages, Request.Form.Files,
                rules: rules);
            var result = await _mediator.Send(new CreateOrEditShopInterfaceCommand
            {
                ShopId = shopId,
                RequestModel = requestModel
            });
            if (!result.IsSuccess)
                return new ApiResult<bool> { ResponseCode = 500, Data = false, ErrorMessage = result.ErrorMessage };
            return new ApiResult<bool> { ResponseCode = 200, Data = true };
        }

        [HttpGet("{shopId}")]
        public async Task<ApiResult<ShopInterfaceDTO>> GetShopInterface(int shopId)
        {
            var command = new FindShopInterfaceByShopIdCommand
            {
                ShopId = shopId
            };

            var result = await _mediator.Send(command);

            return new ApiResult<ShopInterfaceDTO> { ResponseCode = 200, Data = result.Response };
        }

        [HttpGet("images/{imageId}")]
        public IActionResult Index(string imageId)
        {
            var fileResponse = _fileStore.GetFile(imageId);
            if (!fileResponse.IsExisted)
                return StatusCode(404);
            return PhysicalFile(fileResponse.FullPath, fileResponse.MimeType);
        }
    }
}
