﻿namespace DatabaseAccessor.Models
{
    public class ShopInterface
    {
        public int Id { get; set; }

        public int ShopId { get; set; }

        public string ShopAddress { get; set; }
        public string ShopName { get; set; }

        public string ShopPhoneNumber { get; set; }

        public string ShopDescription { get; set; }

        public string Images { get; set; }
    }
}
