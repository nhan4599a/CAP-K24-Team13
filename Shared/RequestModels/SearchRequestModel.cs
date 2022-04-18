using Shared.Models;

namespace Shared.RequestModels
{
    public class SearchRequestModel
    {
        public string Keyword { get; set; }

        public int PageNumber { get; set; } = PaginationInfo.Default.PageNumber;

        public int PageSize { get; set; } = PaginationInfo.Default.PageSize;

        public bool IncludeFilter { get; set; } = true;
    }
}
