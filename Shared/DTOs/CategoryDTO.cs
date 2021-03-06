namespace Shared.DTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }

        public int ShopId { get; set; }

        public string CategoryName { get; set; }

        public int Special { get; set; }

        public bool IsDisabled { get; set; }

        public string Image { get; set; }
    }
}
