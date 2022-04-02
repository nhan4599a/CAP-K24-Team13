using Shared.RequestModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseAccessor.Models
{
    [Table("ShopCategories", Schema = "dbo")]
    public class ShopCategory
    {
        public int Id { get; set; }
      
        public int ShopId { get; set; }

        public string CategoryName { get; set; }

        public int Special { get; set; }

        public bool IsDisabled { get; set; }

        public string Image { get; set; }

        public virtual IList<ShopProduct> ShopProducts { get; set; }

        public ShopCategory AssignByRequestModel(CreateOrEditCategoryRequestModel requestModel)
        {
            CategoryName = requestModel.CategoryName;
            Special = requestModel.Special;
            Image = requestModel.ImagePath;
            return this;
        }
    }
}
