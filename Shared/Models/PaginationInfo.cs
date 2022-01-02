namespace Shared.Models
{
    public class PaginationInfo
    {
        public int? PageNumber { get; set; }

        public int PageSize { get; set; } = 5;

        public static PaginationInfo Default => new()
        {
            PageNumber = 1,
            PageSize = 5
        };
    }
}
