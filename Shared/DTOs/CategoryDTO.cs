using DatabaseAccessor.Model;

namespace Shared.DTOs
{
    public class CategoryDTO : DTO<ShopCategory>
    {
        public int Id { get; set; }

        public string CategoryName { get; set; }

        public object MapFromSource(ShopCategory originalObject)
        {
            return Mapping.Mapper.GetInstance().MapToCategoryDTO(originalObject);
        }

        public static object FromSource(ShopCategory originalObject)
        {
            return new CategoryDTO().MapFromSource(originalObject);
        }
    }
}
