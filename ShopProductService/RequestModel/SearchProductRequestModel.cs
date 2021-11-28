using Shared;

namespace ShopProductService.RequestModel
{
    public class SearchProductRequestModel
    {
        public string Keyword { get; set; } = string.Empty;

        public PaginationInfo PaginationInfo { get; set; }
    }
}
