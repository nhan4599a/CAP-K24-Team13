using GUI.Models;
using Refit;
using Shared.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GUI.Clients
{
    public class ShopClientImpl : IShopClient
    {
        private readonly IExternalShopClient _externalShopClient;

        private readonly IShopInterfaceClient _interfaceClient;

        private readonly ShopDTO _virtualShop = new()
        {
            Id = 0,
            ShopName = "Test shop",
            Description = "This shop in for testing purpose"
        };

        public ShopClientImpl(IExternalShopClient externalShopClient, IShopInterfaceClient interfaceClient)
        {
            _externalShopClient = externalShopClient;
            _interfaceClient = interfaceClient;
        }

        public async Task<ApiResponse<ExternalApiPaginatedList<ShopDTO>>> FindShops(string keyword, int pageNumber, int pageSize)
        {
            var externalShopResponse = await _externalShopClient.FindShops(keyword, pageNumber, pageSize);
            if (_virtualShop.ShopName.Contains(keyword, System.StringComparison.CurrentCultureIgnoreCase))
            {
                externalShopResponse = IntegrateVirtualShop(externalShopResponse, pageNumber, pageSize);
            }
            if (externalShopResponse.IsSuccessStatusCode)
            {
                var avatars = await GetAvatar(externalShopResponse.Content.Items.Select(item => item.Id).ToList());
                if (avatars != null)
                {
                    for (int i = 0; i < externalShopResponse.Content.Items.Count; i++)
                    {
                        var shopId = externalShopResponse.Content.Items[i].Id;
                        externalShopResponse.Content.Items[i].Avatar = avatars[shopId];
                    }
                }
            }
            return externalShopResponse;
        }

        public async Task<ApiResponse<List<ShopDTO>>> GetAllShops()
        {
            var externalShopResponse = await _externalShopClient.GetAllShops();
            externalShopResponse = IntegrateVirtualShop(externalShopResponse);
            if (externalShopResponse.IsSuccessStatusCode)
            {
                var avatars = await GetAvatar(externalShopResponse.Content.Select(item => item.Id).ToList());
                if (avatars != null)
                {
                    for (int i = 0; i < externalShopResponse.Content.Count; i++)
                    {
                        var shopId = externalShopResponse.Content[i].Id;
                        externalShopResponse.Content[i].Avatar = avatars[shopId];
                    }
                }
            }
            return externalShopResponse;
        }

        public async Task<ApiResponse<ExternalApiResult<ShopDTO>>> GetShop(int shopId)
        {
            if (shopId == 0)
            {

                var avatar = await GetAvatar(new List<int> { 0 });
                var virtualShopCopy = new ShopDTO
                {
                    Id = _virtualShop.Id,
                    ShopName = _virtualShop.ShopName,
                    Description = _virtualShop.Description,
                };
                if (avatar != null)
                {
                    virtualShopCopy.Avatar = avatar.GetValueOrDefault(0);
                }
                var apiResult = new ExternalApiResult<ShopDTO>
                {
                    IsSuccessed = true,
                    Message = string.Empty,
                    ResultObj = virtualShopCopy
                };
                HttpResponseMessage message = new(System.Net.HttpStatusCode.OK)
                {
                    Content = JsonContent.Create(apiResult)
                };
                return new ApiResponse<ExternalApiResult<ShopDTO>>(message, apiResult, null);
            }
            var externalShopResponse = await _externalShopClient.GetShop(shopId);
            if (externalShopResponse.IsSuccessStatusCode)
            {
                var avatar = await GetAvatar(new List<int> { shopId });
                if (avatar != null)
                {
                    externalShopResponse.Content.ResultObj.Avatar = avatar[externalShopResponse.Content.ResultObj.Id];
                }
            }
            return externalShopResponse;
        }

        private ApiResponse<ExternalApiPaginatedList<ShopDTO>> IntegrateVirtualShop(
            ApiResponse<ExternalApiPaginatedList<ShopDTO>> apiResponse, int pageNumber, int pageSize)
        {
            var newItems = new List<ShopDTO> { _virtualShop };
            if (apiResponse.IsSuccessStatusCode)
            {
                newItems.AddRange(apiResponse.Content.Items);
            }
            var paginatedList = new ExternalApiPaginatedList<ShopDTO>
            {
                Items = newItems,
                PageCount = (newItems.Count / pageSize) + 1,
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalRecords = newItems.Count
            };
            HttpResponseMessage message = new(System.Net.HttpStatusCode.OK)
            {
                Content = JsonContent.Create(paginatedList)
            };
            return new ApiResponse<ExternalApiPaginatedList<ShopDTO>>(message, paginatedList, null);
        }

        private ApiResponse<List<ShopDTO>> IntegrateVirtualShop(ApiResponse<List<ShopDTO>> apiResponse)
        {
            var newItems = new List<ShopDTO> { _virtualShop };
            if (apiResponse.IsSuccessStatusCode)
            {
                newItems.AddRange(apiResponse.Content);
            }
            HttpResponseMessage message = new(System.Net.HttpStatusCode.OK)
            {
                Content = JsonContent.Create(newItems)
            };
            return new ApiResponse<List<ShopDTO>>(message, newItems, null);
        }

        private async Task<Dictionary<int, string>> GetAvatar(List<int> shopId)
        {
            var result = await _interfaceClient.GetShopAvatar(shopId);
            if (result.IsSuccessStatusCode)
                return result.Content.Data;
            return null;
        }
    }
}
