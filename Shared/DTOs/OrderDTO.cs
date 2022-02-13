using System;

namespace Shared.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }

        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }   

        public DateTime Created { get; set; }

        public int Price { get; set; }   

        public string Images { get; set; }
    }
}
