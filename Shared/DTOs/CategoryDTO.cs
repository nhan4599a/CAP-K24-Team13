using DatabaseAccessor.Model;
using Shared.Mapping;

namespace Shared.DTOs
{
    public class CategoryDTO : IDTO<ShopCategory>
    {
        public CategoryDTO(ShopCategory category)
        {
            MapFromSource(category);
        }

        public int Id { get; set; }

        public int ShopId { get; set; }

        public string CategoryName { get; set; }

        public int Special { get; set; }

        public bool IsDisabled { get; set; }

        public object MapFromSource(ShopCategory category)
        {
            return Mapper.GetInstance().MapToCategoryDTO(category);
        }
    }
}
