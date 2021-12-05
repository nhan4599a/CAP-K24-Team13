using DatabaseAccessor.Model;
using Shared.Mapping;

namespace Shared.DTOs
{
    public class ProductDTO : IDTO<ShopProduct>
    {
        public ProductDTO(ShopProduct product)
        {
            MapFromSource(product);
        }

        public string Id { get; set; }

        public string ProductName { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }

        public int Discount { get; set; }

        public string CategoryName { get; set; }

        public string[] Images { get; set; }

        public bool IsDisabled { get; set; }

        public object MapFromSource(ShopProduct product)
        {
            return Mapper.GetInstance().MapToProductDTO(product);
        }
    }
}