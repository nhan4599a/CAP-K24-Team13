﻿namespace DatabaseAccessor.Model
{
    public class ShopInterface
    {
        public int Id { get; set; }

        public int ShopId { get; set; }

        public int Option { get; set; }

        public string Description { get; set; }

        public virtual ShopImage ShopImage { get; set; }
    }
}
