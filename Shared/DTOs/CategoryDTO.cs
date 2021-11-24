using DatabaseAccessor.Model;
using Shared.Mapping;
using System;

namespace Shared.DTOs
{
    public class CategoryDTO : DTO<ShopCategory>
    {
        public int Id { get; set; }

        public int ShopId { get; set; }

        public string CategoryName { get; set; }

        public int Special { get; set; }


        public object MapFromSource(ShopCategory category)
        {
            return Mapper.GetInstance().MapToCategoryDTO(category);
        }

        public static object FromSource(ShopCategory category)
        {
            return new CategoryDTO().MapFromSource(category);
        }
    }
}
