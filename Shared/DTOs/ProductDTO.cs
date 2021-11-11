using DatabaseAccessor.Model;
using Shared.Mapping;

namespace Shared.DTOs
{
    public class ProductDTO : DTO<ShopProduct>
    {

        public string ProductName { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public int Discount { get; set; }

        public string CategoryName { get; set; }

        public string[] Images { get; set; }

        public object MapFromSource(ShopProduct product)
        {
            return Mapper.GetInstance().MapToProductDTO(product);
        }
    }
}
