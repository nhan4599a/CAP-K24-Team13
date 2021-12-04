using DatabaseAccessor.Model;
using Shared.Mapping;

namespace Shared.DTOs
{
    public class CategoryDTO : DTO<ShopCategory>
    {
        public int Id { get; set; }

        public int ShopId { get; set; }

        public string CategoryName { get; set; }

        public int Special { get; set; }

        public bool IsDisabled { get; set; }

        public object MapFromSource(ShopCategory category)
        {
            return Mapper.GetInstance().MapToCategoryDTO(category);
        }

        public static CategoryDTO FromSource(ShopCategory category)
        {
            return (CategoryDTO) new CategoryDTO().MapFromSource(category);
        }
    }
}
