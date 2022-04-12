using GUI.Models;
using Refit;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GUI.Clients
{
    public class ShopClientImpl : IShopClient
    {
        private readonly IExternalShopClient _externalShopClient;

        private readonly IShopInterfaceClient _interfaceClient;

        private readonly IUserClient _userClient;

        public ShopClientImpl(IExternalShopClient externalShopClient, IShopInterfaceClient interfaceClient, IUserClient userClient)
        {
            _externalShopClient = externalShopClient;
            _interfaceClient = interfaceClient;
            _userClient = userClient;
        }

        public async Task<ApiResponse<ExternalApiPaginatedList<ShopDTO>>> FindShops(string keyword, int pageNumber, int pageSize)
        {
            var externalShopResponse = await _externalShopClient.FindShops(keyword, pageNumber, pageSize);
            if (externalShopResponse.IsSuccessStatusCode)
            {
                var avatars = await GetShopInformation(externalShopResponse.Content.Items.Select(item => item.Id).ToArray());
                if (avatars != null)
                {
                    for (int i = 0; i < externalShopResponse.Content.Items.Count; i++)
                    {
                        var shopId = externalShopResponse.Content.Items[i].Id;
                        externalShopResponse.Content.Items[i].Avatar = avatars.ContainsKey(shopId) ? avatars[shopId].Avatar : string.Empty;
                        externalShopResponse.Content.Items[i].Images = avatars.ContainsKey(shopId) ? avatars[shopId].Images : Array.Empty<string>();
                    }
                }
            }
            return externalShopResponse;
        }

        public async Task<ApiResponse<List<ShopDTO>>> GetAllShops()
        {
            var externalShopResponse = await _externalShopClient.GetAllShops();
            if (externalShopResponse.IsSuccessStatusCode)
            {
                var avatars = await GetShopInformation(externalShopResponse.Content.Select(item => item.Id).ToArray());
                if (avatars != null)
                {
                    for (int i = 0; i < externalShopResponse.Content.Count; i++)
                    {
                        var shopId = externalShopResponse.Content[i].Id;
                        externalShopResponse.Content[i].Avatar = avatars.ContainsKey(shopId) ? avatars[shopId].Avatar : string.Empty;
                        externalShopResponse.Content[i].Images = avatars.ContainsKey(shopId) ? avatars[shopId].Images : Array.Empty<string>();
                    }
                }
            }
            return externalShopResponse;
        }

        public async Task<ApiResponse<ExternalApiResult<ShopDTO>>> GetShop(int shopId)
        {
            var externalShopResponse = await _externalShopClient.GetShop(shopId);
            if (externalShopResponse.IsSuccessStatusCode)
            {
                var avatar = await GetShopInformation(new int[] { shopId });
                if (avatar != null)
                {
                    externalShopResponse.Content.ResultObj.Avatar = avatar.ContainsKey(shopId) ? avatar[shopId].Avatar : string.Empty;
                    externalShopResponse.Content.ResultObj.Images = avatar.ContainsKey(shopId) ? avatar[shopId].Images : Array.Empty<string>();
                }
                var userInfo = await GetUserInfo(externalShopResponse.Content.ResultObj.UserId);
                if (userInfo != null)
                {
                    externalShopResponse.Content.ResultObj.Name = userInfo.FullName;
                    externalShopResponse.Content.ResultObj.Phone = userInfo.PhoneNumber;
                    externalShopResponse.Content.ResultObj.Email = userInfo.Email;
                }
            }
            return externalShopResponse;
        }

        public async Task<ApiResponse<List<ShopDTO>>> FindShops(string keyword)
        {
            var externalShopResponse = await _externalShopClient.FindShops(keyword);

            if (externalShopResponse.IsSuccessStatusCode)
            {
                var avatars = await GetShopInformation(externalShopResponse.Content.Select(item => item.Id).ToArray());
                if (avatars != null)
                {
                    for (int i = 0; i < externalShopResponse.Content.Count; i++)
                    {
                        var shopId = externalShopResponse.Content[i].Id;
                        externalShopResponse.Content[i].Avatar = avatars.ContainsKey(shopId) ? avatars[shopId].Avatar : string.Empty;
                        externalShopResponse.Content[i].Images = avatars.ContainsKey(shopId) ? avatars[shopId].Images : Array.Empty<string>();
                    }
                }
            }
            return externalShopResponse;
        }

        private async Task<Dictionary<int, ShopInterfaceDTO>> GetShopInformation(int[] shopId)
        {
            var result = await _interfaceClient.GetShopInterface(shopId);
            if (result.IsSuccessStatusCode)
                return result.Content.Data;
            return null;
        }

        private async Task<UserDTO> GetUserInfo(string userId)
        {
            var result = await _userClient.GetUserInfo(userId);
            if (result.IsSuccessStatusCode)
                return result.Content.Data;
            return null;
        }
    }
}
