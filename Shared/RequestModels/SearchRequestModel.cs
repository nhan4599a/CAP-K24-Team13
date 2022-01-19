using Shared.Models;

namespace Shared.RequestModels
{
    public class SearchRequestModel
    {
        public string Keyword { get; set; } = string.Empty;

        public PaginationInfo PaginationInfo { get; set; } = PaginationInfo.Default;
    }
}
