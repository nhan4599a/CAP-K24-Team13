namespace Shared.RequestModels
{
    public class CreateOrEditInterfaceRequestModel
    {
        public string Name { get; set; }

        public string ShopAddress { get; set; }

        public string ShopEmail { get; set; }

        public string ShopPhoneNumber { get; set; }

        public string ShopDescription { get; set; }

        public string[] ShopImages { get; set; }
    }
}
