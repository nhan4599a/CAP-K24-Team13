using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DatabaseAccessor.Model
{
    public class ShopCategory
    {

        public int Id { get; set; }
        [Required]
        public int? ShopId { get; set; } = null;

        public string CategoryName { get; set; }

        public int Special { get; set; }

        public virtual List<ShopProduct> ShopProducts { get; set; }
    }
}
