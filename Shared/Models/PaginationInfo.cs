namespace Shared.Models
{
    public class PaginationInfo
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; }

        public static PaginationInfo Default => new()
        {
            PageSize = 5
        };
    }
}
