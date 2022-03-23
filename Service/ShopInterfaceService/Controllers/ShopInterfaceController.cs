using AspNetCoreSharedComponent.FileValidations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Exceptions;
using Shared.Models;
using Shared.RequestModels;
using Shared.Validations;
using ShopInterfaceService.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopInterfaceService.Controllers
{
    [Authorize]
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
        public async Task<ApiResult> CreateShopInterface(int shopId, 
            [FromForm(Name = "requestModel")] CreateOrEditInterfaceRequestModel requestModel)
        {
            try
            {
                requestModel.ShopImages = await _fileStore.SaveFilesAsync(Request.Form.Files, rules: rules);
            }
            catch (ImageValidationException ex)
            {
                return ApiResult.CreateErrorResult(400, ex.Message);
            }
            var result = await _mediator.Send(new CreateOrEditShopInterfaceCommand
            {
                ShopId = shopId,
                RequestModel = requestModel
            });
            if (!result.IsSuccess)
                return ApiResult.CreateErrorResult(500, result.ErrorMessage);
            return ApiResult.SucceedResult;
        }

        [HttpPut("{shopId}")]
        public async Task<ApiResult> EditShopInterface(int shopId,
            [FromForm(Name = "requestModel")] CreateOrEditInterfaceRequestModel requestModel)
        {
            try
            {
                requestModel.ShopImages = await _fileStore.EditFilesAsync(requestModel.ShopImages, Request.Form.Files,
                    rules: rules);
            }
            catch (ImageValidationException ex)
            {
                return ApiResult.CreateErrorResult(400, ex.Message);
            }
            var result = await _mediator.Send(new CreateOrEditShopInterfaceCommand
            {
                ShopId = shopId,
                RequestModel = requestModel
            });
            if (!result.IsSuccess)
                return ApiResult.CreateErrorResult(500, result.ErrorMessage);
            return ApiResult.SucceedResult;
        }

        [HttpGet("{shopId}")]
        public async Task<ApiResult> GetShopInterface(int shopId)
        {
            var command = new FindShopInterfaceByShopIdQuery
            {
                ShopId = shopId
            };

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return ApiResult.CreateErrorResult(500, result.ErrorMessage);
            
            return ApiResult<ShopInterfaceDTO>.CreateSucceedResult(result.Response);
        }

        [HttpGet("avatar/{shopId*}")]
        public async Task<ApiResult> GetAvatar(params int[] shopId)
        {
            var result = await _mediator.Send(new GetShopAvatarQuery
            {
                ShopId = shopId
            });
            return ApiResult<Dictionary<int, string>>.CreateSucceedResult(result);
        }

        [AllowAnonymous]
        [HttpGet("images/{imageId}")]
        public IActionResult Image(string imageId)
        {
            var fileResponse = _fileStore.GetFile(imageId);
            if (!fileResponse.IsExisted)
                return StatusCode(StatusCodes.Status404NotFound);
            return PhysicalFile(fileResponse.FullPath, fileResponse.MimeType);
        }
    }
}
