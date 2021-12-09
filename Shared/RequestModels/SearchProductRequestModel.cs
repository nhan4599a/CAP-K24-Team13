namespace Shared.RequestModels
{
    public class SearchProductRequestModel
    {
        public string Keyword { get; set; } = string.Empty;

        public PaginationInfo PaginationInfo { get; set; }
    }
}
