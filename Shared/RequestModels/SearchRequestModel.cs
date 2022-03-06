namespace Shared.RequestModels
{
    public class SearchRequestModel
    {
        public string Keyword { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}
