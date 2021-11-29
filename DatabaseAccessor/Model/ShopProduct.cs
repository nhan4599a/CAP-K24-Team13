using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccessor.Model
{
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

        public virtual ShopCategory Category { get; set; }
    }
}
