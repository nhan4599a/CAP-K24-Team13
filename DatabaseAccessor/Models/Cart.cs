using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseAccessor.Models
{
    [Table("Carts")]
    public class Cart
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public virtual User User { get; set; }

        public virtual IList<CartDetail> Details { get; set; }
    }
}
