using GUI.Models;
using Refit;
using Shared.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GUI.Clients
{
    public class ShopClientImpl : IShopClient
    {
        private readonly IExternalShopClient _externalShopClient;

        private readonly IShopInterfaceClient _interfaceClient;

        public ShopClientImpl(IExternalShopClient externalShopClient, IShopInterfaceClient interfaceClient)
        {
            _externalShopClient = externalShopClient;
            _interfaceClient = interfaceClient;
        }

        public async Task<ApiResponse<ExternalApiPaginatedList<ShopDTO>>> FindShops(string keyword, int pageNumber, int pageSize)
        {
            var externalShopResponse = await _externalShopClient.FindShops(keyword, pageNumber, pageSize);
            if (externalShopResponse.IsSuccessStatusCode)
            {
                var avatars = await GetAvatar(externalShopResponse.Content.Items.Select(item => item.Id).ToArray());
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
            if (externalShopResponse.IsSuccessStatusCode)
            {
                var avatars = await GetAvatar(externalShopResponse.Content.Select(item => item.Id).ToArray());
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
            var externalShopResponse = await _externalShopClient.GetShop(shopId);
            if (externalShopResponse.IsSuccessStatusCode)
            {
                var avatar = await GetAvatar(new int[] { shopId });
                if (avatar != null)
                {
                    externalShopResponse.Content.ResultObj.Avatar = avatar[externalShopResponse.Content.ResultObj.Id];
                }
            }
            return externalShopResponse;
        }

        public async Task<ApiResponse<List<ShopDTO>>> FindShops(string keyword)
        {
            var externalShopResponse = await _externalShopClient.FindShops(keyword);

            if (externalShopResponse.IsSuccessStatusCode)
            {
                var avatars = await GetAvatar(externalShopResponse.Content.Select(item => item.Id).ToArray());
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

        private async Task<Dictionary<int, string>> GetAvatar(int[] shopId)
        {
            var result = await _interfaceClient.GetShopAvatar(shopId);
            if (result.IsSuccessStatusCode)
                return result.Content.Data;
            return null;
        }
    }
}
