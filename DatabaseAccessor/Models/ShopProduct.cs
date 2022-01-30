using Shared.RequestModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseAccessor.Models
{
    [Table("ShopProducts")]
    public class ShopProduct
    {
        public Guid Id { get; set; }

        public string ProductName { get; set; }

        public string Description { get; set; }

        public string Images { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }

        public int Discount { get; set; }

        public bool IsDisabled { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CategoryId { get; set; }

        public int ShopId { get; set; }

        public virtual ShopCategory Category { get; set; }

        public virtual IList<InvoiceDetail> Invoices { get; set; }

        public virtual IList<ProductComment> Comments { get; set; }

        public virtual IList<CartDetail> CartDetails { get; set; }

        public ShopProduct AssignByRequestModel(CreateOrEditProductRequestModel requestModel)
        {
            ProductName = requestModel.ProductName;
            CategoryId = requestModel.CategoryId;
            Description = requestModel.Description;
            Quantity = requestModel.Quantity;
            Price = requestModel.Price;
            Discount = requestModel.Discount;
            if (requestModel.ImagePaths != null)
                Images = string.Join(';', requestModel.ImagePaths);
            return this;
        }
    }
}
