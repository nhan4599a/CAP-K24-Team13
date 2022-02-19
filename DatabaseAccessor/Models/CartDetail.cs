using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseAccessor.Models
{
    [Table("CartDetails", Schema = "CartDetail")]
    public class CartDetail
    {
        public int Id { get; set; }

        public int CartId { get; set; }

        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public int ShopId { get; set; }

        public virtual Cart Cart { get; set; }

        public virtual ShopProduct Product { get; set; }
    }
}
