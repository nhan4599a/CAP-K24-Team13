namespace Shared.RequestModels
{
    public class CreateOrEditShopInterfaceRequestModel
    {
        public string ShopAddress { get; set; }

        public string ShopPhoneNumber { get; set; }

        public string ShopDescription { get; set; }

        public string[] ShopImages { get; set; }
    }
}
