using Shared.RequestModels;
using System.Collections.Generic;

namespace DatabaseAccessor.Models
{
    public class ShopCategory
    {
        public int Id { get; set; }
      
        public int ShopId { get; set; }

        public string CategoryName { get; set; }

        public int Special { get; set; }

        public bool IsDisabled { get; set; }

        public virtual List<ShopProduct> ShopProducts { get; set; }

        public ShopCategory AssignByRequestModel(CreateOrEditCategoryRequestModel requestModel)
        {
            CategoryName = requestModel.CategoryName;
            Special = requestModel.Special;
            return this;
        }
    }
}
