using Shared.RequestModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseAccessor.Models
{
    [Table("ShopInterfaces", Schema = "dbo")]
    public class ShopInterface
    {
        public int Id { get; set; }

        public int ShopId { get; set; }

        public string ShopAddress { get; set; }

        public string ShopEmail { get; set; }

        public string ShopName { get; set; }

        public string ShopPhoneNumber { get; set; }

        public string ShopDescription { get; set; }

        public string Images { get; set; }

        public ShopInterface AssignByRequestModel(CreateOrEditInterfaceRequestModel requestModel)
        {
            ShopAddress = requestModel.ShopAddress;
            ShopPhoneNumber = requestModel.ShopPhoneNumber;
            ShopDescription = requestModel.ShopDescription;
            ShopName = requestModel.Name;
            ShopEmail = requestModel.ShopEmail;
            if (requestModel.ShopImages != null)
                Images = string.Join(";", requestModel.ShopImages);
            return this;
        }
    }
}
