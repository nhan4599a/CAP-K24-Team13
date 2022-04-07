namespace Shared.DTOs
{
    public class ProductDTO : MinimalProductDTO
    {
        public string Description { get; set; }

        public string CategoryName { get; set; }

        public string[] Images { get; set; }

        public bool IsDisabled { get; set; }
    }
}