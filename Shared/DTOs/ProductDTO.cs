﻿namespace Shared.DTOs
{
    public class ProductDTO
    {
        public string Id { get; set; }

        public string ProductName { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }

        public int Discount { get; set; }

        public string CategoryName { get; set; }

        public string[] Images { get; set; }

        public bool IsDisabled { get; set; }
    }
}