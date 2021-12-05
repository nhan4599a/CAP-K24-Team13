using DatabaseAccessor.Model;

namespace Shared.DTOs
{
    public class ShopInterfaceDTO : IDTO<ShopInterface>
    {
        public ShopInterfaceDTO(ShopInterface shopInterface)
        {
            MapFromSource(shopInterface);
        }

        public int Id { get; set; }

        public int ShopId { get; set; }

        public string ShopAddress { get; set; }

        public string ShopPhoneNumber { get; set; }

        public string ShopDescription { get; set; }

        public string[] Images { get; set; }

        public object MapFromSource(ShopInterface originalObject)
        {
            return Mapping.Mapper.GetInstance().MapToShopInterfaceDTO(originalObject);
        }
    }
}
