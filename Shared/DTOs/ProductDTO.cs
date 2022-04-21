namespace Shared.DTOs
{
    public class ProductDTO : MinimalProductDTO
    {
        public string Description { get; set; }

        public string CategoryName { get; set; }

        public double AverageRating { get; set; }
    }
}