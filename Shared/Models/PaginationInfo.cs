namespace Shared.Models
{
    public class PaginationInfo
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 5;


        public static readonly PaginationInfo Default = new();
    }
}
 