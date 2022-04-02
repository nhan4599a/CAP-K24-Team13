﻿using Shared.RequestModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseAccessor.Models
{
    [Table("ShopInterfaces", Schema = "dbo")]
    public class ShopInterface
    {
        public int ShopId { get; set; }

        public string Avatar { get; set; }

        public string Images { get; set; }

        public ShopInterface AssignByRequestModel(CreateOrEditInterfaceRequestModel requestModel)
        {
            Avatar = requestModel.Avatar;
            if (requestModel.ShopImages != null)
                Images = string.Join(";", requestModel.ShopImages);
            return this;
        }
    }
}
